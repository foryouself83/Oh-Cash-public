using System.Text.Json.Serialization;

namespace PopupCash.Account.Models.Socials.Google
{
    public class GoogleUserResponse
    {
        [JsonPropertyName("sub")]
        public string? Sub { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("given_name")]
        public string? GivenName { get; set; }
        [JsonPropertyName("family_name")]
        public string? FamilyName { get; set; }

        [JsonPropertyName("picture")]
        public string? Picture { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [JsonPropertyName("email_verified")]
        public bool IsVerified { get; set; }

        [JsonPropertyName("locale")]
        public string? Locale { get; set; }
    }
    public class Properties
    {
        [JsonPropertyName("nickname")]
        public string? NickName { get; set; }
    }
    public class GoogleAccount
    {
        [JsonPropertyName("profile_nickname_needs_agreement")]
        public bool NeedNickName { get; set; }
        [JsonPropertyName("properties")]
        public Properties? Properties { get; set; }
        [JsonPropertyName("has_age_range")]
        public bool HasAgeRange { get; set; }
        [JsonPropertyName("age_range_needs_agreement")]
        public bool NeedAgeRange { get; set; }
        [JsonPropertyName("age_range")]
        public string? AgeRange { get; set; }
        [JsonPropertyName("has_birthday")]
        public bool HasBirthday { get; set; }
        [JsonPropertyName("birthday_needs_agreement")]
        public bool NeedBirthday { get; set; }
        [JsonPropertyName("birthday")]
        public string? Birthday { get; set; }
        [JsonPropertyName("birthday_type")]
        public string? BirthdayType { get; set; }
        [JsonPropertyName("has_gender")]
        public bool HasGender { get; set; }
        [JsonPropertyName("gender_needs_agreement")]
        public bool NeedGender { get; set; }
        [JsonPropertyName("gender")]
        public string? Gender { get; set; }

    }
}
