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
		private decimal montoTotal = 0;
        private decimal montoA30Dias = 0;
        private decimal montoA60Dias = 0;
        private decimal montoA90Dias = 0;
        private decimal montoA180Dias = 0;
        private decimal montoA360Dias = 0;
        private decimal montoA720Dias = 0; 
        private decimal montoAMas720Dias = 0;
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

        public decimal MontoA360Dias
        {
            get { return montoA360Dias; }
            set { montoA360Dias = value; }
        }

        public decimal MontoA720Dias
        {
            get { return montoA720Dias; }
            set { montoA720Dias = value; }
        }

        public decimal MontoAMas720Dias
        {
            get { return montoAMas720Dias; }
            set { montoAMas720Dias = value; }
        }
        public bool sinDeuda
        {
            get { return sindeuda; }
            set { sindeuda = value; }
        }

        /// <summary>
        /// Funcion Auxiliar que permite calcular el estado de cuenta (deuda) por antiguedad de saldo
        /// </summary>
        /// <param name="antiguedad">entero que indica la antiguedad a la cual se quiere consultar 
        /// el saldo</param>
        /// <returns>deuda según lo facturado en la antiguedad indicada</returns>

        public decimal auxiliarConsultarEstadoDeCuentaAS(int antiguedad)
        {
            DataSet ds = new DataSet();
            EstadoDeCuentaDAO ud = new EstadoDeCuentaDAO();
            decimal total = 0;

            ds = ud.EdoCtaMontoAntiguedad(medico, antiguedad);

            //Este médico no tiene cuentas por pagar que estén pendientes por pagar a la antiguedad
            if (ds.Tables[0].Rows.Count != 0) 
            {
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                {
                    if (decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString()) != 0)
                    {
                        total = decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString());
                    }
                }
            }
            return total; 
        }

        /// <summary>
        /// Consultar Estado de cuenta por Antiguedad saldo
        /// </summary>
        public void ConsultarEstadoDeCuentaAS()
        {
            DataSet ds = new DataSet();
            EstadoDeCuentaDAO ud = new EstadoDeCuentaDAO();

            //**MONTO TOTAL DE LA DEUDA**
            ds = ud.EdoCtaMontoTotal(medico);

            //Este médico no tiene cuentas por pagar que estén pendientes por pagar
            if (ds.Tables[0].Rows.Count == 0) { sindeuda = true; return; }
            else
            {
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                {
                    if (!ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString().Equals("0,00"))
                    {
                        montoTotal = decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString());
                        montoA30Dias = auxiliarConsultarEstadoDeCuentaAS(30);
                        montoA60Dias = auxiliarConsultarEstadoDeCuentaAS(60);
                        montoA90Dias = auxiliarConsultarEstadoDeCuentaAS(90);
                        montoA180Dias = auxiliarConsultarEstadoDeCuentaAS(180);
                        montoA360Dias = auxiliarConsultarEstadoDeCuentaAS(360);
                        montoA720Dias = auxiliarConsultarEstadoDeCuentaAS(720);
                        montoAMas720Dias = auxiliarConsultarEstadoDeCuentaAS(721);
                    }
                    else
                    {
                        sindeuda = true; return; 
                    }
                }
                else
                {
                    sindeuda = true; return; 
                }
            }
        }

        /* VERSION QUE NO! CONSULTA TABLA TEMP_ESTADO_CUENTA
        /// <summary>
        /// Funcion Auxiliar que permite calcular el estado de cuenta (deuda) por antiguedad de saldo
        /// </summary>
        /// <param name="antiguedad">entero que indica la antiguedad a la cual se quiere consultar 
        /// el saldo</param>
        /// <returns>deuda según lo facturado en la antiguedad indicada</returns>
        
        public decimal auxiliarConsultarEstadoDeCuentaAS(int antiguedad)
        {
            DataSet ds = new DataSet();
            EstadoDeCuentaDAO ud = new EstadoDeCuentaDAO();
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
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) == DBNull.Value)
                {
                    total = 0;
                    return total;
                }
                else
                {
                    monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                    monto_facturado = decimal.Parse(monto);
                    
                    //MONTO POR ANTIGUEDAD DE LA DEUDA. MONTO notas credito
                    ud = new EstadoDeCuentaDAO();
                    ds = ud.EdoCtaMontoNCredAntiguedad(medico, antiguedad);

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                        {
                            monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                            monto_notas_Cd = decimal.Parse(monto);
                        }
                    }

                    //MONTO POR ANTIGUEDAD DE LA DEUDA. MONTO notas debito
                    ud = new EstadoDeCuentaDAO();
                    ds = ud.EdoCtaMontoNDebAntiguedad(medico, antiguedad);

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                        {
                            monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                            monto_notas_Deb = decimal.Parse(monto);
                        }
                    }

                    //MONTO POR ANTIGUEDAD DE LA DEUDA. MONTO pagado
                    ud = new EstadoDeCuentaDAO();
                    ds = ud.EdoCtaMontoPagadoAntiguedad(medico, antiguedad);

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                        {
                            monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                            monto_pagado = decimal.Parse(monto);
                        }
                    }

                    //MONTO POR ANTIGUEDAD DE LA DEUDA. Total Deuda
                    total = monto_facturado - (monto_notas_Cd - monto_notas_Deb) - monto_pagado;
                    return total;
                }
            }
        }
    
        /// <summary>
        /// Consultar Estado de cuenta por Antiguedad saldo
        /// </summary>
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
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) == DBNull.Value)
                {
                    sindeuda = true;
                    return;
                }
                else
                {
                    monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                    monto_facturado = decimal.Parse(monto);
                    
                    //MONTO TOTAL DE LA DEUDA. MONTO notas credito
                    ud = new EstadoDeCuentaDAO();
                    ds = ud.EdoCtaMontoNCredTotal(medico);
                    
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                        {
                            monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                            monto_notas_Cd = decimal.Parse(monto);
                        }
                    }
                    
                    //MONTO TOTAL DE LA DEUDA. MONTO notas debito
                    ud = new EstadoDeCuentaDAO();
                    ds = ud.EdoCtaMontoNDebTotal(medico);

                    if (ds.Tables[0].Rows.Count != 0) 
                    { 
                        if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value )
                        {
                            monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                            monto_notas_Deb = decimal.Parse(monto);
                        }
                    }
                    
                    //MONTO TOTAL DE LA DEUDA. MONTO pagado
                    ud = new EstadoDeCuentaDAO();
                    ds = ud.EdoCtaMontoPagadoTotal(medico);

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                        {
                            monto = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                            monto_pagado = decimal.Parse(monto);
                        }
                    }

                    montoTotal = monto_facturado - (monto_notas_Cd - monto_notas_Deb) - monto_pagado;
                    montoA30Dias = auxiliarConsultarEstadoDeCuentaAS(30); 
                    montoA60Dias = auxiliarConsultarEstadoDeCuentaAS(60);
                    montoA90Dias = auxiliarConsultarEstadoDeCuentaAS(90);
                    montoA180Dias = auxiliarConsultarEstadoDeCuentaAS(180);
                    montoA360Dias = auxiliarConsultarEstadoDeCuentaAS(360);
                    montoAMas360Dias = auxiliarConsultarEstadoDeCuentaAS(361);
                }
            }
        }
        */
   }
}
