using Backlog.Core.Domain.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backlog.Data.Configuration.Masters
{
    public class ProjectEmployeeMapConfig : IEntityTypeConfiguration<ProjectEmployeeMap>
    {
        public void Configure(EntityTypeBuilder<ProjectEmployeeMap> builder)
        {
            builder.ToTable(nameof(ProjectEmployeeMap));

            builder.HasKey(x => x.Id);

            builder.HasOne(f => f.Employee)
                .WithMany()
                .HasForeignKey(p => p.EmployeeId)
                .HasConstraintName("FK_ProjectEmployeeMap_Employee")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);

            builder.HasOne(f => f.Project)
                .WithMany()
                .HasForeignKey(p => p.ProjectId)
                .HasConstraintName("FK_ProjectEmployeeMap_Project")
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true);
        }
    }
}