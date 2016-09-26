using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
                throw new Exception("数据格式不正确" +v1.Name+": " + obj);
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

        /// <summary>
        /// string 转换成int，如果不是数字，返回默认值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static int ToInt(this string str, int defaultvalue=1) {

            if (str == null) {
                return defaultvalue;
            }
            try
            {
                return Convert.ToInt32(str);
            }
            catch { }
            return defaultvalue;
        }
        /// <summary>
        /// 如果NULL输出 defaultstring，去掉NULL
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultstring"></param>
        /// <returns></returns>
        public static string ToStringNoNull(this string str, string defaultstring = "") {
            if (str == null)
                return defaultstring;
            return str;
        }
        /// <summary>
        /// string 转换boolean，转换失败输出默认值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static bool ToBool(this string str, bool defaultvalue = false) {
            if (str == null)
                return defaultvalue;
            try {
                return bool.Parse(str);
            }

            catch { }
            return defaultvalue;
        }
        public static T HttpRequestConvertToT<T>(this HttpRequest request, out object ID) where T : new()
        {
            T t = new T();
            ID = 0;
            var form = request.Form;
            var v = t.GetType().GetProperties();

            //反过来写
            //foreach (var key in form.Keys)
            //{
            //    string k = key.ToString();
            //    if ( key.ToString() == oper)
            //        continue;
            //    if (key.ToString() == "id")
            //    {
            //        if (form[k] == "_empty")
            //            continue;
            //        k = "ID";
            //        ID = int.Parse(form[k]);
            //    }
            //    var OBJ1 = form[k];

            //    var t1 = t.GetType().GetProperty(k);


            //    if (t1 != null)
            //    {
            //        var newobj = t1.getPropertyInfoValue(OBJ1);
            //        t1.SetValue(t, newobj);
            //    }

            //    //查找

            //}

            foreach (var v1 in v)
            {
                if (form[v1.Name] != null || (v1.isPrimaryKeyAttribute()))
                {

                    //过滤默认的小写id；

                    if (v1.Name.ToUpper() == nameof(ID) || v1.isPrimaryKeyAttribute())
                    {
                        if (form["oper"] == "add")
                        {
                            continue;
                        }
                        if (form["oper"] == "del")
                        {

                            var tkeyidlist = form[v1.Name] == null ? form["id"] : form[v1.Name];

                            if (tkeyidlist.IndexOf(",") > -1) // 多个删除的ID;
                            {
                                ID = tkeyidlist;
                                continue;
                            }

                        }
                    }
                    var OBJ1 = form[v1.Name] == null ? form["id"] : form[v1.Name];
                    var newobj = v1.getPropertyInfoValue(OBJ1);
                    if (newobj != null)
                    {
                        v1.SetValue(t, newobj);
                    }
                    if (v1.Name.ToUpper() == nameof(ID)) //判断主键 
                    {
                        ID = newobj;
                        continue;
                    }
                    if (v1.isPrimaryKeyAttribute()) {
                        ID = newobj;
                    }
                }

            }
            return t;
        }
        public  static  bool isPrimaryKeyAttribute(this PropertyInfo v1) {
            try
            {
                var attribute = v1.GetCustomAttributes(true);
                if (attribute != null && attribute.Length > 0)
                {

                    foreach (var tt in attribute)
                    {

                        if (tt is PrimaryKeyAttribute)
                        {
                            //获取值
                            return true;
                        }
                    }
                }
            }
            catch { }
            return false;
        }
        public static T HttpRequestConvertToT<T>(this HttpRequest request, List<String> filedList,bool fiterType, out object ID) where T : new()
        {
            if (filedList == null)
            {
                return  request.HttpRequestConvertToT<T>(out ID);

            }
            T t = new T();
            ID = 0;
            var form = request.Form;
            var v = t.GetType().GetProperties();
            foreach (var v1 in v)
            {

               //如果是主键转换成ID;

                if (form[v1.Name] != null ||(v1.isPrimaryKeyAttribute()))
                {

                    //过滤默认的小写id；

                    if (v1.Name.ToUpper() == nameof(ID) || v1.isPrimaryKeyAttribute())
                    {
                        if (form["oper"] == "add")
                        {
                            continue;
                        }
                        if (form["oper"] == "del") {

                           var tkeyidlist= form[v1.Name] == null ? form["id"] : form[v1.Name];

                            if (tkeyidlist.IndexOf(",") > -1) // 多个删除的ID;
                            {
                                ID = tkeyidlist;
                                continue;
                            }
                          
                        }
                        //增加删除列;
                    }

                    var OBJ1 = form[v1.Name] == null ? form["id"] : form[v1.Name];
                    if (fiterType)
                    {
                        if (filedList.Find(a => a == v1.Name) != null)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        ///设置值判断是否需要设置
                        ///
                        if (filedList.Find(a => a == v1.Name) == null)
                        {
                            continue;
                        }
                    }
                    var newobj = v1.getPropertyInfoValue(OBJ1);

                    if (newobj != null)
                    {
                        v1.SetValue(t, newobj);

                        ///如果true，过滤string 数组


                    }
                    if (v1.Name.ToUpper() == nameof(ID)) //判断主键 
                    {
                        ID = newobj;
                        continue;
                    }
                    if (v1.isPrimaryKeyAttribute())
                    {
                        ID = newobj;
                    }

                }

            }
            return t;
        }





    }
}
