namespace Eloe.RemoteEfExecute
{
    public class RemoteDbSetExecuteParam
    {
        public string DbSetName { get; set; } = string.Empty;
        public List<string> Includes { get; set; } = new List<string>();
        public List<string> Expressions { get; set; } = new List<string>();
    }
}
