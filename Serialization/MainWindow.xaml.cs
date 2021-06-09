using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Serialization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Performance _performance;
        private Concert _concert;
        private readonly List<Concert> _concerts;

        public MainWindow()
        {
            InitializeComponent();
            _concerts = Concert.ReadConcerts("_concerts");
            _concerts.ForEach(concert => { ConcertList.Items.Add(concert.ToString()); });
        }

        private void CreateBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _performance = new Performance();
            _concert = new Concert();
            var createWindow = new CreateWindow(_performance, _concert);
            if (createWindow.ShowDialog() == true)
            {
                ConcertList.Items.Add(_concert.ToString());
            }
            else
                MessageBox.Show("Changes didn't save");
        }

        private void EditInfoBtn_OnClick(object sender, RoutedEventArgs e)
        {
            int index = ConcertList.SelectedIndex;
            if (ConcertList.SelectedIndex < 0)
            {
                MessageBox.Show("Choose item");
                return;
            }

            var createWindow = new CreateWindow(_concert.Performances[index], _concert);

            if (createWindow.ShowDialog() == true)
            {
                ConcertList.Items[index] = _concert.ToString();
            }
            else
                MessageBox.Show("Changes didn't save");
        }

        private void DeleteDataBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            var result = MessageBox.Show("Save?", "Alert", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes) Concert.WriteConcert("_concerts", _concerts);
        }
    }
}