using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public partial class Form1 : Form
{
	public Form1()
	{
		InitializeComponent();

		logBox.Visible = false;

		Text = "Tumblr Pics";
		//			FormBorderStyle = FormBorderStyle.FixedDialog;
		//			MaximizeBox = false;
		//			MinimizeBox = false;
		//			HelpButton = true;

		Init();
		Form1_Resize(null, null);

		Timer t = new Timer();
		t.Tick += TickLog;
		t.Interval = 200;
		t.Start();

		scrollTimer = new Timer();
		scrollTimer.Tick += TickScroll;
	}

	/// <summary> 是否切换图集 </summary>
	private bool isChangeSet = true;

	/// <summary> 定时读取log信息
	/// </summary>
	public void TickLog(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(Program.logMsg))
			return;
		logBox.AppendText(Program.logMsg);
		Program.logMsg = "";

		imgBar.Maximum = PostMgr.inst.GetPostCount;
		if (imgBar.Maximum > 0 && isChangeSet == true)
		{
			isChangeSet = false;
			ShowPic();
		}
	}



	/// <summary> 定时翻页
	/// </summary>
	public void TickScroll(object sender, EventArgs e)
	{
		panelScroll();
	}

	/// <summary> 开始显示
	/// </summary>
	public void Start()
	{
		imgBar.Value = 0;
		line = -1;
		ShowPic();
	}

	private List<PicBox> pics;
	private PicMenu picMenu;

	private int line = -1;
	private int formWid = -1;
	private int formHei = -1;

	private void Init()
	{
		picMenu = new PicMenu();

		///init urlbox
		DirectoryInfo thisOne = new DirectoryInfo(".");
		FileInfo[] fileInfo = thisOne.GetFiles("*.dat");
		foreach (FileInfo info in fileInfo)
		{
			int idx = info.Name.IndexOf(".");
			urlBox.Items.Add(info.Name.Substring(0, idx));
		}
		if (!urlBox.Items.Contains(Program.initSet))
			urlBox.Items.Add(Program.initSet);

		urlBox.SelectedIndex = urlBox.Items.IndexOf(Program.initSet);

		///init pics
		pics = new List<PicBox>();
		for (int i = 0; i < 100; i++)
		{
			PicBox pic = new PicBox();
			pic.SetParent(imgPanel);
			pics.Add(pic);
		}
	}

	/// <summary> 显示的图片宽度
	/// </summary>
	static public int picWidth = 100; 

	private void ShowPic()
	{
		if (PostMgr.inst.GetPostCount == 0)
			return;

		if (line == imgBar.Value && formWid == Width && formHei == Height)
			return;

//		int postLen = PostMgr.inst.GetPostCount - 1;
		line = imgBar.Value;
		formWid = Width;
		formHei = Height;

		for (int i = 0; i < 100; i++)
			pics[i].SetVisible(false);

		int boundWid = imgPanel.Width - 15;
		int boundHei = imgPanel.Height;
		int ox = 0, oy = 0;
		int maxHei = 0;

		int maxCol = (int)(boundWid / (picWidth + 5)) + (boundWid % (picWidth + 5) >= picWidth ? 1 : 0);
		int maxRow = 0;

		for (int i = 0; i < 100; i++)
		{
			PicBox pic = pics[i];
			PostInfo info = PostMgr.inst.GetPost(imgBar.Value + i);
			if (info == null)
				break;

			int picHei = (int)(picWidth * info.hei / info.wid);
			if (ox + picWidth > boundWid)
			{
				ox = 0;
				oy += maxHei + 25;
				if (oy > boundHei)
					break;
				maxHei = picHei;
				maxRow++;
			}
			else if (maxHei < picHei)
				maxHei = picHei;

			pic.SetInfo(info);
			pic.SetVisible(true);
			pic.LoadPic(getPicUrl(info, picWidth));
			pic.SetBounds(ox, oy, picWidth, picHei);
			ox += picWidth + 5;
		}

		if (maxRow == 0)
			maxRow = 1;
		imgBar.SmallChange = maxCol; //一行
		imgBar.LargeChange = maxCol * maxRow; //一页
	}

	string getPicUrl(PostInfo info, int width)
	{
		int start = info.picUrl.LastIndexOf("_");
		int end = info.picUrl.LastIndexOf(".");
		string url = info.picUrl.Substring(0, start) + "_" + width + info.picUrl.Substring(end);
		return url;
	}

	public void ShowPicMenu(PostInfo info, Control c, Point pos)
	{
		picMenu.Show(info, c, pos);
	}

	private void Form1_Resize(object sender, EventArgs e)
	{
		urlBox.SetBounds(5, 5, Width - 150, 20);
		autoScrollBn.SetBounds(Width - 140, 5, 52, 20);
		archiveBn.SetBounds(Width - 83, 5, 20, 20);
		htmlBn.SetBounds(Width - 58, 5, 20, 20);
		logBn.SetBounds(Width - 33, 5, 20, 20);

		logBox.SetBounds(5, 30, Width - 20, Height - 60);
		imgPanel.SetBounds(5, 30, Width - 20, Height - 60);
		imgBar.SetBounds(imgPanel.Width - 15, 0, 15, imgPanel.Height);

		if (Width < 575)
			picWidth = 100;
//		else if(Width<1315)
//			picWidth = 250;
		else
			picWidth = 250;

		ShowPic();
	}

	private void Form1_Closed(object sender, EventArgs e)
	{
		Program.Exit();
	}

	/// <summary> 鼠标滚轮翻页
	/// </summary>
	private void imgPanel_MouseWheel(object sender, MouseEventArgs e)
	{
		scrollFlag = e.Delta;
		if (scrollAuto > 0)
		{
			autoScrollBn.Text = (scrollAuto == 0 ? "关闭" : (scrollFlag > 0 ? "上" : "下") + scrollAuto + "秒");
			scrollTimer.Stop();
			scrollTimer.Start();
		}

		panelScroll();
	}

	/// <summary> 鼠标中键按下(随机翻页)
	/// </summary>
	private void imgPanel_MouseMiddelClick(object sender, MouseEventArgs e)
	{
		if (e.Button != MouseButtons.Middle)
			return;

		scrollFlag = 0;
		if (scrollAuto > 0)
		{
			autoScrollBn.Text = bnTitle();
			scrollTimer.Stop();
			scrollTimer.Start();
		}

		panelScroll();
	}

	private void panelScroll()
	{
//		Console.WriteLine("panel mouse wheel");
		if (scrollFlag > 0) //向上
			imgBar.Value = (imgBar.Value > imgBar.LargeChange ? imgBar.Value - imgBar.LargeChange : 0);
		else if (scrollFlag == 0) //随机
			imgBar.Value = rnd.Next(0, imgBar.Maximum - imgBar.LargeChange + 1);
		else //向下
			imgBar.Value = (imgBar.Value < imgBar.Maximum - imgBar.LargeChange? 
				imgBar.Value + imgBar.LargeChange: 
				imgBar.Value);
	}

	private void imgPanel_MouseEnter(object sender, EventArgs e)
	{
		imgPanel.Focus();
	}

	private void imgBar_Scroll(object sender, ScrollEventArgs e)
	{
		tt.SetToolTip(imgBar, imgBar.Value.ToString());
	}

	private void imgBar_ValueChanged(object sender, EventArgs e)
	{
		ShowPic();
	}

	/// <summary> 回车, 添加新的图集
	/// </summary>
	private void urlBox_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.KeyCode != Keys.Enter)
			return;
		PostMgr.inst.init(urlBox.Text);
		if (!urlBox.Items.Contains(urlBox.Text))
			urlBox.Items.Add(urlBox.Text);
		imgBar.Value = 0;
		line = -1;
		isChangeSet = true;
//		ShowPic();
	}

	/// <summary> 选择另一图集
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void urlBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		PostMgr.inst.init(urlBox.Text);
		imgBar.Value = 0;
		line = -1;
		isChangeSet = true;
//		ShowPic();
	}

	private Timer scrollTimer;
	private int scrollAuto = 0;
	private int scrollFlag = -1;
	private Random rnd = new Random();

	private void autoScrollBn_Click(object sender, EventArgs e)
	{
		if (scrollAuto == 0)
			scrollAuto = 5;
		else if (scrollAuto == 5)
			scrollAuto = 10;
		else if (scrollAuto == 10)
			scrollAuto = 20;
		else if (scrollAuto == 20)
			scrollAuto = 0;

		autoScrollBn.Text = bnTitle();
		scrollTimer.Stop();
		if (scrollAuto == 0)
			return;

		scrollTimer.Interval = scrollAuto * 1000;
		scrollTimer.Start();
	}

	private string bnTitle()
	{
		string flagStr = (scrollFlag > 0 ? "上" : (scrollFlag == 0 ? "随机" : "下"));
		return (scrollAuto == 0 ? "关闭" : flagStr + scrollAuto + "秒");
	}

	private void archiveBn_Click(object sender, EventArgs e)
	{
		Process.Start("http://" + PostMgr.inst.TumblrName + ".tumblr.com/archive");  
	}

	private void htmlBn_Click(object sender, EventArgs e)
	{
		PostMgr.inst.ToHtmlFile();
		Process.Start(PostMgr.inst.TumblrName + ".html");
	}

	private void logBn_Click(object sender, EventArgs e)
	{
		logBox.Visible = !logBox.Visible;
		imgPanel.Visible = !imgPanel.Visible;
	}



}
