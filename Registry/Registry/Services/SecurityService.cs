using System.Security.Cryptography;
using System.Text;

namespace Registry.Services
{
  public class SecurityService
  {
    public static string Crypt(string value)
    {
      StringBuilder Sb = new StringBuilder();

      using (SHA256 hash = SHA256.Create())
      {
        Encoding enc = Encoding.UTF8;
        var result = hash.ComputeHash(enc.GetBytes(value));

        foreach (var b in result)
          Sb.Append(b.ToString("x2"));
      }

      return Sb.ToString();
    }
  }
}