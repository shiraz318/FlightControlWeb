//This code was generated by a tool.
//Changes to this file will be lost if the code is regenerated.
// See the blog post here for help on using the generated code: http://erikej.blogspot.dk/2014/10/database-first-with-sqlite-in-universal.html
using System.Data.SQLite;
using System;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using FlightControlWeb.Models;
using static FlightControlWeb.Models.FlightPlan;

namespace FlightControlWeb
{

    public enum FlightPlanE
    {
        Id = 0,
        Passengers,
        CompanyName
    }
    public enum InitialLocationE
    {
        Id = 0,
        Longitude,
        Latitude,
        DateTime
    }
    public enum SegmentsE
    {
        Id = 0,
        FlightId,
        Place,
        Longitude,
        Latitude,
        TimespanSecond
    }
    public class SQLiteDb
    {
        int flightPlanIdE;
        int flightPlanPassangersE;
        int flightPlanCompanyNameE;

        int initialLocationIdE;
        int initialLocationLongitudeE;
        int initialLocationLatitudeE;
        int initalLocationDateTimeE;

        int segmentsIdE;
        int segmentFlightIdE;
        int segmentPlaceE;
        int segmentLongitudeE;
        int segmentLatitudeE;
        int segmenTimespanSecondE;

        string _path;
        int segmentsCount = 0;
        FlightGenerator flightGenerator = new FlightGenerator();
        public SQLiteDb(string path)
        {
            _path = path;
            // FlightPlan Enum.
            flightPlanIdE = (int)FlightPlanE.Id;
            flightPlanPassangersE = (int)FlightPlanE.Passengers;
            flightPlanCompanyNameE = (int)FlightPlanE.CompanyName;

            // InitialLocation Enum.
             initialLocationIdE = (int)InitialLocationE.Id;
             initialLocationLongitudeE = (int)InitialLocationE.Longitude;
             initialLocationLatitudeE = (int)InitialLocationE.Latitude;
             initalLocationDateTimeE = (int)InitialLocationE.DateTime;

            // Segments Enum.
             segmentsIdE = (int)SegmentsE.Id;
             segmentFlightIdE = (int)SegmentsE.FlightId;
             segmentPlaceE = (int)SegmentsE.Place;
             segmentLongitudeE = (int)SegmentsE.Longitude;
             segmentLatitudeE = (int)SegmentsE.Latitude;
             segmenTimespanSecondE = (int)SegmentsE.TimespanSecond;

        }

        public void Create()
        {
            using (SQLiteConnection db = new SQLiteConnection(_path))
            {
            }
        }

        public SqliteConnection OpenConnection()
        {
            SqliteConnection conn = new SqliteConnection(@"Data Source = " + _path);
            conn.Open();
            return conn;
        }

        public object[] ReadFromTableSingleRow(SqliteConnection conn, string commendText)
        {

            SqliteCommand selectCommand = new SqliteCommand();
            selectCommand.Connection = conn;
            selectCommand.CommandText = commendText;
            SqliteDataReader query = selectCommand.ExecuteReader();
            object[] row = new object[query.FieldCount];
            if (query.Read())
            {
                query.GetValues(row);
            }
            query.Close();
            return row;
        }
       
        public List<object[]> ReadMultipleLines(SqliteConnection conn, string commendText)
        {
            SqliteCommand selectCommand = new SqliteCommand();
            selectCommand.Connection = conn;
            selectCommand.CommandText = commendText;
            SqliteDataReader query = selectCommand.ExecuteReader();
            List<object[]> rows = new List<object[]>();
            
            while (query.Read())
            {
                object[] row = new object[query.FieldCount];
                query.GetValues(row);

                rows.Add(row);
                //query.GetValues(row);
            }
            query.Close();
            return rows;

        }

        public void getSegmentsCount(SqliteConnection conn, string id)
        {

            SqliteCommand selectCommand = new SqliteCommand();
            selectCommand.Connection = conn;

            selectCommand.CommandText = "SELECT MAX(Place) FROM SegmentsTable WHERE FlightId = '" + id + "'";

            SqliteDataReader query = selectCommand.ExecuteReader();
            if (query.Read())
            {
                segmentsCount = query.GetInt32(0);
            }
        }


        public bool NotStarted (object[] initialLocation, DateTime requiredTime)
        {
            DateTime flightPlanDateTime = Convert.ToDateTime(initialLocation[initalLocationDateTimeE]); 
            int result = DateTime.Compare(requiredTime, flightPlanDateTime);

            // requiredTime is earlier than flightPlanDateTime
            if (result < 0)
            {
                return true;
            }
            return false;
        }

        public bool AlreadyFinished(int timeOfAllFlight ,DateTime startTime, DateTime requiredTime)
        {
            TimeSpan duration = new TimeSpan(0, 0, 0, timeOfAllFlight);
            DateTime endTime = startTime.Add(duration);

            int result = DateTime.Compare(requiredTime, endTime);
            // requiredTime is later than endTime
            if (result >= 0)
            {
                return true;
            }
            return false;
        }

        public Flights CreateFlight(SqliteConnection conn, object[] initialLocation, List<Object[]> segements, bool isExternal, DateTime time)
        {
            Flights flight = flightGenerator.CreateFlightFromGivenData(initialLocation, segements, isExternal, time);
            object[] flightPlan = ReadFromTableSingleRow(conn, "SELECT * FROM FlightPlanTable WHERE Id = '" + flight.FlightId + "'");
            flight.CompanyName = Convert.ToString(flightPlan[flightPlanCompanyNameE]);
            flight.Passengers = Convert.ToInt32(flightPlan[flightPlanPassangersE]);
            return flight;
        }

        public List<Flights> GetExternal(DateTime time)
        {
            List<Flights> flights = new List<Flights>();
            return flights;
        }

        public List<Flights> GetFlights(DateTime time)
        {
            List<Flights> flights = new List<Flights>();
            flights = GetInternalFlights(time);
            return flights;
        } 

        public List<Flights> GetInternalFlights(DateTime time)
        {
            // Reading all the data.
            SqliteConnection conn = OpenConnection();
            SqliteCommand selectCommand = new SqliteCommand();
            selectCommand.Connection = conn;
            selectCommand.CommandText = "SELECT * FROM InitialLocationTable";
            SqliteDataReader query = selectCommand.ExecuteReader();
            List<Flights> flights = new List<Flights>();
            while (query.Read())
            {
                object[] row = new object[query.FieldCount];
                query.GetValues(row);
                // InitialTime is in the future
                if (NotStarted(row, time))
                {
                    continue;
                }
                string command = "SELECT SUM(TimespanSeconds) FROM SegmentsTable WHERE FlightId= '" + row[initialLocationIdE].ToString() + "'";
                object[] sumTime = ReadFromTableSingleRow(conn, command);
                if (AlreadyFinished(Convert.ToInt32(sumTime[0]), Convert.ToDateTime(row[initalLocationDateTimeE]), time))
                {
                    continue;
                }
                command = "SELECT * FROM SegmentsTable WHERE FlightId= '" + row[initialLocationIdE].ToString() + "'";
                List<Object[]> sgements = ReadMultipleLines(conn, command);
                flights.Add(CreateFlight(conn, row, sgements, false, time));
            }
            query.Close();
            return flights;
        }

        public List<Server> CreateServers(List<object[]> tempServers)
        {
            List<Server> servers = new List<Server>();

            foreach (object[] server in tempServers)
            {
                Server serv = new Server();
                serv.ServerId = Convert.ToString(server[0]);
                serv.ServerURL = Convert.ToString(server[1]);
                servers.Add(serv);
            }

            return servers;

        }

        public List<Server> GetServers()
        {
            SqliteConnection conn = OpenConnection();
            List<Server> servers = new List<Server>();
            List<object[]> tempServers = ReadMultipleLines(conn, "SELECT * FROM ServersTable");
            servers = CreateServers(tempServers);
            conn.Close();
            return servers;

        }

        public FlightPlan GetFlightPlan(string id)
        {
            SqliteConnection conn = OpenConnection();
            //getSegmentsCount(conn, id);
            object[] basicData = ReadFromTableSingleRow(conn, "SELECT * FROM FlightPlanTable WHERE Id = '" + id + "'");
            object[] initialLocation = ReadFromTableSingleRow(conn, "SELECT * FROM InitialLocationTable WHERE Id= '" + id + "'");
            // ORDER BY Place ASC
            List<object[]> segments = ReadMultipleLines(conn, "SELECT * FROM SegmentsTable  WHERE FlightId= '" + id + "' ORDER BY Place ASC");
            conn.Close();
            return setFlightPlan(basicData, initialLocation, segments);
        }

        public List<FlightPlan> GetAllFlightPlans()
        {
            SqliteConnection conn = OpenConnection();
            List<FlightPlan> list = new List<FlightPlan>();
            List<object[]> ids = ReadMultipleLines(conn, "SELECT Id FROM FlightPlanTable");
            foreach (object[] id in ids)
            {
                list.Add(GetFlightPlan(Convert.ToString(id[0])));
            }
            conn.Close();
            return list;
        }


        public void InsertServer(Server server)
        {
            SqliteConnection conn = OpenConnection();
            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = conn;

            insertCommand.CommandText = "INSERT INTO ServersTable VALUES (@Id , @Url)";
            insertCommand.Parameters.AddWithValue("@Id", server.ServerId);
            insertCommand.Parameters.AddWithValue("@Url", server.ServerURL);
            try
            {
                insertCommand.ExecuteReader();
            }
            catch { }

            conn.Close();
        }

        public void InsertToFlightPlanTable(SqliteConnection conn, FlightPlan flightPlan, string id)
        {
            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = conn;

            insertCommand.CommandText = "INSERT INTO FlightPlanTable VALUES (@Id , @Passengers, @CompanyName)";
            insertCommand.Parameters.AddWithValue("@Id", id);
            insertCommand.Parameters.AddWithValue("@Passengers", flightPlan.Passengers);
            insertCommand.Parameters.AddWithValue("@CompanyName", flightPlan.CompanyName);

            try
            {
                insertCommand.ExecuteReader();
            }
            catch { }
        }

        public void InsertToInitialLocationTable(SqliteConnection conn, FlightPlan flightPlan, string id)
        {
            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = conn;

            insertCommand.CommandText = "INSERT INTO InitialLocationTable VALUES (@Id , @Longitude , @Latitude , @DateTime)";
            insertCommand.Parameters.AddWithValue("@Id", id);
            insertCommand.Parameters.AddWithValue("@Longitude", flightPlan.InitialLocation.Longitude);
            insertCommand.Parameters.AddWithValue("@Latitude", flightPlan.InitialLocation.Latitude);
            insertCommand.Parameters.AddWithValue("@DateTime", flightPlan.InitialLocation.DateTime);

            try
            {
                insertCommand.ExecuteReader();
            }
            catch { }
        }

        public void InsertToSegmentsTable(SqliteConnection conn, FlightPlan flightPlan, string id)
        {
            int i, size = flightPlan.Segments.Count;
            for (i = 0; i < size; i++)
            {
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = conn;
                insertCommand.CommandText = "INSERT INTO SegmentsTable VALUES (NULL, @FlightId, @Place, @Longitude , @Latitude, @TimespanSeconds)";
                insertCommand.Parameters.AddWithValue("@FlightId", id);
                insertCommand.Parameters.AddWithValue("@Place", i+1);
                insertCommand.Parameters.AddWithValue("@Longitude", flightPlan.Segments[i].Longitude);
                insertCommand.Parameters.AddWithValue("@Latitude", flightPlan.Segments[i].Latitude);
                insertCommand.Parameters.AddWithValue("@TimespanSeconds", flightPlan.Segments[i].TimespanSeconds);
                try
                {
                    insertCommand.ExecuteReader();
                }
                catch { }
            }
        }

        public void InsertFlightPlan(FlightPlan flightPlan, string id)
        {
            SqliteConnection conn = OpenConnection();
            InsertToFlightPlanTable(conn, flightPlan, id);
            //InsertToDictionaryTable(id);
            InsertToInitialLocationTable(conn, flightPlan, id);
            InsertToSegmentsTable(conn, flightPlan, id);
            conn.Close();
        }

        public void DeleteServer(string id)
        {
            SqliteConnection conn = OpenConnection();
            SqliteCommand deleteCommand = new SqliteCommand();
            deleteCommand.Connection = conn;
            deleteCommand.CommandText = "DELETE FROM ServersTable WHERE Id='" + id + "'";
            deleteCommand.ExecuteReader();
            conn.Close();
        }

        public bool DeleteFlightPlan(string id)
        {
            SqliteConnection conn = OpenConnection();
            bool returnVal = true;
            
            string[] tables = { "FlightPlanTable", "InitialLocationTable", "SegmentsTable" };
            int i = 0;
            for (i = 0; i < tables.Length; i++)
            {
                SqliteCommand deleteCommand = new SqliteCommand();
                deleteCommand.Connection = conn;
                deleteCommand.CommandText = "DELETE FROM " + tables[i] + " WHERE Id='" + id + "'";
                deleteCommand.ExecuteReader();
            }
            conn.Close();
            return returnVal;
        }

         public FlightPlan setFlightPlan(object[] basicData, object[] initialLocation, List<object[]> segments)
        {
            FlightPlan flightPlan = new FlightPlan();

            //flightPlan.Id = Convert.ToString(basicData[flightPlanIdE]);
            flightPlan.Passengers = Convert.ToInt32(basicData[flightPlanPassangersE]);
            flightPlan.CompanyName = Convert.ToString(basicData[flightPlanCompanyNameE]);

            double longitude = Convert.ToDouble(initialLocation[initialLocationLongitudeE]);
            double latitude = Convert.ToDouble(initialLocation[initialLocationLatitudeE]);
            DateTime dateTime = Convert.ToDateTime(initialLocation[initalLocationDateTimeE]);
            Location location = new Location(longitude, latitude, dateTime);
            flightPlan.InitialLocation = location;

            foreach (object[] segment in segments)
            {
                double longitudeS = Convert.ToDouble(segment[segmentLongitudeE]);
                double latitudeS = Convert.ToDouble(segment[segmentLatitudeE]);
                int TimespanSecond = Convert.ToInt32(segment[segmenTimespanSecondE]);
                Segment segment1 = new Segment(longitudeS, latitudeS, TimespanSecond);
                flightPlan.Segments.Add(segment1);
            }
            return flightPlan;
        }

        public static void CreateFlightPlanTable(SqliteConnection conn)
        {
            string tableCommand = @"CREATE TABLE IF NOT EXISTS FlightPlanTable (Id TEXT PRIMARY KEY, Passengers INTEGER, CompanyName TEXT)";
            SqliteCommand createTable = new SqliteCommand(tableCommand, conn);
            createTable.ExecuteReader();
        }

        public static void CreateInitialLocationTable(SqliteConnection conn)
        {
            string tableCommand = @"CREATE TABLE IF NOT EXISTS InitialLocationTable (Id TEXT PRIMARY KEY, Longitude REAL , Latitude REAL, DateTime TEXT)";
            SqliteCommand createTable = new SqliteCommand(tableCommand, conn);
            createTable.ExecuteReader();
        }

        public static void CreateSegmentsTable(SqliteConnection conn)
        {
            string tableCommand = @"CREATE TABLE IF NOT EXISTS SegmentsTable (Id TEXT PRIMARY KEY, FlightId TEXT, Place INTEGER, Longitude REAL , Latitude REAL, TimespanSeconds INTEGER)";
            SqliteCommand createTable = new SqliteCommand(tableCommand, conn);
            createTable.ExecuteReader();
        }

        public static void CreateServersTable(SqliteConnection conn)
        {
            string tableCommand = @"CREATE TABLE IF NOT EXISTS ServersTable (Id TEXT PRIMARY KEY, Url TEXT)";
            SqliteCommand createTable = new SqliteCommand(tableCommand, conn);
            createTable.ExecuteReader();
        }

        public static void InitializeDatabase()
        {

            //string dbPath = Environment.CurrentDirectory + @"\Database.sqlite";
            string dbPath = AppDomain.CurrentDomain.BaseDirectory + @"\Database.sqlite";
            SqliteConnection conn = new SqliteConnection(@"Data Source = " + dbPath);
            conn.Open();
            //string com = "DROP Table SegmentsTable";
            //SqliteCommand delete = new SqliteCommand(com, conn);
            //delete.ExecuteReader();
            CreateFlightPlanTable(conn);
            CreateInitialLocationTable(conn);
            CreateSegmentsTable(conn);
            CreateServersTable(conn);
            conn.Close();
       }
    }
}
