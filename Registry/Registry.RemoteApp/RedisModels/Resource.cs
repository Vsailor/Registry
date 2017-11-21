namespace Registry.Data.RedisModels
{
  public class Resource
  {
    public string Name { get; set; }

    public string Description { get; set; }

    public string Url { get; set; }

    public string FileName { get; set; }

    public string Owner { get; set; }

    public string SaveDate { get; set; }
  }
}