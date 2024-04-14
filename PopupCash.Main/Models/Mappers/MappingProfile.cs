using AutoMapper;
using PopupCash.Account.Models.Authenthications.Impl;
using PopupCash.Account.Models.Authenthications.Kakao.Impl;
using PopupCash.Account.Models.Login.Impl;
using PopupCash.Account.Models.Users.Impl;
using PopupCash.Contents.Models.Cashs;
using PopupCash.Database.Models.Users;
using PopupCash.Main.Extensions;
using PopupCash.Main.Models.Joins;
using PopupCash.Main.Models.Users;
using PopupCash.Main.Models.Verifications;

namespace PopupCash.Main.Models.Mappers
{
    /// <summary>
    /// DTO, VO 같은 변환이 필요할 때 등록
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<KakaoTokenResponse, AuthTokenInfo>();

            CreateMap<LoginResponse, Authorization>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token));

            CreateMap<JoinResponse, Authorization>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token));

            CreateMap<UserResponse, User>()
            .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.Profile))
            .ForMember(dest => dest.MissionPoint, opt => opt.MapFrom(src => src.Mission_point));

            CreateMap<UserResponse, CurrentUser>()
            .ForMember(dest => dest.MissionPoint, opt => opt.MapFrom(src => src.Mission_point));

            CreateMap<User, CurrentUser>();

            CreateMap<CashSaveHistoryResponse, CashHistory>()
            .ForMember(dest => dest.Cash, opt => opt.MapFrom(src => src.Save == null ? new List<CashData>() : src.Save.Cash));


            CreateMap<JoinInfo, JoinRequest>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password.ToUnsecuredString()));

            CreateMap<ResponseIdentityVerification, IdentityVerificationResponse>();

            CreateMap<IdentityVerificationResponse, JoinInfo>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RSLT_NAME))
                .ForMember(dest => dest.Birth, opt => opt.MapFrom(src => src.RSLT_BIRTHDAY))
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.RSLT_SEX_CD))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.TEL_NO));

        }
    }
}
