using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Application.Commands.Auth;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Application.Commands.Allowances;
using StudentAllowanceTracker.Application.DTOs;

namespace StudentAllowanceTracker.Application.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterCommand, AppIdentityUser>();
              
            CreateMap<CreateAllowanceCommand, Allowance>()
                .ForMember(dest =>  dest.AllowanceID, opt  =>opt.MapFrom(_=> Guid.NewGuid()))
                .ForMember(dest =>dest.UserId, opt => opt.Ignore());

            CreateMap<Allowance, AllowanceDTO>().ReverseMap();

        }
    }
}
