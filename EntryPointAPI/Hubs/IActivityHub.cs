using System.Threading.Tasks;

namespace EntryPointAPI.Hubs
{
    public interface IActivityHub
    {
        Task SendMessage(string message);
    }
}