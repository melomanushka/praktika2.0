using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace писец_какой_то.Model
{
    internal class DataBaseModel
    {
        public static readonly string Connect = "Data Source=_MELOMANUSHKA_;Initial Catalog=Praktika;Integrated Security=True";
        public bool IsValidLogin(string login, string password)
        {
            bool validUser;

            using (var connection = new SqlConnection(Connect))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "SELECT * FROM [Users] WHERE Login=@Login AND Password=@Password";
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);

                validUser = command.ExecuteScalar() != null;
            }

            return validUser;
        }

        public UserModel GetUserByLogin(string login)
        {
            UserModel user = null;

            using (var connection = new SqlConnection(Connect))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "SELECT * FROM [Users] WHERE Login=@Login";
                command.Parameters.AddWithValue("@Login", login);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Password = reader["Password"].ToString(),
                            Login = reader["Login"].ToString(),
                            AccessLevel = Convert.ToInt32(reader["AccessLevel"])
                        };
                    }
                }
            }

            return user;
        }
        public bool LoginExists(string login)
        {
            bool exists;

            using (var connection = new SqlConnection(Connect))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "SELECT COUNT(*) FROM [Users] WHERE Login=@Login";
                command.Parameters.AddWithValue("@Login", login);

                exists = (int)command.ExecuteScalar() > 0;
            }

            return exists;
        }
        public void CreateUser(string login, string password, string firstName, string lastName)
        {
            using (var connection = new SqlConnection(Connect))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "INSERT INTO [Users] (Login, Password, FirstName, LastName, AccessLevel) " +
                                      "VALUES (@Login, @Password, @FirstName, @LastName, 2)";
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);

                command.ExecuteNonQuery();
            }
        }
        public List<UserModel> GetAllUsers()
        {
            List<UserModel> users = new List<UserModel>();

            using (var connection = new SqlConnection(Connect))
            using (var command = new SqlCommand("SELECT * FROM [Users]", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new UserModel
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Password = reader["Password"].ToString(),
                            Login = reader["Login"].ToString(),
                            AccessLevel = Convert.ToInt32(reader["AccessLevel"])
                        };
                        users.Add(user);
                    }
                }
            }

            return users;
        }
        public void Delete(string login, string firstName, string lastName)
        {
            using (var connection = new SqlConnection(Connect))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;

                command.CommandText = "DELETE FROM [Users] WHERE Login=@Login AND LastName=@LastName AND FirstName=@FirstName";
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);

                command.ExecuteNonQuery();
            }
        }
    }
}
