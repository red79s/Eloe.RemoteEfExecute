namespace Eloe.RemoteEfExecute
{
    public interface IRemoteDbSetExecuter
    {
        Task<string?> FirstOrDefault(RemoteDbSetExecuteParam execParams);
        Task<string?> ToList(RemoteDbSetExecuteParam execParams);
        Task<int> SaveChanges(RemoteDbSetSaveChangesParams saveChangesParams);
    }
}
