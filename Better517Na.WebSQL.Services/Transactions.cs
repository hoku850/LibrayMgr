//-----------------------------------------------------------------------
// <copyright file="Transactions.cs" company="517Na Enterprises">
// * Copyright (C) 2014 517Na科技有限公司 版权所有。
// * version : 1.0.0.1
// * author  : shaoyu
// * FileName: Transactions.cs
// * history : created by shaoyu 2014/1/16 9:41:50 
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;

namespace Better517Na.LibrayMgr.Services
{
    /// <summary>
    /// Transactions类 
    /// </summary>
    public abstract class Transactions
    {
        public Dictionary<Byte, IDbConnection> DBConnections { get; protected set; }

        public Dictionary<Byte, IDbTransaction> DBTransactions { get; private set; }

        public void excute()
        {
            foreach (var connDic in DBConnections)
            {
                if (connDic.Value == null) return;
                connDic.Value.Open();
                DBTransactions.Add(connDic.Key, connDic.Value.BeginTransaction());
            }
            try
            {
                ExcuteMethods();
                foreach (var trans in DBTransactions)
                {
                    if (trans.Value != null)
                        trans.Value.Commit();
                }
            }
            catch
            {
                foreach (var trans in DBTransactions)
                {
                    if (trans.Value != null)
                        trans.Value.Rollback();
                }
                throw;
            }
            finally
            {
                foreach (var connDic in DBConnections)
                {
                    if (connDic.Value != null)
                    {
                        connDic.Value.Close();
                        connDic.Value.Dispose();
                    }

                }
            }
        }

        protected abstract void ExcuteMethods();
    }
}
