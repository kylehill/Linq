using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class InsertWordList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblRandomWord.Text = LinqData.GetRandomWord();
    }
    protected void btnAddWords_Click(object sender, EventArgs e)
    {
        foreach (string w in txtWordList.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
        {
            LinqData.InsertWord(w);
        }

    }
}