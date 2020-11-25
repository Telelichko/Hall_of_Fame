using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HallOfFame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet("api/v1/persons")]
        public List<Person> Get()
        {
            try
            {
                var persons = _ctx.Persons.Include(x => x.Skills).ToList();

                return persons;
            }
            catch (Exception e)
            {
                throw new HttpListenerException(500, Constants.Response.Error500);
            }
        }

        // GET api/person/id
        [HttpGet("api/v1/person/{id}")]
        public ActionResult Get(long id)
        {
            try
            {
                var person = _ctx.Persons.Include(x => x.Skills).FirstOrDefault(x => x.Id == id);

                if (person == null)
                {
                    throw new HttpListenerException(404, Constants.Response.Error404);
                }

                return Ok(person);
            }
            catch (Exception e)
            {
                throw new HttpListenerException(500, Constants.Response.Error500);
            }
        }

        // POST api/person
        [HttpPost("api/v1/person")]
        public void Post([FromBody] Person person)
        {
            try
            {
                _ctx.Persons.Add(person);

                _ctx.SaveChanges();
            }
            catch (Exception e)
            {
                throw new HttpListenerException(500, Constants.Response.Error500);
            }
        }
        
        // PUT api/person/id
        [HttpPut("api/v1/person/{id}")]
        public void Put(long id, [FromBody] Person person)
        {
            try
            {
                var personExist = _ctx.Persons.FirstOrDefault(x => x.Id == id);

                if (personExist == null)
                {
                    throw new HttpListenerException(404, Constants.Response.Error404);
                }

                if (personExist != null)
                {
                    personExist.Name = person.Name;
                    personExist.DisplayName = person.DisplayName;

                    var skillsExist = _ctx.Skills.Where(x => x.PersonId == id).ToList();
                    var skillsNew = person.Skills.ToList();

                    var iMax = Math.Max(skillsExist.Count(), skillsNew.Count());
                    for (int i = 0; i < iMax; i++)
                    {
                        if (skillsNew.Count() - 1 < i)
                        {
                            _ctx.Skills.Remove(skillsExist[i]);
                            continue;
                        }
                        if (skillsExist.Count() - 1 < i)
                        {
                            skillsNew[i].PersonId = id;
                            _ctx.Skills.Add(skillsNew[i]);
                            continue;
                        }

                        skillsExist[i].Name = skillsNew[i].Name;
                        skillsExist[i].Level = skillsNew[i].Level;
                    }
                }

                _ctx.SaveChanges();
            }
            catch (Exception e)
            {
                throw new HttpListenerException(500, Constants.Response.Error500);
            }            
        }

        // DELETE api/person/id
        [HttpDelete("api/v1/person/{id}")]
        public void Delete(long id)
        {
            try
            {
                var person = _ctx.Persons.FirstOrDefault(x => x.Id == id);

                if (person == null)
                {
                    throw new HttpListenerException(404, Constants.Response.Error404);
                }

                var skills = _ctx.Skills.Where(x => x.PersonId == id).ToList();

                foreach (Skill skill in skills)
                {
                    _ctx.Skills.Remove(skill);
                }

                _ctx.Persons.Remove(person);
                _ctx.SaveChanges();
            }
            catch (Exception e)
            {
                throw new HttpListenerException(500, Constants.Response.Error500);
            }
        }
    }
}
