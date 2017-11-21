using System.Configuration;

namespace Registry.Data.Services
{
  public abstract class BaseRepository
  {
    protected readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
  }
}