using KCRV_Statistics.Core.Interfaces;
using KCRV_Statistics.Model.MessageService.MessageBoxService;

namespace KCRV_Statistics.Model.MessageService.MessageClasses
{
    public class MessageReceiver
    {
        private readonly IMessageService _messageService;

        public MessageReceiver(IMessageService messageService)
        {
            _messageService = messageService;
            _messageService.Subscribe((string message) =>
            {
                GetMessageBox.Show(message);
            });
        }
    }
}