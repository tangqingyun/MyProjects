using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ObjectExtension.Test
{
    public static class ObjectCustomExtension
    {
        private static readonly Type ValueTypeType = typeof(ValueType);

        /// <summary>
        /// 将字符转换成自己的类型
        /// </summary>
        /// <param name="val">System.String</param>
        /// <returns>如果转换失败将返回 T 的默认值</returns>
        public static T ToT<T>(this object val)
        {
            if (val != null)
            {
                return val.ToT<T>(default(T));
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 当前对象转换成特定类型，如果转换失败或者对象为null，返回defaultValue。
        /// 如果传入的是可空类型：会试着转换成其真正类型后返回。
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="value">原类型对象</param>
        /// <param name="defaultValue">转换出错或者对象为空的时候的默认返回值</param>
        /// <returns>转换后的值</returns>
        public static T ToT<T>(this object value, T defaultValue)
        {
            if (value == null)
            {
                return defaultValue;
            }
            else if (value is T)
            {
                return (T)value;
            }
            try
            {
                Type typ = typeof(T);
                if (typ.BaseType == ObjectCustomExtension.ValueTypeType && typ.IsGenericType)//可空泛型
                {
                    Type[] typs = typ.GetGenericArguments();
                    return (T)Convert.ChangeType(value, typs[0]);
                }
                else
                {
                    T curobj = Activator.CreateInstance<T>();
                    foreach (var pro in curobj.GetType().GetProperties())
                    {
                        try
                        {
                            pro.SetValue(curobj, value.GetType().GetProperty(pro.Name).GetValue(value, null), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    return curobj;
                }
                //return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string DToString(this DateTime? value, string format)
        {
            return value.DToString("", format);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string DToString(this DateTime? value, string defaultValue, string format)
        {
            if (value == null)
            {
                return defaultValue;
            }

            return String.IsNullOrEmpty(format) ? ((DateTime)value).ToString() :
                ((DateTime)value).ToString(format);

        }

        /// <summary>
        /// Creator: edmund.li
        /// Date: 2012-6-27
        /// Function: 自定义拷贝方法
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="value"> 可实例化的类</param>
        /// <returns>返回拷贝后的新对象</returns>
        public static T CustomCopy<T>(this T value) where T : class
        {
            if (value == null)
            {
                return default(T);
            }

            BinarySerializer<T> ser = new BinarySerializer<T>();
            var obj = ser.Serialize(value);

            return ser.Deserialize(obj);
        }


        #region wenze.liu 自定义常用类型转换方法
        public static int ToInt32(this string value, int defaultvalue)
        {
            if (string.IsNullOrEmpty(value)) return defaultvalue;
            int ov = defaultvalue;
            bool success = int.TryParse(value, out ov);
            return success ? ov : defaultvalue;

        }
        public static Int64 ToInt64(this string value, Int64 defaultvalue)
        {
            if (string.IsNullOrEmpty(value)) return defaultvalue;
            Int64 ov = defaultvalue;
            bool success = Int64.TryParse(value, out ov);
            return success ? ov : defaultvalue;
        }
        public static Int16 ToInt16(this string value, Int16 defaultvalue)
        {
            if (string.IsNullOrEmpty(value)) return defaultvalue;
            Int16 ov = defaultvalue;
            bool success = Int16.TryParse(value, out ov);
            return success ? ov : defaultvalue;
        }
        public static Byte ToByte(this string value, Byte defaultvalue)
        {
            if (string.IsNullOrEmpty(value)) return defaultvalue;
            Byte ov = defaultvalue;
            bool success = Byte.TryParse(value, out ov);
            return success ? ov : defaultvalue;
        }
        public static bool ToBool(this string value, bool defaultvalue)
        {
            if (string.IsNullOrEmpty(value)) return defaultvalue;
            bool ov = defaultvalue;
            bool success = bool.TryParse(value, out ov);
            return success ? ov : defaultvalue;
        }
        public static DateTime ToDateTime(this string value, DateTime defaultvalue)
        {
            if (string.IsNullOrEmpty(value)) return defaultvalue;
            DateTime ov = defaultvalue;
            bool success = DateTime.TryParse(value, out ov);
            return success ? ov : defaultvalue;
        }
        public static string ToDateTimeStr(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            DateTime ov = DateTime.Now;
            bool success = DateTime.TryParse(value.Trim(), out ov);
            return success ? ov.ToString("yyyy/MM/dd") : value;
        }
        public static Guid ToGuid(this string value, Guid defaultvalue)
        {
            if (string.IsNullOrEmpty(value)) return defaultvalue;
            Guid ov = defaultvalue;
            bool success = Guid.TryParse(value, out ov);
            return success ? ov : defaultvalue;
        }
        #endregion
        /// <summary>
        /// Creator: wenze.liu
        /// Date: 2013.3.4
        /// 自定义数据转换方法，列明必须与实体类的属性名称一致
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dr">数据集合DataRow</param>
        /// <returns>返回拷贝后的新对象</returns>
        public static T toEntity<T>(this DataRow dr) where T : class
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
                        entityCols.SetValue(entity, dr[entityCols.Name], null);
                    }
                }
                catch
                {
                    continue;
                }

            }
            return (T)entity;
        }
        /// <summary>
        /// Creator: wenze.liu
        /// Date: 2013.3.4
        /// 自定义Model转换方法，两个model属性名称一致
        /// </summary>
        public static ToModel EntityAssembleFromModel<FromModel, ToModel>(this FromModel frommodel) where ToModel : class
        {
            if (frommodel == null) return default(ToModel);
            Type totype = typeof(ToModel);
            Object toentity = Activator.CreateInstance(totype);
            Type ftype = typeof(FromModel);
            Object fentity = Activator.CreateInstance(ftype);
            foreach (PropertyInfo entityCols in totype.GetProperties())
            {
                // ftype.GetProperties()[entityCols.Name].GetValue();
                var fpro = ftype.GetProperty(entityCols.Name);
                if (fpro == null) continue;
                entityCols.SetValue(toentity, fpro.GetValue(fentity, null), null);
            }
            return (ToModel)toentity;
        }
        /// <summary>
        /// Creator: wenze.liu
        /// Date: 2013.3.18
        /// 获取特性DisplayName值
        /// </summary>
        public static string GetDisplayName(this PropertyInfo info, string defaultname)
        {
            try
            {
                string name = string.Empty;
                foreach (DisplayNameAttribute attr in info.GetCustomAttributes(typeof(DisplayNameAttribute), false))
                {
                    name = attr.DisplayName;
                }
                return name;
            }
            catch
            {
                return defaultname;
            }
        }

        /// <summary>
        /// Creator: edmund.li
        /// Data: 2013-3-12
        /// Des: 获取字符串长度，中文算2个字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int StrLength(this String str)
        {
            int len = 0;
            byte[] b;

            for (int i = 0; i < str.Length; i++)
            {
                b = Encoding.Default.GetBytes(str.Substring(i, 1));
                if (b.Length > 1)
                    len += 2;
                else
                    len++;
            }

            return len;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encount">英文数量</param>
        /// <param name="chcount">中文数量</param>
        /// <returns></returns>
        public static int StrLength(this String str, out int encount, out int chcount)
        {
            encount = 0;
            chcount = 0;
            int len = 0;
            byte[] b;
            for (int i = 0; i < str.Length; i++)
            {
                b = Encoding.Default.GetBytes(str.Substring(i, 1));
                if (b.Length > 1)
                {
                    len += 2;
                    chcount++;
                }
                else
                {
                    len++;
                    encount++;
                }
            }
            return len;
        }

        /// <summary>
        /// 作者：jianquan.zheng
        /// 时间：2012-08-13
        /// 功能：截取字符串,区分中英文
        /// </summary>
        /// <param name="s">原始字符串</param>
        /// <param name="l">截取长度(如入英文需要长度，两个英文等于一个中文)</param>
        /// <returns></returns>
        public static string CutOffStr(this String s, int l)
        {
            try
            {
                if (s.StrLength() <= l)
                {
                    return s;
                }
                else
                {
                    string temp = string.Empty;
                    int len = 0;
                    byte[] b;

                    for (int i = 0; i < s.Length; i++)
                    {
                        temp += s.Substring(i, 1);
                        b = Encoding.Default.GetBytes(s.Substring(i, 1));
                        if (b.Length > 1)
                            len += 2;
                        else
                            len++;

                        if (len >= l)
                        {
                            break;
                        }
                    }
                    return temp + "...";
                }
            }
            catch (Exception ex)
            {
               // CampusLog.CampusLogHelper.Error("CutOffStr方法出错", ex);
                return s;
            }


        }

    }

    public static class GetGenericObject<T, K> where T : new() where K : IDataReader
    {
        public static T GetObject(K dataReader)
        {
            T newInstance = Activator.CreateInstance<T>();
            if (dataReader.Read()) { SetModelValue(dataReader, newInstance); } dataReader.Close(); return newInstance;
        }

        public static IList<T> GetObjectList(K dataReader)
        {
            IList<T> objectLists = new List<T>();
            while (dataReader.Read())
            {
                T newInstance = Activator.CreateInstance<T>();
                SetModelValue(dataReader, newInstance); objectLists.Add(newInstance);
            }
            dataReader.Close(); return objectLists;
        }

        private static void SetModelValue(K dataReader, T newInstance)
        {
            Type type = newInstance.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                if (CheckDataReader(dataReader, pi.Name))
                    pi.SetValue(newInstance, dataReader[pi.Name], null);
            }
        }

        private static bool CheckDataReader(K dataReader, string fieldName)
        {
            int fields = dataReader.FieldCount;
            bool state = false;
            for (int i = 0; i < fields; i++)
            {
                if (dataReader.GetName(i) == fieldName)
                    if (dataReader[i] != null && dataReader[i] != DBNull.Value)
                        state = true;
            } return state;
        }

    }

}
