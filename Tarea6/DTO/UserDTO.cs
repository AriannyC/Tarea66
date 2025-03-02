namespace Tarea6.DTO
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string refreshtoken1 { get; set; }
        public DateTime TokenExpired { get; set; }

        public DateTime TokenCreated { get; set; }
    }
}
