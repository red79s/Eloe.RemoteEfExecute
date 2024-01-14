namespace Eloe.RemoteEfExecute
{
    public interface IExecuteDbSet
    {
        public string DbSetName { get; }
        public string? FirstOrDefault(List<string> includes, List<string> expressions);
        public string? ToList(List<string> includes, List<string> expressions);
        public void Add(string serializedValue);
        public void Update(string serializedValue);
        public void Delete(string serializedValue);
    }
}
