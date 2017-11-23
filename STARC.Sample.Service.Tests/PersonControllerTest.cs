using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using STARC.Sample.Models;
using STARC.Sample.Service.Controllers;
using STARC.Sample.Service.DataServices;

namespace STARC.Sample.Service.Tests
{
    [TestClass]
    public class PersonControllerTest
    {
        //Initialize Mock Servicess
        static Mock<IPersonsService> personServiceMock = new Mock<IPersonsService>();
        static Mock<ILogger<PersonController>> loggerMock = new Mock<ILogger<PersonController>>();

        static PersonControllerTest()
        {
            //setup Mocks
            personServiceMock.Setup(m => m.GetAllPersons()).Returns(() =>
            {
                return Task.Run(() =>
                {
                    return new List<Person> {
                                new Person { FirstName = "Person 1", LastName = "Person 1"},
                                new Person { FirstName = "Person 2", LastName = "Person 2"  }
                            };
                });
            });

        }

        [TestMethod]
        [Owner("mome@ciklum.com")]
        public void Ping_OkResult_Test()
        {
            PersonController personController = new PersonController(loggerMock.Object, personServiceMock.Object);

            var result = personController.Ping() as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode.Value, (int)HttpStatusCode.OK);
        }

        [TestMethod]
        [Owner("mome@xciklum.com")]
        public async void GetAllPersons_OkResult_Test()
        {
            PersonController personController = new PersonController(loggerMock.Object, personServiceMock.Object);
            var result =  await personController.GetAllPersons() as OkObjectResult;

            List<Person> persons = result.Value as List<Person>;
            Assert.AreEqual(result.StatusCode.Value, (int)HttpStatusCode.OK);
            Assert.IsTrue(persons.Count > 0);
        }
    }
}
