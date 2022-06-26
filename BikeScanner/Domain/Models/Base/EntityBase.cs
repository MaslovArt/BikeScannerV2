using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeScanner.Domain.Models.Base
{
    public abstract class EntityBase
    {
        public int Id { get; protected set; }
    }

}

