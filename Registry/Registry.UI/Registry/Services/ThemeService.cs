using System;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Registry.Common;
using Registry.Data.Models;
using Registry.Data.Services.Abstract;
using Registry.Services.Abstract;

namespace Registry.Services
{
  public class ThemeService : IThemeService
  {
    private IThemeRepository _themeRepository;
    public ThemeService()
    {
      _themeRepository = RegistryCommon.Instance.Container.Resolve<IThemeRepository>();
    }

    public async Task<GetAllThemesResult[]> GetAllThemes()
    {
      return await _themeRepository.GetAllThemes();
    }

    public async Task UpdateTheme(Guid id, string name, string ownerLogin)
    {
      await _themeRepository.UpdateTheme(id, name, ownerLogin);
    }

    public async Task DeleteTheme(Guid id)
    {
      await _themeRepository.DeleteTheme(id);
    }

    public async Task CreateTheme(string name, string ownerLogin)
    {
      await _themeRepository.CreateTheme(ownerLogin, name);
    }
  }
}