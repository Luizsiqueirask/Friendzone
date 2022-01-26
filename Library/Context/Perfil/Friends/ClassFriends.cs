using Library.Models.Perfil;
using Library.Models.Places;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Library.Context.Perfil.Friends
{
    public class ClassFriends : ThrowPerson
    {
        private readonly Bridge _conn;
        private readonly SqlConnection _sqlConnection;

        public ClassFriends()
        {
            _conn = new Bridge();
            _sqlConnection = new SqlConnection(_conn.Connect());
        }

        public new IEnumerable<FriendsDomain> List()
        {
            var allFriends = new List<FriendsDomain>();

            try
            {
                using (SqlCommand command = new SqlCommand("ListFriends", _sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    _sqlConnection.Open();

                    SqlDataReader dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        if (dataReader.HasRows)
                        {
                            var friendDomain = new FriendsDomain()
                            {
                                Id = (int)dataReader["Id"],
                                FirstName = dataReader["FirstName"].ToString(),
                                LastName = dataReader["LastName"].ToString(),
                                Age = (int)dataReader["Age"],
                                Birthday = (DateTime)dataReader["Birthday"],
                                Picture = new PicturesDomain()
                                {
                                    Id = (int)dataReader["Id"],
                                    Symbol = dataReader["Symbol"].ToString(),
                                    Path = dataReader["Path"].ToString()
                                },
                                Contacts = new ContactsDomain()
                                {
                                    Id = (int)dataReader["Id"],
                                    Email = dataReader["Email"].ToString(),
                                    Mobile = dataReader["Mobile"].ToString()
                                },
                                CountryId = (int)dataReader["CountryId"]
                            };

                            allFriends.Add(friendDomain);
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

            return allFriends;
        }
        public new FriendsDomain Get(int? Id)
        {
            var friendDomain = new FriendsDomain();

            using (SqlCommand command = new SqlCommand("GetFriends", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdFriends", Id);
                _sqlConnection.Open();

                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    friendDomain = new FriendsDomain()
                    {
                        Id = (int)dataReader["Id"],
                        FirstName = dataReader["FirstName"].ToString(),
                        LastName = dataReader["LastName"].ToString(),
                        Age = (int)dataReader["Age"],
                        Birthday = (DateTime)dataReader["Birthday"],
                        Picture = new PicturesDomain()
                        {
                            Id = (int)dataReader["Id"],
                            Symbol = dataReader["Symbol"].ToString(),
                            Path = dataReader["Path"].ToString()
                        },
                        Contacts = new ContactsDomain()
                        {
                            Id = (int)dataReader["Id"],
                            Email = dataReader["Email"].ToString(),
                            Mobile = dataReader["Mobile"].ToString()
                        },
                        CountryId = (int)dataReader["CountryId"]
                    };
                }
            }
            _sqlConnection.Close();
            return friendDomain;
        }
        public new void Post(FriendsDomain friendDomain)
        {
            using (SqlCommand command = new SqlCommand("PostFriends", _sqlConnection))
            {
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    // -- Friends
                    command.Parameters.AddWithValue("@FirstName", friendDomain.FirstName);
                    command.Parameters.AddWithValue("@LastName", friendDomain.LastName);
                    command.Parameters.AddWithValue("@Age", friendDomain.Age);
                    command.Parameters.AddWithValue("@Birthday", friendDomain.Birthday);
                    // -- Picture
                    command.Parameters.AddWithValue("@Symbol", friendDomain.Picture.Symbol);
                    command.Parameters.AddWithValue("@Path", friendDomain.Picture.Path);
                    // -- Contacts
                    command.Parameters.AddWithValue("@Email", friendDomain.Contacts.Email);
                    command.Parameters.AddWithValue("@Mobile", friendDomain.Contacts.Mobile);
                    // -- Country
                    command.Parameters.AddWithValue("@CountryId", friendDomain.CountryId);

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
        public new void Put(FriendsDomain friendDomain, int? Id)
        {
            using (SqlCommand command = new SqlCommand("PutFriends", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdFriends", Id);
                // -- Friends
                command.Parameters.AddWithValue("@FirstName", friendDomain.FirstName);
                command.Parameters.AddWithValue("@LastName", friendDomain.LastName);
                command.Parameters.AddWithValue("@Age", friendDomain.Age);
                command.Parameters.AddWithValue("@Birthday", friendDomain.Birthday);
                // -- Picture
                command.Parameters.AddWithValue("@Symbol", friendDomain.Picture.Symbol);
                command.Parameters.AddWithValue("@Path", friendDomain.Picture.Path);
                // -- Contacts
                command.Parameters.AddWithValue("@Email", friendDomain.Contacts.Email);
                command.Parameters.AddWithValue("@Mobile", friendDomain.Contacts.Mobile);
                // -- Country
                command.Parameters.AddWithValue("@CountryId", friendDomain.CountryId);

                _sqlConnection.Open();
                var running = command.ExecuteNonQuery();
            }
            _sqlConnection.Close();
        }
        public new void Delete(int? Id)
        {
            using (SqlCommand command = new SqlCommand("DeleteFriends", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdFriends", Id);

                _sqlConnection.Open();
                var running = command.ExecuteNonQuery();
            }
            _sqlConnection.Close();
        }
    }
}
