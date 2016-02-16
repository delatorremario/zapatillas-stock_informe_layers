using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Conecta con los datos del servidor sql
/// Mario de la Torre
/// </summary>
/// namespace informe_stock_server.dao

public class SqlTransaccional
{
    public  SqlConnection cn;
    private SqlTransaction transaction;
    private Boolean enTransaccion;
    private SqlCommand cmd;
    //private string conexion = ConfigurationManager.ConnectionStrings["BASEDEDATOSConnectionString"].ToString(); 

    public SqlTransaccional(Boolean usaTransaccion,string conexion)
    {
        try
        {
            cn = new SqlConnection();
            cn.ConnectionString = conexion;//@"Data Source=.\SQLEXPRESS;Initial Catalog=ELECTRO;Persist Security Info=True;User ID=sa;Password=123";//ConfigurationManager.ConnectionStrings["DatosSqlServer"].ToString();
            cn.Open();
            cmd= cn.CreateCommand();
            
            //si es transaccion 
            if (usaTransaccion == true)
            {
                transaction = cn.BeginTransaction("miTran");
                cmd.Connection = cn;
                cmd.Transaction = transaction;
            }
            else { cmd.Connection = cn; }
            enTransaccion = usaTransaccion;
        }
        catch (Exception)
        {
            /*if (cn.State == ConnectionState.Open)
                {
                             
                cn.Close(); 
                }
             * */
            throw;
       
        }
          
       
    }
   
    public SqlDataReader consulta_SqlDataReader(string strSql)
    {
        try
        {
            
          //  if(enTransaccion ) cmd = new SqlCommand(strSql, cn,transaction );
          //  else cmd = new SqlCommand(strSql, cn);


            cmd.CommandText=strSql;
            SqlDataReader dr = cmd.ExecuteReader();

            return dr;
        }
        catch (Exception)
        {

            throw;
        }
       
       

      }
    public Object consulta_escalar(string strSql)
        {
            Object resultado;

            cmd.CommandText = strSql;
            
            resultado = cmd.ExecuteScalar();
            //cerrar_conexion();
            return resultado;
            
        }
    public bool  consulta_nonquery(ref string strSql)
    {
        try 
        {
            int rows;

            cmd.CommandText = strSql;

            rows = cmd.ExecuteNonQuery();
            strSql = rows.ToString() + " fila(s) afectada(s)";
            return true;
         }
        catch (Exception exc) //Module failed to load
        {
            strSql = exc.Message;
            return false;
        }
        //finally
        //{
        // //   cerrar_conexion();
        //}
       
    }
    public string confirmar_transaccion()
    {
        try
        {

            transaction.Commit();
            return "Confirmado";
          }
        catch (SqlException exc) //Module failed to load
        {
            return exc.Message;
        }
       
        finally
        {
           cn.Close();
        }
    }
    public string deshacer_transaccion()
    {
        try
        {
            transaction.Rollback();
            return "Deshecho";
        }
        catch (Exception exc) //Module failed to load
        {
            return exc.Message;
        }
        finally
        {
            cn.Close();
        }
    }
    protected  void cerrar_conexion()
    {
        if (enTransaccion == false) { cn.Close(); }
    }

    //internal bool consulta_nonquery(string miStrSql)
    //{
    //    throw new NotImplementedException();
    //}
}
