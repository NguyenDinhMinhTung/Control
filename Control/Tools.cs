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

        public static int getUserIdFromString(string s)
        {
            return int.Parse(s.Substring(0, s.IndexOf('.')));
        }

        public static string getUserNameFromString(string s)
        {
            return s.Substring(s.IndexOf('.') + 2);
        }

        public static List<User> getListUser()
        {
            string[] sep = { "</br>" };
            string[] users = Tools.request("http://akita123.atwebpages.com/main.php?type=6").Split(sep, StringSplitOptions.None);

            List<User> result = new List<User>();

            foreach (string user in users)
            {
                if (user != null && user != "" && getUserIdFromString(user) != 1)
                {
                    int id = getUserIdFromString(user);
                    string name = getUserNameFromString(user);

                    result.Add(new User(id, name));
                }
            }

            return result;
        }

        public static void updateUsername(int id, string name)
        {
            request("http://akita123.atwebpages.com/main.php?type=9&id=" + id + "&username=" + name);
        }
    }
}
