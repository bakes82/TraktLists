using System;
using System.Collections.Generic;
using MediaBrowser.Model.Plugins;
using TraktLists.Api;

namespace TraktLists
{
    public class PluginConfig : BasePluginConfiguration
    {
        public Guid Guid = new Guid("CD48A6A8-D26B-48EB-8857-D06A62875039"); // Also Needs Set In HTML File

        public PluginConfig()
        {
            TraktUser = new TraktUser();
            CustomTraktChannels = new List<TraktChannel>();
            PreConfiguredTraktChannels = new List<TraktChannel>();
            CustomTraktChannels = new List<TraktChannel>();


                /*PreConfiguredTraktChannels = new List<TraktChannel>
                {
                    new TraktChannel
                    {
                        Uri = "https://trakt.tv/users/mmounirou/lists/imdb-top-250-movies?sort=rank,asc",
                        Enabled = true,
                        ChannelName = "IMDB Top 250 Movies",
                        Id = 1
                    },
                    new TraktChannel
                    {
                        Uri = "https://trakt.tv/users/justin/lists/imdb-top-rated-movies?sort=rank,asc",
                        Enabled = true,
                        ChannelName = "IMDB Top Rated Movies",
                        Id = 2
                    }
                };
    

            
                var blankCustoms = new List<TraktChannel>();

                for (var i = 0; i < 10; i++)
                {
                    var number = i + 1;
                    blankCustoms.Add(new TraktChannel
                    {
                        Uri = string.Empty,
                        Enabled = false,
                        ChannelName = "Channel " + number,
                        Id = number
                    });
                }

                CustomTraktChannels = blankCustoms;*/
            

            
        }

        public string PluginName => "TraktLists";
        public string PluginDesc => "Generate Channels from TraktLists";
        public string Pin { get; set; }
        public List<TraktChannel> CustomTraktChannels { get; set; }
        public List<TraktChannel> PreConfiguredTraktChannels { get; set; }
        public TraktUser TraktUser { get; set; }
    }

    public class TraktChannel
    {
        public string Uri { get; set; }
        public string TraktListUserName => TraktHelpers.TraktListUriParse(Uri).Username;
        public string TraktListName => TraktHelpers.TraktListUriParse(Uri).ListName;
        public bool Enabled { get; set; }
        public string ChannelName { get; set; }
        public int Id { get; set; }
    }
}