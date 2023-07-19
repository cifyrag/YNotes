using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Data;

namespace YNotes
{
    /// <summary>
    /// Interaction logic for NewAccount.xaml
    /// </summary>
    public partial class NewAccount : Page
    {
        DataBase db = new DataBase();
        public NewAccount()
        {
            InitializeComponent();
        }
        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            
            NavigationService.Navigate(new AutorisationUser());
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            var login = Login.Text;
            var password = Password.Password;
            var rePassword = RePassword.Password;
            var email = Email.Text;

            
            var queryString = $"insert into Users(login, password, email) values('{login}','{password}', '{email}')";
            var command = new SqlCommand(queryString, db.GetConnection());
            bool checkUser = CheckUser(login, password, rePassword, email);
            db.OpenConnection();
            

          
            if(checkUser && command.ExecuteNonQuery() == 1)
            {
                var listWin = new Lists();
                listWin.Show();
                var win = Window.GetWindow(this);
                win.Hide();
            }
            else
            {
                MessageBox.Show("Account hasn't created", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            db.CloseConnection();

            Login.Text = "";
            Password.Password = "";
            RePassword.Password = "";
            Email.Text = "";
        }

        private Boolean CheckUser(string login, string password, string rePassword, string email)
        {
            if (password == rePassword)
            {
                var adabter = new SqlDataAdapter();
                var table = new DataTable();
                var gueryString = $"select id, login, email from users where login = '{login}' or email = '{email}'" ;
                var command = new SqlCommand(gueryString, db.GetConnection());

                adabter.SelectCommand = command;
                adabter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    MessageBox.Show("Account with this login or email already exist", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                } else { return true; }
            }
            else
            {
                return false;
            }
           
            
        }
        

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Login.MaxLength = 50;
            Password.MaxLength = 50;
            RePassword.MaxLength = 50;
            Email.MaxLength = 100;
        }
    }
}
