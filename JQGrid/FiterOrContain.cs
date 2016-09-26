using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.UI.JQGrid
{
    public class FiterOrContain
    {
        public string[] FiterList;//过滤或者保护列表
        public bool IsFiter;//是过滤？
        public FiterOrContain(string[] fiterorcontainList, bool isftier) {
            this.FiterList = fiterorcontainList;
            this.IsFiter = isftier;
        }
        public FiterOrContain(string[] fiterList) : this(fiterList, true) {

        }

        public FiterOrContain(HttpRequest request) {
            try
            {
                if (request.QueryString["_FiterList"] != null) {
                    this.FiterList = request.QueryString["_FiterList"].TrimEnd(',').Split(',');
                }
                this.IsFiter= request.QueryString["_IsFiter"].ToBool(true);

            }
            catch {
                this.IsFiter = false;
                this.FiterList = null;
            }


        }

    }
}
