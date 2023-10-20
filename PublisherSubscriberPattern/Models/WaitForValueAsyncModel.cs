namespace PublisherSubscriberPattern.Models
{
    /// <summary>
    /// Модель подписки на получение значения
    /// </summary>
    public class WaitForValueAsyncModel
    {
        /// <summary>
        /// Уникальный ключ
        /// </summary>
        public required string Key { get; set; }

        /// <summary>
        /// Время ожидания в ms
        /// </summary>
        public required int MillisecondsWait { get; set; }
    }
}
