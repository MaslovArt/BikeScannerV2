using System;

namespace BikeScanner.Domain.Models.Base
{
    public abstract class StatefulCrudBase : CrudBase
    {
        public string State { get; set; }

        public bool IsInState(Enum state) =>
            state.ToString().Equals(State, StringComparison.OrdinalIgnoreCase);

        public bool IsInState(string state) =>
            state.Equals(State, StringComparison.OrdinalIgnoreCase);

        public void SetState(string state) => State = state;

        public void SetState(Enum state) => State = state.ToString();
    }

}

