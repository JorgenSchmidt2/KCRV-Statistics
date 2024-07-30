using KCRV_Statistics.Core.Interfaces;

namespace KCRV_Statistics.Model.MessageService.MessageClasses
{
    public class MessageObjects
    {
        private static IMessageService MessageBus = new MessageService();

        public static MessageSender Sender = new MessageSender(MessageBus);
        public static MessageReceiver Receiver = new MessageReceiver(MessageBus);
    }
}