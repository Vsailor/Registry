using System;

namespace Registry.Data.Models
{
  public class GetResourceDetailsResult
  {
    public Guid Category { get; set; }

    public Guid[] ResourceGroups { get; set; }

    public string[] Tags { get; set; }

    public string Uid { get; set; }
  }
}