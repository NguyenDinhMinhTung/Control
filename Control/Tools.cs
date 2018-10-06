using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Control
{
    static class Tools
    {
        public static string request(string url)
        {
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            string responseFromServer = reader.ReadToEnd();

            reader.Close();
            response.Close();

            return responseFromServer;
        }

        public static void sendCommand(string command, int touserid, int sessionid)
        {
            Tools.request("http://akita123.atwebpages.com/main.php?type=2&cmd='" + command + "'&fromuserid=1&touserid=" + touserid + "&ssid=" + sessionid);
        }
    }
}
