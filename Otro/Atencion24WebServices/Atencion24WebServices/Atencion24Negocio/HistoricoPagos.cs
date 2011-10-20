using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Globalization;
using Atencion24WebServices.Atencion24DAO;

namespace Atencion24WebServices.Atencion24Negocio
{
    public class HistoricoPagos
    {
        private ArrayList pagos;
        private string medico;
        private bool sinpagos = false;
        private bool excede = false;
        private string fechaI;
        private string fechaF;

        ///Constructor
        public HistoricoPagos(string codMedico, string fechaI_tb, string fechaF_tb)
        {
            medico = codMedico;
            fechaI = fechaI_tb;
            fechaF = fechaF_tb;
        }

        public HistoricoPagos() { }

        public bool sinPagos
        {
            get { return sinpagos; }
            set { sinpagos = value; }
        }

        public bool Excede
        {
            get { return excede; }
            set { excede = value; }
        }

        public ArrayList Pagos
        {
            get { return pagos; }
            set { pagos = value; }
        }

        /// <summary>
        /// Consultar Histórico de Pagos
        /// </summary>
        public void consultarHistoricoPagos()
        {
            DataSet ds = new DataSet();
            DataSet dsDeducciones = new DataSet();
            DataSet dsConcepto = new DataSet();
            HistoricoPagosDAO ud = new HistoricoPagosDAO();
            PagoDAO ud1 = new PagoDAO();

            Pago pago;
            String fechaPago;
            String concepto;
            String monto;
            Deduccion deduccion;
            
            //Monto Liberado
            ds = ud.HistoricoPagosFechayMontoLiberado(medico, fechaI, fechaF);

            //En esta nómina no se liberaron honorarios para este médico
            if (ds.Tables[0].Rows.Count == 0) { sinpagos = true; return; }
            else
            {
                if (ds.Tables[0].Rows.Count > 50) { excede = true; return; }
                else
                {
                    pagos = new ArrayList();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        pago = new Pago(medico);
                        DataRow row = ds.Tables[0].Rows[i];

                        //Fecha de Pago
                        if (row.ItemArray.ElementAt(0) != DBNull.Value)
                        {
                            fechaPago = row.ItemArray.ElementAt(0).ToString();
                            pago.FechaPago = fechaPago;

                            //Deducciones
                            ud = new HistoricoPagosDAO();
                            dsDeducciones = new DataSet();
                            dsDeducciones = ud.HistoricoPagosDeducciones(medico, fechaPago);

                            if (dsDeducciones.Tables[0].Rows.Count == 0) { pago.Deducciones = null; }
                            else
                            {
                                pago.Deducciones = new ArrayList();
                                foreach (DataRow dr in dsDeducciones.Tables[0].Rows)
                                {
                                    deduccion = new Deduccion();
                                    //Concepto deduccion
                                    if (dr.ItemArray.ElementAt(0) != DBNull.Value)
                                    {
                                        concepto = dr.ItemArray.ElementAt(0).ToString();
                                        ud1 = new PagoDAO();
                                        dsConcepto = ud1.NombreConcepto(concepto);
                                        if (dsConcepto.Tables[0].Rows.Count != 0)
                                        {
                                            if (dsConcepto.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                                            {
                                                concepto = dsConcepto.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                                                deduccion.Concepto = concepto;
                                            }
                                        }
                                    }

                                    //Monto
                                    if (dr.ItemArray.ElementAt(1) != DBNull.Value)
                                    {
                                        monto = dr.ItemArray.ElementAt(1).ToString();
                                        deduccion.Monto = decimal.Parse(monto);
                                        pago.MontoNeto -= decimal.Parse(monto);
                                    }

                                    //Agregamos la deduccion al ArrayList
                                    pago.Deducciones.Add(deduccion);
                                }
                            }
                        }

                        //Monto Liberado 
                        if (row.ItemArray.ElementAt(1) != DBNull.Value)
                        {
                            pago.MontoLiberado = decimal.Parse(row.ItemArray.ElementAt(1).ToString());
                            pago.MontoNeto += pago.MontoLiberado;
                        }

                        pagos.Add(pago);
                    }
                }
            }
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
