using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VectorIcons.CustomShapes
{
    public class Arc : Shape
    {
        static Arc()
        {
            Shape.StretchProperty.OverrideMetadata(typeof(Arc), (PropertyMetadata)new FrameworkPropertyMetadata((object)Stretch.None));
        }

        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register("StartAngle", typeof(double), typeof(Arc), new PropertyMetadata(default(double), OnGeometryChanged));
        [TypeConverter(typeof(LengthConverter)), Category("Appearance"), Description("Angle counted from x axis positive counter clockwise")]
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register("EndAngle", typeof(double), typeof(Arc), new PropertyMetadata(90.0, OnGeometryChanged));
        [TypeConverter(typeof(LengthConverter)), Category("Appearance"), Description("Angle counted from x axis positive counter clockwise")]
        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        public static readonly DependencyProperty OuterRadiusProperty = DependencyProperty.Register("OuterRadius", typeof(double), typeof(Arc), new PropertyMetadata(10.0, OnGeometryChanged));
        [TypeConverter(typeof(LengthConverter)), Category("Appearance")]
        public double OuterRadius
        {
            get { return (double)GetValue(OuterRadiusProperty); }
            set { SetValue(OuterRadiusProperty, value); }
        }

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register("InnerRadius", typeof(double), typeof(Arc), new PropertyMetadata(8.0, OnGeometryChanged));
        [TypeConverter(typeof(LengthConverter)), Category("Appearance")]
        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs ek)
        {
            Arc arc = (Arc)d;
            arc.InvalidateVisual();
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                // Create a StreamGeometry for describing the shape
                StreamGeometry geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;
                var cp = new Point(OuterRadius - StrokeThickness, OuterRadius - StrokeThickness);
                using (StreamGeometryContext context = geometry.Open())
                {
                    var ip = new Point(cp.X + InnerRadius, cp.Y);
                    Point op = new Point(cp.X + OuterRadius, cp.Y);
                    var minRot = new RotateTransform(StartAngle, cp.X, cp.Y);
                    var maxRot = new RotateTransform(EndAngle, cp.X, cp.Y);
                    var angle = StartAngle - EndAngle;
                    bool isLargeArc = Math.Abs(angle) > 180;
                    Point minOuter = minRot.Transform(op);
                    Point minInner = minRot.Transform(ip);
                    Point maxOuter = maxRot.Transform(op);
                    Point maxInner = maxRot.Transform(ip);
                    context.BeginFigure(minInner, true, true);
                    context.LineTo(minOuter, true, true);
                    context.ArcTo(maxOuter, new Size(OuterRadius, OuterRadius), angle, isLargeArc, StartAngle < EndAngle ? SweepDirection.Clockwise : SweepDirection.Counterclockwise, true, true);
                    context.LineTo(maxInner, true, true);
                    context.ArcTo(minInner, new Size(InnerRadius, InnerRadius), angle, isLargeArc, StartAngle < EndAngle ? SweepDirection.Counterclockwise : SweepDirection.Clockwise, true, true);
                    context.Close();
                }

                // Freeze the geometry for performance benefits
                geometry.Freeze();
                return geometry;
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            double num = 2 * (OuterRadius-StrokeThickness);
            return new Size(num, num);
        }

    }
}
