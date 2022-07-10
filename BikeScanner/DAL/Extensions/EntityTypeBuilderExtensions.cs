using BikeScanner.Domain.Models.Base;
using BikeScanner.Domain.States;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeScanner.DAL.Extensions
{
    public static class EntityTypeBuilderExtensions
	{
        public static void HasIdColumn<T>(this EntityTypeBuilder<T> builder)
            where T : EntityBase
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();
        }

        public static void HasCrudColumns<T>(this EntityTypeBuilder<T> builder)
            where T : StatefulCrudBase
        {
            builder.HasIndex(x => x.State);
            //builder
            //    .Property(x => x.CreateDate)
            //    .HasColumnType("DATE")
            //    .ValueGeneratedOnAdd();
            //builder
            //    .Property(x => x.UpdateDate)
            //    .HasColumnType("DATE")
            //    .ValueGeneratedOnUpdate();
        }

        public static void UseExcludeDeleteStateFilter<T>(this EntityTypeBuilder<T> builder)
            where T : StatefulCrudBase =>
            builder.HasQueryFilter(x => x.State != BaseStates.Deleted.ToString());

        public static void UseActiveStateFilter<T>(this EntityTypeBuilder<T> builder)
            where T : StatefulCrudBase =>
            builder.HasQueryFilter(x => x.State == BaseStates.Active.ToString());

    }
}

