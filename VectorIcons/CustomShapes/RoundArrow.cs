﻿namespace VectorIcons.CustomShapes
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class RoundArrow : Shape
    {
        public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(
            "Length",
            typeof(double),
            typeof(RoundArrow),
            new PropertyMetadata(10.0, OnGeometryChanged));

        public static readonly DependencyProperty ShaftWidthProperty = DependencyProperty.Register(
            "ShaftWidth",
            typeof(double),
            typeof(RoundArrow),
            new PropertyMetadata(3.0, OnGeometryChanged));

        public static readonly DependencyProperty HeadWidthProperty = DependencyProperty.Register(
            "HeadWidth",
            typeof(double),
            typeof(RoundArrow),
            new PropertyMetadata(6.0, OnGeometryChanged));

        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register(
            "StartAngle",
            typeof(double),
            typeof(RoundArrow),
            new PropertyMetadata(default(double), OnGeometryChanged));

        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(
            "Direction",
            typeof(SweepDirection),
            typeof(RoundArrow),
            new PropertyMetadata(default(SweepDirection), OnGeometryChanged));

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            "Radius",
            typeof(double),
            typeof(RoundArrow),
            new PropertyMetadata(10.0, OnGeometryChanged));

        static RoundArrow()
        {
            StretchProperty.OverrideMetadata(typeof(RoundArrow), new FrameworkPropertyMetadata(Stretch.None));
        }

        [Category("Appearance")]
        public double Length
        {
            get { return (double)this.GetValue(LengthProperty); }
            set { this.SetValue(LengthProperty, value); }
        }

        [Category("Appearance")]
        public double ShaftWidth
        {
            get { return (double)this.GetValue(ShaftWidthProperty); }
            set { this.SetValue(ShaftWidthProperty, value); }
        }

        [Category("Appearance")]
        public double HeadWidth
        {
            get { return (double)this.GetValue(HeadWidthProperty); }
            set { this.SetValue(HeadWidthProperty, value); }
        }

        [Category("Appearance")]
        public double StartAngle
        {
            get { return (double)this.GetValue(StartAngleProperty); }
            set { this.SetValue(StartAngleProperty, value); }
        }

        [Category("Appearance")]
        public SweepDirection Direction
        {
            get { return (SweepDirection)this.GetValue(DirectionProperty); }
            set { this.SetValue(DirectionProperty, value); }
        }

        [Category("Appearance")]
        public double Radius
        {
            get { return (double)this.GetValue(RadiusProperty); }
            set { this.SetValue(RadiusProperty, value); }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                // Create a StreamGeometry for describing the shape
                var geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;

                var cp = new Point(this.Radius + (this.HeadWidth / 2), this.Radius + (this.HeadWidth / 2));
                using (var context = geometry.Open())
                {
                    var headStartOffsetAngle = ((this.Length - (this.HeadWidth / 2)) / this.Radius) * 180 / Math.PI * (this.Direction == SweepDirection.Clockwise ? -1 : 1);
                    var startRot = new RotateTransform(this.StartAngle, cp.X, cp.Y);
                    var headRot = new RotateTransform(this.StartAngle + headStartOffsetAngle, cp.X, cp.Y);
                    var xi = cp.X + this.Radius - (this.ShaftWidth / 2);
                    var xo = cp.X + this.Radius + (this.ShaftWidth / 2);

                    var xhi = cp.X + this.Radius - (this.HeadWidth / 2);
                    var xho = cp.X + this.Radius + (this.HeadWidth / 2);
                    var pi = new Point(xi, cp.Y);
                    var po = new Point(xo, cp.Y);
                    var phi = new Point(xhi, cp.Y);
                    var pho = new Point(xho, cp.Y);
                    var ep = new Point(cp.X + this.Radius, cp.Y + (this.HeadWidth / 2 * (this.Direction == SweepDirection.Clockwise ? -1 : 1)));

                    var startPoint = startRot.Transform(pi);
                    var p1 = headRot.Transform(pi);
                    var p2 = headRot.Transform(phi);
                    var p3 = headRot.Transform(ep);
                    var p4 = headRot.Transform(pho);
                    var p5 = headRot.Transform(po);
                    var p6 = startRot.Transform(po);

                    context.BeginFigure(startPoint, true, true);
                    context.ArcTo(p1, new Size(this.Radius - (this.ShaftWidth / 2), this.Radius - (this.ShaftWidth / 2)), headStartOffsetAngle, Math.Abs(headStartOffsetAngle) > 180, this.Direction == SweepDirection.Clockwise ? SweepDirection.Counterclockwise : SweepDirection.Clockwise, true, true);
                    context.LineTo(p2, true, true);
                    context.LineTo(p3, true, true);
                    context.LineTo(p4, true, true);
                    context.LineTo(p5, true, true);
                    context.ArcTo(p6, new Size(this.Radius + (this.ShaftWidth / 2), this.Radius + (this.ShaftWidth / 2)), headStartOffsetAngle, Math.Abs(headStartOffsetAngle) > 180, this.Direction, true, true);
                    context.Close();
                }

                // Freeze the geometry for performance benefits
                geometry.Freeze();
                return geometry;
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var num = 2 * (this.Radius + (this.HeadWidth / 2));
            return new Size(num, num);
        }

        private static void OnGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs ek)
        {
            var roundArrow = (RoundArrow)d;
            roundArrow.InvalidateVisual();
        }
    }
}
