using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace BeefThemeEditor
{
	public static class Utility
	{
		public static int getConfigInt(string token, int notFound = 0)
		{
			string text = ConfigurationManager.AppSettings[token];
			if (!string.IsNullOrEmpty(text))
			{
				int n = 0;
				bool ok = int.TryParse(text, out n);
				if (ok) return n;
			}
			return notFound;
		}

		public static string getConfigString(string token, string notFound = "")
		{
			string text = ConfigurationManager.AppSettings[token];
			if (!string.IsNullOrEmpty(text)) return text;
			return notFound;
		}

		public static T Clone<T>(this T controlToClone)
			where T : Control
		{
			PropertyInfo[] controlProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			T instance = Activator.CreateInstance<T>();

			foreach (PropertyInfo propInfo in controlProperties)
			{
				if (propInfo.CanWrite)
				{
					if (propInfo.Name != "WindowTarget")
					{
						object o = propInfo.GetValue(controlToClone);
						try
						{
							propInfo.SetValue(instance, o);
						}
						catch { }
					}
				}
			}

			return instance;
		}

		public static Bitmap ResizeImage(Image image, int width, int height)
		{
			if (image.Width == width && image.Height == height) return (Bitmap)image;
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}
	}
}
