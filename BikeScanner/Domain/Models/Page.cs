using System;
namespace BikeScanner.Domain.Models
{
    public class Page<T>
    {
        public T[] Items { get; set; }
        public int Total { get; set; }
        public int Offset { get; set; }
    }

}

