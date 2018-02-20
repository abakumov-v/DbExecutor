using System;
using System.Collections.Generic;
using System.Data;

namespace DbConn.DbExecutor.Abstract
{
    /// <inheritdoc />
    /// <summary>
    /// Wrapper for getting data and execute scripts for database
    /// </summary>
    public interface IDbExecutor : IDisposable
    {
        IDbConnection InnerConnection { get; }
        IDbTransaction Transaction { get; }

        void UseTransaction(IDbTransaction transaction);

        IEnumerable<TResult> Query<TResult>(string sql);
        IEnumerable<TResult> Query<TResult>(string sql, object param);

        void Execute(string sql);
        void Execute(string sql, object param, CommandType? commandType = default(CommandType?), int? commandTimeout = default(int?));

        void Commit();
        void Rollback();
    }
}