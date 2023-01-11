namespace CorePackage.Entities.Concrete
{
    public class Role : IEntity
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
    }
}