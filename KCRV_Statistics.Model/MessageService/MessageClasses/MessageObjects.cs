using KCRV_Statistics.Core.Interfaces;

namespace KCRV_Statistics.Model.MessageService.MessageClasses
{
    /// <summary>
    /// Поля, содержащие экзампляры классов отправщика и подписчика
    /// </summary>
    public class MessageObjects
    {
        private static IMessageService MessageService = new MessageService();

        public static MessageSender Sender = new MessageSender(MessageService);
        public static MessageReceiver Receiver = new MessageReceiver(MessageService);
    }
}