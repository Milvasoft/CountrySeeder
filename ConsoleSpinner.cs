namespace CountrySeeder;

public class ConsoleSpinner
{
    private readonly string[] _sequence = new[] { "|", "/", "-", "\\" };
    private int _counter = 0;
    private bool _active = false;
    private readonly int _left;
    private readonly int _top;
    private Thread _thread;

    public ConsoleSpinner(string label)
    {
        Console.Write(label + " ");
        _left = Console.CursorLeft;
        _top = Console.CursorTop;
        Start();
    }

    public void Start()
    {
        _active = true;
        _thread = new Thread(Spin);
        _thread.Start();
    }

    private void Spin()
    {
        while (_active)
        {
            Console.SetCursorPosition(_left, _top);
            Console.Write(_sequence[_counter++ % _sequence.Length]);
            Thread.Sleep(100);
        }
    }

    public void Stop(string message = "✓")
    {
        _active = false;
        _thread.Join(); // Thread bitmeden devam etmesin
        Console.SetCursorPosition(_left, _top);
        Console.Write(message + "\n");
    }
}
