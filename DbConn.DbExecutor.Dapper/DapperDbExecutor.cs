using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DbConn.DbExecutor.Abstract;

namespace DbConn.DbExecutor.Dapper
{
    public class DapperDbExecutor : IDbExecutor
    {
        private bool _isDisposed;

        public DapperDbExecutor(string connectionString, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            bool isNeedTransactional = true)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            InnerConnection = new SqlConnection(connectionString);
            InnerConnection.Open();
            if (isNeedTransactional)
            {
                Transaction = BeginTransaction(isolationLevel);
            }
        }

        private IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return InnerConnection.BeginTransaction(isolationLevel);
        }

        /// <inheritdoc />
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Transaction?.Dispose();

                    if (InnerConnection != null)
                    {
                        if (InnerConnection.State != ConnectionState.Closed)
                        {
                            InnerConnection.Close();
                        }
                        InnerConnection.Dispose();
                    }
                }
            }
            _isDisposed = true;
        }

        #region Query

        public IDbConnection InnerConnection { get; }

        public IDbTransaction Transaction { get; private set; }

        public void UseTransaction(IDbTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));
            Transaction = transaction;
        }

        public IEnumerable<TResult> Query<TResult>(string sql)
        {
            return InnerConnection.Query<TResult>(sql, transaction: Transaction);
        }

        public IEnumerable<TResult> Query<TResult>(string sql, object param)
        {
            return InnerConnection.Query<TResult>(sql, param, Transaction);
        }

        #endregion

        #region Execute

        public virtual void Execute(string sql)
        {
            InnerConnection.Execute(sql, transaction: Transaction);
        }

        public virtual void Execute(string sql, object param, CommandType? commandType = default(CommandType?),
            int? commandTimeout = default(int?))
        {
            InnerConnection.Execute(sql, param, Transaction, commandTimeout, commandType);
        }

        public virtual void Commit()
        {
            Transaction?.Commit();
        }

        public virtual void Rollback()
        {
            Transaction?.Rollback();
        }

        #endregion
    }
}