using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.EntityFrameworkCore;
using Rditil.Data;
using Rditil.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Rditil.ViewModels;

public sealed class ProgressViewModel : ViewModelBase, INavigable
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;
    private readonly INavigationService _nav;
    private readonly IAppState _appState;

    public RelayCommand BackCommand { get; }

    // ===== UI collections =====
    public ObservableCollection<AttemptRowVm> RecentAttempts { get; } = new();
    public ObservableCollection<HeatCellVm> HeatmapCells { get; } = new();
    public string HeatmapDateLabel { get; private set; } = "";
    public ObservableCollection<LinePointVm> LinePoints { get; } = new();

    // ===== LiveCharts properties =====
    private ISeries[] _chartSeries = Array.Empty<ISeries>();
    public ISeries[] ChartSeries
    {
        get => _chartSeries;
        set { _chartSeries = value; OnPropertyChanged(); }
    }

    private Axis[] _xAxes = Array.Empty<Axis>();
    public Axis[] XAxes
    {
        get => _xAxes;
        set { _xAxes = value; OnPropertyChanged(); }
    }

    private Axis[] _yAxes = Array.Empty<Axis>();
    public Axis[] YAxes
    {
        get => _yAxes;
        set { _yAxes = value; OnPropertyChanged(); }
    }

    // ===== Header =====
    private string _title = "Progression";
    public string Title { get => _title; set { _title = value; OnPropertyChanged(); } }

    private string _subtitle = "Objectif du jour : 1 examen";
    public string Subtitle { get => _subtitle; set { _subtitle = value; OnPropertyChanged(); } }

    // ===== Top cards =====
    private string _lastScoreBig = "—";
    public string LastScoreBig { get => _lastScoreBig; set { _lastScoreBig = value; OnPropertyChanged(); } }

    private string _lastScoreSub = "Dernier examen";
    public string LastScoreSub { get => _lastScoreSub; set { _lastScoreSub = value; OnPropertyChanged(); } }

    private string _avg7Big = "—";
    public string Avg7Big { get => _avg7Big; set { _avg7Big = value; OnPropertyChanged(); } }

    private string _avg7Sub = "Moyenne (7 derniers)";
    public string Avg7Sub { get => _avg7Sub; set { _avg7Sub = value; OnPropertyChanged(); } }

    private string _best30Big = "—";
    public string Best30Big { get => _best30Big; set { _best30Big = value; OnPropertyChanged(); } }

    private string _best30Sub = "Meilleur (30 jours)";
    public string Best30Sub { get => _best30Sub; set { _best30Sub = value; OnPropertyChanged(); } }

    private string _streakBig = "0";
    public string StreakBig { get => _streakBig; set { _streakBig = value; OnPropertyChanged(); } }

    private string _streakSub = "Streak (jours)";
    public string StreakSub { get => _streakSub; set { _streakSub = value; OnPropertyChanged(); } }

    // ===== Premium badges =====
    private string _levelName = "Bronze";
    public string LevelName { get => _levelName; set { _levelName = value; OnPropertyChanged(); } }

    private string _levelHint = "Continue comme ça 💪";
    public string LevelHint { get => _levelHint; set { _levelHint = value; OnPropertyChanged(); } }

    private int _level = 0; // 0..2
    public int Level { get => _level; set { _level = value; OnPropertyChanged(); } }

    private string _trendText = "—";
    public string TrendText { get => _trendText; set { _trendText = value; OnPropertyChanged(); } }

    private string _trendArrow = "•";
    public string TrendArrow { get => _trendArrow; set { _trendArrow = value; OnPropertyChanged(); } }

    private Brush _trendBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#667085")!);
    public Brush TrendBrush { get => _trendBrush; set { _trendBrush = value; OnPropertyChanged(); } }

    // ===== Goal ring =====
    private double _goalRingValue = 0;
    public double GoalRingValue { get => _goalRingValue; set { _goalRingValue = value; OnPropertyChanged(); } }

    private Brush _goalRingStroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2F6DFF")!);
    public Brush GoalRingStroke { get => _goalRingStroke; set { _goalRingStroke = value; OnPropertyChanged(); } }

    private string _goalRingCenterTop = "0/1";
    public string GoalRingCenterTop { get => _goalRingCenterTop; set { _goalRingCenterTop = value; OnPropertyChanged(); } }

    private string _goalRingCenterBottom = "objectif";
    public string GoalRingCenterBottom { get => _goalRingCenterBottom; set { _goalRingCenterBottom = value; OnPropertyChanged(); } }

    private string _goalChipText = "Objectif du jour : 1 examen";
    public string GoalChipText { get => _goalChipText; set { _goalChipText = value; OnPropertyChanged(); } }

    private Brush _goalChipBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A2F6DFF")!);
    public Brush GoalChipBackground { get => _goalChipBackground; set { _goalChipBackground = value; OnPropertyChanged(); } }

    private string _goalText = "Objectif du jour : 1 examen";
    public string GoalText { get => _goalText; set { _goalText = value; OnPropertyChanged(); } }

    // ===== Right panel texts =====
    private string _lastListSubtitle = "10 derniers examens";
    public string LastListSubtitle { get => _lastListSubtitle; set { _lastListSubtitle = value; OnPropertyChanged(); } }

    private string _tipTitle = "Astuce";
    public string TipTitle { get => _tipTitle; set { _tipTitle = value; OnPropertyChanged(); } }

    private string _tipText = "Fais 1 examen par jour pour booster ton niveau et garder le rythme.";
    public string TipText { get => _tipText; set { _tipText = value; OnPropertyChanged(); } }

    private Task? _loadTask;

    public ProgressViewModel(IDbContextFactory<AppDbContext> dbFactory, INavigationService nav, IAppState appState)
    {
        _dbFactory = dbFactory;
        _nav = nav;
        _appState = appState;
        BackCommand = new RelayCommand(_ => _nav.NavigateTo<WelcomeViewModel>());

        // Initialize empty chart
        InitializeChart(new List<double>(), new List<string>());
    }

    public void OnNavigatedTo(Dictionary<string, object?>? parameters)
    {
        // Force reload à chaque navigation pour avoir les données fraîches
        _loadTask = null;
        _ = EnsureLoadedAsync();
    }

    public Task EnsureLoadedAsync() => _loadTask ??= LoadAsync();

    private void InitializeChart(List<double> values, List<string> labels)
    {
        var greenColor = new SKColor(24, 195, 126); // #18C37E
        var blueColor = new SKColor(47, 109, 255);  // #2F6DFF

        ChartSeries = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = values.Count > 0 ? values : new List<double> { 0 },
                Fill = new LinearGradientPaint(
                    new[] { greenColor.WithAlpha(80), greenColor.WithAlpha(0) },
                    new SKPoint(0.5f, 0),
                    new SKPoint(0.5f, 1)),
                Stroke = new SolidColorPaint(greenColor, 3),
                GeometryFill = new SolidColorPaint(greenColor),
                GeometryStroke = new SolidColorPaint(SKColors.White, 2),
                GeometrySize = 10,
                LineSmoothness = 0.65,
                EnableNullSplitting = false
            }
        };

        XAxes = new Axis[]
        {
            new Axis
            {
                Labels = labels.Count > 0 ? labels : new[] { "" },
                LabelsPaint = new SolidColorPaint(SKColors.White.WithAlpha(180)),
                TextSize = 11,
                SeparatorsPaint = new SolidColorPaint(SKColors.White.WithAlpha(30)),
                TicksPaint = new SolidColorPaint(SKColors.White.WithAlpha(50)),
            }
        };

        YAxes = new Axis[]
        {
            new Axis
            {
                MinLimit = 0,
                MaxLimit = 100,
                LabelsPaint = new SolidColorPaint(SKColors.White.WithAlpha(180)),
                TextSize = 11,
                SeparatorsPaint = new SolidColorPaint(SKColors.White.WithAlpha(30)),
                Labeler = value => $"{value}%"
            }
        };
    }

    private async Task LoadAsync()
    {
        RecentAttempts.Clear();
        HeatmapCells.Clear();
        LinePoints.Clear();

        var u = _appState.CurrentUser;
        if (u == null)
        {
            Title = "Progression";
            Subtitle = "Connecte-toi pour voir l'historique.";
            InitializeChart(new List<double>(), new List<string>());
            return;
        }

        Title = $"Progression — {u.Prenom} {u.Nom}".Trim();

        await using var db = await _dbFactory.CreateDbContextAsync();

        // ----- Last 10 attempts (for line chart + table) -----
        var last10 = await db.ExamAttempts
            .Where(a => a.Id_Utilisateur == u.Id_Utilisateur)
            .OrderByDescending(a => a.FinishedAt ?? a.StartedAt)
            .Take(10)
            .ToListAsync();

        var last10Asc = last10
            .OrderBy(a => a.FinishedAt ?? a.StartedAt)
            .ToList();

        // Build chart data
        var chartValues = new List<double>();
        var chartLabels = new List<string>();

        foreach (var a in last10Asc)
        {
            var dt = (a.FinishedAt ?? a.StartedAt).ToLocalTime();
            var percent = a.TotalQuestions <= 0 ? 0 : (a.Score * 100.0 / a.TotalQuestions);

            chartValues.Add(Math.Round(percent, 1));
            chartLabels.Add(dt.ToString("dd/MM"));

            LinePoints.Add(new LinePointVm
            {
                Label = dt.ToString("dd/MM"),
                Value = Math.Clamp(percent, 0, 100)
            });
        }

        // Update chart
        InitializeChart(chartValues, chartLabels);

        foreach (var a in last10.OrderByDescending(a => a.FinishedAt ?? a.StartedAt))
        {
            var dt = (a.FinishedAt ?? a.StartedAt).ToLocalTime();
            var dur = a.FinishedAt.HasValue ? (a.FinishedAt.Value - a.StartedAt) : TimeSpan.Zero;
            RecentAttempts.Add(new AttemptRowVm
            {
                DateText = dt.ToString("dd/MM/yyyy HH:mm"),
                ScoreText = $"{a.Score}/{a.TotalQuestions}",
                DurationText = dur == TimeSpan.Zero ? "—" : dur.ToString(@"hh\:mm\:ss")
            });
        }

        LastListSubtitle = $"{RecentAttempts.Count} derniers examens";

        // ----- Stats -----
        if (last10.Count > 0)
        {
            var last = last10.OrderByDescending(a => a.FinishedAt ?? a.StartedAt).First();
            var lastPct = last.TotalQuestions <= 0 ? 0 : (last.Score * 100.0 / last.TotalQuestions);
            LastScoreBig = $"{Math.Round(lastPct)}%";
            LastScoreSub = $"{last.Score}/{last.TotalQuestions} (dernier)";
        }
        else
        {
            LastScoreBig = "—";
            LastScoreSub = "Aucun examen";
        }

        // Avg 7 last attempts
        var last7 = last10.OrderByDescending(a => a.FinishedAt ?? a.StartedAt).Take(7).ToList();
        if (last7.Count > 0)
        {
            var avg = last7.Average(a => a.TotalQuestions <= 0 ? 0 : (a.Score * 100.0 / a.TotalQuestions));
            Avg7Big = $"{Math.Round(avg)}%";
            Avg7Sub = "Moyenne (7 derniers)";
            SetLevel(avg);
        }
        else
        {
            Avg7Big = "—";
            Avg7Sub = "Moyenne (7 derniers)";
            SetLevel(0);
        }

        // Best 30 days
        var since30 = DateTime.UtcNow.Date.AddDays(-29);
        var best30 = await db.ExamAttempts
            .Where(a => a.Id_Utilisateur == u.Id_Utilisateur)
            .Where(a => (a.FinishedAt ?? a.StartedAt) >= since30)
            .OrderByDescending(a => a.TotalQuestions <= 0 ? 0 : (a.Score * 100.0 / a.TotalQuestions))
            .FirstOrDefaultAsync();

        if (best30 != null)
        {
            var best = best30.TotalQuestions <= 0 ? 0 : (best30.Score * 100.0 / best30.TotalQuestions);
            Best30Big = $"{Math.Round(best)}%";
            Best30Sub = $"{best30.Score}/{best30.TotalQuestions}";
        }
        else
        {
            Best30Big = "—";
            Best30Sub = "30 jours";
        }

        // Streak (consecutive days with >=1 finished attempt)
        var streak = await ComputeStreakAsync(db, u.Id_Utilisateur);
        StreakBig = $"{streak}";
        StreakSub = "Streak (jours)";

        // Trend (last vs avg previous 3)
        ComputeTrend(last10);

        // Goal today
        await ComputeGoalAsync(db, u.Id_Utilisateur);

        // Heatmap 30 days (5 columns x 7 rows = 35, we fill last 30 + padding)
        await BuildHeatmapAsync(db, u.Id_Utilisateur);

        Subtitle = GoalChipText;
    }

    private async Task ComputeGoalAsync(AppDbContext db, int userId)
    {
        var todayUtc = DateTime.UtcNow.Date;
        var countToday = await db.ExamAttempts
            .Where(a => a.Id_Utilisateur == userId)
            .Where(a => a.FinishedAt != null)
            .Where(a => a.FinishedAt >= todayUtc && a.FinishedAt < todayUtc.AddDays(1))
            .CountAsync();

        bool done = countToday >= 1;

        GoalRingValue = done ? 100 : (countToday * 100);
        GoalRingStroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(done ? "#18C37E" : "#2F6DFF")!);
        GoalRingCenterTop = $"{Math.Min(countToday, 1)}/1";
        GoalRingCenterBottom = "objectif";
        GoalChipBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(done ? "#1A18C37E" : "#1A2F6DFF")!);
        GoalChipText = done ? "Objectif du jour atteint ✅" : "Objectif du jour : 1 examen";
        GoalText = GoalChipText;
    }

    private void ComputeTrend(List<Models.ExamAttempt> last10)
    {
        if (last10.Count == 0)
        {
            TrendArrow = "•";
            TrendText = "Pas de données";
            TrendBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#667085")!);
            return;
        }

        var ordered = last10.OrderByDescending(a => a.FinishedAt ?? a.StartedAt).ToList();
        var last = ordered[0];
        var lastPct = last.TotalQuestions <= 0 ? 0 : (last.Score * 100.0 / last.TotalQuestions);

        var prev3 = ordered.Skip(1).Take(3).ToList();
        if (prev3.Count == 0)
        {
            TrendArrow = "•";
            TrendText = "Premier score";
            TrendBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#667085")!);
            return;
        }

        var avgPrev = prev3.Average(a => a.TotalQuestions <= 0 ? 0 : (a.Score * 100.0 / a.TotalQuestions));
        var delta = lastPct - avgPrev;

        if (Math.Abs(delta) < 0.5)
        {
            TrendArrow = "•";
            TrendText = "Stable";
            TrendBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#667085")!);
        }
        else if (delta > 0)
        {
            TrendArrow = "▲";
            TrendText = $"+{Math.Round(delta)}% vs 3 précédents";
            TrendBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#18C37E")!);
        }
        else
        {
            TrendArrow = "▼";
            TrendText = $"{Math.Round(delta)}% vs 3 précédents";
            TrendBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E23B3B")!);
        }
    }

    private void SetLevel(double avg7)
    {
        // Bronze < 60, Argent 60..79, Or >= 80
        if (avg7 >= 80)
        {
            Level = 2;
            LevelName = "Or";
            LevelHint = "Excellent niveau 👑";
            TipTitle = "Mode Or";
            TipText = "Tu es en forme ! Garde le rythme avec 1 examen/jour.";
        }
        else if (avg7 >= 60)
        {
            Level = 1;
            LevelName = "Argent";
            LevelHint = "Solide et régulier ⚡";
            TipTitle = "Mode Argent";
            TipText = "Tu progresses vite. Fais un petit examen même les jours chargés.";
        }
        else
        {
            Level = 0;
            LevelName = "Bronze";
            LevelHint = "On construit la base 💪";
            TipTitle = "Mode Bronze";
            TipText = "Focus sur la régularité : 1 examen/jour, même court.";
        }
    }

    private static async Task<int> ComputeStreakAsync(AppDbContext db, int userId)
    {
        // Weekend-safe streak:
        // - counts consecutive working days (Mon–Fri) that have >= 1 finished attempt
        // - weekend days do NOT break the streak (Fri -> Mon is OK)
        // - if today is weekend, we evaluate from the most recent working day

        var since = DateTime.UtcNow.Date.AddDays(-90);
        var dates = await db.ExamAttempts
            .Where(a => a.Id_Utilisateur == userId)
            .Where(a => a.FinishedAt != null && a.FinishedAt >= since)
            .Select(a => (a.FinishedAt ?? a.StartedAt))
            .ToListAsync();

        var localDays = dates
            .Select(d => d.ToLocalTime().Date)
            .Distinct()
            .ToHashSet();

        static bool IsWeekend(DateTime d) =>
            d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday;

        static DateTime PrevWorkingDay(DateTime d)
        {
            var x = d.AddDays(-1);
            while (IsWeekend(x))
                x = x.AddDays(-1);
            return x;
        }


        var day = DateTime.Now.Date;
        while (IsWeekend(day))
            day = day.AddDays(-1);

        if (!localDays.Contains(day))
            return 0;

        int streak = 0;
        while (localDays.Contains(day))
        {
            streak++;
            day = PrevWorkingDay(day);
        }

        return streak;

    }

    private async Task BuildHeatmapAsync(AppDbContext db, int userId)
    {
        var todayLocal = DateTime.Now.Date;
        var startLocal = todayLocal.AddDays(-29);

        HeatmapDateLabel = $"{startLocal:dd/MM} → {todayLocal:dd/MM}";
        OnPropertyChanged(nameof(HeatmapDateLabel));

        var startUtc = startLocal.ToUniversalTime().Date;
        var endUtc = todayLocal.AddDays(1).ToUniversalTime().Date;

        var attempts = await db.ExamAttempts
            .Where(a => a.Id_Utilisateur == userId)
            .Where(a => (a.FinishedAt ?? a.StartedAt) >= startUtc && (a.FinishedAt ?? a.StartedAt) < endUtc)
            .ToListAsync();

        var perDay = attempts
            .GroupBy(a => (a.FinishedAt ?? a.StartedAt).ToLocalTime().Date)
            .ToDictionary(
                g => g.Key,
                g => g.Max(a => a.TotalQuestions <= 0 ? 0 : (a.Score * 100.0 / a.TotalQuestions))
            );

        int totalCells = 35;
        var daysToShow = Enumerable.Range(0, 30).Select(i => startLocal.AddDays(i)).ToList();
        int padding = totalCells - daysToShow.Count;

        for (int i = 0; i < padding; i++)
            HeatmapCells.Add(new HeatCellVm { Level = 0, Tooltip = "" });

        foreach (var day in daysToShow)
        {
            perDay.TryGetValue(day, out var pct);
            int level = pct <= 0 ? 0 :
                        pct < 50 ? 1 :
                        pct < 70 ? 2 :
                        pct < 85 ? 3 : 4;

            HeatmapCells.Add(new HeatCellVm
            {
                Level = level,
                Tooltip = pct <= 0 ? $"{day:dd/MM}: aucun examen" : $"{day:dd/MM}: {Math.Round(pct)}%"
            });
        }
    }
}

public sealed class LinePointVm
{
    public string Label { get; set; } = "";
    public double Value { get; set; } // 0..100
}

public sealed class HeatCellVm
{
    public int Level { get; set; } // 0..4
    public string Tooltip { get; set; } = "";
}

public sealed class AttemptRowVm : ViewModelBase
{
    public string DateText { get; set; } = "";
    public string ScoreText { get; set; } = "";
    public string DurationText { get; set; } = "";
}
