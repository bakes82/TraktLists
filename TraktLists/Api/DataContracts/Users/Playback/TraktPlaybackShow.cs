using System;
using TraktLists.Api.DataContracts.BaseModel;

namespace TraktLists.Api.DataContracts.Users.Playback
{
    public class TraktPlaybackEpisode
    {
        public TraktEpisode episode { get; set; }

        public float progress { get; set; }

        public DateTime paused_at { get; set; }
    }
}
