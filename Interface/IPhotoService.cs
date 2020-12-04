using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Api.Interface
{
    public interface IPhotoService
    { 
         Task<ImageUploadResult> AddPhoto(IFormFile file);
         Task<DeletionResult> DeletePhoto(string PublicId);
    }
}