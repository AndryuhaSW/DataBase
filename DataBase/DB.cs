using Newtonsoft.Json.Linq;
using Npgsql;
using System.Data;
using System.Diagnostics;

namespace DB3
{
    public class DB_ConnectionStringFromConsole : DB
    {
        protected override string GetConnectionString()
        {
            Console.WriteLine("Write connection string");
            return Console.ReadLine();
        }
    }

    public class DB_ConnectionStringFromJSON : DB
    {
        protected override string GetConnectionString()
        {
            string configFileContents = File.ReadAllText("config.json");
            JObject config = JObject.Parse(configFileContents);
            return config["database"]["connectionString"].ToString();
        }
    }

    abstract public class DB
    {
        private string _connectionString;
        private NpgsqlConnection _dbConnection;
        private NpgsqlCommand _dbCommand;

        public DB()
        {
            _connectionString = GetConnectionString();

            ConnectDB();

            Console.WriteLine("Data base was connected");
        }

        //option to give connection string to app
        protected virtual string GetConnectionString() => null;


        //point 6
        public bool OptimizeDB()
        {
            string sqlRequest = "create index index_gender_fio on people (gender, fio);";

            if (ExecuteRequestReturnSuccess(sqlRequest))
                return true;
            else
                return false;
        }


        //Point 5
        public void PrintPoint5()
        {
            string sqlRequest = "SELECT count(*) FROM people WHERE gender='Male' and fio >= 'a' AND fio< 'b';";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var dataTable = ExecuteRequestReturnTable(sqlRequest);

            stopwatch.Stop();

            Console.WriteLine($"Count selected notes : {dataTable.Rows[0][0].ToString()}");
            Console.WriteLine($"Execution time : {stopwatch.Elapsed.TotalMilliseconds} milliseconds");
        }


        //Point 4
        public void Point4()
        {
            for (int i = 0; i < 1_000_000; i++)
                AddNote(Rand.GetRandomFio(), Rand.GetRandomDate(), Rand.GetRandomGender());

            for (int i = 0; i < 100; i++)
                AddNote(Rand.GetRandomFio('F'), Rand.GetRandomDate(), "Male");
        }


        //Point 3
        public void PrintPoint3()
        {
            string sqlRequest = "select distinct on (fio, date_of_birth) fio, date_of_birth, " +
                "gender, extract(year from age(now(), date_of_birth)) as age from people order by fio ASC;";
            DataTable dataTable = ExecuteRequestReturnTable(sqlRequest);

            //print dataTable to console
            Console.WriteLine("fio\tdate_of_birth\tgender\tage");
            foreach (DataRow row in dataTable.Rows)
            {
                Console.WriteLine($"{row["fio"]}\t{((DateTime)row["date_of_birth"]).ToString("yyyy-MM-dd")}" +
                    $"\t{row["gender"]}\t{row["age"]}");
            }
        }

        // point 2
        public bool AddNote(string fio, string dateOfBirth, string gender)
        {
            string sqlRequest = "insert into people(FIO, DATE_OF_BIRTH, GENDER) " +
                "values('" + fio + "', '" + dateOfBirth + "', '" + gender + "')";
            if (ExecuteRequestReturnCount(sqlRequest) > 0)
                return true;
            else
                return false;
        }

        //point 1
        public bool CreateTable()
        {
            string sqlRequest = "create table people( FIO varchar(50) not null," +
                "DATE_OF_BIRTH date not null, GENDER varchar(10) not null);";
            if (ExecuteRequestReturnSuccess(sqlRequest))
                return true;
            else
                return false;
        }


        //return result of sqlRequest
        private NpgsqlCommand WriteSqlRequest(string sqlRequest)
        {
            if (IsDBClosed())
                throw new Exception("data base doesnt connect");

            _dbCommand = new NpgsqlCommand();
            _dbCommand.Connection = _dbConnection;
            _dbCommand.CommandText = sqlRequest;

            return _dbCommand;
        }

        // for: insert, update, delete
        private int ExecuteRequestReturnCount(string sqlRequest)
        {
            _dbCommand = WriteSqlRequest(sqlRequest);

            try
            {
                return _dbCommand.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("Table not exist");
            }
        }

        // for: select
        private DataTable ExecuteRequestReturnTable(string sqlRequest)
        {
            _dbCommand = WriteSqlRequest(sqlRequest);

            NpgsqlDataReader dataReader = _dbCommand.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);

            return dataTable;
        }

        // for: create, alter, drop
        private bool ExecuteRequestReturnSuccess(string sqlRequest)
        {
            _dbCommand = WriteSqlRequest(sqlRequest);

            try
            {
                _dbCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //is data base not connected to app
        private bool IsDBClosed()
        {
            return _dbConnection.State == ConnectionState.Closed;
        }

        //connect to data base
        private void ConnectDB()
        {
            _dbConnection = new NpgsqlConnection();
            try
            {
                _dbConnection.ConnectionString = _connectionString;
            }
            catch
            {
                throw new Exception("Incorrect connection string");
            }

            if (IsDBClosed())
                _dbConnection.Open();
            else
                throw new Exception("DB already connected");
        }
    }
}
