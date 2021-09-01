public interface IObserver
{
    public void OnNotify(object value, NotificationType notificationType = NotificationType.Default);
}
