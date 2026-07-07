using Moq;
using Xunit;
using UniSys.Controllers; // Adjust to match your real controller namespace
using UniSys.Models;

namespace UniSys.UnitTests
{
    public class SubjectUnitTests
    {
        [Fact]
        public void Test_Your_Business_Logic_Here()
        {
            // ARRANGE: Setup local, fake data in memory (No database connections!)
            var fakeSubject = new Subject { Id = 1, Name = "Math 101" };
            
            // ACT: Execute the exact method logic you want to check
            var resultName = fakeSubject.Name;

            // ASSERT: Verify it works flawlessly
            Assert.Equal("Math 101", resultName);
        }
    }
}