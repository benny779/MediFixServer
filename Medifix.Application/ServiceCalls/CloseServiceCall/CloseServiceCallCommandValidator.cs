﻿using FluentValidation;
using MediFix.Application.Extensions.Validation;

namespace MediFix.Application.ServiceCalls.CloseServiceCall;

public class CloseServiceCallCommandValidator : AbstractValidator<CloseServiceCallCommand>
{
    public CloseServiceCallCommandValidator()
    {
        RuleFor(x => x.CloseDetails)
            .NotEmpty();

        RuleFor(x => x.QrCode)
            .IsGuid();
    }
}
