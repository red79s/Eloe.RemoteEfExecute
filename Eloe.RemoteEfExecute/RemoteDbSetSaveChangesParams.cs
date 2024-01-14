namespace Eloe.RemoteEfExecute
{
    public class RemoteDbSetSaveChangesParams
    {
        public List<SerializedEntity> AddEntities { get; set; } = new List<SerializedEntity>();
        public List<SerializedEntity> UpdatedEntities { get; set; } = new List<SerializedEntity>();
        public List<SerializedEntity> DeletedEntities { get; set; } = new List<SerializedEntity>();
        public bool HaveItems => AddEntities.Count > 0 || UpdatedEntities.Count > 0 || DeletedEntities.Count > 0;
    }
}
