namespace PublisherSubscriberPattern.Api.Models
{
    /// <summary>
    /// Модель добавления значения для отправки подписчикам
    /// </summary>
    public class AddValueModel
    {
        /// <summary>
        /// Уникальный ключ
        /// </summary>
        public required string Key { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public required string Value { get; set; }
    }
}
