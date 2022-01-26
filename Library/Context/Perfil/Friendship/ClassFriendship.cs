using Library.Models.Perfil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Library.Context.Perfil.Friends
{
    public class ClassFriendship : ThrowFriendship
    {
        private readonly Bridge _conn;
        private readonly SqlConnection _sqlConnection;

        public ClassFriendship()
        {
            _conn = new Bridge();
            _sqlConnection = new SqlConnection(_conn.Connect());
        }

        public new IEnumerable<FriendshipDomain> List()
        {
            var allFriendship = new List<FriendshipDomain>();
            //var allFriend = new List<FriendDomain>();

            try
            {
                using (SqlCommand command = new SqlCommand("ListFriendship", _sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    _sqlConnection.Open();
                    SqlDataReader dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        if (dataReader.HasRows)
                        {
                            var friendshipDomain = new FriendshipDomain()
                            {
                                PersonId = (int)dataReader["PersonId"],
                                FriendsId = (int)dataReader["FriendsId"],
                            };

                            allFriendship.Add(friendshipDomain);
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
            return allFriendship;
        }
        public new FriendshipDomain Get(int? Id)
        {
            var allFriendship = new List<FriendshipDomain>();
            FriendshipDomain friendshipDomain = new FriendshipDomain();

            using (SqlCommand command = new SqlCommand("GetFriendship", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdPerson", friendshipDomain.PersonId).Equals(Id);
                _sqlConnection.Open();
                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    friendshipDomain = new FriendshipDomain()
                    {
                        PersonId = (int)dataReader["PersonId"],
                        FriendsId = (int)dataReader["FriendsId"]
                    };

                    allFriendship.Add(friendshipDomain);
                }
            }

            return friendshipDomain;
        }
        public new void Post(FriendshipDomain friendshipDomain)
        {
            using (SqlCommand command = new SqlCommand("PostFriendship", _sqlConnection))
            {
                try
                {
                    var friend = new FriendsDomain();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonId", friendshipDomain.PersonId);
                    command.Parameters.AddWithValue("@FriendsId", friendshipDomain.FriendsId);

                    _sqlConnection.Open();
                    var running = command.ExecuteNonQuery();
                }
                finally
                {
                    _sqlConnection.Close();
                }
            }

        }
        public new void Put(FriendshipDomain friendshipDomain, int? Id)
        {
            using (SqlCommand command = new SqlCommand("PutFriendship", _sqlConnection))
            {
                var friend = new FriendsDomain();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PersonId", friendshipDomain.PersonId.Equals(Id));
                command.Parameters.AddWithValue("@FriendsId", friendshipDomain.FriendsId);

                _sqlConnection.Open();
                var running = command.ExecuteNonQuery();
            }
            _sqlConnection.Close();
        }
        public new void Delete(int? Id)
        {
            var friendshipDomain = new FriendshipDomain();

            using (SqlCommand command = new SqlCommand("DeleteFriendship", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdPerson", friendshipDomain.PersonId.Equals(Id));

                _sqlConnection.Open();
                var running = command.ExecuteNonQuery();
            }
            _sqlConnection.Close();
        }
    }
}
