using System;
using System.IO;
using System.Runtime.Serialization.Json;
using AsserPlayer.Models;

namespace AsserPlayer.Services
{
    internal sealed class AppStorage
    {
        private readonly string _dataDirectory;
        private readonly string _statePath;

        public AppStorage()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _dataDirectory = Path.Combine(localAppData, "not.asser", "Data");
            _statePath = Path.Combine(_dataDirectory, "state.json");

            // Preserve existing libraries/playlists from the earlier NovaPlayer builds.
            var legacyState = Path.Combine(localAppData, "NovaPlayer", "Data", "state.json");
            if (!File.Exists(_statePath) && File.Exists(legacyState))
            {
                try
                {
                    Directory.CreateDirectory(_dataDirectory);
                    File.Copy(legacyState, _statePath, false);
                }
                catch
                {
                    // Migration is best-effort; a fresh state will be created if it fails.
                }
            }
        }

        public string StatePath { get { return _statePath; } }

        public AppState Load()
        {
            try
            {
                if (!File.Exists(_statePath)) return new AppState();

                using (var stream = File.OpenRead(_statePath))
                {
                    var serializer = new DataContractJsonSerializer(typeof(AppState));
                    var state = serializer.ReadObject(stream) as AppState;
                    return Normalize(state ?? new AppState());
                }
            }
            catch
            {
                return new AppState();
            }
        }

        public void Save(AppState state)
        {
            Directory.CreateDirectory(_dataDirectory);
            var tempPath = _statePath + ".tmp";

            using (var stream = File.Create(tempPath))
            {
                var serializer = new DataContractJsonSerializer(typeof(AppState));
                serializer.WriteObject(stream, Normalize(state ?? new AppState()));
            }

            if (File.Exists(_statePath))
            {
                File.Copy(tempPath, _statePath, true);
                File.Delete(tempPath);
            }
            else
            {
                File.Move(tempPath, _statePath);
            }
        }

        private static AppState Normalize(AppState state)
        {
            if (state.Library == null) state.Library = new System.Collections.Generic.List<MediaItem>();
            if (state.Playlists == null) state.Playlists = new System.Collections.Generic.List<Playlist>();
            if (state.Volume < 0 || state.Volume > 100) state.Volume = 70;

            foreach (var playlist in state.Playlists)
            {
                if (playlist.MediaIds == null) playlist.MediaIds = new System.Collections.Generic.List<string>();
            }

            return state;
        }
    }
}
