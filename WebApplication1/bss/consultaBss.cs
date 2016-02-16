using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using informe_stock_layers.dao;
using System.Data;


namespace informe_stock_layers.bss
{
      public class Resumen
        {
            public string codigo;
            public string articulo;
            public List<ResumenDetalle> detalle;
           
        }
        public class ResumenDetalle
        {
            public DateTime fecha{get;set;}
            public string movimiento { get; set; }
            public string numero { get; set; }
            public string tipo { get; set; }//E=entra S=sale
            public decimal cantidad { get; set; }
        }

   
    public class consultaBss
    {
        private  SqlTransaccional miSql;// = new SqlTransaccional(false);
        public consultaBss(string conexionStr){
            miSql = new SqlTransaccional(false,conexionStr);
        }
        public List<Resumen> consultarResumen(DateTime fecha_inicio, DateTime fecha_fin, string[] codigos)
        {
            try
            {
                List<Resumen> listResumen = new List<Resumen>();

                Resumen resumen;
                List<ResumenDetalle> resumenDetalle;
                foreach (string c in codigos)
                {
                    if (c!=null && c!="")
                    {
                        resumen = new Resumen { codigo = c };
                        resumenDetalle = consultarDetalle(fecha_inicio, fecha_fin, c, out resumen.articulo, miSql);
                        if (resumen.articulo != "")
                        {
                            resumen.detalle = resumenDetalle;
                            listResumen.Add(resumen);
                        }
                        
                    }
                    
                }


                return listResumen;

            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public List<ResumenDetalle> consultarDetalle(DateTime fecha_inicio, DateTime fecha_fin, string codigo, out string articulo,SqlTransaccional miSql)
        {
            try
            {
                List<ResumenDetalle> listResumenDetalle=new List<ResumenDetalle>();
                Movimiento_PER movPer = new Movimiento_PER();
                DataTable dt;
               
                    dt = movPer.listarMovimientos(fecha_inicio, fecha_fin, codigo,out articulo, miSql );
                    listResumenDetalle = (from DataRow row in dt.Rows
                                                            select new ResumenDetalle
                                                            {
                                                                fecha = DateTime.Parse( row["fecha_mov"].ToString()),
                                                                movimiento = row["movimiento"].ToString(),
                                                                numero = "",
                                                                tipo = row["tipo_mov"].ToString(),
                                                                cantidad =decimal.Parse( row["cantidad"].ToString())
                                                            }).ToList();

             return listResumenDetalle;
        
               /*     new[]
                    { new{
                        fecha = DateTime.Parse("28/02/2015"),
                        movimiento = "Ajuste de Stock",
                        numero = "0001",
                        tipo = "E",
                        cantidad = 3
                    },new
                        {
                        fecha = DateTime.Parse("3/02/2015"),
                        movimiento = "Remito",
                        numero = "0002",
                        tipo = "S",
                        cantidad = 4
                    }}
                    ;*/
              



               

            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}