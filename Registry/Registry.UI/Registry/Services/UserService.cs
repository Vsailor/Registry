using System.Collections.Generic;
using System.Threading.Tasks;
using Registry.Models;
using Registry.Permissions;
using Registry.Services.Abstract;

namespace Registry.Services
{
  public class UserService : IUserService
  {
    public async Task<IEnumerable<UserBasicInfo>> GetAllUsers()
    {
      await Task.Delay(3500);
      return new[]
        {
          new UserBasicInfo {Login = "Jc", Name = "Jc"},
          new UserBasicInfo {Login = "Riley", Name = "Riley"},
          new UserBasicInfo {Login = "Royal", Name = "Royal"},
          new UserBasicInfo {Login = "Raymundo", Name = "Raymundo"},
          new UserBasicInfo {Login = "Josiah", Name = "Josiah"},
          new UserBasicInfo {Login = "Modesto", Name = "Modesto"},
          new UserBasicInfo {Login = "Robert", Name = "Robert"},
          new UserBasicInfo {Login = "Charley", Name = "Charley"},
          new UserBasicInfo {Login = "Cameron", Name = "Cameron"},
          new UserBasicInfo {Login = "Theron", Name = "Theron"},
          new UserBasicInfo {Login = "Adalberto", Name = "Adalberto"},
          new UserBasicInfo {Login = "Ronny", Name = "Ronny"},
          new UserBasicInfo {Login = "Deangelo", Name = "Deangelo"},
          new UserBasicInfo {Login = "Alfred", Name = "Alfred"},
          new UserBasicInfo {Login = "Angel", Name = "Angel"},
          new UserBasicInfo {Login = "Raymond", Name = "Raymond"},
          new UserBasicInfo {Login = "Lincoln", Name = "Lincoln"},
          new UserBasicInfo {Login = "Mac", Name = "Mac"},
          new UserBasicInfo {Login = "Luigi", Name = "Luigi"},
          new UserBasicInfo {Login = "Elton", Name = "Elton"},
          new UserBasicInfo {Login = "Roderick", Name = "Roderick"},
          new UserBasicInfo {Login = "Sid", Name = "Sid"},
          new UserBasicInfo {Login = "Randall", Name = "Randall"},
          new UserBasicInfo {Login = "Terence", Name = "Terence"},
          new UserBasicInfo {Login = "Al", Name = "Al"},
          new UserBasicInfo {Login = "Yong", Name = "Yong"},
          new UserBasicInfo {Login = "Teddy", Name = "Teddy"},
          new UserBasicInfo {Login = "Garfield", Name = "Garfield"},
          new UserBasicInfo {Login = "Lyndon", Name = "Lyndon"},
          new UserBasicInfo {Login = "Ward", Name = "Ward"},
          new UserBasicInfo {Login = "Ronald", Name = "Ronald"},
          new UserBasicInfo {Login = "Stuart", Name = "Stuart"},
          new UserBasicInfo {Login = "Rich", Name = "Rich"},
          new UserBasicInfo {Login = "Mariano", Name = "Mariano"},
          new UserBasicInfo {Login = "Gale", Name = "Gale"},
          new UserBasicInfo {Login = "Florentino", Name = "Florentino"},
          new UserBasicInfo {Login = "Vaughn", Name = "Vaughn"},
          new UserBasicInfo {Login = "Dannie", Name = "Dannie"},
          new UserBasicInfo {Login = "Danilo", Name = "Danilo"},
          new UserBasicInfo {Login = "Dwight", Name = "Dwight"},
          new UserBasicInfo {Login = "Caleb", Name = "Caleb"},
          new UserBasicInfo {Login = "Quentin", Name = "Quentin"},
          new UserBasicInfo {Login = "Rickey", Name = "Rickey"},
          new UserBasicInfo {Login = "Vincent", Name = "Vincent"},
          new UserBasicInfo {Login = "Brandon", Name = "Brandon"},
          new UserBasicInfo {Login = "Sterling", Name = "Sterling"},
          new UserBasicInfo {Login = "Abel", Name = "Abel"},
          new UserBasicInfo {Login = "Allen", Name = "Allen"},
          new UserBasicInfo {Login = "Willian", Name = "Willian"},
          new UserBasicInfo {Login = "Wilmer", Name = "Wilmer"},
        };

    }

    public async Task<UserDetailedInfo> GetUser(string login)
    {
      await Task.Delay(1000);
      return new UserDetailedInfo
      {
        Login = login,
        Name = login,
        IsActive = true,
        Password = "",
        Role = Role.Admin
      };
    }
  }
}