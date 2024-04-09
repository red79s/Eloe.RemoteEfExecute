using Eloe.RemoteEfExecute.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eloe.RemoteEfExecute.Tests
{
    internal class TestDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
    }
}
