using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace FSH.Core.Mediator;

public interface IQuery<out T> : IRequest<T> where T : notnull
{
}