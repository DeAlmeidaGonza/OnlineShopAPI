using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using WebApplication1.Utiles;

namespace OnlineShopAPI.Models
{
    public static class Products
    {
        public static List<Product> getAllProducts()
        {
            List<Product> ReturnList = new List<Product>();

            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "select * from products";

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
                        ReturnList.Add(new Product
                        {
                            Product_id = (ulong)OneRegistry["product_id"],
                            Title = (string)OneRegistry["title"],
                            Price = (float)OneRegistry["price"],
                            Description = (string)OneRegistry["description"],
                            Category = (string)OneRegistry["category"],
                            Image = (string)OneRegistry["image"],
                        });
                    }
                }
            }

            return ReturnList;

        }

        public static Product getProductById(ulong SearchId)
        {
            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "select * from products where product_id=@id";
                MyCommand.Parameters.AddWithValue("@id", SearchId);

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
                        string Title = (string)MyTable.Rows[0]["title"];
                        float Price = (float)MyTable.Rows[0]["price"];
                        string Description = (string)MyTable.Rows[0]["description"];
                        string Category = (string)MyTable.Rows[0]["category"];
                        string Image = (string)MyTable.Rows[0]["image"];

                        return new Product { Product_id = SearchId, Title = Title, Price = Price, Description = Description , Category = Category, Image = Image };
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

        public static List<Product> getProductByCategory(string SearchCategory)
        {
            Console.WriteLine(SearchCategory);
            List<Product> ReturnList = new List<Product>();

            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "select * from products where category=@category";
                MyCommand.Parameters.AddWithValue("@category", SearchCategory);

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
                        ReturnList.Add(new Product
                        {
                            Product_id = (ulong)OneRegistry["product_id"],
                            Title = (string)OneRegistry["title"],
                            Price = (float)OneRegistry["price"],
                            Description = (string)OneRegistry["description"],
                            Category = (string)OneRegistry["category"],
                            Image = (string)OneRegistry["image"],
                        });
                    }
                }
                else
                {
                    ReturnList = null;
                }
            }

            return ReturnList;

        }

        public static Product createNewProduct(Product NewProduct, out string Errors)
        {
            Errors = "";
            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "insert into products (product_id,title,price,description,category,image) values (@ProductId,@Title,@Price,@Description,@Category,@Image)";
                MyCommand.Parameters.AddWithValue("@ProductId", NewProduct.Product_id);
                MyCommand.Parameters.AddWithValue("@Title", NewProduct.Title);
                MyCommand.Parameters.AddWithValue("@Price", NewProduct.Price);
                MyCommand.Parameters.AddWithValue("@Description", NewProduct.Description);
                MyCommand.Parameters.AddWithValue("@Category", NewProduct.Category);
                MyCommand.Parameters.AddWithValue("@Image", NewProduct.Image);

                try
                {
                    MyCommand.ExecuteNonQuery();
                    MyCommand.CommandText = "select max(product_id) as new_id from products";
                    NewProduct.Product_id = (ulong)MyCommand.ExecuteScalar();
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
                    NewProduct = null;
                }
                catch (SystemException error)
                {
                    Errors = error.Message;
                    NewProduct = null;
                }
            }

            return NewProduct;

        }

        public static bool deleteProductById(ulong DeleteId, out string Errors)
        {
            Errors = "";
            bool Success = false;
            MySqlConnection MyConnection = Util.getConnection();

            if (MyConnection != null)
            {
                MySqlCommand MyCommand = new MySqlCommand();
                MyCommand.Connection = MyConnection;
                MyCommand.CommandType = CommandType.Text;
                MyCommand.CommandText = "delete from products where product_id=@Id";
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
    /// Products in the inventory
    /// </summary>
    public class Product
    {
        public ulong Product_id { get; set; }
        public string Title { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
    }

}
