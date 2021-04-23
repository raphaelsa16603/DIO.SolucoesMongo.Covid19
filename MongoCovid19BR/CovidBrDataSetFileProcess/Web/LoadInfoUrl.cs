using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace CovidBrDataSetFileProcess.Web
{
    public class LoadInfoUrl
    {
        
        public static string GetDataUrlCovid19BrFiles()
        {
            string url = @"https://brasil.io/dataset/covid19/files/";
            string html;

            using (WebClient wc = new WebClient()) {
                //wc.Headers["Cookie"] = "security=true";
                wc.Encoding = System.Text.Encoding.UTF8;
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36");

                try
                {
                    html = wc.DownloadString(url);
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("429"))
                    {
                        System.Threading.Thread.Sleep(1 * 60 * 1000);
                        html = wc.DownloadString(url);
                    }
                    else
                    {
                        throw new Exception(e.Message);
                    }
                }
            }

            // string XPath = "/html/body/main/div/div/div/h2";
            // string find = "Data de captura:";
            string dados = "";
            string pattern = "<h2.*?>Data de captura:(.*?)<\\/h2>";
            MatchCollection matches = Regex.Matches(html, pattern);
            if (matches.Count > 0)
                foreach (Match m in matches)
                    dados = m.Groups[1].Value;

            return dados;
        }
    }
}