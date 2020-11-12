using System;

namespace TraktLists
{
    public static class TraktHelpers
    {
        public static TraktListFields TraktListUriParse(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return new TraktListFields{Username = string.Empty, ListName = string.Empty};
            }
            
            uri = uri.ToLower();
            if (uri.Contains("/users/") && uri.Contains("/lists/"))
            {
                var questionMarkIndex = uri.IndexOf('?');
                if (questionMarkIndex > 0)
                {
                    uri = uri.Remove(questionMarkIndex);
                }

                
                var splitString = uri.Split(new [] {"/users/"}, StringSplitOptions.None)[1].Split(new [] {"/lists/"}, StringSplitOptions.None);
                var userName = splitString[0];
                var listName = splitString[1];
                
                return new TraktListFields{Username = userName, ListName = listName};
            }

            return null;
        }
    }

    public class TraktListFields
    {
        public string Username { get; set; }
        public string ListName { get; set; }
    }
}