using Serialize.Linq.Serializers;
using System.Linq.Expressions;

namespace Eloe.RemoteEfExecute
{
    public class RemoteDbSet<T> : IRemoteDbSet<T>
    {
        private List<string> _navigationPropertyPaths = new List<string>();
        private List<Expression<Func<T, bool>>> _whereExpressions = new List<Expression<Func<T, bool>>>();
        private readonly string _dbSetName;
        private readonly IRemoteDbSetExecuter _remoteDbSetExecuter;
        private List<T> _addedEntities = new List<T>();
        private List<T> _updatedEntities = new List<T>();
        private List<T> _deletedEntities = new List<T>();

        public RemoteDbSet(string dbSetName, IRemoteDbSetExecuter remoteDbSetExecuter)
        {
            _dbSetName = dbSetName;
            _remoteDbSetExecuter = remoteDbSetExecuter;
        }

        public RemoteDbSet(string dbSetName, IRemoteDbSetExecuter remoteDbSetExecuter, List<string> navigationPropertyPaths, List<Expression<Func<T, bool>>> whereExpressions)
            : this (dbSetName, remoteDbSetExecuter)
        {
            _navigationPropertyPaths.AddRange(navigationPropertyPaths);
            _whereExpressions.AddRange(whereExpressions);
        }


        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            // Create a serializer
            var serializer = new ExpressionSerializer(new JsonSerializer());

            var serializedExpressions = new List<string> { serializer.SerializeText(expression) };
            foreach (var exp in _whereExpressions)
            {
                string serializedExpression = serializer.SerializeText(exp);
                serializedExpressions.Add(serializedExpression);
            }

            var param = new RemoteDbSetExecuteParam { DbSetName = _dbSetName, Includes = _navigationPropertyPaths, Expressions = serializedExpressions };
            var ret = await _remoteDbSetExecuter.FirstOrDefault(param);
            if (ret == null)
                return default(T);

            return System.Text.Json.JsonSerializer.Deserialize<T>(ret);
        }

        public async Task<T?> FirstOrDefaultAsync()
        {
            // Create a serializer
            var serializer = new ExpressionSerializer(new JsonSerializer());

            var serializedExpressions = new List<string>();
            foreach (var exp in _whereExpressions)
            {
                string serializedExpression = serializer.SerializeText(exp);
                serializedExpressions.Add(serializedExpression);
            }

            var param = new RemoteDbSetExecuteParam { DbSetName = _dbSetName, Includes = _navigationPropertyPaths, Expressions = serializedExpressions };
            var ret = await _remoteDbSetExecuter.FirstOrDefault(param);
            if (ret == null)
                return default(T);

            return System.Text.Json.JsonSerializer.Deserialize<T>(ret);
        }

        public async Task<IList<T>> ToListAsync()
        {
            var serializer = new ExpressionSerializer(new JsonSerializer());
            var serializedExpressions = new List<string>();
            foreach (var exp in _whereExpressions)
            {
                string serializedExpression = serializer.SerializeText(exp);
                serializedExpressions.Add(serializedExpression);
            }


            var param = new RemoteDbSetExecuteParam { DbSetName = _dbSetName, Includes = _navigationPropertyPaths, Expressions = serializedExpressions };
            var ret = await _remoteDbSetExecuter.ToList(param);
            if (ret == null)
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<IList<T>>(ret);
        }

        public IRemoteDbSet<T> Where(Expression<Func<T, bool>> expression)
        {
            var we = _whereExpressions.ToList();
            we.Add(expression);
            return new RemoteDbSet<T>(_dbSetName, _remoteDbSetExecuter, _navigationPropertyPaths, we);
        }

        public IRemoteDbSet<T> Include(string navigationPropertyPath)
        {
            var npps = _navigationPropertyPaths.ToList();
            npps.Add(navigationPropertyPath);
            return new RemoteDbSet<T>(_dbSetName, _remoteDbSetExecuter, npps, _whereExpressions);
        }

        public List<SerializedEntity> GetAndRemoveAddedEntities()
        {
            var typeName = typeof(T).FullName;
            if (typeName == null)
                throw new ArgumentNullException(nameof(typeName));

            var list = new List<SerializedEntity>();
            foreach (var entity in _addedEntities)
            {
                var seralizedValue = System.Text.Json.JsonSerializer.Serialize(entity);
                list.Add(new SerializedEntity { DbSetName = _dbSetName, TypeName = typeName, SerializedValue = seralizedValue });
            }

            _addedEntities.Clear();

            return list;
        }

        public List<SerializedEntity> GetAndRemoveUpdatedEntities()
        {
            var typeName = typeof(T).FullName;
            if (typeName == null)
                throw new ArgumentNullException(nameof(typeName));

            var list = new List<SerializedEntity>();
            foreach (var entity in _updatedEntities)
            {
                var seralizedValue = System.Text.Json.JsonSerializer.Serialize(entity);
                list.Add(new SerializedEntity { DbSetName = _dbSetName, TypeName = typeName, SerializedValue = seralizedValue });
            }

            _updatedEntities.Clear();

            return list;
        }

        public List<SerializedEntity> GetAndRemoveDeletedEntities()
        {
            var typeName = typeof(T).FullName;
            if (typeName == null)
                throw new ArgumentNullException(nameof(typeName));

            var list = new List<SerializedEntity>();
            foreach (var entity in _deletedEntities)
            {
                var seralizedValue = System.Text.Json.JsonSerializer.Serialize(entity);
                list.Add(new SerializedEntity { DbSetName = _dbSetName, TypeName = typeName, SerializedValue = seralizedValue });
            }

            _deletedEntities.Clear();

            return list;
        }

        public void Add(T item)
        {
            _addedEntities.Add(item);
        }

        public void Update(T item)
        {
            _updatedEntities.Add(item);
        }

        public void Delete(T item)
        {
            _deletedEntities.Add(item);
        }

        public void Clear()
        {
            _addedEntities.Clear();
            _updatedEntities.Clear();
            _deletedEntities.Clear();
            _navigationPropertyPaths.Clear();
            _whereExpressions.Clear();
        }
    }
}
