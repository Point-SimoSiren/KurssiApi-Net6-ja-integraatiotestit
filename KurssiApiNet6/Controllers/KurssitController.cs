using KurssiApiNet6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KurssiApiNet6.Models;

namespace KurssiApiNet6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KurssitController : ControllerBase
    {
        private KurssiDBContext db = new KurssiDBContext();

        // Hae kaikki kurssit
        [HttpGet]
        [Route("")]
        public List<Kurssit> HaeKurssit()
        {
            try
            {
                List<Kurssit> kurssit = db.Kurssit.ToList();
                return kurssit;
            }
            finally
            {
                db.Dispose();
            }
        }


        // Haku id:llä
        [HttpGet]
        [Route("{id}")]
        public Kurssit GetOneById(int id)
        {
            try
            {
                Kurssit kurssi = db.Kurssit.Find(id);
                return kurssi;
            }
            finally
            {
                db.Dispose();
            }
        }


        // Uuden luonti
        [HttpPost]
        [Route("")]
        public ActionResult LuoKurssi([FromBody] Kurssit kurssi)
        {
            try
            {
                db.Kurssit.Add(kurssi);
                db.SaveChanges();
                return Ok(kurssi.KurssiId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                db.Dispose();
            }
        }


        //Poisto Id:llä
        [HttpDelete]
        [Route("{id}")]
        public ActionResult PoistaKurssi(int id)
        {
            try
            {
                Kurssit kurssi = db.Kurssit.Find(id);
                if (kurssi != null)
                {
                    db.Kurssit.Remove(kurssi);
                    db.SaveChanges();
                    return Ok("Kurssi id:llä " + id + " poistettiin");
                }
                else
                {
                    return NotFound("Kurssia id:llä" + id + " ei löydy");
                }
            }
            catch
            {
                return BadRequest();
            }
            finally
            {
                db.Dispose();
            }
        }

    }
}
