using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VectorIcons.CustomShapes
{
    public class Arrow : Shape
    {
        static Arrow()
        {
            StretchProperty.OverrideMetadata(typeof(Arrow), (PropertyMetadata)new FrameworkPropertyMetadata((object)Stretch.None));
        }

        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register("Length", typeof(double), typeof(Arrow), new PropertyMetadata(10.0, OnGeometryChanged));
        [Category("Appearance")]
        public double Length
        {
            get { return (double) this.GetValue(LengthProperty); }
            set { this.SetValue(LengthProperty, value); }
        }

        public static readonly DependencyProperty ShaftWidthProperty = DependencyProperty.Register("ShaftWidth", typeof(double), typeof(Arrow), new PropertyMetadata(3.0, OnGeometryChanged));
        [Category("Appearance")]
        public double ShaftWidth
        {
            get { return (double) this.GetValue(ShaftWidthProperty); }
            set { this.SetValue(ShaftWidthProperty, value); }
        }

        public static readonly DependencyProperty HeadWidthProperty =
            DependencyProperty.Register("HeadWidth", typeof(double), typeof(Arrow), new PropertyMetadata(6.0, OnGeometryChanged));
        [Category("Appearance")]
        public double HeadWidth
        {
            get { return (double) this.GetValue(HeadWidthProperty); }
            set { this.SetValue(HeadWidthProperty, value); }
        }

        private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs ek)
        {
            Arrow arrow = (Arrow)d;
            arrow.InvalidateVisual();
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                // Create a StreamGeometry for describing the shape
                StreamGeometry geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;

                using (StreamGeometryContext context = geometry.Open())
                {
                    double y1 = this.HeadWidth / 2 - this.ShaftWidth / 2;
                    double x1 = this.Length - this.HeadWidth / 2;
                    double y2 = this.HeadWidth / 2 + this.ShaftWidth / 2;
                    ScaleTransform scaleTransform;
                    scaleTransform = this.Width *this.Height > 0
                        ? new ScaleTransform(this.Width /this.Length, this.Height /this.HeadWidth)
                        : new ScaleTransform(1, 1);
                    var startPoint = scaleTransform.Transform(new Point(0, y1));
                    var p1 = scaleTransform.Transform(new Point(x1, y1));
                    var p2 = scaleTransform.Transform(new Point(x1, 0));
                    var p3 = scaleTransform.Transform(new Point(this.Length, this.HeadWidth / 2));
                    var p4 = scaleTransform.Transform(new Point(x1, this.HeadWidth));
                    var p5 = scaleTransform.Transform(new Point(x1, y2));
                    var p6 = scaleTransform.Transform(new Point(0, y2));

                    context.BeginFigure(startPoint, true, true);
                    context.LineTo(p1, true, true);
                    context.LineTo(p2, true, true);
                    context.LineTo(p3, true, true);
                    context.LineTo(p4, true, true);
                    context.LineTo(p5, true, true);
                    context.LineTo(p6, true, true);
                    context.Close();
                }

                // Freeze the geometry for performance benefits
                geometry.Freeze();
                return geometry;
            }
        }

    }
}