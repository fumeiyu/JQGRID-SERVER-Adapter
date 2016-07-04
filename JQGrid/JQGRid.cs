using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.UI.JQGrid
{
    // <summary>
    /// 分页ViewModel
    /// </summary>
    public class jQGrid<T> where T : class
    {
        public jQGrid()
        { }

        /// <summary>
        /// 带4个参数的构造函数
        /// </summary>
        /// <param name="rows">每页显示行数</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="totalRecord">结果总记录数</param>
        /// <param name="jsonArray">JSON数据</param>
        public jQGrid(int rows, int currentPage, int totalRecord, IList<T> jsonArray)
        {
            TotalPage = this.CalculateTotalPage(rows, totalRecord);
            CurrentPage = currentPage;
            TotalRecord = totalRecord;
            JsonArray = jsonArray;
        }
        [JsonProperty(PropertyName = "total")]
        public int TotalPage { get; set; }
        [JsonProperty(PropertyName = "page")]

        public int CurrentPage { get; set; }
        
       [JsonProperty(PropertyName = "records")]

        public int TotalRecord { get; set; }
        [JsonProperty(PropertyName = "rows")]

        public IList<T> JsonArray { get; set; }

        /// <summary>
        /// 根据每页显示数与总记录数计算出总页数
        /// </summary>
        /// <param name="rows">每页显示数</param>
        /// <param name="totalRecord">结果总记录数</param>
        /// <returns></returns>
        public int CalculateTotalPage(int rows, int totalRecord)
        {
            return Convert.ToInt32(Math.Ceiling((double)totalRecord / (double)rows));
        }
    }
}
