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

        protected void AddRemoteDbSet(IRemoteDbSetBase remoteDbSet)
        {
            _remoteDbSets.Add(remoteDbSet);
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
