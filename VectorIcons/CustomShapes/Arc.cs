using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VectorIcons.CustomShapes
{
    public class Arc : Shape
    {
        static Arc()
        {
            StretchProperty.OverrideMetadata(typeof(Arc), (PropertyMetadata)new FrameworkPropertyMetadata((object)Stretch.None));
        }

        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register("StartAngle", typeof(double), typeof(Arc), new PropertyMetadata(default(double), OnGeometryChanged));
        [TypeConverter(typeof(LengthConverter)), Category("Appearance"), Description("Angle counted from x axis positive counter clockwise")]
        public double StartAngle
        {
            get { return (double) this.GetValue(StartAngleProperty); }
            set { this.SetValue(StartAngleProperty, value); }
        }

        public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register("EndAngle", typeof(double), typeof(Arc), new PropertyMetadata(90.0, OnGeometryChanged));
        [TypeConverter(typeof(LengthConverter)), Category("Appearance"), Description("Angle counted from x axis positive counter clockwise")]
        public double EndAngle
        {
            get { return (double) this.GetValue(EndAngleProperty); }
            set { this.SetValue(EndAngleProperty, value); }
        }

        public static readonly DependencyProperty OuterRadiusProperty = DependencyProperty.Register("OuterRadius", typeof(double), typeof(Arc), new PropertyMetadata(10.0, OnGeometryChanged));
        [TypeConverter(typeof(LengthConverter)), Category("Appearance")]
        public double OuterRadius
        {
            get { return (double) this.GetValue(OuterRadiusProperty); }
            set { this.SetValue(OuterRadiusProperty, value); }
        }

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register("InnerRadius", typeof(double), typeof(Arc), new PropertyMetadata(8.0, OnGeometryChanged));
        [TypeConverter(typeof(LengthConverter)), Category("Appearance")]
        public double InnerRadius
        {
            get { return (double) this.GetValue(InnerRadiusProperty); }
            set { this.SetValue(InnerRadiusProperty, value); }
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
                var cp = new Point(this.OuterRadius - this.StrokeThickness, this.OuterRadius - this.StrokeThickness);
                using (StreamGeometryContext context = geometry.Open())
                {
                    var ip = new Point(cp.X + this.InnerRadius, cp.Y);
                    Point op = new Point(cp.X + this.OuterRadius, cp.Y);
                    var minRot = new RotateTransform(this.StartAngle, cp.X, cp.Y);
                    var maxRot = new RotateTransform(this.EndAngle, cp.X, cp.Y);
                    var angle = this.StartAngle - this.EndAngle;
                    bool isLargeArc = Math.Abs(angle) > 180;
                    Point minOuter = minRot.Transform(op);
                    Point minInner = minRot.Transform(ip);
                    Point maxOuter = maxRot.Transform(op);
                    Point maxInner = maxRot.Transform(ip);
                    context.BeginFigure(minInner, true, true);
                    context.LineTo(minOuter, true, true);
                    context.ArcTo(maxOuter, new Size(this.OuterRadius, this.OuterRadius), angle, isLargeArc, this.StartAngle < this.EndAngle ? SweepDirection.Clockwise : SweepDirection.Counterclockwise, true, true);
                    context.LineTo(maxInner, true, true);
                    context.ArcTo(minInner, new Size(this.InnerRadius, this.InnerRadius), angle, isLargeArc, this.StartAngle < this.EndAngle ? SweepDirection.Counterclockwise : SweepDirection.Clockwise, true, true);
                    context.Close();
                }

                // Freeze the geometry for performance benefits
                geometry.Freeze();
                return geometry;
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            double num = 2 * (this.OuterRadius- this.StrokeThickness);
            return new Size(num, num);
        }

    }
}
