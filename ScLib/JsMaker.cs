using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ScLib
{
    public class JsMaker
    {
        public static void MakeTable(StringBuilder sb, string jstablename, string fnname, DataTable dt)
        {
            sb.AppendLine("var " + jstablename + ";");

            for (int i = 0; i < dt.Columns.Count; i++)
                sb.AppendLine("<!-- " + dt.Columns[i].ColumnName + " " + dt.Columns[i].DataType.Name + " -->");

            sb.AppendLine("function " + fnname + "()");
            sb.AppendLine("{");
            sb.AppendLine("var i = 0;");
            sb.AppendLine(jstablename + " = new Array();");
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append(jstablename + "[i++] = new Array( ");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i > 0)
                        sb.Append(",");
                    sb.Append( MakeJsValue( dr[i], dt.Columns[i].DataType ) );
                }
                sb.AppendLine(");");
            }
            sb.AppendLine("}");
        }

        private static string MakeJsValue(object p, Type type)
        {
            try
            {
                if (type == typeof(int))
                    return ((int)p).ToString();
                if (type == typeof(Decimal))
                {
                    if (p == DBNull.Value )
                        return "0";
                    return ((Decimal)p).ToString();
                }
                return "'" + p.ToString() + "'";
            }
            catch (Exception ex)
            {
                throw new Exception( ":" + p.ToString() + ":" + p.GetType().Name + ":" + type.Name + " threw " + ex.Message);
            }
        }
    }
}
