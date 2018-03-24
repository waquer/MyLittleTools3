using System.Drawing;
using System.Windows.Forms;

namespace MyLittleTools3
{
    public partial class NotifyForm : Form
    {
        public NotifyForm()
        {
            InitializeComponent();
            int x = Screen.PrimaryScreen.WorkingArea.Size.Width - 300;
            int y = Screen.PrimaryScreen.WorkingArea.Size.Height - 200;
            Point p = new Point(x, y);
            this.PointToScreen(p);
            this.Location = p;
        }
    }
}
