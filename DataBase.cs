using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Runtime.Serialization.Formatters;

namespace YNotes
{
    internal class DataBase
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source = CIFYRAG; Initial Catalog= YNotesDB; Integrated Security=true; ");
        //internal int idUser;
        internal int idList = -1;




        public void OpenConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void CloseConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }

        }


        public SqlConnection GetConnection()
        {
            return sqlConnection; 
        }

        
    }

}
