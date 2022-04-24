using AutoMapper;
using ParkyAPI.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.MapperProfiles
{
    public class NationalParkProfile : Profile
    {
        public NationalParkProfile()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
        }
    }
}
