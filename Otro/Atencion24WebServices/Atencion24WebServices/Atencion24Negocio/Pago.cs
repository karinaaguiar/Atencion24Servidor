﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Windows;
using Atencion24WebServices.Atencion24DAO;

namespace Atencion24WebServices.Atencion24Negocio
{
    public class Pago
    {
        private string medico;
        private String montoLiberado;
        private string[,] deducciones;
        private decimal montoNeto;
        private bool sinpago = false;

        ///Constructor
        public Pago(string codMedico)
        {
            medico = codMedico;
        }

        //Getter y Setters
        public String MontoLiberado
        {
            get { return montoLiberado; }
            set { montoLiberado = value; }
        }

        public string[,]  Deducciones
        {
            get { return deducciones; }
            set { deducciones = value; }
        }

        public decimal MontoNeto
        {
            get { return montoNeto; }
            set { montoNeto = value; }
        }

        public bool sinPago
        {
            get { return sinpago; }
            set { sinpago = value; }
        }

        //Consultar Proximo Pago
        public void consultarProximoPago()
        {
            DataSet ds = new DataSet();
            PagoDAO ud = new PagoDAO();

            //Monto Liberado
            ds = ud.ProximoPagoMontoLiberado(medico);

            //En esta nómina no se liberaron honorarios para este médico
            if (ds.Tables[0].Rows.Count == 0) { sinpago = true; return; }
            else
            {
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) == DBNull.Value)
                {
                    sinpago = true;
                    return;
                }
                else
                {
                    montoLiberado = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                    montoNeto = decimal.Parse(montoLiberado);

                    //Deducciones
                    ud = new PagoDAO();
                    ds = ud.ProximoPagoDeducciones(medico);
                    String concepto;
                    String monto;
                    int numDed = 0;

                    string[,] deducciones;

                    if (ds.Tables[0].Rows.Count == 0) { Deducciones = deducciones; }
                    else
                    {
                        if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) == DBNull.Value)
                        {
                            Deducciones = deducciones;
                        }
                        else
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                concepto = dr.ItemArray.ElementAt(0).ToString();
                                monto = dr.ItemArray.ElementAt(1).ToString();

                                //Buscamos el nombre del concepto
                                ud = new PagoDAO();
                                ds = ud.NombreConcepto(concepto);
                                if (ds.Tables[0].Rows.Count == 0) { concepto = ""; }
                                else
                                {
                                    if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) == DBNull.Value)
                                    {
                                        concepto = "";
                                    }
                                    else
                                    {
                                        concepto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                                    }
                                }
                                deducciones[numDed, 0] = concepto;
                                deducciones[numDed, 1] = monto;
                                montoNeto -= decimal.Parse(monto);
                                numDed++;
                            }
                        }
                    }
                }
            }
        }



    }
}
