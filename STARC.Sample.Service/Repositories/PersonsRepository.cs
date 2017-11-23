using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using STARC.Sample.Models;
using STARC.Sample.Service.DBContexts;

namespace STARC.Sample.Service.Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly PersonsContext _context;

        public PersonsRepository(PersonsContext personCtx)
        {
            _context = personCtx;
        }

        public Task<List<Person>> GetAllPersons()
        {
            return _context.Persons.Select(person => new Person
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                PersonId = person.PersonId
            }).ToListAsync();
        }

        public Task<Person> GetPersonById(int personId)
        {
            var person = _context.Persons.FirstOrDefault(p => p.PersonId == personId);

            if (person != null)
            {
                return Task.FromResult<Person>(new Person { FirstName = person.FirstName, LastName = person.LastName, PersonId = person.PersonId });
            }
            else
            {
                return null;
            }
        }

        public Person AddPerson(EntityModels.Person newPerson)
        {
            if (newPerson == null)
            {
                throw new ArgumentNullException("newPerson");
            }

            _context.Persons.Add(newPerson);

            _context.SaveChanges();

            return new Person { FirstName = newPerson.FirstName, LastName = newPerson.LastName, PersonId = newPerson.PersonId };
        }
    }
}

