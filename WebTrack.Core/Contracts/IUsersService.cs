using WebTrack.Data.Entities;

namespace WebTrack.Core.Contracts
{
    public interface IUsersService
    {
        Task<List<User>> GetAllVisitors();
    }
}
