using KCRV_Statistics.Core.Interfaces;

namespace KCRV_Statistics.Model.MessageService.MessageClasses
{
    /// <summary>
    /// Используемые в приложении поля, участвующие в 
    /// </summary>
    public class MessageObjects
    {
        private static IMessageService MessageService = new MessageService();

        public static MessageSender Sender = new MessageSender(MessageService);
        public static MessageReceiver Receiver = new MessageReceiver(MessageService);
    }
}