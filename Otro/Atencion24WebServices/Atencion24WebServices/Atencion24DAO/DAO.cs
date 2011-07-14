using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient; //Librería para interactuar con SQL SERVER 2008
using System.Configuration; //Acceder al archivo de configuracion, donde están los datos de la conexion con SQL Server

namespace Atencion24WebServices.Atencion24DAO
{
    public class DAO 
    {
        private SqlConnection _cnn;
        private SqlCommand _cmd;
        private SqlDataAdapter _da;
        private DataSet _ds;
        private QueryAtencion24 _queryDao;
        private SqlDataReader _dr;

        public SqlConnection Cnn
        {
            get { return _cnn; }
            set { _cnn = value; }
        }

        public SqlCommand Cmd
        {
            get { return _cmd; }
            set { _cmd = value; }
        }

        public SqlDataAdapter Da
        {
            get { return _da; }
            set { _da = value; }
        }

        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }

        public QueryAtencion24 QueryAtencion24
        {
            get { return _queryDao; }
            set { _queryDao = value; }
        }

        public SqlDataReader Dr
        {
            get { return _dr; }
            set { _dr = value; }
        }

        public DAO()
        {
            QueryAtencion24 = new QueryAtencion24();

            //Preparo y abro una nueva conexión con la BD. MyCnn es el string de conexión obtenido del WebConfin en ConnectionStrings
            //string MyCnn = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["MyConnection"].ToString();
            string MyCnn = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString; 
            Cnn = new SqlConnection(MyCnn);
            Cnn.Open();
            Cmd = new SqlCommand();
            Cmd.CommandType = CommandType.Text;
            Da = new SqlDataAdapter();
            Ds = new DataSet();
            Cmd.Connection = Cnn;
        }

        //Función para cerrar una conexión a la base de datos
        public void CerrarConexionBd()
        {
            Cnn.Close();
        }
 
    }
}
