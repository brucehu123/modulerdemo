﻿
using YunHu.Lib.Data.Abstractions.Enums;

namespace YunHu.Lib.Data.Query
{
    /// <summary>
    /// 查询排序模型
    /// </summary>
    public class QuerySortModel
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 排序类型
        /// </summary>
        public SortType Type { get; set; }
    }
}
