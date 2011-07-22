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
        private String nombrePaciente;
	    private String fechaEmisionFactura;
	    private String nroCaso;
	    private String unidadNegocio;
	    private String ciPaciente;
	    private String responsablePago;
	    private decimal montoFacturado;
	    private decimal montoExonerado;
	    private decimal montoAbonado;
	    private decimal totalDeuda;
	    private ArrayList honorarios;
        private bool simple = false;
        
        ///Constructor
        public Caso() { }

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

        public String CiPaciente
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
        
       
    }
}
