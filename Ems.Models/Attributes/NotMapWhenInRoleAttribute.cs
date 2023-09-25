﻿using Ems.Core.Entities.Enums;

namespace Ems.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class NotMapWhenInRoleAttribute : Attribute
{
    public NotMapWhenInRoleAttribute(Role role)
    {
        Role = role;
    }

    public Role Role { get; }
}