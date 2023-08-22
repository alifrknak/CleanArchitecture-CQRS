using Core.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class BaseEntityConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
	where TEntity : Entity<TId>
{
	public void Configure(EntityTypeBuilder<TEntity> builder)
	{
		builder.Property(b => b.Id).HasColumnName("Id").IsRequired();
		builder.HasQueryFilter(e => !e.DeletedDate.HasValue);
	}
}