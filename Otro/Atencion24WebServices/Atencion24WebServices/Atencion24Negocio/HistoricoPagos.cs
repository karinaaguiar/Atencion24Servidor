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
        public ArrayList Pagos
        {
            get { return pagos; }
            set { pagos = value; }
        }

        //Consultar Histórico de Pagos
        public void consultarHistoricoPagos()
        {
            DataSet ds = new DataSet();
            DataSet dsDeducciones = new DataSet();
            DataSet dsConceptos = new DataSet();
            HistoricoPagosDAO ud = new HistoricoPagosDAO();

            Pago pago;
            String fechaPago;
            String concepto;
            String monto;
            string[,] deducciones;
            int numDed = 0;

            //Monto Liberado
            ds = ud.HistoricoPagosFechayMontoLiberado(medico, fechaI, fechaF);

            //En esta nómina no se liberaron honorarios para este médico
            if (ds.Tables[0].Rows.Count == 0) { sinpagos = true; return; }
            else
            {
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) == DBNull.Value)
                {
                    sinpagos = true;
                    return;
                }
                else
                {
                    pagos = new ArrayList();// [ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        pago = new Pago(medico);
                        DataRow row = ds.Tables[0].Rows[i];
                        
                        //Fecha de Pago
                        fechaPago = row.ItemArray.ElementAt(0).ToString();
                        pago.FechaPago = fechaPago;
                        
                        //Monto Liberado 
                        pago.MontoLiberado = row.ItemArray.ElementAt(1).ToString();
                        pago.MontoNeto = decimal.Parse(pago.MontoLiberado);

                        //Deducciones
                        ud = new HistoricoPagosDAO();
                        dsDeducciones = new DataSet();
                        dsDeducciones = ud.HistoricoPagosDeducciones(medico, fechaPago);

                        if (dsDeducciones.Tables[0].Rows.Count == 0) { pago.Deducciones = null; }
                        else
                        {
                            if (dsDeducciones.Tables[0].Rows[0].ItemArray.ElementAt(0) == DBNull.Value)
                            {
                                pago.Deducciones = null;
                            }
                            else
                            {
                                deducciones = new string[dsDeducciones.Tables[0].Rows.Count, 2];
                                foreach (DataRow dr in dsDeducciones.Tables[0].Rows)
                                {
                                    concepto = dr.ItemArray.ElementAt(0).ToString();
                                    monto = dr.ItemArray.ElementAt(1).ToString();

                                    //Buscamos el nombre del concepto
                                    PagoDAO ud1 = new PagoDAO();
                                    dsConceptos = new DataSet();
                                    dsConceptos = ud1.NombreConcepto(concepto);
                                    if (dsConceptos.Tables[0].Rows.Count == 0) { concepto = ""; }
                                    else
                                    {
                                        if (dsConceptos.Tables[0].Rows[0].ItemArray.ElementAt(0) == DBNull.Value)
                                        {
                                            concepto = "";
                                        }
                                        else
                                        {
                                            String concepto1 = dsConceptos.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                                            concepto = concepto1.Substring(0, 1).ToUpper() + concepto1.Substring(1).ToLower(); 
                                        }
                                    }
                                    deducciones[numDed, 0] = concepto;
                                    deducciones[numDed, 1] = monto;
                                    pago.MontoNeto -= decimal.Parse(monto);
                                    numDed++;
                                }
                                pago.Deducciones = deducciones;
                                numDed = 0; 
                            }
                        }
                        pagos.Add(pago);
                    }
                }
            }
        }


    }
}
