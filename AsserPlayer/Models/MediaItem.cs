using System;
using System.IO;
using System.Runtime.Serialization;

namespace AsserPlayer.Models
{
    [DataContract]
    public sealed class MediaItem
    {
        [DataMember] public string Id { get; set; }
        [DataMember] public string Path { get; set; }
        [DataMember] public string Title { get; set; }
        [DataMember] public string MediaType { get; set; }
        [DataMember] public bool IsFavorite { get; set; }
        [DataMember] public DateTime AddedUtc { get; set; }
        [DataMember] public DateTime? LastPlayedUtc { get; set; }
        [DataMember] public int PlayCount { get; set; }
        [DataMember] public string ArtworkPath { get; set; }

        public string Folder
        {
            get
            {
                try { return System.IO.Path.GetDirectoryName(Path) ?? string.Empty; }
                catch { return string.Empty; }
            }
        }

        public bool Exists
        {
            get { return !string.IsNullOrWhiteSpace(Path) && File.Exists(Path); }
        }
    }
}
