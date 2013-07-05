using System;
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
            Shape.StretchProperty.OverrideMetadata(typeof(Arrow), (PropertyMetadata)new FrameworkPropertyMetadata((object)Stretch.None));
        }

        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register("Length", typeof(double), typeof(Arrow), new PropertyMetadata(10.0, OnGeometryChanged));
        [Category("Appearance")]
        public double Length
        {
            get { return (double)GetValue(LengthProperty); }
            set { SetValue(LengthProperty, value); }
        }

        public static readonly DependencyProperty ShaftWidthProperty = DependencyProperty.Register("ShaftWidth", typeof(double), typeof(Arrow), new PropertyMetadata(3.0, OnGeometryChanged));
        [Category("Appearance")]
        public double ShaftWidth
        {
            get { return (double)GetValue(ShaftWidthProperty); }
            set { SetValue(ShaftWidthProperty, value); }
        }

        public static readonly DependencyProperty HeadWidthProperty =
            DependencyProperty.Register("HeadWidth", typeof(double), typeof(Arrow), new PropertyMetadata(6.0, OnGeometryChanged));
        [Category("Appearance")]
        public double HeadWidth
        {
            get { return (double)GetValue(HeadWidthProperty); }
            set { SetValue(HeadWidthProperty, value); }
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
                    double y1 = HeadWidth / 2 - ShaftWidth / 2;
                    double x1 = Length - HeadWidth / 2;
                    double y2 = HeadWidth / 2 + ShaftWidth / 2;
                    ScaleTransform scaleTransform;
                    scaleTransform = Width * Height > 0
                        ? new ScaleTransform(Width / Length, Height / HeadWidth)
                        : new ScaleTransform(1, 1);
                    var startPoint = scaleTransform.Transform(new Point(0, y1));
                    var p1 = scaleTransform.Transform(new Point(x1, y1));
                    var p2 = scaleTransform.Transform(new Point(x1, 0));
                    var p3 = scaleTransform.Transform(new Point(Length, HeadWidth / 2));
                    var p4 = scaleTransform.Transform(new Point(x1, HeadWidth));
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