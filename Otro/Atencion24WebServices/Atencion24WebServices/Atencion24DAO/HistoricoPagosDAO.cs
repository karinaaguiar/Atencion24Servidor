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
    public class HistoricoPagosDAO : DAO
    {
        public HistoricoPagosDAO(): base()
        {}

        //Consultar Fecha y Monto Liberado
        public DataSet HistoricoPagosFechayMontoLiberado(String medico, String fechaI , String fechaF)
        {
            Cmd.CommandText = QueryAtencion24.HistoricoPagosFechayMontoLiberado(medico, fechaI, fechaF);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Deducciones que le aplicaron al médico en la fecha de pago indicada
        public DataSet HistoricoPagosDeducciones(String medico, String fechapago)
        {
            Cmd.CommandText = QueryAtencion24.HistoricoPagosDeducciones(medico, fechapago);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }
    }
}
