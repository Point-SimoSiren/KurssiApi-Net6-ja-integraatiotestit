
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

        //*** Testeissä oletetaan että testitietokannassa ei ole aluksi kursseja ****//
    
        // Luodaan uusi kurssi ja testataan että vastauksen statuskoodi on "ok" (200)
        [TestMethod]
        public async Task TestPostKurssi()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            Kurssit uusiKurssi = new Kurssit();
            uusiKurssi.Nimi = "Testikurssi";
            uusiKurssi.Laajuus = 1;

            // Muutetaan edellä luotu objekti Jsoniksi
            string input = JsonConvert.SerializeObject(uusiKurssi);
            StringContent content = new StringContent(input, Encoding.UTF8, "application/json");

            // Lähetetään muodostettu data testattavalle api:lle post pyyntönä
            var response = await httpClient.PostAsync("api/kurssit", content);

            Assert.AreEqual(response.StatusCode.ToString().ToLower(), "ok");
        }


        // Testataan että määrä on kasvanut yhdellä
        [TestMethod]
        public async Task TestGetKurssit2()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await httpClient.GetAsync("api/kurssit");
            var json = await response.Content.ReadAsStringAsync();

            List<Kurssit> kurssit = JsonConvert.DeserializeObject<Kurssit[]>(json).ToList();

            id = kurssit[0].KurssiId; // Asetetaan testissä luodun kurssin id muistiin
            Assert.AreEqual(1, kurssit.Count());
        }

        // Testataan vielä että statuskoodi on "ok" (200) get pyynnölle
        [TestMethod]
        public async Task TestGetKurssitStatusCode()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await httpClient.GetAsync("api/kurssit");

            Assert.AreEqual(response.StatusCode.ToString().ToLower(), "ok");
        }

        // Poistetaan luotu kurssi ja katsotaan että statuskoodi on "ok" (200)
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