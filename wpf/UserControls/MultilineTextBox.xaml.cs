using System.Windows;
using System.Windows.Controls;
using SqlViewer.ViewModels;

namespace SqlViewer.UserControls
{
    /// <summary>
    /// Interaction logic for MultilineTextBox.xaml
    /// </summary>
    public partial class MultilineTextBox : UserControl
    {
        public static new readonly DependencyProperty HeightProperty
            = DependencyProperty.Register("Height", typeof(double), typeof(MultilineTextBox),
                new PropertyMetadata(0.0));
        
        public static new readonly DependencyProperty WidthProperty
            = DependencyProperty.Register("Width", typeof(double), typeof(MultilineTextBox),
                new PropertyMetadata(0.0));

        public static readonly DependencyProperty IsReadOnlyProperty
            = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(MultilineTextBox),
                new PropertyMetadata(false));

        public static readonly DependencyProperty TextProperty
            = DependencyProperty.Register("Text", typeof(string), typeof(MultilineTextBox),
                new PropertyMetadata(string.Empty));

        public static new readonly DependencyProperty FontSizeProperty
            = DependencyProperty.Register("FontSize", typeof(double), typeof(MultilineTextBox),
                new PropertyMetadata(12.0));

        public new double Height
        {
            get => (double)GetValue(HeightProperty);
            set 
            {
                SetValue(HeightProperty, value);
                tbMultiline.MinHeight = value; 
                tbMultiline.MaxHeight = value; 
                brdMultiline.Height = value; 
            }
        }

        public new double Width
        {
            get => (double)GetValue(WidthProperty);
            set 
            {
                SetValue(WidthProperty, value);
                tbMultiline.MinWidth = value; 
                tbMultiline.MaxWidth = value; 
                brdMultiline.Width = value; 
            }
        }
        
        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set 
            {
                SetValue(IsReadOnlyProperty, value);
                tbMultiline.IsReadOnly = value; 
            }
        }
        
        public string Text
        {
            get 
            {
                SetValue(TextProperty, tbMultiline.Text);
                return (string)GetValue(TextProperty);
            }
            set 
            {
                SetValue(TextProperty, value);
                tbMultiline.Text = value; 
            }
        }
        
        public new double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set 
            {
                SetValue(FontSizeProperty, value);
                tbMultiline.FontSize = value; 
            }
        }

        public MultilineTextBox()
        {
            InitializeComponent();

            Loaded += (o, e) => 
            {
                tbMultiline.MinHeight = this.Height; 
                tbMultiline.MaxHeight = this.Height; 
                brdMultiline.Height = this.Height; 

                tbMultiline.MinWidth = this.Width; 
                tbMultiline.MaxWidth = this.Width; 
                brdMultiline.Width = this.Width; 

                tbMultiline.IsReadOnly = this.IsReadOnly;
                tbMultiline.Text = this.Text;
                tbMultiline.FontSize = this.FontSize; 
            }; 
        }
    }
}
