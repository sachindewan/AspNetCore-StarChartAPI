using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            this._context = context;
        }
        [HttpGet("{id:int}")]
        [ActionName("GetById")]
        public IActionResult GetById(int id)
        {
            var data = _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();
            if (data != null)
            {
                data.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();
                return Ok(data);
            }
            return NotFound();
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var data = _context.CelestialObjects.Where(c=>c.Name==name).ToList();
            if (!data.Any())
                return NotFound();
            foreach(var d in data)
            {
                d.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == d.Id).ToList();
            }
            return Ok(data);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _context.CelestialObjects.ToList();
            foreach (var d in data)
            {
                d.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == d.Id).ToList();
            }
            return Ok(data);
        }
    }
}
