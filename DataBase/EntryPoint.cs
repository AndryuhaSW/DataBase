using DB3;

class EntryPoint
{
    public static void Main(string[] args)
    {
        DB _db;

        Console.WriteLine("Chose option to give app connection string");
        Console.WriteLine("    c - from console input");
        Console.WriteLine("    j - from json config");
        switch (Convert.ToChar(Console.ReadLine()))
        {
            case 'c':
                _db = new DB_ConnectionStringFromConsole();
                break;
            case 'j':
                _db = new DB_ConnectionStringFromJSON();
                break;
            default:
                throw new Exception("No such option");
        }

        Console.WriteLine("======================================================");
        Console.WriteLine("    1 - point1. create data base");
        Console.WriteLine("    2 Andrey 2019/12/31 Male - point2. create note");
        Console.WriteLine("    3 - point3. uniq fio + date");
        Console.WriteLine("    4 - point4. create 1_000_000 + 100");
        Console.WriteLine("    5 - point5. Male + fio_F");
        Console.WriteLine("    6 - point6. Optimize data base");
        Console.WriteLine("======================================================");
        while (true)
        {
            string input = Console.ReadLine();
            switch (input[0])
            {
                case '1':
                    Point1(_db);
                    break;
                case '2':
                    Point2(_db, input);
                    break;
                case '3':
                    Point3(_db);
                    break;
                case '4':
                    Point4(_db);
                    break;
                case '5':
                    Point5(_db);
                    break;
                case '6':
                    Point6(_db);
                    break;
                default:
                    throw new Exception("No such action");
            }
            Console.WriteLine("------------------------------------------------------");
        }
    }


    private static void Point1(DB _db)
    {
        if (_db.CreateTable())
            Console.WriteLine("DB was created");
        else
            Console.WriteLine("DB not created");
    }

    private static void Point2(DB _db, string input)
    {
        string[] inputValues = input.Split(' ');
        try
        {
            if (_db.AddNote(inputValues[1], inputValues[2], inputValues[3]))
                Console.WriteLine("Note was created");
            else
                Console.WriteLine("Note not created");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Invalid input format");
        }
    }

    private static void Point3(DB _db)
    {
        _db.PrintPoint3();
    }

    private static void Point4(DB _db)
    {
        Thread executionThread = new Thread(() =>
        {
            _db.Point4();
        });
        executionThread.Start();

        //Simulated loading until executionThread executed
        int dotCount = 0;
        int cursorLeft = Console.CursorLeft;
        int cursorTop = Console.CursorTop;
        while (executionThread.IsAlive)
        {
            Thread.Sleep(300);

            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.Write(new string(' ', 3));

            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.Write(new string('.', dotCount));

            dotCount = (dotCount + 1) % 4;
        }

        Console.SetCursorPosition(cursorLeft, cursorTop);
        Console.WriteLine("Done");
    }

    private static void Point5(DB _db)
    {
        _db.PrintPoint5();
    }

    private static void Point6(DB _db)
    {
        Thread executionThread = new Thread(() =>
        {
            if (!_db.OptimizeDB())
                throw new Exception("Ops, something wrong");
        });
        executionThread.Start();

        //Simulated loading until executionThread executed
        int dotCount = 0;
        int cursorLeft = Console.CursorLeft;
        int cursorTop = Console.CursorTop;
        while (executionThread.IsAlive)
        {
            Thread.Sleep(300);

            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.Write(new string(' ', 3));

            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.Write(new string('.', dotCount));

            dotCount = (dotCount + 1) % 4;
        }

        Console.SetCursorPosition(cursorLeft, cursorTop);
        Console.WriteLine("Table was optimized");
    }
}