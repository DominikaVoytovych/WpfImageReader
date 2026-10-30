using Microsoft.Win32;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace WpfImageReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string[] Extensions =
            {".jpg", ".jpeg", ".png", ".bmp", ".gif"};
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            string imageUrl = txtUrl.Text;
            //"https://src.zakaz.atbmarket.com/prodwebp/catalog_product_gal/i33/catalog_product_gal_101973.webp";
            string dir = "images";
            Directory.CreateDirectory(dir);
            try
            {
                HttpClient client = new HttpClient();
                var buffer = client.GetByteArrayAsync(imageUrl).Result;
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                var imageSave = Path.Combine(dir, fileName);
                File.WriteAllBytes(imageSave, buffer);
                MessageBox.Show("Файл успішно збережено! " + imageSave);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Вкажіть шлях");
            }
        }

        
        private void ReadDataFolder(string path)
        {
            var files = Directory.GetFiles(path)
                .Where(f => Extensions.Contains(Path.GetExtension(f).ToLower()));

            var images = new List<ImageItem>();
            foreach (var file in files)
            {
                var dateCration = File.GetCreationTime(file);
                var fileSize = File.ReadAllBytes(file).Length;
                var fileName = Path.GetFileName(file);

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(file);
                bitmap.DecodePixelWidth = 300;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                images.Add(new ImageItem
                {
                    Image = bitmap,
                    FileName = fileName,
                    FileSize = fileSize / 1024,
                    DateCreated = dateCration
                });

            }
            GalleryList.ItemsSource = images;
        }

        private void btnReadImages_Click(object sender, RoutedEventArgs e)
        {
            string currentDirectorry = Directory.GetCurrentDirectory();
            string folder = Path.Combine(currentDirectorry, "images");
            ReadDataFolder(folder);

        }
    }
}