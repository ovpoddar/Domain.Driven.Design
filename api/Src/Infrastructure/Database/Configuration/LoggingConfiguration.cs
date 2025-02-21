using DDD.Domain.Entities.LoggingEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Infrastructure.Database.Configuration;

public class LoggingConfiguration : IEntityTypeConfiguration<Logging>
{
    public void Configure(EntityTypeBuilder<Logging> builder) =>
        builder.Property(a => a.Properties)
                   .HasColumnType("xml");
}
