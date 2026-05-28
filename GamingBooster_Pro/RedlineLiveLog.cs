using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace GamingBooster_Pro
{
    internal enum LiveLogLevel
    {
        Info,
        Step,
        Success,
        Warning,
        Error,
        Muted
    }

    internal sealed class RedlineLiveLogController
    {
        public RichTextBox Box { get; }
        public TextBlock FooterStatus { get; }
        public TextBlock HeaderBadge { get; }

        private readonly bool _english;

        private RedlineLiveLogController(RichTextBox box, TextBlock footer, TextBlock badge, bool english)
        {
            Box = box;
            FooterStatus = footer;
            HeaderBadge = badge;
            _english = english;
        }

        public static (Border Card, RedlineLiveLogController Controller) Create(
            string title,
            string startMessage,
            bool english,
            double logHeight = 420)
        {
            Brush red = new SolidColorBrush(Color.FromRgb(237, 28, 56));
            Brush muted = new SolidColorBrush(Color.FromRgb(136, 152, 178));
            Brush cardBorder = new SolidColorBrush(Color.FromRgb(48, 58, 78));
            Brush logBg = new SolidColorBrush(Color.FromRgb(12, 16, 24));
            Brush text = new SolidColorBrush(Color.FromRgb(236, 240, 248));

            Border card = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(18, 24, 36)),
                BorderBrush = cardBorder,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(16),
                Padding = new Thickness(18)
            };

            Grid header = new Grid { Margin = new Thickness(0, 0, 0, 12) };
            header.ColumnDefinitions.Add(new ColumnDefinition());
            header.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            StackPanel titleStack = new StackPanel();
            titleStack.Children.Add(new TextBlock
            {
                Text = title,
                Foreground = text,
                FontSize = 15,
                FontWeight = FontWeights.UltraBold
            });
            titleStack.Children.Add(new TextBlock
            {
                Text = english ? "Activity output" : "Aktivitäts-Ausgabe",
                Foreground = muted,
                FontSize = 11,
                Margin = new Thickness(0, 3, 0, 0)
            });
            Grid.SetColumn(titleStack, 0);
            header.Children.Add(titleStack);

            TextBlock badge = new TextBlock
            {
                Text = english ? "Ready" : "Bereit",
                Foreground = muted,
                FontSize = 11,
                FontWeight = FontWeights.SemiBold,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(badge, 1);
            header.Children.Add(badge);

            RedlineLiveLogController ctrl = null!;
            Button clearBtn = new Button
            {
                Content = english ? "Clear" : "Leeren",
                Padding = new Thickness(12, 5, 12, 5),
                Background = new SolidColorBrush(Color.FromRgb(26, 32, 48)),
                Foreground = text,
                BorderBrush = cardBorder,
                BorderThickness = new Thickness(1),
                FontSize = 11,
                Cursor = System.Windows.Input.Cursors.Hand,
                Margin = new Thickness(8, 0, 0, 0)
            };
            clearBtn.Click += (_, _) => ctrl.Clear();

            RichTextBox box = new RichTextBox
            {
                Height = logHeight,
                IsReadOnly = true,
                IsUndoEnabled = false,
                Focusable = false,
                BorderThickness = new Thickness(0),
                Background = logBg,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                FontFamily = new FontFamily("Segoe UI, Arial"),
                FontSize = 13,
                Padding = new Thickness(12, 10, 12, 10)
            };

            TextBlock footer = new TextBlock
            {
                Text = english ? "Ready" : "Bereit",
                Foreground = muted,
                FontSize = 11,
                Margin = new Thickness(0, 12, 0, 0)
            };

            Grid headerRow2 = new Grid();
            headerRow2.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            headerRow2.ColumnDefinitions.Add(new ColumnDefinition());
            headerRow2.Children.Add(clearBtn);
            Grid.SetColumn(headerRow2, 1);

            StackPanel root = new StackPanel();
            root.Children.Add(header);
            root.Children.Add(box);
            root.Children.Add(footer);
            card.Child = root;

            ctrl = new RedlineLiveLogController(box, footer, badge, english);
            ctrl.Append(startMessage, LiveLogLevel.Muted);
            return (card, ctrl);
        }

        public void Clear()
        {
            Box.Document.Blocks.Clear();
            SetFooter(_english ? "Cleared" : "Geleert", LiveLogLevel.Muted);
        }

        public void Append(string message, LiveLogLevel? level = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Box.Document.Blocks.Add(new Paragraph(new Run(" ")) { Margin = new Thickness(0, 4, 0, 4) });
                return;
            }

            LiveLogLevel lv = level ?? Classify(message);
            string icon = lv switch
            {
                LiveLogLevel.Success => "✓",
                LiveLogLevel.Error => "✗",
                LiveLogLevel.Warning => "!",
                LiveLogLevel.Step => "›",
                LiveLogLevel.Muted => "·",
                _ => "•"
            };

            Paragraph p = new Paragraph { Margin = new Thickness(0, 2, 0, 4), LineHeight = 20 };
            p.Inlines.Add(new Run(icon + " ") { Foreground = BrushFor(lv), FontWeight = FontWeights.SemiBold });
            p.Inlines.Add(new Run(message) { Foreground = BrushFor(lv == LiveLogLevel.Muted ? LiveLogLevel.Info : lv) });
            Box.Document.Blocks.Add(p);
            Box.ScrollToEnd();
            SetFooter(message.Length > 64 ? message[..64] + "…" : message, lv);
        }

        public void SetHeaderBusy(bool busy)
        {
            HeaderBadge.Text = busy
                ? (_english ? "Running…" : "Läuft…")
                : (_english ? "Ready" : "Bereit");
            HeaderBadge.Foreground = busy
                ? new SolidColorBrush(Color.FromRgb(251, 146, 60))
                : new SolidColorBrush(Color.FromRgb(136, 152, 178));
        }

        private void SetFooter(string text, LiveLogLevel lv)
        {
            FooterStatus.Text = text;
            FooterStatus.Foreground = BrushFor(lv);
        }

        public static LiveLogLevel Classify(string text)
        {
            if (text.Contains("✅") || text.Contains("Erfolgreich", StringComparison.OrdinalIgnoreCase)
                || text.Contains("Successfully", StringComparison.OrdinalIgnoreCase)
                || text.Contains("Fertig", StringComparison.OrdinalIgnoreCase))
                return LiveLogLevel.Success;
            if (text.Contains("❌") || text.Contains("Fehlgeschlagen", StringComparison.OrdinalIgnoreCase)
                || text.Contains("failed", StringComparison.OrdinalIgnoreCase)
                || text.Contains("FEHLER", StringComparison.OrdinalIgnoreCase))
                return LiveLogLevel.Error;
            if (text.Contains("⚠") || text.Contains("Hinweis", StringComparison.OrdinalIgnoreCase)
                || text.Contains("Warning", StringComparison.OrdinalIgnoreCase)
                || text.Contains("NICHT", StringComparison.OrdinalIgnoreCase))
                return LiveLogLevel.Warning;
            if (text.StartsWith("=====", StringComparison.Ordinal) || text.StartsWith("Schritt", StringComparison.OrdinalIgnoreCase)
                || text.StartsWith("Step ", StringComparison.OrdinalIgnoreCase))
                return LiveLogLevel.Step;
            return LiveLogLevel.Info;
        }

        private static Brush BrushFor(LiveLogLevel lv) => lv switch
        {
            LiveLogLevel.Success => new SolidColorBrush(Color.FromRgb(74, 222, 128)),
            LiveLogLevel.Warning => new SolidColorBrush(Color.FromRgb(251, 146, 60)),
            LiveLogLevel.Error => new SolidColorBrush(Color.FromRgb(248, 113, 113)),
            LiveLogLevel.Step => new SolidColorBrush(Color.FromRgb(237, 28, 56)),
            LiveLogLevel.Muted => new SolidColorBrush(Color.FromRgb(100, 116, 139)),
            _ => new SolidColorBrush(Color.FromRgb(226, 232, 240))
        };
    }
}
