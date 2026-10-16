using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AsserPlayer.Models
{
    [DataContract]
    public sealed class AppState
    {
        public AppState()
        {
            Library = new List<MediaItem>();
            Playlists = new List<Playlist>();
            Volume = 70;
            IsMuted = false;
        }

        [DataMember] public List<MediaItem> Library { get; set; }
        [DataMember] public List<Playlist> Playlists { get; set; }
        [DataMember] public int Volume { get; set; }
        [DataMember] public bool IsMuted { get; set; }
        [DataMember] public string QueueBannerPath { get; set; }
    }
}
