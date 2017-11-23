using System.Collections.Generic;
using System.Threading.Tasks;
using STARC.Sample.Models;

namespace STARC.Sample.Service.DataServices
{
    public interface IPersonsService
    {
        Task<List<Person>> GetAllPersons();

        Task<Person> GetPersonById(int personId);

        Person AddPerson(EntityModels.Person newPerson);
    }
}
