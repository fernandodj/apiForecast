using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ApiRest.Models;

namespace ApiRest.Controllers
{
    public class ClimatesController : ApiController
    {
        private ForecastEntities3 db = new ForecastEntities3();

        // GET: api/Climates
        public IQueryable<Climates> GetClimates()
        {
            return db.Climates;
        }

        // GET: api/Climates/5
        [ResponseType(typeof(Climates))]
        public IHttpActionResult GetClimates(int id)
        {
            Climates climates = db.Climates.Find(id);
            if (climates == null)
            {
                return NotFound();
            }

            return Ok(climates);
        }

        // GET: api/Climates?day={day}
        [ResponseType(typeof(Climates))]
        public IHttpActionResult GetClimatesByDay([FromUri]int day)
        {
            Climates climates = db.Climates.Where(x => x.day == day).First();
            if (climates == null)
            {
                return NotFound();
            }

            return Ok(climates);
        }

        // PUT: api/Climates/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClimates(int id, Climates climates)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != climates.id)
            {
                return BadRequest();
            }

            db.Entry(climates).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClimatesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Climates
        [ResponseType(typeof(Climates))]
        public IHttpActionResult PostClimates(Climates climates)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Climates.Add(climates);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = climates.id }, climates);
        }

        // DELETE: api/Climates/5
        [ResponseType(typeof(Climates))]
        public IHttpActionResult DeleteClimates(int id)
        {
            Climates climates = db.Climates.Find(id);
            if (climates == null)
            {
                return NotFound();
            }

            db.Climates.Remove(climates);
            db.SaveChanges();

            return Ok(climates);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClimatesExists(int id)
        {
            return db.Climates.Count(e => e.id == id) > 0;
        }
    }
}