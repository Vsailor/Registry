using Microsoft.Practices.Unity;
using StackExchange.Redis;

namespace Registry.Data
{
  public class RegistryCommon
  {
    public static UnityContainer Container { get; set; }

    public static IDatabase RedisDb => ConnectionMultiplexer.Connect("52.232.56.117:6379").GetDatabase();
  }
}
