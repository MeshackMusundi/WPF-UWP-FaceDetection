using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace FaceDetection.Converters
{
    public class BitmapToBitmapSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return null;

            using var bitmap = (Bitmap)value;
            using var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Bmp);
            stream.Position = 0;
            var bmpImg = new BitmapImage();
            bmpImg.BeginInit();
            bmpImg.CacheOption = BitmapCacheOption.OnLoad;
            bmpImg.StreamSource = stream;
            bmpImg.EndInit();
            bmpImg.Freeze();
            return bmpImg;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
