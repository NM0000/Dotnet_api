using Microsoft.AspNetCore.Mvc;

namespace myAPI.Controllers;

public class MemberController: ControllerBase
{
    public static List<Member> Members = new List<Member>();

    [HttpPost("/api/member")]
    public IActionResult Create([FromBody] MemberDto memberDto)
    {
        var member = new Member
        {
            Id = Members.Count + 1,
            FirstName = memberDto.FirstName,
            Email = memberDto.Email,
            PhoneNumber = memberDto.PhoneNumber,
            Address = memberDto.Address
        };

        Members.Add(member);
        return Ok(member);
    }


    [HttpGet("/api/member")]
    public IActionResult GetAll([FromQuery] MemberFilterDto filter)
    {
        var members = Members.Where(x =>
        (string.IsNullOrEmpty(filter.FirstName) || x.FirstName.Contains(filter.FirstName))
         && (string.IsNullOrEmpty(filter.Address) || x.Address.Contains(filter.Address))
         ).ToList();
        return Ok(members);
    }

    [HttpGet("/api/member/{id}")]
    public IActionResult GetById(int id)
    {
        var member = Members.FirstOrDefault(x => x.Id == id);
        if (member == null)
        {
            return NotFound();
        }
        return Ok(member);
    }


    [HttpPut("/api/member/{id}")]
    public IActionResult Update(int id, [FromBody] MemberDto memberDto)
    {
        var exisingMember = Members.FirstOrDefault(x => x.Id == id);

        if (exisingMember == null)
        {
            return NotFound();
        }

        exisingMember.FirstName = memberDto.FirstName;
        exisingMember.Email = memberDto.Email;
        exisingMember.PhoneNumber = memberDto.PhoneNumber;
        exisingMember.Address = memberDto.Address;

        return Ok("Member updated successfully");
    }

    [HttpDelete("/api/member/{id}")]
    public IActionResult Delete(int id)
    {
        var member = Members.FirstOrDefault(x => x.Id == id);
        if (member == null)
        {
            return NotFound();
        }

        Members.Remove(member);
        return Ok("Member deleted successfully");
    }

}


public class MemberFilterDto
{
    public string? FirstName { get; set; }
    public string? Address { get; set; }
}

public class MemberDto // Data Transfer Object
{
    public string FirstName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}

public class Member // Model | Entity | Table
{
    public int Id { get; set; } // auto generate
    public string FirstName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}