using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using WebApplication1.Utiles;

namespace OnlineShopAPI.Models
{
    public static class Carts
    {
        public static List<Cart> getAllCarts()
        {
            List<Cart> ReturnList = new List<Cart>();

            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "select * from carts";

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
                        ReturnList.Add(new Cart
                        {
                            Cart_id = (ulong)OneRegistry["cart_id"],
                            User_id = (ulong)OneRegistry["user_id"],
                            Creation_date = (DateTime)OneRegistry["creation_date"],
                            Purchased = (bool)OneRegistry["purchased"],
                        });
                    }
                }
            }

            return ReturnList;

        }

        public static Cart getCartById(ulong SearchId)
        {
            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "select * from carts where cart_id=@Id";
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
                        ulong User_id = (ulong)MyTable.Rows[0]["user_id"];
                        DateTime Creation_date = (DateTime)MyTable.Rows[0]["creation_date"];
                        bool Purchased = (bool)MyTable.Rows[0]["purchased"];

                        return new Cart { Cart_id = SearchId, User_id = User_id, Creation_date = Creation_date, Purchased = Purchased };
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

        public static Cart createNewCart(Cart NewCart, out string Errors)
        {
            Errors = "";
            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "insert into carts (user_id,creation_date,purchased) values (@UserId,@CreationDate,@Purchased)";
                MyCommand.Parameters.AddWithValue("@UserId", NewCart.User_id);
                MyCommand.Parameters.AddWithValue("@CreationDate", NewCart.Creation_date);
                MyCommand.Parameters.AddWithValue("@Purchased", NewCart.Purchased);

                try
                {
                    MyCommand.ExecuteNonQuery();
                    MyCommand.CommandText = "select max(cart_id) as new_id from carts";
                    NewCart.Cart_id = (ulong)MyCommand.ExecuteScalar();
                }
                catch (MySqlException error)
                {
                    if (error.Number == 1062)
                    {
                        //Registro duplicado
                        Errors = "Registro duplicado";
                    }
                    else
                    {
                        Errors = error.Message;
                    }
                    NewCart = null;
                }
                catch (SystemException error)
                {
                    Errors = error.Message;
                    NewCart = null;
                }
            }

            return NewCart;

        }

        public static bool deleteCartById(ulong DeleteId, out string Errors)
        {
            Errors = "";
            bool Success = false;
            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "delete from carts where cart_id=@Id";
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

        public static Cart patchCartById(ulong SearchId, Cart UpdateCart, out string Errors)
        {
            Errors = "";

            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "update carts set purchased=@Purchased where cart_id=@Id";
                MyCommand.Parameters.AddWithValue("@Purchased", UpdateCart.Purchased);
                MyCommand.Parameters.AddWithValue("@Id", SearchId);

                try
                {
                    MyCommand.ExecuteNonQuery();
                    UpdateCart.Cart_id = SearchId;
                }
                catch (MySqlException error)
                {
                    Errors = error.Message;
                    UpdateCart = null;
                }
                catch (SystemException error)
                {
                    Errors = error.Message;
                    UpdateCart = null;
                }
            }

            return UpdateCart;

        }

    }

    /// <summary>
    /// Shopping carts for the users
    /// </summary>
    public class Cart
    {
        public ulong Cart_id { get; set; }
        public ulong User_id { get; set; }
        public DateTime Creation_date { get; set; }
        public bool Purchased { get; set; }
    }

}
