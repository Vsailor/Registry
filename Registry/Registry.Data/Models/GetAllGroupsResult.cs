using System;

namespace Registry.Data.Models
{
  public class GetAllGroupsResult
  {
    public Guid Id { get; set; }
    public string Leader { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
  }
}