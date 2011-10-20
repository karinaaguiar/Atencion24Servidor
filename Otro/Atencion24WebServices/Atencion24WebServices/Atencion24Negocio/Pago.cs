using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Atencion24WebServices.Atencion24DAO;

namespace Atencion24WebServices.Atencion24Negocio
{
    public class Pago
    {
        private string medico;
        private string fechaPago = " ";
        private decimal montoLiberado = 0;
        private ArrayList deducciones = null;
        private decimal montoNeto = 0;
        private bool sinpago = false;

        ///Constructor
        public Pago(string codMedico)
        {
            medico = codMedico;
        }

        public Pago() { }

        //Getter y Setters
        public decimal MontoLiberado
        {
            get { return montoLiberado; }
            set { montoLiberado = value; }
        }

        public ArrayList Deducciones
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

        public string FechaPago
        {
            get { return fechaPago; }
            set { fechaPago = value; }
        }

        /// <summary>
        /// Consultar proximo pago que recibirá el medico
        /// </summary>
        public void consultarProximoPago()
        {
            DataSet ds = new DataSet();
            DataSet dsConcepto = new DataSet();
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
                    montoLiberado = decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString());
                    montoNeto = montoLiberado;

                    //Deducciones
                    ud = new PagoDAO();
                    ds = ud.ProximoPagoDeducciones(medico);

                    Deduccion deduccion;
                    String concepto = "";
                    decimal monto = 0;
                    
                    if (ds.Tables[0].Rows.Count == 0) { Deducciones = null; }
                    else
                    {
                        Deducciones = new ArrayList();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            deduccion = new Deduccion();
                            //Concepto deduccion
                            if (dr.ItemArray.ElementAt(0) != DBNull.Value)
                            {
                                concepto = dr.ItemArray.ElementAt(0).ToString();
                                ud = new PagoDAO();
                                dsConcepto = ud.NombreConcepto(concepto);
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
                                monto = decimal.Parse(dr.ItemArray.ElementAt(1).ToString());
                                deduccion.Monto = monto;
                                montoNeto -= monto;
                            }
                            
                            //Agregamos la deduccion al ArrayList
                            deducciones.Add(deduccion);
                        }
                    }

                    //Fecha de pago
                    DateTime hoy = DateTime.Today; 
                    DateTime ayer = hoy.AddDays(-1);
                    FechaPago = ayer.ToString();
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
