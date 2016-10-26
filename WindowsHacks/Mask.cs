using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsAPI;

namespace WindowsHacks
{

    /// <summary>
    /// This is a transparent layer that can be placed on top of a window to create an effect.
    /// </summary>
    public partial class Mask : Form
    {
        bool mouseDown = false;
        Point clickPoint = new Point(0, 0);
        bool movementEnabled = true;

        public Mask()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create a form to overlay the desktop with an image.
        /// </summary>
        /// <param name="bmp">The overlay image.</param>
        public Mask(Bitmap bmp)
        {
            InitializeComponent();
            OverlayWindow(bmp);
        }

        /// <summary>
        /// Create a form to overlay a window with an image.
        /// </summary>
        /// <param name="hWnd">The handle to the window.</param>
        /// <param name="bmp">The overlay image.</param>
        public Mask(IntPtr hWnd, Bitmap bmp)
        {
            InitializeComponent();
            OverlayWindow(hWnd, bmp);
        }

        /// <summary>
        /// Overlays the desktop with a specified image.
        /// </summary>
        /// <param name="bmp">The overlay image.</param>
        public void OverlayWindow(Bitmap bmp)
        {
            Show();
            Location = new Point(0, 0);
            Size = Screen.PrimaryScreen.Bounds.Size;
            TopMost = true;
            Picture.Image = bmp;
            Refresh();
        }

        /// <summary>
        /// Overlays a window with a specified image.
        /// </summary>
        /// <param name="hWnd">The handle to the window.</param>
        /// <param name="bmp">The overlay image.</param>
        public void OverlayWindow(IntPtr hWnd, Bitmap bmp)
        {
            Show();
            Location = Window.GetLocation(hWnd);
            Size = Window.GetSize(hWnd);
            TopMost = true;
            Picture.Image = bmp;
            Refresh();
        }

        /// <summary>
        /// Allows the user to drag the layer to new locations.
        /// </summary>
        public void EnableMovement()
        {
            movementEnabled = true;
        }

        /// <summary>
        /// Prevents the user from dragging the layer to new locations.
        /// </summary>
        public void DisableMovement()
        {
            movementEnabled = false;
        }

        private void Picture_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown && movementEnabled)
            {
                Location = new Point(Cursor.Position.X + clickPoint.X, Cursor.Position.Y + clickPoint.Y);
            }
        }

        private void Picture_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                clickPoint = new Point(Location.X - Cursor.Position.X, Location.Y - Cursor.Position.Y);
            }
        }

        private void Picture_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void Picture_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) Application.Exit();
        }

    }
}
