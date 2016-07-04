using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.UI.JQGrid
{
   public static class Extensions
    {
        /// <summary>
        /// 获取属性值，并转换值类型
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object getPropertyInfoValue(this PropertyInfo v1,string obj)
        {

            try
            {
                if (v1.PropertyType == typeof(String))
                {
                    return (string)obj;
                }
                else if (v1.PropertyType == typeof(DateTime))
                {
                    return DateTime.Parse(obj.ToString());
                }
                else if (v1.PropertyType == typeof(DateTime?))
                {
                    return DateTime.Parse(obj.ToString());
                }
                else if (v1.PropertyType == typeof(bool))
                {
                    return Boolean.Parse(obj.ToString());


                }
                else if (v1.PropertyType == typeof(bool?))
                {
                    return Boolean.Parse(obj.ToString());

                }
                else if (v1.PropertyType == typeof(int))
                {
                    return int.Parse(obj.ToString());

                }

                else if (v1.PropertyType == typeof(int?))
                {
                    return int.Parse(obj.ToString());

                }
                else if (v1.PropertyType == typeof(long))
                {
                    return long.Parse(obj.ToString());


                }
                else if (v1.PropertyType == typeof(decimal))
                {

                    return decimal.Parse(obj.ToString());
                }

                else if (v1.PropertyType == typeof(float))
                {

                    return float.Parse(obj.ToString());
                }

                else if (v1.PropertyType == typeof(double))
                {

                    return double.Parse(obj.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("数据格式不正确");
            }
            return null;

        }
        public static Boolean isDate(this PropertyInfo v1)
        {
            if (v1.PropertyType == typeof(DateTime))
            {
                return true;
            }
            else if (v1.PropertyType == typeof(DateTime?))
            {
                return true;
            }
            return false;
        }
    }
}
