using MediatR;

namespace Transferciniz.API.Notifications;

public class ParallelNotificationPublisher: INotificationPublisher
{
    public Task Publish(IEnumerable<NotificationHandlerExecutor> handlerExecutors, INotification notification, CancellationToken cancellationToken)
    {
        var tasks = handlerExecutors.Select(executor => executor.HandlerCallback(notification, cancellationToken));
        return Task.WhenAll(tasks);
    }
}