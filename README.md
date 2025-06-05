# 🌍 CountrySeeder

A high-performance EF Core seeding tool that imports countries, states, and cities into a PostgreSQL database from a large JSON file.  
It uses `EFCore.BulkExtensions` for blazing-fast data insert and supports command-line or environment-based configuration.

---

## 📦 Features

- ⚡ Bulk insert with [`EFCore.BulkExtensions`](https://github.com/borisdj/EFCore.BulkExtensions)
- 🌐 Loads hierarchical country-state-city data from `countries_states_cities.json`
- 🧩 Supports full JSON structure: coordinates, translations, timezones, etc.
- 🧼 Automatic truncate of existing data (with `CASCADE` and identity reset)
- 🧪 Safe seeding: detects existing records and skips unless `--force` is used
- 💻 CLI/ENV-based config (`--force`, `--truncate`, connection string)

---

## 📁 Folder Structure

```

CountrySeeder/
├── Program.cs                 → Entry point with seeding logic
├── AppDbContext.cs           → EF Core DbContext
├── Entities/                 → Country, State, City entities
├── Models/                   → CountryModel, StateModel, CityModel (for deserialization)
├── countries\_states\_cities.json  → JSON data source (not included in repo)

````

---

## 🚀 Usage

### 1. 🔧 Set Up

Install dependencies:

```bash
dotnet restore
````

Add the required NuGet package:

```bash
dotnet add package EFCore.BulkExtensions
```

Make sure your PostgreSQL database has the following tables:

* `Countries`
* `States`
* `Cities`

> **Tip:** Use EF Core migrations if needed.

---

### 2. 📦 Run Seeder

#### With command-line argument:

```bash
dotnet run -- "Host=localhost;Database=GeoDb;Username=postgres;Password=1234"
```

#### With environment variable:

```bash
set Geo_Connection_String=Host=localhost;Database=GeoDb;Username=postgres;Password=1234
dotnet run
```

---

### 3. 🧪 Optional Flags

| Flag / Env              | Description                                        |
| ----------------------- | -------------------------------------------------- |
| `--force`               | Forces seeding even if tables already contain data |
| `FORCE_SEED=true`       | Same as `--force`, for CI or container use         |

---

## 🧬 JSON Format

This tool expects a file named `countries_states_cities.json` at the root. File exists in folder but if not exists;

You can download it from the original source:

📥 [https://github.com/dr5hn/countries-states-cities-database/blob/master/json/countries%2Bstates%2Bcities.json](https://github.com/dr5hn/countries-states-cities-database/blob/master/json/countries%2Bstates%2Bcities.json)

> Rename to: `countries_states_cities.json`

---

## ✅ Example Output

```
Checking table existence...
Cleaning tables...
Seeding starting...
Reading json data file...
Seeding...
✔ Completed!
All seed operations has been completed!
```

---

## 🛠 Dependencies

* [.NET 8+](https://dotnet.microsoft.com)
* PostgreSQL 12+
* [EF Core Bulk Extensions](https://github.com/borisdj/EFCore.BulkExtensions)

---

## 📄 License

MIT License © 2025 – use freely, attribution appreciated!


