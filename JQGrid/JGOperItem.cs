using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Common.UI.JQGrid;
namespace Common.UI.JQGrid
{
    /// <summary>
    /// v2.0版本加入了FiledList ，用来筛选属性值是否要赋值 afterrequestdataconvert, SaveT 保存后的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JGOperItem<T> where T : new() 
    {
        public string oper = "";
        private T t;
        private HttpRequest request;
        public object ID;
        public List<String> FiledLists;// 使用filed
        public Action<T> beforeAdd, beforeDel, beforemodify;
        public Action<T, JGOperItem<T>> afterrequestdataconvert;//数据转换玩后执行，用来重新设置ID

        /// <summary>
        /// 保存后save对象
        /// </summary>
        public T SaveT
        {
            get
            {
                return t;
            }
        }
        public JGOperItem(HttpRequest request, Action<T> beforeAdd, Action<T> beforemodify, Action<T> beforeDel)
     : this(request, beforeAdd, beforemodify, beforeDel, null,null) { }
        public JGOperItem(HttpRequest request):this(request,null,null,null)
        {

        }

        public JGOperItem(HttpRequest request, Action<T> beforeAdd, Action<T> beforemodify, Action<T> beforeDel, Action<T, JGOperItem<T>> afterrequestdataconvert, List<String> filedList)
        {

            this.beforeAdd = beforeAdd;
            this.request = request;
            this.beforemodify = beforemodify;
            this.beforeDel = beforeDel;
            this.afterrequestdataconvert = afterrequestdataconvert;
            FiledLists = filedList;
        }


        void ConvertFromData()  {

            T t = new T();
            this.t = this.request.HttpRequestConvertToT<T>(FiledLists, out ID);
            oper = request.Form["oper"];
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
                if (FiledFiter(v1.Name)) {
                    continue;
                }
                if (request.Form[v1.Name] != null)
                {
                    var obj = v1.getPropertyInfoValue(request.Form[v1.Name]);
                    v1.SetValue(t, obj);
                }
            }
        }

        private bool FiledFiter(string name) {
            if (FiledLists == null)
                return false;
            return FiledLists.Find(a => a == name) == null ? true : false;
        }
        public bool  DoDataAction() {

            ConvertFromData();
            if (afterrequestdataconvert != null) {
                afterrequestdataconvert(t,this);
            }
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
            if (beforemodify!=null) {
                beforemodify(n);
            }
            f.Invoke(n, new object[0]);
            t = n;
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
