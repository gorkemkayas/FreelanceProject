using AutoMapper;
using FreelanceProject.Data.Entities;
using FreelanceProject.Models.ViewModels;

namespace FreelanceProject.Data.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, ExtendedProfileViewModel>().ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Email));
            CreateMap<ExtendedProfileViewModel, AppUser>().ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress));

            CreateMap<AppUser, VisitorProfileViewModel>().ReverseMap();
            CreateMap<JobEntity, EditJobViewModel>().ReverseMap();
        }
    }
}
