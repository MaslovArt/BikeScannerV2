using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeScanner.Domain.Models.Base
{
    public abstract class StatefulCrudBase : CrudBase
    {
        public string State { get; set; }
    }

}

