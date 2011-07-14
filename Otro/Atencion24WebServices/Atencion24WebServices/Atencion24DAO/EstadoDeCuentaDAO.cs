using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration; //Acceder al archivo de configuracion, donde están los datos de la conexion con SQL Server
using System.Data.SqlClient; //Librería para interactuar con SQL SERVER 2008
using Atencion24WebServices.Atencion24Negocio;

namespace Atencion24WebServices.Atencion24DAO
{
    public class EstadoDeCuentaDAO : DAO
    {
        public EstadoDeCuentaDAO(): base()
        {}

        public DataSet EdoCtaMontoFacturadoTotal(String medico)
        {
            Cmd.CommandText = QueryAtencion24.EdoCtaMontoFacturadoTotal(medico);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }
    }
}
