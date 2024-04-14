using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PopupCash.Controls.Buttons
{
    public class ImageButton : Button
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ImageButton), new PropertyMetadata(""));

        public static readonly DependencyProperty TextPaddingProperty =
            DependencyProperty.Register("TextPadding", typeof(Thickness), typeof(ImageButton), new PropertyMetadata(new Thickness(10, 0, 0, 0)));

        public static readonly DependencyProperty TextDecorationsProperty =
            DependencyProperty.Register("TextDecorations", typeof(TextDecorationCollection), typeof(ImageButton), new PropertyMetadata(null, OnTextDecorationsChanged));

        public static readonly DependencyProperty TextUnderlineOffsetProperty =
            DependencyProperty.Register("TextUnderlineOffset", typeof(double), typeof(ImageButton), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty ImageOrientationProperty =
            DependencyProperty.Register("ImageOrientation", typeof(Orientation), typeof(ImageButton), new PropertyMetadata(Orientation.Horizontal));

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        public static readonly DependencyProperty IsImageToolgeProperty =
            DependencyProperty.Register("IsImageToolge", typeof(bool), typeof(ImageButton), new PropertyMetadata(false));

        public static readonly DependencyProperty CheckedImageSourceProperty =
            DependencyProperty.Register("CheckedImageSource", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        public static readonly DependencyProperty MouseOverImageSourceProperty =
            DependencyProperty.Register("MouseOverImageSource", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(double), typeof(ImageButton), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register("ImageHeight", typeof(double), typeof(ImageButton), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ImageButton), new PropertyMetadata(new CornerRadius(0)));

        public static readonly DependencyProperty AllowMouseOverColorProperty =
            DependencyProperty.Register("AllowMouseOverColor", typeof(bool), typeof(ImageButton), new PropertyMetadata(false));

        public static readonly DependencyProperty MouseOverColorProperty =
            DependencyProperty.Register("MouseOverColor", typeof(Brush), typeof(ImageButton), new PropertyMetadata(Brushes.Transparent));

        /// <summary>
        /// 버튼 텍스트
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        /// <summary>
        /// 텍스트 Padding
        /// </summary>
        public Thickness TextPadding
        {
            get { return (Thickness)GetValue(TextPaddingProperty); }
            set { SetValue(TextPaddingProperty, value); }
        }

        /// <summary>
        /// 텍스트 Decoration
        /// </summary>
        public TextDecorationCollection TextDecorations
        {
            get { return (TextDecorationCollection)GetValue(TextDecorationsProperty); }
            set { SetValue(TextDecorationsProperty, value); }
        }

        /// <summary>
        /// 텍스트 Underline Offset
        /// </summary>
        public double TextUnderlineOffset
        {
            get { return (double)GetValue(TextUnderlineOffsetProperty); }
            set { SetValue(TextUnderlineOffsetProperty, value); }
        }

        /// <summary>
        /// 이미지 방향
        /// </summary>
        public Orientation ImageOrientation
        {
            get { return (Orientation)GetValue(ImageOrientationProperty); }
            set { SetValue(ImageOrientationProperty, value); }
        }


        /// <summary>
        /// 이미지
        /// </summary>
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        /// <summary>
        /// 토글 시 이미지를 변경여부
        /// CheckedImageSource 함께 사용
        /// </summary>
        public bool IsImageToolge
        {
            get { return (bool)GetValue(IsImageToolgeProperty); }
            set { SetValue(IsImageToolgeProperty, value); }
        }

        /// <summary>
        /// 토글됐을 대 이미지
        /// IsImageToolge 함께 사용
        /// </summary>
        public ImageSource CheckedImageSource
        {
            get { return (ImageSource)GetValue(CheckedImageSourceProperty); }
            set { SetValue(CheckedImageSourceProperty, value); }
        }

        /// <summary>
        /// 마우스 오버  이미지
        /// IsImageToolge 함께 사용
        /// </summary>
        public ImageSource MouseOverImageSource
        {
            get { return (ImageSource)GetValue(MouseOverImageSourceProperty); }
            set { SetValue(MouseOverImageSourceProperty, value); }
        }

        /// <summary>
        /// 이미지 넓이
        /// </summary>
        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        /// <summary>
        /// 이미지 높이
        /// </summary>
        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        /// <summary>
        /// 버튼의 Conrner radius
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// Mouse Over시 배경색 변경 여부
        /// MouseOverColor 함께 사용
        /// </summary>
        public bool AllowMouseOverColor
        {
            get { return (bool)GetValue(AllowMouseOverColorProperty); }
            set { SetValue(AllowMouseOverColorProperty, value); }
        }

        /// <summary>
        /// Mouse Over시 배경색 변경 여부
        /// AllowMouseOverColor 함께 사용
        /// </summary>
        public Brush MouseOverColor
        {
            get { return (Brush)GetValue(MouseOverColorProperty); }
            set { SetValue(MouseOverColorProperty, value); }
        }

        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));

        }

        // TextDecorations 속성 값이 변경될 때 호출되는 이벤트 핸들러
        private static void OnTextDecorationsChanged(DependencyObject dp, DependencyPropertyChangedEventArgs args)
        {
            if (dp is ImageButton button && args.NewValue is TextDecorationCollection decorations)
            {
                // TextDecorationCollection에 Underline이 포함되어 있는지 확인
                bool containsUnderline = false;
                foreach (var decoration in decorations)
                {
                    if (decoration.Location == TextDecorationLocation.Underline)
                    {
                        containsUnderline = true;
                        break;
                    }
                }

                // Underline이 포함되어 있다면 픽셀 단위로 위치를 조절
                if (containsUnderline)
                {
                    // 밑줄을 포함하는 TextDecoration 객체 생성
                    TextDecoration underline = new TextDecoration();
                    underline.Location = TextDecorationLocation.Underline;

                    // 픽셀 단위로 밑줄의 위치를 조절 (예: 2 픽셀 위로)
                    underline.PenOffset = button.TextUnderlineOffset;

                    // TextDecorationCollection에 추가
                    decorations.Add(underline);

                    // 속성 변경을 다시 적용
                    button.TextDecorations = decorations;
                }
            }
        }
    }
}
