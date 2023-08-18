namespace DB3
{
    internal class Rand
    {
        private static Random _random = new Random();

        private static int _minYear = 1963;
        private static int _maxYear = 2023;
        //for equal distribution FIO
        private static int _count;


        //random date
        public static string GetRandomDate()
        {
            string result = "";

            result += _random.Next(_minYear, _maxYear) + "/";
            result += _random.Next(1, 13) + "/";
            result += _random.Next(1, 29);

            return result;
        }


        //random gender
        public static string GetRandomGender()
        {
            return _random.Next(2) % 2 == 0 ? "Female" : "Male";
        }


        //random fio
        public static string GetRandomFio()
        {
            int length = _random.Next(3, 10);
            string result = "";
            result += (char)((int)'A' + (_count % 26));

            for (int i = 0; i < length - 1; i++)
            {
                result += GetRandomSymbol();
            }

            _count++;
            return result;
        }

        //random fio with your first symbol
        public static string GetRandomFio(char startSymbol)
        {
            int length = _random.Next(3, 9);
            string result = "";
            result += startSymbol;

            for (int i = 0; i < length - 1; i++)
                result += GetRandomSymbol();

            return result;
        }

        //random symbol
        private static char GetRandomSymbol()
        {
            return (char)((int)'a' + (_random.Next() % 26));
        }
    }
}
