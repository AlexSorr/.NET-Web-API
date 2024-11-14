namespace API.Services.Messaging;

/// <summary>
/// Интерфейс для работы с RabbitMQ, предназначенный для отправки и получения сообщений.
/// </summary>
public interface IRabbitMQService {
    /// <summary>
    /// Инициализирует соединение с RabbitMQ и настраивает очередь.
    /// </summary>
    Task InitializeAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Метод для обработки сообщений, полученных из очереди RabbitMQ.
    /// </summary>
    /// <param name="message">Сообщение для обработки.</param>
    void ProcessMessage(string message);

    /// <summary>
    /// Отправка сообщения в очередь RabbitMQ.
    /// </summary>
    /// <param name="message">Сообщение для отправки в очередь.</param>
    Task SendMessageAsync(string message);

    /// <summary>
    /// Метод для завершения работы с RabbitMQ и освобождения ресурсов.
    /// </summary>
    Task ShutdownAsync(CancellationToken cancellationToken);
}
