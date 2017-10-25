using System.Collections.Generic;

namespace Registry.Common
{
  public enum Permission
  {
    Unknown,
    CreateUser,
    UpdateUser,
    DeleteUser,
    SeeUserList,
    SeeCategoriesList,
    CreateCategory,
    UpdateCategory,
    DeleteCategory,
    UpdatePermissions,
    SeeThemesList,
    CreateTheme,
    DeleteTheme,
    CanSeeListOfResources,
    CanCreateResource
  }

  public class PermissionCommon
  {
    public static readonly Dictionary<Permission, string> Titles = new Dictionary<Permission, string>()
    {
      { Permission.CreateUser, "Creation of user" },
      { Permission.UpdateUser, "Updating of user" },
      { Permission.DeleteUser, "Deletion of user" },
      { Permission.SeeUserList, "Can see list of users" },
      { Permission.SeeCategoriesList, "Can see list of categories" },
      { Permission.CreateCategory, "Can create category" },
      { Permission.UpdateCategory, "Can update category" },
      { Permission.DeleteCategory, "Can delete category" },
      { Permission.UpdatePermissions, "Can update permissions" },
      { Permission.SeeThemesList, "Can see list of themes" },
      { Permission.CreateTheme, "Can create theme" },
      { Permission.DeleteTheme, "Can delete theme" },
      { Permission.CanSeeListOfResources, "Can see list of resources" },
      { Permission.CanCreateResource, "Can create resource" }
    };
  }
}