using Registry.Common;

namespace Registry.Data.Models
{
  public class UpdateUserRequest
  {
    public string Login { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public Role Role { get; set; }

    public bool IsEnabled { get; set; }
  }
}