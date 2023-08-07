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


namespace YNotes
{
    /// <summary>
    /// Interaction logic for Lists.xaml
    /// </summary>
    public partial class Lists : Window
    {
        DataBase db = new DataBase();
        
        internal static int idList;
        internal static int idUser; 
        public Lists(int idOfUser)
        {
            idUser = idOfUser;
            InitializeComponent();
            DataGridDemonstrate();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            TaskName.IsOpen = true;
        }

        private void OKButtonAddTask_Click(object sender, RoutedEventArgs e)
        {
            string text;

            text = InputTaskName.Text;
            //ListBoxLists.Items.Add(text);
            //myDB.AddList(text);
            InputTaskName.Text = "";
            TaskName.IsOpen = false;
        }
    

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            //if (ListBoxTasks.SelectedIndex == -1)
            //{
            //    MessageBox.Show("You haven't selected an item", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
            //else
            //{
            //    myDB.DeleteTask(ListBoxTasks.SelectedItem.ToString(), selectedList);
            //    ListBoxTasks.Items.RemoveAt(ListBoxTasks.SelectedIndex);

            //}
            MessageBoxResult result = System.Windows.MessageBox.Show("Do you want to delete?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {

            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            //ListBoxTasks.Items.Clear();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Do you want to delete?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {

            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            ListName.IsOpen = true;
        }

        private void OKButtonAddList_Click(object sender, RoutedEventArgs e)
        {
            string text;

            text = InputListName.Text;
            //ListBoxLists.Items.Add(text);
            //myDB.AddList(text);
            InputListName.Text = "";
            ListName.IsOpen = false;
        }

         
       private void DataGridDemonstrate()
        {
            var adabter = new SqlDataAdapter();
            var table = new DataTable();
            var queryString = $"select id, title from Lists where id_user = '{idUser}'";
            var command = new SqlCommand(queryString, db.GetConnection());

            //this.ListsDataGrid.Columns[0].Visible = false;
            adabter.SelectCommand = command;
            adabter.Fill(table);
            ListsDataGrid.ItemsSource = table.DefaultView;
        
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
                
                var idList = selectedRow["ID"].ToString();
                TitleTextBlock.Text = title;

                var adabter = new SqlDataAdapter();
                var table = new DataTable();
                var queryString = $"select title from Tasks where id_user = '{idUser}' and id_list = '{idList}' ";
                var command = new SqlCommand(queryString, db.GetConnection());

                adabter.SelectCommand = command;
                adabter.Fill(table);
                TasksDataGrid.ItemsSource = table.DefaultView;

            }
        }

        
    }
}
