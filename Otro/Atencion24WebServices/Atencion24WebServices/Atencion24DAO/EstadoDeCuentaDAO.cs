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

        //Consultar TOTAL Facturado
        public DataSet EdoCtaMontoFacturadoTotal(String medico)
        {
            Cmd.CommandText = QueryAtencion24.EdoCtaMontoFacturadoTotal(medico);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Consultar TOTAL Notas Credito
        public DataSet EdoCtaMontoNCredTotal(String medico)
        {
            Cmd.CommandText = QueryAtencion24.EdoCtaMontoNCredTotal(medico);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Consultar TOTAL Notas Débito
        public DataSet EdoCtaMontoNDebTotal(String medico)
        {
            Cmd.CommandText = QueryAtencion24.EdoCtaMontoNDebTotal(medico);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Consultar TOTAL Pagado
        public DataSet EdoCtaMontoPagadoTotal(String medico)
        {
            Cmd.CommandText = QueryAtencion24.EdoCtaMontoPagadoTotal(medico);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //MONTOS POR ANTIGUEDAD//
        //Consultar Facturado por Antiguedad
        public DataSet EdoCtaMontoFacturadoAntiguedad(String medico, int antiguedad)
        {
            Cmd.CommandText = QueryAtencion24.EdoCtaMontoFacturadoAntiguedad(medico, antiguedad);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Consultar Notas de Crédito por Antiguedad 
        public DataSet EdoCtaMontoNCredAntiguedad(String medico, int antiguedad)
        {
            Cmd.CommandText = QueryAtencion24.EdoCtaMontoNCredAntiguedad(medico,antiguedad);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Consultar Notas de Débito por Antiguedad 
        public DataSet EdoCtaMontoNDebAntiguedad(String medico, int antiguedad)
        {
            Cmd.CommandText = QueryAtencion24.EdoCtaMontoNDebAntiguedad(medico,antiguedad);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Consultar Pagado por Antiguedad 
        public DataSet EdoCtaMontoPagadoAntiguedad(String medico, int antiguedad)
        {
            Cmd.CommandText = QueryAtencion24.EdoCtaMontoPagadoAntiguedad(medico,antiguedad);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }
    }
}
