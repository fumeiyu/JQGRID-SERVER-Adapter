using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.UI.JQGrid
{
  public  class JqGridFitler
    {
        public string groupOp;
        public List<Rules> rules;
        public List<JqGridFitler> groups;
    }

    public class Rules {

        public String field, op, data;

    }
}
