using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace WpfImageReader;

public class ImageItem
{
    public BitmapImage Image { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }      // у КБ
    public DateTime DateCreated { get; set; }
}
