using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Atencion24WebServices.Atencion24DAO;

namespace Atencion24WebServices.Atencion24Negocio
{
    public class resumenPorUDN
    {
        private string medico;
        private float montoTotal;
        private float montoPorHospitalizacion;
        private float montoPorEmergencia;
        private float montoPorCirugiaAmb;
        private float montoPorConvenios;

        //Constructor
        public resumenPorUDN(string codMedico)
        {
            medico = codMedico;
        }

        public string Medico
        {
            get { return medico; }
            set { medico = value; }
        }

        //Consultar Estado de cuenta por UDN
        public void ConsultarEstadoDeCuentaUDN()
        {
            /*DataSet ds = new DataSet();
            float montoAPagarHosp;
			float montoAPagarEmer;
			float montoAPagarCirug;
			float montoAPagarConvenios;
			float montoNotasCredHosp;
			float montoNotasCredEmer;
			float montoNotasCredCirug;
			float montoNotasCredConvenios;
			float montoNotasDebHosp;
			float montoNotasDebEmer;
			float montoNotasDebCirug;
			float montoNotasDebConvenios;
			float montoPagadoHosp;
			float montoPagadoEmer;
			float montoPagadoCirug;
			float montoPagadoConvenios;			
			
			resumenPorUDN_DAO ud = new resumenPorUDN_DAO();
            
			ds = ud.ConsultarTotalMontoAPagarUDN(this.medico);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoNotasCreditoUDN(this.medico);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoNotasDebitoUDN(this.medico);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoPagadoUDN(this.medico);
			//Proceso DataSet 
			
			this.montoPorHospitalizacion = montoAPagarHosp - montoNotasCredHosp + montoNotasDebHosp - montoPagadoHosp;
			this.montoPorEmergencia = montoAPagarEmer - montoNotasCredEmer + montoNotasDebEmer - montoPagadoEmer;
			this.montoPorCirugiaAmb = montoAPagarCirug - montoNotasCredCirug + montoNotasDebCirug - montoPagadoCirug;
			this.montoPorConvenios = montoAPagarConvenios - montoNotasCredConvenios + montoNotasDebConvenios - montoPagadoConvenios;
			this.montoTotal = montoPorHospitalizacion + montoPorEmergencia + montoPorCirugiaAmb + montoPorConvenios;
        }
		
		public void ConsultarFacturadoUDN(DateTime fecha_inicio, DateTime fecha_fin)
        {
            DataSet ds = new DataSet();
				
			resumenPorUDN_DAO ud = new resumenPorUDN_DAO();
            
			ds = ud.ConsultarTotalMontoFacturadoUDN(this.medico, fecha_inicio, fecha_fin);
			//Proceso DataSet 
			this.montoTotal = montoPorHospitalizacion + montoPorEmergencia + montoPorCirugiaAmb + montoPorConvenios;
	    } */
        }
    }
}