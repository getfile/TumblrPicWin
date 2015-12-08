
/* 
 * JGB  2015/6/28 10:50:00
 */
using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Threading;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;


public delegate void TumblrCallBack(TumblrJson tj);

public enum TumblrJsonState
{
	Null, //正在加载
	OK, //正确完成
	LoadError, //加载失败
	ParseError, //解析失败
	TimeOutError, //超时失败
}

/// <summary>
/// json读取, 解析
/// 
/// </summary>
public class TumblrJson
{

	private string url;
	private TumblrCallBack callback;
	private TumblrJsonState state;
	private string errorMsg;

	private WebClient loader;
	private Hashtable jsonObj;

	private Timer timer;
	private int timeLast;

	public TumblrJson()
	{
		this.timeLast = Environment.TickCount;
		timer = new Timer(CheckTime, null, 500, 1000);
	}

	public Hashtable JsonObj
	{
		get { return jsonObj; }
	}

	public string ErrorMsg
	{
		get { return errorMsg; }
	}

	/// <summary> 是否正确
	/// </summary>
	public bool IsOk()
	{
		return (state == TumblrJsonState.OK);
	}

	/// <summary> 隔一秒检查一次进度
	/// </summary>
	private void CheckTime(object obj)
	{
		if (state != TumblrJsonState.Null)
			return;

		if (Environment.TickCount - timeLast > 15000)
			Over(TumblrJsonState.TimeOutError, "Time Out");
		else
			Program.Log("_", false);
	}

	private void Over(TumblrJsonState ts, string msg)
	{
		state = ts;
		errorMsg = msg;

		if (state == TumblrJsonState.OK)
		{
			if (callback != null)
				callback(this);
			return;
		}

		if (state != TumblrJsonState.Null)
		{
			Program.Log(" " + errorMsg);
			LoadJson(url, callback);
			return;
		}
	}

	public void LoadJson(string url, TumblrCallBack callback)
	{
		if (loader != null)
		{
			loader.DownloadProgressChanged -= ProgressHandler;
			loader.DownloadStringCompleted -= CompleteHandler;
			loader.CancelAsync();
		}
		state = TumblrJsonState.Null;
		this.timeLast = Environment.TickCount;
		this.callback = callback;
		this.url = url;
		Program.Log("\t", false);

		loader = new WebClient();
		loader.DownloadProgressChanged += ProgressHandler;
		loader.DownloadStringCompleted += CompleteHandler;
		loader.DownloadStringAsync(new Uri(url));
	}

	private void ProgressHandler(object sender, DownloadProgressChangedEventArgs e)
	{
		Program.Log(".", false);
		timeLast = Environment.TickCount;
	}

	private void CompleteHandler(object sender, DownloadStringCompletedEventArgs e)
	{
		if (e.Cancelled)
			return;
		if (e.Error != null)
		{
			Over(TumblrJsonState.LoadError, e.Error.Message);
			return;
		}

		Program.Log("o");

		string jsonStr = e.Result;
		jsonStr = jsonStr.Substring(22);
		jsonObj = Json.jsonDecode(jsonStr) as Hashtable;
		Over((jsonObj == null ? TumblrJsonState.ParseError : TumblrJsonState.OK), "");
	}


}
