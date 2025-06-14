using CozyHaven.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CozyHaven.Views
{
    public partial class EmployeeAccountView : Window
    {
        private readonly EmployeeViewModel _viewModel = new EmployeeViewModel();

        public EmployeeAccountView()
        {
            InitializeComponent();
            DataContext = _viewModel;
            _viewModel.CloseAction = Close;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void OpenCalendar_Click(object sender, RoutedEventArgs e)
        {
            CalendarPopup.IsOpen = true;
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            CalendarPopup.IsOpen = false;
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            string projectRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"));
            string imagesFolder = Path.Combine(projectRoot, "Images");

            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg",
                InitialDirectory = imagesFolder
            };

            if (openFileDialog.ShowDialog() == true)
            {
                if (DataContext is EmployeeViewModel vm)
                {
                    string fullPath = openFileDialog.FileName;

                    if (!File.Exists(fullPath))
                    {
                        MessageBox.Show("Odabrana slika ne postoji.");
                        return;
                    }

                    string relativePath = Path.GetRelativePath(projectRoot, fullPath)
                                             .Replace("\\", "/");

                    vm.ProfilePicturePath = relativePath;
                }
            }
        }
    }
}
