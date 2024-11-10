//dotnet add package RabbitMQ.Client --version 6.2.1
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace API.Services;

/// <summary>
/// Сервис для работы с RabbitMQ для отправки и получения данных о бронированиях.
/// </summary>
public abstract class RabbitMQService : BackgroundService, IRabbitMQService, IDisposable {

    private readonly IConnection _connection; // Соединение с RabbitMQ
    private readonly IModel _channel; // Канал для обмена сообщениями
    private readonly ILogger<RabbitMQService> _logger; // Логгер для событий
    private readonly string _queueName = string.Empty; // Имя очереди для обмена сообщениями

    /// <summary>
    /// Инициализирует новый экземпляр сервиса RabbitMQ.
    /// Создает соединение и канал, а также настраивает очередь.
    /// </summary>
    /// <param name="configuration">Конфигурация для подключения к RabbitMQ.</param>
    /// <param name="logger">Логгер для событий RabbitMQ.</param>
    public RabbitMQService(IConfiguration configuration, ILogger<RabbitMQService> logger) {
        _logger = logger;

        var factory = new ConnectionFactory() {
            HostName = configuration["RabbitMQ:HostName"], // Указание хоста RabbitMQ
            UserName = configuration["RabbitMQ:UserName"], // Указание имени пользователя
            Password = configuration["RabbitMQ:Password"]  // Указание пароля
        };

        _connection = factory.CreateConnection(); // Создание соединения с RabbitMQ
        _channel = _connection.CreateModel(); // Создание канала для обмена сообщениями
        _queueName = configuration["RabbitMQ:QueueName"]; // Получение имени очереди из конфигурации

        // Декларируем очередь (создаем, если не существует)
        _channel.QueueDeclare(queue: _queueName,
                                durable: true, // Очередь будет сохраняться после перезапуска RabbitMQ
                                exclusive: false, // Очередь не будет уникальной для соединения
                                autoDelete: false, // Очередь не будет автоматически удалена
                                arguments: null); // Без дополнительных аргументов
    }

    /// <summary>
    /// Инициализирует соединение с RabbitMQ и настраивает очередь.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task InitializeAsync(CancellationToken cancellationToken) {
        // Логика инициализации, если нужно
        await Task.CompletedTask;
    }

    /// <summary>
    /// Обрабатывает полученное сообщение.
    /// </summary>
    /// <param name="message">Сообщение для обработки.</param>
    public void ProcessMessage(string message) {
        _logger.LogInformation($"Processing booking message: {message}");
        // Логика обработки сообщения
    }

    /// <summary>
    /// Отправка сообщения в очередь RabbitMQ.
    /// </summary>
    /// <param name="message">Сообщение для отправки в очередь.</param>
    public async Task SendMessageAsync(string message){
        var messageBody = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: messageBody);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Завершающий метод для остановки фона и закрытия соединений.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task ShutdownAsync(CancellationToken cancellationToken) {
        _channel.Close();
        _connection.Close();
        await base.StopAsync(cancellationToken);
    }

    /// <summary>
    /// Запускает фоновую задачу для прослушивания очереди RabbitMQ и обработки сообщений.
    /// </summary>
    /// <param name="stoppingToken">Токен для отмены операции.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        // Устанавливаем слушатель на очередь
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (sender, args) =>
            {
                byte[] body = args.Body.ToArray();
                string message = Encoding.UTF8.GetString(body); // Преобразуем байты в строку
                _logger.LogInformation($"Received message: {message}");
                ProcessMessage(message); // Обработка полученного сообщения

                _channel.BasicAck(args.DeliveryTag, false); // Подтверждаем обработку сообщения
            };

        // Начинаем слушать очередь
        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

        // Ожидаем завершения работы службы
        await Task.CompletedTask;
    }

    /// <summary>
    /// Освобождает ресурсы, закрывая соединение и канал RabbitMQ.
    /// </summary>
    public override void Dispose() {
        _channel?.Close(); // Закрываем канал
        _connection?.Close(); // Закрываем соединение
        base.Dispose(); // Завершаем освобождение ресурсов
    }
    
}