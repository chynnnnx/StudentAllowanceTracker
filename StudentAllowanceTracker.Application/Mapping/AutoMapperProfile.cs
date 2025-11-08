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
using StudentAllowanceTracker.Application.Commands.Expense;
using StudentAllowanceTracker.Application.Commands.Goals;

namespace StudentAllowanceTracker.Application.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterCommand, AppIdentityUser>();

            //Allowance Mappings
            CreateMap<CreateAllowanceCommand, Allowance>()
                .ForMember(dest => dest.AllowanceID, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<UpdateAllowanceCommand, Allowance>()
            .ForMember(dest => dest.AllowanceID, opt => opt.Ignore())  
            .ForMember(dest => dest.UserId, opt => opt.Ignore());       


            CreateMap<Allowance, AllowanceDTO>().ReverseMap();

            //Expense Mappings
            CreateMap<CreateExpenseCommand, ExpenseEntity>()
                .ForMember(dest => dest.ExpenseID, opt => opt.Ignore())
                .ForMember(dest => dest.UserID, opt => opt.Ignore());
            CreateMap<UpdateExpenseCommand, ExpenseEntity>()
                .ForMember(dest => dest.ExpenseID, opt => opt.Ignore())
                .ForMember(dest => dest.UserID, opt => opt.Ignore())
                .ForMember(dest => dest.AllowanceID, opt => opt.Ignore());

            CreateMap<ExpenseEntity, ExpenseDTO>().ReverseMap();

            // Goal Mappings
            CreateMap<CreateGoalCommand, GoalsEntity>()
            .ForMember(dest => dest.GoalID, opt => opt.Ignore())
            .ForMember(dest => dest.UserID, opt => opt.Ignore());

            CreateMap<UpdateGoalCommand, GoalsEntity>()
                .ForMember(dest => dest.GoalID, opt => opt.Ignore())
                .ForMember(dest => dest.UserID, opt => opt.Ignore());
            CreateMap<GoalsEntity, GoalsDTO>().ReverseMap();


        }
    }
}
