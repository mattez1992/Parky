using AutoMapper;
using ParkyAPI.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.MapperProfiles
{
    public class TrailProfile : Profile
    {
        public TrailProfile()
        {
            CreateMap<Trail, ReadTrailDto>().ReverseMap();
            CreateMap<CreateTrailDto, Trail>();
            CreateMap<TrailUpdateDto, Trail>();
        }
    }
}
