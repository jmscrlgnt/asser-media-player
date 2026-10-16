using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AsserPlayer.Models
{
    [DataContract]
    public sealed class Playlist
    {
        public Playlist()
        {
            MediaIds = new List<string>();
        }

        [DataMember] public string Id { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public List<string> MediaIds { get; set; }
        [DataMember] public DateTime CreatedUtc { get; set; }
        [DataMember] public DateTime ModifiedUtc { get; set; }

        public override string ToString()
        {
            return Name ?? "Untitled Playlist";
        }
    }
}
