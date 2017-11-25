using System;

namespace Registry.Data.Models
{
  public class CreateResourceRequest
  {
    public string OwnerLogin { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Url { get; set; }

    public string[] Tags { get; set; }

    public string FileName { get; set; }

    public Guid CategoryId { get; set; }

    public Guid[] ResourceGroups { get; set; } 

    public string SaveDate { get; set; }

    public string Uid { get; set; }
  }
}