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
		Dictionary<string,Dictionary<int, RowData>> rows = new Dictionary<string, Dictionary<int, RowData>>();
		Dictionary<int, RowData> currDict;
		RowData currRow;
		int currIx=-1;
		DataGridView currDgv;

		public Form1()
		{
			InitializeComponent();
			OpenToml.MouseUp += toolStrip_MouseUp;
			OpenTheme.MouseUp += toolStrip_MouseUp;
			OpenPNG.MouseUp += toolStrip_MouseUp;
			folderBrowserDialog1.ShowNewFolderButton = false;
			string dir = Utility.getConfigString("WorkingDirectory", Directory.GetCurrentDirectory());
			folderBrowserDialog1.SelectedPath = dir;
			openFileDialog1.InitialDirectory = dir;
			tabControl1.Controls.Remove(tabPage1);
		}

		void DataError(object sender, DataGridViewDataErrorEventArgs e)	{	}

		private void toolStrip_MouseUp(object sender, MouseEventArgs e)
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
							CleanUp();
							string dir = folderBrowserDialog1.SelectedPath;
							label1.Text = getTheme(dir);
							string[] files = Directory.GetFiles(dir);
							int fileCnt = 0;
							foreach (string file in files)
							{
								switch (Path.GetExtension(file).ToLower())
								{
									case ".toml":
										if (ProcessToml(file)) fileCnt++;
										break;
									case ".png":
										if (ProcessPng(file)) fileCnt++;
										break;
									default:
										break;
								}
							}
							if (fileCnt == 0)
							{
								errMsg.Text = "No valid files found to process";
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
							CleanUp();
							label1.Text = getTheme(Path.GetDirectoryName(openFileDialog1.FileName));
							ProcessToml(openFileDialog1.FileName);
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
							CleanUp();
							label1.Text = getTheme(Path.GetDirectoryName(openFileDialog1.FileName));
							ProcessPng(openFileDialog1.FileName);
						}
					}
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

		DataGridView makeDGV(bool usedIn)
		{
			DataGridView dgv = new DataGridView();
			dgv.Dock = DockStyle.Fill;
			dgv.RowHeadersVisible = false;
			dgv.AllowUserToDeleteRows = false;
			dgv.AllowUserToAddRows = false;
			dgv.CellContentClick += new DataGridViewCellEventHandler(dataGridView1_CellContentClick);
			dgv.MouseClick += new MouseEventHandler(dataGridView1_MouseClick);
			dgv.Scroll += new ScrollEventHandler(dataGridView1_Scroll);
			dgv.DataError += new DataGridViewDataErrorEventHandler(DataError);
			dgv.CellFormatting += new DataGridViewCellFormattingEventHandler(CellFormatting);

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

		void dataGridView1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				DataGridView.HitTestInfo hti = currDgv.HitTest(e.X, e.Y);
				int row = hti.RowIndex;
				int col = hti.ColumnIndex;

				if (row >= 0)
				{
					currRow = currDict[row];
					currIx = row;
					ContextMenu m = new ContextMenu();
					m.MenuItems.Add(new MenuItem("Copy", copy_Click));
					m.MenuItems.Add(new MenuItem("Paste", paste_Click));
					m.MenuItems.Add(new MenuItem("Edit", edit_Click));
					m.MenuItems.Add(new MenuItem("Delete"));

					m.Show(currDgv, new Point(e.X, e.Y));
				}

			}
		}

		void copy_Click(object sender, System.EventArgs e)
		{
			if (currRow.updated) Clipboard.SetImage(currRow.newImg);
			else Clipboard.SetImage(currRow.currImg);
		}

		void paste_Click(object sender, System.EventArgs e)
		{
			currRow.newImg = (Bitmap)Clipboard.GetImage();
			currDgv[4, currIx].Value = currRow.newImg;
			currRow.updated = true;
		}

		void edit_Click(object sender, System.EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			string fileName = Path.Combine(Directory.GetCurrentDirectory(), "temp.png");
			if (File.Exists(fileName)) File.Delete(fileName);
			if (currRow.updated) currRow.newImg.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
			else currRow.currImg.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
			string spawn = Utility.getConfigString("ImageEditor", "mspaint.exe");
			Process process = Process.Start(spawn, fileName);
			process.WaitForExit();
			process.Dispose();
			Bitmap bm = (Bitmap)Bitmap.FromFile(fileName);
			currRow.newImg = new Bitmap(bm.Width, bm.Height);
			for (int x=0;x<bm.Width;x++)
			{
				for (int y=0;y<bm.Height;y++)
				{
					currRow.newImg.SetPixel(x,y,bm.GetPixel(x, y));
				}
			}
			bm.Dispose();
			currDgv[4, currIx].Value = currRow.newImg;
			currRow.updated = true;
			if (File.Exists(fileName)) File.Delete(fileName);
			Cursor = Cursors.Default;
		}

		void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == 2 || e.ColumnIndex == 4)
			{
				string hexValue = Utility.getConfigString("ImageBackground", "0xff707070");
				int col = Convert.ToInt32(hexValue,16);
				Color c = Color.FromArgb(col);
				e.CellStyle.BackColor = c;
			}
		}

		void CleanUp()
		{
			rows.Clear();
			tabControl1.Controls.Clear();
		}

		bool ProcessToml(string fileName)
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
					rows.Add(kvp1.Key, new Dictionary<int, RowData>());
					currDict = rows[kvp1.Key];

					DataGridView dgv = makeDGV(true);
					tp.Controls.Add(dgv);

					foreach (KeyValuePair<string, TomlNode> kvp in tomlTable.RawTable)
					{
						int row = dgv.Rows.Add();
						DataGridViewRow dgr = dgv.Rows[row];
						dgr.Height = 32;
						dgr.Cells[0].Value = kvp.Key;
						dgr.Cells[1].Value = kvp.Value.ToInlineToml();
						Bitmap bm = new Bitmap(32, 32);
						Color color = System.Drawing.Color.FromArgb((int)((TomlInteger)kvp.Value).Value);
						for (int x = 0; x < 32; x++)
						{
							for (int y = 0; y < 32; y++)
							{
								bm.SetPixel(x, y, color);
							}
						}
						dgr.Cells[2].Value = bm;
						dgr.Cells[3].Value = string.Empty;
						Bitmap bm1 = new Bitmap(32, 32);
						dgr.Cells[4].Value = bm1;

						dgr.Cells[5].Value = kvp.Value.Comment;

						RowData rd = new RowData(fileName, RowData.Type.toml);
						rd.currImg = bm;
						rd.newImg = bm1;
						rd.Add(kvp.Value.ToInlineToml(), color);
						currDict.Add(row, rd);
						//tp.Refresh();
					}
				}
			}
			SetTabPage(tabControl1.TabPages[0]);
			return true;
		}

		bool ProcessPng(string fileName)
		{
			string file = Path.GetFileNameWithoutExtension(fileName);
			TabPage tp = tabPage1.Clone();
			tp.Text = file;
			tabControl1.Controls.Add(tp);
			rows.Add(file, new Dictionary<int, RowData>());
			currDict = rows[file];

			DataGridView dgv = makeDGV(false);
			tp.Controls.Add(dgv);

			Bitmap bm = (Bitmap)Image.FromFile(fileName);
			int fact = bm.Width / 400;
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
						System.Drawing.Color color = bm.GetPixel(x, y);
						if (color.ToArgb().ToString("x").PadLeft(8,'0').StartsWith("0"))
						{
							color = Color.FromArgb(Convert.ToInt32("00ffffff", 16));
						}
						bm1.SetPixel(x1, y1, color);
						int col = color.ToArgb();
						if (col.ToString("x").PadLeft(8,'0') != "00ffffff" && !colDict.ContainsKey(col))
						{
							colDict.Add(col, color);
						}
						y1++;
					}
					x1++;

					if ((x + 1) % (20*fact) == 0)
					{
						DataGridViewRow dgr = new DataGridViewRow();
						RowData rd = new RowData(fileName, RowData.Type.png);
						rd.scale = fact;

						dgr.Height = (20*fact)+12;

						dgr.Cells.Add(makeTextCell((i < (int)RowData.ImageIdx.COUNT) ? ((RowData.ImageIdx)i).ToString() : ""));

						int[] colvals = colDict.Keys.ToArray();
						string[] sa = new string[colvals.Length];
						for (int k = 0; k < colvals.Length; k++)
						{
							sa[k] = "0x" + colvals[k].ToString("x").PadLeft(8,'0');
						}

						if (colvals.Length==1)
						{
							dgr.Cells.Add(makeTextCell(sa[0]));
						} else
						{
							dgr.Cells.Add(makeComboCell(sa));
						}

						dgr.Cells.Add(makeImageCell(bm1));
						dgv.Columns[2].Width = bm1.Width + 12;

						if (colvals.Length == 1)
						{
							dgr.Cells.Add(makeTextCell(sa[0]));
						}
						else
						{
							dgr.Cells.Add(makeComboCell(sa));
						}

						Bitmap bm2 = new Bitmap(20*fact, 20*fact);
						dgr.Cells.Add(makeImageCell(bm2));
						dgv.Columns[4].Width = bm2.Width + 12;

						int row = dgv.RowCount;
						dgv.Rows.Add(dgr);

						foreach (KeyValuePair<int, Color> kvp in colDict)
						{
							rd.Add("0x" + kvp.Key.ToString("x").PadLeft(8, '0'), kvp.Value);
						}
						rd.currImg = bm1;
						rd.newImg = bm2;
						currDict.Add(row, rd);
						colDict.Clear();

						bm1 = new Bitmap(20*fact, 20*fact);
						x1 = 0;
						i++;
						//tp.Refresh();
					}
				}
			}
			SetTabPage(tabControl1.TabPages[0]);
			return true;
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex==currIx)
			{
				colorEditor1.Visible = false;
				return;
			}
			if (e.ColumnIndex == 2 || e.ColumnIndex == 4)
			{
				currDgv = (DataGridView)sender;
				DataGridViewRow dgr = currDgv.Rows[e.RowIndex];
				DataGridViewCell dgc = dgr.Cells[e.ColumnIndex];
				Rectangle r = currDgv.GetCellDisplayRectangle(4, e.RowIndex, true);
				Point p = r.Location;
				p.X += r.Width;
				p.Y += r.Height;

				currRow = currDict[e.RowIndex];
				currIx = e.RowIndex;

				string hexValue = dgr.Cells[1].Value.ToString();
				int ix = currRow.GetIx(hexValue);
				if (e.ColumnIndex==2 || currRow.newHexValues[ix]==null)
				{
					currRow.newColours[ix] = currRow.colours[ix];
					currRow.newHexValues[ix] = hexValue;
				}
				colorEditor1.Color = currRow.newColours[ix];
				colorEditor1.Location = p;
				colorEditor1.ShowAlphaChannel = true;
				colorEditor1.Visible = true;
				colorEditor1.BringToFront();
			} else
			{
				colorEditor1.Visible = false;
			}
		}

		private void colorEditor1_ColorChanged(object sender, EventArgs e)
		{
			System.Drawing.Color color = colorEditor1.Color;
			DataGridViewRow dgr = currDgv.Rows[currIx];

			Bitmap bm = new Bitmap(32, 32);
			for (int x = 0; x < 32; x++)
			{
				for (int y = 0; y < 32; y++)
				{
					bm.SetPixel(x, y, color);
				}
			}
			dgr.Cells[4].Value = bm;

			int c = color.ToArgb();
			dgr.Cells[3].Value = "0x"+c.ToString("x").PadLeft(8, '0');
			currRow.newHexValues[currRow.ix] = "0x"+c.ToString("x").PadLeft(8, '0');
			currRow.newColours[currRow.ix] = color;
		}

		private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
		{
			if (colorEditor1.Visible)
			{
				Rectangle r = currDgv.GetCellDisplayRectangle(4, currIx, true);
				Point p = r.Location;
				p.X += r.Width;
				p.Y += r.Height;
				colorEditor1.Location = p;
			}
		}

		private void tabControl1_Selected(object sender, TabControlEventArgs e)
		{
			SetTabPage(e.TabPage);
		}

		void SetTabPage(TabPage tabPage)
		{
			if (rows.Count>0)
			{
				currIx = -1;
				currDict = rows[tabPage.Text];
				currDgv = (DataGridView)tabPage.Controls[0];
				colorEditor1.Visible = false;
				if (!tabPage.Controls.Contains(colorEditor1))
					tabPage.Controls.Add(colorEditor1);
			}
		}
	}
}
