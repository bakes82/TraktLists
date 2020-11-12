using TraktLists.Api.DataContracts.BaseModel;

namespace TraktLists.Api.DataContracts.Users.Ratings
{
    
    public class TraktShowRated : TraktRated
    {
        public TraktShow show { get; set; }
    }
}