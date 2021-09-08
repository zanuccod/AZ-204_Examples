using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public interface IBlobStorageService
    {
        Task<IEnumerable<string>> GetFilesFromDataContainerAsync();
        Task<string> GetFileDataByFileNameAsync(string fileName);
    }
}
