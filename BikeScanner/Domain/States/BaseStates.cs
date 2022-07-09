using System.ComponentModel;

namespace BikeScanner.Domain.States
{
    public enum BaseStates
	{
		[Description("Активный")] Active,
        [Description("Удален")] Deleted
	}
}

