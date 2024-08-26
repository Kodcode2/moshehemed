using MVCTest.Models;

namespace MVCTest.Services
{
    public interface ITargetService
    {
        Task<List<TargetModel>?> GetAllAgentsAsync();
    }
}
