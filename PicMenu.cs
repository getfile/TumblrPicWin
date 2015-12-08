
/* 
 * JGB  2015/7/16 15:17:38
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class PicMenu
{
	private PostInfo info;
	private ContextMenu menu;

	public PicMenu()
	{
		MenuItem[] items = new MenuItem[4];
		items[0] = new MenuItem("to post", Item0Handler);
		items[1] = new MenuItem("pic 250", Item1Handler);
		items[2] = new MenuItem("pic 500", Item2Handler);
		items[3] = new MenuItem("pic 1280", Item3Handler);

		menu = new ContextMenu(items);
	}

	public void Show(PostInfo info, Control c, Point pos)
	{
		this.info = info;
		menu.Show(c, pos);
	}

	private void Item0Handler(object sender, EventArgs e)
	{
		Process.Start(info.url);
	}

	private void Item1Handler(object sender, EventArgs e)
	{
		JumpToPic(250);
	}

	private void Item2Handler(object sender, EventArgs e)
	{
		JumpToPic(500);
	}

	private void Item3Handler(object sender, EventArgs e)
	{
		JumpToPic(1280);
	}

	private void JumpToPic(int width)
	{
		int start = info.picUrl.LastIndexOf("_");
		int end = info.picUrl.LastIndexOf(".");
		string url = info.picUrl.Substring(0, start) + "_" + width + info.picUrl.Substring(end);
		Process.Start(url);
	}

}
