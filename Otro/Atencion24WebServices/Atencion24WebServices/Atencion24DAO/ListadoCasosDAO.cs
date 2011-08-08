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
    public class ListadoCasosDAO : DAO
    {
        public ListadoCasosDAO(): base()
        {}

        //Consultar Listado de casos por apellido
        public DataSet DetalleCasoListadoDeCasos(String medico, String []apellido)
        {
            Cmd.CommandText = QueryAtencion24.DetalleCasoListadoDeCasos(medico, apellido);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }
    }
}
