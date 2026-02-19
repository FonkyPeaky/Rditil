using System;
using System.Windows;
using System.Windows.Input;

namespace Rditil.Views
{
    public partial class PinDialog : Window
    {
        private readonly string _expectedPin;
        private readonly int _maxAttempts;
        private readonly int _lockoutSeconds;

        private static int _failedAttempts;
        private static DateTime? _lockedUntilUtc;

        public PinDialog(string expectedPin, int maxAttempts, int lockoutSeconds)
        {
            InitializeComponent();
            _expectedPin = expectedPin ?? string.Empty;
            _maxAttempts = maxAttempts <= 0 ? 3 : maxAttempts;
            _lockoutSeconds = lockoutSeconds <= 0 ? 60 : lockoutSeconds;
            Loaded += (_, __) => PinBox.Focus();
            PreviewKeyDown += PinDialog_PreviewKeyDown;
            RefreshLockoutMessage();
        }

        private void PinDialog_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                Validate();
            }
            else if (e.Key == Key.Escape)
            {
                e.Handled = true;
                DialogResult = false;
                Close();
            }
        }

        private void RefreshLockoutMessage()
        {
            if (_lockedUntilUtc is null) return;

            var now = DateTime.UtcNow;
            if (now >= _lockedUntilUtc.Value)
            {
                _lockedUntilUtc = null;
                _failedAttempts = 0;
                ErrorText.Text = string.Empty;
                PinBox.IsEnabled = true;
                return;
            }

            var secondsLeft = (int)Math.Ceiling((_lockedUntilUtc.Value - now).TotalSeconds);
            ErrorText.Text = $"Trop d'essais. Réessaie dans {secondsLeft}s.";
            PinBox.IsEnabled = false;
        }

        private void Ok_Click(object sender, RoutedEventArgs e) => Validate();

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Validate()
        {
            RefreshLockoutMessage();
            if (!PinBox.IsEnabled)
                return;

            var pin = (PinBox.Password ?? string.Empty).Trim();
            if (pin == _expectedPin)
            {
                _failedAttempts = 0;
                _lockedUntilUtc = null;
                DialogResult = true;
                Close();
                return;
            }

            _failedAttempts++;
            var remaining = Math.Max(0, _maxAttempts - _failedAttempts);

            if (_failedAttempts >= _maxAttempts)
            {
                _lockedUntilUtc = DateTime.UtcNow.AddSeconds(_lockoutSeconds);
                RefreshLockoutMessage();
                return;
            }

            ErrorText.Text = $"PIN incorrect. Il te reste {remaining} essai(s).";
            PinBox.Password = string.Empty;
            PinBox.Focus();
        }
    }
}
