using WebTrack.Data.Entities;

namespace WebTrack.Core.Contracts
{
    public interface IUsersService
    {
        List<User> GetAllVisitors();
    }
}
