
/* 
 * JGB  2015/7/12 11:17:11
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class PicBox
{

	private WebClient loader;
	private PostInfo info;
	private PictureBox pic;
	private Label title;

	public PicBox()
	{
		pic = new PictureBox();
		pic.SizeMode = PictureBoxSizeMode.CenterImage;
		pic.Cursor = Cursors.Hand;
		pic.MouseClick += PicClickHandler;

		title = new Label();
		title.AutoSize = true;
		title.TextAlign = ContentAlignment.BottomLeft;
	}

	public void SetInfo(PostInfo pi)
	{
		info = pi;
		string num = (info.type == PostType.Photo ? ":" + info.picNum : "");
		title.Text = info.no.ToString() + " (" + info.type.ToString() + num + ")";
	}

	public void SetVisible(bool v)
	{
		pic.Visible = v;
		title.Visible = v;
	}

	public void SetParent(Control p)
	{
		pic.Parent = p;
		title.Parent = p;
	}

	public void SetBounds(int ox, int oy, int ow, int oh)
	{
		title.SetBounds(ox, oy, ow, 15);
		pic.SetBounds(ox, oy + 13, ow, oh);
	}

	public void LoadPic(string url)
	{
		pic.Image = null;

		if (loader != null)
		{
			loader.DownloadDataCompleted -= PicLoadCompleted;
			loader.DownloadProgressChanged -= PicLoadProgress;
			loader.CancelAsync();
		}
		loader = new WebClient();
		loader.CachePolicy = new RequestCachePolicy(RequestCacheLevel.Default);
		loader.DownloadDataCompleted += PicLoadCompleted;
		loader.DownloadProgressChanged += PicLoadProgress;
		loader.DownloadDataAsync(new Uri(url), info);
	}

	private void PicLoadCompleted(object sender, DownloadDataCompletedEventArgs e)
	{
		if (e.Cancelled)
		{
			Program.Log("cancel!");
			return;
		}
		if (e.Error != null)
		{
			Program.Log("pic loade error: " + e.Error.Message+": "+((PostInfo)e.UserState).picUrl);
			return;
		}

		WebClient wc = (WebClient)sender;
		MemoryStream ms = new MemoryStream(e.Result);
		Image img = Image.FromStream(ms);
		pic.Image = img;
		pic.Width = img.Width;
		pic.Height = img.Height;

		if (pic.Width != Form1.picWidth)
		{
			pic.Height = (int)((float)Form1.picWidth / pic.Width * pic.Height);
			pic.Width = Form1.picWidth;
			pic.SizeMode=PictureBoxSizeMode.Zoom;
		}
		else
			pic.SizeMode = PictureBoxSizeMode.Normal;
	}

	private void PicLoadProgress(object sender, DownloadProgressChangedEventArgs e)
	{
		
	}

	private void PicClickHandler(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left) //如果是gif, 打开250, 否则1280
//			Process.Start(info.url);
		{
			int start = info.picUrl.LastIndexOf("_");
			int end = info.picUrl.LastIndexOf(".");

			int flag = (info.picUrl.Substring(end) == ".gif" ? 250 : 1280);
			string url = info.picUrl.Substring(0, start) + "_" + flag + info.picUrl.Substring(end);
			Process.Start(url);
		}

		if (e.Button == MouseButtons.Right)
			(pic.Parent.Parent as Form1).ShowPicMenu(info, pic, e.Location);
	}
}
