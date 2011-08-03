using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Atencion24WebServices.Atencion24DAO;

namespace Atencion24WebServices.Atencion24Negocio
{
    public class Usuario
    {
        private string login;
        private string password;
        private string nombre = " ";
        private string cedula = " ";
        private ArrayList codigosPago = null;
        private bool valido = true;

        //Constructor
        public Usuario() { }

        //Constructor
        public Usuario(string log, string pass)
        {
            login = log;
            password = pass;
        }

        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public ArrayList CodigosPago
        {
            get { return codigosPago; }
            set { codigosPago = value; }
        }

        public bool Valido
        {
            get { return valido; }
            set { valido = value; }
        }

        //Función que llama a otra función en la capa de datos para decir si un usuario existe o no.
        public void ExisteUsuario()
        {
            DataSet ds = new DataSet();
            UsuarioDAO ud = new UsuarioDAO();

            ds = ud.InicioSesionExisteUsuario(login);
            if (ds.Tables[0].Rows.Count == 0)
                valido = false;
            else
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) == DBNull.Value)
                    valido = false;
        }

        public String ConsultarUsuario()
        {
            DataSet ds = new DataSet();
            UsuarioDAO ud = new UsuarioDAO();
            String codigo = " ";

            ds = ud.InicioSesionConsultarUsuario(login, password);
            if (ds.Tables[0].Rows.Count == 0)
                valido = false;
            else
            {
                //Clave
                if (ds.Tables[0].Rows[0].ItemArray.ElementAt(0) == DBNull.Value)
                    valido = false;
                else
                {
                    if (!ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString().Equals("1"))
                        valido = false;
                    else
                    {
                        //Cedula
                        if (ds.Tables[0].Rows[0].ItemArray.ElementAt(1) == DBNull.Value)
                            valido = false;
                        else
                            cedula = ds.Tables[0].Rows[0].ItemArray.ElementAt(1).ToString();

                        //Nombre
                        if (ds.Tables[0].Rows[0].ItemArray.ElementAt(2) != DBNull.Value)
                            nombre = ds.Tables[0].Rows[0].ItemArray.ElementAt(2).ToString();

                        //Codigo
                        if (ds.Tables[0].Rows[0].ItemArray.ElementAt(3) == DBNull.Value)
                            valido = false;
                        else
                            codigo = ds.Tables[0].Rows[0].ItemArray.ElementAt(3).ToString();
                    }
                }
            }
            return codigo;
        }

        public void ConsultarCodigosPago(String codigoPropio)
        {
            DataSet ds = new DataSet();
            DataSet dsCodigos = new DataSet();
            UsuarioDAO ud = new UsuarioDAO();

            String codigo = "";
            String nombre = "";

            //Consultamos los códigos de pago del usuario 
            ds = ud.InicioSesionConsultarCodigosPago(this.cedula);

            //Verificamos que tnga códigos de pago
            if (ds.Tables[0].Rows.Count != 0)
            {
                codigosPago = new ArrayList();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CodigoPago codPago = new CodigoPago();
                    //Codigo
                    if (dr.ItemArray.ElementAt(0) != DBNull.Value)
                    {
                        codigo = dr.ItemArray.ElementAt(0).ToString();
                        //Nombre
                        ud = new UsuarioDAO();
                        dsCodigos = ud.InicioSesionConsultarNombreCodigosPago(codigo);
                        if (dsCodigos.Tables[0].Rows.Count != 0)
                        {
                            if (dsCodigos.Tables[0].Rows[0].ItemArray.ElementAt(0) != DBNull.Value)
                            {
                                nombre = dsCodigos.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                                codPago.Codigo = codigo;
                                codPago.Nombre = nombre;
                                codigosPago.Add(codPago);
                            }
                        }
                    }
                }
            }
            else
            {
                codigosPago = new ArrayList();
                CodigoPago codPago = new CodigoPago();
                codPago.Codigo = codigoPropio;
                codPago.Nombre = this.nombre;
                codigosPago.Add(codPago);
            }
        }

        /*VERISON VIEJA
        //Función que llama a otra función en la capa de datos para decir si un usuario existe o no.
        public DataSet ExisteUsuario(Usuario user)
        {
            DataSet ds = new DataSet();

            UsuarioDAO ud = new UsuarioDAO();

            ds = ud.ExisteUsuarioDAO(user);
            return ds;
        }

        //Función que llama a otra función en la capa de datos para devolver los datos de un usuario
        public DataSet ConsultarUsuario(Usuario user)
        {
            DataSet ds = new DataSet();

            UsuarioDAO ud = new UsuarioDAO();

            ds = ud.ConsultarUsuarioDAO(user);
            return ds;
        }*/
    }
}
