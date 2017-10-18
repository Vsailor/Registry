namespace Registry.Data.Models
{
  public class GetUserByLoginResult
  {
    public string Name { get; set; }

    public string Password { get; set; }

    public bool Enabled { get; set; }

    public string Permissions { get; set; }
  }
}