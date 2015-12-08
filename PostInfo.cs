
/* 
 * JGB  2015/7/7 16:29:37
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;


public enum PostType
{
	Photo,
	Video,
	Regular,
	Other
};

/// <summary>
/// post info
/// </summary>
[Serializable]
public class PostInfo
{
	public readonly int no;
	public readonly string id;
	public readonly PostType type;
	public readonly string url;
	public readonly string picUrl;
	public readonly int picNum;
	public readonly int wid;
	public readonly int hei;

	private static string defaultIconUrl = "https://secure.assets.tumblr.com/images/default_avatar/cube_closed_128.png";

	/// <summary> 字节解码
	/// </summary>
	public PostInfo(Byte[] data)
	{
		if (data == null || data.Length < 1)
			return;
		MemoryStream ms = new MemoryStream(data);
		BinaryReader br = new BinaryReader(ms);

		no = br.ReadInt32();
		id = br.ReadString();
		type = (PostType) br.ReadInt32();
		url = br.ReadString();
		picUrl = br.ReadString();
		picNum = br.ReadInt32();
		wid = br.ReadInt32();
		hei = br.ReadInt32();

		br.Close();
		ms.Close();
	}

	/// <summary> 字符串解码
	/// </summary>
	public PostInfo(Hashtable data, int idx)
	{
		no = idx;
		id = GetString(data["id"]);
		type = SetType(GetString(data["type"]));
		url = GetString(data["url"]);
		picUrl = defaultIconUrl;
		picNum = 0;
		wid = hei = 100;
		if (type == PostType.Photo)
		{
//			picUrl = GetString(data["photo-url-100"]);
			picUrl = GetString(data["photo-url-250"]);
			ArrayList picList = (ArrayList)data["photos"];
			picNum = (picList == null ? 0 : picList.Count);
			string num;
			num = data["width"]+""; // as string;
			if (num == "False") num = "";
			wid = (string.IsNullOrEmpty(num) ? 100 : int.Parse(num));
			num = data["height"]+""; // as string;
			if (num == "False") num = "";
			hei = (string.IsNullOrEmpty(num) ? 100 : int.Parse(num));
		}
		else if (type == PostType.Video)
		{
			string str = GetString(data["video-player-250"]);
			str = (string.IsNullOrEmpty(str) ? defaultIconUrl : str);
			if (!string.IsNullOrEmpty(str))
				picUrl = ParseVideoStr(str, "poster=");
		}
	}

	public string ParseVideoStr(string str, string key)
	{
		int idx = str.IndexOf(key);
		if (idx < 0) 
			return defaultIconUrl;

		string split = str.Substring(idx + key.Length, 1);
		int start = str.IndexOf(split, idx);
		int end = str.IndexOf(split, start + 1);
		return str.Substring(start + 1, end - start - 1);
	}

	/// <summary> 序列化
	/// </summary>
	public Byte[] Code()
	{
		MemoryStream ms = new MemoryStream();
		BinaryWriter bw = new BinaryWriter(ms);

		bw.Write(no);
		bw.Write(id);
		bw.Write((int)type);
		bw.Write(url);
		bw.Write(picUrl);
		bw.Write(picNum);
		bw.Write(wid);
		bw.Write(hei);

		bw.Close();
		return ms.ToArray();
	}

	private PostType SetType(string str)
	{
		if (str == "photo") return PostType.Photo;
		if (str == "video") return PostType.Video;
		if (str == "regular") return PostType.Regular;
		return PostType.Other;
	}

	private string GetString(Object obj)
	{
		string one = obj as string;
		if (one == null) return "";
		return one;
	}


}
