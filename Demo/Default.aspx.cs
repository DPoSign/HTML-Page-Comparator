using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Demo
{
    public partial class _Default : System.Web.UI.Page
    {
        string ReadTextFromUrl(string url)
        {
            // WebClient is still convenient
            // Assume UTF8, but detect BOM - could also honor response charset I suppose
            using (var client = new WebClient())
            using (var stream = client.OpenRead(url))
            using (var textReader = new StreamReader(stream, Encoding.UTF8, true))
            {
                return textReader.ReadToEnd();
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            //string downloadStringLIVE = client.DownloadString("http://www.incelaw.com/en/knowledge-bank");
            //string downloadStringDEV = client.DownloadString("http://ince2017_import.grouptreedev.net/en/knowledge-bank");

            string downloadStringLIVE = ReadTextFromUrl("http://www.incelaw.com/en/knowledge-bank");
            string downloadStringDEV = ReadTextFromUrl("http://ince2017_import.grouptreedev.net/en/knowledge-bank");
            

            string oldText = @downloadStringLIVE;

            string newText = @downloadStringDEV;

            HtmlDiff.HtmlDiff diffHelper = new HtmlDiff.HtmlDiff(oldText, newText);
            litOldText.Text = newText;
            litNewText.Text = oldText;

            // Lets add a block expression to group blocks we care about (such as dates)
            diffHelper.AddBlockExpression(new Regex(@"[\d]{1,2}[\s]*(Jan|Feb)[\s]*[\d]{4}", RegexOptions.IgnoreCase));

            litDiffText.Text = diffHelper.Build();
        }
    }
}
