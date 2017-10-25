using System;

namespace Registry.Data.Models
{
  public class GetAllResourcesResult
  {
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Url { get; set; }

    public string FileName { get; set; }
  }
}