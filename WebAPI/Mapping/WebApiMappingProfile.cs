using AutoMapper;
using FindFi.CL.Application.Reviews.Commands;
using FindFi.CL.Domain.Entities;
using WebAPI.DTOs.Reviews;

namespace WebAPI.Mapping;

public sealed class WebApiMappingProfile : Profile
{
    public WebApiMappingProfile()
    {
        CreateMap<Review, ReviewDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id.ToString()))
            .ForMember(d => d.Title, o => o.MapFrom(s => s.Title == null ? null : s.Title.Value))
            .ForMember(d => d.Text, o => o.MapFrom(s => s.Text == null ? null : s.Text.Value))
            .ForMember(d => d.Rating, o => o.MapFrom(s => s.Stars.Value))
            .ForMember(d => d.Photos, o => o.MapFrom(s => s.Photos));

        CreateMap<CreateReviewRequest, CreateReviewCommand>();
    }
}
