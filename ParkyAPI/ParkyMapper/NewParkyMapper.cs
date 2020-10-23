using AutoMapper;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.ParkyMapper
{
    public class NewParkyMapper : Profile
    {
		public NewParkyMapper()
		{
			CreateMap<NationalPark, NationalParkDto>().ReverseMap();
			CreateMap<Trail, TrailDto>().ReverseMap();
			CreateMap<TrailCreateDto, Trail>().ReverseMap();
			CreateMap<TrailUpdateDto, Trail>().ReverseMap();
		}
    }
}
