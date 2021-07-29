using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using WebApplication1.Utiles;

namespace OnlineShopAPI.Models
{
    public static class CartItems
    {
        public static List<CartItem> getAllCartItems()
        {
            List<CartItem> ReturnList = new List<CartItem>();

            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "select * from cart_items";

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
                        ReturnList.Add(new CartItem
                        {
                            Cart_item_id = (ulong)OneRegistry["cart_item_id"],
                            Cart_id = (ulong)OneRegistry["cart_id"],
                            Product_id = (ulong)OneRegistry["product_id"],
                            Quantity = (int)OneRegistry["quantity"]
                        });
                    }
                }
            }

            return ReturnList;

        }

        public static CartItem getCartItemById(ulong SearchId)
        {
            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "select * from cart_items where cart_item_id=@Id";
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
                        ulong Cart_id = (ulong)MyTable.Rows[0]["cart_id"];
                        ulong Product_id = (ulong)MyTable.Rows[0]["product_id"];
                        int Quantity = (int)MyTable.Rows[0]["quantity"];

                        return new CartItem { Cart_item_id = SearchId, Cart_id = Cart_id, Product_id = Product_id, Quantity = Quantity };
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

        public static CartItem createNewCartItem(CartItem NewCartItem, out string Errors)
        {
            Errors = "";
            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "insert into cart_items (cart_id,product_id,quantity) values (@CartId,@ProductId,@Quantity)";
                MyCommand.Parameters.AddWithValue("@CartId", NewCartItem.Cart_id);
                MyCommand.Parameters.AddWithValue("@ProductId", NewCartItem.Product_id);
                MyCommand.Parameters.AddWithValue("@Quantity", NewCartItem.Quantity);

                try
                {
                    MyCommand.ExecuteNonQuery();
                    MyCommand.CommandText = "select max(cart_item_id) as new_id from cart_items";
                    NewCartItem.Cart_item_id = (ulong)MyCommand.ExecuteScalar();
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
                    NewCartItem = null;
                }
                catch (SystemException error)
                {
                    Errors = error.Message;
                    NewCartItem = null;
                }
            }

            return NewCartItem;

        }

        public static bool deleteCartItemById(ulong DeleteId, out string Errors)
        {
            Errors = "";
            bool Success = false;
            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "delete from cart_items where cart_item_id=@Id";
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

    /// <summary>
    /// Items to add to the shopping carts
    /// </summary>
    public class CartItem
    {
        public ulong Cart_item_id { get; set; }
        public ulong Cart_id { get; set; }
        public ulong Product_id { get; set; }
        public int Quantity { get; set; }
    }

}
