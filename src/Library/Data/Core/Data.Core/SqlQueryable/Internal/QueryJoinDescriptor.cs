﻿using System.Linq.Expressions;
using YunHu.Lib.Data.Abstractions.Entities;
using YunHu.Lib.Data.Abstractions.Enums;

namespace YunHu.Lib.Data.Core.SqlQueryable.Internal
{
    /// <summary>
    /// 查询表连接信息
    /// </summary>
    internal class QueryJoinDescriptor
    {
        /// <summary>
        /// 连接类型
        /// </summary>
        public JoinType Type { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 实体信息
        /// </summary>
        public IEntityDescriptor EntityDescriptor { get; set; }

        /// <summary>
        /// 连接条件
        /// </summary>
        public LambdaExpression On { get; set; }
    }
}
