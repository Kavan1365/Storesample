using BaseCore.Configuration;
using BaseCore.Core;
using Core.Repositories.Base;

namespace Api.Configuration
{
    public class BaseDTOController<T> : BaseController where T : BaseEntity
    {
        public readonly IRepository<T> Repository;
        public BaseDTOController(IRepository<T> repository)
        {
            Repository = repository;
        }

    }
}
