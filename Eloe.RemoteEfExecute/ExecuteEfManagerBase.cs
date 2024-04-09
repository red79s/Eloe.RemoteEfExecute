using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Eloe.RemoteEfExecute
{
    public class ExecuteEfManagerBase
    {
        private List<IExecuteDbSet> _dbSetExecuters = new List<IExecuteDbSet>();
        public ExecuteEfManagerBase()
        {
        }

        public void AddDbSetExecuter(IExecuteDbSet executer)
        {
            _dbSetExecuters.Add(executer);
        }

        public void Add<T>(DbSet<T> dbSet, [CallerArgumentExpression("dbSet")] string dbSetName = "") where T : class
        {
            var index = dbSetName.IndexOf('.');
            var name = index >= 0 ? dbSetName.Substring(index + 1) : dbSetName;
            var ex = new EfCoreExecuteDbSet<T>(name, dbSet);
            AddDbSetExecuter(ex);
        }

        public void AddAllDbSets(DbContext context)
        {
            var properties = context.GetType().GetProperties();

            foreach (var property in properties)
            {
                var setType = property.PropertyType;

                var isDbSet = setType.IsGenericType && (typeof(DbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition()));

                if (isDbSet)
                {
                    var ga = property.PropertyType.GetGenericArguments();
                    if (ga.Length != 1)
                        continue;

                    var dbSetName = property.Name;

                    var inst = property.GetValue(context, null);

                    var t = typeof(EfCoreExecuteDbSet<>);
                    Type[] typeArgs = { ga[0] };
                    var genericType = t.MakeGenericType(typeArgs);
                    object executer = Activator.CreateInstance(genericType, new object[] { dbSetName, inst });
                    AddDbSetExecuter((IExecuteDbSet)executer);
                }
            }
        }

        public List<IExecuteDbSet> GetDbSetExecuters()
        { 
            return _dbSetExecuters; 
        }

        public string? FirstOrDefault(string dbSet, List<string> includes, List<string> expressions)
        {
            var executer = _dbSetExecuters.FirstOrDefault(x => x.DbSetName == dbSet);
            if (executer == null)
            {
                throw new ArgumentOutOfRangeException(nameof(dbSet));
            }

            return executer.FirstOrDefault(includes, expressions);
        }

        public string? ToList(string dbSet, List<string> includes, List<string> expressions)
        {
            var executer = _dbSetExecuters.FirstOrDefault(x => x.DbSetName == dbSet);
            if (executer == null)
            {
                throw new ArgumentOutOfRangeException(nameof(dbSet));
            }

            return executer.ToList(includes, expressions);
        }

        public int SaveChanges(RemoteDbSetSaveChangesParams saveChangesParams)
        {
            foreach (var entity in saveChangesParams.AddEntities)
            {
                var executer = _dbSetExecuters.FirstOrDefault(x => x.DbSetName == entity.DbSetName);
                if (executer == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(entity));
                }

                executer.Add(entity.SerializedValue);
            }

            foreach (var entity in saveChangesParams.UpdatedEntities)
            {
                var executer = _dbSetExecuters.FirstOrDefault(x => x.DbSetName == entity.DbSetName);
                if (executer == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(entity));
                }

                executer.Update(entity.SerializedValue);
            }

            foreach (var entity in saveChangesParams.DeletedEntities)
            {
                var executer = _dbSetExecuters.FirstOrDefault(x => x.DbSetName == entity.DbSetName);
                if (executer == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(entity));
                }

                executer.Delete(entity.SerializedValue);
            }

            return SaveChanges();
        }

        protected virtual int SaveChanges()
        {
            throw new NotImplementedException();
        }
    }

}
