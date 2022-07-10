using System;
using System.Threading.Tasks;
using BikeScanner.App.Models;

namespace BikeScanner.App.Interfaces
{
    /// <summary>
    /// Interface for content load services
    /// </summary>
    public interface ICrawler
    {
        /// <summary>
        /// Load items since specific date
        /// </summary>
        /// <param name="loadSince">Load content since date</param>
        /// <returns></returns>
        Task<ContentModel[]> Get(DateTime loadSince);
    }
}

