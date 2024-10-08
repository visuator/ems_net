﻿using Ems.Core.Entities.Enums;
using FluentValidation;

namespace Ems.Domain.Models;

public class CreateSettingModel
{
    public Quarter CurrentQuarter { get; set; }

    public class Validator : AbstractValidator<CreateSettingModel>
    {
    }
}