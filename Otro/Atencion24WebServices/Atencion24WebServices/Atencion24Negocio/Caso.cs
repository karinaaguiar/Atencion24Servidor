using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Atencion24WebServices.Atencion24DAO;

namespace Atencion24WebServices.Atencion24Negocio
{
    public class Caso
    {
        private String medico;
        private String nombrePaciente=" ";
	    private String fechaEmisionFactura=" ";
	    private String nroCaso=" ";
	    private String unidadNegocio=" ";
        private decimal ciPaciente = 0;
        private String responsablePago = " ";
	    private decimal montoFacturado = 0;
	    private decimal montoExonerado = 0;
        private decimal montoAbonado = 0;
        private decimal totalDeuda = 0;
	    private ArrayList honorarios = null;
        private bool simple = false;
        
        ///Constructor
        public Caso() { }

        public Caso(String medico, String caso_tb, String udn_tb)
        {
            this.medico = medico;
            this.nroCaso = caso_tb;
            this.unidadNegocio = udn_tb;
        }

        //Getter y Setters
        public String NombrePaciente
        {
            get { return nombrePaciente; }
            set { nombrePaciente = value; }
        }

        public String FechaEmision
        {
            get { return fechaEmisionFactura; }
            set { fechaEmisionFactura = value; }
        }

        public String NroCaso
        {
            get { return nroCaso; }
            set { nroCaso = value; }
        }

        public String UnidadNegocio
        {
            get { return unidadNegocio; }
            set { unidadNegocio = value; }
        }

        public decimal CiPaciente
        {
            get { return ciPaciente; }
            set { ciPaciente = value; }
        }

        public String ResponsablePago
        {
            get { return responsablePago; }
            set { responsablePago = value; }
        }

        public decimal MontoFacturado
        {
            get { return montoFacturado; }
            set { montoFacturado = value; }
        }

        public decimal MontoExonerado
        {
            get { return montoExonerado; }
            set { montoExonerado = value; }
        }

        public decimal MontoAbonado
        {
            get { return montoAbonado; }
            set { montoAbonado = value; }
        }

        public decimal TotalDeuda
        {
            get { return totalDeuda; }
            set { totalDeuda = value; }
        }

        public ArrayList Honorarios
        {
            get { return honorarios; }
            set { honorarios = value; }
        }

        public bool Simple
        {
            get { return simple; }
            set { simple = value; }
        }
        
        //Consultar Detalle de un caso
        public void ConsultarDetalleDeCaso()
        {
            DataSet ds = new DataSet();
            CasoDAO ud = new CasoDAO();
            String codigoResp;
            String tipoResp;

            //Monto Facturado en el caso
            ds = ud.DetalleDeCasoTotalFacturado(medico, nroCaso, unidadNegocio);

            if (ds.Tables[0].Rows.Count != 0) 
            {
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                 montoFacturado = decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString());
            }
            
            //Monto Notas de Crédito
            ud = new CasoDAO();
            ds = ud.DetalleDeCasoTotalNotasCred(medico, nroCaso, unidadNegocio);

            if (ds.Tables[0].Rows.Count != 0) 
            {
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                 montoExonerado = decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString());
            }

            //Monto Notas de Débito
            ud = new CasoDAO();
            ds = ud.DetalleDeCasoTotalNotasDeb(medico, nroCaso, unidadNegocio);

            if (ds.Tables[0].Rows.Count != 0) 
            {
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                 montoExonerado -= decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString());
            }
            
            //Monto Abonado
            ud = new CasoDAO();
            ds = ud.DetalleDeCasoTotalAbonado(medico, nroCaso, unidadNegocio);

            if (ds.Tables[0].Rows.Count != 0) 
            {
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                 montoAbonado = decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString());
            }

            totalDeuda = montoFacturado - montoExonerado - montoAbonado;

            //Nombre y Cédula del paciente
            ud = new CasoDAO();
            ds = ud.DetalleDeCasoNombreyCedulaPaciente(nroCaso, unidadNegocio);

            if (ds.Tables[0].Rows.Count != 0) 
            {
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                    nombrePaciente = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();

                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(1) != DBNull.Value)
                    ciPaciente = decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(1).ToString());

                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(2) != DBNull.Value)
                    fechaEmisionFactura = ds.Tables[0].Rows[0].ItemArray.ElementAt(2).ToString();
                     
            }

            //Principal responsable de pago
            ud = new CasoDAO();
            ds = ud.DetalleDeCasoPpalResponsable(nroCaso, unidadNegocio);

            if (ds.Tables[0].Rows.Count != 0) 
            {
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                {
                    codigoResp = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString().Trim();
                    if (codigoResp.Equals("120"))
                        responsablePago = "Particular";
                    else 
                    {
                        if(codigoResp.Equals("118"))
                            responsablePago = "Fianza médica";
                        else
                        {
                            //Buscar tipo responsable
                            ud = new CasoDAO();
                            ds = ud.DetalleDeCasoTipoDeResponsable(nroCaso, unidadNegocio, codigoResp);
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                                {
                                    tipoResp = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString().Trim();

                                    //Nombre del responsable
                                    ud = new CasoDAO();
                                    ds = ud.DetalleDeCasoNombreResponsable(codigoResp, tipoResp);
                                    if (ds.Tables[0].Rows.Count != 0)
                                    {
                                        if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                                            responsablePago = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString().Trim();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Honorarios prestados en el caso 
            ud = new CasoDAO();
            ds = ud.DetalleDeCasoListadoHonorarios(medico, nroCaso, unidadNegocio);

            if (ds.Tables[0].Rows.Count == 0) { honorarios = null; }
            else
            {
                
                honorarios = new ArrayList();
                String servicio;
                String suministro;
                String area;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    if ((dr.ItemArray.ElementAt(0) != DBNull.Value) &&
                        (dr.ItemArray.ElementAt(1) != DBNull.Value) &&
                        (dr.ItemArray.ElementAt(2) != DBNull.Value))
                    {
                        Honorario honorario = new Honorario();
                        suministro = dr.ItemArray.ElementAt(0).ToString();
                        area = dr.ItemArray.ElementAt(1).ToString();
                        servicio = dr.ItemArray.ElementAt(2).ToString();

                        //Nombre Honorario
                        ud = new CasoDAO();
                        ds = ud.DetalleDeCasoNombreHonorario(suministro, area);

                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                                honorario.Nombre = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                        }

                        //Monto Facturado por honorario
                        ud = new CasoDAO();
                        ds = ud.DetalleDeCasoFacturadoHonorario(medico, nroCaso, unidadNegocio, servicio, suministro, area);

                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                                honorario.MontoFacturado = decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString());
                        }

                        //Monto NotasCred por honorario
                        ud = new CasoDAO();
                        ds = ud.DetalleDeCasoNotasCredHonorario(medico, nroCaso, unidadNegocio, servicio, suministro, area);

                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                                honorario.MontoExonerado = decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString());
                        }

                        //Monto NotasDeb por honorario
                        ud = new CasoDAO();
                        ds = ud.DetalleDeCasoNotasDebHonorario(medico, nroCaso, unidadNegocio, servicio, suministro, area);

                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                                honorario.MontoExonerado -= decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString());
                        }

                        //Monto Pagado por honorario
                        ud = new CasoDAO();
                        ds = ud.DetalleDeCasoPagadoPorHonorario(medico, nroCaso, unidadNegocio, servicio, suministro, area);

                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                                honorario.MontoAbonado = decimal.Parse(ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString());
                        }

                        honorario.TotalDeuda = honorario.MontoFacturado - honorario.MontoExonerado - honorario.MontoAbonado;
                        honorarios.Add(honorario);
                    }
                }
            }         
        }

       
    }
}
