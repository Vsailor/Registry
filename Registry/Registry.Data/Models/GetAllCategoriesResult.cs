using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registry.Data.Models
{
  public class GetAllCategoriesResult
  {
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid? ParentId { get; set; }
  }
}