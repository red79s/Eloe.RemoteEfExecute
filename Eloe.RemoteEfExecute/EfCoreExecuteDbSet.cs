using Microsoft.EntityFrameworkCore;

namespace Eloe.RemoteEfExecute
{
    public class EfCoreExecuteDbSet<T> : ExecuteDbSet<T> where T : class
    {
        private readonly DbSet<T> _dbSet;

        public EfCoreExecuteDbSet(string dbSetName, DbSet<T> dbSet)
            : base(dbSetName)
        {
            _dbSet = dbSet;
        }

        protected override IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public override void Add(string serializedEntity)
        {
            var entity = System.Text.Json.JsonSerializer.Deserialize<T>(serializedEntity);
            _dbSet.Add(entity);
        }

        public override void Update(string serializedEntity)
        {
            var entity = System.Text.Json.JsonSerializer.Deserialize<T>(serializedEntity);
            var trackedEntity = _dbSet.Attach(entity);
            trackedEntity.State = EntityState.Modified;
        }
        public override void Delete(string serializedEntity)
        {
            var entity = System.Text.Json.JsonSerializer.Deserialize<T>(serializedEntity);
            _dbSet.Remove(entity);
        }
    }
}
