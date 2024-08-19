namespace KCRV_Statistics.Core.Interfaces
{
    /// <summary>
    /// Соглашение по обменнику сообщениями в проектах, не связанных с UI
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Позволяет получать сообщения на шину обменника
        /// </summary>
        void Publish<T>(T message);
        /// <summary>
        /// Позволяет потребителям шины обменника подписываться на public-maker'а
        /// </summary>
        void Subscribe<T>(Action<T> handler);
    }
}