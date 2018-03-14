using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace TMShopsCore.Models
{
    public partial class TMShopsContext : DbContext
    {
        public virtual DbSet<Bill> Bill { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<GroupItem> GroupItem { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Modules> Modules { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<SubBill> SubBill { get; set; }
        public virtual DbSet<SubItem> SubItem { get; set; }
        public virtual DbSet<SubRoles> SubRoles { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        public TMShopsContext(DbContextOptions options) : base(options) { }
        //public TMShopsContext(DbContextOptions<TMShopsContext> options) : base(options) { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //    optionsBuilder.UseSqlServer(cs);
        //}

        /// <summary>  
        /// Overriding Save Changes  
        /// </summary>  
        /// <returns></returns>  
        public override int SaveChanges()
        {
            //Gt user Name from  session or other authentication   
            var Username = TMShopsCore.Common.Auth.isAuth ? TMShopsCore.Common.Auth.AuthUser.Username : "";
            //Entity List Added
            var selectedEntityList = ChangeTracker.Entries().Where(x => x.Entity is EntityBase && x.State == EntityState.Added);
            foreach (var entity in selectedEntityList)
            {
                ((EntityBase)entity.Entity).CreatedBy = Username;
                ((EntityBase)entity.Entity).CreatedAt = DateTime.Now;
            }
            //Entity List Modified
            selectedEntityList = ChangeTracker.Entries().Where(x => x.Entity is EntityBase && x.State == EntityState.Modified);
            foreach (var entity in selectedEntityList)
            {
                ((EntityBase)entity.Entity).UpdatedBy = Username;
                ((EntityBase)entity.Entity).UpdatedAt = DateTime.Now;
            }
            //Entity List Deleted
            selectedEntityList = ChangeTracker.Entries().Where(x => x.Entity is EntityBase && x.State == EntityState.Deleted);
            foreach (var entity in selectedEntityList)
            {
                ((EntityBase)entity.Entity).DeleteBy = Username;
                ((EntityBase)entity.Entity).DeleteAt = DateTime.Now;
            }
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bill>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(128);

                entity.Property(e => e.CodeKey)
                    .HasColumnName("codeKey")
                    .HasMaxLength(128);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasMaxLength(256);

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerId")
                    .HasMaxLength(128);

                entity.Property(e => e.DeleteAt)
                    .HasColumnName("deleteAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("deleteBy")
                    .HasMaxLength(256);

                entity.Property(e => e.Details)
                    .HasColumnName("details")
                    .HasMaxLength(2000);

                entity.Property(e => e.Extras).HasColumnName("extras");

                entity.Property(e => e.Flag).HasColumnName("flag");

                entity.Property(e => e.Orders).HasColumnName("orders");

                entity.Property(e => e.TotalItem).HasColumnName("totalItem");

                entity.Property(e => e.TotalPrice)
                    .HasColumnName("totalPrice")
                    .HasColumnType("decimal");

                entity.Property(e => e.TotalQuantity).HasColumnName("totalQuantity");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.CardId)
                    .HasColumnName("cardId")
                    .HasMaxLength(128);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasMaxLength(256);

                entity.Property(e => e.DateOfBirth)
                    .HasColumnName("dateOfBirth")
                    .HasMaxLength(50);

                entity.Property(e => e.DeleteAt)
                    .HasColumnName("deleteAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("deleteBy")
                    .HasMaxLength(256);

                entity.Property(e => e.Details)
                    .HasColumnName("details")
                    .HasMaxLength(2000);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.Extras).HasColumnName("extras");

                entity.Property(e => e.Facebook)
                    .HasColumnName("facebook")
                    .HasMaxLength(255);

                entity.Property(e => e.Flag).HasColumnName("flag");

                entity.Property(e => e.FullName)
                    .HasColumnName("fullName")
                    .HasMaxLength(150);

                entity.Property(e => e.Images).HasColumnName("images");

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasMaxLength(150);

                entity.Property(e => e.Orders).HasColumnName("orders");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AppKey)
                    .HasColumnName("appKey")
                    .HasMaxLength(128);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasMaxLength(256);

                entity.Property(e => e.DeleteAt)
                    .HasColumnName("deleteAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("deleteBy")
                    .HasMaxLength(256);

                entity.Property(e => e.Details)
                    .HasColumnName("details")
                    .HasMaxLength(2000);

                entity.Property(e => e.Extras).HasColumnName("extras");

                entity.Property(e => e.Flag).HasColumnName("flag");

                entity.Property(e => e.IdKey)
                    .HasColumnName("idKey")
                    .HasMaxLength(128);

                entity.Property(e => e.Levels).HasColumnName("levels");

                entity.Property(e => e.Orders).HasColumnName("orders");

                entity.Property(e => e.ParentId).HasColumnName("parentId");

                entity.Property(e => e.ParentsId).HasColumnName("parentsId");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<GroupItem>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AppKey)
                    .HasColumnName("appKey")
                    .HasMaxLength(128);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasMaxLength(256);

                entity.Property(e => e.DeleteAt)
                    .HasColumnName("deleteAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("deleteBy")
                    .HasMaxLength(256);

                entity.Property(e => e.Details)
                    .HasColumnName("details")
                    .HasMaxLength(2000);

                entity.Property(e => e.EndedAt)
                    .HasColumnName("endedAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.Extras).HasColumnName("extras");

                entity.Property(e => e.Flag).HasColumnName("flag");

                entity.Property(e => e.GroupId).HasColumnName("groupId");

                entity.Property(e => e.ItemId).HasColumnName("itemId");

                entity.Property(e => e.Orders).HasColumnName("orders");

                entity.Property(e => e.StartedAt)
                    .HasColumnName("startedAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasMaxLength(256);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupItem)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_group_item_group");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.GroupItem)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_group_item_item");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AppKey)
                    .HasColumnName("appKey")
                    .HasMaxLength(255);

                entity.Property(e => e.CodeKey)
                    .HasColumnName("codeKey")
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasMaxLength(256);

                entity.Property(e => e.DeleteAt)
                    .HasColumnName("deleteAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("deleteBy")
                    .HasMaxLength(256);

                entity.Property(e => e.Details)
                    .HasColumnName("details")
                    .HasMaxLength(2000);

                entity.Property(e => e.EndedAt)
                    .HasColumnName("endedAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.Extras).HasColumnName("extras");

                entity.Property(e => e.Flag).HasColumnName("flag");

                entity.Property(e => e.IdKey)
                    .HasColumnName("idKey")
                    .HasMaxLength(255);

                entity.Property(e => e.Image)
                    .HasColumnName("image")
                    .HasColumnType("image");

                entity.Property(e => e.Images).HasColumnName("images");

                entity.Property(e => e.Orders).HasColumnName("orders");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal");

                entity.Property(e => e.PriceOld)
                    .HasColumnName("priceOld")
                    .HasColumnType("decimal");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.QuantityTotal).HasColumnName("quantityTotal");

                entity.Property(e => e.StartedAt)
                    .HasColumnName("startedAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Modules>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasColumnName("action")
                    .HasMaxLength(512);

                entity.Property(e => e.AppKey)
                    .IsRequired()
                    .HasColumnName("appKey")
                    .HasMaxLength(128);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("createdBy")
                    .HasMaxLength(256);

                entity.Property(e => e.DeleteAt)
                    .HasColumnName("deleteAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("deleteBy")
                    .HasMaxLength(256);

                entity.Property(e => e.Details)
                    .HasColumnName("details")
                    .HasMaxLength(2000);

                entity.Property(e => e.Extras).HasColumnName("extras");

                entity.Property(e => e.Flag).HasColumnName("flag");

                entity.Property(e => e.Icon)
                    .HasColumnName("icon")
                    .HasMaxLength(256);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128);

                entity.Property(e => e.Orders).HasColumnName("orders");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updatedBy")
                    .HasMaxLength(256);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("createdBy")
                    .HasMaxLength(256);

                entity.Property(e => e.DeleteAt)
                    .HasColumnName("deleteAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("deleteBy")
                    .HasMaxLength(256);

                entity.Property(e => e.Details)
                    .HasColumnName("details")
                    .HasMaxLength(2000);

                entity.Property(e => e.Modules)
                    .HasColumnName("modules");

                entity.Property(e => e.Flag).HasColumnName("flag");

                entity.Property(e => e.Icon)
                    .HasColumnName("icon")
                    .HasMaxLength(256);

                entity.Property(e => e.AppKey)
                    .IsRequired()
                    .HasColumnName("appKey")
                    .HasMaxLength(128);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(256);

                entity.Property(e => e.Orders).HasColumnName("orders");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updatedBy")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<Settings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasMaxLength(256);

                entity.Property(e => e.DeleteAt)
                    .HasColumnName("deleteAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("deleteBy")
                    .HasMaxLength(256);

                entity.Property(e => e.Details)
                    .HasColumnName("details")
                    .HasMaxLength(2000);

                entity.Property(e => e.Extras).HasColumnName("extras");

                entity.Property(e => e.Flag).HasColumnName("flag");

                entity.Property(e => e.ModuleKey)
                    .IsRequired()
                    .HasColumnName("moduleKey")
                    .HasMaxLength(255);

                entity.Property(e => e.Orders).HasColumnName("orders");

                entity.Property(e => e.SubKey)
                    .IsRequired()
                    .HasColumnName("subKey")
                    .HasMaxLength(255);

                entity.Property(e => e.SubValue).HasColumnName("subValue");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasMaxLength(256);

                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<SubBill>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CodeKey)
                    .HasColumnName("codeKey")
                    .HasMaxLength(128);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasMaxLength(256);

                entity.Property(e => e.DeleteAt)
                    .HasColumnName("deleteAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("deleteBy")
                    .HasMaxLength(256);

                entity.Property(e => e.Details)
                    .HasColumnName("details")
                    .HasMaxLength(2000);

                entity.Property(e => e.Extras).HasColumnName("extras");

                entity.Property(e => e.Flag).HasColumnName("flag");

                entity.Property(e => e.IdKey)
                    .HasColumnName("idKey")
                    .HasMaxLength(128);

                entity.Property(e => e.ItemId).HasColumnName("itemId");

                entity.Property(e => e.Orders).HasColumnName("orders");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal");

                entity.Property(e => e.PriceOld)
                    .HasColumnName("priceOld")
                    .HasColumnType("decimal");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.Property(e => e.TotalPrice)
                    .HasColumnName("totalPrice")
                    .HasColumnType("decimal");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasMaxLength(256);

                entity.HasOne(d => d.IdKeyNavigation)
                    .WithMany(p => p.SubBill)
                    .HasForeignKey(d => d.IdKey)
                    .HasConstraintName("FK_sub_bill_bill");
            });

            modelBuilder.Entity<SubItem>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AppKey)
                    .HasColumnName("appKey")
                    .HasMaxLength(128);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasMaxLength(256);

                entity.Property(e => e.DeleteAt)
                    .HasColumnName("deleteAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("deleteBy")
                    .HasMaxLength(256);

                entity.Property(e => e.Details)
                    .HasColumnName("details")
                    .HasMaxLength(2000);

                entity.Property(e => e.Extras).HasColumnName("extras");

                entity.Property(e => e.Flag).HasColumnName("flag");

                entity.Property(e => e.IdKey).HasColumnName("idKey");

                entity.Property(e => e.Images).HasColumnName("images");

                entity.Property(e => e.ItemId).HasColumnName("itemId");

                entity.Property(e => e.MainKey)
                    .HasColumnName("mainKey")
                    .HasMaxLength(255);

                entity.Property(e => e.Orders).HasColumnName("orders");

                entity.Property(e => e.SubValue)
                    .HasColumnName("subValue")
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasMaxLength(256);

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasMaxLength(255);

                entity.HasOne(d => d.IdKeyNavigation)
                    .WithMany(p => p.SubItem)
                    .HasForeignKey(d => d.IdKey)
                    .HasConstraintName("FK_sub_item_item");
            });

            modelBuilder.Entity<SubRoles>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("createdBy")
                    .HasMaxLength(256);

                entity.Property(e => e.DeleteAt)
                    .HasColumnName("deleteAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("deleteBy")
                    .HasMaxLength(256);

                entity.Property(e => e.Flag).HasColumnName("flag");

                entity.Property(e => e.IsDelete).HasColumnName("isDelete");

                entity.Property(e => e.IsInsert).HasColumnName("isInsert");

                entity.Property(e => e.IsSelect).HasColumnName("isSelect");

                entity.Property(e => e.IsUpdate).HasColumnName("isUpdate");

                entity.Property(e => e.ModulesId).HasColumnName("modulesId");

                entity.Property(e => e.RolesId).HasColumnName("rolesId");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasColumnName("updatedBy")
                    .HasMaxLength(256);

                entity.HasOne(d => d.Modules)
                    .WithMany(p => p.SubRoles)
                    .HasForeignKey(d => d.ModulesId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SubRoles_Modules");

                entity.HasOne(d => d.Roles)
                    .WithMany(p => p.SubRoles)
                    .HasForeignKey(d => d.RolesId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SubRoles_Roles");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(1024);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasMaxLength(256);

                entity.Property(e => e.DeleteAt)
                    .HasColumnName("deleteAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("deleteBy")
                    .HasMaxLength(256);

                entity.Property(e => e.Details)
                    .HasColumnName("details")
                    .HasMaxLength(2000);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(256);

                entity.Property(e => e.Extras).HasColumnName("extras");

                entity.Property(e => e.Flag).HasColumnName("flag");

                entity.Property(e => e.FullName)
                    .HasColumnName("fullName")
                    .HasMaxLength(256);

                entity.Property(e => e.Images)
                    .HasColumnName("images")
                    .HasMaxLength(256);

                entity.Property(e => e.LastChangePassword)
                    .HasColumnName("lastChangePassword")
                    .HasColumnType("datetime");

                entity.Property(e => e.LastLogin)
                    .HasColumnName("lastLogin")
                    .HasColumnType("datetime");

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasMaxLength(128);

                entity.Property(e => e.Orders).HasColumnName("orders");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(256);

                entity.Property(e => e.Roles).HasColumnName("roles");

                entity.Property(e => e.Salt)
                    .HasColumnName("salt")
                    .HasMaxLength(128);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedAt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedBy")
                    .HasMaxLength(256);

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(128);
            });
        }
    }
}