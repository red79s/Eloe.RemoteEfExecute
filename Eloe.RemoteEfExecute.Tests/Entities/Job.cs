namespace Eloe.RemoteEfExecute.Tests.Entities
{
    internal class Job
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set;}
    }
}
