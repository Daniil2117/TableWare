using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TablewareBozh.Classes
{
    public class Tovars
    {
        public string Name { get; set; } // название товара
        public string Description { get; set; } // описание товара
        public string Manufacturer { get; set; } // производитель товара
        public decimal Cost { get; set; } // стоимость товара
        public int QuantityInStock { get; set; } // количество товара
        public string ImageSources { get; set; } // фото товара

        public int ProductDiscountAmountMax { get; set; } //текущая скидка
        public string ProductArticleNumber { get; set; } //артикул товара скидка
        public string ProductCategory { get; set; } //категория товара

        public Tovars(string name, string description, string manufacturer, decimal cost, int quantityInStock, string imageSources, int productDiscountAmountMax, string productArticleNumber, string productCategory)
        {
            Name = name;
            Description = description;
            Manufacturer = manufacturer;
            Cost = cost;
            QuantityInStock = quantityInStock;
            ImageSources = imageSources; // добавление
            ProductDiscountAmountMax = productDiscountAmountMax;
            ProductArticleNumber = productArticleNumber;
            ProductCategory = productCategory;
        }

        // загрузка данных из БД
        public static List<Tovars> LoadFromDB()
        {
            List<Tovars> spisok = new List<Tovars>();
            try
            {
                using (SqlConnection connection = new SqlConnection(Connection.Connect))
                {
                    connection.Open();
                    string sqlExp = "SELECT [ProductName],[ProductDescription],[NameManufacturer], " +
                        " [ProductCost],[ProductQuantityInStock]," +
                        " [ProductPhoto],CAST([ProductDiscountAmount] as int) as [ProductDiscountAmount],[ProductArticleNumber],[CategoryName] FROM[dbo].[Product]" +
                        " JOIN [dbo].[Manufacturers] ON [dbo].[Manufacturers].[ManufacturerID] = [dbo].[Product].ProductManufacturer" +
                        " JOIN [dbo].[Categories] ON [dbo].[Categories].[CategoryID] = [dbo].[Product].[ProductCategory]";
                    SqlCommand cmd = new SqlCommand(sqlExp, connection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        spisok.Add(new Tovars(
                            reader.GetString(0), // ProductName
                            reader.GetString(1), // ProductDescription
                            reader.GetString(2), // ProductManufacturer
                            reader.GetDecimal(3), // ProductCost
                            reader.GetInt32(4), // ProductQuantityInStock
                            reader.GetString(5),  // ProductPhoto
                            reader.GetInt32(6),
                            reader.GetString(7),
                            reader.GetString(8)
                            ));

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return spisok;
        }

        internal static string[] GetDataUser(int selectionGridIndex)
        {
            SqlConnection connection = new SqlConnection(Connection.Connect);
            string[] strings = new string[8];
            int p = 0;
            try
            {
                connection.Open();
                string sqlExp = "SELECT [ProductName],[ProductDescription],[NameManufacturer], " +
                        " [ProductCost],[ProductQuantityInStock]," +
                        " [ProductPhoto],CAST([ProductDiscountAmount] as int) as [ProductDiscountAmount],[ProductArticleNumber], [CategoryName] FROM[dbo].[Product]" +
                        " JOIN [dbo].[Manufacturers] ON[dbo].[Manufacturers].[ManufacturerID] = [dbo].[Product].ProductManufacturer" +
                        " JOIN [dbo].[Categories] ON [dbo].[Categories].[CategoryID] = [dbo].[Product].[ProductCategory]";
                SqlCommand cmd = new SqlCommand(sqlExp, connection);
                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        if (p == selectionGridIndex)
                        {
                            strings[0] = r[0].ToString();
                            strings[1] = r[1].ToString();
                            strings[2] = r[2].ToString();
                            strings[3] = r[3].ToString();
                            strings[4] = r[4].ToString();
                            strings[5] = r[5].ToString();
                            strings[6] = r[6].ToString();
                            strings[7] = r[7].ToString();
                            strings[8] = r[8].ToString();
                            break;
                        }
                        else
                        {
                            p++;
                        }
                    }
                    r.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
            return strings;
        }

        //метод добавляющий новую запись в БД
        public static void AddNewTovar(string name, string description, string manufacturer, decimal cost, int quantityInStock, 
            string imageSources, int productDiscountAmountMax, string productArticleNumber, string productCategory)
        {
            SqlConnection connection = new SqlConnection(Connection.Connect);
            try
            {
                connection.Open();
                string sqlExp = $"INSERT INTO [dbo].[Product] Values('{productArticleNumber}','{name}','{cost}','{productDiscountAmountMax+15}'," +
                    $" (SELECT [ManufacturerID] FROM [dbo].[Manufacturers] WHERE [NameManufacturer] = '{manufacturer}')," +
                    $" (SELECT [CategoryID] FROM [dbo].[Categories] WHERE [CategoryName] = '{productCategory}')," +
                    $" '{productDiscountAmountMax}', '{quantityInStock}','{description}','..\\Images\\{imageSources}') ";
                SqlCommand cmd = new SqlCommand(sqlExp, connection);
                SqlDataReader r = cmd.ExecuteReader();
                MessageBox.Show("Данные успешно добавлены");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
        //метод обновляющий запись
        public static void UpdateStaff(string name, string birthday, string telephone, string staff)
        {
            string[] fields = new string[4];
            fields = Tovars.GetDataUser(Pages.PageMain.selectionGridIndex);

            SqlConnection connection = new SqlConnection(Connection.Connect);
            try
            {
                connection.Open();
                string sqlExp = $"UPDATE Employee SET Name = '{name}',Birthday ='{birthday}',Telephone ='{telephone}',Staff ='{staff}'" +
                    $"WHERE Name = '{fields[0]}' AND Birthday = '{fields[1]}' AND Telephone = '{fields[2]}' AND Staff = '{fields[3]}'";
                SqlCommand cmd = new SqlCommand(sqlExp, connection);
                SqlDataReader r = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        //метод удаляющий запись
        public static void DeleteStaff()
        {
            string[] fields = new string[4];
            fields = Tovars.GetDataUser(Pages.PageMain.selectionGridIndex);

            SqlConnection connection = new SqlConnection(Connection.Connect);
            try
            {
                connection.Open();
                string sqlExp = $"DELETE Employee WHERE Name = '{fields[0]}' AND Birthday = '{fields[1]}' " +
                    $"AND Telephone = '{fields[2]}' AND Staff = '{fields[3]}'";
                SqlCommand cmd = new SqlCommand(sqlExp, connection);
                SqlDataReader r = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }
        */
    }
}
