using Domain.Models;

namespace Application.Abstractions;

public interface IRegularUrlRepository : IRepository<RegularUrl,Guid>
{
}