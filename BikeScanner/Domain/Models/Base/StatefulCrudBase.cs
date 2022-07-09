using System;

namespace BikeScanner.Domain.Models.Base
{
    public abstract class StatefulCrudBase : CrudBase
    {
        public string State { get; set; }

        public bool IsInState<TEnum>(TEnum state) where TEnum : Enum =>
            state.ToString().Equals(State, StringComparison.OrdinalIgnoreCase);

        public TEnum StateEnum<TEnum>() where TEnum : struct, Enum =>
            Enum.Parse<TEnum>(State);
    }

}

