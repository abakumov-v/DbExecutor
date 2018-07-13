using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

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
        Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql);

        IEnumerable<TResult> Query<TResult>(string sql, object param);
        Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object param);

        void Execute(string sql, object param = null, CommandType? commandType = default(CommandType?),
            int? commandTimeout = default(int?));

        Task<int> ExecuteAsync(string sql, object param = null, CommandType? commandType = default(CommandType?),
            int? commandTimeout = default(int?));

        void Commit();
        void Rollback();
    }
}