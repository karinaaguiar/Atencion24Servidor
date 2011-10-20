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
    public class ListadoFianzas
    {
        private ArrayList fianzas;
        private string medico;
        private bool sinFianzas = false;

        ///Constructor
        public ListadoFianzas(string codMedico)
        {
            medico = codMedico;
        }

        public ListadoFianzas() { }

        public bool SinFianzas
        {
            get { return sinFianzas; }
            set { sinFianzas = value; }
        }
        public ArrayList Fianzas
        {
            get { return fianzas; }
            set { fianzas = value; }
        }

        //Consultar Listado de fianzas
        public void ConsultarListadoFianzas()
        {
            DataSet ds = new DataSet();
            DataSet dsNombre = new DataSet();
            ListadoFianzasDAO ud = new ListadoFianzasDAO();
            Fianza fianza;

            //Listado de casos
            ds = ud.ListadoFianzas(medico);

            //Este médico no tiene fianzas pendientes
            if (ds.Tables[0].Rows.Count == 0) { sinFianzas = true; return; }
            else
            {
                fianzas = new ArrayList();// [ds.Tables[0].Rows.Count];
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    fianza = new Fianza();
                    if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                        fianza.NroCaso = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();

                    if (ds.Tables[0].Rows[0].ItemArray.ElementAt(1) != DBNull.Value)
                        fianza.FechaEmision = ds.Tables[0].Rows[0].ItemArray.ElementAt(1).ToString();

                    if (ds.Tables[0].Rows[0].ItemArray.ElementAt(2) != DBNull.Value)
                    {
                        fianza.Paciente = ds.Tables[0].Rows[0].ItemArray.ElementAt(2).ToString();
                        ud = new ListadoFianzasDAO();
                        dsNombre = ud.NombrePaciente(fianza.Paciente);
                        if (dsNombre.Tables[0].Rows.Count != 0)
                        {
                            if (dsNombre.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                            {
                                fianza.Paciente = dsNombre.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                            }
                        }
                    }

                    if (ds.Tables[0].Rows[0].ItemArray.ElementAt(3) != DBNull.Value)
                        fianza.MontoACobrar = decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(3).ToString());

                    if (ds.Tables[0].Rows[0].ItemArray.ElementAt(4) != DBNull.Value)
                        fianza.MontoAbonado = decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(4).ToString());

                    if (ds.Tables[0].Rows[0].ItemArray.ElementAt(5) != DBNull.Value)
                        fianza.MontoReintegro = decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(5).ToString());

                    if (ds.Tables[0].Rows[0].ItemArray.ElementAt(6) != DBNull.Value)
                        fianza.MontoNotasCred = decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(6).ToString());

                    if (ds.Tables[0].Rows[0].ItemArray.ElementAt(7) != DBNull.Value)
                        fianza.MontoNotasDeb = decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(7).ToString());

                    fianza.MontoNeto = fianza.MontoACobrar - fianza.MontoReintegro - fianza.MontoNotasCred - fianza.MontoAbonado + fianza.MontoNotasDeb;
                    fianzas.Add(fianza);
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
