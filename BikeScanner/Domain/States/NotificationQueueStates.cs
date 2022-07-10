using System.ComponentModel;

namespace BikeScanner.Domain.States
{
    public enum NotificationQueueStates
	{
        [Description("Активный")] Active,
        [Description("Удален")] Deleted,
        [Description("Запланирован")] Scheduled,
        [Description("Отправлен")] Sended,
        [Description("Ошибка отправки")] Error
    }
}

