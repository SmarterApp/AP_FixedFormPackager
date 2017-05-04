using FixedFormPackager.Test.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FixedFormPackager.Test.Integration
{
    [TestClass]
    public class ProgramTest
    {
        [TestMethod]
        public void TestForPresenceOfSevereErrors()
        {
            ProcessUtility.LaunchExecutable(
                "-i \"C:\\Projects\\SmarterBalanced\\Resources\\DELETE\\STS-Limited-Perf-ELA-3.xml\" -o \"FFP_Test\" -sv");
            var x = 0;
        }
    }
}