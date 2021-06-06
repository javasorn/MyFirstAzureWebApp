using MyFirstAzureWebApp.Models;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Business
{
    public interface IFileManagerLogic
    {
        Task Upload(FileModel model);
        Task<byte[]> Get(string imageName);
        Task Delete(string imageName);
    }
    
}
