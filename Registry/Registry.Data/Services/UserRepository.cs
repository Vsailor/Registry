﻿using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Registry.Data.Models;
using Registry.Data.Services.Abstract;

namespace Registry.Data.Services
{
  public class UserRepository : IUserRepository
  {
    private readonly string _connectionString = 
      ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    public async Task CreateUser(CreateUserRequest request)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@login", request.Login);
      parameters.Add("@name", request.Name);
      parameters.Add("@password", request.Password);
      parameters.Add("@role", (int)request.Role);

      using (IDbConnection conn = new SqlConnection(_connectionString))
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
      parameters.Add("@role", (int)request.Role);
      parameters.Add("@enabled", request.IsEnabled);

      using (IDbConnection conn = new SqlConnection(_connectionString))
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

      using (IDbConnection conn = new SqlConnection(_connectionString))
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
      using (IDbConnection conn = new SqlConnection(_connectionString))
      {
         result = await conn.QueryAsync<GetAllUsersResult>(
          StoredProcedures.GetAllUsers,
          commandType: CommandType.StoredProcedure);
      }

      if (result.Count() == 0)
      {
        return new GetAllUsersResult[0];
      }

      return result.ToArray();
    }

    public async Task<GetUserByLoginResult> GetUserByLogin(string login)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@login", login);

      IEnumerable<GetUserByLoginResult> result;
      using (IDbConnection conn = new SqlConnection(_connectionString))
      {
        result = await conn.QueryAsync<GetUserByLoginResult>(
         StoredProcedures.GetUserByLogin,
         parameters,
         commandType: CommandType.StoredProcedure);
      }

      if (result.Count() == 0)
      {
        return null;
      }

      return result.Single();
    }
  }
}