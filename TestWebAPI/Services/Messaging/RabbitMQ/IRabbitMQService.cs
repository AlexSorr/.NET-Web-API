namespace dNetAPI.Services.Messaging;

/// <summary>
/// Интерфейс для работы с RabbitMQ, предназначенный для отправки и получения сообщений о событиях.
/// </summary>
public interface IRabbitMQService {
    
    /// <summary>
    /// Инициализирует соединение с RabbitMQ и настраивает очередь для обмена сообщениями.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    Task InitializeAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Регистрация обработчика для обработки входящих сообщений из очереди.
    /// </summary>
    /// <param name="onMessageReceived">Функция, которая обрабатывает входящее сообщение и возвращает результат обработки.</param>
    void RegisterMessageReceiver(Func<string, Task<string>> onMessageReceived);

    /// <summary>
    /// Отправляет сообщение в очередь RabbitMQ.
    /// </summary>
    /// <param name="message">Сообщение для отправки в очередь.</param>
    Task SendMessageAsync(string message);

    /// <summary>
    /// Отправляет ответное сообщение после обработки входящего сообщения.
    /// </summary>
    /// <param name="responseMessage">Сообщение с результатом обработки.</param>
    Task SendResponseAsync(string responseMessage);

    /// <summary>
    /// Завершает работу с RabbitMQ, закрывая соединение и освобождая ресурсы.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    Task ShutdownAsync(CancellationToken cancellationToken);
}
