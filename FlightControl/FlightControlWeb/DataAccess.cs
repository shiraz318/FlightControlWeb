//This code was generated by a tool.
//Changes to this file will be lost if the code is regenerated.
// See the blog post here for help on using the generated code: http://erikej.blogspot.dk/2014/10/database-first-with-sqlite-in-universal.html
using System.Data.SQLite;
using System;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using FlightControlWeb.Models;

namespace FlightControlWeb
{
    public class SQLiteDb
    {
        string _path;
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

        public DummyFP GetDummyFP(string id)
        {

            SqliteConnection conn = new SqliteConnection(@"Data Source = " + _path);
            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = conn;
            conn.Open();
            SqliteCommand selectCommand = new SqliteCommand();
            selectCommand.Connection = conn;
            selectCommand.CommandText = "SELECT CompanyName FROM DummyFlightPlanTable WHERE Id=id";

            SqliteDataReader query = selectCommand.ExecuteReader();
            string companyName = null;
            while (query.Read()) {
                 companyName = query.GetString(0);
            }
            query.Close();
            conn.Close();
            SqliteConnection conn2 = new SqliteConnection(@"Data Source = " + _path);
            SqliteCommand insertCommand2 = new SqliteCommand();
            insertCommand.Connection = conn2;
            conn2.Open();
            SqliteCommand selectCommand2 = new SqliteCommand();
            selectCommand2.Connection = conn2;
            DummyFP dfp = new DummyFP();
            dfp.CompanyName = companyName;
            
            selectCommand2.CommandText = "SELECT Passengers FROM DummyFlightPlanTable WHERE Id=id";

            SqliteDataReader query2 = selectCommand2.ExecuteReader();
            int passanger = 0;
            while (query2.Read())
            {
                passanger = query2.GetInt32(0);
            }
            dfp.Passengers = passanger;
            dfp.Id = id;
            query2.Close();
            conn.Close();
            return dfp;
        }

            public void InsertDummyFlightPlan(DummyFP dfp)
        {
            SqliteConnection conn = new SqliteConnection(@"Data Source = " + _path);
            SqliteCommand insertCommand = new SqliteCommand();
            insertCommand.Connection = conn;
            conn.Open();

            insertCommand.CommandText = "INSERT INTO DummyFlightPlanTable VALUES (@Id, @Passenger, @ComapnyName);";
            insertCommand.Parameters.AddWithValue("@Id", dfp.Id);
            insertCommand.Parameters.AddWithValue("@Passenger", dfp.Passengers);
            insertCommand.Parameters.AddWithValue("@ComapnyName", dfp.CompanyName);

            insertCommand.ExecuteReader();
            conn.Close();
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
                selectCommand.CommandText = "SELECT Text_Entry FROM newTable WHERE price='24'";
                
                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0));
                }
                query.Close();
            }
            catch 
            {

            }
           
            conn.Close();
            return entries;

        }

        public static void InitializeDatabase()
        {

            string dbPath = Environment.CurrentDirectory + @"\Database.sqlite";
            SqliteConnection conn = new SqliteConnection(@"Data Source = " + dbPath);
            try
            {
                conn.Open();
                String tableCommand = @"CREATE TABLE IF NOT EXISTS newTable (id INTEGER PRIMARY KEY,
            Text_Entry TEXT, price INT)";

                //String tableCommand = "CREATE TABLE IF NOT " +
                //    "EXISTS TestTable (Primary_Key INTEGER PRIMARY KEY, " +
                //    "Text_Entry NVARCHAR(2048) NULL)" +
                //    "Secont_Text";

                SqliteCommand createTable = new SqliteCommand(tableCommand, conn);
                createTable.ExecuteReader();
                tableCommand = @"CREATE TABLE IF NOT EXISTS DummyFlightPlanTable (Id TEXT, Passengers INTEGER, CompanyName TEXT)";
                createTable = new SqliteCommand(tableCommand, conn);
                createTable.ExecuteReader();

                conn.Close();

            }
            catch { }
        }
    }
}