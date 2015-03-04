using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Zhaopin.Universal.ZPDBHelper;
using ZP.Campus.Framework.Core.Common;

namespace ZP.Campus.Framework.Core.StoredProcedure
{

    public class StoredProcedure<SPModel, ToModel> where SPModel : IStoredProcedure
    {
        private static DbHelperService dbHelper = null;

        public static StoredProcedure<SPModel, ToModel> GetInstance(string connectionString, bool isnotstr = false)
        {
            dbHelper = null;//CampusDbHelper.GetInstance(connectionString);
            return new StoredProcedure<SPModel, ToModel>();
        }

        public DataSet ExecuteRenturnDataSet(SPModel model)
        {
            var outputParms = new Dictionary<int, string>();
            var parms = getParams(model, out outputParms);
            DataSet ds = dbHelper.ExecuteDataset(CommandType.StoredProcedure, StoredProedureParameter<SPModel>.Name, parms);

            if (outputParms.Count > 0)
            {
                foreach (var outputParm in outputParms)
                {
                    var ctype = model.GetType();
                    var cfiT = ctype.GetProperty(outputParm.Value);
                    cfiT.SetValue(model, parms[outputParm.Key].Value, null);
                }
            }

            return ds;
        }
        public ToModel ExecuteToModel(SPModel model)
        {
            var outputParms = new Dictionary<int, string>();
            var parms = getParams(model, out outputParms);

            DataSet DS = dbHelper.ExecuteDataset(CommandType.StoredProcedure, StoredProedureParameter<SPModel>.Name, parms);

            if (outputParms.Count > 0)
            {
                foreach (var outputParm in outputParms)
                {
                    var ctype = model.GetType();
                    var cfiT = ctype.GetProperty(outputParm.Value);
                    cfiT.SetValue(model, parms[outputParm.Key].Value, null);
                }
            }

            if (DS == null) return default(ToModel);
            if (DS.Tables.Count <= 0) return default(ToModel); ;
            if (DS.Tables[0].Rows.Count <= 0) return default(ToModel); ;
            return getListModelByDataSet<ToModel>(DS.Tables[0]).FirstOrDefault();

        }

        public IList<ToModel> ExecuteToList(SPModel model)
        {
            var outputParms = new Dictionary<int, string>();
            var parms = getParams(model, out outputParms);

            DataSet DS = dbHelper.ExecuteDataset(CommandType.StoredProcedure, StoredProedureParameter<SPModel>.Name, parms);

            if (outputParms.Count > 0)
            {
                foreach (var outputParm in outputParms)
                {
                    var ctype = model.GetType();
                    var cfiT = ctype.GetProperty(outputParm.Value);
                    cfiT.SetValue(model, parms[outputParm.Key].Value, null);
                }
            }

            if (DS == null) return null;
            if (DS.Tables.Count <= 0) return null;
            if (DS.Tables[0].Rows.Count <= 0) return null;
            return getListModelByDataSet<ToModel>(DS.Tables[0]);
        }
        public bool ExcuteStateNonQuery(SPModel model)
        {
            var outputParms = new Dictionary<int, string>();
            var parms = getParams(model, out outputParms);

            int rtn = dbHelper.ExecuteNonQuery(CommandType.StoredProcedure, StoredProedureParameter<SPModel>.Name, parms);

            if (outputParms.Count > 0)
            {
                foreach (var outputParm in outputParms)
                {
                    var ctype = model.GetType();
                    var cfiT = ctype.GetProperty(outputParm.Value);
                    cfiT.SetValue(model, parms[outputParm.Key].Value, null);
                }
            }
            return rtn > 0;

        }
        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ExcuteExtisData(SPModel model)
        {
            var outputParms = new Dictionary<int, string>();
            var parms = getParams(model, out outputParms);

            DataSet DS = dbHelper.ExecuteDataset(CommandType.StoredProcedure, StoredProedureParameter<SPModel>.Name, parms);

            if (DS == null || DS.Tables == null || DS.Tables.Count <= 0)
            {
                return true;
            }
            else
            {
                if (DS.Tables[0].Rows.Count > 0)
                {
                    return false;
                }
                else
                    return true;

            }

        }
        public void ExcuteNonQuery(SPModel model)
        {
            var outputParms = new Dictionary<int, string>();
            var parms = getParams(model, out outputParms);

            int rtn = dbHelper.ExecuteNonQuery(CommandType.StoredProcedure, StoredProedureParameter<SPModel>.Name, parms);

            if (outputParms.Count > 0)
            {
                foreach (var outputParm in outputParms)
                {
                    var ctype = model.GetType();
                    var cfiT = ctype.GetProperty(outputParm.Value);
                    cfiT.SetValue(model, parms[outputParm.Key].Value, null);
                }
            }


        }
        private void ExcuteNonQuery(SPModel model, SqlParameter[] sqlparams)
        {
            int rtn = dbHelper.ExecuteNonQuery(CommandType.StoredProcedure, StoredProedureParameter<SPModel>.Name, sqlparams);

        }
        public int ExcuteReturnInt(SPModel model)
        {
            var outputParms = new Dictionary<int, string>();
            var parms = getParams(model, out outputParms);

            SqlParameter[] sqlparms = parms;
            ExcuteNonQuery(model, sqlparms);

            if (outputParms.Count > 0)
            {
                foreach (var outputParm in outputParms)
                {
                    var ctype = model.GetType();
                    var cfiT = ctype.GetProperty(outputParm.Value);
                    cfiT.SetValue(model, parms[outputParm.Key].Value, null);
                }
            }

            return (int)sqlparms[sqlparms.Length - 1].Value;//.ToString().ToInt32(0);
        }

        /// <summary>
        /// 输出参数需定义DisplayName["output"]
        /// </summary>
        /// <param name="model"></param>
        /// <param name="outputParms"></param>
        /// <returns></returns>
        private SqlParameter[] getParams(SPModel model, out Dictionary<int, string> outputParms)
        {
            outputParms = new Dictionary<int, string>();

            PropertyInfo[] pis = StoredProedureParameter<SPModel>.PropertiesInfo;
            SqlParameter[] sqlparms = new SqlParameter[pis.Length];
            for (int i = 0; i < pis.Length; i++)
            {

                if ("output".Equals("pis[i].GetDisplayName(pis[i].Name)".ToLower()))
                {
                    sqlparms[i] = new SqlParameter() { ParameterName = string.Format("@{0}", pis[i].Name), SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
                    outputParms.Add(i, pis[i].Name);
                }
                else
                {

                    sqlparms[i] = new SqlParameter(string.Format("@{0}", pis[i].Name), pis[i].GetValue(model, null));
                }
                // sqlparms[i].Value = pis[i].GetValue(model, null);
            }
            return sqlparms;
        }
        public SqlDbType GetDBType(string systemtypename)
        {
            SqlParameter p1 = new SqlParameter();
            var tc = System.ComponentModel.TypeDescriptor.GetConverter(p1.SqlDbType);
            p1.SqlDbType = (SqlDbType)tc.ConvertFrom(systemtypename);
            return p1.SqlDbType;
        }
        public IList<T> getListModelByDataSet<T>(DataTable dt)
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T model = toEntity<T>(dr);
                if (model != null)
                    list.Add(model);
            }
            return list;
        }
        private T toEntity<T>(DataRow dr)
        {
            if (dr == null) return default(T);
            Type type = typeof(T);
            Object entity = Activator.CreateInstance(type);         //创建实例               
            foreach (PropertyInfo entityCols in type.GetProperties())
            {
                try
                {
                    if (!string.IsNullOrEmpty(dr[entityCols.Name].ToString()))
                    {
                        //if (entityCols.PropertyType.Name.Equals("DateTime"))
                        //{
                        //    string entityvalue = dr[entityCols.Name].ToString();
                        //    DateTime dt = Convert.ToDateTime(entityvalue);
                        //    if (dt.ToString().Equals("0001/1/1 0:00:00"))
                        //        entityCols.SetValue(entity, DateTime.MaxValue, null);
                        //}

                        if (entityCols.PropertyType.Name.ToUpper().Equals("STRING"))//此步骤是为了兼容老方法
                        {
                            object value = dr[entityCols.Name] == null ? "" : dr[entityCols.Name];
                            entityCols.SetValue(entity, value.ToString(), null);//TODO:如果日期类型转不了 所以要TOString（）
                        }
                        else
                        {
                            object value = dr[entityCols.Name] == null ? "" : dr[entityCols.Name];
                            entityCols.SetValue(entity, value, null);
                        }

                    }
                }
                catch (Exception ex)
                {

                    continue;
                }

            }
            return (T)entity;
        }

        // // 转换为SqlDbType类型
        //public static SqlDbType SqlTypeString2SqlType(string sqlTypeString)
        //{
        //SqlDbType dbType = SqlDbType.Variant;//默认为Object

        //switch (sqlTypeString)
        //{
        //       case "int":
        //         dbType = SqlDbType.Int;
        //         break;
        //       case "varchar":
        //         dbType = SqlDbType.VarChar;
        //         break;
        //       case "bit":
        //         dbType = SqlDbType.Bit;
        //         break;
        //       case "datetime":
        //         dbType = SqlDbType.DateTime;
        //         break;
        //       case "decimal":
        //         dbType = SqlDbType.Decimal;
        //         break;
        //       case "float":
        //         dbType = SqlDbType.Float;
        //         break;
        //       case "image":
        //         dbType = SqlDbType.Image;
        //         break;
        //       case "money":
        //         dbType = SqlDbType.Money;
        //         break;
        //       case "ntext":
        //         dbType = SqlDbType.NText;
        //         break;
        //       case "nvarchar":
        //         dbType = SqlDbType.NVarChar;
        //         break;
        //       case "smalldatetime":
        //         dbType = SqlDbType.SmallDateTime;
        //         break;
        //       case "smallint":
        //         dbType = SqlDbType.SmallInt;
        //         break;
        //       case "text":
        //         dbType = SqlDbType.Text;
        //         break;
        //       case "bigint":
        //         dbType = SqlDbType.BigInt;
        //         break;
        //       case "binary":
        //         dbType = SqlDbType.Binary;
        //         break;
        //       case "char":
        //         dbType = SqlDbType.Char;
        //         break;
        //       case "nchar":
        //         dbType = SqlDbType.NChar;
        //         break;
        //       case "numeric":
        //         dbType = SqlDbType.Decimal;
        //         break;
        //       case "real":
        //         dbType = SqlDbType.Real;
        //         break;
        //       case "smallmoney":
        //         dbType = SqlDbType.SmallMoney;
        //         break;
        //       case "sql_variant":
        //         dbType = SqlDbType.Variant;
        //         break;
        //       case "timestamp":
        //         dbType = SqlDbType.Timestamp;
        //         break;
        //       case "tinyint":
        //         dbType = SqlDbType.TinyInt;
        //         break;
        //       case "uniqueidentifier":
        //         dbType = SqlDbType.UniqueIdentifier;
        //         break;
        //       case "varbinary":
        //         dbType = SqlDbType.VarBinary;
        //         break;
        //       case "xml":
        //         dbType = SqlDbType.Xml;
        //         break;
        //}
        //return dbType;
        //}
    }

    public class StoredProedureParameter<SPModel>
    {
        public static Type ParameterType { get; private set; }
        public static PropertyInfo[] PropertiesInfo { get; private set; }
        public static string Name { get; private set; }
        static StoredProedureParameter()
        {
            ParameterType = typeof(SPModel);
            Name = "[zhaopin].[" + ParameterType.Name + "]";//zhaopin可以从配置文件中取
            PropertiesInfo = ParameterType.GetProperties();
        }

    }

}
