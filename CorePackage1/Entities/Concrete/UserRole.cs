namespace CorePackage.Entities.Concrete
{
    public class UserRole : IEntity
    {
        public Guid UserRoleId { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}