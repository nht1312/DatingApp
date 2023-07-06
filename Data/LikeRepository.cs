using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Data
{
    public class LikeRepository : ILikeRepository
    {
        
        public Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> GetUserWithLikes(int userId)
        {
            throw new NotImplementedException();
        }
    }
}