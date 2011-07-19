using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Atencion24WebServices.Atencion24DAO;

namespace Atencion24WebServices.Atencion24Negocio
{
    public class EstadoDeCuenta
    {
        private string medico;
		private decimal montoTotal;
        private decimal montoA30Dias;
        private decimal montoA60Dias;
        private decimal montoA90Dias;
        private decimal montoA180Dias;
        private decimal montoAMas180Dias;
        private bool sindeuda = false;

        ///Constructor
        public EstadoDeCuenta(string codMedico)
        {
            medico = codMedico;
        }

        //Getter y Setters
        public decimal MontoTotal
        {
            get { return montoTotal; }
            set { montoTotal = value; }
        }

        public decimal MontoA30Dias
        {
            get { return montoA30Dias; }
            set { montoA30Dias = value; }
        }

        public decimal MontoA60Dias
        {
            get { return montoA60Dias; }
            set { montoA60Dias = value; }
        }

        public decimal MontoA90Dias
        {
            get { return montoA90Dias; }
            set { montoA90Dias = value; }
        }

        public decimal MontoA180Dias
        {
            get { return montoA180Dias; }
            set { montoA180Dias = value; }
        }

        public decimal MontoAMas180Dias
        {
            get { return montoAMas180Dias; }
            set { montoAMas180Dias = value; }
        }
        public bool sinDeuda
        {
            get { return sindeuda; }
            set { sindeuda = value; }
        }

        //Funcion Auxiliar que permite calcular el estado de cuenta (deuda) por antiguedad de saldo
        public decimal auxiliarConsultarEstadoDeCuentaAS(EstadoDeCuentaDAO ud, int antiguedad)
        {
            DataSet ds = new DataSet();
            decimal monto_facturado = 0;
            decimal monto_notas_Cd = 0;
            decimal monto_notas_Deb = 0;
            decimal monto_pagado = 0;
            decimal total = 0; 
            String monto; 
            
            ds = ud.EdoCtaMontoFacturadoAntiguedad(medico, antiguedad);

            //Este médico no tiene cuentas por pagar que estén pendientes por pagar a 30dias
            if (ds.Tables[0].Rows.Count == 0) { total = 0; return total; }
            else
            {
                monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                if (monto.Equals("null") || monto.Equals("NULL"))
                {
                    total = 0;
                    return total;
                }
                else
                {
                    monto_facturado = decimal.Parse(monto);
                    System.Diagnostics.Debug.WriteLine("monto_facturado " + monto_facturado);

                    //MONTO TOTAL DE LA DEUDA. MONTO notas credito
                    ds = ud.EdoCtaMontoNCredAntiguedad(medico, antiguedad);

                    if (ds.Tables[0].Rows.Count == 0)
                        monto_notas_Cd = 0;
                    else
                    {
                        monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                        if (monto.Equals("null") || monto.Equals("NULL"))
                            monto_notas_Cd = 0;
                        else
                            monto_notas_Cd = decimal.Parse(monto);
                    }
                    System.Diagnostics.Debug.WriteLine("monto_notas_Cd " + monto_notas_Cd);

                    //MONTO TOTAL DE LA DEUDA. MONTO notas debito
                    ds = ud.EdoCtaMontoNDebAntiguedad(medico, antiguedad);

                    if (ds.Tables[0].Rows.Count == 0)
                        monto_notas_Deb = 0;
                    else
                    {
                        monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                        if (monto.Equals("null") || monto.Equals("NULL"))
                            monto_notas_Deb = 0;
                        else
                            monto_notas_Deb = decimal.Parse(monto);
                    }
                    System.Diagnostics.Debug.WriteLine("monto_notas_Deb " + monto_notas_Deb);

                    //MONTO TOTAL DE LA DEUDA. MONTO pagado
                    ds = ud.EdoCtaMontoPagadoAntiguedad(medico, antiguedad);

                    if (ds.Tables[0].Rows.Count == 0)
                        monto_pagado = 0;
                    else
                    {
                        monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                        if (monto.Equals("null") || monto.Equals("NULL"))
                            monto_pagado = 0;
                        else
                            monto_pagado = decimal.Parse(monto);
                    }
                    System.Diagnostics.Debug.WriteLine("monto_pagado " + monto_pagado);

                    total = monto_facturado - (monto_notas_Cd - monto_notas_Deb) - monto_pagado;
                    System.Diagnostics.Debug.WriteLine("total " + total);
                    return total;
                }
            }
        }

		//Consultar Estado de cuenta por Antiguedad saldo
        public void ConsultarEstadoDeCuentaAS()
        {
            DataSet ds = new DataSet();
            EstadoDeCuentaDAO ud = new EstadoDeCuentaDAO();
            decimal monto_facturado = 0;
            decimal monto_notas_Cd = 0;
            decimal monto_notas_Deb = 0;
            decimal monto_pagado = 0;
            String monto; 
            

            //**MONTO TOTAL DE LA DEUDA**
           
            //MONTO TOTAL DE LA DEUDA. MONTO facturado
            ds = ud.EdoCtaMontoFacturadoTotal(medico);

            //Este médico no tiene cuentas por pagar que estén pendientes por pagar
            if (ds.Tables[0].Rows.Count == 0) { sindeuda = true; return; }
            else
            {
                monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                if (monto.Equals("null") || monto.Equals("NULL"))
                {
                    sindeuda = true;
                    return;
                }
                else
                {
                    monto_facturado = decimal.Parse(monto);
                    System.Diagnostics.Debug.WriteLine("monto_facturado " + monto_facturado);

                    //MONTO TOTAL DE LA DEUDA. MONTO notas credito
                    ds = ud.EdoCtaMontoNCredTotal(medico);
                    
                    if (ds.Tables[0].Rows.Count == 0)
                        monto_notas_Cd = 0;
                    else
                    {
                        monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                        if (monto.Equals("null") || monto.Equals("NULL"))
                            monto_notas_Cd = 0;
                        else
                            monto_notas_Cd = decimal.Parse(monto);
                    }
                    System.Diagnostics.Debug.WriteLine("monto_notas_Cd " + monto_notas_Cd);
                    //MONTO TOTAL DE LA DEUDA. MONTO notas debito
                    ds = ud.EdoCtaMontoNDebTotal(medico);

                    if (ds.Tables[0].Rows.Count == 0)
                        monto_notas_Deb = 0;
                    else
                    {
                        monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                        if (monto.Equals("null") || monto.Equals("NULL"))
                            monto_notas_Deb = 0;
                        else
                            monto_notas_Deb = decimal.Parse(monto);
                    }
                    System.Diagnostics.Debug.WriteLine("monto_notas_Deb " + monto_notas_Deb);
                    //MONTO TOTAL DE LA DEUDA. MONTO pagado
                    ds = ud.EdoCtaMontoPagadoTotal(medico);

                    if (ds.Tables[0].Rows.Count == 0)
                        monto_pagado = 0;
                    else
                    {
                        monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                        if (monto.Equals("null") || monto.Equals("NULL"))
                            monto_pagado = 0;
                        else
                            monto_pagado = decimal.Parse(monto);
                    }
                    System.Diagnostics.Debug.WriteLine("monto_pagado " + monto_pagado);
                    montoTotal = monto_facturado - (monto_notas_Cd - monto_notas_Deb) - monto_pagado;
                    System.Diagnostics.Debug.WriteLine("montoTotal " + montoTotal);
                    /*montoA30Dias = auxiliarConsultarEstadoDeCuentaAS(ud,30);
                    montoA60Dias = auxiliarConsultarEstadoDeCuentaAS(ud,60);
                    montoA90Dias = auxiliarConsultarEstadoDeCuentaAS(ud,90);
                    montoA180Dias = auxiliarConsultarEstadoDeCuentaAS(ud,180);
                    montoAMas180Dias = auxiliarConsultarEstadoDeCuentaAS(ud,181);*/
                }
            }
            ud.CerrarConexionBd();
        }
    }
}
