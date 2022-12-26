using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TablewareBozh.Classes;

namespace TablewareBozh.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddOrUpdateTovar.xaml
    /// </summary>
    public partial class AddOrUpdateTovar : Page
    {
        public AddOrUpdateTovar()
        {
            InitializeComponent();
            LoadComboBoxManuf();
            LoadComboBoxCateg();
        }

        private void AddNewTovat_Click(object sender, RoutedEventArgs e)
        {
            if(TovarName.Text == "" || DescriptionTovar.Text == "" || NameManuf.Items.ToString() == "" || CostTovar.Text == "" || KolvoOnSklad.Text == ""
                || NamePict.Text == "" || MaxDiscount.Text == "" || TovarId.Text == "" || CategTovar.Items.ToString() == "")
            {
                System.Windows.MessageBox.Show("Заполните все поля!");
            }
            else
            {
                Tovars.AddNewTovar(TovarName.Text, DescriptionTovar.Text, NameManuf.Text, decimal.Parse(CostTovar.Text), int.Parse(KolvoOnSklad.Text),
                NamePict.Text, int.Parse(MaxDiscount.Text), TovarId.Text, CategTovar.Text);
                NavigatePages.MainFrame.Navigate(new ViewTovars());
            }
        }

        private void GetNamePict_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG;)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            openFileDialog.Title = "Please select an image file.";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    PhotoTovar.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    NamePict.Text = openFileDialog.SafeFileName.ToString();
                }
                catch
                {
                    System.Windows.MessageBox.Show("Не удалось открыть фотографию", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        void LoadComboBoxManuf()
        {
            SqlConnection connection = new SqlConnection(Connection.Connect);
            try
            {
                connection.Open();
                string sqlExpression = "SELECT DISTINCT [NameManufacturer] FROM [dbo].[Manufacturers]";
                SqlCommand cmd = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        NameManuf.Items.Add(reader[0]);
                    }
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            connection.Close();
        }
        void LoadComboBoxCateg()
        {
            SqlConnection connection = new SqlConnection(Connection.Connect);
            try
            {
                connection.Open();
                string sqlExpression = "SELECT DISTINCT [CategoryName] FROM [dbo].[Categories]";
                SqlCommand cmd = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategTovar.Items.Add(reader[0]);
                    }
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            connection.Close();
        }
    }
}
