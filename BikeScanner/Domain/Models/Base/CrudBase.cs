using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeScanner.Domain.Models.Base
{
    public abstract class CrudBase : EntityBase
    {
        public DateTime CreateDate { get; protected set; }
        public DateTime? UpdateDate { get; protected set; }
    }

}

