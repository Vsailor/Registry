﻿using Registry.Common;

namespace Registry.Data.Models
{
  public class UpdateUserRequest
  {
    public string Login { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public bool IsEnabled { get; set; }

    public string Permissions { get; set; }
  }
}