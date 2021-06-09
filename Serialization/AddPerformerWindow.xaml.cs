using System;
using System.ComponentModel;
using System.Windows;

namespace Serialization
{
    public partial class AddPerformerWindow : Window
    {
        private readonly Performer _performer;

        public AddPerformerWindow(Performer performer)
        {
            InitializeComponent();
            _performer = performer;

            CompositorNameTextBox.Text = _performer?.Name ?? string.Empty;
            CompositorSurnameTextBox.Text = _performer?.Surname ?? string.Empty;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (CompositorNameTextBox.Text == string.Empty || CompositorSurnameTextBox.Text == string.Empty)
            {
                MessageBox.Show("All fields must be filled");
                return;
            }
            
            Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddPerformerWindow_OnClosing(object sender, CancelEventArgs e)
        {
            var messageBoxResult = MessageBox.Show("Save changes?", "Alert", MessageBoxButton.YesNo);
            if (messageBoxResult != MessageBoxResult.Yes || CompositorNameTextBox.Text == string.Empty ||
                CompositorSurnameTextBox.Text == string.Empty) return;
            _performer.Name = CompositorNameTextBox.Text;
            _performer.Surname = CompositorSurnameTextBox.Text;
            DialogResult = true;
        }
    }
}