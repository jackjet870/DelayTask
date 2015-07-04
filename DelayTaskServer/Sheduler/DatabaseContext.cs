﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using DelayTaskServer.Tasks;

namespace DelayTaskServer.Sheduler
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// 约定生成
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 移除表名复数约定
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Sql任务
        /// </summary>
        public DbSet<SqlDelayTask> SqlDelayTask { get; set; }

        /// <summary>
        /// Http任务
        /// </summary>
        public DbSet<HttpDelayTask> HttpDelayTask { get; set; }

        /// <summary>
        /// 失败的Http任务
        /// </summary>
        public DbSet<FailureHttpTask> FailureHttpTask { get; set; }

        /// <summary>
        /// 失败的Sql任务
        /// </summary>
        public DbSet<FailureSqlTask> FailureSqlTask { get; set; }
    }
}