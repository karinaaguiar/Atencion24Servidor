﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Globalization;
using Atencion24WebServices.Atencion24DAO;

namespace Atencion24WebServices.Atencion24Negocio
{
    public class ListadoCasos
    {
        private ArrayList casos;
        private string medico;
        private bool sinCasos = false;
        private string apellido;

        ///Constructor
        public ListadoCasos(string codMedico, string apellido)
        {
            medico = codMedico;
            this.apellido = apellido;
        }

        public ListadoCasos() { }

        public bool SinCasos
        {
            get { return sinCasos; }
            set { sinCasos = value; }
        }
        public ArrayList Casos
        {
            get { return casos; }
            set { casos = value; }
        }

        //Consultar Listado de casos
        public void ConsultarListadoDeCasos()
        {
            DataSet ds = new DataSet();
            ListadoCasosDAO ud = new ListadoCasosDAO();
            Caso caso;

            //Listado de casos
            ds = ud.DetalleCasoListadoDeCasos(medico, apellido);

            //Este médico no atendió a ningún paciente con este apellido
            if (ds.Tables[0].Rows.Count == 0) { sinCasos = true; return; }
            else
            {
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) == DBNull.Value)
                {
                    sinCasos = true;
                    return;
                }
                else
                {
                    casos = new ArrayList();// [ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        caso = new Caso();
                        DataRow row = ds.Tables[0].Rows[i];
                        
                        //Número de caso 
                        caso.NroCaso = row.ItemArray.ElementAt(0).ToString();
                        
                        //Unidad de negocio  
                        caso.UnidadNegocio = row.ItemArray.ElementAt(1).ToString();
                        
                        //Nombre del paciente
                        caso.NombrePaciente = row.ItemArray.ElementAt(2).ToString();
                        
                        //Fecha emisión factura 
                        caso.FechaEmision = row.ItemArray.ElementAt(3).ToString();

                        caso.Simple = true;
                        casos.Add(caso);
                    }
                }
            }
        }

    }
}