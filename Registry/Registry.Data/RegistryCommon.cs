using Microsoft.Practices.Unity;
using StackExchange.Redis;

namespace Registry.Data
{
  public class RegistryCommon
  {
    public static UnityContainer Container { get; set; }

    public static IDatabase RedisDb => ConnectionMultiplexer.Connect("localhost").GetDatabase();
  }
}
