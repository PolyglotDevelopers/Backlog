using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backlog.Core.Domain.Localization;

namespace Backlog.Data.Configuration.Localization
{
    public class LocaleResourceConfig : IEntityTypeConfiguration<LocaleResource>
    {
        public void Configure(EntityTypeBuilder<LocaleResource> builder)
        {
            builder.ToTable(nameof(LocaleResource));

            builder.HasKey(x => x.Id);

            builder.HasOne(f => f.Language)
                .WithMany()
                .HasForeignKey(p => p.LanguageId)
                .HasConstraintName("FK_LocaleResource_Language")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);
        }
    }
}