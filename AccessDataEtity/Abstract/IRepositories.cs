using TMShopsCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessDataEtity.Abstract
{
    public interface IBillRepository : IEntityBaseRepository<Bill> { }
    public interface ICustomersRepository : IEntityBaseRepository<Customers> { }
    public interface IGroupItemRepository : IEntityBaseRepository<GroupItem> { }
    public interface IGroupRepository : IEntityBaseRepository<Group> { }
    public interface IItemRepository : IEntityBaseRepository<Item> { }
    public interface IModulesRepository : IEntityBaseRepository<Modules> { }
    public interface IRolesRepository : IEntityBaseRepository<Roles> { }
    public interface ISettingsRepository : IEntityBaseRepository<Settings> { }
    public interface ISubBillRepository : IEntityBaseRepository<SubBill> { }
    public interface ISubItemRepository : IEntityBaseRepository<SubItem> { }
    public interface ISubRolesRepository : IEntityBaseRepository<SubRoles> { }
    public interface IUsersRepository : IEntityBaseRepository<Users> { }
}
