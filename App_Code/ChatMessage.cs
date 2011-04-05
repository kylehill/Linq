using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ChatMessage
/// </summary>
public class ChatMessage
{
    public int lineId;
    public DateTime dt;
    public string voice;
    public bool isExCathedra;
    public string message;

	public ChatMessage(LinqGameChat gc)
	{
        lineId = gc.ChatLineID;
        dt = gc.PostedAt;
        voice = gc.User.Nickname;
        isExCathedra = gc.IsExCathedra.GetValueOrDefault(false);
        message = gc.ChatText;
	}
}