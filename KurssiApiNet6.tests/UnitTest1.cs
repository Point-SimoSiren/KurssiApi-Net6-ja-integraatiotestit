
// Tämä testiprojekti on MsTest tyyppinen, ei siis esim xUnit.
// Lisätty project reference joka viittaa testattavaan projektiiin.

// Lisää myös testattavan projektin xml projektitiedostoon merkintä vastaavaksi kuin alla,
// jotta internal muuttujat näkyy testiprojektille:
//       <ItemGroup>
//          < InternalsVisibleTo Include = "$(AssemblyName).Tests" />
//       </ ItemGroup >

// Huom. Asennettu tämä nugetpaketti (versio joka alkaa 6 numerolla koska testattava
// projekti on .NET6 tyyppinen)
using Microsoft.AspNetCore.Mvc.Testing;

using Newtonsoft.Json;
using KurssiApiNet6.Models;
using System.Text;
using KurssiApiNet6.Tests;

// Huom. MsTesting library ajaa testit aakkosjärjestyksessä, siksi nimet Test1, Test2, jne.
//*** Testeissä oletetaan että testitietokannassa ei ole aluksi kursseja ****//
namespace KurssiApiNet6.tests
{

    [TestClass]
    public class UnitTest1
    {

        // Luodaan uusi kurssi ja testataan että vastauksen statuskoodi on "ok" (200)
        [TestMethod]
        public async Task Test1()
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

            // Tarkistetaan onko vastauksen statuskoodi ok
            Assert.AreEqual(response.StatusCode.ToString().ToLower(), "ok");
        }


        // Testataan että lisätty kurssi löytyy oikean nimisenä
        [TestMethod]
        public async Task Test2()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await httpClient.GetAsync("api/kurssit");
            var json = await response.Content.ReadAsStringAsync();

            var kurssi = JsonConvert.DeserializeObject<Kurssit[]>(json).FirstOrDefault();

            // Jouduin luomaan staattisen luokan jonne voi globaalisti tallettaa id:n kun edellisen testin
            // aikana päätasolle talletettu int muuttuja ei näy enää seuraavalle testille.
            Muistipaikka.id = kurssi.KurssiId; // Asetetaan testissä luodun kurssin id muistiin poistoa varten
            Assert.AreEqual("Testikurssi", kurssi.Nimi);
        }

        
        // Poistetaan luotu kurssi ja katsotaan että statuskoodi on "ok" (200)
        [TestMethod]
        public async Task Test3()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();
            
            // Tehdään delete pyyntö, URL parametri KurssiId otetaan staattisen luokan "muistipaikasta".
            var response = await httpClient.DeleteAsync("api/kurssit/" + Muistipaikka.id);

            Assert.AreEqual("ok", response.StatusCode.ToString().ToLower());
        }

        // Testataan että poistettua kurssia ei enää löydy
        [TestMethod]
        public async Task Test4()
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