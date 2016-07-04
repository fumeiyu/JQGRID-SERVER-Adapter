using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.UI.JQGrid
{
   public class JGOperItem<T> where T : new()
    {
        public string oper = "";
        private T t;
        private HttpRequest request;
        public object ID;

        public Action<T> beforeAdd, beforeDel, beforemodify;

       
        public JGOperItem( HttpRequest request, Action<T> beforeAdd,Action<T> beforemodify,Action<T> beforeDel)
        {
            T t = new T();
            this.t = t;
            this.beforeAdd = beforeAdd;
            this.request = request;
            this.beforemodify = beforemodify;
            this.beforeDel = beforeDel;


        }
        public JGOperItem(HttpRequest request):this(request,null,null,null)
        {

        }
        void ConvertFromData()  {

            var form = request.Form;
            var v=t.GetType().GetProperties();

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
                if (form[v1.Name] != null)
                {

                    //过滤默认的小写id；

                    if (v1.Name.ToUpper() == nameof(ID)) {
                        if (form["oper"] == "add") {
                            continue;
                        }
                    }
                    var OBJ1 = form[v1.Name];
                    var newobj = v1.getPropertyInfoValue(OBJ1);
                    if (newobj != null)
                    {
                        v1.SetValue(t, newobj);
                    }
                    if (v1.Name.ToUpper() == nameof(ID))
                    {
                        ID = newobj;
                    }

                }

            }
            oper = form["oper"];
            //return t;
        }

        //private static object ConvertToT(object obj, System.Reflection.PropertyInfo v1)
        //{
        //    try {
        //        if (v1.PropertyType == typeof(String))
        //        {
        //            return (string)obj;

        //        }
        //        else if (v1.PropertyType == typeof(DateTime))
        //        {
        //            return DateTime.Parse(obj.ToString());
        //        }
        //        else if (v1.PropertyType == typeof(bool))
        //        {
        //            return Boolean.Parse(obj.ToString());


        //        }

        //        else if (v1.PropertyType == typeof(int))
        //        {
        //            return int.Parse(obj.ToString());

        //        }
        //        else if (v1.PropertyType == typeof(long))
        //        {
        //            return long.Parse(obj.ToString());


        //        }
        //    }
        //    catch (Exception ex) {
        //        throw new Exception(v1.PropertyType + "没有定义的类型转换");
        //    }
        //    return null;
        //}

        //public void ConvertFromData() 
        //{
        //    var form = this.request.Form;

        //    //先判断是否是更新，或者删除
        
        //    oper = form["oper"];
        //}


        void SetTValue(T t) {
            var v = t.GetType().GetProperties();
            foreach (var v1 in v)
            {
                if (request.Form[v1.Name] != null)
                {
                    var obj = v1.getPropertyInfoValue(request.Form[v1.Name]);
                    v1.SetValue(t, obj);
                }
            }
        }
   
        public bool  DoDataAction() {
            ConvertFromData();    
            switch (oper) {
                case "add":
                    if (beforeAdd != null) {
                        beforeAdd(t);
                    }
                    Add();
                    break;
                case "edit":
                    Modify();
                    break;
                case "del":
                    Delete();
                    break;
            }
            ///判断操作
            //switch (this.oper) {
            //    case "add":

            //}
            return true;
        }

        bool Add() {
            var f =t.GetType().GetMethod("Save");
            f.Invoke(t,new object[0]);
            return true;
        }
        bool Modify() {

           var n = Find();
           var f = n.GetType().GetMethod("Update");
            //重新复制
            SetTValue(n);
            f.Invoke(n, new object[0]);
            return true;

        }
        bool Delete() {

            var n = Find();
            if (beforeDel != null) {
                beforeDel(n);
            }
            var f = n.GetType().GetMethod("Delete");
            f.Invoke(n, new object[0]);

            return true;
        }
        T Find() {
            var t1 = t.GetType().BaseType.GetMethod("Find");
            //SELECT 新T;
            var newt =(T)t1.Invoke(null, new object[1] { ID });
            return newt;
        }
    }
}
