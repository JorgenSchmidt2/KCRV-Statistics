using KCRV_Statistics.Core.Interfaces;

namespace KCRV_Statistics.Model.MessageService.MessageClasses
{
    /// <summary>
    /// Класс-отправщик, отвечает за отправку сообщений на шину.
    /// </summary>
    public class MessageSender
    {
        private readonly IMessageService _messageService;

        public MessageSender(IMessageService messageBus)
        {
            _messageService = messageBus;
        }

        public void SendMessage(string Message)
        {
            // Отправка сообщения через шину
            _messageService.Publish(Message);
        }
    }
}