using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Microsoft.Data.Sqlite;


namespace xApplication
{
    public class Database : IDisposable
    {
        protected SqliteConnection _SQLiteConnection;

        public Database()
        {
            string connectionString = @"DataSource=.\Database\xApplicationDatabase.db;";
            
            _SQLiteConnection = new SqliteConnection(connectionString);
        }

        public List<Dictionary<string, object>> Query(string sql, SqliteParameter[] parameters = null)
        {

            if (String.IsNullOrEmpty(sql))
            {
                throw new Exception("SQL-String cannot be null!");
            }

            try
            {
                SqliteCommand command = new SqliteCommand(sql, _SQLiteConnection);

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                EnsureConnectionOpen();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

                        while (reader.Read())
                        {
                            Dictionary<string, object> d = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                d.Add(reader.GetName(i).ToUpper(), reader.GetValue(i));
                            }

                            result.Add(d);
                        }

                        EnsureConnectionClosed();

                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int NonQuery(string sql, SqliteParameter[] parameters = null)
        {

            if (String.IsNullOrEmpty(sql))
            {
                throw new Exception("SQL-String cannot be null!");
            }

            try
            {
                SqliteCommand command = new SqliteCommand(sql, _SQLiteConnection);

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                EnsureConnectionOpen();

                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public void EnsureConnectionOpen()
        {
            try
            {
                _SQLiteConnection.Open();
            }
            catch (Exception e)
            {

            }
        }

        public void EnsureConnectionClosed()
        {
            try
            {
                _SQLiteConnection.Close();
            }
            catch (Exception e)
            {

            }
        }

        public void Dispose()
        {
            EnsureConnectionClosed();
        }
    }
}