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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Net.Mime.MediaTypeNames;


namespace YNotes
{
    /// <summary>
    /// Interaction logic for Lists.xaml
    /// </summary>
    public partial class Lists : Window
    {
        DataBase db = new DataBase();
        
        internal int idList = -1;
        internal int idUser;
        internal int idTask= -1;
        internal int selectedList = -1;
        public Lists(int idOfUser)
        {
            idUser = idOfUser;
            InitializeComponent();
            DataGridDemonstrate();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (selectedList != -1)
            {
                TaskName.IsOpen = true;
            }else
            {
                System.Windows.MessageBox.Show("You didn't selected list", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }

        private void OKButtonAddTask_Click(object sender, RoutedEventArgs e)
        {
            string text;
            text = InputTaskName.Text;
            
            var queryString = $"insert into Tasks(id_user, id_list, title) values({idUser}, {idList}, '{text}') ";
            var command = new SqlCommand(queryString, db.GetConnection());


            db.OpenConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                DataGridDemonstrate();
                
            }
            else
            {
                System.Windows.MessageBox.Show("Task hasn't created", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            db.CloseConnection();
            InputTaskName.Text = "";
            TaskName.IsOpen = false;
        }
    
        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Do you want to delete the task?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                var queryString = $"delete from Tasks where id= {this.idTask} ";
                var command = new SqlCommand(queryString, db.GetConnection());
                


                db.OpenConnection();

                if (command.ExecuteNonQuery() == 1)
                {
                    DataGridDemonstrate();
                }
                else
                {
                    System.Windows.MessageBox.Show("Task hasn't deleted", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                db.CloseConnection();
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Do you want to clear?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                var queryString = $"delete from Tasks where id_list={idList} ";
                var command = new SqlCommand(queryString, db.GetConnection());


                db.OpenConnection();

                if (command.ExecuteNonQuery() >0)
                {
                    DataGridDemonstrate();
                }
                else
                {
                    System.Windows.MessageBox.Show("List hasn't cleared", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                db.CloseConnection();

            }
            

        }

        private void DeleteList_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Do you want to delete the list with all tasks ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                var queryString = $"delete from Tasks where id_list={idList} ";
                var command = new SqlCommand(queryString, db.GetConnection());
                var queryString2 = $"delete from Lists where id={idList} ";
                var command2 = new SqlCommand(queryString2, db.GetConnection());


                db.OpenConnection();

                if (command.ExecuteNonQuery() > 0 && command2.ExecuteNonQuery()>0)
                {
                    DataGridDemonstrate();
                }
                else
                {
                    System.Windows.MessageBox.Show("List hasn't deleted", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                db.CloseConnection();
            }
        }

        private void AddList_Click(object sender, RoutedEventArgs e)
        {
            ListName.IsOpen = true;
        }

        private void OKButtonAddList_Click(object sender, RoutedEventArgs e)
        {

            string text;
            text = InputListName.Text;

            var queryString = $"insert into Lists(id_user, title) values({idUser}, '{text}') ";
            var command = new SqlCommand(queryString, db.GetConnection());


            db.OpenConnection();

            if (command.ExecuteNonQuery() > 0)
            {
                DataGridDemonstrate();
            }
            else
            {
                System.Windows.MessageBox.Show("List hasn't created", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            db.CloseConnection();
            InputListName.Text = "";
            ListName.IsOpen = false;
        }

        private void DataGridDemonstrate()
        {
            var adabter = new SqlDataAdapter();
            var table = new DataTable();
            var queryString = $"select id, title from Lists where id_user = '{idUser}'";
            var command = new SqlCommand(queryString, db.GetConnection());

            adabter.SelectCommand = command;
            adabter.Fill(table);
            ListsDataGrid.ItemsSource = table.DefaultView;
            ListsDataGrid.SelectedIndex = selectedList;
        }

        private void ListsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ListsDataGrid.SelectedIndex == -1)
            {
                TitleTextBlock.Text = string.Empty;
            }else
            {
                DataRowView selectedRow = (DataRowView)ListsDataGrid.SelectedItem;
                var title = selectedRow["Title"].ToString();
                this.selectedList = ListsDataGrid.SelectedIndex;
                
                this.idList = (int)selectedRow["ID"];
                TitleTextBlock.Text = title;

                var adabter = new SqlDataAdapter();
                var table = new DataTable();
                var queryString = $"select id, title from Tasks where id_user = '{idUser}' and id_list = '{idList}' ";
                var command = new SqlCommand(queryString, db.GetConnection());

                adabter.SelectCommand = command;
                adabter.Fill(table);
                TasksDataGrid.ItemsSource = table.DefaultView;

            }
        }

        private void TasksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)TasksDataGrid.SelectedItem;
            var inRow = selectedRow;

            if (ListsDataGrid.SelectedIndex != -1 && inRow != null)
            {
                
                this.idTask = (int)selectedRow["id"];
            }
            else
            {
                this.idTask = -1;
            }
        }
    }
}
