using System.ComponentModel;
using System.Security;
using CommunityToolkit.Mvvm.ComponentModel;
using PopupCash.Core.Models.Constants;
using PopupCash.Main.Extensions;

namespace PopupCash.Main.Models.Joins
{
    public partial class JoinInfo : ObservableObject
    {
        /// <summary>
        /// 이메일 주소 (아이디)
        /// </summary>
        [ObservableProperty]
        private string _email;

        /// <summary>
        /// 비밀번호
        /// </summary>
        [ObservableProperty]
        private SecureString _password;

        /// <summary>
        /// 비밀번호
        /// </summary>
        [ObservableProperty]
        private SecureString _passwordConfirm;

        /// <summary>
        /// 사용자 이름
        /// </summary>
        [ObservableProperty]
        private string _name;

        /// <summary>
        /// 핸드폰 번호
        /// </summary>
        [ObservableProperty]
        private string _phone;

        /// <summary>
        /// 추천인 코드
        /// </summary>
        [ObservableProperty]
        private string _recommender;

        /// <summary>
        /// 생년월일 (YYYYMMDD)
        /// </summary>
        public string? Birth { get; set; }

        /// <summary>
        /// 성별 (1: 남자, 2: 여자)
        /// </summary>
        public string? Sex { get; set; }

        /// <summary>
        /// 이용약관 체크
        /// </summary>
        [ObservableProperty]
        private bool _isCheckedPolicy;

        /// <summary>
        /// 개인정보방침 체크
        /// </summary>
        [ObservableProperty]
        private bool _isCheckedPrivacy;


        /// <summary>
        /// 나이 체크
        /// </summary>
        [ObservableProperty]
        private bool _isCheckedAge;

        [ObservableProperty]
        private bool _isIdentityVerification;


        /// <summary>
        /// 가입 유형(0: 힐링캐시, 1:구글, 2: 페이스북, 3: 카카오, 4: 네이버)
        /// </summary>        
        [ObservableProperty]
        private string _loginType;


        /// <summary>
        /// Join 조건 완료 여부
        /// </summary>
        [ObservableProperty]
        private bool _isAbleJoin;

        /// <summary>
        /// Join 조건 완료 여부
        /// </summary>
        [ObservableProperty]
        private bool _isAbleEmail;

        /// <summary>
        /// Join 조건 완료 여부
        /// </summary>
        [ObservableProperty]
        private bool _isAblePassword;

        /// <summary>
        /// Join 조건 완료 여부
        /// </summary>
        [ObservableProperty]
        private bool _isAbleChecked;

        public JoinInfo()
        {
            Email = string.Empty;
            Password = new SecureString();
            PasswordConfirm = new SecureString();
            Name = string.Empty;
            Phone = string.Empty;
            Recommender = string.Empty;

            LoginType = ConstantString.AppType;

            IsCheckedPolicy = false;
            IsCheckedPrivacy = false;
            IsCheckedAge = false;
        }

        public bool ValidatePassword()
        {
            return Password.Length > 0 && Password.ToUnsecuredString() == PasswordConfirm.ToUnsecuredString();
        }
        public bool ValidateEmail()
        {
            // '@' 기호가 한 번만 등장하는지 확인
            int atIndex = Email.IndexOf('@');
            if (atIndex == -1 || Email.LastIndexOf('@') != atIndex)
            {
                return false;
            }

            // '@' 기호 뒤에 '.'가 존재하는지 확인
            int dotIndex = Email.LastIndexOf('.');
            if (dotIndex == -1 || dotIndex <= atIndex)
            {
                return false;
            }

            return true;
        }

        public bool ValidateChecked()
        {
            return IsCheckedPolicy && IsCheckedPrivacy && IsCheckedAge;
        }


        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName == nameof(Email))
            {
                IsAbleEmail = ValidateEmail();
            }

            if (e.PropertyName == nameof(Password) ||
                e.PropertyName == nameof(PasswordConfirm))
            {
                IsAblePassword = ValidatePassword();
            }
            if (e.PropertyName == nameof(IsCheckedPolicy) ||
                e.PropertyName == nameof(IsCheckedPrivacy) ||
                e.PropertyName == nameof(IsCheckedAge))
            {
                IsAbleChecked = ValidateChecked();
            }
            if (e.PropertyName == nameof(IsAbleEmail) ||
                e.PropertyName == nameof(IsAblePassword) ||
                e.PropertyName == nameof(IsAbleChecked) ||
                e.PropertyName == nameof(IsIdentityVerification))
            {
                if (LoginType is not null && LoginType.Equals(ConstantString.AppType))
                {
                    IsAbleJoin = IsAbleEmail && IsAblePassword && IsAbleChecked && IsIdentityVerification;
                }
                else
                {
                    IsAbleJoin = IsAbleEmail && IsAbleChecked && IsIdentityVerification;
                }
            }
        }
    }
}
