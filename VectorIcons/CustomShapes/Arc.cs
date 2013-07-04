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
        public static readonly DependencyProperty CenterPointProperty = DependencyProperty.Register("CenterPoint", typeof(Point), typeof(Arc), new PropertyMetadata(default(Point), OnGeometryChanged));
        [TypeConverter(typeof(PointConverter))]
        public Point CenterPoint
        {
            get { return (Point)GetValue(CenterPointProperty); }
            set { SetValue(CenterPointProperty, value); }
        }

        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register("StartAngle", typeof(double), typeof(Arc), new PropertyMetadata(default(double), OnGeometryChanged));
        [TypeConverter(typeof(LengthConverter))]
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register("EndAngle", typeof(double), typeof(Arc), new PropertyMetadata(90.0, OnGeometryChanged));
        [TypeConverter(typeof(LengthConverter))]
        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        public static readonly DependencyProperty OuterRadiusProperty = DependencyProperty.Register("OuterRadius", typeof(double), typeof(Arc), new PropertyMetadata(10.0, OnGeometryChanged));
        [TypeConverter(typeof(LengthConverter))]
        public double OuterRadius
        {
            get { return (double)GetValue(OuterRadiusProperty); }
            set { SetValue(OuterRadiusProperty, value); }
        }

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register("InnerRadius", typeof(double), typeof(Arc), new PropertyMetadata(8.0, OnGeometryChanged));
        [TypeConverter(typeof(LengthConverter))]
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

                using (StreamGeometryContext context = geometry.Open())
                {
                    var ip = new Point(CenterPoint.X + InnerRadius, CenterPoint.Y);
                    Point op = new Point(CenterPoint.X + OuterRadius, CenterPoint.Y);
                    var minRot = new RotateTransform(StartAngle, CenterPoint.X, CenterPoint.Y);
                    var maxRot = new RotateTransform(EndAngle, CenterPoint.X, CenterPoint.Y);
                    var angle = StartAngle - EndAngle;
                    bool isLargeArc = Math.Abs(angle) > 180;
                    Point minOuter = minRot.Transform(op);
                    Point minInner = minRot.Transform(ip);
                    Point maxOuter = maxRot.Transform(op);
                    Point maxInner = maxRot.Transform(ip);
                    context.BeginFigure(minInner, true, true);
                    context.LineTo(minOuter, false, true);
                    context.ArcTo(maxOuter, new Size(OuterRadius, OuterRadius), angle, isLargeArc, StartAngle < EndAngle ? SweepDirection.Clockwise : SweepDirection.Counterclockwise, false, true);
                    context.LineTo(maxInner, false, true);
                    context.ArcTo(minInner, new Size(InnerRadius, InnerRadius), angle, isLargeArc, StartAngle < EndAngle ? SweepDirection.Counterclockwise : SweepDirection.Clockwise, false, true);
                    context.Close();
                }

                // Freeze the geometry for performance benefits
                geometry.Freeze();
                return geometry;
            }
        }
    }

}
