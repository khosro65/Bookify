using Bookify.Domain.Abstractions;
using MediatR;

namespace Bookify.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>, IBaseCommand { }


public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand { }

/// <summary>
/// for implementing cross cutting concerns in the pipeline
/// </summary>
public interface IBaseCommand { }