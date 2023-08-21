using Ems.Core.Entities;

namespace Ems.Models;

public interface ICurrentClassBinding
{
    Class? CurrentClass { get; set; }
}