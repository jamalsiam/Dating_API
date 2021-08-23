using System.Threading.Tasks;
using Api.Repos;

namespace Api.Interface
{
    public interface IUnitOfWork
    {
        IUserRepo UserRepository {get; }
        IMessageRepo MessageRepository {get;}
         
        Task<bool> Complete();
        bool HasChanges();
    }
}