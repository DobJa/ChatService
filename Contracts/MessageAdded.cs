using ChatService.Model;

namespace Contracts
{
    public interface MessageAdded
    {
        public Message Message { get; }
    }
}