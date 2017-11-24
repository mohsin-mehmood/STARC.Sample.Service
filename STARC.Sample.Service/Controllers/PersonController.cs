using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using STARC.Sample.Service.DataServices;

namespace STARC.Sample.Service.Controllers
{


    [Produces("application/json")]
    [Route("api/v1/persons")]
    public class PersonController : Controller
    {
        private ILogger<PersonController> _logger;
        private readonly IPersonsService _personService;
        public PersonController(ILogger<PersonController> logger, IPersonsService personService)
        {
            _logger = logger;
            _personService = personService;
        }

        /// <summary>
        /// Get list of all persons 
        /// </summary>
        /// <returns>List of person</returns>
        [HttpGet]
        [Route("")]
        [Authorize(Policy = "ReadStudentPolicy")]
        public async Task<IActionResult> GetAllPersons()
        {
            var result = await _personService.GetAllPersons();
            return Ok(result);
        }

        /// <summary>
        /// Get a person by identifier
        /// </summary>
        /// <param name="id">Person Identifier</param>
        /// <returns>Person Details</returns>
        [HttpGet]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetPersonById(int id)
        {
            var person = await _personService.GetPersonById(id);
            if (person != null)
            {
                return Ok(person);
            }
            else
            {
                return NotFound();
            }

        }

        /// <summary>
        /// Ensures service is up and running
        /// </summary>        
        /// <returns>OK status</returns>
        [HttpGet]
        [Route("ping")]
        public IActionResult Ping()
        {
            return Ok("OK");
        }

        /// <summary>
        /// Adds new person
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Newly added person</returns>
        [HttpPost]
        [Authorize]
        public IActionResult AddPerson(EntityModels.Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addedPerson = _personService.AddPerson(person);
            return Created($"/api/v1/persons/{addedPerson.PersonId}", addedPerson);
        }
    }
}