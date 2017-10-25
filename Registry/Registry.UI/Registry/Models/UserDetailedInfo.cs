using Registry.Common;

namespace Registry.Models
{
  public class UserDetailedInfo
  {
    public string Login { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public bool IsActive { get; set; }
  }
}