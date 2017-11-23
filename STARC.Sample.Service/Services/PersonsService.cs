using System.Collections.Generic;
using System.Threading.Tasks;
using STARC.Sample.Models;
using STARC.Sample.Service.DataServices;
using STARC.Sample.Service.Repositories;

namespace STARC.Sample.Service
{
    public class PersonsService : IPersonsService
    {
        private readonly IPersonsRepository _personsRepository;

        public PersonsService(IPersonsRepository repository)
        {
            _personsRepository = repository;

        }

        public Task<List<Person>> GetAllPersons()
        {
            return _personsRepository.GetAllPersons();
        }

        public Task<Person> GetPersonById(int personId)
        {
            return _personsRepository.GetPersonById(personId);
        }

        public Person AddPerson(EntityModels.Person newPerson)
        {
            return _personsRepository.AddPerson(newPerson);
        }

    }
}
