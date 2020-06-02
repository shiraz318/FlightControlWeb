using System;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using FlightControlWeb.Models;
using static FlightControlWeb.Models.FlightPlan;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;

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
    public class DataAccess : IDataAccess
    {
        private int flightPlanPassangersE;
        private int flightPlanCompanyNameE;

        private int initialLocationIdE;
        private int initialLocationLongitudeE;
        private int initialLocationLatitudeE;
        private int initalLocationDateTimeE;

        private int segmentLongitudeE;
        private int segmentLatitudeE;
        private int segmenTimespanSecondE;

       // private string _path;
        private static SqliteConnection conn;
        private static Mutex mutex = new Mutex();
        private FlightGenerator flightGenerator = new FlightGenerator();

        // Constructor.
        public DataAccess()
        {
            // FlightPlan Enum.
            flightPlanPassangersE = (int)FlightPlanE.Passengers;
            flightPlanCompanyNameE = (int)FlightPlanE.CompanyName;

            // InitialLocation Enum.
            initialLocationIdE = (int)InitialLocationE.Id;
            initialLocationLongitudeE = (int)InitialLocationE.Longitude;
            initialLocationLatitudeE = (int)InitialLocationE.Latitude;
            initalLocationDateTimeE = (int)InitialLocationE.DateTime;

            // Segments Enum.
            segmentLongitudeE = (int)SegmentsE.Longitude;
            segmentLatitudeE = (int)SegmentsE.Latitude;
            segmenTimespanSecondE = (int)SegmentsE.TimespanSecond;

        }
        // Delete all flights of a given server.
        public void DeleteServerFromExternalFlight(Server server)
        {
            string id = server.ServerId;
            if (IsExist("ExternalFlightsTable", "Id", id))
            {
                OpenConnection();
                SqliteCommand deleteOtherCommand = new SqliteCommand();
                deleteOtherCommand.Connection = conn;
                deleteOtherCommand.CommandText = "DELETE FROM ExternalFlightsTable WHERE Id='" + id + "'";
                deleteOtherCommand.ExecuteReader();
                CloseConncetion();
            }
        }



        // Insert a Server and FlightId to the ExternalFlightsTable.
        public void InsertExtenalFlightId(Server server, string id)
        {
            OpenConnection();
            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = conn;

            insertCommand.CommandText = "INSERT INTO ExternalFlightsTable VALUES" +
                " (@Id, @Url , @FlightId)";
            insertCommand.Parameters.AddWithValue("@Id", server.ServerId);
            insertCommand.Parameters.AddWithValue("@Url", server.ServerURL);
            insertCommand.Parameters.AddWithValue("@FlightId", id);
            try
            {
                insertCommand.ExecuteReader();
            }
            catch { }
            CloseConncetion();

        }


        // Read a single row from the data base.
        private object[] ReadFromTableSingleRow(string commendText)
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
        // Read multiple rows from the data base.
        private List<object[]> ReadMultipleLines(string commendText)
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
            }
            query.Close();
            return rows;

        }

        // Check if a Flight started already.
        private bool NotStarted(object[] initialLocation, DateTime requiredTime)
        {
            DateTime flightPlanDateTime = 
                Convert.ToDateTime(initialLocation[initalLocationDateTimeE]);
            int result = DateTime.Compare(requiredTime, flightPlanDateTime);

            // requiredTime is earlier than flightPlanDateTime
            if (result < 0)
            {
                return true;
            }
            return false;
        }
        // Check if a Flight is already finish flying.
        private bool AlreadyFinished(int timeOfAllFlight, DateTime startTime,
            DateTime requiredTime)
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

        // Create a Flights object.
        private Flights CreateFlight(object[] initialLocation, List<Object[]> segements,
            bool isExternal, DateTime time)
        {
            Flights flight = flightGenerator.CreateFlightFromGivenData(initialLocation, segements,
                isExternal, time);
            object[] flightPlan =  ReadFromTableSingleRow("SELECT * FROM FlightPlanTable " +
                "WHERE Id = '" + flight.FlightId + "'");
            flight.CompanyName = Convert.ToString(flightPlan[flightPlanCompanyNameE]);
            flight.Passengers = Convert.ToInt32(flightPlan[flightPlanPassangersE]);
            return flight;
        }

        // Get all internal Flights from the data base.
        public List<Flights> GetFlights(DateTime time)
        {

            List<Flights> flights = new List<Flights>();
            OpenConnection();
            // Get all Flights's initialLocation.
            SqliteDataReader query = GetAllInitialLocationsQuery();

            // For each location.
            while (query.Read())
            {
                // Checks if the Flight of the current location is currently flying.
                Flights flight1 = LocationIterationCurrentFlights(time, query);
                if (flight1 != null)
                {
                    flights.Add(flight1);

                }
            }
            query.Close();
            CloseConncetion();
            return flights;
        }

        // Read from the data bade all initial locations.
        private SqliteDataReader GetAllInitialLocationsQuery()
        {
            SqliteCommand selectCommand = new SqliteCommand();
            selectCommand.Connection = conn;
            selectCommand.CommandText = "SELECT * FROM InitialLocationTable";
            SqliteDataReader query = selectCommand.ExecuteReader();
            return query;
        }

        // Cheack if a flight is currently active.
        private Flights LocationIterationCurrentFlights(DateTime time, SqliteDataReader query)
        {
            object[] location = new object[query.FieldCount];
            query.GetValues(location);
            // InitialTime is in the future.
            if (NotStarted(location, time))
            {
                return null;
            }

            // Get the sum of all the timespanSecond of the Flight's segments.
            string command = "SELECT SUM(TimespanSeconds) FROM" +
                " SegmentsTable WHERE FlightId= '" + location[initialLocationIdE].ToString() + "'";
            object[] sumTime = ReadFromTableSingleRow(command);

            // EndTime is in the past.
            if (AlreadyFinished(Convert.ToInt32(sumTime[0]),
                Convert.ToDateTime(location[initalLocationDateTimeE]), time))
            {
                return null;
            }

            // If we got here, this flight is currently flying - we need to create
            //and add it to the flying flights.
            command = "SELECT * FROM SegmentsTable WHERE FlightId= '" +
                location[initialLocationIdE].ToString() + "'";
            List<Object[]> sgements = ReadMultipleLines(command);

            return CreateFlight(location, sgements, false, time);
        }

        // Create Servers Collection.
        private List<Server> CreateServers(List<object[]> tempServers)
        {
            List<Server> servers = new List<Server>();

            foreach (object[] server in tempServers.ToArray())
            {
                Server serv = new Server();
                serv.ServerId = Convert.ToString(server[0]);
                serv.ServerURL = Convert.ToString(server[1]);
                servers.Add(serv);
            }

            return servers;

        }
        // Get a Server by a given FlightId.
        public Server GetServerByIdOfFlight(string id)
        {
            OpenConnection();
            object[] tempServer = ReadFromTableSingleRow("SELECT * FROM ExternalFlightsTable" +
                " WHERE FlightId = '" + id + "'");

            CloseConncetion();

             Server server = new Server();
            if (tempServer[0] == null)
            {
                return null;
            }
            server.ServerId = Convert.ToString(tempServer[0]);
            server.ServerURL = Convert.ToString(tempServer[1]);
            
            return server;
        }

        // Get all servers from the data base
        public List<Server> GetServers()
        {
            OpenConnection();
            List<Server> servers = new List<Server>();
            List<object[]> tempServers = ReadMultipleLines("SELECT * FROM ServersTable");
            CloseConncetion();
            servers = CreateServers(tempServers);

            return servers;

        }


        // Get a FlightPlan by a given id.
        public FlightPlan GetFlightPlan(string id)
        {
            OpenConnection();
            object[] basicData =  ReadFromTableSingleRow("SELECT * FROM FlightPlanTable" +
                " WHERE Id = '" + id + "'");
            object[] initialLocation =  ReadFromTableSingleRow("SELECT * FROM " +
                "InitialLocationTable WHERE Id= '" + id + "'");
            List<object[]> segments = ReadMultipleLines("SELECT * FROM SegmentsTable  WHERE" +
                " FlightId= '" + id + "' ORDER BY Place ASC");
            CloseConncetion();
            return SetFlightPlan(basicData, initialLocation, segments);
        }

        // Insert the given Server into the ServersTable.
        public void InsertServer(Server server)
        {
            OpenConnection();
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

            CloseConncetion();

        }
        // Insert the given FlightPlan into the FlightPlanTable.
        private void InsertToFlightPlanTable(FlightPlan flightPlan, string id)
        {
            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = conn;

            insertCommand.CommandText = "INSERT INTO FlightPlanTable VALUES" +
                " (@Id , @Passengers, @CompanyName)";
            insertCommand.Parameters.AddWithValue("@Id", id);
            insertCommand.Parameters.AddWithValue("@Passengers", flightPlan.Passengers);
            insertCommand.Parameters.AddWithValue("@CompanyName", flightPlan.CompanyName);

            try
            {
                insertCommand.ExecuteReader();
            }
            catch { }
        }
        // Insert the given FlightPlan's InitialLocation into the InitialLocationTable.
        private void InsertToInitialLocationTable(FlightPlan flightPlan, string id)
        {
            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = conn;

            insertCommand.CommandText = "INSERT INTO InitialLocationTable VALUES" +
                " (@Id , @Longitude , @Latitude , @DateTime)";
            insertCommand.Parameters.AddWithValue("@Id", id);
            insertCommand.Parameters.AddWithValue("@Longitude",
                flightPlan.InitialLocation.Longitude);
            insertCommand.Parameters.AddWithValue("@Latitude",
                flightPlan.InitialLocation.Latitude);
            insertCommand.Parameters.AddWithValue("@DateTime",
                flightPlan.InitialLocation.DateTime);

            try
            {
                insertCommand.ExecuteReader();
            }
            catch { }
        }
        // Insert the given FlightPlan's Segments into the SegmentsTable.
        private void InsertToSegmentsTable(FlightPlan flightPlan, string id)
        {
            int i, size = flightPlan.Segments.Count;
            for (i = 0; i < size; i++)
            {
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = conn;
                insertCommand.CommandText = "INSERT INTO SegmentsTable VALUES" +
                    " (NULL, @FlightId, @Place, @Longitude , @Latitude, @TimespanSeconds)";
                insertCommand.Parameters.AddWithValue("@FlightId", id);
                insertCommand.Parameters.AddWithValue("@Place", i + 1);
                insertCommand.Parameters.AddWithValue("@Longitude", flightPlan.Segments[i].Longitude);
                insertCommand.Parameters.AddWithValue("@Latitude", flightPlan.Segments[i].Latitude);
                insertCommand.Parameters.AddWithValue("@TimespanSeconds",
                    flightPlan.Segments[i].TimespanSeconds);
                try
                {
                    insertCommand.ExecuteReader();
                }
                catch { }
            }
        }
        // Insert a FlightPlan into the data base.
        public void InsertFlightPlan(FlightPlan flightPlan, string id)
        {
            OpenConnection();
            InsertToFlightPlanTable(flightPlan, id);
            InsertToInitialLocationTable(flightPlan, id);
            InsertToSegmentsTable(flightPlan, id);
            CloseConncetion();

        }
        // Delete a Server with the given id.
        public bool DeleteServer(string id)
        {
            if (!IsExist("ServersTable", "Id", id))
            {
                return false;
            }
            OpenConnection();
            SqliteCommand deleteCommand = new SqliteCommand();
            deleteCommand.Connection = conn;
            deleteCommand.CommandText = "DELETE FROM ServersTable WHERE Id='" + id + "'";
            deleteCommand.ExecuteReader();
            CloseConncetion();

            if (IsExist("ExternalFlightsTable", "Id", id))
            {
                OpenConnection();
                SqliteCommand deleteOtherCommand = new SqliteCommand();
                deleteOtherCommand.Connection = conn;
                deleteOtherCommand.CommandText = "DELETE FROM ExternalFlightsTable WHERE Id='" + id + "'";
                deleteOtherCommand.ExecuteReader();
                CloseConncetion();
            }

           
            return true;
        }

        // Check if a given id is in the given tableNam in the given isColumn.
        private bool IsExist(string tableName, string idColumn, string id)
        {
            OpenConnection();
            object[] returnValue = ReadFromTableSingleRow("SELECT * FROM " + tableName +
                " WHERE " + idColumn + " = '" + id + "'");
            CloseConncetion();

            // id was not found.
            if (returnValue[0] == null)
            {
                return false;
            }
            return true;
        }

        // Delete a FlightPlan with the given id.
        public bool DeleteFlightPlan(string id)
        {
            int i = 0;
            bool returnVal = true;

            if (!IsExist("FlightPlanTable", "Id", id))
            {
                return false;
            }
            OpenConnection();
            string[] tables = { "FlightPlanTable", "InitialLocationTable", "SegmentsTable" };

            for (i = 0; i < tables.Length; i++)
            {
                SqliteCommand deleteCommand = new SqliteCommand();
                deleteCommand.Connection = conn;
                deleteCommand.CommandText = "DELETE FROM " + tables[i] + " WHERE Id='" + id + "'";
                deleteCommand.ExecuteReader();
            }
            CloseConncetion();
            return returnVal;
        }

        // Create a FlightPlan object.
        private FlightPlan SetFlightPlan(object[] basicData, object[] initialLocation,
            List<object[]> segments)
        {
            FlightPlan flightPlan = new FlightPlan();
            if (basicData[0] == null)
            {
                return null;
            }

            flightPlan.Passengers = Convert.ToInt32(basicData[flightPlanPassangersE]);
            flightPlan.CompanyName = Convert.ToString(basicData[flightPlanCompanyNameE]);

            double longitude = Convert.ToDouble(initialLocation[initialLocationLongitudeE]);
            double latitude = Convert.ToDouble(initialLocation[initialLocationLatitudeE]);
            DateTime dateTime = Convert.ToDateTime(initialLocation[initalLocationDateTimeE]);
            Location location = new Location(longitude, latitude, dateTime);
            flightPlan.InitialLocation = location;

            foreach (object[] segment in segments.ToArray())
            {
                double longitudeS = Convert.ToDouble(segment[segmentLongitudeE]);
                double latitudeS = Convert.ToDouble(segment[segmentLatitudeE]);
                int TimespanSecond = Convert.ToInt32(segment[segmenTimespanSecondE]);
                Segment segment1 = new Segment(longitudeS, latitudeS, TimespanSecond);
                flightPlan.Segments.Add(segment1);
            }
            return flightPlan;
        }

        // Create a table.
        public static void CreateTable(string nameOfTable, string columns)
        {
            string tableCommand = @"CREATE TABLE IF NOT EXISTS " + nameOfTable + columns;

            SqliteCommand createTable = new SqliteCommand(tableCommand, conn);
            createTable.ExecuteReader();
        }

        // Delete all the tables if exists.
        public static void ResetTables ()
        {
            try
            {
                string com = "DROP Table ExternalFlightsTable";
                SqliteCommand delete = new SqliteCommand(com, conn);
                delete.ExecuteReader();
                com = "DROP Table ServersTable";
                delete = new SqliteCommand(com, conn);
                delete.ExecuteReader();
                com = "DROP Table FlightPlanTable";
                delete = new SqliteCommand(com, conn);
                delete.ExecuteReader();
                com = "DROP Table InitialLocationTable";
                delete = new SqliteCommand(com, conn);
                delete.ExecuteReader();
                com = "DROP Table SegmentsTable";
                delete = new SqliteCommand(com, conn);
                delete.ExecuteReader();
            // Tables does not exists.
            } catch(Exception e)
            {
                string message = e.Message;
            }

        }
        // Open the sqlite connection.
        private void OpenConnection()
        {
            mutex.WaitOne();
            conn.Open();
        }
        // Close the sqlite connection.
        private void CloseConncetion()
        {
            conn.Close();
            mutex.ReleaseMutex();
        }

        // Initialize the data base connection and tables.
        public static void InitializeDatabase()
        {
            string dbPath = AppDomain.CurrentDomain.BaseDirectory + @"\Database.sqlite";
            conn = new SqliteConnection(@"Data Source = " + dbPath);
            mutex = new Mutex();
            mutex.WaitOne();
            conn.Open();

            ResetTables();

            CreateTable("ServersTable", "(Id TEXT PRIMARY KEY, Url TEXT)");
            CreateTable("SegmentsTable", "(Id TEXT PRIMARY KEY, FlightId TEXT," +
                " Place INTEGER, Longitude REAL, Latitude REAL, TimespanSeconds INTEGER)");

            CreateTable("InitialLocationTable",
                "(Id TEXT PRIMARY KEY, Longitude REAL, Latitude REAL, DateTime TEXT)");
            CreateTable("FlightPlanTable",
                "(Id TEXT PRIMARY KEY, Passengers INTEGER, CompanyName TEXT)");
            CreateTable("ExternalFlightsTable", "(Id TEXT, Url TEXT, FlightId TEXT PRIMARY KEY)");
            conn.Close();
            mutex.ReleaseMutex();
        }
    }
}
