using CozyHaven.Models;
using CozyHaven.ViewModels;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace CozyHaven.Views
{
    public partial class AddProductView : Window
    {
        public AddProductView()
        {
            InitializeComponent();
        }

        public AddProductView(Product product)
        {
            InitializeComponent();
            DataContext = new AddProductViewModel(product); 
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

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            string projectRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"));
            string imagesFolder = Path.Combine(projectRoot, "Images");

            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg",
                InitialDirectory = Directory.Exists(imagesFolder) ? imagesFolder : Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string fullPath = openFileDialog.FileName;

                if (!File.Exists(fullPath))
                {
                    MessageBox.Show("Odabrana slika ne postoji.");
                    return;
                }

                string relativePath = Path.GetRelativePath(projectRoot, fullPath).Replace("\\", "/");

                if (DataContext is ViewModels.AddProductViewModel vm)
                {
                    vm.ImagePath = relativePath;
                }
            }
        }
    }
}
