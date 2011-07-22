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
    public class ListadoFianzasDAO : DAO
    {
        public ListadoFianzasDAO(): base()
        {}

        //Consultar listado de fianzas 
        public DataSet ListadoFianzas(String medico)
        {
            Cmd.CommandText = QueryAtencion24.ListadoFianzas(medico);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Nombre del paciente 
        public DataSet NombrePaciente(String cedula)
        {
            Cmd.CommandText = QueryAtencion24.NombrePaciente(cedula);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }
    }
}
