using System;
using System.Threading.Tasks;
using Registry.Data.Models;

namespace Registry.Data.Services.Abstract
{
  public interface IThemeRepository
  {
    Task<GetAllThemesResult[]> GetAllThemes();

    Task UpdateTheme(Guid id, string name);

    Task DeleteTheme(Guid id);

    Task<string[]> GetThemes(string login);

    Task CreateTheme(string name, CreateThemeUserRequest[] createThemeUserRequest);
  }
}