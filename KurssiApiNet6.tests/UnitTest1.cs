
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
using Newtonsoft.Json;
using KurssiApiNet6.Models;

namespace KurssiApiNet6.tests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public async Task TestReturnHello()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var client = webAppFactory.CreateDefaultClient();

            var response = await client.GetAsync("api/kurssit/hello");
            var stringResult = await response.Content.ReadAsStringAsync();

            Assert.AreEqual("Hello World!", stringResult);
        }


        [TestMethod]
        public async Task TestGetKurssit()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await httpClient.GetAsync("api/kurssit");
            var json = await response.Content.ReadAsStringAsync();

            IEnumerable<Kurssit> kurssit = JsonConvert.DeserializeObject<Kurssit[]>(json);

            Assert.AreEqual(8, kurssit.Count() );

        }

        [TestMethod]
        public async Task TestGetKurssitStatusCode()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await httpClient.GetAsync("api/kurssit");
 
            Assert.AreEqual(response.StatusCode.ToString().ToLower(), "ok");

        }


    }
}