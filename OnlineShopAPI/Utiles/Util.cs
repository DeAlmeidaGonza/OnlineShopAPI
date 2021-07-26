using System;
using MySql.Data.MySqlClient;

namespace WebApplication1.Utiles
{
    public static class Util
    {
        public static MySqlConnection getConnection()
        {
            MySqlConnection MyConnection = new MySqlConnection();
            MyConnection.ConnectionString = "Server=18.218.91.244; Uid=onlshp_admin; Pwd=OnlShp5894; Database=online_shop_v2";
            bool Success = true;
            try
            {
                MyConnection.Open();
                Console.WriteLine("Conectado a la base de datos.");
            }
            catch (SystemException error)
            {
                Console.WriteLine("Error al intentar conectarse a la base de datos: " + error.Message);
                Success = false;
            }

            if (Success)
            {
                return MyConnection;
            }
            else
            {
                return null;
            }
        }
    }
}
