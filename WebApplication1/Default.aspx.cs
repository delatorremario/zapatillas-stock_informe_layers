using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

using System.Data.OleDb;
using System.Data;
using System.Diagnostics;
using informe_stock_layers.bss;

namespace WebApplication1
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    var conexs = ConfigurationManager.ConnectionStrings;
                    foreach (var c in conexs)
                    {
                        rbConexiones.Items.Add(c.ToString());
                    }
                }
                
            }
            catch (Exception ex)
            {

                string myScript2 = "alertify.error('message." + ex.Message + "')";
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                                Guid.NewGuid().ToString(), myScript2, true);
                return;
                //throw;
            }
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            try
            {
                    //' Specify the path on the server to
                    //' save the uploaded file to.
                var savePath = Server.MapPath("~/upload/");
                //' Before attempting to perform operations
                //' on the file, verify that the FileUpload 
                //' control contains a file.

                if (FileUpload1.HasFile) {

            
                    //' Get the name of the file to upload.
                    var fileName = FileUpload1.FileName;
                    var extension = System.IO.Path.GetExtension(fileName);

                    if (extension == ".xlsx" || extension == ".xls" ){

                
                        //' Append the name of the file to upload to the path.
                        savePath += fileName;

                        //' Call the SaveAs method to save the 
                        //' uploaded file to the specified path.
                        //' This example does not perform all
                        //' the necessary error checking.               
                        //' If a file with the same name
                        //' already exists in the specified path,  
                        //' the uploaded file overwrites it.
                        FileUpload1.SaveAs(savePath);

                        //' Notify the user of the name the file
                        //' was saved under.
                        UploadStatusLabel.Text = "Su archivo se grabó correctamente";// '"Your file was saved as " & fileName;

                        //string archivo = savePath; // 'Server.MapPath("agenda.xlsx") 'Asignamos la ruta
                        Session["archivo"]=savePath;
                        
                        //GridView1.DataSource = LeerArchivoExcel(archivo); //' Invocamos a la funcion LeerArchivoExcel, la cual devolverá un Dataset y sera el origen de los datos para el GridView
                        //GridView1.DataBind();
                        //UploadStatusLabel.Text = "Se encontraron " + GridView1.Rows.Count + " registros - " + this.UploadStatusLabel.Text;

                    }else{
                        UploadStatusLabel.Text = "No es un archivo de Excel"; //'"Your file was saved as " & fileName
                    }
                }else{
                    //' Notify the user that a file was not uploaded.
                    UploadStatusLabel.Text = "Debe seleccionar un archivo";
                }
                
            
            }
            catch (Exception ex)
            {
                string myScript2 = "alertify.error('message."+ex.Message+"')";
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                                Guid.NewGuid().ToString(), myScript2, true);
                return; 
                //throw;
            }
        }


        private DataSet LeerArchivoExcel (string file){
            try 
	        {	
                
		        string m_sConn1 = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + file + "; Extended Properties='Excel 12.0;HDR=YES'";

            //'Generamos objeto de conexion
            var conn2 = new OleDbConnection(m_sConn1);

            //'Definimos la consulta SQL para leer la informacion del archivo de Excel, noten que hacemos referencia a las Hojas, se puede leer cualquier hoja, siempre y cuando indiquemos el nombre con un signo $ y encerrado entre []
            string consulta = "Select * From [Hoja1$]";

            //'Lo siguiente ejecutar la conexion y la consulta y llenar el DataSet que devolvera la función
            var da = new OleDbDataAdapter(consulta, conn2);

            var ds = new DataSet();


            da.Fill(ds);
            conn2.Close();
            return ds;
	        }
	        catch (Exception)
	        {
		
		        throw;
	        }
        }

        protected void btProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = LeerArchivoExcel(Session["archivo"].ToString());

                var arr= ds.Tables[0].Rows.Cast<DataRow>()
                                        .Select(row=>row["COD_ARTICU"].ToString())
                                        .ToArray();

                 Debug.WriteLine(arr.ToString());

                 DateTime fini = DateTime.Parse ( "01/01/2015");
                 DateTime ffin = Convert.ToDateTime ("31/01/2015");
                 string conexionStr = rbConexiones.SelectedValue;
                 consultaBss consultaBss = new consultaBss(conexionStr);
                 List<Resumen> miResumen = consultaBss.consultarResumen(fini, ffin, arr);

                 GridView1.DataSource = miResumen;
                 GridView1.DataBind();

                /*foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Debug.WriteLine(dr["COD_ARTICU"].ToString());
                }*/

                
            }
            catch (Exception ex)
            {

                string myScript2 = "alertify.error('message." + ex.Message + "')";
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                                Guid.NewGuid().ToString(), myScript2, true);
                return;
                //throw;
            }
        } 



    }
}