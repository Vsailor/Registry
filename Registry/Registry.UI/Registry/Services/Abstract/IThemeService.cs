using System;
using System.Threading.Tasks;
using Registry.Data.Models;

namespace Registry.Services.Abstract
{
  public interface IThemeService
  {
    Task<GetAllThemesResult[]> GetAllThemes();

    Task UpdateTheme(Guid id, string name, string ownerLogin);

    Task DeleteTheme(Guid id);

    Task CreateTheme(string name, string ownerLogin);
  }
}