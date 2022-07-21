using System;
using System.ComponentModel;

namespace BikeScanner.Domain.States
{
	public enum ContentStates
	{
        [Description("Активный")] Active,
        [Description("Удален")] Deleted,
        [Description("Архив")] Archive,
    }
}

