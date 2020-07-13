using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Omipay.Core
{
    /// <summary>
    /// 分页模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageModel<T> 
    {
        public PageModel()
        {
            Data = new List<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalRecords">总记录数。</param>
        /// <param name="totalPages">页数。</param>
        /// <param name="pageSize">页面大小。</param>
        /// <param name="pageNumber">页码。</param>
        /// <param name="data">当前页面数据。</param>
        public PageModel(int totalRecords, int totalPages, int pageSize, int pageNumber, List<T> data)
        {
            this.TotalPages = totalPages;
            this.TotalRecords = totalRecords;
            this.PageSize = pageSize;
            this.PageIndex = pageNumber;
            this.Data = data;
        }

        public PageModel(int totalRecords, int pageSize, int pageNumber, List<T> data)
        {
            TotalPages = (int)Math.Ceiling((double)totalRecords / (double)PageSize);
            TotalRecords = totalRecords;
            PageSize = pageSize;
            PageIndex = pageNumber;
            Data = data;
        }


        /// <summary>
        /// 总记录数。
        /// <para>序列化为total_count</para>
        /// </summary>
        [JsonProperty("total_count")]
        public int TotalRecords { get; set; }

        /// <summary>
        /// 总页数。
        /// </summary>
        /// <para>序列化为total_page</para>
        [JsonProperty("total_page")]
        public int TotalPages { get; set; }

        /// <summary>
        /// 页面大小。
        /// </summary>
        /// <para>序列化为page_size</para>
        [JsonProperty("page_size")]
        public int PageSize { get; set; }

        /// <summary>
        /// 页码。
        /// </summary>
        /// <para>序列化为page_index</para>
        [JsonProperty("page_index")]
        public int PageIndex { get; set; }

        /// <summary>
        /// 当前页数据。
        /// </summary>
        /// <para>序列化为data</para>
        [JsonProperty("data")]
        public List<T> Data { get; set; }

    }
}
