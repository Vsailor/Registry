namespace Registry.Data.Models
{
  public class CreateResourceRequest
  {
    public string OwnerLogin { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Url { get; set; }

    public string Tags { get; set; }

    public string FileName { get; set; }
  }
}