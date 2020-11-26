using System.Data;
using System.Data.SqlClient;

namespace LM.DAL.Implement
{
    public class BaseRepository
    {
        protected IDbConnection connection;
        public BaseRepository()
        {
            connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=LibraryManagementDB;Integrated Security=True");
        }
    }
}
