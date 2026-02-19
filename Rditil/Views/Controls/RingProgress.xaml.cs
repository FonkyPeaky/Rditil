using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rditil.Views.Controls
{
    public partial class RingProgress : UserControl
    {
        public RingProgress()
        {
            InitializeComponent();
            Loaded += (_, __) => UpdateArc();
            SizeChanged += (_, __) => UpdateArc();
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(RingProgress),
                new PropertyMetadata(0d, (_, __) => ((RingProgress)_).UpdateArc()));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty RingStrokeProperty =
            DependencyProperty.Register(nameof(RingStroke), typeof(Brush), typeof(RingProgress),
                new PropertyMetadata(Brushes.DodgerBlue));

        public Brush RingStroke
        {
            get => (Brush)GetValue(RingStrokeProperty);
            set => SetValue(RingStrokeProperty, value);
        }

        // Alias for RingStroke
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(RingProgress),
                new PropertyMetadata(Brushes.DodgerBlue, OnStrokeChanged));

        public Brush Stroke
        {
            get => (Brush)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        private static void OnStrokeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RingProgress rp && e.NewValue is Brush b)
                rp.RingStroke = b;
        }

        // Alias for RingThickness
        public static readonly DependencyProperty StrokeWidthProperty =
            DependencyProperty.Register(nameof(StrokeWidth), typeof(double), typeof(RingProgress),
                new PropertyMetadata(10d, OnStrokeWidthChanged));

        public double StrokeWidth
        {
            get => (double)GetValue(StrokeWidthProperty);
            set => SetValue(StrokeWidthProperty, value);
        }

        private static void OnStrokeWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RingProgress rp && e.NewValue is double v)
            {
                rp.RingThickness = v;
                rp.UpdateArc();
            }
        }

        public static readonly DependencyProperty TrackStrokeProperty =
            DependencyProperty.Register(nameof(TrackStroke), typeof(Brush), typeof(RingProgress),
                new PropertyMetadata(Brushes.DodgerBlue));

        public Brush TrackStroke
        {
            get => (Brush)GetValue(TrackStrokeProperty);
            set => SetValue(TrackStrokeProperty, value);
        }

        public static readonly DependencyProperty RingThicknessProperty =
            DependencyProperty.Register(nameof(RingThickness), typeof(double), typeof(RingProgress),
                new PropertyMetadata(10d, (_, __) => ((RingProgress)_).UpdateArc()));

        public double RingThickness
        {
            get => (double)GetValue(RingThicknessProperty);
            set => SetValue(RingThicknessProperty, value);
        }

        public static readonly DependencyProperty CenterContentProperty =
            DependencyProperty.Register(nameof(CenterContent), typeof(object), typeof(RingProgress),
                new PropertyMetadata(null));

        public object CenterContent
        {
            get => GetValue(CenterContentProperty);
            set => SetValue(CenterContentProperty, value);
        }

        private void UpdateArc()
        {
            if (!IsLoaded) return;

            double size = Math.Min(ActualWidth, ActualHeight);
            if (size <= 0) return;

            double radius = (size / 2) - (RingThickness / 2);
            if (radius <= 0) return;

            double v = Math.Clamp(Value, 0, 100);
            if (v <= 0)
            {
                ArcPath.Data = null;
                return;
            }

            Point center = new(size / 2, size / 2);

            double startAngle = -90;
            double sweepAngle = 360 * v / 100.0;
            double endAngle = startAngle + sweepAngle;

            Point start = PointOnCircle(center, radius, startAngle);
            Point end = PointOnCircle(center, radius, endAngle);

            var arc = new ArcSegment
            {
                Point = end,
                Size = new Size(radius, radius),
                IsLargeArc = sweepAngle >= 180,
                SweepDirection = SweepDirection.Clockwise
            };

            var figure = new PathFigure { StartPoint = start };
            figure.Segments.Add(arc);

            var geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            ArcPath.Data = geometry;
        }

        private static Point PointOnCircle(Point center, double radius, double angle)
        {
            double rad = angle * Math.PI / 180;
            return new Point(
                center.X + radius * Math.Cos(rad),
                center.Y + radius * Math.Sin(rad)
            );
        }
    }
}
