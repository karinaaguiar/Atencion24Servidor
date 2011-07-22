using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atencion24WebServices.Atencion24Negocio;

/*using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SiteCentral_V5.Negocio;*/

namespace Atencion24WebServices.Atencion24DAO
{
    public class QueryAtencion24
    {
        private string _query;

        //Get y Set de los atributos de la clase
        public string Query
        {
            get { return _query; }
            set { _query = value; }
        }

        //Constructor
        public QueryAtencion24()
        {
        }

        //**INICIO SESIÓN**//
        //Query inicio de sesión. Para verificar si el nombre de usuario ingresado existe
        public string ExisteUsuario(Usuario user)
        {
            Query = "Select NOMBREUSUARIO From USUARIO where NOMBREUSUARIO = '" + user.Login + "'";
            return Query;
        }
        
        //Query inicio de sesión. Retorna los datos del usuario que intenta ingresar al sistema.
        public string ConsultarUsuario(Usuario user)
        {
            Query = "Select NOMBRE, APELLIDO, CODIGOMEDICO, NOMBREUSUARIO From USUARIO where NOMBREUSUARIO = '" + user.Login + "' and CLAVE ='" + user.Password + "'";
            return Query;
        }

        //**ESTADO DE CUENTA**//
        //MONTOS TOTALES//
        //Consultar TOTAL Facturado
        public string EdoCtaMontoFacturadoTotal(string medico)
        {
            Query = "SELECT SUM(MONTOAPAGAR)FROM TBL_CUENTASPORPAGAR " + 
                    "WHERE PROVEEDOR= '" +medico+ "' AND PAGADO = 0";
            return Query;
        }

        //Consultar TOTAL Notas Credito
        public string EdoCtaMontoNCredTotal(string medico)
        {
            Query = "SELECT SUM(B.MONTOTOTAL) "+
                    "FROM TBL_NOTADECREDITOYDEBITO A INNER JOIN TBL_NOTADECREDITOYDEBITODETALLE B "+ 
                    "ON A.NRONOTA = B.NRONOTA "+
                    "INNER JOIN TBL_CUENTASPORPAGAR C "+
                    "ON A.NROID = C.NROID AND A.UNIDADDENEGOCIO = C.UNIDADDENEGOCIO AND B.CODIGOTIPOP = C.PROVEEDOR AND "+
                    "B.SERVICIO = C.CLASIFICACIONHONORARIO AND B.SUMINISTRO = C.TIPOHONORARIO AND B.AREA = C.AREAHONORARIO "+
                    "WHERE A.TIPONOTA = 1 AND C.PROVEEDOR= '" + medico + "' AND C.PAGADO = 0 ";
            return Query;
        }

        //Consultar TOTAL Notas Débito
        public string EdoCtaMontoNDebTotal(string medico)
        {
            Query = "SELECT SUM(B.MONTOTOTAL) " +
                    "FROM TBL_NOTADECREDITOYDEBITO A INNER JOIN TBL_NOTADECREDITOYDEBITODETALLE B " +
                    "ON A.NRONOTA = B.NRONOTA " +
                    "INNER JOIN TBL_CUENTASPORPAGAR C " +
                    "ON A.NROID = C.NROID AND A.UNIDADDENEGOCIO = C.UNIDADDENEGOCIO AND B.CODIGOTIPOP = C.PROVEEDOR AND " +
                    "B.SERVICIO = C.CLASIFICACIONHONORARIO AND B.SUMINISTRO = C.TIPOHONORARIO AND B.AREA = C.AREAHONORARIO " +
                    "WHERE A.TIPONOTA = 2 AND C.PROVEEDOR= '" + medico + "' AND C.PAGADO = 0 ";
            return Query;
        }

        //Consultar TOTAL Pagado
        public string EdoCtaMontoPagadoTotal(string medico)
        {
            Query = "SELECT SUM(A.MONTO) " +
                    "FROM TBL_HPAGOSHONORARIOS A INNER JOIN TBL_CUENTASPORPAGAR B " +
                    "ON  A.UNIDADDENEGOCIO = B.UNIDADDENEGOCIO AND A.CASO = B.NROID AND A.SERVICIO = B.CLASIFICACIONHONORARIO AND " +
                    "A.SUMINISTRO = B.TIPOHONORARIO AND A.AREA = B.AREAHONORARIO AND A.MEDICO = B.PROVEEDOR " +
                    "WHERE B.PROVEEDOR= '" + medico + "' AND B.PAGADO = 0 AND A.TIPO = 1 AND A.CONCEPTO = 1 AND A.APAGAR =1 ";
            return Query;
        }

        //MONTOS POR ANTIGUEDAD//
        public string antiguedadSaldo(string Query, int antiguedad)
        {
            if (antiguedad == 30)
                Query = Query + "B.FECHAEMISION >= DATEADD(day, -30, GETDATE()) and B.FECHAEMISION <= GETDATE()";
            if (antiguedad == 60)
                Query = Query + "B.FECHAEMISION >= DATEADD(day, -60, GETDATE()) and B.FECHAEMISION < DATEADD(day, -30, GETDATE())";
            if (antiguedad == 90)
                Query = Query + "B.FECHAEMISION >= DATEADD(day, -90, GETDATE()) and B.FECHAEMISION < DATEADD(day, -60, GETDATE())";
            if (antiguedad == 180)
                Query = Query + "B.FECHAEMISION >= DATEADD(day, -180, GETDATE()) and B.FECHAEMISION < DATEADD(day, -90, GETDATE())";
            if (antiguedad == 181)
                Query = Query + "B.FECHAEMISION < DATEADD(day, -180, GETDATE())";

            return Query;
        }

        //Consultar Facturado por Antiguedad
        public string EdoCtaMontoFacturadoAntiguedad(string medico, int antiguedad)
        {
            Query = "SELECT SUM(A.MONTOAPAGAR) " +
                    "FROM TBL_CUENTASPORPAGAR A INNER JOIN TBL_HCASO B " +
                    "ON A.NROID = B.CASO AND A.UNIDADDENEGOCIO = B.UNIDADNEGOCIO " +
                    "WHERE A.PROVEEDOR= '" + medico + "' AND A.PAGADO = 0 AND ";

            Query = antiguedadSaldo(Query, antiguedad);  
            return Query;
        }

        //Consultar Notas de Crédito por Antiguedad 
        public string EdoCtaMontoNCredAntiguedad(string medico, int antiguedad)
        {
            Query = "SELECT SUM(C.MONTOTOTAL) " +
                    "FROM TBL_NOTADECREDITOYDEBITO A INNER JOIN TBL_NOTADECREDITOYDEBITODETALLE C " +
                    "ON A.NRONOTA = C.NRONOTA " +
                    "INNER JOIN TBL_CUENTASPORPAGAR D " +
                    "ON A.NROID = D.NROID AND A.UNIDADDENEGOCIO = D.UNIDADDENEGOCIO AND C.CODIGOTIPOP = D.PROVEEDOR AND " +
                    "C.SERVICIO = D.CLASIFICACIONHONORARIO AND C.SUMINISTRO = D.TIPOHONORARIO AND C.AREA = D.AREAHONORARIO " +
                    "INNER JOIN  TBL_HCASO B " +
                    "ON D.NROID = B.CASO AND D.UNIDADDENEGOCIO = B.UNIDADNEGOCIO " +
                    "WHERE A.TIPONOTA = 1 AND D.PROVEEDOR= '" + medico + "' AND D.PAGADO = 0 AND ";

            Query = antiguedadSaldo(Query, antiguedad);  
            return Query;
        }

        //Consultar Notas de Débito por Antiguedad 
        public string EdoCtaMontoNDebAntiguedad(string medico, int antiguedad)
        {
            Query = "SELECT SUM(C.MONTOTOTAL) " +
                    "FROM TBL_NOTADECREDITOYDEBITO A INNER JOIN TBL_NOTADECREDITOYDEBITODETALLE C " +
                    "ON A.NRONOTA = C.NRONOTA " +
                    "INNER JOIN TBL_CUENTASPORPAGAR D " +
                    "ON A.NROID = D.NROID AND A.UNIDADDENEGOCIO = D.UNIDADDENEGOCIO AND C.CODIGOTIPOP = D.PROVEEDOR AND " +
                    "C.SERVICIO = D.CLASIFICACIONHONORARIO AND C.SUMINISTRO = D.TIPOHONORARIO AND C.AREA = D.AREAHONORARIO " +
                    "INNER JOIN  TBL_HCASO B " +
                    "ON D.NROID = B.CASO AND D.UNIDADDENEGOCIO = B.UNIDADNEGOCIO " +
                    "WHERE A.TIPONOTA = 2 AND D.PROVEEDOR= '" + medico + "' AND D.PAGADO = 0 AND ";

            Query = antiguedadSaldo(Query, antiguedad);  
            return Query;
        }

        //Consultar Pagado por Antiguedad 
        public string EdoCtaMontoPagadoAntiguedad(string medico, int antiguedad)
        {
            Query = "SELECT SUM(A.MONTO) " +
                    "FROM TBL_HPAGOSHONORARIOS A INNER JOIN TBL_CUENTASPORPAGAR C " +
                    "ON  A.UNIDADDENEGOCIO = C.UNIDADDENEGOCIO AND A.CASO = C.NROID AND A.SERVICIO = C.CLASIFICACIONHONORARIO AND " +
                    "A.SUMINISTRO = C.TIPOHONORARIO AND A.AREA = C.AREAHONORARIO AND A.MEDICO = C.PROVEEDOR " +
                    "INNER JOIN TBL_HCASO B " +
                    "ON C.NROID = B.CASO AND C.UNIDADDENEGOCIO = B.UNIDADNEGOCIO " +
                    "WHERE C.PROVEEDOR= '" + medico + "' AND C.PAGADO = 0 AND A.TIPO = 1 AND A.CONCEPTO = 1 AND A.APAGAR =1 AND ";
            
            Query = antiguedadSaldo(Query, antiguedad);  
            return Query;
        }

        //**HONORARIOS PAGADOS**//
        //**PRÓXIMO PAGO**

        //Monto liberado
        public string ProximoPagoMontoLiberado(string medico)
        {
            Query = "SELECT SUM(MONTO) "+
                    "FROM TBL_PAGOSHONORARIOS "+
                    "WHERE MEDICO = '" + medico + "' AND CONCEPTO = 1 AND TIPO = 1 ";
            return Query;
        }

        //Deducciones
        public string ProximoPagoDeducciones(string medico)
        {
            Query = "SELECT CONCEPTO,SUM(MONTO) "+
                    "FROM TBL_PAGOSHONORARIOS "+
                    "WHERE MEDICO = '" + medico + "' AND TIPO = -1 " +
                    "GROUP BY CONCEPTO";
            return Query;
        }
    
        //Nombre del Concepto por Deducción 
        public string NombreConcepto(string codigoConcepto)
        {
            Query = "SELECT NOMBRE "+
                    "FROM TBL_CONCEPTODEPAGOS "+
                    "WHERE CODIGO = '"+codigoConcepto+"'";
            return Query;
        }

        //**HISTORICO DE PAGOS**

        //Fecha del pago y Monto liberado
        public string HistoricoPagosFechayMontoLiberado(string medico, string fechaI, string fechaF)
        {
            Query = "SELECT CONVERT(VARCHAR(10),FEC_ENVIADOAPAGAR,103), SUM(MONTO) " +
                    "FROM TBL_HPAGOSHONORARIOS " +
                    "WHERE FEC_ENVIADOAPAGAR >= '"+ fechaI +"' AND " +
                    "FEC_ENVIADOAPAGAR <= '" + fechaF + "' AND CONCEPTO = 1 AND TIPO = 1 AND APAGAR = 1 " + 
                    "AND MEDICO = '" + medico + "' " +
                    "GROUP BY FEC_ENVIADOAPAGAR " +
                    "ORDER BY FEC_ENVIADOAPAGAR DESC"; 
            return Query;
        }

        //Deducciones 
        public string HistoricoPagosDeducciones(string medico, string fechapago)
        {
            Query = "SELECT CONCEPTO, SUM(MONTO) " +
                    "FROM TBL_HPAGOSHONORARIOS " +
                    "WHERE CONVERT(VARCHAR(10),FEC_ENVIADOAPAGAR,103) = '" + fechapago + "' AND " +
                    "TIPO = -1 AND APAGAR = 1 AND MEDICO = '" + medico + "' " +
                    "GROUP BY CONCEPTO";
            return Query;
        }


        //**HONORARIOS FACTURADOS**//
         public string HonorariosFacturadosMontoPorUDN(string medico, string fechaI, string fechaF, string udn)
        {
            Query = "SELECT SUM(A.MONTOAPAGAR) " +
                    "FROM TBL_CUENTASPORPAGAR A INNER JOIN TBL_HCASO B " +
                    "ON A.UNIDADDENEGOCIO = B.UNIDADNEGOCIO AND A.NROID = B.CASO " +
                    "WHERE A.PROVEEDOR= '" + medico + "' " +
                    "AND B.FECHAEMISION >= convert(datetime, '" + fechaI + "', 121) " +
                    "AND B.FECHAEMISION <= convert(datetime, '" + fechaF + "', 121) " +
                    "AND A.UNIDADDENEGOCIO  = '" + udn + "'";
            return Query;
        }

        //**DETALLE DE UN CASO**//

        //LISTADO DE CASOS 
         public string DetalleCasoListadoDeCasos(string medico, string apellido)
        {
            Query = "SELECT A.CASO, A.UNIDADNEGOCIO, B.NOMBRE, CONVERT(VARCHAR(10),A.FECHAEMISION,103) " +
                    "FROM TBL_HCASO A INNER JOIN TBL_PACIENTE B ON A.PACIENTE = B.CEDULA " +
                    "WHERE EXISTS " +
                    "(SELECT * FROM TBL_CUENTASPORPAGAR C " +
                    "WHERE C.PROVEEDOR = '" + medico + "' AND A.UNIDADNEGOCIO = C.UNIDADDENEGOCIO AND A.CASO = C.NROID) AND " +
                    "B.NOMBRE LIKE '%" + apellido + "%' " +
                    "ORDER BY A.FECHAEMISION DESC";
            return Query;
        }


        //Consultar TOTAL Estado de cuenta . Consultar TOTAL Monto a Pagar.
        //String o int codMedico?
        /*public string ConsultarTotalMontoAPagar(string codMedico)
        {
            Query = "select sum(MONTOAPAGAR)from CUENTA_POR_PAGAR " + 
                    "where PROVEEDOR= '" + codMedico + "' and PAGADO = 0";
            return Query;
        }

        //Consultar TOTAL Estado de cuenta . Consultar TOTAL Monto Notas Credito.
        //String o int codMedico?
        public string ConsultarTotalMontoNotasCredito(string codMedico)
        {
            Query = "select sum(MONTO)from NOTA_DE_DEBITO_Y_CREDITO A inner join CUENTA_POR_PAGAR B on " + 
                    "A.PROVEEDOR = B.PROVEEDOR and A.CASO = B.CASO and A.SUMINISTRO = B.SUMINISTRO and A.UDN = B.UDN " +
                    "where B.PROVEEDOR= '" + codMedico + "' and PAGADO = 0" +
                    "and TIPONOTA = 1"; 
            return Query;
        }

        //Consultar TOTAL Estado de cuenta . Consultar TOTAL Monto Notas Débito.
        //String o int codMedico?
        public string ConsultarTotalMontoNotasDebito(string codMedico)
        {
            Query = "select sum(MONTO)from NOTA_DE_DEBITO_Y_CREDITO A inner join CUENTA_POR_PAGAR B on " +
                    "A.PROVEEDOR = B.PROVEEDOR and A.CASO = B.CASO and A.SUMINISTRO = B.SUMINISTRO and A.UDN = B.UDN " +
                    "where B.PROVEEDOR= '" + codMedico + "' and PAGADO = 0" +
                    "and TIPONOTA = 2";
            return Query;
        }

        //Consultar TOTAL Estado de cuenta . Consultar TOTAL Monto Pagado.
        //String o int codMedico?
        public string ConsultarTotalMontoPagado(string codMedico)
        {
            Query = "select sum(MONTOPAGO)from PAGO A inner join CUENTA_POR_PAGAR B on " +
                    "A.PROVEEDOR = B.PROVEEDOR and A.CASO = B.CASO and A.SUMINISTRO = B.SUMINISTRO and A.UDN = B.UDN " +
                    "where B.PROVEEDOR= '" + codMedico + "' and PAGADO = 0"; 
            return Query;
        }

        //**ESTADO DE CUENTA**/
        //**POR UNIDAD DE NEGOCIO**//
        //Consultar POR UNIDAD DE NEGOCIO Estado de cuenta . Consultar TOTAL Monto a Pagar.
        //String o int codMedico?
        /*public string ConsultarTotalMontoAPagarUDN(string codMedico)
        {
            Query = "select UDN, sum(MONTOAPAGAR) from CUENTA_POR_PAGAR " +
                    "where PROVEEDOR= '" + codMedico + "' and PAGADO = 0" +
                    "group by UDN";
            return Query;
        }

        //Consultar POR UNIDAD DE NEGOCIO Estado de cuenta . Consultar TOTAL Monto Notas Credito.
        //String o int codMedico?
        public string ConsultarTotalMontoNotasCreditoUDN(string codMedico)
        {
            Query = "select A.UDN, sum(A.MONTO)from NOTA_DE_DEBITO_Y_CREDITO A inner join CUENTA_POR_PAGAR B on " +
                    "A.PROVEEDOR = B.PROVEEDOR and A.CASO = B.CASO and A.SUMINISTRO = B.SUMINISTRO and A.UDN = B.UDN " +
                    "where B.PROVEEDOR= '" + codMedico + "' and B.PAGADO = 0" +
                    "and A.TIPONOTA = 1" +
                    "group by B.UDN";
            return Query;
        }

        //Consultar POR UNIDAD DE NEGOCIO Estado de cuenta . Consultar TOTAL Monto Notas Débito.
        //String o int codMedico?
        public string ConsultarTotalMontoNotasDebitoUDN(string codMedico)
        {
            Query = "select A.UDN, sum(A.MONTO)from NOTA_DE_DEBITO_Y_CREDITO A inner join CUENTA_POR_PAGAR B on " +
                    "A.PROVEEDOR = B.PROVEEDOR and A.CASO = B.CASO and A.SUMINISTRO = B.SUMINISTRO and A.UDN = B.UDN " +
                    "where B.PROVEEDOR= '" + codMedico + "' and B.PAGADO = 0" +
                    "and A.TIPONOTA = 2" +
                    "group by B.UDN";
            return Query;
        }

        //Consultar POR UNIDAD DE NEGOCIO Estado de cuenta . Consultar TOTAL Monto Pagado.
        //String o int codMedico?
        public string ConsultarTotalMontoPagadoUDN(string codMedico)
        {
            Query = "select A.UDN, sum(A.MONTOPAGO)from PAGO A inner join CUENTA_POR_PAGAR B on " +
                    "A.PROVEEDOR = B.PROVEEDOR and A.CASO = B.CASO and A.SUMINISTRO = B.SUMINISTRO and A.UDN = B.UDN " +
                    "where B.PROVEEDOR= '" + codMedico + "' and PAGADO = 0" +
                    "group by B.UDN";
            return Query;
        }

        //**ESTADO DE CUENTA**/
        //**POR ANTIGUEDAD DE SALDO**//
        //Consultar POR ANTIGUEDAD DE SALDO Estado de cuenta . Consultar TOTAL Monto a Pagar.
        //String o int codMedico?
        /*public string antiguedadSaldo(string Query, int antiguedad)
        {
            if (antiguedad == 30)
                Query = Query + "B.FECHAEMISION >= DATEADD(day, -30, GETDATE()) and B.FECHAEMISION <= GETDATE()";
            if (antiguedad == 60)
                Query = Query + "B.FECHAEMISION >= DATEADD(day, -60, GETDATE()) and B.FECHAEMISION < DATEADD(day, -30, GETDATE())";
            if (antiguedad == 90)
                Query = Query + "B.FECHAEMISION >= DATEADD(day, -90, GETDATE()) and B.FECHAEMISION < DATEADD(day, -60, GETDATE())";
            if (antiguedad == 180)
                Query = Query + "B.FECHAEMISION >= DATEADD(day, -180, GETDATE()) and B.FECHAEMISION < DATEADD(day, -90, GETDATE())";
            if (antiguedad == 181)
                Query = Query + "B.FECHAEMISION < DATEADD(day, -180, GETDATE())";

            return Query;
        }
        
        public string ConsultarTotalMontoAPagarAS(string codMedico, int antiguedad)
        {
            Query = "select sum(A.MONTOAPAGAR)from CUENTA_POR_PAGAR A inner join CASO B on " +
                    "A.CASO = B.NROCASO " +   
                    "where A.PROVEEDOR= '" + codMedico + "' and A.PAGADO = 0 and ";

            return antiguedadSaldo(Query, antiguedad);
            
        }

        //Consultar POR ANTIGUEDAD DE SALDO Estado de cuenta . Consultar TOTAL Monto Notas Credito.
        //String o int codMedico?
        public string ConsultarTotalMontoNotasCreditoAS(string codMedico, int antiguedad)
        {
            Query = "select sum(C.MONTO)from CUENTA_POR_PAGAR A join CASO B on " +
                    "A.CASO = B.NROCASO " +
                    "join NOTA_DE_DEBITO_Y_CREDITO C on " +  
                    "A.PROVEEDOR = C.PROVEEDOR and A.CASO = C.CASO and A.SUMINISTRO = C.SUMINISTRO and A.UDN = C.UDN " +
                    "where A.PROVEEDOR= '" + codMedico + "' and A.PAGADO = 0" +
                    "and C.TIPONOTA = 1 and ";

            return antiguedadSaldo(Query, antiguedad);
        }

        //Consultar POR ANTIGUEDAD DE SALDO Estado de cuenta . Consultar TOTAL Monto Notas Débito.
        //String o int codMedico?
        public string ConsultarTotalMontoNotasDebitoAS(string codMedico, int antiguedad)
        {
            Query = "select sum(C.MONTO)from CUENTA_POR_PAGAR A join CASO B on " +
                    "A.CASO = B.NROCASO " +
                    "join NOTA_DE_DEBITO_Y_CREDITO C on " +
                    "A.PROVEEDOR = C.PROVEEDOR and A.CASO = C.CASO and A.SUMINISTRO = C.SUMINISTRO and A.UDN = C.UDN " +
                    "where A.PROVEEDOR= '" + codMedico + "' and A.PAGADO = 0" +
                    "and C.TIPONOTA = 2 and ";

            return antiguedadSaldo(Query, antiguedad);
        }

        //Consultar POR ANTIGUEDAD DE SALDO Estado de cuenta . Consultar TOTAL Monto Pagado.
        //String o int codMedico?
        public string ConsultarTotalMontoPagadoAS(string codMedico, int antiguedad)
        {
            Query = "select sum(C.MONTOPAGO)from CUENTA_POR_PAGAR A join CASO B on " +
                    "A.CASO = B.NROCASO " +
                    "join PAGO C on " +
                    "A.PROVEEDOR = C.PROVEEDOR and A.CASO = C.CASO and A.SUMINISTRO = C.SUMINISTRO and A.UDN = C.UDN " +
                    "where A.PROVEEDOR= '" + codMedico + "' and A.PAGADO = 0 and ";

            return antiguedadSaldo(Query, antiguedad);
        }

        //**HONORARIOS PAGADOS**/
        //PAGO RECIENTE
        //Estoy segura que asi consulto el NRO NOMINA mas reciente ¿TOP1?
        //Que hay de la fecha de pago? Es la misma para todos los pagos que entran en 
        //una nómina?
        /*public string ConsultarUltimoPagoHonorariosMontoLiberado(string codMedico)
        {
            Query = "select top 1 NRONOMINA, sum(MONTOPAGO) from PAGO " +
                    "where PROVEEDOR= '" + codMedico + "' " +
                    "group by NRONOMINA";

            return Query;
        }

        //**HONORARIOS PAGADOS**/
        //PAGO POR RANGO DE FECHAS
        /*public string ConsultarHistoricoPagoHonorariosMontoLiberado(string codMedico, DateTime fecha_inicio, DateTime fecha_fin)
        {
            Query = "select NRONOMINA, sum(MONTOPAGO) from PAGO" +
                    "where PROVEEDOR= '" + codMedico + "' and " +
                    "FECHAPAGO >= '" + fecha_inicio + "' and FECHAPAGO <= '" + fecha_fin + "' " +
                    "group by NRONOMINA";
            return Query;
        }

        //el nronomina me lo da las dos consultas anteriores
        public string ConsultarUltimoPagoHonorariosDeducciones(string codMedico, string nronomina)
        {
            Query = "select A.CODIGODEDUCCION, B.NOMBREDEDUCCION, A.MONTO " +
                    "from APLICAN A INNER JOIN DEDUCCIONES B ON A.CODIGODEDUCCION = B.CODIGODEDUCCION" +
                    "where A.PROVEEDOR= '" + codMedico + "' and NRONOMINA = '" + nronomina + "'";

            return Query;
        }

        //**HONORARIOS FACTURADOS**/
        //Consultar honorarios Facturados. MONTO TOTAL
        //String o int codMedico?
        /*public string ConsultarTotalMontoFacturado(string codMedico, DateTime fecha_inicio, DateTime fecha_fin)
        {
            Query = "select sum(A.MONTOAPAGAR)from CUENTA_POR_PAGAR A " +
                    "inner join CASO B on A.CASO = B.NROCASO " +
                    "where A.PROVEEDOR= '" + codMedico + "' and " +
                    "B.FECHAEMISION >= '" + fecha_inicio + "' and B.FECHAEMISION <= '" + fecha_fin + "'";

            return Query;
        }

        //Consultar honorarios Facturados. MONTO POR UNIDAD DE NEGOCIO
        //String o int codMedico?
        public string ConsultarTotalMontoFacturadoUDN(string codMedico, DateTime fecha_inicio, DateTime fecha_fin)
        {
            Query = "select A.UDN, sum(A.MONTOAPAGAR)from CUENTA_POR_PAGAR A " +
                    "inner join CASO B on A.CASO = B.NROCASO " +
                    "where A.PROVEEDOR= '" + codMedico + "' and " +
                    "B.FECHAEMISION >= '" + fecha_inicio + "' and B.FECHAEMISION <= '" + fecha_fin + "' " +
                    "group by A.UDN";
            
            return Query;
        }
        //Consultar nombre de unidad de negocio
        public string ConsultarNombreUDN(float udn)
        {
            Query = "select NOMBRE from UNIDAD_DE_NEGOCIO " +
                    "where CODIGO= '" + udn + "'";

            return Query;
        }
        
        //**DETALLE DE UN CASO**/
		//POR NUMERO DE CASO 
        //HONORARIOS PRESTADOS POR EL MEDICO EN ESE CASO Y TOTAL FACTURADO POR CADA SUMINISTRO PRESTADO
		//ME PERMITE VERIFICAR DE IGUAL MANERA SI SE TRATA DE UN CASO VÁLIDO
		/*public string ConsultarHonorariosPrestadosCaso(string Codmedico, string codCaso)
        {
            Query = "select B.NOMBRESUMINISTRO, A.UDN, A.SUMINISTRO, A.MONTOAPAGAR from CUENTA_POR_PAGAR A " +
                    "inner join SUMINISTRO B on A.SUMINISTRO = B.CODIGOSUMINISTRO " +
                    "where A.CASO= '" + codCaso + "' and A.PROVEEDOR = '"+ Codmedico+"'";

            return Query;
        }
		
		//TOTAL EXONERADO
		//El suministro y la udn me lo dá la consulta anterior.
		public string ConsultarExoneradoSuministroCaso(string Codmedico, string codCaso, string suministro, string udn)
        {
            Query = "select sum(MONTO) from NOTA_DE_DEBITO_Y_CREDITO " +
                    "where CASO= '" + codCaso + "' and PROVEEDOR = '"+ Codmedico+"' and " +
					"UDN= '" + udn + "' and SUMINISTRO = '"+ suministro+"' and " +
					"TIPONOTA = 1" +
					"group by SUMINISTRO, UDN ";

            return Query;
        }
		
		//TOTAL ABONADO 
		//El suministro y la udn me lo dá la consulta anterior.
		public string ConsultarPagadoSuministroCaso(string Codmedico, string codCaso, string suministro, string udn)
        {
            Query = "select sum(MONTOPAGO) from PAGO " +
                    "where CASO= '" + codCaso + "' and PROVEEDOR = '"+ Codmedico+"' and " +
					"UDN= '" + udn + "' and SUMINISTRO = '"+ suministro+"' and " +
					"group by SUMINISTRO, UDN ";

            return Query;
        }
		
        //NOMBRE DEL PACIENTE
        public string ConsultarPacienteCaso(string codCaso)
        {
            Query = "select B.CEDULA, B.NOMBREPACIENTE from CASO A " +
                    "inner join PACIENTE B on A.PACIENTE = B.CEDULA " +
                    "where A.NROCASO= '" + codCaso + "'";

            return Query;
        }

        //NOMBRE DEL RESPONSABLE(S) DE PAGO
        public string ConsultarResponsablePagoCaso(string codCaso)
        {
            Query = "select B.NOMBRERESPONSABLE from ES_RESPONSABILIDAD A " +
                    "inner join RESPONSABLE_DE_PAGO B on A.RESPONSABLE = B.CODIGORESPONSABLE " +
                    "where A.CASO= '" + codCaso + "'";

            return Query;
        }
		
		//DETALLE DE UN CASO 
		//POR APELLIDO
		public string ConsultarCasosPorApellido(string Codmedico, string apellido)
        {
            Query = "select A.NROCASO, A.FECHAEMISION C.NOMBRE from CASO A " +
                    "join CUENTA_POR_PAGAR B on A.NROCASO = B.CASO " +
					"join PACIENTE C on A.PACIENTE = C.CEDULA " +
                    "where B.PROVEEDOR= '" + Codmedico + "' and C.APELLIDO_PAC = '"+ apellido +"'";

            return Query;
        }
		
		//LISTADO DE FIANZAS 
		public string ConsultarListadoFianzas(string Codmedico)
        {
            Query = "select A.CASO, C.NOMBREPACIENTE, A.MONTOACOBRAR, A.MONTOABONADO from ES_RESPONSABILIDAD A " +
                    "join CASO B on A.CASO = B.NROCASO " +
					"join PACIENTE C on B.PACIENTE = C.CEDULA " +
                    "where B.RESPONSABLE= '" + Codmedico + "'";

            return Query;
        }*/
        
        //AL MOMENTO DE INICIAR SESION. FALTA UN QUERY DONDE PREGUNTE SI EL MÉDICO PERTENECE A UN POOL 
        //EN CASO DE SER ASI DEBO DEVOLVER EL CODIGO DE ESE PROVEEDOR (POOL). LA IMPLICACIÓN ES QUE AL CONSULTAR 
        //ESTADO DE CUENTA, LO FACTURADO, LOS PAGOS, EL DETALLE DE UN CASO DEBO INCLUIR TODOS LOS DE ESE CODIGO POOL.
        //LAS FIANZAS NO CIERTO? CREO QUE NO PUEDE HABER FIANZA A NOMBRE DE UN POOL
        //OJO YO NO CONTEMPLÉ ESTO EN MI INTERFAZ!! INCLUIRLO. SI SOLO EXISTE UN SOLO POOL QUE ES EL DE LOS INTENSIVISTAS 
        //Y SI DE VERDAD EN EL ESTADO DE CUENTA DEBO HACER EL CALCULO PORCENTUAL DE LO QUE LE CORRESPONDE A CADA MEDICO?
        //CREO QUE NO HAY PAGOS A NOMBRE DEL POOL.. PARA ALGO REPARTEN EL POTE CIERTO? Y LAS DEDUCCIONES?
        //CUIDADO CON ESTO PREGUNTARLE MUY BIEN A BELKIS!! 
		
    }
}

