using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DbConn.DbExecutor.Abstract;

namespace DbConn.DbExecutor.Dapper.Fakes
{
    public class FakeDbExecutorFactory : IDbExecutorFactory
    {
        private readonly string _connectionString;

        public FakeDbExecutorFactory()
        {

        }
        public FakeDbExecutorFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            _connectionString = connectionString;
        }

        public IDbExecutor Create(string connectionString)
        {
            return string.IsNullOrWhiteSpace(_connectionString)
                ? new DapperDbExecutorWithoutCommit(connectionString)
                : new DapperDbExecutorWithoutCommit(_connectionString);
        }

        public IDbExecutor CreateTransactional(string connectionString, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return string.IsNullOrWhiteSpace(_connectionString)
                ? new DapperDbExecutorWithoutCommit(connectionString, isolationLevel)
                : new DapperDbExecutorWithoutCommit(_connectionString, isolationLevel);
        }
    }
}
