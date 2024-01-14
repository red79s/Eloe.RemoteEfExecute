using System.Linq.Expressions;
using System.Text.Json.Serialization;
using System.Text.Json;
using Serialize.Linq.Serializers;
using Microsoft.EntityFrameworkCore;

namespace Eloe.RemoteEfExecute
{
    public class ExecuteDbSet<T> : IExecuteDbSet where T : class
    {
        public string DbSetName { get; private set; }

        public ExecuteDbSet(string dbSetName)
        {
            DbSetName = dbSetName;   
        }

        public string? FirstOrDefault(List<string> includes, List<string> expressions)
        {
            var query = SetupQuery(includes, expressions);

            var res = query.FirstOrDefault();
            if (res == null)
                return null;

            return System.Text.Json.JsonSerializer.Serialize(res, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles });
        }

        public string? ToList(List<string> includes, List<string> expressions)
        {
            var query = SetupQuery(includes, expressions);

            var list = query.ToList();

            if (list == null)
                return null;

            return System.Text.Json.JsonSerializer.Serialize(list, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles });
        }

        private IQueryable<T> SetupQuery(List<string> includes, List<string> expressions)
        {
            IQueryable<T> query = GetQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var serializer = new ExpressionSerializer(new Serialize.Linq.Serializers.JsonSerializer());
            foreach (var exp in expressions)
            {
                var deserializedExpression = serializer.DeserializeText(exp);
                query = query.Where((Expression<Func<T, bool>>)deserializedExpression);
            }

            return query;
        }

        protected virtual IQueryable<T> GetQueryable()
        {
            throw new NotImplementedException();
        }

        public virtual void Add(string serializedEntity)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(string serializedEntity) 
        { 
            throw new NotImplementedException();
        }

        public virtual void Delete(string serializedEntity) 
        {  
            throw new NotImplementedException(); 
        }
    }
}
