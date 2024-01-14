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
                saveChangesParams.AddEntities.AddRange(dbSet.GetAddedEntities());
                saveChangesParams.UpdatedEntities.AddRange(dbSet.GetUpdatedEntities());
                saveChangesParams.DeletedEntities.AddRange(dbSet.GetDeletedEntities());
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
