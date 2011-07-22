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

        //Consultar nombre del responsable
        public DataSet DetalleDeCasoNombreResponsable(String codigoResp, String tipoResp)
        {
            Cmd.CommandText = QueryAtencion24.DetalleDeCasoNombreResponsable(codigoResp, tipoResp);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Listado de Honorarios prestados en el caso por el médico  
        public DataSet DetalleDeCasoListadoHonorarios(String medico, String nroCaso, String unidadNegocio)
        {
            Cmd.CommandText = QueryAtencion24.DetalleDeCasoListadoHonorarios(medico, nroCaso, unidadNegocio);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Nombre de Honorario prestado en el caso por el médico  
        public DataSet DetalleDeCasoNombreHonorario(String suministro, String area)
        {
            Cmd.CommandText = QueryAtencion24.DetalleDeCasoNombreHonorario(suministro, area);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }
               
        //Facturado por honorario prestado en el caso por el médico  
        public DataSet DetalleDeCasoFacturadoHonorario(String medico, String nroCaso, String unidadNegocio, String servicio, String suministro, String area)
        {
            Cmd.CommandText = QueryAtencion24.DetalleDeCasoFacturadoHonorario(medico, nroCaso, unidadNegocio, servicio, suministro, area);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }
                
        //Notas de Crédito por honorario prestado en el caso por el médico   
        public DataSet DetalleDeCasoNotasCredHonorario(String medico, String nroCaso, String unidadNegocio, String servicio, String suministro, String area)
        {
            Cmd.CommandText = QueryAtencion24.DetalleDeCasoNotasCredHonorario(medico, nroCaso, unidadNegocio, servicio, suministro, area);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }
                
        //Notas de Débito por honorario prestado en el caso por el médico  
        public DataSet DetalleDeCasoNotasDebHonorario(String medico, String nroCaso, String unidadNegocio, String servicio, String suministro, String area)
        {
            Cmd.CommandText = QueryAtencion24.DetalleDeCasoNotasDebHonorario(medico, nroCaso, unidadNegocio, servicio, suministro, area);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Monto pagado por honorario prestado en el caso por el médico  
        public DataSet DetalleDeCasoPagadoPorHonorario(String medico, String nroCaso, String unidadNegocio, String servicio, String suministro, String area)
        {
            Cmd.CommandText = QueryAtencion24.DetalleDeCasoPagadoPorHonorario(medico, nroCaso, unidadNegocio, servicio, suministro, area);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

    }

}
