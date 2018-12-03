using System;
using System.Threading.Tasks;
using ThumbsApi.Models;

namespace ThumbsApi.Services.Interfaces
{
    internal interface IReportRepository
    {
        Task<Report> GetAsync(DateTime startDate, DateTime endDate, string product);
    }
}
