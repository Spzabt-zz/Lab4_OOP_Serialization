using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace Serialization
{
    public partial class CreateWindow : Window
    {
        private readonly Performance _performance;
        private readonly Concert _concert;
        private Performer _performer;
        private readonly List<Performer> _performers;

        public CreateWindow(Performance performance, Concert concert)
        {
            InitializeComponent();
            _performance = performance;
            _concert = concert;

            var compositions = (Composition[]) Enum.GetValues(typeof(Composition));
            foreach (var composition in compositions)
            {
                CompositionComboBox.Items.Add(composition.ToString());
            }

            //_performers = new List<Performer>();
            _performers = Performer.ReadPerformers("_performers");

            foreach (var performer in _performers)
            {
                PerformerComboBox.Items.Add(performer?.ToString());
            }

            DatePicker.Text = _concert?.ConcertDate.ToString();
            PerformanceDurationTextBox.Text = _performance?.PerformanceDuration.ToString() ?? string.Empty;
            CompositionNameTextBox.Text = _performance?.NameOfComposition ?? string.Empty;
            OrganizationNameTextBox.Text = _concert?.OrganizationName;
        }

        private void AddPerformerButton_OnClick(object sender, RoutedEventArgs e)
        {
            _performer = new Performer();
            var addPerformerWindow = new AddPerformerWindow(_performer);
            if (addPerformerWindow.ShowDialog() == true)
            {
                PerformerComboBox.Items.Add(_performer.ToString());
                _performers.Add(_performer);
                Performer.WritePerformer("_performers", _performers);
            }
            else
                MessageBox.Show("Changes didn't save");
        }

        private void EditPerformerButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (PerformerComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Choose author");
                return;
            }

            var addPerformerWindow = new AddPerformerWindow(_performers[PerformerComboBox.SelectedIndex]);

            if (addPerformerWindow.ShowDialog() == true)
            {
                PerformerComboBox.Items[PerformerComboBox.SelectedIndex] =
                    _performers[PerformerComboBox.SelectedIndex].ToString();
                Performer.WritePerformer("_performers", _performers);
            }
            else
                MessageBox.Show("Changes didn't save");
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (CompositionComboBox.SelectedIndex < 0 || PerformerComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Choose composition and author");
                return;
            }

            if (PerformanceDurationTextBox.Text != string.Empty && CompositionNameTextBox.Text != string.Empty &&
                OrganizationNameTextBox.Text != string.Empty && DatePicker.Text != string.Empty)
            {
                _performance.Composition =
                    (Composition) Enum.Parse(typeof(Composition), CompositionComboBox.SelectedIndex.ToString());
                _performance.Performer = _performers[PerformerComboBox.SelectedIndex];
                _performance.PerformanceDuration =
                    int.TryParse(PerformanceDurationTextBox.Text, out var num) ? num : 60;
                _performance.NameOfComposition = CompositionNameTextBox.Text;
                _concert.OrganizationName = OrganizationNameTextBox.Text;
                _concert.ConcertDate = DateTime.Parse(DatePicker.ToString());
                _concert.Performances.Add(_performance);
                DialogResult = true;
            }
            else
                MessageBox.Show("All fields must be filled");
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}