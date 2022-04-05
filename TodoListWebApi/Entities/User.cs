using System.ComponentModel.DataAnnotations;

namespace TodoListWebApi.Entities
{
    public class User
    {
        [Key]
        public uint Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public RoleTypes Role { get; set; } = RoleTypes.Standard;
    }
}
