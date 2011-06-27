using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Atencion24WebServices.Atencion24DAO;

namespace Atencion24WebServices.Atencion24Negocio
{
    public class EstadoDeCuentaPorAntiguedadSaldo
    {
        private string medico;
		private float montoTotal;
        private float montoA30Dias;
        private float montoA60Dias;
        private float montoA90Dias;
        private float montoA180Dias;	
		private float montoAMas180Dias;		

        ///Constructor
        public void resumenPorUDN(string codMedico)
        {
            medico = codMedico;
        }
		
		//Consultar Estado de cuenta por Antiguedad saldo
        public void ConsultarEstadoDeCuentaAS()
        {
            /*DataSet ds = new DataSet();
            float montoAPagar30;
			float montoAPagar60;
			float montoAPagar90;
			float montoAPagar180;
			float montoAPagarMas180;			
			float montoNotasCred30;
			float montoNotasCred60;
			float montoNotasCred90;
			float montoNotasCred180;
			float montoNotasCredMas180;			
			float montoNotasDeb30;
			float montoNotasDeb60;
			float montoNotasDeb90;
			float montoNotasDeb180;
			float montoNotasDebMas180;			
			float montoPagado30;
			float montoPagado60;
			float montoPagado90;
			float montoPagado180;			
			float montoPagadoMas180;			
			
			resumenPorUDN_DAO ud = new resumenPorUDN_DAO();
            
			ds = ud.ConsultarTotalMontoAPagarAS(this.medico, 30);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoAPagarAS(this.medico, 60);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoAPagarAS(this.medico, 90);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoAPagarAS(this.medico, 180);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoAPagarAS(this.medico, 181);
			//Proceso DataSet 
			
			ds = ud.ConsultarTotalMontoNotasCreditoAS(this.medico, 30);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoNotasCreditoAS(this.medico, 60);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoNotasCreditoAS(this.medico, 90);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoNotasCreditoAS(this.medico, 180);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoNotasCreditoAS(this.medico, 181);
			//Proceso DataSet 
			
			ds = ud.ConsultarTotalMontoNotasDebitoAS(this.medico, 30);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoNotasDebitoAS(this.medico, 60);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoNotasDebitoAS(this.medico, 90);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoNotasDebitoAS(this.medico, 180);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoNotasDebitoAS(this.medico, 181);
			//Proceso DataSet 
			
			ds = ud.ConsultarTotalMontoPagadoAS(this.medico, 30);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoPagadoAS(this.medico, 60);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoPagadoAS(this.medico, 90);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoPagadoAS(this.medico, 180);
			//Proceso DataSet 
			ds = ud.ConsultarTotalMontoPagadoAS(this.medico, 181);
			//Proceso DataSet 
			
			this.montoA30Dias = montoAPagar30 - montoNotasCred30 + montoNotasDeb30 - montoPagado30;
			this.montoA60Dias = montoAPagar60 - montoNotasCred60 + montoNotasDeb60 - montoPagado60;
			this.montoA90Dias = montoAPagar90 - montoNotasCred90 + montoNotasDeb90 - montoPagado90;
			this.montoA180Dias = montoAPagar180 - montoNotasCred180 + montoNotasDeb180 - montoPagado180;
			this.montoAMas180Dias = montoAPagarMas180 - montoNotasCredMas180 + montoNotasDebMas180 - montoPagadoMas180;
			this.montoTotal = montoA30Dias + montoA60Dias + montoA90Dias + montoA180Dias + montoAMas180Dias;*/
        }
    }
}
