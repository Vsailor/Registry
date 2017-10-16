namespace Registry.Data.Models
{
  public class GetUserByLoginResult
  {
    public string Name { get; set; }

    public string Password { get; set; }

    public byte Role { get; set; }

    public bool Enabled { get; set; }
  }
}