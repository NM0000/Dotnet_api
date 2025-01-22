using Microsoft.AspNetCore.Mvc;

namespace myAPI.Controllers;

public class StudentController : ControllerBase
{
    [HttpPost("api/register")]
    public IActionResult Register(Student student)
    {
        var response = new 
        {
            status = "success",
            message = "Student registered",
            Data =student

        };
        return Ok(response);
            
    }
}

public class Student
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string ConfirmPassword { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}