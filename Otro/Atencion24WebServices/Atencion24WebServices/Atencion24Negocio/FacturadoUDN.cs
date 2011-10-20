using System;
using System.Collections;
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
        private ArrayList factPorUdn;
        private bool sinFacturado = false;

        ///Constructor

        public FacturadoUDN() { }

        public FacturadoUDN(string codMedico, string fechaI_tb, string fechaF_tb)
        {
            medico = codMedico;
            fechaI = fechaI_tb;
            fechaF = fechaF_tb;
            this.montoTotal = 0; 
        }

        //Getter y Setters
        public decimal MontoTotal
        {
            get { return montoTotal; }
            set { montoTotal = value; }
        }

        public ArrayList FactPorUdn
        {
            get { return factPorUdn; }
            set { factPorUdn = value; }
        }

        public bool SinFacturado
        {
            get { return sinFacturado; }
            set { sinFacturado = value; }
        }

        //Consultar Honorarios facturados por UDN
        public void consultarHonorariosFacturados()
        {
            DataSet ds = new DataSet();
            FacturadoDAO ud = new FacturadoDAO();
            Facturado fact; 

            //Monto Total Facturado por UDN
            ds = ud.HonorariosFacturadosMontoPorUDN(medico, fechaI, fechaF);

            //Verificamos que el medico haya facturado honorarios 
            if (ds.Tables[0].Rows.Count == 0) { sinFacturado = true; return; }
            else
            {
                factPorUdn = new ArrayList();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr.ItemArray.ElementAt(0) != DBNull.Value)
                    {
                        if (decimal.Parse(dr.ItemArray.ElementAt(0).ToString()) != 0)
                        {
                            fact = new Facturado();

                            //Monto facturado
                            fact.Monto = decimal.Parse(dr.ItemArray.ElementAt(0).ToString());

                            //Nombre de la UDN
                            if (dr.ItemArray.ElementAt(2) != DBNull.Value)
                                fact.Udn = dr.ItemArray.ElementAt(2).ToString();

                            factPorUdn.Add(fact);

                            montoTotal += fact.Monto;

                        }
                    }
                }
            }
            if (montoTotal == 0) sinFacturado = true;
        }

        //Funcion que verifica si la base de datos está disponible
        public bool DisponibleBD()
        {
            DataSet ds = new DataSet();
            UsuarioDAO ud = new UsuarioDAO();
            bool disponible = true;

            ds = ud.InicioSesionDisponibleBD();
            if (ds.Tables[0].Rows.Count == 0)
                disponible = false;
            else
            {
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) == DBNull.Value)
                    disponible = false;
                else
                {
                    if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString().Equals("False"))
                        disponible = false;
                }
            }
            return disponible;
        }
    }
}
