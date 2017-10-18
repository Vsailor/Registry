using Registry.Common;

namespace Registry.Data.Models
{
  public class CreateUserRequest
  {
    public string Login { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }
    public string Permissions { get; set; }
  }
}