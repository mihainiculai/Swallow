using AutoMapper;
using Swallow.DTOs.Authentication;
using Swallow.Models;

namespace Swallow.Mappings
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .Build();

            CreateMap<User, UserDto>()
                .ForMember(dto => dto.Username, opt => opt.MapFrom(src => src.PublicUsername))
                .ForMember(dto => dto.ProfilePictureUrl, opt => opt.MapFrom((src, dto, _, context) =>
                {
                    if (!src.HasCustomProfilePicture && src.GoogleProfilePictureUrl != null)
                    {
                        return src.GoogleProfilePictureUrl;
                    }

                    if (src.HasCustomProfilePicture && src.ProfilePicturePath != null)
                    {
                        string? serverUrl = configuration["ServerURL"];
                        return $"{serverUrl}/files/profile-picture?photoId={src.ProfilePicturePath}";
                    }

                    return null;
                }));
        }
    }
}
