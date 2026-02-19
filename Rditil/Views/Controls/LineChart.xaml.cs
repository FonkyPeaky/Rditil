using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Rditil.Views.Controls;

public partial class LineChart : UserControl
{
    public LineChart()
    {
        InitializeComponent();
        Loaded += (_, __) => Redraw();
        SizeChanged += (_, __) => Redraw();
    }

    public static readonly DependencyProperty PointsProperty =
        DependencyProperty.Register(nameof(Points), typeof(IList), typeof(LineChart),
            new PropertyMetadata(null, OnPointsChanged));

    public IList? Points
    {
        get => (IList?)GetValue(PointsProperty);
        set => SetValue(PointsProperty, value);
    }

    public static readonly DependencyProperty StrokeProperty =
        DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(LineChart),
            new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2F6DFF")), (_, __) => { }));

    public Brush Stroke
    {
        get => (Brush)GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    public static readonly DependencyProperty FillProperty =
        DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(LineChart),
            new PropertyMetadata(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#332F6DFF")), (_, __) => { }));

    public Brush Fill
    {
        get => (Brush)GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }

    public static readonly DependencyProperty YMaxProperty =
        DependencyProperty.Register(nameof(YMax), typeof(double), typeof(LineChart),
            new PropertyMetadata(100d, (_, __) => { }));

    public double YMax
    {
        get => (double)GetValue(YMaxProperty);
        set => SetValue(YMaxProperty, value);
    }

    private static void OnPointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var c = (LineChart)d;

        if (e.OldValue is INotifyCollectionChanged oldObs)
            oldObs.CollectionChanged -= c.OnCollectionChanged;

        if (e.NewValue is INotifyCollectionChanged newObs)
            newObs.CollectionChanged += c.OnCollectionChanged;

        c.Redraw();
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => Redraw();

    private void Redraw()
    {
        if (!IsLoaded) return;
        ChartCanvas.Children.Clear();

        double w = ActualWidth;
        double h = ActualHeight;
        if (w < 10 || h < 10) return;

        double leftPad = 34;
        double bottomPad = 22;
        double topPad = 8;
        double rightPad = 8;

        double plotW = Math.Max(1, w - leftPad - rightPad);
        double plotH = Math.Max(1, h - topPad - bottomPad);

        DrawText("100", 0, topPad - 2, leftPad - 6, HorizontalAlignment.Right);
        DrawText("0", 0, topPad + plotH - 8, leftPad - 6, HorizontalAlignment.Right);

        var midY = topPad + plotH / 2;
        ChartCanvas.Children.Add(new Line
        {
            X1 = leftPad, X2 = leftPad + plotW,
            Y1 = midY, Y2 = midY,
            Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#220B2D5B")),
            StrokeThickness = 1
        });

        if (Points == null || Points.Count == 0) return;

        double[] values = Points.Cast<object>()
            .Select(p => GetPropDouble(p, "Value"))
            .ToArray();

        string[] labels = Points.Cast<object>()
            .Select(p => GetPropString(p, "Label"))
            .ToArray();

        int n = values.Length;
        if (n == 1) n = 2;

        var pts = values.Select((v, i) =>
        {
            double x = leftPad + plotW * (Points.Count == 1 ? 0.5 : (double)i / (Points.Count - 1));
            double y = topPad + plotH * (1 - Math.Clamp(v / Math.Max(1, YMax), 0, 1));
            return new Point(x, y);
        }).ToArray();

        var fillGeo = new PathGeometry();
        var fillFig = new PathFigure { StartPoint = new Point(pts[0].X, topPad + plotH) };
        fillFig.Segments.Add(new LineSegment(pts[0], true));
        for (int i = 1; i < pts.Length; i++)
            fillFig.Segments.Add(new LineSegment(pts[i], true));
        fillFig.Segments.Add(new LineSegment(new Point(pts[^1].X, topPad + plotH), true));
        fillFig.IsClosed = true;
        fillGeo.Figures.Add(fillFig);

        ChartCanvas.Children.Add(new Path
        {
            Data = fillGeo,
            Fill = Fill,
            StrokeThickness = 0
        });

        var lineGeo = new PathGeometry();
        var fig = new PathFigure { StartPoint = pts[0], IsClosed = false, IsFilled = false };
        for (int i = 1; i < pts.Length; i++)
            fig.Segments.Add(new LineSegment(pts[i], true));
        lineGeo.Figures.Add(fig);

        ChartCanvas.Children.Add(new Path
        {
            Data = lineGeo,
            Stroke = Stroke,
            StrokeThickness = 2.5,
            StrokeLineJoin = PenLineJoin.Round,
            StrokeStartLineCap = PenLineCap.Round,
            StrokeEndLineCap = PenLineCap.Round
        });

        for (int i = 0; i < pts.Length; i++)
        {
            var dot = new Ellipse
            {
                Width = 8,
                Height = 8,
                Fill = Brushes.White,
                Stroke = Stroke,
                StrokeThickness = 2
            };
            Canvas.SetLeft(dot, pts[i].X - 4);
            Canvas.SetTop(dot, pts[i].Y - 4);
            dot.ToolTip = $"{labels[i]} : {values[i]:0}%";
            ChartCanvas.Children.Add(dot);

            DrawText(labels[i], pts[i].X, topPad + plotH + 4, 0, HorizontalAlignment.Center);
        }
    }

    private static double GetPropDouble(object obj, string name)
    {
        var p = obj.GetType().GetProperty(name);
        if (p == null) return 0;
        var v = p.GetValue(obj);
        if (v == null) return 0;
        return Convert.ToDouble(v);
    }

    private static string GetPropString(object obj, string name)
    {
        var p = obj.GetType().GetProperty(name);
        if (p == null) return "";
        return p.GetValue(obj)?.ToString() ?? "";
    }

    private void DrawText(string text, double x, double y, double xOffset, HorizontalAlignment align)
    {
        var tb = new TextBlock
        {
            Text = text,
            FontSize = 10,
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#667085")),
        };

        tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        var w = tb.DesiredSize.Width;

        double left = align switch
        {
            HorizontalAlignment.Right => xOffset - w,
            HorizontalAlignment.Center => x - w / 2,
            _ => x + xOffset
        };

        Canvas.SetLeft(tb, left);
        Canvas.SetTop(tb, y);
        ChartCanvas.Children.Add(tb);
    }
}
