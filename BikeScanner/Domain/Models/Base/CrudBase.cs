using System;

namespace BikeScanner.Domain.Models.Base
{
    public abstract class CrudBase : EntityBase
    {
        public DateTime CreateDate { get; protected set; }
        public DateTime? UpdateDate { get; protected set; }

        public void MarkCreated() => CreateDate = DateTime.Now;

        public void MarkUpdated() => UpdateDate = DateTime.Now;
    }

}

