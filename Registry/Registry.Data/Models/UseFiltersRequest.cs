﻿using System;

namespace Registry.Data.Models
{
  public class UseFiltersRequest
  {
    public int? Id { get; set; }

    public string Name { get; set; }

    public string[] Tags { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? ResourceGroupId { get; set; }
  }
}