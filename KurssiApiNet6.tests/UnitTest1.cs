
// Huom. Asennettu t�m� nugetpaketti (versio joka alkaa 6 numerolla)
using Microsoft.AspNetCore.Mvc.Testing;

// Lis�� my�s testattavan projektin projektitiedostoon merkint� vastaavaksi kuin alla,
// jotta internal muuttujat n�kyy testiprojektille:
//       <ItemGroup>
//          < InternalsVisibleTo Include = "$(AssemblyName).Tests" />
//       </ ItemGroup >

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using KurssiApiNet6;
using Newtonsoft.Json;
using KurssiApiNet6.Models;
using System.Text;

namespace KurssiApiNet6.tests
{
    [TestClass]
    public class UnitTest1
    {

        // Muistipaikka testien aikana luotavan kurssin KurssiId:lle poistoa varten
        public int id;

        // Hello Worldin testi
        [TestMethod]
        public async Task TestReturnHello()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var client = webAppFactory.CreateDefaultClient();

            var response = await client.GetAsync("api/kurssit/hello");
            var stringResult = await response.Content.ReadAsStringAsync();

            Assert.AreEqual("Hello World!", stringResult);
        }

        //*** Testeiss� oletetaan ett� testitietokannassa ei ole aluksi kursseja ****//
    
        // Luodaan uusi kurssi ja testataan ett� vastauksen statuskoodi on "ok" (200)
        [TestMethod]
        public async Task TestPostKurssi()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            Kurssit uusiKurssi = new Kurssit();
            uusiKurssi.Nimi = "Testikurssi";
            uusiKurssi.Laajuus = 1;

            // Muutetaan edell� luotu objekti Jsoniksi
            string input = JsonConvert.SerializeObject(uusiKurssi);
            StringContent content = new StringContent(input, Encoding.UTF8, "application/json");

            // L�hetet��n muodostettu data testattavalle api:lle post pyynt�n�
            var response = await httpClient.PostAsync("api/kurssit", content);

            Assert.AreEqual(response.StatusCode.ToString().ToLower(), "ok");
        }


        // Testataan ett� m��r� on kasvanut yhdell�
        [TestMethod]
        public async Task TestGetKurssit2()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await httpClient.GetAsync("api/kurssit");
            var json = await response.Content.ReadAsStringAsync();

            List<Kurssit> kurssit = JsonConvert.DeserializeObject<Kurssit[]>(json).ToList();

            id = kurssit[0].KurssiId; // Asetetaan testiss� luodun kurssin id muistiin
            Assert.AreEqual(1, kurssit.Count());
        }

        // Testataan viel� ett� statuskoodi on "ok" (200) get pyynn�lle
        [TestMethod]
        public async Task TestGetKurssitStatusCode()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await httpClient.GetAsync("api/kurssit");

            Assert.AreEqual(response.StatusCode.ToString().ToLower(), "ok");
        }

        // Poistetaan luotu kurssi ja katsotaan ett� statuskoodi on "ok" (200)
        [TestMethod]
        public async Task TestDeleteKurssitStatusCode()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await httpClient.DeleteAsync("api/kurssit" + "/" + id);

            Assert.AreEqual(response.StatusCode.ToString().ToLower(), "not found");
        }

        [TestMethod]
        public async Task TestGetKurssit3()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await httpClient.GetAsync("api/kurssit");
            var json = await response.Content.ReadAsStringAsync();

            List<Kurssit> kurssit = JsonConvert.DeserializeObject<Kurssit[]>(json).ToList();

            Assert.AreEqual(0, kurssit.Count());
        }

    }
}