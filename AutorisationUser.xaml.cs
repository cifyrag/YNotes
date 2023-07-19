using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

namespace YNotes
{
    /// <summary>
    /// Interaction logic for AutorisationUser.xaml
    /// </summary>
    public partial class AutorisationUser : Page
    {
        DataBase db = new DataBase();

        public AutorisationUser()
        {
            InitializeComponent();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {

            var login = Login.Text;
            var password = Password.Password;

            var adabter = new SqlDataAdapter();
            var tablet = new DataTable();

            var queryString = $"select login, password, email from Users where (login = {login} or email= {login}) and password = {password} ";
            var command = new SqlCommand(queryString, db.GetConnection());

            adabter.SelectCommand = command;
            adabter.Fill(tablet);
            
            if(tablet.Rows.Count == 1)
            {
                var listWin = new Lists();
                listWin.Show();
                Login.Text = "";
                Password.Password = "";
                var win = Window.GetWindow(this);
                win.Hide();
            }
            else
            {
                MessageBox.Show("Wrong login or password", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NewAccount());
           
        }

        private void Login_TextChanged(object sender, TextChangedEventArgs e)
        {
            Login.MaxLength = 100;
            Password.MaxLength = 50;

        }
    }
}
