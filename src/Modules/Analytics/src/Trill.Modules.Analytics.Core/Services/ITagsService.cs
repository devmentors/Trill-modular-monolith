using System.Threading.Tasks;
using Trill.Modules.Analytics.Core.Models;

namespace Trill.Modules.Analytics.Core.Services
{
    internal interface ITagsService
    {
        Task<bool> TryAddAsync(Tag tag);
        Task IncrementOccurrencesCountAsync(string tag);
    }
}