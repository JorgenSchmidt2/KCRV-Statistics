using KCRV_Statistics.Core.Interfaces;

namespace KCRV_Statistics.Model.MessageService
{
    /// <summary>
    /// Содержит логику работы при отправке сообщений на шину, либо подписке обновлений с неё.
    /// Реализует шаблон Publisher-Subscriber для межкомпонентного взаимодействия.
    /// </summary>
    public class MessageService : IMessageService
    {
        // Хранилище подписчиков, где:
        // Ключ - тип сообщения (Type)
        // Значение - список обработчиков (Action) для этого типа
        private readonly Dictionary<Type, List<object>> _subscribers = new Dictionary<Type, List<object>>();

        /// <summary>
        /// Публикует сообщение для всех подписанных обработчиков
        /// </summary>
        /// <typeparam name="T">Тип сообщения</typeparam>
        /// <param name="message">Экземпляр сообщения</param>
        public void Publish<T>(T message)
        {
            Type messageType = typeof(T);

            if (_subscribers.ContainsKey(messageType))
            {
                foreach (var subscriber in _subscribers[messageType])
                {
                    ((Action<T>)subscriber)(message);
                }
            }
        }

        /// <summary>
        /// Регистрирует обработчик для сообщений указанного типа
        /// </summary>
        /// <typeparam name="T">Тип сообщения для подписки</typeparam>
        /// <param name="handler">Обработчик сообщений</param>
        public void Subscribe<T>(Action<T> handler)
        {
            Type messageType = typeof(T);

            if (!_subscribers.ContainsKey(messageType))
            {
                _subscribers[messageType] = new List<object>();
            }
            _subscribers[messageType].Add(handler);
        }
    }
}