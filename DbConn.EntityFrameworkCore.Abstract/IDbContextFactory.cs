using System;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace DbConn.EntityFrameworkCore.Abstract
{
    public interface IDbContextFactory<TContext> where TContext : DbContext
    {
        TContext Create(string connectionString);

        TContext CreateTransactional(string connectionString,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
