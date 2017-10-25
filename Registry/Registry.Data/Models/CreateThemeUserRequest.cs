using System;
using Registry.Common;

namespace Registry.Data.Models
{
  public class CreateThemeUserRequest
  {
    public string UserLogin { get; set; }

    public int Role { get; set; }
  }
}