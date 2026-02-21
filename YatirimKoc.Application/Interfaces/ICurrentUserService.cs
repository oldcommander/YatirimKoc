using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YatirimKoc.Application.Interfaces
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        Guid AdminProfileId { get; }
    }
}
