namespace Todo.Domain.Events.Handlers
{
    public class LoggingEventHandler :
                        INotificationHandler<CreatedTodoItemNotification>,
                        INotificationHandler<DeletedTodoItemNotification>,
                        INotificationHandler<MarkedAsDoneTodoItemNotification>,
                        INotificationHandler<UpdatedTodoItemNotification>
    {
        private readonly ILogger<LoggingEventHandler> _logger;

        public LoggingEventHandler(ILogger<LoggingEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(CreatedTodoItemNotification notification, CancellationToken cancellationToken)
        {
            await SerializeAndLog(notification, EMessageType.Created);
        }

        public async Task Handle(DeletedTodoItemNotification notification, CancellationToken cancellationToken)
        {
            await SerializeAndLog(notification, EMessageType.Deleted);
        }

        public async Task Handle(MarkedAsDoneTodoItemNotification notification, CancellationToken cancellationToken)
        {
            await SerializeAndLog(notification, EMessageType.Updated);
        }

        public async Task Handle(UpdatedTodoItemNotification notification, CancellationToken cancellationToken)
        {
            await SerializeAndLog(notification, EMessageType.Updated);
        }

        private Task SerializeAndLog(object data, EMessageType type)
        {
            var message = JsonManagerSerialize.Serialize(
                            new
                            {
                                Type = type.ToString(),
                                Data = data
                            });

            _logger.LogInformation($"LoggingEventHandler: {message}");

            return Task.CompletedTask;
        }
    }
}