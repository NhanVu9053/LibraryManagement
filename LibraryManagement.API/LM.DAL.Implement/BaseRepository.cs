using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LM.DAL.Implement
{
    public class BaseRepository
    {
        protected IDbConnection connection;
        public BaseRepository()
        {
            connection = new SqlConnection(@"Data Source=TUNGVADILLO\SQLEXPRESS;Initial Catalog=LibraryManagementDB;Integrated Security=True");
        }
    }
}
