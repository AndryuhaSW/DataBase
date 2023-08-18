C# + PostgreSQL

В приложении 2  способа передачи строки подключения к базе данных. Один через консоль, а второй с помощью json файла (config.json).

Есть следующие функции:

    1 - создание таблицы people.
    2 _FIO _DateOfBirth Gender - Создание записи в таблице people с такими параметрами.
    3 - вывод таблицы: фио, дата, пол, возраст. С уникальными ФИО+дата, сортировка по ФИО.
    4 - создание 1_000_000 записей с рандомными параметрами(первая буква ФИО и пол должны распределяться
    равномерно по таблице). + 100: пол=мужской, ФИО гачинается с 'F'.
    5 - Результат выборки пол=мужской, ФИО начинается с 'F'. + скорость выполнения.
    6 - оптимизация таблицы с помощью добавления индекса(ФИО+Пол)


В проекте 3 основных класса:

(1)EntryPoint -- отвечает за консольное меню


(2)DB --> DB_ConnectionStringFromJSON, DB_ConnectionStringFromConsole

    конструктор
    virtual string GetConnectionString() - возвращает строку подключения. 
    Виртуальный чтобы переопределить его в клссах наследниках,
    сделав разную реализацию передачи строки подключения.
    void ConnectDB() - пробует подключиться к базе данных

    bool ExecuteRequestReturnSuccess(string sqlRequest) // for: create, alter, drop
    DataTable ExecuteRequestReturnTable(string sqlRequest)   // for: select
    int ExecuteRequestReturnCount(string sqlRequest)   // for: insert, update, delete
    --используются в зависимости от того, что возвращает sql запрос.

    bool CreateTable() - Пункт 1
    bool AddNote(string fio, string dateOfBirth, string gender) - Пункт 2
    void PrintPoint3() - Пункт 3
    void Point4() - Пункт 4
    void PrintPoint5() - Пункт 5
    bool OptimizeDB() - Пункт 6


(3)Rand - класс с функциями для генерации ФИО, даты рождения и пола с нужными нам пропорциями

    string GetRandomFio () - возврвщвет рандомное ФИО. Есть счетчик 'c', он нужен чтобы
    распределение в таблице ФИО(буква с которой начинается) было относительно одинаковым.
    string GetRandomFio (char startSymbol) - возврвщвет рандомное ФИО начинающееся с нужной буквы.
    string GetRandomGender() - возвращает один из 2х полов.
    string GetRandomDate() - возвращает случайную дату от _minYear до _maxYear.

