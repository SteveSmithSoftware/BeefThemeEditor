using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using TOML;

namespace BeefThemeEditor
{
	public partial class Form1 : Form
	{
		TomlTable table;
		List<TabData> tabs = new List<TabData>();
		TabPage currTab;
		TabData currTD;
		RowData currRow;
		int currIx=-1;

		DataGridView currDgv;
		DataGridView.HitTestInfo hti;
		Bitmap copyBM=null;

		enum OpenType : byte
		{
			None,
			Theme, Toml, Png
		}
		OpenType oType;

		public Form1()
		{
			InitializeComponent();
			OpenToml.MouseUp += toolStrip_MouseUp;
			OpenTheme.MouseUp += toolStrip_MouseUp;
			OpenPNG.MouseUp += toolStrip_MouseUp;
			Save.MouseUp += toolStrip_MouseUp;
			SaveAs.MouseUp += toolStrip_MouseUp;
			Exit.MouseUp += toolStrip_MouseUp;
			folderBrowserDialog1.ShowNewFolderButton = false;
			string dir = Utility.getConfigString("WorkingDirectory", Directory.GetCurrentDirectory());
			folderBrowserDialog1.SelectedPath = dir;
			openFileDialog1.InitialDirectory = dir;
			openFileDialog1.FileName = "theme.toml";
			tabControl1.Controls.Remove(tabPage1);
			tabControl1.MouseUp += tabPage_MouseUp;

		}

		void dataError(object sender, DataGridViewDataErrorEventArgs e)	{	}

		void toolStrip_MouseUp(object sender, MouseEventArgs e)
		{
			ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
			errMsg.Text = "";
			switch (tsmi.Name)
			{
				case "OpenTheme":
					{
						if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
						{
							Cursor = Cursors.WaitCursor;
							cleanUp();
							string dir = folderBrowserDialog1.SelectedPath;
							label1.Text = getTheme(dir);
							string[] files = Directory.GetFiles(dir);
							int fileCnt = 0;
							foreach (string file in files)
							{
								switch (Path.GetExtension(file).ToLower())
								{
									case ".toml":
										if (processToml(file)) fileCnt++;
										break;
									case ".png":
										if (processPng(file)) fileCnt++;
										break;
									default:
										break;
								}
							}
							if (fileCnt == 0)
							{
								errMsg.Text = "No valid files found to process";
							} else
							{
								oType = OpenType.Theme;
							}
						}
					}
					break;
				case "OpenToml":
					{
						openFileDialog1.Filter = "Toml Files (*.toml)|*.toml";
						if (openFileDialog1.ShowDialog() == DialogResult.OK)
						{
							Cursor = Cursors.WaitCursor;
							openFileDialog1.InitialDirectory = Path.GetDirectoryName(openFileDialog1.FileName);
							cleanUp();
							label1.Text = getTheme(Path.GetDirectoryName(openFileDialog1.FileName));
							if (processToml(openFileDialog1.FileName))
							{
								oType = OpenType.Toml;
							}
						}
					}
					break;
				case "OpenPNG":
					{
						openFileDialog1.Filter = "Png Files (*.png)|*.png";
						if (openFileDialog1.ShowDialog() == DialogResult.OK)
						{
							Cursor = Cursors.WaitCursor;
							openFileDialog1.InitialDirectory = Path.GetDirectoryName(openFileDialog1.FileName);
							cleanUp();
							label1.Text = getTheme(Path.GetDirectoryName(openFileDialog1.FileName));
							if (processPng(openFileDialog1.FileName))
							{
								oType = OpenType.Png;
							}
						}
					}
					break;
				case "Save":
					switch (oType)
					{
						case OpenType.Theme:
							saveTheme();
							break;
						case OpenType.Toml:
							saveToml();
							break;
						case OpenType.Png:
							currTD.savePng();
							break;
						default:
							errMsg.Text = "Nothing Open to Save";
							break;
					}
					break;
				case "SaveAs":
					switch (oType)
					{
						case OpenType.Theme:
							if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
							{
								Cursor = Cursors.WaitCursor;
								saveTheme(folderBrowserDialog1.SelectedPath);
							}
							break;
						case OpenType.Toml:
							{
								saveFileDialog1.Filter = "Toml Files (*.toml)|*.toml";
								saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(currTD.Filename);
								if (saveFileDialog1.ShowDialog() == DialogResult.OK)
								{
									Cursor = Cursors.WaitCursor;
									foreach (TabData td1 in tabs)
									{
										td1.Filename = saveFileDialog1.FileName;
									}
									saveToml();
								}
							}
							break;
						case OpenType.Png:
							{
								saveFileDialog1.Filter = "Png Files (*.png)|*.png";
								saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(currTD.Filename);
								if (saveFileDialog1.ShowDialog() == DialogResult.OK)
								{
									Cursor = Cursors.WaitCursor;
									currTD.savePng(saveFileDialog1.FileName);
								}
							}
							break;
						default:
							errMsg.Text = "Nothing Open to Save";
							break;
					}
					break;
				case "Exit":
					Application.Exit();
					break;
				default:
					break;
			}
			Cursor = Cursors.Default;
		}

		string getTheme(string dir)
		{
			string[] sa = dir.Split('\\');
			return sa[sa.Length - 1];
		}

		void tabPage_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
					ContextMenu m = new ContextMenu();
					m.MenuItems.Add(new MenuItem("Delete", deleteTabPage));
					m.Show(currDgv, new Point(e.X, e.Y));
			}
		}

		DataGridView makeDGV(bool usedIn)
		{
			DataGridView dgv = new DataGridView();
			dgv.Dock = DockStyle.Fill;
			dgv.RowHeadersVisible = false;
			dgv.AllowUserToDeleteRows = false;
			dgv.AllowUserToAddRows = false;
			dgv.CellContentClick += new DataGridViewCellEventHandler(dataGridView_CellContentClick);
			dgv.MouseClick += new MouseEventHandler(dataGridView_MouseClick);
			dgv.Scroll += new ScrollEventHandler(dataGridView_Scroll);
			dgv.DataError += new DataGridViewDataErrorEventHandler(dataError);
			dgv.CellFormatting += new DataGridViewCellFormattingEventHandler(cellFormatting);
			dgv.CellEndEdit += new DataGridViewCellEventHandler(dataGridView_Edit);

			dgv.Columns.Add(makeTextColumn("Description",300));
			dgv.Columns.Add(makeTextColumn("Hex Colour"));
			dgv.Columns.Add(makeImageColumn(""));
			dgv.Columns.Add(makeTextColumn("New Hex Colour"));
			dgv.Columns.Add(makeImageColumn(""));
			if (usedIn) dgv.Columns.Add(makeTextColumn("Comment", 500, true));
			return dgv;
		}

		DataGridViewTextBoxColumn makeTextColumn(string name, int width = 100, bool fill = false)
		{
			DataGridViewTextBoxColumn tb = new DataGridViewTextBoxColumn();
			tb.SortMode = DataGridViewColumnSortMode.NotSortable;
			tb.HeaderText = name;
			tb.Width = width;
			if (fill) tb.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			return tb;
		}

		DataGridViewImageColumn makeImageColumn(string name)
		{
			DataGridViewImageColumn ic = new DataGridViewImageColumn();
			ic.SortMode = DataGridViewColumnSortMode.NotSortable;
			ic.HeaderText = name;
			ic.Width = 32;
			return ic;
		}

		DataGridViewTextBoxCell makeTextCell(string value)
		{
			DataGridViewTextBoxCell tb = new DataGridViewTextBoxCell();
			tb.Value = value;
			return tb;
		}

		DataGridViewImageCell makeImageCell(Bitmap value)
		{
			DataGridViewImageCell ic = new DataGridViewImageCell();
			ic.Value = value;
			return ic;
		}

		DataGridViewComboBoxCell makeComboCell(string[] values)
		{
			DataGridViewComboBoxCell cb = new DataGridViewComboBoxCell();
			foreach (string s in values)
			{
				cb.Items.Add(s);
			}
			if (values.Length>0) cb.Value = values[0];
			return cb;
		}

		void dataGridView_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				colorEditor1.Visible = false;
				hti = currDgv.HitTest(e.X, e.Y);
				int row = hti.RowIndex;
				int col = hti.ColumnIndex;

				if (row >= 0)
				{
					currRow = currTD.rows[row];
					ContextMenu m = new ContextMenu();
					m.MenuItems.Add(new MenuItem("Copy", copy_Click));
					m.MenuItems.Add(new MenuItem("Paste", paste_Click));
					if (currTD.Datatype == TabData.DataType.png)
					{
						m.MenuItems.Add(new MenuItem("Edit", edit_Click));
					}
					if (currRow.Updated) m.MenuItems.Add(new MenuItem("Clear", clear_Click));
					if (currTD.Datatype == TabData.DataType.toml)
					{
						m.MenuItems.Add(new MenuItem("Delete", delete_Click));
					}

					m.Show(currDgv, new Point(e.X, e.Y));
				}

			}
		}

		void clear_Click(object sender, System.EventArgs e)
		{
			int row = hti.RowIndex;
			int col = hti.ColumnIndex;
			errMsg.Text = "";
			currRow.Updated = false;
			currRow.NewHexValue = null;
			currRow.NewImg = null;
			currDgv[3, row].Value = string.Empty;
			Bitmap bm;
			if (currTD.Datatype == TabData.DataType.toml) {
				bm = new Bitmap(32, 32);
			} else
			{
				bm = new Bitmap(20 * currTD.Scale, 20 * currTD.Scale);
			}
			currDgv[4, row].Value = bm;
		}

		void copy_Click(object sender, System.EventArgs e)
		{
			int row = hti.RowIndex;
			int col = hti.ColumnIndex;
			switch (col)
			{
				case 0:
					Clipboard.SetText(currRow.Name);
					break;
				case 1:
					Clipboard.SetText(currRow.HexValue);
					break;
				case 3:
					if (currRow.Updated) Clipboard.SetText(currRow.NewHexValue);
					break;
				case 5:
					Clipboard.SetText(currRow.Comment);
					break;
				default:
					if (currRow.Updated) copyBM = currRow.NewImg;
					else copyBM = currRow.CurrImg;
					break;
			}
		}

		void paste_Click(object sender, System.EventArgs e)
		{
			int row = hti.RowIndex;
			int col = hti.ColumnIndex;
			errMsg.Text = "";
			switch (col)
			{
				case 3:
					if (Clipboard.ContainsText())
					{
						string hexVal = Clipboard.GetText();
						try
						{
							int c = Convert.ToInt32(hexVal, 16);

							currDgv[col, row].Value = hexVal;
							currRow.NewHexValue = hexVal;
							currRow.Updated = true;
							Color color;
							currRow.NewImg = makeBitmap(hexVal, currTD.Scale, out color);
							currDgv[4, row].Value = currRow.NewImg;
						}
						catch
						{
							errMsg.Text = "Clipboard does not contain a hex Color value";
						}
					}
					else
					{
						errMsg.Text = "Cannot paste from Clipboard";
					}
					break;
				case 5:
					if (Clipboard.ContainsText())
					{
						currDgv[col, row].Value = Clipboard.GetText();
						currRow.Comment = Clipboard.GetText();
						currRow.Updated = true;
					}
					else
					{
						errMsg.Text = "No Text in Clipboard";
					}
					break;
				default:
					if (currTD.Datatype == TabData.DataType.png && copyBM != null)
					{
						Bitmap bm = new Bitmap(copyBM.Width, copyBM.Height);
						for (int x = 0; x < bm.Width; x++)
						{
							for (int y=0;y<bm.Height;y++)
							{
								bm.SetPixel(x, y, copyBM.GetPixel(x, y));
							}
						}
						
						Bitmap bm1 = Utility.ResizeImage(bm, 20 * currTD.Scale, 20 * currTD.Scale);
						currRow.NewImg = bm1;
						currDgv[4, row].Value = bm1;

						Dictionary<int, Color> colDict = new Dictionary<int, Color>();
						for (int x=0;x<bm1.Width;x++)
						{
							for (int y=0;y<bm1.Height;y++)
							{
								Color color = bm1.GetPixel(x, y);
								if (color.ToArgb().ToString("x").PadLeft(8, '0').StartsWith("00"))
								{
									//color = Color.FromArgb(Convert.ToInt32("00ffffff", 16));
								}
								else
								{
									int col1 = color.ToArgb();
									if (!colDict.ContainsKey(col1))
									{
										colDict.Add(col1, color);
									}
								}
							}
						}
						int[] colvals = colDict.Keys.ToArray();
						string[] sa = new string[colvals.Length];
						for (int k = 0; k < colvals.Length; k++)
						{
							sa[k] = colvals[k].ToString("x").PadLeft(8, '0');
						}

						if (colvals.Length == 1)
						{
							currDgv[3,row] = makeTextCell(sa[0]);
						}
						else
						{
							//currDgv[3, row] = makeComboCell(sa);
							currDgv[3, row] = makeTextCell(string.Empty);
						}

						currRow.Updated = true;
					}
					else
					{
						errMsg.Text = "Cannot paste from Clipboard";
					}
					break;
			}
		}

		void edit_Click(object sender, System.EventArgs e)
		{
			int row = hti.RowIndex;
			int col = hti.ColumnIndex;
			Cursor = Cursors.WaitCursor;
			string fileName = Path.Combine(Directory.GetCurrentDirectory(), "temp.png");
			if (File.Exists(fileName)) File.Delete(fileName);
			if (col==4) currRow.NewImg.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
			else currRow.CurrImg.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
			string spawn = Utility.getConfigString("ImageEditor", "mspaint.exe");
			Process process = Process.Start(spawn, fileName);
			process.WaitForExit();
			process.Dispose();
			Bitmap bm = (Bitmap)Bitmap.FromFile(fileName);
			currRow.NewImg = new Bitmap(bm.Width, bm.Height);
			for (int x=0;x<bm.Width;x++)
			{
				for (int y=0;y<bm.Height;y++)
				{
					currRow.NewImg.SetPixel(x,y,bm.GetPixel(x, y));
				}
			}
			currRow.NewImg = Utility.ResizeImage(currRow.NewImg, 20 * currTD.Scale, 20 * currTD.Scale);
			bm.Dispose();
			currDgv[4, currIx].Value = currRow.NewImg;
			currRow.Updated = true;
			if (File.Exists(fileName)) File.Delete(fileName);
			Cursor = Cursors.Default;
		}

		void delete_Click(object sender, System.EventArgs e)
		{
			int row = hti.RowIndex;
			int col = hti.ColumnIndex;
			currDgv.Rows.RemoveAt(row);
			currTD.rows.RemoveAt(row);
			currRow = null;
			if  (currTD.rows.Count==0)
			{
				deleteTabPage(sender,e);
			}
		}

		void cellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == 2 || e.ColumnIndex == 4)
			{
				string hexValue = Utility.getConfigString("ImageBackground", "0xff707070");
				int col = Convert.ToInt32(hexValue,16);
				Color c = Color.FromArgb(col);
				e.CellStyle.BackColor = c;
			}
		}

		void cleanUp()
		{
			tabs.Clear();
			tabControl1.Controls.Clear();
			oType = OpenType.None;
		}

		bool processToml(string fileName)
		{
			Stream fileStream = File.Open(fileName, FileMode.Open);
			using (TextReader tr = new StreamReader(fileStream))
			{
				List<Exception> exceptions;
				bool ok = Toml.Parse(tr, out table, out exceptions);
				if (!ok)
				{
					errMsg.Text = fileName + " "+exceptions[0].Message;
					return false;
				}
				foreach (KeyValuePair<string, TomlNode> kvp1 in table.RawTable)
				{
					TomlTable tomlTable = (TomlTable)kvp1.Value;
					TabPage tp = tabPage1.Clone();
					tp.Text = kvp1.Key;
					tabControl1.Controls.Add(tp);
					TabData td = new TabData(kvp1.Key, fileName, TabData.DataType.toml);
					td.Scale = 0;
					tabs.Add(td);
					currTD = td;

					DataGridView dgv = makeDGV(true);
					tp.Controls.Add(dgv);

					foreach (KeyValuePair<string, TomlNode> kvp in tomlTable.RawTable)
					{
						int row = dgv.Rows.Add();
						DataGridViewRow dgr = dgv.Rows[row];
						dgr.Height = 32;
						dgr.Cells[0].Value = kvp.Key;
						string hexVal = kvp.Value.ToInlineToml().Replace("0x", "");
						dgr.Cells[1].Value = hexVal;
						Color color;
						Bitmap bm = makeBitmap(hexVal, 0, out color);
						dgr.Cells[2].Value = bm;
						dgr.Cells[3].Value = string.Empty;
						Bitmap bm1 = new Bitmap(32, 32);
						dgr.Cells[4].Value = bm1;

						dgr.Cells[5].Value = kvp.Value.Comment;

						RowData rd = new RowData();
						rd.Name = kvp.Key;
						rd.Comment = kvp.Value.Comment;
						rd.CurrImg = bm;
						rd.NewImg = bm1;
						rd.Set(hexVal, color);
						td.rows.Add(rd);
					}
				}
			}
			setTabPage(tabControl1.TabPages[0]);
			return true;
		}

		Bitmap makeBitmap(string hexVal, int scale, out Color color)
		{
			try
			{
				int size = (scale == 0) ? 32 : 20 * scale;
				int c = Convert.ToInt32(hexVal, 16);
				color = Color.FromArgb(c);
				Bitmap bm = new Bitmap(size, size);
				for (int x = 0; x < bm.Width; x++)
				{
					for (int y = 0; y < bm.Height; y++)
					{
						bm.SetPixel(x, y, color);
					}
				}
				return bm;
			} catch	{	}
			color = new Color();
			return null;
		}

		bool processPng(string fileName)
		{
			string file = Path.GetFileNameWithoutExtension(fileName);
			TabPage tp = tabPage1.Clone();
			tp.Text = file;
			tabControl1.Controls.Add(tp);
			TabData td = new TabData(file, fileName, TabData.DataType.png);
			tabs.Add(td);
			currTD = td;

			DataGridView dgv = makeDGV(false);
			tp.Controls.Add(dgv);

			Bitmap bm = (Bitmap)Image.FromFile(fileName);
			int fact = bm.Width / 400;
			td.Scale = fact;
			if (bm.Width % 400 != 0 || bm.Height % 160 != 0)
			{
				errMsg.Text = "Invalid PNG file " + fileName;
				return false;
			}
			Bitmap bm1 = new Bitmap(20*fact, 20*fact);
			Dictionary<int, Color> colDict = new Dictionary<int, Color>();
			int x1 = 0;
			int y1 = 0;
			int i = 0;
			for (int h = 0; h < bm.Height; h += (20*fact))
			{
				for (int x = 0; x < bm.Width; x++)
				{
					y1 = 0;
					for (int y = h; y < h + (20*fact); y++)
					{
						Color color = bm.GetPixel(x, y);
						if (color.ToArgb().ToString("x").PadLeft(8, '0').StartsWith("00"))
						{
							//color = Color.FromArgb(Convert.ToInt32("00ffffff", 16));
						}
						else
						{
							int col = color.ToArgb();
							if (!colDict.ContainsKey(col))
							{
								colDict.Add(col, color);
							}
						}
						bm1.SetPixel(x1, y1, color);
						y1++;
					}
					x1++;

					if ((x + 1) % (20*fact) == 0)
					{
						DataGridViewRow dgr = new DataGridViewRow();
						RowData rd = new RowData();
						rd.X = x - (20 * fact)+1;
						rd.Y = h;

						dgr.Height = (20*fact)+12;

						dgr.Cells.Add(makeTextCell((i < (int)RowData.ImageIdx.COUNT) ? ((RowData.ImageIdx)i).ToString() : ""));

						int[] colvals = colDict.Keys.ToArray();
						string[] sa = new string[colvals.Length];
						for (int k = 0; k < colvals.Length; k++)
						{
							sa[k] = colvals[k].ToString("x").PadLeft(8,'0');
						}

						if (colvals.Length <2)
						{
							string s = string.Empty;
							if (colvals.Length == 1)
							{
								s = sa[0];
								rd.Set(s, colDict[colvals[0]]);
							}
							dgr.Cells.Add(makeTextCell(s));
						} else
						{
							//dgr.Cells.Add(makeComboCell(sa));
							dgr.Cells.Add(makeTextCell(string.Empty));
						}

						dgr.Cells.Add(makeImageCell(bm1));
						dgv.Columns[2].Width = bm1.Width + 12;

						dgr.Cells.Add(makeTextCell(string.Empty));


						Bitmap bm2 = new Bitmap(20*fact, 20*fact);
						dgr.Cells.Add(makeImageCell(bm2));
						dgv.Columns[4].Width = bm2.Width + 12;

						int row = dgv.RowCount;
						dgv.Rows.Add(dgr);

						rd.CurrImg = bm1;
						rd.NewImg = bm2;
						currTD.rows.Add(rd);
						colDict.Clear();

						bm1 = new Bitmap(20*fact, 20*fact);
						x1 = 0;
						i++;
					}
				}
			}
			setTabPage(tabControl1.TabPages[0]);
			return true;
		}

		void saveTheme(string dir=null)
		{
			if (!string.IsNullOrEmpty(dir))
			{
				foreach (TabData td in tabs)
				{
					string file = Path.GetFileName(td.Filename);
					if (td.Datatype == TabData.DataType.toml)
					{
						td.Filename = Path.Combine(dir, file);
					} else
					{
						td.savePng(Path.Combine(dir, file));
					}
				}
			}
			else
			{
				foreach (TabData td in tabs)
				{
					if (td.Datatype== TabData.DataType.png) td.savePng();
				}
			}
			saveToml();
		}

		void saveToml()
		{
			List<string> output = new List<string>();
			bool first = true;
			string outFile=string.Empty;
			foreach (TabData td in tabs)
			{
				if (td.rows.Count == 0) continue;
				if (td.Datatype == TabData.DataType.toml)
				{
					if (first)
					{
						outFile = td.Filename;
						first = false;
					} else
					{
						if (td.Filename != outFile)
						{
							File.WriteAllLines(outFile, output.ToArray());
							output.Clear();
							outFile = td.Filename;
						}
					}
					output.Add("[" + td.Name + "]");
					foreach (RowData rd in td.rows)
					{
						string hexVal = (string.IsNullOrEmpty(rd.NewHexValue)) ? rd.HexValue : rd.NewHexValue;
						output.Add(rd.Name + " = 0x" + hexVal + " #" + rd.Comment);
					}
					output.Add("");
				}
			}
			File.WriteAllLines(outFile, output.ToArray());
		}

		void dataGridView_Edit(object sender, DataGridViewCellEventArgs e)
		{
			colorEditor1.Visible = false;
			currIx = e.RowIndex;
			currRow = currTD.rows[e.RowIndex];
			DataGridViewRow dgr = currDgv.Rows[e.RowIndex];
			DataGridViewCell dgc = dgr.Cells[e.ColumnIndex];
			if (e.ColumnIndex==3)
			{
				Color color;
				string hexValue = dgc.Value.ToString();
				Bitmap bm = makeBitmap(hexValue, currTD.Scale, out color);
				if (bm != null)
				{
					dgr.Cells[4].Value = bm;
					currRow.NewColour = color;
					currRow.NewHexValue = hexValue;
					currRow.Updated = true;
					currRow.NewImg = bm;
				} else
				{
					errMsg.Text = "Invalid Hex Color code";
				}
			}
			if (e.ColumnIndex==5)
			{
				currRow.Comment = dgc.Value.ToString();
			}
		}


		void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			colorEditor1.Visible = false;
			if (e.RowIndex==currIx)
			{
				return;
			}
			currIx = e.RowIndex;
			currRow = currTD.rows[e.RowIndex];
			if (string.IsNullOrEmpty(currRow.HexValue)) return;
			if (e.ColumnIndex == 2 || e.ColumnIndex == 4)
			{
				currDgv = (DataGridView)sender;
				DataGridViewRow dgr = currDgv.Rows[e.RowIndex];
				DataGridViewCell dgc = dgr.Cells[e.ColumnIndex];
				Rectangle r = currDgv.GetCellDisplayRectangle(4, e.RowIndex, true);
				Point p = r.Location;
				p.X += r.Width;
				p.Y += r.Height;


				string hexValue = dgr.Cells[e.ColumnIndex-1].Value.ToString();
				if (e.ColumnIndex==2 || currRow.NewHexValue==null)
				{
					currRow.NewColour = currRow.Colour;
					currRow.NewHexValue = hexValue;
					currRow.NewImg = currRow.CurrImg;
				}
				colorEditor1.Color = currRow.NewColour;
				colorEditor1.Location = p;
				colorEditor1.ShowAlphaChannel = true;
				colorEditor1.Visible = true;
				colorEditor1.BringToFront();
			}
		}

		string ColorToHex(Color color)
		{
			int c = color.ToArgb();
			return c.ToString("x").PadLeft(8, '0');
		}



		void colorEditor1_ColorChanged(object sender, EventArgs e)
		{
			Color color = colorEditor1.Color;
			DataGridViewRow dgr = currDgv.Rows[currIx];

			string hexVal = ColorToHex(color);
			dgr.Cells[3].Value = hexVal;

			Bitmap bm;
			if (currTD.Datatype== TabData.DataType.toml) bm = makeBitmap(hexVal, 0, out color);
			else
			{
				bm = new Bitmap(20 * currTD.Scale, 20 * currTD.Scale);
				for (int x=0;x<bm.Width;x++)
				{
					for (int y=0;y<bm.Height;y++)
					{
						Color col = currRow.NewImg.GetPixel(x, y);
						if (col == currRow.NewColour)
						{
							bm.SetPixel(x, y, color);
						}
					}
				}
			}
			currRow.NewHexValue = hexVal;
			currRow.NewColour = color;
			currRow.Updated = true;
			currRow.NewImg = bm;
			dgr.Cells[4].Value = bm;

		}

		void dataGridView_Scroll(object sender, ScrollEventArgs e)
		{
			
			if (colorEditor1.Visible)
			{
				DataGridViewRow dgr = currDgv.Rows[currIx];
				if (dgr.Displayed)
				{
					Rectangle r = currDgv.GetCellDisplayRectangle(4, currIx, true);
					Point p = r.Location;
					p.X += r.Width;
					p.Y += r.Height;
					colorEditor1.Location = p;
					colorEditor1.BringToFront();
				} else
				{
					colorEditor1.SendToBack();
				}
			}
		}

		void tabControl1_Selected(object sender, TabControlEventArgs e)
		{
			setTabPage(e.TabPage);
		}

		void setTabPage(TabPage tabPage)
		{
			if (tabs.Count>0)
			{
				currIx = -1;
				currTD = getTD(tabPage.Text);
				currDgv = (DataGridView)tabPage.Controls[0];
				currTab = tabPage;
				colorEditor1.Visible = false;
				if (!tabPage.Controls.Contains(colorEditor1))
					tabPage.Controls.Add(colorEditor1);
			}
		}

		void deleteTabPage(object sender, System.EventArgs e)
		{
			tabs.Remove(currTD);
			currTD = null;
			currRow = null;
			currIx = -1;
			currDgv = null;
			tabControl1.Controls.Remove(currTab);
			colorEditor1.Visible = false;
		}

		TabData getTD(string name)
		{
			foreach (TabData td in tabs)
			{
				if (td.Name == name) return td;
			}
			return null;
		}
	}
}
