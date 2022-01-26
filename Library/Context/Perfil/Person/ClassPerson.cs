using Library.Models.Perfil;
using Library.Models.Places;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Library.Context.Perfil.Person
{
    public class ClassPerson : ThrowPerson
    {
        public readonly Bridge _conn;
        public readonly SqlConnection _sqlConnection;

        public ClassPerson()
        {
            _conn = new Bridge();
            _sqlConnection = new SqlConnection(_conn.Connect());
        }

        public new IEnumerable<PersonDomain> List()
        {
            var allPerson = new List<PersonDomain>();

            try
            {
                using (SqlCommand command = new SqlCommand("ListPerson", _sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    _sqlConnection.Open();

                    SqlDataReader dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        if (dataReader.HasRows)
                        {   
                            var personDomain = new PersonDomain()
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
                                CountryId = (int)dataReader["CountryId"],
                            };

                            allPerson.Add(personDomain);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                _sqlConnection.Close();
            }

            return allPerson;
        }
        public new PersonDomain Get(int? Id)
        {
            var personDomain = new PersonDomain();

            using (SqlCommand command = new SqlCommand("GetPerson", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdPerson", Id);
                _sqlConnection.Open();

                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    personDomain = new PersonDomain()
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
            return personDomain;
        }
        public new void Post(PersonDomain personDomain)
        {
            using (SqlCommand command = new SqlCommand("PostPerson", _sqlConnection))
            {
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    // -- Person
                    command.Parameters.AddWithValue("@FirstName", personDomain.FirstName);
                    command.Parameters.AddWithValue("@LastName", personDomain.LastName);
                    command.Parameters.AddWithValue("@Age", personDomain.Age);
                    command.Parameters.AddWithValue("@Birthday", personDomain.Birthday);
                    // -- Picture
                    command.Parameters.AddWithValue("@Symbol", personDomain.Picture.Symbol);
                    command.Parameters.AddWithValue("@Path", personDomain.Picture.Path);
                    // -- Contacts
                    command.Parameters.AddWithValue("@Email", personDomain.Contacts.Email);
                    command.Parameters.AddWithValue("@Mobile", personDomain.Contacts.Mobile);
                    // -- Country
                    command.Parameters.AddWithValue("@CountryId", personDomain.CountryId);

                    int running = command.ExecuteNonQuery();
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
        public new void Put(PersonDomain personDomain, int? Id)
        {
            using (SqlCommand command = new SqlCommand("PutPerson", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                // -- Person
                command.Parameters.AddWithValue("@IdPerson", Id);
                command.Parameters.AddWithValue("@FirstName", personDomain.FirstName);
                command.Parameters.AddWithValue("@LastName", personDomain.LastName);
                command.Parameters.AddWithValue("@Age", personDomain.Age);
                command.Parameters.AddWithValue("@Birthday", personDomain.Birthday);
                // -- Picture
                command.Parameters.AddWithValue("@Symbol", personDomain.Picture.Symbol);
                command.Parameters.AddWithValue("@Path", personDomain.Picture.Path);
                // -- Contacts
                command.Parameters.AddWithValue("@Email", personDomain.Contacts.Email);
                command.Parameters.AddWithValue("@Mobile", personDomain.Contacts.Mobile);
                // -- Country
                command.Parameters.AddWithValue("@CountryId", personDomain.CountryId);

                _sqlConnection.Open();
                var running = command.ExecuteNonQuery();
            }
            _sqlConnection.Close();
        }
        public new void Delete(int? Id)
        {
            using (SqlCommand command = new SqlCommand("DeletePerson", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdPerson", Id);

                _sqlConnection.Open();
                var running = command.ExecuteNonQuery();
            }
            _sqlConnection.Close();
        }
    }
}
