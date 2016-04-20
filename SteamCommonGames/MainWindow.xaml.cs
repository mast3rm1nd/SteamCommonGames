using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Text.RegularExpressions;
using System.IO;
using System.Net;

namespace SteamCommonGames
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Go_button_Click(object sender, RoutedEventArgs e)
        {
            var profilesLinks = ProfilesList_textBox.Text.Replace("\r\n", "\n").Split('\n');

            var usersGames = new List<List<string>>();

            foreach(var link in profilesLinks)
            {
                var html = GetHtmlByURL(link);

                var matches = Regex.Matches(html, "\"name\":\"(?<GameName>.+?)\"");

                var userGames = new List<string>();

                foreach (Match match in matches)
                    userGames.Add(match.Groups["GameName"].Value);

                usersGames.Add(userGames);
            }

            string[] intersections = usersGames[0].Intersect(usersGames[1]).ToArray();

            for (int i = 2; i < usersGames.Count; i++)
                intersections = intersections.Intersect(usersGames[i]).ToArray();
            //var intersection = usersGames[0].Intersect(usersGames[1]).ToArray();
            // "name":"(?<GameName>.+?)"
            if (intersections.Length == 0)
                CommonsList_textBox.Text = "Nothing in common";
            else
                CommonsList_textBox.Text = string.Join(Environment.NewLine, intersections);
        }

        static string GetHtmlByURL(string url)
        {
            var html = "";

            using (var webClient = new WebClient())
            {
                Stream data = webClient.OpenRead(url);
                StreamReader reader = new StreamReader(data);
                html = reader.ReadToEnd();
            }

            return html;
        }
    }
}