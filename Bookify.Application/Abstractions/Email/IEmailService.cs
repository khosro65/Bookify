﻿using Bookify.Domain.Users;

namespace Bookify.Application.Abstractions.Email;

public interface IEmailService
{
    Task SendAsync(Domain.Users.Email email,string subject,string body);
}
