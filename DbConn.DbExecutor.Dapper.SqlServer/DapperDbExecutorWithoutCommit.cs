using System.Data;

namespace DbConn.DbExecutor.Dapper.SqlServer
{
    /// <summary>
    /// Dapper DB-executor without real commit transaction
    /// </summary>
    public class DapperDbExecutorWithoutCommit : DapperDbExecutor
    {
        public DapperDbExecutorWithoutCommit(string connectionString,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, bool isNeedTransactional = true) : base(
            connectionString, isolationLevel, isNeedTransactional)
        {
        }

        /// <summary>
        /// WARNING: This method will not make a real commit. 
        /// Instead, it will always roll back. This can be useful in some scenarios, such as 
        /// integration tests, because we will not commit changes made by integration tests.
        /// </summary>
        public override void Commit()
        {
            // ignore commit and always rollback transaction
            Rollback();
        }
    }
}