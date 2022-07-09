using System;
using System.ComponentModel;

namespace BikeScanner.Domain.States
{
	public enum UserStates
	{
        [Description("Активнен")] Active,
        [Description("Остановлен")] Stopped,
        [Description("Заблокирован")] Banned,
        [Description("Удален")] Deleted
    }
}

