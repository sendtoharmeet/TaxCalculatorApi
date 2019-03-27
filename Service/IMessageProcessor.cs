using Service.Model;

namespace Service
{
    public interface IMessageProcessor
    {
        Invoice ProcessMessage(string inputMessage);

        bool ValidateMessage(string inputMessage);
    }
}
