namespace Eloe.RemoteEfExecute
{
    public class ExecuteEfManagerBase
    {
        private List<IExecuteDbSet> _dbSetExecuters = new List<IExecuteDbSet>();
        public ExecuteEfManagerBase()
        {
        }

        protected void AddDbSetExecuter(IExecuteDbSet executer)
        {
            _dbSetExecuters.Add(executer);
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
