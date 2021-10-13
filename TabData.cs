using System;
using System.Collections.Generic;
using System.Drawing;

namespace BeefThemeEditor
{
	class TabData
	{
		public enum DataType : byte
		{
			toml, png
		}

		public string Name;
		public string Filename;
		public DataType Datatype;
		public int Scale = 1;

		public List<RowData> rows = new List<RowData>();

		public TabData(string name, string filename, DataType type)
		{
			Name = name;
			Filename = filename;
			Datatype = type;
		}

		public bool savePng(string fileName = null)
		{
			Bitmap orig = new Bitmap(400 * Scale, 160 * Scale);
			try
			{
				if (rows.Count == 0) return false;
				foreach (RowData rd in rows)
				{
					int x1 = rd.X;
					for (int x = 0; x < 20 * Scale; x++)
					{
						int y1 = rd.Y;
						for (int y = 0; y < 20 * Scale; y++)
						{
							Color c;
							if (rd.Updated) c = rd.NewImg.GetPixel(x, y);
							else c = rd.CurrImg.GetPixel(x, y);
							orig.SetPixel(x1, y1, c);
							y1++;
						}
						x1++;
					}
					rd.Updated = false;
				}
				if (!string.IsNullOrEmpty(fileName)) Filename = fileName;
				orig.Save(Filename);
				orig.Dispose();
				return true;
			} catch { }
			orig.Dispose();
			return false;
		}
	}
}
