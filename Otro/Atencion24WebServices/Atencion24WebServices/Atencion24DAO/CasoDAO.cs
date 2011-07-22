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
    public class CasoDAO: DAO
    {
        public CasoDAO(): base()
        {}

        //Consultar total facturado en el caso 
        public DataSet DetalleDeCasoTotalFacturado(String medico, String nroCaso, String udn)
        {
            Cmd.CommandText = QueryAtencion24.DetalleDeCasoTotalFacturado(medico, nroCaso, udn);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Consultar total notas de crédito 
        public DataSet DetalleDeCasoTotalNotasCred(String medico, String nroCaso, String udn)
        {
            Cmd.CommandText = QueryAtencion24.DetalleDeCasoTotalNotasCred(medico, nroCaso, udn);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Consultar total notas de débito
        public DataSet DetalleDeCasoTotalNotasDeb(String medico, String nroCaso, String udn)
        {
            Cmd.CommandText = QueryAtencion24.DetalleDeCasoTotalNotasDeb(medico, nroCaso, udn);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Consultar total abonado en el caso 
        public DataSet DetalleDeCasoTotalAbonado(String medico, String nroCaso, String udn)
        {
            Cmd.CommandText = QueryAtencion24.DetalleDeCasoTotalAbonado(medico, nroCaso, udn);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Consultar nombre y cedula del paciente
        public DataSet DetalleDeCasoNombreyCedulaPaciente(String nroCaso, String udn)
        {
            Cmd.CommandText = QueryAtencion24.DetalleDeCasoNombreyCedulaPaciente(nroCaso, udn);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Consultar principal responsable de pago del caso 
        public DataSet DetalleDeCasoPpalResponsable(String nroCaso, String udn)
        {
            Cmd.CommandText = QueryAtencion24.DetalleDeCasoPpalResponsable(nroCaso, udn);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Consultar tipo de responsble
        public DataSet DetalleDeCasoTipoDeResponsable(String nroCaso, String udn, String codigoResp)
        {
            Cmd.CommandText = QueryAtencion24.DetalleDeCasoTipoDeResponsable(nroCaso, udn, codigoResp);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Consultar nombre del responsble
        public DataSet DetalleDeCasoNombreResponsable(String codigoResp, String tipoResp)
        {
            Cmd.CommandText = QueryAtencion24.DetalleDeCasoNombreResponsable(codigoResp, tipoResp);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }
    }
}
