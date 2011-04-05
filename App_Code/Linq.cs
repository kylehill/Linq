using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Linq : System.Web.Services.WebService {

    public Linq () { }

    [WebMethod]
    public string GetRandomWord() 
    {
        return LinqData.GetRandomWord();
    }

    [WebMethod]
    public bool CreateNewUser(string email, string nickname, string password)
    {
        return LinqData.CreateUser(email, nickname, password);
    }

    [WebMethod]
    public string Login(string email, string password)
    {
        if (LinqData.CheckUserCredentials(email, password))
        {
            return LinqData.GetSessionToken(email);
        }
        return string.Empty;
    }

    [WebMethod]
    public List<ChatMessage> ChatInit(string token)
    {
        if (LinqData.CheckSessionToken(token))
        {
            return LinqData.ChatInit();
        }
        
        return new List<ChatMessage>();
    }

    [WebMethod]
    public List<ChatMessage> GetLatestChat(string token, int latestLine)
    {
        if (LinqData.CheckSessionToken(token))
        {
            return LinqData.GetLatestChat(latestLine);
        }

        return new List<ChatMessage>();
    }

    [WebMethod]
    public bool PostChat(string token, string email, string message)
    {
        if (LinqData.CheckSessionToken(token))
        {
            return LinqData.PostChat(email, message);
        }
        return false;
    }
}
