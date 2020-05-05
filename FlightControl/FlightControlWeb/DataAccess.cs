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
    public class SQLiteDb
    {
        string _path;
        int segmentsCount = 0;
        public SQLiteDb(string path)
        {
            _path = path;
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
        public object[] ReadFromTable(SqliteConnection conn, string commendText)
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
       
        public List<object[]> ReadSegments(SqliteConnection conn, string commendText)
        {
            SqliteCommand selectCommand = new SqliteCommand();
            selectCommand.Connection = conn;
            selectCommand.CommandText = commendText;
            SqliteDataReader query = selectCommand.ExecuteReader();
            List<object[]> list = new List<object[]>();
            
            while (query.Read())
            {
                object[] row = new object[query.FieldCount];
                query.GetValues(row);
                list.Add(row);
                //query.GetValues(row);
            }
            query.Close();
            return list;

        }
        public FlightPlan setFlightPlan(object[] basicData, object[] initialLocation, List<object[]> segments)
        {
            FlightPlan flightPlan = new FlightPlan();

            flightPlan.Id = Convert.ToString(basicData[0]);
            flightPlan.Passengers = Convert.ToInt32(basicData[1]);
            flightPlan.CompanyName = Convert.ToString(basicData[2]);
            
            // InitialLocation[0] is Id.
            double longitude = Convert.ToDouble(initialLocation[1]);
            double latitude = Convert.ToDouble(initialLocation[2]);
            string dateTime = Convert.ToString(initialLocation[3]);
            Location location = new Location(longitude, latitude, dateTime);
            flightPlan.InitialLocation = location;

            foreach(object[] segment in segments)
            {
                // segment[0] is Id.
                // segment[1] is FlightId.
                // segment[2] is Place.
                double longitudeS = Convert.ToDouble(segment[3]);
                double latitudeS = Convert.ToDouble(segment[4]);
                int TimespanSecond = Convert.ToInt32(segment[5]);
                Segment segment1 = new Segment(longitudeS, latitudeS, TimespanSecond);
                flightPlan.Segments.Add(segment1);
            }
            return flightPlan;
        }
        public void getSegmentsCount(SqliteConnection conn, string id)
        {

            SqliteCommand selectCommand = new SqliteCommand();
            selectCommand.Connection = conn;

            selectCommand.CommandText = "SELECT MAX(Place) FROM SegmentsTable WHERE FlightId = " + id;

            SqliteDataReader query = selectCommand.ExecuteReader();
            if (query.Read())
            {
                segmentsCount = query.GetInt32(0);
            }
        }
        public FlightPlan GetFlightPlan(string id)
        {
            SqliteConnection conn = OpenConnection();
            //getSegmentsCount(conn, id);
            object[] basicData = ReadFromTable(conn, "SELECT * FROM FlightPlanTable WHERE Id = '" + id + "'");
            object[] initialLocation = ReadFromTable(conn, "SELECT * FROM InitialLocationTable WHERE Id= '" + id + "'");
            List<object[]> segments = ReadSegments(conn, "SELECT * FROM SegmentsTable WHERE FlightId= '" + id + "'");
            conn.Close();
             return setFlightPlan(basicData, initialLocation, segments);
        }

        public void InsertToFlightPlanTable(SqliteConnection conn, FlightPlan flightPlan)
        {
            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = conn;

            insertCommand.CommandText = "INSERT INTO FlightPlanTable VALUES (@Id , @Passengers, @CompanyName)";
            insertCommand.Parameters.AddWithValue("@Id", flightPlan.Id);
            insertCommand.Parameters.AddWithValue("@Passengers", flightPlan.Passengers);
            insertCommand.Parameters.AddWithValue("@CompanyName", flightPlan.CompanyName);

            try
            {
                insertCommand.ExecuteReader();
            }
            catch { }
        }
        public void InsertToInitialLocationTable(SqliteConnection conn, FlightPlan flightPlan)
        {
            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = conn;

            insertCommand.CommandText = "INSERT INTO InitialLocationTable VALUES (@Id , @Longitude , @Latitude , @DateTime)";
            insertCommand.Parameters.AddWithValue("@Id", flightPlan.Id);
            insertCommand.Parameters.AddWithValue("@Longitude", flightPlan.InitialLocation.Longitude);
            insertCommand.Parameters.AddWithValue("@Latitude", flightPlan.InitialLocation.Latitude);
            insertCommand.Parameters.AddWithValue("@DateTime", flightPlan.InitialLocation.DateTime);

            try
            {
                insertCommand.ExecuteReader();
            }
            catch { }
        }

        public void InsertToSegmentsTable(SqliteConnection conn, FlightPlan flightPlan)
        {
            int i, size = flightPlan.Segments.Count;
            for (i = 0; i < size; i++)
            {
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = conn;
                insertCommand.CommandText = "INSERT INTO SegmentsTable VALUES (NULL, @FlightId, @Place, @Longitude , @Latitude, @TimespanSeconds)";
                insertCommand.Parameters.AddWithValue("@FlightId", flightPlan.Id);
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
        public void InsertFlightPlan(FlightPlan flightPlan)
        {
            SqliteConnection conn = OpenConnection();
            InsertToFlightPlanTable(conn, flightPlan);
            InsertToInitialLocationTable(conn, flightPlan);
            InsertToSegmentsTable(conn, flightPlan);
            conn.Close();
        }
        public void InsertFlight(Flights flight)
        {
            SqliteConnection conn = OpenConnection();
            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = conn;

            insertCommand.CommandText = "INSERT INTO FlightsTable VALUES (@Id, @Longitude, @Latitude, @Passengers, @CompanyName, @DateTime, @IsExternal)";
            insertCommand.Parameters.AddWithValue("@Id", flight.FlightId);
            insertCommand.Parameters.AddWithValue("@Longitude", flight.Longitude);
            insertCommand.Parameters.AddWithValue("@Latitude", flight.Latitude);
            insertCommand.Parameters.AddWithValue("@Passengers", flight.Passengers);
            insertCommand.Parameters.AddWithValue("@CompanyName", flight.CompanyName);
            insertCommand.Parameters.AddWithValue("@DateTime", flight.DateTime);
            insertCommand.Parameters.AddWithValue("@IsExternal", flight.IsExternal);
            try
            {
                insertCommand.ExecuteReader();
            }
            catch { }
            conn.Close();
        }

        public bool DeleteFlight(string id)
        {
            SqliteConnection conn = OpenConnection();
            bool returnVal = true;
            SqliteCommand deleteCommand = new SqliteCommand();
            deleteCommand.Connection = conn;
            deleteCommand.CommandText = "DELETE FROM FlightsTable WHERE Id=" + id;
            deleteCommand.ExecuteReader();
            conn.Close();
            return returnVal;
        }

        public void InsertData(string data, int num)
        {
            SqliteConnection conn = new SqliteConnection(@"Data Source = " + _path);
            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = conn;
            conn.Open();

            // Use parameterized query to prevent SQL injection attacks
            insertCommand.CommandText = "INSERT INTO newTable VALUES (NULL, @Entry, @price);";
            insertCommand.Parameters.AddWithValue("@Entry", data);
            insertCommand.Parameters.AddWithValue("@price", num);
            
            insertCommand.ExecuteReader();
            conn.Close();
        }



        public List<string> GetData()
        {
            List<string> entries = new List<string>();
            SqliteConnection conn = new SqliteConnection(@"Data Source = " + _path);
            try
            {
                conn.Open();

                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = conn;
                selectCommand.CommandText = "SELECT * FROM newTable WHERE price='20'";
                
                SqliteDataReader query = selectCommand.ExecuteReader();
                object[] row = new object[query.FieldCount];
               while (query.Read())
                {
                    query.GetValues(row);
                    foreach (var value in row)
                    {
                        entries.Add(value.ToString());
                    }
                }
                //while (query.Read())
                //{
                //    query.GetValues();
                //    entries.Add(query.GetString(0));
                //}
                query.Close();
            }
            catch 
            {

            }
            conn.Close();
            return entries;

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
        public static void CreateFlightsTable(SqliteConnection conn)
        {
            string tableCommand = @"CREATE TABLE IF NOT EXISTS FlightsTable (Id TEXT PRIMARY KEY, Longitude REAL, Latitude REAL, Passengers INTEGER, CompanyName TEXT, DateTime TEXT, IsExternal INTEGER)";
            SqliteCommand createTable = new SqliteCommand(tableCommand, conn);
            createTable.ExecuteReader();
        }

        public static void InitializeDatabase()
        {

            string dbPath = Environment.CurrentDirectory + @"\Database.sqlite";
            SqliteConnection conn = new SqliteConnection(@"Data Source = " + dbPath);

                conn.Open();
            //    String tableCommand = @"CREATE TABLE IF NOT EXISTS newTable (id INTEGER PRIMARY KEY,
            //Text_Entry TEXT, price INT)";
            //SqliteCommand createTable = new SqliteCommand(tableCommand, conn);
            //createTable.ExecuteReader();
            CreateFlightPlanTable(conn);
            CreateInitialLocationTable(conn);
            CreateSegmentsTable(conn);
            CreateFlightsTable(conn);
            conn.Close();
       }
    }
}
