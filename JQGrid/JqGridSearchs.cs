using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using System.Web;
using System.Reflection;

namespace Common.UI.JQGrid
{
    /// <summary>
    /// V2.1 版本加入输出内容控制,FiledLists字段列 增加 sord，多排序，使用，分隔多个order，修正MaxResults bug ,值为每页大小，而不是最大值
    /// 
    /// V2.3版本 加入了通过非 httprequest 获取数据 ，加入新的构造函数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JqGridSearch<T> where T : class, new()
    {
        public List<String> FiledLists;// 使用filed  过滤列表或者只选择转换列表
        bool fiterType;
        public int rows, pageindex;
        public string sidx;
        public string sord;
        public bool isSearch;
        JqGridFitler fiter; //可以用来修改password
        ICriterion defaultfiter;

        public int FirstResult
        {
            get { return (pageindex - 1) * rows; }
        }

        public int MaxResults
        {
            get { return   rows; }
        }
        public JqGridSearch(HttpRequest request) : this(request, null)
        {


            //if (request.QueryString["searchingtext"] != null)
            //{
            //    searchingtext = request.QueryString["searchingtext"].ToString();
            //}
        }

        /// <summary>
        ///  获取特定页面记录数
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="pageindex"></param>
        /// <param name="sord">排序</param>
        /// <param name="sidx">排序字段</param>
        /// <param name="defaultfiter">默认的条件</param>
        public JqGridSearch(int rows, int pageindex, string sord, string sidx, ICriterion defaultfiter) {

            this.rows = rows;
            this.pageindex = pageindex;
            this.sord = sord;
            this.sidx = sidx;
            this.defaultfiter = defaultfiter;
        }


        public JqGridSearch(HttpRequest request, ICriterion defaultfiter)
        {
            //rows = request.QueryString["rows"].ToInt();
            //pageindex = int.Parse(request.QueryString["page"]);
            //sord = request.QueryString["sord"].ToString();
            //sidx = request.QueryString["sidx"].ToString();
            rows = request.QueryString["rows"].ToInt();
            pageindex = request.QueryString["page"].ToInt();
            sord = request.QueryString["sord"].ToStringNoNull("");
            sidx = request.QueryString["sidx"].ToStringNoNull();

            isSearch = request.QueryString["_search"].ToBool();
            if (isSearch)
            {
                string fiters = request.QueryString["filters"].ToStringNoNull();
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


        public Order[] getorder()
        {

            Order[] o;
            if (sidx != "")
            {
                string[] tt = sidx.Split(',');
                o = new Order[tt.Length];
                string[] sords = sord.Split(',');
                for (int i = 0; i < tt.Length; i++) {
                    bool asc ;
                    if (sords.Length >= i)
                    {
                        asc = sords[0] == "asc" ? true : false;
                    }
                    else {
                        asc = sords[i] == "asc" ? true : false;

                    }
                    o[i] = new Order(tt[i], asc);
                }
                //o = new Order[1];
                //o[0] = new Order(sidx, sord == "asc" ? true
                //        : false);
                return o;
            }
            else
                return null;
        }

        public JqGridSearch()
        {
        }
        private List<T> _Search(out long recordcount, bool needCount)
        {
            recordcount = 0;
            T t = new T();
            List<T> list = new List<T>();
            var o = getorder();
            var ic = getCriterion(Fiter);
            //  var p = t.GetType().GetMethod("RecordCount");

            List<ICriterion> lists = new List<ICriterion>();
            if (defaultfiter != null)
                lists.Add(defaultfiter);
            MethodInfo p = null;
            if (needCount)
            {
                p = t.GetType().GetMethod("RecordCount", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, null, new Type[1] { lists.ToArray().GetType() }, null);
            }
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

                if (needCount)
                {
                    recordcount = (long)p.Invoke(null, new object[1] { lists.ToArray() });
                }

            }
            else
            {
                var t1 = t.GetType().BaseType.GetMethod("SlicedFindAll", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, null, new Type[4] { typeof(int), typeof(int), o.GetType(), lists.ToArray().GetType() }, null);
                list.AddRange((T[])t1.Invoke(null, new object[4] { FirstResult, MaxResults, o, lists.ToArray() }));
                if (needCount)
                {
                    recordcount = (long)p.Invoke(null, new object[1] { lists.ToArray() });
                }
            }




            //获得IC

            return list;



            //    return  obj.SlicedFindAll(FirstResult, MaxResults);


        }




        public jQGrid<T> Search(bool needCount)
        {
            T t = new T();
            long totalrecord;
            var l = _Search(out totalrecord, needCount);
            jQGrid<T> jg = new jQGrid<T>(rows, pageindex, (int)totalrecord, l);
            return jg;
        }
        public jQGrid<T> Search()
        {

            //查找内容 
            return Search(true);

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


        public ICriterion ToCriterion()
        {
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
                    if (vs != null)
                    {
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
            return getSql(fiter);

        }
        string getSql(JqGridFitler fiter)
        {

            string s = "";
            foreach (var rules in fiter.rules)
            {

                s += string.Format("{0} {1} {2} {3} ", rules.field, rules.op, rules.data, fiter.groupOp);
            }

            if (fiter.groups.Count > 0)
            {

                s += " ( ";
            }
            foreach (var v in fiter.groups)
            {
                s += getSql(v);

            }
            if (fiter.groups.Count > 0)
            {
                s += " ) ";
            }

            if (fiter.groups.Count == 0)
            {
                s = s.Remove(s.Length - fiter.groupOp.Length - 1);
            }

            return s;
        }
    }
}
