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
        //це запис розширень які будуть доступні, щоб додавати
        private static readonly string[] Extensions =
            {".jpg", ".jpeg", ".png", ".bmp", ".gif"}; 
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            string imageUrl = txtUrl.Text; //беремо введене користувачем посилання з txtUrl
            //"https://src.zakaz.atbmarket.com/prodwebp/catalog_product_gal/i33/catalog_product_gal_101973.webp";
            
            string dir = "images"; //називаємо папку, де буде 
            Directory.CreateDirectory(dir); //створення папки в якій будуть зберігатись наші фото

            try
            {
                HttpClient client = new HttpClient(); //створюю http клієнта
                var buffer = client.GetByteArrayAsync(imageUrl).Result; //скачую файл, як масив байтів
                string fileName = Guid.NewGuid().ToString() + ".jpg"; //унікальне ім'я файлу з розширенням
                var imageSave = Path.Combine(dir, fileName); //складаю шлях
                File.WriteAllBytes(imageSave, buffer); //записуємо байти на диск
                MessageBox.Show("Файл успішно збережено! " + imageSave); //виводимо повідомлення
            }
            catch (Exception ex) //якщо зловить помилку, то просить вказати шлях
            {
                MessageBox.Show("Вкажіть шлях");
            }
        }

        
        private void ReadDataFolder(string path)
        {
            var files = Directory.GetFiles(path) //отримуємо всі файли з папки, яка передана в path
                .Where(f => Extensions.Contains(Path.GetExtension(f).ToLower())); //фільтруємо файли по розширенням, які вказані в Extensions

            var images = new List<ImageItem>(); //створюємо список об'єктів, які будуть містити інформацію про фото
            //проходимось по кожному файлу в папці
            foreach (var file in files)
            {
                var dateCration = File.GetCreationTime(file); //отримую дату створення
                var fileSize = File.ReadAllBytes(file).Length; //отримую розмір файлу в байтах
                var fileName = Path.GetFileName(file); //отримую ім'я файлу з повного шляху

                var bitmap = new BitmapImage(); //об'єкт який буде містити фото
                bitmap.BeginInit();//початок ініціалізації
                bitmap.UriSource = new Uri(file); //вказуємо шлях до фото
                bitmap.DecodePixelWidth = 300; //ширина фото
                bitmap.CacheOption = BitmapCacheOption.OnLoad; //керування кешом
                bitmap.EndInit(); //закінчення ініціалізації
                images.Add(new ImageItem //стврорюємо новий об'єкт  і додаємо його в список images
                {
                    Image = bitmap, //вказуємо фото
                    FileName = fileName, //вказуємо ім'я файлу
                    FileSize = fileSize / 1024, //вказуємо розмір файлу в КБ
                    DateCreated = dateCration //вказуємо дату створення файлу
                });

            }
            GalleryList.ItemsSource = images; //вказуємо джерело даних, який буде відображати фото
        }

        private void btnReadImages_Click(object sender, RoutedEventArgs e) 
        {
            string currentDirectorry = Directory.GetCurrentDirectory(); //поточне розташування програми
            string folder = Path.Combine(currentDirectorry, "images"); //складаємо шлях до папки images
            ReadDataFolder(folder); //читаємо дані з папки images

        }
    }
}