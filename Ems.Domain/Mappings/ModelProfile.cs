using AutoMapper;
using Ems.Domain.Jobs;
using Ems.Models;

namespace Ems.Domain.Mappings;

public class ModelProfile : Profile
{
    public ModelProfile()
    {
        CreateMap<PublishClassVersionModel, PublishClassVersionJob>();
    }
}