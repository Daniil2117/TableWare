using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TablewareBozh.Classes;

namespace TablewareBozh.Pages
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Page
    {
        public static string Fio { get; set; }
        public static string UserId { get; set; }
        public static string Role { get; set; }

        public Autorization()
        {
            InitializeComponent();
        }
        void LoginInSystem()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(Connection.Connect))
                {
                    connect.Open();
                    if (!(Login.Text != "" && Password.Text != ""))
                    {
                        MessageBox.Show("Введите данные"); return;
                    }
                    string sqlExp = "SELECT [UserSurname],[UserPassword],[UserName],[UserPatronymic],[UserRole],[UserLogin] FROM [dbo].[User] WHERE [UserLogin] = @login";
                    SqlCommand cmd = new SqlCommand(sqlExp, connect);
                    cmd.Parameters.AddWithValue("@login", Login.Text);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        Fio = reader["UserSurname"].ToString() + " " + reader["UserName"].ToString() + " " + reader["UserPatronymic"].ToString();
                        UserId = reader["UserLogin"].ToString();
                        Role = reader["UserRole"].ToString();

                        if (reader[1].ToString() == Password.Text)
                            switch (reader["UserRole"])
                            {
                                case 1:
                                    NavigatePages.MainFrame.Navigate(new ViewTovars());
                                    break;
                                case 2:
                                    NavigatePages.MainFrame.Navigate(new ViewTovars());
                                    break;
                                case 3:
                                    NavigatePages.MainFrame.Navigate(new ViewTovars());
                                    break;
                                default:
                                    MessageBox.Show("Ошибка роли");
                                    break;
                            }
                        else
                        {
                            MessageBox.Show("Неверный пароль");
                            Password.Text = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Нет такого пользователя");
                        Login.Text = "";
                        Password.Text = "";
                    }
                    MessageBox.Show("Вы успешно вошли в систему!");
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            LoginInSystem();
        }

        private void ViewTovars_Click(object sender, RoutedEventArgs e)
        {
            NavigatePages.MainFrame.Navigate(new ViewTovars());
        }
    }
}
