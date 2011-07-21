using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Atencion24WebServices.Atencion24DAO;

namespace Atencion24WebServices.Atencion24Negocio
{
    public class FacturadoUDN
    {
        private string medico;
        private string fechaI;
        private string fechaF;
		private decimal montoTotal;
        private decimal hospitalizacion;
        private decimal emergencia;
        private decimal cirugia;
        private decimal convenios;
        private bool sinFacturado = false;
        private static string HOSP = "1";
        private static string EMER = "4";
        private static string CIRU = "7";
        private static string CONV = "15";

        ///Constructor
        public FacturadoUDN(string codMedico, string fechaI_tb, string fechaF_tb)
        {
            medico = codMedico;
            fechaI = fechaI_tb;
            fechaF = fechaF_tb;
            this.montoTotal = 0;
            this.hospitalizacion = 0; 
            this.emergencia = 0;
            this.cirugia = 0;
            this.convenios = 0; 
        }

        //Getter y Setters
        public decimal MontoTotal
        {
            get { return montoTotal; }
            set { montoTotal = value; }
        }

        public decimal Hospitalizacion
        {
            get { return hospitalizacion; }
            set { hospitalizacion = value; }
        }

        public decimal Emergencia
        {
            get { return emergencia; }
            set { emergencia = value; }
        }

        public decimal Cirugia
        {
            get { return cirugia; }
            set { cirugia = value; }
        }

        public decimal Convenios
        {
            get { return convenios; }
            set { convenios = value; }
        }

        public bool SinFacturado
        {
            get { return sinFacturado; }
            set { sinFacturado = value; }
        }

        //Consultar Estado de cuenta por Antiguedad saldo
        public void consultarHonorariosFacturados()
        {
            DataSet ds = new DataSet();
            FacturadoDAO ud = new FacturadoDAO();

            String monto;

            //Monto Total Hospitalización 
            ds = ud.HonorariosFacturadosMontoPorUDN(medico, fechaI, fechaF, HOSP);

            //Verificamos que el medico haya facturado honorarios en hospitalización 
            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                {
                    monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                    hospitalizacion = decimal.Parse(monto);
                }
            }
            montoTotal = hospitalizacion;

            //Monto Total Emergencia
            ud = new FacturadoDAO();
            ds = ud.HonorariosFacturadosMontoPorUDN(medico, fechaI, fechaF, EMER);

            //Verificamos que el medico haya facturado honorarios en emergencia 
            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                {
                    monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                    emergencia = decimal.Parse(monto);
                }
            }
            montoTotal += emergencia;

            //Monto Total Cirugía Ambulatoria 
            ud = new FacturadoDAO();
            ds = ud.HonorariosFacturadosMontoPorUDN(medico, fechaI, fechaF, CIRU);

            //Verificamos que el medico haya facturado honorarios en cirugía ambulatoria  
            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                {
                    monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                    cirugia = decimal.Parse(monto);
                }
            }
            montoTotal += cirugia;

            //Monto Total Convenios 
            ud = new FacturadoDAO();
            ds = ud.HonorariosFacturadosMontoPorUDN(medico, fechaI, fechaF, CONV);

            //Verificamos que el medico haya facturado honorarios en convenios 
            if (ds.Tables[0].Rows.Count != 0)
            {
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                {
                    monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                    convenios = decimal.Parse(monto);
                }
            }
            montoTotal += convenios;
            if (montoTotal == 0) sinFacturado = true;

        }

    }
}
