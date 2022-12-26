using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для ViewTovars.xaml
    /// </summary>
    public partial class ViewTovars : Page
    {
        public List<Tovars> spisok { get; set; }

        public ViewTovars()
        {
            InitializeComponent();
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBoxFiltrCost.Items.Add("Убрать фильтр");
            ComboBoxFiltrCost.Items.Add("Сначала дешёвые"); 
            ComboBoxFiltrCost.Items.Add("Сначала дорогие");
            ComboBoxFiltrManuf.Items.Add("Убрать фильтр");
            LoadComboBox();
            spisok = Tovars.LoadFromDB();
            listView.ItemsSource = spisok;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            NavigatePages.MainFrame.Navigate(new Autorization());
        }

        List<Tovars> SearchFam(string textSearch)
        {
            List<Tovars> searchList = new List<Tovars>();

            foreach (Tovars s in spisok)
            {
                if (s.Name.ToLower().StartsWith(textSearch.ToLower()) || s.Cost.ToString().ToLower().StartsWith(textSearch.ToLower()) 
                    || s.Description.ToLower().StartsWith(textSearch.ToLower()) || s.Manufacturer.ToLower().StartsWith(textSearch.ToLower())
                    || s.QuantityInStock.ToString().ToLower().StartsWith(textSearch.ToLower()))
                    searchList.Add(s);
            }
            return searchList;
        }

        List<Tovars> FiltrCost(int index)
        {
            List<Tovars> searchList = new List<Tovars>();

            var sortedCost = from s in spisok
                             orderby s.Cost 
                             select s;

            var sortedCostDesc = from s in spisok
                             orderby s.Cost descending
                             select s;
            if (ComboBoxFiltrCost.SelectedIndex == 1)
            {
                foreach (Tovars s in sortedCost)
                {
                    searchList.Add(s);
                }
            }
            if (ComboBoxFiltrCost.SelectedIndex == 2)
            {
                foreach (Tovars s in sortedCostDesc)
                {
                    searchList.Add(s);
                }
            }
            if (ComboBoxFiltrCost.SelectedIndex == 0)
            {
                foreach (Tovars s in spisok)
                {
                    searchList.Add(s);
                }
            }
            return searchList;
        }

        List<Tovars> FiltrManuf(string index)
        {
            List<Tovars> searchList = new List<Tovars>();

            var selectedManuf = from s in spisok
                                where s.Manufacturer.ToString() == ComboBoxFiltrManuf.SelectedItem.ToString()
                                select s;
            if(ComboBoxFiltrManuf.SelectedIndex != 0 || ComboBoxFiltrManuf.SelectedIndex != -1)
            {
                foreach (Tovars s in selectedManuf)
                {
                    searchList.Add(s);
                }
            }
            if(ComboBoxFiltrManuf.SelectedIndex == 0)
            {
                foreach (Tovars s in spisok)
                {
                    searchList.Add(s);
                }
            }
            
            return searchList;
        }

        void LoadComboBox()
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
                        ComboBoxFiltrManuf.Items.Add(reader[0]);
                    }
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            listView.ItemsSource = SearchFam(TextBoxSearch.Text);
        }

        private void ComboBoxFiltrCost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listView.ItemsSource = FiltrCost(ComboBoxFiltrCost.SelectedIndex);
        }

        private void ComboBoxFiltrManuf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listView.ItemsSource = FiltrManuf(ComboBoxFiltrManuf.SelectedItem.ToString());
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigatePages.MainFrame.Navigate(new AddOrUpdateTovar());
        }
    }
}
