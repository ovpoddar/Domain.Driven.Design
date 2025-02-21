using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Entities.BaseEntities;

public abstract class Entity<T>
{
    [Key]
    public T Id { get; init; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; set; }

    protected Entity(T id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }
}
