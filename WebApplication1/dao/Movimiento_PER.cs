using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;

namespace informe_stock_layers.dao
{
    public class Movimiento_PER
    {

        public DataTable listarMovimientos(DateTime fecha_inicio, DateTime fecha_fin, string codigo, out string articulo,SqlTransaccional miSql)
        {
            try
            {
                DataTable dt = new DataTable();
                articulo = "";




                string sql = "select DESCRIPCIO + ' - ' + DESC_ADIC from sta11 where COD_ARTICU='" + codigo + "'";
                var resultado = miSql.consulta_escalar(sql);

                if (resultado != null)
                {

                    articulo = resultado.ToString();

                    sql = "declare @codigo as nvarchar(50) \n" +
                          "declare @fecha_inicio as datetime \n" +
                          "declare @fecha_fin as datetime \n" +
                          "set @codigo ='" + codigo + "' \n" +
                          "set @fecha_inicio='" + fecha_inicio.ToString("yyyyMMdd") + "' \n" +
                          "set @fecha_fin = '" + fecha_fin.ToString("yyyyMMdd") + "' \n" +
                          "--select top 10 *  from sta20 where COD_ARTICU=@codigo \n" +
                          "--historial agrupado \n" +
                          "select DATEADD(DD,-2, @fecha_inicio) as fecha_mov, 'Saldo Anterior' as movimiento,'' as tipo_mov \n" +
                          ",sum(case when tipo_mov = 'E' then cantidad  \n" +
                                  "else cantidad * -1 \n" +
                                  "end \n" +
                                  ") as cantidad from sta27 \n" +
                          "where COD_ARTICU=@codigo and FECHA_MOV>@fecha_inicio \n" +
                          "group by COD_ARTICU \n" +
                          "--mov anterior a la fecha inicial \n" +
                          "union all \n" +
                          "select DATEADD(DD,-1, @fecha_inicio) as fecha_mov, 'Saldo Anterior' as movimiento,'' as tipo_mov \n" +
                          ",sum(case when tipo_mov = 'E' then cantidad  \n" +
                          "		else cantidad * -1 \n" +
                          "		end \n" +
                          "		) as cantidad from sta20 \n" +
                          "where COD_ARTICU=@codigo and FECHA_MOV<@fecha_inicio \n" +
                          "group by COD_ARTICU \n" +
                          "--mov entre fechas \n" +
                          "union all \n" +
                          "select fecha_mov, " +
                          "case tcomp_in_s when 'VE' then '.....1'  " +
                          "when 'FR' then 'FAC' " +
                          "when 'AJ' then '.....3' " +
                          "when 'VS' then '.....2' " +
                          "else tcomp_in_s end   " +
                          "+ '  ' + (select  coalesce( e.n_comp,e.ncomp_orig) from sta14 e  " +
                          "where e.ncomp_in_s=d.ncomp_in_s and e.tcomp_in_s=d.tcomp_in_s) " +
                          "as movimiento " +
                          ",tipo_mov,case when tipo_mov = 'E' then cantidad  \n" +
                          "		else cantidad * -1 \n" +
                          "		end as cantidad  from sta20 d \n" +
                          "where COD_ARTICU=@codigo and FECHA_MOV between @fecha_inicio and @fecha_fin \n" +
                          "--mov posterior a la fecha final  \n" +
                          "union all \n" +
                          "select DATEADD(DD,1, @fecha_fin) as fecha_mov, 'Saldo Posterior' as movimiento,'' as tipo_mov \n" +
                          ",sum(case when tipo_mov = 'E' then cantidad  \n" +
                          "		else cantidad * -1 \n" +
                          "		end \n" +
                          "		) as cantidad from sta20 \n" +
                          "where COD_ARTICU=@codigo and FECHA_MOV>@fecha_fin \n" +
                          "group by COD_ARTICU \n";


                    dt.Load(miSql.consulta_SqlDataReader(sql));
                }
                return dt;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        
    }
}