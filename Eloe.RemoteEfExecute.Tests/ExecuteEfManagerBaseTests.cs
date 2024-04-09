using Eloe.RemoteEfExecute.Tests.Entities;

namespace Eloe.RemoteEfExecute.Tests
{
    [TestClass]
    public class ExecuteEfManagerBaseTests
    {
        [TestMethod]
        public void TestAddDbSetExecuter()
        {
            var tdc = new TestDbContext();
            var em = new ExecuteEfMangerTestClass();
            em.AddDbSetExecuter(new EfCoreExecuteDbSet<User>("Users", tdc.Users));

            var executers = em.GetDbSetExecuters();
            Assert.IsNotNull(executers);
            Assert.AreEqual(1, executers.Count);
            Assert.AreEqual("Users", executers[0].DbSetName);
        }

        [TestMethod]
        public void TestAdd()
        {
            var tdc = new TestDbContext();
            var em = new ExecuteEfMangerTestClass();
            em.Add(tdc.Users);

            var executers = em.GetDbSetExecuters();
            Assert.IsNotNull(executers);
            Assert.AreEqual(1, executers.Count);
            Assert.AreEqual("Users", executers[0].DbSetName);
        }

        [TestMethod]
        public void TestAddAllDbSets()
        {
            var tdc = new TestDbContext();
            var em = new ExecuteEfMangerTestClass();
            em.AddAllDbSets(tdc);

            var executers = em.GetDbSetExecuters();
            Assert.IsNotNull(executers);
            Assert.AreEqual(2, executers.Count);
            Assert.AreEqual("Users", executers[0].DbSetName);
            Assert.AreEqual("Jobs", executers[1].DbSetName);
        }
    }

    public class ExecuteEfMangerTestClass : ExecuteEfManagerBase
    {
        public ExecuteEfMangerTestClass()
        {

        }
    }
}