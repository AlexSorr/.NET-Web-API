using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace dNetAPI.Services.Messaging;

/// <summary>
/// Сервис для работы с RabbitMQ для отправки и получения сообщений о создании событий.
/// </summary>
public class RabbitMQService : BackgroundService, IRabbitMQService, IDisposable {

    private readonly IConnection _connection; // Соединение с RabbitMQ
    private readonly IModel _channel; // Канал для обмена сообщениями
    private readonly ILogger<RabbitMQService> _logger; // Логгер для событий
    private readonly string _queueName; // Имя очереди для обмена сообщениями
    private Func<string, Task<string>>? _onMessageReceived; // Обработчик входящих сообщений (может быть null)

    /// <summary>
    /// Инициализирует новый экземпляр RabbitMQService, создавая соединение и канал.
    /// </summary>
    /// <param name="configuration">Конфигурация для подключения к RabbitMQ.</param>
    /// <param name="logger">Логгер для событий RabbitMQ.</param>
    public RabbitMQService(IConfiguration configuration, ILogger<RabbitMQService> logger) {
        _logger = logger;

        var factory = new ConnectionFactory {
            HostName = configuration["RabbitMQ:HostName"], // Указание хоста RabbitMQ
            UserName = configuration["RabbitMQ:UserName"], // Указание имени пользователя
            Password = configuration["RabbitMQ:Password"]  // Указание пароля
        };

        _connection = factory.CreateConnection(); // Создание соединения с RabbitMQ
        _channel = _connection.CreateModel(); // Создание канала для обмена сообщениями
        _queueName = configuration["RabbitMQ:QueueName"] ?? "defaultQueueName"; // Получение имени очереди или дефолтное значение

        // Декларация очереди
        _channel.QueueDeclare(queue: _queueName,
                              durable: true, 
                              exclusive: false, 
                              autoDelete: false, 
                              arguments: null);
    }

    /// <inheritdoc/>
    public async Task InitializeAsync(CancellationToken cancellationToken) {
        // Дополнительная логика инициализации, если требуется
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public void RegisterMessageReceiver(Func<string, Task<string>> onMessageReceived) {
        _onMessageReceived = onMessageReceived ?? throw new ArgumentNullException(nameof(onMessageReceived), "Message handler cannot be null.");
    }

    /// <inheritdoc/>
    public async Task SendMessageAsync(string message) {
        var messageBody = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: messageBody);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task SendResponseAsync(string responseMessage) {
        var messageBody = Encoding.UTF8.GetBytes(responseMessage);
        _channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: messageBody);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (sender, args) => {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Received message: {message}");

            if (_onMessageReceived != null) {
                var response = await _onMessageReceived(message);
                await SendResponseAsync(response);
            }

            _channel.BasicAck(args.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task ShutdownAsync(CancellationToken cancellationToken) {
        _channel.Close();
        _connection.Close();
        await base.StopAsync(cancellationToken);
    }

    /// <summary>
    /// Освобождает ресурсы, закрывая соединение и канал RabbitMQ.
    /// </summary>
    public override void Dispose() {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}
