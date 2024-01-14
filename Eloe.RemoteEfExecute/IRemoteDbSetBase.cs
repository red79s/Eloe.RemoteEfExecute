namespace Eloe.RemoteEfExecute
{
    public interface IRemoteDbSetBase
    {
        void Clear();
        List<SerializedEntity> GetAddedEntities();
        List<SerializedEntity> GetUpdatedEntities();
        List<SerializedEntity> GetDeletedEntities();
    }
}
