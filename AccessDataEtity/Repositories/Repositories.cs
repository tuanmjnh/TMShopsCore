using AccessDataEtity.Abstract;
using TMShopsCore.Models;

namespace AccessDataEtity.Repositories
{
    public class BillRepository : EntityBaseRepository<Bill>, IBillRepository
    {
        public BillRepository(TMShopsContext context) : base(context) { }
    }
    public class CustomersRepository : EntityBaseRepository<Customers>, ICustomersRepository
    {
        public CustomersRepository(TMShopsContext context) : base(context) { }
    }
    public class GroupItemRepository : EntityBaseRepository<GroupItem>, IGroupItemRepository
    {
        public GroupItemRepository(TMShopsContext context) : base(context) { }
    }
    public class GroupRepository : EntityBaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(TMShopsContext context) : base(context) { }
    }
    public class ItemRepository : EntityBaseRepository<Item>, IItemRepository
    {
        public ItemRepository(TMShopsContext context) : base(context) { }
    }
    public class ModulesRepository : EntityBaseRepository<Modules>, IModulesRepository
    {
        public ModulesRepository(TMShopsContext context) : base(context) { }
    }
    public class RolesRepository : EntityBaseRepository<Roles>, IRolesRepository
    {
        public RolesRepository(TMShopsContext context) : base(context) { }
    }
    public class SettingsRepository : EntityBaseRepository<Settings>, ISettingsRepository
    {
        public SettingsRepository(TMShopsContext context) : base(context) { }
    }
    public class SubBillRepository : EntityBaseRepository<SubBill>, ISubBillRepository
    {
        public SubBillRepository(TMShopsContext context) : base(context) { }
    }
    public class SubItemRepository : EntityBaseRepository<SubItem>, ISubItemRepository
    {
        public SubItemRepository(TMShopsContext context) : base(context) { }
    }
    public class SubRolesRepository : EntityBaseRepository<SubRoles>, ISubRolesRepository
    {
        public SubRolesRepository(TMShopsContext context) : base(context) { }
    }
    public class UsersRepository : EntityBaseRepository<Users>, IUsersRepository
    {
        public UsersRepository(TMShopsContext context) : base(context) { }
    }
}
