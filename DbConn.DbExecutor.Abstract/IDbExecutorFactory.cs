using System.Data;

namespace DbConn.DbExecutor.Abstract
{
    public interface IDbExecutorFactory
    {
        IDbExecutor Create(string connectionString);
        IDbExecutor CreateTransactional(string connectionString, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}