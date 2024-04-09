using Eloe.RemoteEfExecute.Tests.Entities;
using Moq;

namespace Eloe.RemoteEfExecute.Tests
{
    [TestClass]
    public class RemoteDbContextBaseTests
    {
        private Mock<IRemoteDbSetExecuter> _mockExecuter;
        private TestRemoteDbContext _context;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockExecuter = new Mock<IRemoteDbSetExecuter>();
            _context = new TestRemoteDbContext(_mockExecuter.Object);
        }

        [TestMethod]
        public void TestInitializeRemoteDbSet()
        {
            Assert.IsNotNull(_context.Users);
        }
    }

    internal class TestRemoteDbContext : RemoteDbContextBase
    {
        public IRemoteDbSet<User> Users { get; set; }
        public IRemoteDbSet<Job> Jobs { get; set; }
        public TestRemoteDbContext(IRemoteDbSetExecuter executer)
            : base(executer)
        {
            Users = InitializeRemoteDbSet(Users);
            Jobs = InitializeRemoteDbSet(Jobs);
        }

    }
}
