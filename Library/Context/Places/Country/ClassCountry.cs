using Library.Models.Places;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Library.Context.Places.Country
{
    public class ClassCountry : ThrowCountry
    {
        private readonly Bridge _conn;
        private readonly SqlConnection _sqlConnection;

        public ClassCountry()
        {
            _conn = new Bridge();
            _sqlConnection = new SqlConnection(_conn.Connect());
        }

        public new IEnumerable<CountryDomain> List()
        {
            var allCountry = new List<CountryDomain>();

            try
            {
                using (SqlCommand command = new SqlCommand("ListCountry", _sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    _sqlConnection.Open();

                    SqlDataReader dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        if (dataReader.HasRows)
                        {
                            var flagDomain = new FlagDomain()
                            {
                                Id = (int)dataReader["Id"],
                                Symbol = dataReader["Symbol"].ToString(),
                                Path = dataReader["Path"].ToString()
                            };

                            var countryDomain = new CountryDomain()
                            {
                                Id = (int)dataReader["Id"],
                                Label = dataReader["Label"].ToString(),
                                Flag = flagDomain
                            };

                            allCountry.Add(countryDomain);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                _sqlConnection.Close();
            }

            return allCountry;
        }
        public new CountryDomain Get(int? Id)
        {
            var countryDomain = new CountryDomain();

            using (SqlCommand command = new SqlCommand("GetCountry", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdCountry", Id);
                _sqlConnection.Open();

                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var flagDomain = new FlagDomain()
                    {
                        Id = (int)dataReader["Id"],
                        Symbol = dataReader["Symbol"].ToString(),
                        Path = dataReader["Path"].ToString()
                    };
                    countryDomain = new CountryDomain()
                    {
                        Id = (int)dataReader["Id"],
                        Label = dataReader["Label"].ToString(),
                        Flag = flagDomain
                    };
                }
            }
            return countryDomain;
        }
        public new void Post(CountryDomain countryDomain)
        {
            using (SqlCommand command = new SqlCommand("PostCountry", _sqlConnection))
            {
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    // -- Country
                    command.Parameters.AddWithValue("@Label", countryDomain.Label);
                    command.Parameters.AddWithValue("@FlagId", countryDomain.Flag.Id);
                    // -- Flag
                    command.Parameters.AddWithValue("@Symbol", countryDomain.Flag.Symbol);
                    command.Parameters.AddWithValue("@Path", countryDomain.Flag.Path);

                    var running = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQLException: " + ex.Message);
                    throw ex;
                }
                finally
                {
                    _sqlConnection.Close();
                }
            }
        }
        public new void Put(CountryDomain countryDomain, int? Id)
        {
            using (SqlCommand command = new SqlCommand("PutCountry", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                // -- Country
                command.Parameters.AddWithValue("@IdCountry", Id);
                command.Parameters.AddWithValue("@Label", countryDomain.Label);
                command.Parameters.AddWithValue("@FlagId", countryDomain.Flag.Id);
                // -- Flag
                command.Parameters.AddWithValue("@Symbol", countryDomain.Flag.Symbol);
                command.Parameters.AddWithValue("@Path", countryDomain.Flag.Path);

                _sqlConnection.Open();
                var running = command.ExecuteNonQuery();
            }
            _sqlConnection.Close();
        }
        public new bool Delete(int? Id)
        {
            //var countryDomain = new CountryDomain(); ;

            if (Id != null)
            {
                using (SqlCommand command = new SqlCommand("DeleteCountry", _sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdCountry", Id);

                    _sqlConnection.Open();
                    var running = command.ExecuteNonQuery();
                }
            }

            _sqlConnection.Close();
            return true;
        }
    }
}
