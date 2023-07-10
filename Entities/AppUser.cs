using System.ComponentModel.DataAnnotations;
using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string KnowAs { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        //Photos
        public List<Photo> Photos { get; set; } = new ();
        //Likes
        public List<UserLike> LikedByUsers { get; set; }
        public List<UserLike> LikedUsers { get; set; }
        //Messages
        public List<Message> MessageSent { get; set; }
        public List<Message> MessageReceived { get; set; }

        

    }
}