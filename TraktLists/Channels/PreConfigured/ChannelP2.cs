using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Channels;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Channels;
using MediaBrowser.Model.Drawing;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Serialization;
using TraktLists.Api;
using TraktLists.Channels;

namespace TraktLists
{
    public class ChannelP2 : IChannel, IHasCacheKey, IRequiresMediaInfoCallback
    {
        private IHttpClient HttpClient         { get; }
        private ILogger Logger                 { get; }
        private IJsonSerializer JsonSerializer { get; }
        private ILibraryManager LibraryManager { get; }
        private IUserManager UserManager { get; }
        private TraktApi TraktApi { get; }
        
        private readonly TraktChannel _channelToConfigure = Plugin.Instance.PluginConfiguration.PreConfiguredTraktChannels.Single(x => x.Id == 2);

        public ChannelP2(IHttpClient httpClient, ILogManager logManager, IJsonSerializer jsonSerializer, ILibraryManager lib, TraktApi traktApi, IUserManager userManager)
        {
            HttpClient     = httpClient;
            Logger         = logManager.GetLogger(GetType().Name);
            JsonSerializer = jsonSerializer;
            LibraryManager = lib;
            TraktApi = traktApi;
            UserManager = userManager;
        }

        public string DataVersion => "12";

        public InternalChannelFeatures GetChannelFeatures()
        {
            return new InternalChannelFeatures
            {
                ContentTypes = new List<ChannelMediaContentType>
                {
                    ChannelMediaContentType.Movie
                },

                MediaTypes = new List<ChannelMediaType>
                {
                    ChannelMediaType.Video
                },

                SupportsContentDownloading = true,
                SupportsSortOrderToggle    = true,
            };
        }
        
        private async Task<ChannelItemResult> GetChannelItemsInternal(CancellationToken cancellationToken)
        {
            var helper = new ChannelHelper();
            return await helper.GetChannelItemResult(UserManager, LibraryManager, _channelToConfigure, Logger, TraktApi, cancellationToken);
        }

        public bool IsEnabledFor(string userId)
        {
            return true;
        }

        public async Task<ChannelItemResult> GetChannelItems(InternalChannelItemQuery query, CancellationToken cancellationToken)
        {
            return await GetChannelItemsInternal(cancellationToken);
        }

        public async Task<DynamicImageResponse> GetChannelImage(ImageType type, CancellationToken cancellationToken)
        {
            switch (type)
            {
                case ImageType.Backdrop:
                    {
                        var path = GetType().Namespace + ".Images." + type.ToString().ToLower() + ".png";

                        return await Task.FromResult(new DynamicImageResponse
                        {
                            Format = ImageFormat.Png,
                            HasImage = true,
                            Stream = GetType().Assembly.GetManifestResourceStream(path)
                        });
                    }
                default:
                    throw new ArgumentException("Unsupported image type: " + type);

            }
        }

        public IEnumerable<ImageType> GetSupportedChannelImages()
        {
            return new List<ImageType> { ImageType.Primary, ImageType.Thumb };
        }

        public string Name => _channelToConfigure.ChannelName;
        public string Description { get; private set; }
        public string HomePageUrl
        {
            get { return ""; }
        }

        public ChannelParentalRating ParentalRating
        {
            get { return ChannelParentalRating.GeneralAudience; }
        }

        public string GetCacheKey(string userId)
        {
            return Guid.NewGuid().ToString("N");
        }

        public Task<IEnumerable<MediaSourceInfo>> GetChannelItemMediaInfo(string id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}