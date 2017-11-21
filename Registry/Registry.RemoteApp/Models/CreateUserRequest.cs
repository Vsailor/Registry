using System;

namespace Registry.Data.Models
{
  public class CreateUserRequest
  {
    public string Login { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public int GroupId { get; set; }

    public bool IsAdmin { get; set; }
  }
}