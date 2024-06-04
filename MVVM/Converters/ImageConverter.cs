
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace rinconLosCuatroTPVDesktop.MVVM.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int[] imageData)
            {
                if (imageData != null && imageData.Length > 0)
                {
                    byte[] byteArray = new byte[imageData.Length * sizeof(int)];

                    for (int i = 0; i < imageData.Length; i++)
                    { 
                    
                        byteArray[i] = (byte)imageData[i];
                    }

                    String binary = System.Convert.ToBase64String(byteArray);
                    
                    MemoryStream ms = new MemoryStream(System.Convert.FromBase64String(binary));
                    ImageSource imageSource = ImageSource.FromStream(() => ms);

                    return imageSource;

                }  
            }

            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
