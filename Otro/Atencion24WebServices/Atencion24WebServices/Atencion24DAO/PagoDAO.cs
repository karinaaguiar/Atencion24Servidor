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
    public class PagoDAO : DAO
    {
        public PagoDAO(): base()
        {}

        //Consultar Monto Liberado
        public DataSet ProximoPagoMontoLiberado(String medico)
        {
            Cmd.CommandText = QueryAtencion24.ProximoPagoMontoLiberado(medico);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Consultar Deducciones
        public DataSet ProximoPagoDeducciones(String medico)
        {
            Cmd.CommandText = QueryAtencion24.ProximoPagoDeducciones(medico);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Consultar Nombre Concepto por deducción
        public DataSet NombreConcepto(String codigoConcepto)
        {
            Cmd.CommandText = QueryAtencion24.NombreConcepto(codigoConcepto);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }



  
    }
}
