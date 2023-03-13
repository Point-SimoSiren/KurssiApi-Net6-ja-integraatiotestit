using KurssiApiNet6.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurssiApiNet6.Tests
{
    internal class GetProcedure
    {
        public IEnumerable<Kurssit> GetKurssit()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = httpClient.GetAsync("api/kurssit");
            var json = response.Content.ReadAsStringAsync();

            IEnumerable<Kurssit> kurssit = JsonConvert.DeserializeObject<Kurssit[]>(json);
            return kurssit;
        }
    }
}
