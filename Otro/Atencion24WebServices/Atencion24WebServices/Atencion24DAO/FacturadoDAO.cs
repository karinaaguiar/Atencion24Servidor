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
    public class FacturadoDAO : DAO
    {
        public FacturadoDAO()
            : base()
        { }

        //Consultar TOTAL Facturado por unidad de negocio 
        public DataSet HonorariosFacturadosMontoPorUDN(String medico, String fechaI, String fechaF)
        {
            Cmd.CommandText = QueryAtencion24.HonorariosFacturadosMontoPorUDN(medico, fechaI, fechaF);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }
    }
}
