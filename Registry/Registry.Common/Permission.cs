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
    UpdatePermissions
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
      { Permission.UpdatePermissions, "Can update permissions" }
    };
  }
}