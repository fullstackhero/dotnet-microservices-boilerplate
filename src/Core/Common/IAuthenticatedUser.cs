using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSH.Core.Common;

public interface IAuthenticatedUser
{
    string Id { get; }
    string Email { get; }
}
