namespace Eloe.RemoteEfExecute
{
    public interface IRemoteDbSetBase
    {
        void Clear();
        List<SerializedEntity> GetAndRemoveAddedEntities();
        List<SerializedEntity> GetAndRemoveUpdatedEntities();
        List<SerializedEntity> GetAndRemoveDeletedEntities();
    }
}
