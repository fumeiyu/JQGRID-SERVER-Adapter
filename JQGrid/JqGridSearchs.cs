using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using System.Web;

namespace Common.UI.JQGrid
{
    public class JqGridSearch<T> where T : class, new()
    {


        public int rows, pageindex;
        public string sidx;
        public string sord;
        public bool isSearch;
        JqGridFitler fiter;
        ICriterion defaultfiter;

        public int FirstResult
        {
            get { return (pageindex - 1) * rows; }
        }

        public int MaxResults
        {
            get { return (pageindex - 1) * rows + rows; }
        }
        public JqGridSearch(HttpRequest request):this(request, null)
        {

        
            //if (request.QueryString["searchingtext"] != null)
            //{
            //    searchingtext = request.QueryString["searchingtext"].ToString();
            //}
        }

        public JqGridSearch(HttpRequest request,ICriterion defaultfiter)
        {
            rows = int.Parse(request.QueryString["rows"]);
            pageindex = int.Parse(request.QueryString["page"]);
            sord = request.QueryString["sord"].ToString();
            sidx = request.QueryString["sidx"].ToString();
            isSearch = bool.Parse(request.QueryString["_search"].ToString());
            if (isSearch)
            {
                string fiters = request.QueryString["filters"].ToString();
                if (fiters != "")
                {
                    Fiter = Newtonsoft.Json.JsonConvert.DeserializeObject<JqGridFitler>(fiters);
                }
            }
            this.defaultfiter = defaultfiter;

            //if (request.QueryString["searchingtext"] != null)
            //{
            //    searchingtext = request.QueryString["searchingtext"].ToString();
            //}
        }


        public Order[] getorder() {

            Order[] o;
            if (sidx != "")
            {
                o = new Order[1];
                o[0] = new Order(sidx, sord == "asc" ? true
                        : false);
                return o;
            }
            else
                return null;
        }

        public JqGridSearch() {
        }
        private List<T> _Search(out long recordcount)
        {

            T t = new T();
            List<T> list = new List<T>() ;
            var o = getorder();
            var ic = getCriterion(Fiter);
          //  var p = t.GetType().GetMethod("RecordCount");

             List<ICriterion> lists = new List<ICriterion>();
            if (defaultfiter != null)
                lists.Add(defaultfiter);
             var p = t.GetType().GetMethod("RecordCount",System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, null, new Type[1] {lists.ToArray().GetType() },null);

            if (ic == null)
            {

                //if (o == null)
                //{

                //    var z = l.GetType().BaseType.GetMethod("SlicedFindAll", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, null, new Type[3] { typeof(int), typeof(int), lists.ToArray().GetType() }, null);

                //    var t1 = tGetType().BaseType.GetMethod("SlicedFindAll", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, null, new Type[3] { typeof(int), typeof(int), lists.ToArray().GetType() }, null);
                //    list = (List<T>)t1.Invoke(null, new object[2] { FirstResult, MaxResults });
                //}
                //else {
                //    var t1 = t.GetType().BaseType.GetMethod("SlicedFindAll", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, null, new Type[4] { typeof(int), typeof(int), o.GetType() }, null);
                //    list = (List<T>)t1.Invoke(null, new object[3] { FirstResult, MaxResults, o });

                //}
            }
            else
            {

                lists.Add(ic);

            }
            if (o == null)
            {

                var t1 = t.GetType().BaseType.GetMethod("SlicedFindAll", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, null, new Type[3] { typeof(int), typeof(int), lists.ToArray().GetType() }, null);

               list.AddRange((T[])t1.Invoke(null, new object[3] { FirstResult, MaxResults, lists.ToArray() }));

                 
              //  list = (List<T>)t1.Invoke(null, new object[3] { FirstResult, MaxResults, lists.ToArray() });

                recordcount = (long)p.Invoke(null, new object[1] { lists.ToArray() });


            }
            else
            {
                var t1 = t.GetType().BaseType.GetMethod("SlicedFindAll", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, null, new Type[4] { typeof(int), typeof(int), o.GetType(), lists.ToArray().GetType() }, null);
                list.AddRange((T[])t1.Invoke(null, new object[4] { FirstResult, MaxResults, o, lists.ToArray() }));
                recordcount = (long)p.Invoke(null, new object[1] { lists.ToArray() });

            }




            //获得IC

            return list;



            //    return  obj.SlicedFindAll(FirstResult, MaxResults);


        }
        public jQGrid<T> Search() {
         
            //查找内容 

            T t = new T();
            long totalrecord;
            var l=_Search(out totalrecord);
            jQGrid<T> jg = new jQGrid<T>(rows,pageindex,(int)totalrecord,l);
            return jg;

        }
        //private int getTotalRecord<T>() {
        //    T t = new T();
        //    var p= t.GetType().GetMethod("RecordCount");


        //}
        public JqGridSearch(string fiters)
        {


            Fiter = Newtonsoft.Json.JsonConvert.DeserializeObject<JqGridFitler>(fiters);

        }

        public JqGridFitler Fiter
        {
            get
            {
                return fiter;
            }

            set
            {
                fiter = value;
            }
        }


        public ICriterion ToCriterion() {
            return getCriterion(fiter);

        }

        ICriterion getCriterion(JqGridFitler fiter)
        {

            if (fiter == null)
                return null;
            List<ICriterion> list = new List<ICriterion>();
            if (fiter.rules.Count > 0)
            {

                var s = new RulesTransformation<T>(fiter.rules, fiter.groupOp);
                list.Add(s.ToCriterion());
            }
            if (fiter.groups != null)
            {
                foreach (var v in fiter.groups)
                {
                    var vs = getCriterion(v);
                    if (vs != null) { 
                    list.Add(getCriterion(v));
                    }

                }
            }
            ICriterion newc = null;
            list.ForEach(a =>
            {
                if (newc == null)
                {
                    newc = a;
                }
                else
                {
                    if (fiter.groupOp.ToUpper() == "AND")
                    {
                        newc = Expression.And(newc, a);
                    }
                    else
                    {
                        newc = Expression.Or(newc, a);
                    }
                }
            });


            return newc;

        }
        public String ToSql()
        {
          return   getSql(fiter);

        }
        string getSql(JqGridFitler fiter) {

            string s = "";
            foreach (var rules in fiter.rules)
            {

                s += string.Format("{0} {1} {2} {3} ", rules.field, rules.op, rules.data, fiter.groupOp);
            }

            if (fiter.groups.Count > 0) {

                s += " ( ";
            }
            foreach(var v in fiter.groups)
            {
                s += getSql(v);

            }
            if (fiter.groups.Count > 0)
            {
                s += " ) ";
            }

            if (fiter.groups.Count == 0) {
                s=s.Remove(s.Length - fiter.groupOp.Length-1);
            }

                return s;
        }
    }
}
