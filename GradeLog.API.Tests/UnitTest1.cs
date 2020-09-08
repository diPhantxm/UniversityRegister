using GradeLog.API.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace GradeLog.API.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var db = new GradeLogDbContext();
            var sc = new StudentsController(db);

            await sc.PutStudent(1, new Models.Student("Test", "test", "test"));
        }
    }
}
