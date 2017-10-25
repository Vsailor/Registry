using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Registry.Data.Models;
using Registry.Data.Services.Abstract;
using System;

namespace Registry.Data.Services
{
  public class UserRepository : BaseRepository, IUserRepository
  {
    public async Task CreateUser(CreateUserRequest request)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@login", request.Login);
      parameters.Add("@name", request.Name);
      parameters.Add("@password", request.Password);
      parameters.Add("@permissions", request.Permissions);

      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        await conn.ExecuteAsync(
          StoredProcedures.CreateUser, 
          parameters, 
          commandType: CommandType.StoredProcedure);
      }
    }

    public async Task UpdateUser(UpdateUserRequest request)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@login", request.Login);
      parameters.Add("@name", request.Name);
      parameters.Add("@password", request.Password);
      parameters.Add("@enabled", request.IsEnabled);
      parameters.Add("@permissions", request.Permissions);

      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        await conn.ExecuteAsync(
          StoredProcedures.UpdateUser,
          parameters,
          commandType: CommandType.StoredProcedure);
      }
    }

    public async Task DeleteUser(string login)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@login", login);

      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        await conn.ExecuteAsync(
          StoredProcedures.DeleteUser,
          parameters,
          commandType: CommandType.StoredProcedure);
      }
    }

    public async Task<GetAllUsersResult[]> GetAllUsers()
    {
      IEnumerable<GetAllUsersResult> result;
      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
         result = await conn.QueryAsync<GetAllUsersResult>(
          StoredProcedures.GetAllUsers,
          commandType: CommandType.StoredProcedure);
      }

      return result.ToArray();
    }

    public async Task<GetUserByLoginResult> GetUserByLogin(string login)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@login", login);

      IEnumerable<GetUserByLoginResult> result;
      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        result = await conn.QueryAsync<GetUserByLoginResult>(
         StoredProcedures.GetUserByLogin,
         parameters,
         commandType: CommandType.StoredProcedure);
      }

      if (!result.Any())
      {
        return null;
      }

      return result.Single();
    }

    public async Task CreateUserGroup(string name)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@name", name);

      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        await conn.ExecuteAsync(
          StoredProcedures.CreateUserGroup,
          parameters,
          commandType: CommandType.StoredProcedure);
      }
    }

    public async Task UpdateUserGroup(UpdateUserGroupRequest request)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@id", request.Id);
      parameters.Add("@name", request.Name);

      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        await conn.ExecuteAsync(
          StoredProcedures.UpdateUserGroup,
          parameters,
          commandType: CommandType.StoredProcedure);
      }
    }

    public async Task<GetAllUserGroupsResult[]> GetUserGroups()
    {
      IEnumerable<GetAllUserGroupsResult> result;
      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        result = await conn.QueryAsync<GetAllUserGroupsResult>(
         StoredProcedures.GetAllUserGroups,
         commandType: CommandType.StoredProcedure);
      }

      return result.ToArray();
    }

    public async Task DeleteUserGroup(Guid id)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@id", id);

      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        await conn.ExecuteAsync(
          StoredProcedures.DeleteUserGroup,
          parameters,
          commandType: CommandType.StoredProcedure);
      }
    }
  }
}