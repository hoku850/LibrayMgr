// <copyright file="MDbTransaction.cs" company="517na.com">
//     Copyright (c) 517na.com. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.Data;

namespace Better.Infrastructures.DBUtility
{
    /// <summary>
    /// 多数据库支持事务操作类
    /// </summary>
    public abstract class MultiDbTransaction
    {
        /// <summary>
        /// 事务类型，默认为未提交读
        /// </summary>
        public IsolationLevel TransactionIsolationLevel = IsolationLevel.ReadUncommitted;

        /// <summary>
        /// 是否启数据库动事务（IDbConnection.BeginTransaction）
        /// </summary>
        private bool isBeginTransaction = true;

        /// <summary>
        /// 多连接集合
        /// </summary>
        public Dictionary<byte, IDbConnection> Connections { get; protected set; }

        /// <summary>
        /// 对应事务集合
        /// </summary>
        public Dictionary<byte, IDbTransaction> Transactions { get; private set; }

        /// <summary>
        /// 执行方法
        /// </summary>
        public void Execute()
        {
            this.Transactions = new Dictionary<byte, IDbTransaction>();
            foreach (var connDic in this.Connections)
            {
                if (connDic.Value == null)
                {
                    return;
                }

                connDic.Value.Open();


                if (this.isBeginTransaction)
                {
                    this.PreTransactionBegin();

                    this.Transactions.Add(connDic.Key, connDic.Value.BeginTransaction(this.TransactionIsolationLevel));
                }
            }

            try
            {
                this.ExcuteMethods();
                foreach (var trans in this.Transactions)
                {
                    if (trans.Value != null)
                    {
                        trans.Value.Commit();

                        this.Committed();
                    }
                }
                this.AllCommitted();
            }
            catch
            {
                foreach (var trans in this.Transactions)
                {
                    if (trans.Value != null)
                    {
                        trans.Value.Rollback();

                        this.Rollbacked();
                    }
                }
                this.AllRollbacked();
                throw;
            }
            finally
            {
                foreach (var connDic in this.Connections)
                {
                    if (connDic.Value != null)
                    {
                        connDic.Value.Close();
                        connDic.Value.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 抽象业务执行方法
        /// </summary>
        protected abstract void ExcuteMethods();

        /// <summary>
        /// 事务提交后处理动作，如果不要求开启事物(isBeginTransaction=false)将不会执行此方法
        /// </summary>
        protected virtual void Committed()
        {
        }

        /// <summary>
        /// 所有事务提交后处理动作,若没有开启事务，则在核心方法ExcuteMethods处理完后执行
        /// </summary>
        protected virtual void AllCommitted()
        {
        }

        /// <summary>
        /// 事务回滚后处理动作，如果不要求开启事物(isBeginTransaction=false)将不会执行此方法
        /// </summary>
        protected virtual void Rollbacked()
        {
        }

        /// <summary>
        /// 所有事务回滚后处理动作,若没有开启事务，则在错误处理完后执行
        /// </summary>
        protected virtual void AllRollbacked()
        {
        }

        /// <summary>
        /// 开启事务（BeginTransaction）之前执行的方法，如果不要求开启事物(isBeginTransaction=false)将不会执行此方法
        /// </summary>
        protected virtual void PreTransactionBegin()
        {
        }
    }
}
