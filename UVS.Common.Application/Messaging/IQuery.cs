using MediatR;
using UVS.Common.Domain;

namespace UVS.Common.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
