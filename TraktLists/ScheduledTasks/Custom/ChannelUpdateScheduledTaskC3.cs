using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Model.Tasks;

namespace TraktLists
{
    public class ChannelUpdateScheduledTaskC3 : IScheduledTask, IConfigurableScheduledTask
    {
        private ITaskManager TaskManager { get; set; }

        public ChannelUpdateScheduledTaskC3(ITaskManager taskMan)
        {
            TaskManager = taskMan;
        }
        public async Task Execute(CancellationToken cancellationToken, IProgress<double> progress)
        {
            foreach (var t in TaskManager.ScheduledTasks)
            {
                if (t.Name == "Refresh Internet Channels")
                {
                    await TaskManager.Execute(t, new TaskOptions());
                }
            }
        }

        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
            return new[]
            {
                new TaskTriggerInfo
                {
                    Type          = TaskTriggerInfo.TriggerInterval,
                    IntervalTicks = TimeSpan.FromDays(1).Ticks
                }
            };
        }

        private readonly TraktChannel _traktChannel =
            Plugin.Instance.PluginConfiguration.CustomTraktChannels.Single(x => x.Id == 3);
        public string Name        => "Update Channel: " + _traktChannel.ChannelName;
        public string Key         => _traktChannel.ChannelName.Replace(" ", "") + "Channel";
        public string Description => "Create/Update channel from trakt list.";
        public string Category    => "Trakt List Channels";
        public bool IsHidden      => false;
        public bool IsEnabled     => true;
        public bool IsLogged      => true;
    }
}