// See https://aka.ms/new-console-template for more information
using CountrySeeder;
using CountrySeeder.Entities;
using CountrySeeder.Models;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Milvasoft.Core.Helpers.GeoLocation.Models;
using System.Text.Json;

try
{
    string connectionString = GetConnectionString(args);
    bool forceSeed = GetForceOption(args);

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        Console.WriteLine("❌ Missing connection string. Please send as argument or 'Geo_Connection_String' env variable");
        return;
    }

    var options = new DbContextOptionsBuilder<AppDbContext>().UseNpgsql(connectionString).Options;

    using var context = new AppDbContext(options);

    Console.WriteLine("Checking table existence...");

    if (!await TablesExistAsync(context))
    {
        Console.WriteLine("Cannot find 'Countries', 'States' or 'Cities' tables. Please add database and create this tables.");
        return;
    }

    if (!forceSeed && await AreTablesSeededAsync(context))
    {
        Console.WriteLine("Tables already seeded! You can force to seed with '--force' argument or 'FORCE_SEED' env variable. ");
        return;
    }

    Console.WriteLine("Cleaning tables...");

    await TruncateTablesAsync(context);

    Console.WriteLine("Seeding starting...");

    Console.WriteLine("Reading json data file...\n");
    var spinner = new ConsoleSpinner("Seeding...\n");
    await ImportJsonDataAsync(Path.Combine(Environment.CurrentDirectory, "countries_states_cities.json"), context);
    spinner.Stop("\nCompleted!");

    Console.WriteLine("All seed operations has been completed!");
}
catch (Exception ex)
{
    Console.WriteLine("Something went wrong! {0}", ex.Message);
}

static async Task ImportJsonDataAsync(string filePath, AppDbContext dbContext)
{
    using var stream = File.OpenRead(filePath);

    var countriesModel = await JsonSerializer.DeserializeAsync<List<CountryModel>>(stream, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    });

    var countries = new List<Country>();
    var states = new List<State>();
    var cities = new List<City>();

    foreach (var countryModel in countriesModel)
    {
        var country = new Country
        {
            Name = countryModel.Name,
            Iso2 = countryModel.Iso2,
            Iso3 = countryModel.Iso3,
            NumericCode = countryModel.NumericCode,
            PhoneCode = countryModel.PhoneCode,
            Currency = countryModel.Currency,
            Native = countryModel.Native,
            Region = countryModel.Region,
            FlagEmoji = countryModel.Emoji,
            FlagEmojiU = countryModel.EmojiU,
            GeoPoint = new GeoPoint(Parse(countryModel.Latitude), Parse(countryModel.Longitude)),
            Translations = countryModel.Translations ?? [],
            Timezones = countryModel.Timezones?.ToDictionary(t => t.ZoneName, t => t.TzName) ?? [],
            CreationDate = DateTime.UtcNow,
            CreatorUserName = "System"
        };

        countries.Add(country);

        foreach (var stateModel in countryModel.States)
        {
            var state = new State
            {
                Name = stateModel.Name,
                CountryCode = countryModel.Iso2,
                StateCode = stateModel.StateCode,
                Type = stateModel.Type,
                GeoPoint = new GeoPoint(Parse(stateModel.Latitude), Parse(stateModel.Longitude)),
                Country = country,
                CreationDate = DateTime.UtcNow,
                CreatorUserName = "System"
            };

            states.Add(state);

            foreach (var cityModel in stateModel.Cities)
            {
                var city = new City
                {
                    Name = cityModel.Name,
                    CountryCode = countryModel.Iso2,
                    StateCode = stateModel.StateCode,
                    GeoPoint = new GeoPoint(Parse(cityModel.Latitude), Parse(cityModel.Longitude)),
                    State = state,
                    CreationDate = DateTime.UtcNow,
                    CreatorUserName = "System"
                };

                cities.Add(city);
            }
        }
    }

    Console.WriteLine("Adding bulk data...");

    var bulkConfig = new BulkConfig
    {
        PreserveInsertOrder = true,
        SetOutputIdentity = true
    };

    await dbContext.BulkInsertAsync(countries, bulkConfig);

    foreach (var state in states)
    {
        state.CountryId = state.Country?.Id ?? countries.First(c => c.Iso2 == state.CountryCode).Id;
        state.Country = null;
    }
    await dbContext.BulkInsertAsync(states, bulkConfig);

    foreach (var city in cities)
    {
        city.StateId = city.State?.Id ?? states.First(s => s.StateCode == city.StateCode && s.CountryCode == city.CountryCode).Id;
        city.State = null;
    }

    await dbContext.BulkInsertAsync(cities, bulkConfig);
}

static string GetConnectionString(string[] args)
{
    // 1. Öncelik: args[0]
    if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
        return args[0];

    // 2. Alternatif: Environment variable
    var envConn = Environment.GetEnvironmentVariable("Geo_Connection_String");
    if (!string.IsNullOrWhiteSpace(envConn))
        return envConn;

    return null;
}

static bool GetForceOption(string[] args)
{
    return args.Any(a => a.Equals("--force", StringComparison.OrdinalIgnoreCase)) || Environment.GetEnvironmentVariable("FORCE_SEED")?.Equals("true", StringComparison.OrdinalIgnoreCase) == true;
}

static double Parse(string s) => double.TryParse(s, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var val) ? val : 0;

static async Task<bool> TablesExistAsync(AppDbContext dbContext)
{
    var requiredTables = new[] { "Countries", "States", "Cities" };

    var sql = """
        SELECT table_name
        FROM information_schema.tables
        WHERE table_schema = 'public';
    """;

    var result = await dbContext.Set<TableCheckResult>().FromSqlRaw(sql).ToListAsync();

    return requiredTables.All(t => result.Any(r => r.TableName.Equals(t, StringComparison.OrdinalIgnoreCase)));
}

static async Task<bool> AreTablesSeededAsync(AppDbContext dbContext)
{
    var countryCount = await dbContext.Countries.AsNoTracking().CountAsync();
    var stateCount = await dbContext.States.AsNoTracking().CountAsync();
    var cityCount = await dbContext.Cities.AsNoTracking().CountAsync();

    return countryCount > 0 && stateCount > 0 && cityCount > 0;
}

static async Task TruncateTablesAsync(AppDbContext dbContext)
{
    var conn = dbContext.Database.GetDbConnection();
    await conn.OpenAsync();

    using var cmd = conn.CreateCommand();
    cmd.CommandText = "TRUNCATE TABLE \"Cities\", \"States\", \"Countries\" RESTART IDENTITY CASCADE;";
    await cmd.ExecuteNonQueryAsync();

    await conn.CloseAsync();
}