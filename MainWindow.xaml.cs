using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DvrkTweaks.Data;
using DvrkTweaks.Models;
using DvrkTweaks.Services;

namespace DvrkTweaks;

public partial class MainWindow : Window
{
    private readonly Dictionary<string, bool> state = StateStore.Load();
    private readonly Stack<string> history = new();
    private string current = "Home";
    private TextBlock? pageCount;
    private TextBox? searchBox;
    private string currentCategory = "";

    private readonly string[] navItems =
    {
        "Home", "FPS Tweaks", "Network Tweaks", "Fortnite Tweaks", "Windows Tweaks",
        "GPU Tweaks", "CPU Tweaks", "RAM Tweaks", "Storage Tweaks", "System Tweaks",
        "Latency / Ping", "PS5 Optimisation", "System Monitor", "Settings"
    };

    public MainWindow()
    {
        InitializeComponent();
        BuildNav();
        ShowHome();
    }

    private void BuildNav()
    {
        Nav.Children.Clear();
        foreach (var item in navItems)
        {
            var button = new Button
            {
                Content = item == "Home" ? "⌂  HOME" : item == "PS5 Optimisation" ? "🎮  PS5 OPTIMISATION" : "▸  " + item.ToUpperInvariant(),
                Style = (Style)FindResource("NavButton")
            };
            button.Click += (_, _) => Go(item);
            Nav.Children.Add(button);
        }
    }

    private void Go(string page)
    {
        if (page != current)
            history.Push(current);

        current = page;
        switch (page)
        {
            case "Home": ShowHome(); break;
            case "PS5 Optimisation": ShowPS5(); break;
            case "System Monitor": ShowMonitor(); break;
            case "Settings": ShowSettings(); break;
            default: ShowCategory(page); break;
        }
    }

    private StackPanel PageHeader(string title, string subtitle)
    {
        var panel = new StackPanel();
        panel.Children.Add(new TextBlock
        {
            Text = title,
            FontSize = 30,
            FontWeight = FontWeights.Bold,
            Foreground = title == currentCategory && !string.IsNullOrEmpty(currentCategory) ? CategoryAccent(currentCategory) : Brushes.White
        });
        panel.Children.Add(new TextBlock
        {
            Text = subtitle,
            FontSize = 13,
            Foreground = (Brush)FindResource("MutedBrush"),
            Margin = new Thickness(0, 4, 0, 18)
        });
        return panel;
    }

    private Border Card(UIElement child)
    {
        return new Border
        {
            Background = (Brush)FindResource("PanelBrush"),
            BorderBrush = (Brush)FindResource("BorderBrushDvrk"),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(12),
            Padding = new Thickness(16),
            Margin = new Thickness(0, 0, 0, 10),
            Child = child
        };
    }

    private void ShowHome()
    {
        var root = PageHeader("Welcome to DVRK Tweaks", "A cleaner V3 control panel for Windows gaming performance.");
        root.Children.Add(Card(new StackPanel
        {
            Children =
            {
                new TextBlock { Text = "ABOUT DVRK", FontSize = 18, FontWeight = FontWeights.Bold, Foreground = Brushes.White },
                new TextBlock { Text = "DVRK Tweaks is built around useful, reversible Windows controls for gaming PCs. V3 separates FPS, network, Fortnite, Windows, CPU, GPU, RAM, storage and latency tools so you can choose exactly what you want to apply.", TextWrapping = TextWrapping.Wrap, Foreground = (Brush)FindResource("MutedBrush"), Margin = new Thickness(0, 9, 0, 0), FontSize = 14 },
                new TextBlock { Text = "Not every tweak improves every PC. DVRK avoids pretending that a switch guarantees extra FPS or lower ping.", TextWrapping = TextWrapping.Wrap, Foreground = Brushes.White, Margin = new Thickness(0, 12, 0, 0), FontSize = 13 }
            }
        }));

        var stats = new WrapPanel { Margin = new Thickness(0, 4, 0, 12) };
        stats.Children.Add(StatCard("11", "TWEAK CATEGORIES"));
        stats.Children.Add(StatCard("165", "TWEAKS"));
        stats.Children.Add(StatCard("15", "PS5 CHECKS"));
        stats.Children.Add(StatCard("V3", "BUILD"));
        root.Children.Add(stats);

        var links = new StackPanel { Orientation = Orientation.Horizontal };
        links.Children.Add(ActionButton("OPEN WEBSITE", () => OpenUrl("https://dvrk-tweaks.github.io/dvrk-tweaks/")));
        links.Children.Add(ActionButton("OPEN DISCORD", () => OpenUrl("https://discord.gg/8sD2CvxMk")));
        links.Children.Add(ActionButton("OPEN YOUTUBE", () => OpenUrl("https://www.youtube.com/channel/UC34tB1hro48HY9pvlq3IIxg")));
        root.Children.Add(links);

        MainContent.Content = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto, Content = root };
    }

    private Border StatCard(string value, string label)
    {
        var stack = new StackPanel();
        stack.Children.Add(new TextBlock { Text = value, FontSize = 24, FontWeight = FontWeights.Bold, Foreground = Brushes.White });
        stack.Children.Add(new TextBlock { Text = label, FontSize = 10, FontWeight = FontWeights.Bold, Foreground = (Brush)FindResource("MutedBrush") });
        return new Border
        {
            Width = 150,
            Background = (Brush)FindResource("Panel2Brush"),
            BorderBrush = (Brush)FindResource("BorderBrushDvrk"),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(10),
            Padding = new Thickness(15),
            Margin = new Thickness(0, 0, 10, 0),
            Child = stack
        };
    }

    private Button ActionButton(string text, Action action)
    {
        var b = new Button { Content = text, Width = 145, Margin = new Thickness(0, 0, 8, 0) };
        b.Click += (_, _) => action();
        return b;
    }

    private void ShowCategory(string category)
    {
        currentCategory = category;
        var list = TweakCatalog.Categories[category];
        var root = PageHeader(category, $"{list.Count} controls available. Toggle the options you want, then apply the enabled items.");

        var toolbar = new DockPanel { Margin = new Thickness(0, 0, 0, 12) };
        searchBox = new TextBox { Width = 300, Height = 38, Text = "Search tweaks...", Padding = new Thickness(12, 8, 12, 8), Background = (Brush)FindResource("PanelBrush"), Foreground = Brushes.White, BorderBrush = (Brush)FindResource("BorderBrushDvrk") };
        searchBox.TextChanged += (_, _) => RenderTweakList(root, list);
        DockPanel.SetDock(searchBox, Dock.Left);
        toolbar.Children.Add(searchBox);

        var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
        var select = new Button { Content = "SELECT ALL", Width = 110, Margin = new Thickness(5, 0, 5, 0) };
        select.Click += (_, _) => { foreach (var t in list) state[Key(category, t)] = true; StateStore.Save(state); RenderTweakList(root, list); };
        var clear = new Button { Content = "CLEAR", Width = 90, Margin = new Thickness(5, 0, 5, 0) };
        clear.Click += (_, _) => { foreach (var t in list) state[Key(category, t)] = false; StateStore.Save(state); RenderTweakList(root, list); };
        var apply = new Button { Content = "APPLY ENABLED", Width = 140, Margin = new Thickness(5, 0, 0, 0), Background = new SolidColorBrush(Color.FromRgb(109, 61, 224)) };
        apply.Click += (_, _) => ApplyCategory(category, list);
        buttons.Children.Add(select); buttons.Children.Add(clear); buttons.Children.Add(apply);
        DockPanel.SetDock(buttons, Dock.Right);
        toolbar.Children.Add(buttons);
        root.Children.Add(toolbar);

        pageCount = new TextBlock { Foreground = (Brush)FindResource("MutedBrush"), Margin = new Thickness(0, 0, 0, 8) };
        root.Children.Add(pageCount);
        RenderTweakList(root, list);
        MainContent.Content = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto, Content = root };
    }

    private Brush CategoryAccent(string category)
    {
        var hex = category switch
        {
            "FPS Tweaks" => "#A855F7",
            "Network Tweaks" => "#06B6D4",
            "Fortnite Tweaks" => "#F59E0B",
            "Windows Tweaks" => "#3B82F6",
            "GPU Tweaks" => "#EF4444",
            "CPU Tweaks" => "#F97316",
            "RAM Tweaks" => "#22C55E",
            "Storage Tweaks" => "#14B8A6",
            "System Tweaks" => "#8B5CF6",
            "Latency / Ping" => "#EC4899",
            _ => "#8B5CF6"
        };
        return (Brush)new BrushConverter().ConvertFromString(hex)!;
    }

    private void RenderTweakList(StackPanel root, List<Tweak> list)
    {
        while (root.Children.Count > 4) root.Children.RemoveAt(4);
        var query = searchBox?.Text ?? "";
        if (query == "Search tweaks...") query = "";
        var filtered = list.Where(t => string.IsNullOrWhiteSpace(query) || t.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || t.Description.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
        if (pageCount != null) pageCount.Text = $"Showing {filtered.Count} of {list.Count}  •  {list.Count} total in this category";

        foreach (var tweak in filtered)
        {
            var row = new Grid();
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var text = new StackPanel();
            text.Children.Add(new TextBlock { Text = tweak.Name, FontSize = 15, FontWeight = FontWeights.SemiBold, Foreground = Brushes.White });
            text.Children.Add(new TextBlock { Text = tweak.Description, TextWrapping = TextWrapping.Wrap, Foreground = (Brush)FindResource("MutedBrush"), FontSize = 12, Margin = new Thickness(0, 4, 12, 0) });
            Grid.SetColumn(text, 0);
            row.Children.Add(text);

            var toggle = new CheckBox { Style = (Style)FindResource("DvrkToggle"), IsChecked = state.TryGetValue(Key(currentCategory, tweak), out var v) && v, VerticalAlignment = VerticalAlignment.Center };
            toggle.Checked += (_, _) =>
            {
                state[Key(currentCategory, tweak)] = true;
                StateStore.Save(state);
                var result = TweakService.Set(tweak.ActionId, true);
                if (!string.IsNullOrWhiteSpace(result))
                    Status(result);
            };
            toggle.Unchecked += (_, _) =>
            {
                state[Key(currentCategory, tweak)] = false;
                StateStore.Save(state);
                var result = TweakService.Set(tweak.ActionId, false);
                if (!string.IsNullOrWhiteSpace(result))
                    Status(result);
            };
            Grid.SetColumn(toggle, 1);
            row.Children.Add(toggle);
            var tweakCard = Card(row);
            var accent = CategoryAccent(currentCategory);
            tweakCard.BorderBrush = accent;
            tweakCard.Background = new SolidColorBrush(Color.FromArgb(235, 18, 18, 27));
            root.Children.Add(tweakCard);
        }
    }

    private void Status(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
            MessageBox.Show(this, message, "DVRK Tweaks", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void ApplyCategory(string category, List<Tweak> list)
    {
        var results = new List<string>();
        foreach (var tweak in list.Where(t => state.TryGetValue(Key(category, t), out var v) && v))
        {
            results.Add($"{tweak.Name}: {TweakService.Apply(tweak.ActionId)}");
        }
        StateStore.Save(state);
        MessageBox.Show(results.Count == 0 ? "No tweaks are enabled." : string.Join(Environment.NewLine, results), "DVRK Tweaks", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private static string Key(string category, Tweak tweak) => category + "|" + tweak.Name;

    private void ShowPS5()
    {
        var root = PageHeader("PS5 Optimisation", "Practical checks and settings to work through — no fake PC-style switches.");
        root.Children.Add(Card(new TextBlock { Text = "PS5 optimisation is different from Windows tweaking. These are real settings and checks you can do on the console, display and network.", Foreground = Brushes.White, TextWrapping = TextWrapping.Wrap, FontSize = 14 }));
        foreach (var item in TweakCatalog.PS5Checklist)
        {
            var check = new CheckBox { Content = item, Foreground = Brushes.White, FontSize = 14, Margin = new Thickness(0, 2, 0, 2) };
            root.Children.Add(Card(check));
        }
        MainContent.Content = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto, Content = root };
    }

    private void ShowMonitor()
    {
        var root = PageHeader("System Monitor", "Quick access to Windows monitoring tools.");
        root.Children.Add(Card(new StackPanel
        {
            Children =
            {
                new TextBlock { Text = "LIVE TOOLS", FontSize = 17, FontWeight = FontWeights.Bold, Foreground = Brushes.White },
                ActionButton("TASK MANAGER", () => TweakService.Apply("taskmgr")),
                ActionButton("RESOURCE MONITOR", () => TweakService.Apply("resmon")),
                ActionButton("SYSTEM INFORMATION", () => TweakService.Apply("msinfo")),
                ActionButton("NETWORK TEST", () => TweakService.Apply("ping_test"))
            }
        }));
        MainContent.Content = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto, Content = root };
    }

    private void ShowSettings()
    {
        var root = PageHeader("Settings", "Local profile and application controls.");
        var reset = new Button { Content = "RESET ALL SAVED TOGGLES", Width = 220, Background = new SolidColorBrush(Color.FromRgb(90, 35, 45)) };
        reset.Click += (_, _) => { state.Clear(); StateStore.Save(state); MessageBox.Show("Saved toggle profile reset."); };
        root.Children.Add(Card(new StackPanel
        {
            Children =
            {
                new TextBlock { Text = "PROFILE", FontSize = 17, FontWeight = FontWeights.Bold, Foreground = Brushes.White },
                new TextBlock { Text = "DVRK remembers your selected switches locally. Applying a switch is separate from selecting it.", Foreground = (Brush)FindResource("MutedBrush"), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 7, 0, 15) },
                reset
            }
        }));
        MainContent.Content = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto, Content = root };
    }

    private void OpenUrl(string url) => Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
    private void Website(object? sender, RoutedEventArgs e) => OpenUrl("https://dvrk-tweaks.github.io/dvrk-tweaks/");
    private void Discord(object? sender, RoutedEventArgs e) => OpenUrl("https://discord.gg/8sD2CvxMk");
    private void Youtube(object? sender, RoutedEventArgs e) => OpenUrl("https://www.youtube.com/channel/UC34tB1hro48HY9pvlq3IIxg");
    private void Back(object? sender, RoutedEventArgs e)
    {
        if (history.Count > 0) Go(history.Pop());
        else Go("Home");
    }
    private void Exit(object? sender, RoutedEventArgs e) => Close();
}
