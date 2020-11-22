using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HallOfFame.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HallOfFame.Controllers
{
    [ApiController]
    public class PersonsController : Controller
    {
        private AppDatabaseContext _ctx;

        public PersonsController(AppDatabaseContext ctx)
        {
            this._ctx = ctx;
        }

        // GET api/persons
        [HttpGet("api/persons")]
        public ActionResult<Array> Get()
        {
            var persons = _ctx.Persons.Select(x => x.Name).ToArray();

            return persons;
        }

        // GET api/person/id
        [HttpGet("api/person/{id}")]
        public Person Get(long id)
        {
            var person = _ctx.Persons.FirstOrDefault(x => x.Id == id);            

            return person;
        }

        // POST api/person
        [HttpPost("api/person")]
        public void Post([FromBody] Person person)
        {
            _ctx.Persons.Add(person);
            _ctx.SaveChanges();
        }
        
        // PUT api/person/id
        [HttpPut("api/person/{id}")]
        public void Put(long id, [FromBody] Person person)
        {
            var personExist = _ctx.Persons.FirstOrDefault(x => x.Id == id);

            if (personExist != null)
            {
                personExist.Name = person.Name;
                personExist.DisplayName = person.DisplayName;
            }
            
            _ctx.SaveChanges();
        }

        // DELETE api/person/id
        [HttpDelete("api/person/{id}")]
        public void Delete(long id)
        {
            var person = _ctx.Persons.FirstOrDefault(x => x.Id == id);

            var skills = _ctx.Skills.Where(x => x.PersonId == id).ToList();

            foreach (Skill skill in skills)
            {
                _ctx.Skills.Remove(skill);
            }

            _ctx.Persons.Remove(person);
            _ctx.SaveChanges();
        }
    }
}
