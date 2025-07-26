using Newtonsoft.Json;

namespace PopupCash.Account.Models.Socials.Kakaos
{
    public class KakaoUserResponse : KakaoKapiResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("connected_at")]
        public DateTime ConnectedAt { get; set; }

        [JsonProperty("kakao_account")]
        public KakaoAccount? KakaoAccount { get; set; }

        [JsonProperty("properties")]
        public Properties? Properties { get; set; }

        [JsonProperty("for_partner")]
        public ForPartner? ForPartner { get; set; }
    }


    public class KakaoProfile
    {
        [JsonProperty("nickname")]
        public string? Nickname { get; set; }

        [JsonProperty("thumbnail_image_url")]
        public string? ThumbnailImageUrl { get; set; }

        [JsonProperty("profile_image_url")]
        public string? ProfileImageUrl { get; set; }

        [JsonProperty("is_default_image")]
        public bool IsDefaultImage { get; set; }

        [JsonProperty("is_default_nickname")]
        public bool IsDefaultNickname { get; set; }
    }

    public class KakaoAccount
    {
        [JsonProperty("profile_nickname_needs_agreement")]
        public bool ProfileNicknameNeedsAgreement { get; set; }

        [JsonProperty("profile_image_needs_agreement")]
        public bool ProfileImageNeedsAgreement { get; set; }

        [JsonProperty("profile")]
        public KakaoProfile? Profile { get; set; }

        [JsonProperty("name_needs_agreement")]
        public bool NameNeedsAgreement { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("email_needs_agreement")]
        public bool EmailNeedsAgreement { get; set; }

        [JsonProperty("is_email_valid")]
        public bool IsEmailValid { get; set; }

        [JsonProperty("is_email_verified")]
        public bool IsEmailVerified { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("age_range_needs_agreement")]
        public bool AgeRangeNeedsAgreement { get; set; }

        [JsonProperty("age_range")]
        public string? AgeRange { get; set; }

        [JsonProperty("birthyear_needs_agreement")]
        public bool BirthyearNeedsAgreement { get; set; }

        [JsonProperty("birthyear")]
        public string? Birthyear { get; set; }

        [JsonProperty("birthday_needs_agreement")]
        public bool BirthdayNeedsAgreement { get; set; }

        [JsonProperty("birthday")]
        public string? Birthday { get; set; }

        [JsonProperty("birthday_type")]
        public string? BirthdayType { get; set; }

        [JsonProperty("gender_needs_agreement")]
        public bool GenderNeedsAgreement { get; set; }

        [JsonProperty("gender")]
        public string? Gender { get; set; }

        [JsonProperty("phone_number_needs_agreement")]
        public bool PhoneNumberNeedsAgreement { get; set; }

        [JsonProperty("phone_number")]
        public string? PhoneNumber { get; set; }

        [JsonProperty("ci_needs_agreement")]
        public bool CiNeedsAgreement { get; set; }

        [JsonProperty("ci")]
        public string? Ci { get; set; }

        [JsonProperty("ci_authenticated_at")]
        public DateTime? CiAuthenticatedAt { get; set; }
    }

    public class Properties
    {
        [JsonProperty("${CUSTOM_PROPERTY_KEY}")]
        public string? CustomPropertyValue { get; set; }
    }

    public class ForPartner
    {
        [JsonProperty("uuid")]
        public string? Uuid { get; set; }
    }

    //public class KakaoUser
    //{
    //    [JsonProperty("id")]
    //    public long Id { get; set; }
    //    [JsonProperty("connected_at")]
    //    public DateTime ConnectedAt { get; set; }
    //    [JsonProperty("properties")]
    //    public Properties? Properties { get; set; }
    //    [JsonProperty("kakao_account")]
    //    public KakaoAccount? KakaoAccount { get; set; }
    //}
    //public class Properties
    //{
    //    [JsonProperty("nickname")]
    //    public string NickName { get; set; }

    //    public Properties()
    //    {
    //        NickName = string.Empty;
    //    }
    //}
    //public class KakaoAccount
    //{
    //    [JsonProperty("profile_nickname_needs_agreement")]
    //    public bool NeedProfileNickName { get; set; }

    //    [JsonProperty("profile_image_needs_agreement")]
    //    public bool NeedProfileImage { get; set; }

    //    [JsonProperty("profile")]
    //    public Profile? Profile { get; set; }
    //    [JsonProperty("has_age_range")]
    //    public bool HasAgeRange { get; set; }
    //    [JsonProperty("age_range_needs_agreement")]
    //    public bool NeedAgeRange { get; set; }
    //    [JsonProperty("age_range")]
    //    public string? AgeRange { get; set; }
    //    [JsonProperty("has_birthday")]
    //    public bool HasBirthday { get; set; }
    //    [JsonProperty("birthday_needs_agreement")]
    //    public bool NeedBirthday { get; set; }
    //    [JsonProperty("birthday")]
    //    public string? Birthday { get; set; }
    //    [JsonProperty("birthday_type")]
    //    public string? BirthdayType { get; set; }
    //    [JsonProperty("has_gender")]
    //    public bool HasGender { get; set; }
    //    [JsonProperty("gender_needs_agreement")]
    //    public bool NeedGender { get; set; }
    //    [JsonProperty("gender")]
    //    public string? Gender { get; set; }
    //}

    //public class Profile
    //{
    //    [JsonProperty("nickname")]
    //    public string? NickName { get; set; }

    //}
}
