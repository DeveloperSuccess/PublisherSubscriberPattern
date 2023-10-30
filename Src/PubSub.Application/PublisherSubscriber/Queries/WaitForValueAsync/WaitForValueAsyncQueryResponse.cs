namespace PubSub.Application.PublisherSubscriber.Queries.WaitForValueAsync
{
    public class WaitForValueAsyncQueryResponse
    {
        /// <summary>
        /// Полученное значение
        /// </summary>
        public required string Value { get; set; }

        /// <summary>
        /// Флаг успешного получения значения
        /// </summary>
        public required bool Success { get; set; }

        /// <summary>
        /// Текст сообщения об ошибке
        /// </summary>
        public required string ErrorMessage { get; set; }
    }
}
