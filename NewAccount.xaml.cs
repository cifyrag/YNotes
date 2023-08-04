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
using System.Diagnostics.Eventing.Reader;

namespace YNotes
{
    /// <summary>
    /// Interaction logic for NewAccount.xaml
    /// </summary>
    public partial class NewAccount : Page
    {
        DataBase db = new DataBase();
        Lists listWin;
        int idUser;
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
            var login = Login.Text.Trim().ToLower();
            var password = Password.Password.Trim();
            var rePassword = RePassword.Password.Trim();
            var email = Email.Text.Trim().ToLower();

            var checkLog = CheckLoginEmail(login, email);
            var checkPass = CheckPasswordRePassword(password, rePassword);

            if ( checkLog && checkPass )
            {

                var queryString = $"insert into Users(login, password, email) values('{login}','{password}', '{email}')";
                var command = new SqlCommand(queryString, db.GetConnection());
                

                db.OpenConnection();

                if (command.ExecuteNonQuery() == 1)
                {
                    listWin = new Lists(idUser);
                    listWin.Show();
                    var win = Window.GetWindow(this);
                    win.Hide();

                    string queryString2 = "SELECT MAX(id) FROM Users";
                    SqlCommand command2 = new SqlCommand(queryString2, db.GetConnection());

                    object result = command2.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        idUser = Convert.ToInt32(result);
                    }
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
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Login.MaxLength = 50;
            Password.MaxLength = 50;
            RePassword.MaxLength = 50;
            Email.MaxLength = 100;


        }
        
        private bool CheckLoginEmail(string login, string email)
        {
            bool cou = true;
            
            if (string.IsNullOrEmpty(login) || login.Length<3 ) 
            {
                Login.Background = Brushes.IndianRed;
                cou = false;
            }else
            {
                Login.Background = Brushes.White;
            }
            
            if (!email.Contains('@') || !email.Contains('.') || string.IsNullOrEmpty(email))
            {
                Email.Background = Brushes.IndianRed;
                cou = false;
            }
            else
            {
                Email.Background = Brushes.White;
            }
            var adabter = new SqlDataAdapter();
            var table = new DataTable();
            var gueryString = $"select id, login, email from users where login = '{login}' or email = '{email}'";
            var command = new SqlCommand(gueryString, db.GetConnection());
           
            adabter.SelectCommand = command;
            adabter.Fill(table);
            if (table.Rows.Count > 0)
            {
                Login.Text = "";
                Email.Text = "";
                MessageBox.Show("Account with this login or email already exist", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                cou = false;
            }
            return cou;

        }

        private bool CheckPasswordRePassword(string password, string rePassword)
        {
            if (string.IsNullOrEmpty(password) || password.Count(char.IsUpper) == 0 || password.Count(char.IsLower) == 0 || password.Count(char.IsDigit) == 0)
            {
                Password.Background = Brushes.IndianRed;
                
                return false;
            }
            if (password != rePassword)
            {
                RePassword.Background = Brushes.IndianRed;
                return false;
            }else
            {
                RePassword.Background = Brushes.White;

            }
            
            Password.Background = Brushes.White;
            
            return true;

        }
    }
}
