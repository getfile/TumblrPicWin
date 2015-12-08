
/* 
 * JGB  2015/7/7 17:07:47
 */

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Policy;
using System.Text;


/// <summary>
/// manager post
/// read, download, save
/// </summary>
public class PostMgr
{

	public static readonly PostMgr inst;

	static PostMgr()
	{
		inst = new PostMgr();
	}

	private List<PostInfo> posts;
	private Dictionary<int, bool> postsIdx;
	private TumblrJson jsonLoader;

	private PostMgr()
	{
		posts = new List<PostInfo>();
		postsIdx = new Dictionary<int, bool>();
		jsonLoader = new TumblrJson();
	}

	private int postTotal;
	private int postStart;
	private string tumblrName;

	public string TumblrName
	{
		get { return tumblrName; }
		set { tumblrName = value; }
	}

	public int GetPostCount
	{
		get { return posts.Count; }
	}

	public PostInfo GetPost(int idx)
	{
		if (idx >= posts.Count || idx < 0)
			return null;
		return posts[idx];
	}

	public void init(string name)
	{
		if (string.IsNullOrEmpty(name))
			return;
		if (name.Equals(TumblrName))
			return;

		Program.Log("");
		Program.Log("");
		Program.Log("===========================================");
		tumblrName = name;
		posts.Clear();
		postsIdx.Clear();

		ReadPosts();
		string url = "http://" + tumblrName + ".tumblr.com/api/read/json?num=20";
		Program.Log(url);

		jsonLoader.LoadJson(url, TumblrNumHandler);
	}

	/// <summary> 获取帖子总数
	/// </summary>
	private void TumblrNumHandler(TumblrJson tj)
	{
		postTotal = (int)(double)(tj.JsonObj["posts-total"]);
		Program.Log("postTotal: " + postTotal);
		Program.Log("");
		DownPosts();
	}

	/// <summary> 下载所有新的posts
	/// </summary>
	void DownPosts()
	{
		if (postTotal <= posts.Count)
		{
			Program.Log("good job!");
			Program.Log("");
			return;
		}
		postStart = postTotal - posts.Count - 50;
		postStart = (postStart < 0 ? 0 : postStart);
		string url = "http://" + tumblrName + ".tumblr.com/api/read/json?num=50&start=" + postStart;
		Program.Log(url);
		jsonLoader.LoadJson(url, DownPostHanlder);
	}

	/// <summary>下载完的回调
	/// </summary>
	void DownPostHanlder(TumblrJson tj)
	{
		ArrayList list = tj.JsonObj["posts"] as ArrayList;
		int len = list.Count;
		for (int i = len - 1; i >= 0; i--)
		{
			PostInfo item = new PostInfo(list[i] as Hashtable, postTotal - 1 - i - postStart);
			if (postsIdx.ContainsKey(item.no))
				continue;
			posts.Add(item);
			postsIdx.Add(item.no, true);
		}
		SavePosts();
		DownPosts();
	}

	/// <summary> 读入所有的posts
	/// </summary>
	void ReadPosts()
	{
		if (string.IsNullOrEmpty(tumblrName))
			return;

		FileStream fs = new FileStream(tumblrName + ".dat", FileMode.OpenOrCreate);
		BinaryReader sr = new BinaryReader(fs);

		if (fs.Length > 0)
		{
			int len = sr.ReadInt32();
			for (int i = 0; i < len; i++)
			{
				int postLen = sr.ReadInt32();
				byte[] data = sr.ReadBytes(postLen);
				if (data.Length < postLen)
					break;
				PostInfo item = new PostInfo(data);
				if (postsIdx.ContainsKey(item.no))
					continue;
				if (i != item.no)
					break;
				posts.Add(item);
				postsIdx.Add(item.no, true);
			}
		}
		sr.Close();
		fs.Close();

		Program.Log(TumblrName + " posts: " + posts.Count);
	}

	/// <summary> 保存所有的posts
	/// </summary>
	private void SavePosts()
	{
		if (string.IsNullOrEmpty(tumblrName))
			return;

		FileStream fs = new FileStream(tumblrName + ".dat", FileMode.Truncate);
		BinaryWriter sw = new BinaryWriter(fs);

		int len = posts.Count;
		sw.Write(len);
		for (int i = 0; i < len; i++)
		{
			byte[] data = posts[i].Code();
			sw.Write(data.Length);
			sw.Write(data);
		}

		sw.Close();
		fs.Close();
	}

	/// <summary> 收集图片url
	/// </summary>
	public void ToHtmlFile()
	{
		StringBuilder htmlStr = new StringBuilder(10240);

		for (int i = posts.Count - 1; i >= 0; i--)
		{
			PostInfo info = posts[i];
			string widHei = "width='100'";//" height='" + info.hei + "'";
			string date = "";
			string id = "&#10;(" + info.type.ToString() + ":" + info.picNum + " " + info.no + "/" + postTotal + ")";
			string pic = "<a href='" + info.url + "'><img src='" + info.picUrl + "' " + widHei + " title='" + date + id + "'/></a>&nbsp;\n";
			htmlStr.Append(pic);
		}

		StreamWriter file = new StreamWriter(TumblrName + ".html", false);     //覆盖方式添加内容  
		file.Write(htmlStr.ToString());
		file.Close();
	}


}

public delegate void CallbackFunc(int min, int max);
