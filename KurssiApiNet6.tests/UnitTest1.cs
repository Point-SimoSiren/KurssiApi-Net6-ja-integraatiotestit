
// T�m� testiprojekti on MsTest tyyppinen, ei siis esim xUnit.
// Lis�tty project reference joka viittaa testattavaan projektiiin.

// Lis�� my�s testattavan projektin xml projektitiedostoon merkint� vastaavaksi kuin alla,
// jotta internal muuttujat n�kyy testiprojektille:
//       <ItemGroup>
//          < InternalsVisibleTo Include = "$(AssemblyName).Tests" />
//       </ ItemGroup >

// Huom. Asennettu t�m� nugetpaketti (versio joka alkaa 6 numerolla koska testattava
// projekti on .NET6 tyyppinen)
using Microsoft.AspNetCore.Mvc.Testing;

using Newtonsoft.Json;
using KurssiApiNet6.Models;
using System.Text;
using KurssiApiNet6.Tests;

// Huom. MsTesting library ajaa testit aakkosj�rjestyksess�, siksi nimet Test1, Test2, jne.
//*** Testeiss� oletetaan ett� testitietokannassa ei ole aluksi kursseja ****//
namespace KurssiApiNet6.tests
{

    [TestClass]
    public class UnitTest1
    {

        // Luodaan uusi kurssi ja testataan ett� vastauksen statuskoodi on "ok" (200)
        [TestMethod]
        public async Task Test1()
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

            // Tarkistetaan onko vastauksen statuskoodi ok
            Assert.AreEqual(response.StatusCode.ToString().ToLower(), "ok");
        }


        // Testataan ett� lis�tty kurssi l�ytyy oikean nimisen�
        [TestMethod]
        public async Task Test2()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await httpClient.GetAsync("api/kurssit");
            var json = await response.Content.ReadAsStringAsync();

            var kurssi = JsonConvert.DeserializeObject<Kurssit[]>(json).FirstOrDefault();

            // Jouduin luomaan staattisen luokan jonne voi globaalisti tallettaa id:n kun edellisen testin
            // aikana p��tasolle talletettu int muuttuja ei n�y en�� seuraavalle testille.
            Muistipaikka.id = kurssi.KurssiId; // Asetetaan testiss� luodun kurssin id muistiin poistoa varten
            Assert.AreEqual("Testikurssi", kurssi.Nimi);
        }

        
        // Poistetaan luotu kurssi ja katsotaan ett� statuskoodi on "ok" (200)
        [TestMethod]
        public async Task Test3()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();
            
            // Tehd��n delete pyynt�, URL parametri KurssiId otetaan staattisen luokan "muistipaikasta".
            var response = await httpClient.DeleteAsync("api/kurssit/" + Muistipaikka.id);

            Assert.AreEqual("ok", response.StatusCode.ToString().ToLower());
        }

        // Testataan ett� poistettua kurssia ei en�� l�ydy
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