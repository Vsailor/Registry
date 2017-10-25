namespace Registry.Data
{
  public class StoredProcedures
  {
    public const string CreateUser = "createUser";
    public const string GetAllUsers = "getAllUsers";
    public const string GetUserByLogin = "getUserByLogin";
    public const string UpdateUser = "updateUser";
    public const string DeleteUser = "deleteUser";

    public const string GetAllCategories = "getAllCategories";
    public const string UpdateCategory = "updateCategory";
    public const string DeleteCategory = "deleteCategory";
    public const string CreateCategory = "createCategory";

    public const string CreateTheme = "createTheme";
    public const string GetAllThemes = "getAllThemes";
    public const string UpdateTheme = "updateTheme";
    public const string DeleteTheme = "deleteTheme";
    public const string GetUserThemes = "getUserThemes";
    public const string CreateThemeUser = "createThemeUser";

    public const string CreateResource = "createResource";
    public const string GetAllResources = "getAllResources";
  }
}