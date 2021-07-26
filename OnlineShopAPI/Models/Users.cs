using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using WebApplication1.Utiles;

namespace OnlineShopAPI.Models
{
    public static class Users
    {
        public static User userLogin(string Email, string Password)
        {
            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.StoredProcedure;
                MyCommand.CommandText = "sp_login";
                MyCommand.Parameters.AddWithValue("@in_email", Email);
                MyCommand.Parameters.AddWithValue("@in_password", Password);

                DataTable MyTable = new DataTable();
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand;

                bool Success = true;
                try
                {
                    MyAdapter.Fill(MyTable);
                }
                catch (SystemException error)
                {
                    Console.WriteLine("Error de lectura en la base de datos: " + error.Message);
                    Success = false;
                }

                if (Success)
                {
                    if (MyTable.Rows.Count == 1)
                    {
                        ulong User_id = (ulong)MyTable.Rows[0]["user_id"];
                        string First_name = (string)MyTable.Rows[0]["first_name"];
                        string Last_name = (string)MyTable.Rows[0]["last_name"];

                        return new User { User_id = User_id, First_name = First_name, Last_name = Last_name };
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static User userRegistration(User NewUser, out string Errors)
        {
            Errors = "";
            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "insert into users (first_name,last_name,email,password) values (@First_name,@Last_name,@Email,@Password)";
                MyCommand.Parameters.AddWithValue("@First_name", NewUser.First_name);
                MyCommand.Parameters.AddWithValue("@Last_name", NewUser.Last_name);
                MyCommand.Parameters.AddWithValue("@Email", NewUser.Email);
                MyCommand.Parameters.AddWithValue("@Password", NewUser.Password);

                try
                {
                    MyCommand.ExecuteNonQuery();
                    MyCommand.CommandText = "select max(user_id) as new_id from users";
                    NewUser.User_id = (ulong)MyCommand.ExecuteScalar();
                }
                catch (MySqlException error)
                {
                    if (error.Number == 1062)
                    {
                        //Registro duplicado
                        Errors = "Ya existe un usuario registrado con ese e-mail";
                    }
                    else
                    {
                        Errors = error.Message;
                    }
                    NewUser = null;
                }
                catch (SystemException error)
                {
                    Errors = error.Message;
                    NewUser = null;
                }
            }

            return NewUser;

        }

        public static List<User> getAllUsers()
        {
            List<User> ReturnList = new List<User>();

            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "select * from users";

                DataTable MyTable = new DataTable();
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand;

                bool Success = true;
                try
                {
                    MyAdapter.Fill(MyTable);
                }
                catch (SystemException error)
                {
                    Console.WriteLine("Error de lectura en la base de datos: " + error.Message);
                    Success = false;
                }

                if (Success)
                {
                    foreach (DataRow OneRegistry in MyTable.Rows)
                    {
                        ReturnList.Add(new User
                        {
                            User_id = (ulong)OneRegistry["user_id"],
                            First_name = (string)OneRegistry["first_name"],
                            Last_name = (string)OneRegistry["last_name"],
                            Email = (string)OneRegistry["email"],
                        });
                    }
                }
            }

            return ReturnList;

        }

        public static User getUserById(ulong SearchId)
        {
            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "select * from users where user_id=@Id";
                MyCommand.Parameters.AddWithValue("@Id", SearchId);

                DataTable MyTable = new DataTable();
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand;

                bool Success = true;
                try
                {
                    MyAdapter.Fill(MyTable);
                }
                catch (SystemException error)
                {
                    Console.WriteLine("Error de lectura en la base de datos: " + error.Message);
                    Success = false;
                }

                if (Success)
                {
                    if (MyTable.Rows.Count == 1)
                    {
                        string First_name = (string)MyTable.Rows[0]["first_name"];
                        string Last_name = (string)MyTable.Rows[0]["last_name"];
                        string Email = (string)MyTable.Rows[0]["email"];

                        return new User { User_id = SearchId, First_name = First_name, Last_name = Last_name, Email= Email };
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static bool deleteUserById(ulong DeleteId, out string Errors)
        {
            Errors = "";
            bool Success = false;
            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "delete from users where user_id=@Id";
                MyCommand.Parameters.AddWithValue("@Id", DeleteId);

                try
                {
                    MyCommand.ExecuteNonQuery();
                    Success = true;
                }
                catch (MySqlException error)
                {
                    Errors = error.Message;
                }
                catch (SystemException error)
                {
                    Errors = error.Message;
                }
            }

            return Success;
        }

    }

    public class User
    {
        public ulong User_id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }

}