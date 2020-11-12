using System;
using TraktLists.Api.DataContracts.BaseModel;

namespace TraktLists.Api.DataContracts.Users.Playback
{
    public class TraktPlaybackMovie
    {
        public TraktMovie movie { get; set; }

        public float progress { get; set; }

        public DateTime paused_at { get; set; }
    }
}
