using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpeechGenerator.Models.Tests
{
    [TestClass()]
    public class SpeechResourceTests
    {
        [TestMethod()]
        public void LoadSpeechResourcesTest()
        {
            var res = SpeechResource.LoadSpeechResources();
        }
    }
}