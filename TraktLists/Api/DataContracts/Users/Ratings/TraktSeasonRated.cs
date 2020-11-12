using TraktLists.Api.DataContracts.BaseModel;

namespace TraktLists.Api.DataContracts.Users.Ratings
{
    
    public class TraktSeasonRated : TraktRated
    {
        public TraktSeason season { get; set; }
    }
}