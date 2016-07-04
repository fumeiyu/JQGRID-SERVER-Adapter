using System;
using System.Collections.Generic;
using System.Data.Sql;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
namespace Common.UI.JQGrid
{


    public class RulesTransformation<T> where T : class, new()
    {
        //static List<dynamic> operlist = new List<dynamic>();
        ////  static string operlist = "odata: [{ oper:'eq', text:'={0}'},{ oper:'ne', text:'!={0}'},{ oper:'lt', text:'<{0]'},{ oper:'le', text:'<={0}'},{ oper:'gt', text:'>{0}'},{ oper:'ge', text:'>='},{ oper:'bw', text:'not like \"%{0}\"'},{ oper:'bn', text:'like \"{0}%\"'},{ oper:'in', text:'in ({0})'},{ oper:'ni', text:'not in ({0})'},{ oper:'ew', text:'like \"{0}%\"'},{ oper:'en', text:'not like  \"{0}%\"'},{ oper:'cn', text:'\'%{0}%\"''},{ oper:'nc', text:'not like \'%{0}%\''}]";
        //static RulesTransformation() {
        //    //
        //    dynamic d = new System.Dynamic.ExpandoObject();
        //    d.oper = "eq";
        //    d.text = "={0}";
        //    operlist.Add(d);
        //}
        public RulesTransformation() { Rules = new List<RulesTransformation<T>>(); }
        //public string operchar;
        //public string filed;
        //public string data;
        //public bool needQuotes;//需要引号，string，date ，需要引号

        private List<Rules> rules;
        private String groupby; //AND OR
        public RulesTransformation(List<Rules> rules,string groupby) {
            this.rules = rules;
            this.groupby = groupby;
       
            T item = new T();
            //Rules = new List<RulesTransformation<T>>();

            //foreach (Rules r in rules) {

            //    RulesTransformation<T> rule = new RulesTransformation<T>();

            //    var p = item.GetType().GetProperty(r.data);
            //    var db = GetPropertySqlDbType(p, out rule.needQuotes);
            //    rule.data = r.data;
            //    rule.operchar = "";
            //    // rule.operchar=
            //    //判断自动熟悉;

            //}

        //    var v = new RulesTransformation<JGOperItem>();
        }

        public ICriterion ToCriterion()
        {


            List<ICriterion> ic = new List<ICriterion>(); ;
            foreach (Rules r in rules)
            { 

                ic.Add(getExpression(r));
                //RulesTransformation<T> rule = new RulesTransformation<T>();

                //var p = item.GetType().GetProperty(r.data);
                //var db = GetPropertySqlDbType(p, out rule.needQuotes);
                //rule.data = r.data;
                //rule.operchar = "";
                // rule.operchar=
                //判断自动熟悉;

            }

            ICriterion newc = null;
            ic.ForEach(a =>
            {
                if (newc == null)
                {
                    newc = a;
                }
                else
                {
                    if (groupby.ToUpper() == "AND")
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


    
       AbstractCriterion  getExpression(Rules r) {
         T t = new T();
          AbstractCriterion simple = null;
          var v1=  t.GetType().GetProperty(r.field);
            object obj = v1.getPropertyInfoValue( r.data);
            switch (r.op.ToLower())
            {
                case "eq":
                    if (v1.isDate()) {
                        DateTime dt = (DateTime)obj;
                        simple= Restrictions.Between( r.field, obj, dt.AddDays(1));
                    }
                    else { 
                    simple = Restrictions.Eq(r.field, obj);
                    }
                    break;
                case "ne":
                    simple = Restrictions.Not(Restrictions.Eq(r.field, obj));
                    break;
                case "lt":
                    simple = Restrictions.Lt(r.field, obj);

                    break;
                case "le":
                    simple = Restrictions.Le(r.field, obj);

                    break;
                case "gt":
                    simple = Restrictions.Gt(r.field, obj);

                    break;
                case "ge":
                    simple = Restrictions.Ge(r.field, obj);

                    break;
                case "bw":
                    simple = Restrictions.Like(r.field, obj.ToString(), MatchMode.Start);

                    break;
                case "bn":
                    simple = Restrictions.Not(Restrictions.Like(r.field, obj.ToString(), MatchMode.Start));

                    break;
                case "in":

                    simple = Restrictions.In(r.field, new object[1] { obj });

                    break;
                case "ni":
                    simple = Restrictions.Not(Restrictions.In(r.field, new object[1] { obj }));

                    break;
                case "ew":
                    simple = Restrictions.Like(r.field, obj.ToString(), MatchMode.End);

                    break;
                case "en":
                    simple = Restrictions.Not(Restrictions.Like(r.field, obj.ToString(), MatchMode.End));

                    break;
                case "cn":
                    simple = Restrictions.Like(r.field, obj.ToString(), MatchMode.Anywhere);

                    break;
                case "nc":
                    simple = Restrictions.Not(Restrictions.Like(r.field, obj.ToString(), MatchMode.Anywhere));
                    break;
                default:
                    throw new Exception("未找到相应操作符");
            }
            return simple;
        }
        //void DO() {
        //    var v = new RulesTransformation<JGOperItem>();
        //    foreach (var t in v.Rules) {
               
        //    }
        //}
        public List<RulesTransformation<T>> Rules;


        private string operConvert() {
            return "";

        }


        private static object ConvertToT(object obj, System.Reflection.PropertyInfo v1)
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
                else if (v1.PropertyType == typeof(decimal)) {

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
            catch (Exception ex) {
                throw new Exception("数据格式不正确");
            }
            return null;
        }
        private static SqlDbType  GetPropertySqlDbType(System.Reflection.PropertyInfo v1,out bool needQuotes)
        {
            needQuotes = true;
            try
            {
                if (v1.PropertyType == typeof(String))
                {
                    needQuotes = false;
                    return SqlDbType.NVarChar;

                }
                else if (v1.PropertyType == typeof(DateTime))
                {
                    return SqlDbType.DateTime;
                }
                else if (v1.PropertyType == typeof(bool))
                {
                    needQuotes = false;

                    return SqlDbType.Bit;


                }

                else if (v1.PropertyType == typeof(int))
                {
                    needQuotes = false;
                    return SqlDbType.Int;

                }
                else if (v1.PropertyType == typeof(long))
                {
                    return SqlDbType.BigInt;

                }
            }
            catch (Exception ex) {  }
            throw new Exception("无效类型");
        }
    }
}
