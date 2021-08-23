using System.Threading.Tasks;
using Api.Context;
using Api.Interface;
using Api.Repos;
using AutoMapper;

namespace Api.Service
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMapper _mapper;
        private readonly DBContext _context;
        public UnitOfWork(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IUserRepo UserRepository => new UserRepo(_context, _mapper);

        public IMessageRepo MessageRepository => new MessageRepo(_context, _mapper);



        public async Task<bool> Complete() 
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            _context.ChangeTracker.DetectChanges();
            var changes = _context.ChangeTracker.HasChanges();

            return changes;
        }
    }
}