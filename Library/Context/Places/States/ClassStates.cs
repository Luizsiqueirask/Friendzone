using Library.Models.Places;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Library.Context.Places.States
{
    public class ClassStates : ThrowStates
    {
        private readonly Bridge _conn;
        private readonly SqlConnection _sqlConnection;

        public ClassStates()
        {
            _conn = new Bridge();
            _sqlConnection = new SqlConnection(_conn.Connect());
        }

        public new IEnumerable<StateDomain> List()
        {
            var allStates = new List<StateDomain>();

            try
            {
                using (SqlCommand command = new SqlCommand("ListStates", _sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    _sqlConnection.Open();
                    SqlDataReader dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        if (dataReader.HasRows)
                        {
                            var flagStates = new FlagDomain()
                            {
                                Id = (int)dataReader["Id"],
                                Symbol = dataReader["Symbol"].ToString(),
                                Path = dataReader["Path"].ToString()
                            };
                            var stateDomain = new StateDomain()
                            {
                                Id = (int)dataReader["Id"],
                                Label = dataReader["Label"].ToString(),
                                Flag = flagStates,
                                CountryId = (int)dataReader["CountryId"]
                            };

                            allStates.Add(stateDomain);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception msg)
            {
                Console.WriteLine("Exception: " + msg.Message);
            }
            finally
            {
                _sqlConnection.Close();
            }

            return allStates;
        }
        public new StateDomain Get(int? Id)
        {
            var stateDomain = new StateDomain();

            using (SqlCommand command = new SqlCommand("GetStates", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdStates", Id);
                _sqlConnection.Open();

                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    stateDomain = new StateDomain()
                    {
                        Id = (int)dataReader["Id"],
                        Label = dataReader["Label"].ToString(),
                        Flag = new FlagDomain()
                        {
                            Id = (int)dataReader["Id"],
                            Symbol = dataReader["Symbol"].ToString(),
                            Path = dataReader["Path"].ToString()
                        },
                        CountryId = (int)dataReader["CountryId"]
                    };
                }
            }
            _sqlConnection.Close();
            return stateDomain;
        }
        public new void Post(StateDomain stateDomain)
        {
            using (SqlCommand command = new SqlCommand("PostStates", _sqlConnection))
            {
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    // -- States
                    command.Parameters.AddWithValue("@Label", stateDomain.Label);
                    // -- Flag
                    command.Parameters.AddWithValue("@FlagId", stateDomain.Flag.Id);
                    command.Parameters.AddWithValue("@Symbol", stateDomain.Flag.Symbol);
                    command.Parameters.AddWithValue("@Path", stateDomain.Flag.Path);
                    // -- Country
                    command.Parameters.AddWithValue("@CountryId", stateDomain.CountryId);

                    var running = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQLException: " + ex.Message);
                }
                finally
                {
                    _sqlConnection.Close();
                }
            }
        }
        public new void Put(StateDomain stateDomain, int? Id)
        {
            using (SqlCommand command = new SqlCommand("PutStates", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                // -- States
                command.Parameters.AddWithValue("@IdStates", Id);
                command.Parameters.AddWithValue("@Label", stateDomain.Label);
                // -- Flag
                command.Parameters.AddWithValue("@Symbol", stateDomain.Flag.Symbol);
                command.Parameters.AddWithValue("@Path", stateDomain.Flag.Path);
                // -- Country
                command.Parameters.AddWithValue("@CountryId", stateDomain.CountryId);

                _sqlConnection.Open();
                var running = command.ExecuteNonQuery();
            }
            _sqlConnection.Close();
        }
        public new void Delete(int? Id)
        {
            using (SqlCommand command = new SqlCommand("DeleteStates", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdStates", Id);

                _sqlConnection.Open();
                var running = command.ExecuteNonQuery();
            }
            _sqlConnection.Close();
        }
    }
}
