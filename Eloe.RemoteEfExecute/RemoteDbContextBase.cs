using System.Runtime.CompilerServices;

namespace Eloe.RemoteEfExecute
{
    public class RemoteDbContextBase
    {
        private readonly IRemoteDbSetExecuter _executer;
        private List<IRemoteDbSetBase> _remoteDbSets = new List<IRemoteDbSetBase>();

        public RemoteDbContextBase(IRemoteDbSetExecuter executer)
        {
            _executer = executer;
        }

        protected IRemoteDbSetBase AddRemoteDbSet(IRemoteDbSetBase remoteDbSet)
        {
            _remoteDbSets.Add(remoteDbSet);
            return remoteDbSet;
        }

        protected IRemoteDbSet<T> InitializeRemoteDbSet<T>(IRemoteDbSet<T> dbSet, [CallerArgumentExpression("dbSet")] string dbSetName = "") where T : class
        {
            dbSet = new RemoteDbSet<T>(dbSetName, _executer);
            return dbSet;
        }

        public async Task<int> SaveChangesAsync()
        {
            var saveChangesParams = new RemoteDbSetSaveChangesParams();
            foreach (var dbSet in _remoteDbSets)
            {
                saveChangesParams.AddEntities.AddRange(dbSet.GetAndRemoveAddedEntities());
                saveChangesParams.UpdatedEntities.AddRange(dbSet.GetAndRemoveUpdatedEntities());
                saveChangesParams.DeletedEntities.AddRange(dbSet.GetAndRemoveDeletedEntities());
            }

            if (saveChangesParams.HaveItems)
            {
                return await _executer.SaveChanges(saveChangesParams);
            }
            else
            {
                return 0;
            }
        }
    }
}
