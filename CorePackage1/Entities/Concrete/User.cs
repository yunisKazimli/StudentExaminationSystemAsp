namespace CorePackage.Entities.Concrete
{
    public class User : IEntity
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Guid UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
    }
}