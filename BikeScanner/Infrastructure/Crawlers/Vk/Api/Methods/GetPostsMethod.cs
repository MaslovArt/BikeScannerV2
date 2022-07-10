using System;
using BikeScanner.Сonfigs;

namespace BikeScanner.Infrastructure.Crawlers.Vk.Api.Methods
{
    /// <summary>
    /// Get user/group posts from wall
    /// </summary>
    internal class GetPostsMethod : VkApiMethod
    {
        private int _offset;
        private int _count;

        public GetPostsMethod(VkAccessConfig settings)
            : base(settings)
        { }

        public override string Method => "wall.get";

        public override string Params => $"owner_id={OwnerId}&offset={Offset}&count={Count}";

        public int OwnerId { get; set; }

        public int Offset
        {
            get => _offset;
            set
            {
                if (value < 0)
                    throw new ArgumentException($"{nameof(Offset)} must be more than 0");

                _offset = value;
            }
        }

        public int Count
        {
            get => _count;
            set
            {
                if (value < 1 || value > 100)
                    throw new ArgumentException($"{nameof(Count)} must be in range (1, 100)");

                _count = value;
            }
        }
    }
}

