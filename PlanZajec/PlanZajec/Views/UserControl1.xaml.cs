using System.Windows.Controls;

namespace CustomToolTip
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        public double UserControlToolTipX
        {
            get { return this.UserControlToolTipXY.X; }
            set { this.UserControlToolTipXY.X = value; }
        }

        public double UserControlToolTipY
        {
            get { return this.UserControlToolTipXY.Y; }
            set { this.UserControlToolTipXY.Y = value; }
        }

        public string UserControlTextBlockToolTip
        {
            get { return TextBlockToolTip.Text; }
            set { TextBlockToolTip.Text = value; }
        }

        public string UserControlToolTipTitle
        {
            get { return ToolTipTitle.Text; }
            set { ToolTipTitle.Text = value; }
        }
    }
}
