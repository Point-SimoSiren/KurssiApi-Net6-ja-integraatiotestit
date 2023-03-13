
// Huom. Asennettu tämä nugetpaketti (versio joka alkaa 6 numerolla)
using Microsoft.AspNetCore.Mvc.Testing;

// Lisää myös testattavan projektin projektitiedostoon merkintä vastaavaksi kuin alla,
// jotta internal muuttujat näkyy testiprojektille:
//       <ItemGroup>
//          < InternalsVisibleTo Include = "$(AssemblyName).Tests" />
//       </ ItemGroup >

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using KurssiApiNet6;

namespace KurssiApiNet6.tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task ReturnHello()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await httpClient.GetAsync("api/kurssit/hello");
            var stringResult = await response.Content.ReadAsStringAsync();

            Assert.AreEqual("Hello World!", stringResult);
        }
    }
}