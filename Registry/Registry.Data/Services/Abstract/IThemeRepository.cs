using System;
using System.Threading.Tasks;
using Registry.Data.Models;

namespace Registry.Data.Services.Abstract
{
  public interface IThemeRepository
  {
    Task<GetAllThemesResult[]> GetAllThemes();

    Task UpdateTheme(Guid id, string name, string ownerLogin);

    Task CreateTheme(string ownerLogin, string name);

    Task DeleteTheme(Guid id);
  }
}