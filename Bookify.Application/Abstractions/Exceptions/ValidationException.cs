﻿using Bookify.Application.Abstractions.Behaviors;

namespace Bookify.Application.Abstractions.Exceptions;

public sealed class ValidationException : Exception
{
    public IEnumerable<ValidationError> Errors { get; }
    public ValidationException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }
}
