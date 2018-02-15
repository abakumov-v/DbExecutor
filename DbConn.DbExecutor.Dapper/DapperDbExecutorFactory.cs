using System.Data;
using DbConn.DbExecutor.Abstract;

namespace DbConn.DbExecutor.Dapper
{
    public class DapperDbExecutorFactory : IDbExecutorFactory
    {
        /// <summary>
        /// Create DbExecutor instance without starting the transaction
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        /// <returns></returns>
        public IDbExecutor Create(string connectionString)
        {
            return new DapperDbExecutor(connectionString, isNeedTransactional: false);
        }
        /// <summary>
        /// Create DbExecutor instance with the start of transaction
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        public IDbExecutor CreateTransactional(string connectionString,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return new DapperDbExecutor(connectionString, isolationLevel);
        }
    }
}