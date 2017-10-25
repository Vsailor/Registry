using System;

namespace Registry.Data.Models
{
  public class UpdateUserGroupRequest
  {
    public Guid Id { get; set; }

    public string Name { get; set; }
  }
}