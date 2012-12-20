using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point origin;
        private Point start;

        public MainWindow()
        {
            InitializeComponent();

            TransformGroup group = new TransformGroup();

            ScaleTransform xform = new ScaleTransform();
            group.Children.Add(xform);

            TranslateTransform tt = new TranslateTransform();
            group.Children.Add(tt);

            imgMap.RenderTransform = group;

            imgMap.MouseWheel += new MouseWheelEventHandler(imgMap_MouseWheel);
            imgMap.MouseLeftButtonDown += new MouseButtonEventHandler(imgMap_MouseLeftButtonDown);
            imgMap.MouseLeftButtonUp += new MouseButtonEventHandler(imgMap_MouseLeftButtonUp);
            imgMap.MouseMove += new MouseEventHandler(imgMap_MouseMove);
        }

        void imgMap_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            TransformGroup transformGroup = (TransformGroup)imgMap.RenderTransform;
            ScaleTransform transform = (ScaleTransform)transformGroup.Children[0];

            double zoom = e.Delta > 0 ? .2 : -.2;
            if (zoom > 0)
            {
                transform.ScaleX += zoom;
                transform.ScaleY += zoom;
            }
            else
            {
                if (transform.ScaleX >= 1)
                {
                    transform.ScaleX += zoom;
                    transform.ScaleY += zoom;
                }
            }
        }

        void imgMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            imgMap.CaptureMouse();
            var tt = (TranslateTransform)((TransformGroup)imgMap.RenderTransform).Children.First(tr => tr is TranslateTransform);
            start = e.GetPosition(border);
            origin = new Point(tt.X, tt.Y);
        }

        void imgMap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            imgMap.ReleaseMouseCapture();
        }

        void imgMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (!imgMap.IsMouseCaptured) return;

            var tt = (TranslateTransform)((TransformGroup)imgMap.RenderTransform).Children.First(tr => tr is TranslateTransform);
            Vector v = start - e.GetPosition(border);
            tt.X = origin.X - v.X;
            tt.Y = origin.Y - v.Y;
        }
    }
}
