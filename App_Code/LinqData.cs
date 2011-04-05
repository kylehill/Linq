using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LinqData
/// </summary>
public static class LinqData
{
    #region Words

    public static bool InsertWord(string Word)
    {
        try
        {
            LinqGameDataContext dc = new LinqGameDataContext();

            LinqWord word = new LinqWord();
            word.Word = Word;
            word.IsRemoved = false;

            dc.LinqWords.InsertOnSubmit(word);
            dc.SubmitChanges();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static string GetRandomWord()
    {
        try
        {
            LinqGameDataContext dc = new LinqGameDataContext();
            
            Random r = new Random();
            int place = r.Next(0, GetWordCount());

            return (from w in dc.LinqWords
                    where w.IsRemoved == false
                    orderby w.WordID
                    select w.Word).ToArray<string>()[place];
        }
        catch
        {
            return string.Empty;
        }
    }

    public static int GetWordCount()
    {
        try
        {
            LinqGameDataContext dc = new LinqGameDataContext();

            return (from w in dc.LinqWords
                    where w.IsRemoved == false
                    select w.WordID).Count<int>();
        }
        catch
        {
            return 0;
        }
    }

    #endregion

    #region Users

    public static bool CreateUser(string Email, string Nickname, string Password)
    {
        try
        {
            LinqGameDataContext dc = new LinqGameDataContext();

            User u = new User();
            u.Email = Email;
            u.Nickname = Nickname;
            u.VerificationToken = GetVerificationHash(Email, Password);

            u.IsAdmin = false;
            u.JoinDate = DateTime.Now;

            dc.Users.InsertOnSubmit(u);
            dc.SubmitChanges();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool CheckUserCredentials(string Email, string Password)
    {
        try
        {
            LinqGameDataContext dc = new LinqGameDataContext();

            User u = (from user in dc.Users
                      where user.Email == Email
                      select user).First<User>();

            return (u.VerificationToken == GetVerificationHash(Email, Password));
        }
        catch
        {
            return false;
        }
    }

    public static User GetUser(string Email)
    {
        try
        {
            LinqGameDataContext dc = new LinqGameDataContext();

            User u = (from user in dc.Users
                      where user.Email == Email
                      select user).First<User>();

            return u;
        }
        catch
        {
            return new User();
        }
    }

    public static string GetSessionToken(string Email)
    {
        try
        {
            LinqGameDataContext dc = new LinqGameDataContext();
            Guid token = Guid.NewGuid();

            List<UserSession> usList = (from t in dc.UserSessions
                                        where t.UserID == GetUser(Email).UserID
                                        select t).ToList<UserSession>();

            dc.UserSessions.DeleteAllOnSubmit<UserSession>(usList);

            UserSession us = new UserSession();
            us.UserID = GetUser(Email).UserID;
            us.ExpiresOn = DateTime.Now.AddHours(1);
            us.SessionToken = token.ToString();

            dc.UserSessions.InsertOnSubmit(us);
            dc.SubmitChanges();

            return token.ToString();
        }
        catch
        {
            return string.Empty;
        }
    }

    public static bool CheckSessionToken(string token)
    {
        try
        {
            LinqGameDataContext dc = new LinqGameDataContext();

            UserSession us = (from t in dc.UserSessions
                              where t.SessionToken == token &&
                                t.ExpiresOn > DateTime.Now
                              select t).First<UserSession>();

            us.ExpiresOn = DateTime.Now.AddHours(1);
            
            dc.SubmitChanges();

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string GetVerificationHash(string Email, string Password)
    {
        System.Security.Cryptography.SHA256Managed m = new System.Security.Cryptography.SHA256Managed();
        byte[] buffer = new System.Text.UnicodeEncoding().GetBytes(String.Format("{0}{1}{2}", "b7079d7d-80e6-4206-8e92-97c63fd201d1", Email, Password));
        byte[] bArr = m.ComputeHash(buffer);
        string retVal = "";
        foreach (byte b in bArr)
        {
            retVal += b.ToString("X");
        }
        return retVal;
    }


    #endregion

    #region Chat

    public static List<ChatMessage> ChatInit()
    {
        try
        {
            LinqGameDataContext dc = new LinqGameDataContext();

            List<LinqGameChat> lList = (from c in dc.LinqGameChats
                                        where c.PostedAt > DateTime.Now.Subtract(new TimeSpan(0, 5, 0))
                                        select c).ToList<LinqGameChat>();
            
            List<ChatMessage> cmList = new List<ChatMessage>();

            if (lList.Count > 0)
            {
                foreach (LinqGameChat lgc in lList)
                {
                    cmList.Add(new ChatMessage(lgc));
                }
            }
            else
            {
                LinqGameChat lgc = (from c in dc.LinqGameChats
                                    orderby c.ChatLineID descending
                                    select c).First<LinqGameChat>();

                cmList.Add(new ChatMessage(lgc));
            }

            return cmList;
        }
        catch
        {
            return new List<ChatMessage>();
        }
    }

    public static List<ChatMessage> GetLatestChat(int AfterChatLineID)
    {
        try
        {
            LinqGameDataContext dc = new LinqGameDataContext();

            List<LinqGameChat> lList = (from c in dc.LinqGameChats
                                        where c.ChatLineID > AfterChatLineID
                                        select c).ToList<LinqGameChat>();

            List<ChatMessage> cmList = new List<ChatMessage>();

            foreach (LinqGameChat lgc in lList)
            {
                cmList.Add(new ChatMessage(lgc));
            }

            return cmList;
        }
        catch
        {
            return new List<ChatMessage>();
        }
    }

    public static bool PostChat(string email, string message)
    {
        try
        {
            LinqGameDataContext dc = new LinqGameDataContext();

            LinqGameChat lgc = new LinqGameChat();
            lgc.IsExCathedra = false;
            lgc.PostedAt = DateTime.Now;
            lgc.GameID = 1; // Lobby - for testing
            lgc.UserID = GetUser(email).UserID;
            lgc.ChatText = message;

            dc.LinqGameChats.InsertOnSubmit(lgc);
            dc.SubmitChanges();

            return true;
        }
        catch
        {
            return false;
        }
    }

    #endregion
}