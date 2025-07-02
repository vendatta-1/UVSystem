using MediatR;
using UVS.Domain.Common;

namespace UVS.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
