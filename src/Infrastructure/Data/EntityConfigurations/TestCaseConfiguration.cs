using Core.Entities.TestAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations;

public class TestCaseConfiguration : IEntityTypeConfiguration<TestCaseAggregate>
{
    public void Configure(EntityTypeBuilder<TestCaseAggregate> builder)
    {
        
    }
}
