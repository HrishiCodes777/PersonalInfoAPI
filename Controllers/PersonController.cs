using apitask.DAO;
using apitask.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace apitask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonDao _personDao;

        public PersonController(IPersonDao personDao)
        {
            _personDao = personDao;
        }

        [HttpPost("add", Name = "AddPerson")]
        public async Task<ActionResult<Person>> AddPerson(Person person)
        {
            int value = 0;
            if (person == null)
            {
                return BadRequest("Empty Request");
            }
            value = await _personDao.AddPerson(person);
            return Ok(value);
        }

        [HttpGet("persons")]
        public async Task<ActionResult<List<Person>>> GetAllPersons()
        {
            var result = await _personDao.GetAllPersons();
            if (result == null)
            {
                return BadRequest("No data found");
            }
            return Ok(result);
        }

        [HttpDelete("delete-by-aadhar/{aadharCardNumber}")]
        public async Task<IActionResult> DeletePersonByAadharCardNumber(string aadharCardNumber)
        {
            var result = await _personDao.DeletePersonByAadharCardNumber(aadharCardNumber);
            if (result)
                return Ok("Person deleted successfully.");
            else
                return NotFound("Person not found or deletion failed.");
        }

        [HttpPut("update-name/{id}")]
        public async Task<IActionResult> UpdatePersonNameById(int id, string newName)
        {
            var result = await _personDao.UpdatePersonNameById(id, newName);
            if (result)
                return Ok("Person Name Updated successfully.");
            else
                return NotFound("Person not found or update failed.");
        }

        [HttpPut("update-name-by-aadhar/{aadharCardNumber}")]
        public async Task<IActionResult> UpdatePersonNameByAadhar(string aadharCardNumber, [FromBody] string newName)
        {
            var result = await _personDao.UpdatePersonNameByAadhar(aadharCardNumber, newName);
            if (result)
                return Ok(new { message = "Name updated successfully." });
            else
                return NotFound(new { message = "Person not found or update failed." });
        }
    }
}
