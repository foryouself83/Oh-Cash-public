using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PopupCash.Controls.Buttons
{
    public class AccumulateItemButton : Button
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(AccumulateItemButton), new PropertyMetadata(""));

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(AccumulateItemButton), new PropertyMetadata(""));

        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register("CurrentValue", typeof(string), typeof(AccumulateItemButton), new PropertyMetadata(""));

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(string), typeof(AccumulateItemButton), new PropertyMetadata(""));

        public static readonly DependencyProperty ItemImageSourceProperty =
            DependencyProperty.Register("ItemImageSource", typeof(ImageSource), typeof(AccumulateItemButton), new PropertyMetadata(null));

        public static readonly DependencyProperty ItemImageWidthProperty =
            DependencyProperty.Register("ItemImageWidth", typeof(double), typeof(AccumulateItemButton), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty ItemImageHeightProperty =
            DependencyProperty.Register("ItemImageHeight", typeof(double), typeof(AccumulateItemButton), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty AccumulateImageSourceProperty =
         DependencyProperty.Register("AccumulateImageSource", typeof(ImageSource), typeof(AccumulateItemButton), new PropertyMetadata(null));

        public static readonly DependencyProperty AccumulateImageWidthProperty =
            DependencyProperty.Register("AccumulateImageWidth", typeof(double), typeof(AccumulateItemButton), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty AccumulateImageHeightProperty =
            DependencyProperty.Register("AccumulateImageHeight", typeof(double), typeof(AccumulateItemButton), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(AccumulateItemButton), new PropertyMetadata(new CornerRadius(0)));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public string CurrentValue
        {
            get { return (string)GetValue(CurrentValueProperty); }
            set { SetValue(CurrentValueProperty, value); }
        }

        public string MaxValue
        {
            get { return (string)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        #region Item Image

        public ImageSource ItemImageSource
        {
            get { return (ImageSource)GetValue(ItemImageSourceProperty); }
            set { SetValue(ItemImageSourceProperty, value); }
        }

        public double ItemImageWidth
        {
            get { return (double)GetValue(ItemImageWidthProperty); }
            set { SetValue(ItemImageWidthProperty, value); }
        }

        public double ItemImageHeight
        {
            get { return (double)GetValue(ItemImageHeightProperty); }
            set { SetValue(ItemImageHeightProperty, value); }
        }
        #endregion


        #region Accumulate Image

        public ImageSource AccumulateImageSource
        {
            get { return (ImageSource)GetValue(AccumulateImageSourceProperty); }
            set { SetValue(AccumulateImageSourceProperty, value); }
        }


        public double AccumulateImageWidth
        {
            get { return (double)GetValue(AccumulateImageWidthProperty); }
            set { SetValue(AccumulateImageWidthProperty, value); }
        }

        public double AccumulateImageHeight
        {
            get { return (double)GetValue(AccumulateImageHeightProperty); }
            set { SetValue(AccumulateImageHeightProperty, value); }
        }
        #endregion

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        static AccumulateItemButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AccumulateItemButton), new FrameworkPropertyMetadata(typeof(AccumulateItemButton)));

        }
        public AccumulateItemButton()
        {
        }

        public override void OnApplyTemplate()
        {

            if (GetTemplateChild("PART_TitleText") is not TextBlock titleText) return;
            if (GetTemplateChild("PART_DescriptionText") is not TextBlock descriptionText) return;
            if (GetTemplateChild("PART_ValueText") is not TextBlock valueText) return;

            descriptionText.FontSize = titleText.FontSize - 2;
            valueText.FontSize = descriptionText.FontSize - 2;
            base.OnApplyTemplate();
        }
    }
}
