using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class Program
{
	/// <summary>
	/// 应用程序的主入口点。
	/// 
	/// 功能:
	///		7: 读取post, 读取图片.
	///		8-9: 鼠标滚轮翻页, 自动定时翻页
	/// 
	/// </summary>
	[STAThread]
	static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		form = new Form1();
		PostMgr.inst.init(initSet);
		form.Start();
		Application.Run(form);
	}

	private static Form1 form;
	public static string initSet = "sparth";
	public static string logMsg;
	public static void Log(string text, bool line = true)
	{
		logMsg += (text + (line ? "\r\n" : ""));
	}

	public static void Exit()
	{
		Application.Exit();
		Application.ExitThread();
	}

}
