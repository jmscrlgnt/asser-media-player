using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AsserPlayer.Models;
using AsserPlayer.Utilities;

namespace AsserPlayer.Services
{
    internal enum LibraryView
    {
        All,
        Music,
        Videos,
        Favorites,
        Recent,
        Playlist
    }

    internal sealed class MediaLibraryService
    {
        private static readonly string[] StarterPlaylists =
        {
            "Asser Mix", "Arthur Nery", "Chase Atlantic", "Cigarettes After Sex",
            "Lola Amour", "keshi", "The 1975", "Lauv", "LANY", "NIKI", "Sombr",
            "SZA", "Jordan Susanto", "Adie", "Esremborak", "Maroon 5", "Radiohead",
            "Fitterkarma", "Andy Grammer", "Mayonnaise"
        };

        private readonly AppStorage _storage;
        private readonly AppState _state;

        public MediaLibraryService()
        {
            _storage = new AppStorage();
            _state = _storage.Load();
            EnsureStarterPlaylists();
        }

        public IReadOnlyList<Playlist> Playlists
        {
            get { return _state.Playlists.OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase).ToList(); }
        }

        public int Volume { get { return _state.Volume; } }
        public bool IsMuted { get { return _state.IsMuted; } }
        public string StatePath { get { return _storage.StatePath; } }
        public string QueueBannerPath { get { return _state.QueueBannerPath; } }


        public string SetQueueBanner(string sourcePath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || !File.Exists(sourcePath)) return null;

            try
            {
                var dataDirectory = Path.GetDirectoryName(_storage.StatePath);
                Directory.CreateDirectory(dataDirectory);

                var extension = Path.GetExtension(sourcePath);
                if (string.IsNullOrWhiteSpace(extension)) extension = ".jpg";

                var destination = Path.Combine(dataDirectory, "queue-banner" + extension.ToLowerInvariant());

                foreach (var existing in Directory.GetFiles(dataDirectory, "queue-banner.*"))
                {
                    if (!string.Equals(existing, destination, StringComparison.OrdinalIgnoreCase))
                    {
                        try { File.Delete(existing); } catch { }
                    }
                }

                File.Copy(sourcePath, destination, true);
                _state.QueueBannerPath = destination;
                Save();
                return destination;
            }
            catch
            {
                return null;
            }
        }

        public void ClearQueueBanner()
        {
            var oldPath = _state.QueueBannerPath;
            _state.QueueBannerPath = null;
            Save();
            if (!string.IsNullOrWhiteSpace(oldPath) && File.Exists(oldPath))
            {
                try { File.Delete(oldPath); } catch { }
            }
        }


        public string SetArtwork(MediaItem item, string sourcePath)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Id) ||
                string.IsNullOrWhiteSpace(sourcePath) || !File.Exists(sourcePath)) return null;

            try
            {
                var dataDirectory = Path.GetDirectoryName(_storage.StatePath);
                var artworkDirectory = Path.Combine(dataDirectory, "Artwork");
                Directory.CreateDirectory(artworkDirectory);

                var extension = Path.GetExtension(sourcePath);
                if (string.IsNullOrWhiteSpace(extension)) extension = ".jpg";
                extension = extension.ToLowerInvariant();

                foreach (var existing in Directory.GetFiles(artworkDirectory, item.Id + ".*"))
                {
                    try { File.Delete(existing); } catch { }
                }

                var destination = Path.Combine(artworkDirectory, item.Id + extension);
                File.Copy(sourcePath, destination, true);
                item.ArtworkPath = destination;
                Save();
                return destination;
            }
            catch
            {
                return null;
            }
        }

        public void ClearArtwork(MediaItem item)
        {
            if (item == null) return;
            var oldPath = item.ArtworkPath;
            item.ArtworkPath = null;
            Save();

            if (!string.IsNullOrWhiteSpace(oldPath) && File.Exists(oldPath))
            {
                try { File.Delete(oldPath); } catch { }
            }
        }

        public int AddFiles(IEnumerable<string> paths)
        {
            if (paths == null) return 0;
            var added = 0;

            foreach (var rawPath in paths)
            {
                if (string.IsNullOrWhiteSpace(rawPath)) continue;

                string fullPath;
                try { fullPath = Path.GetFullPath(rawPath); }
                catch { continue; }

                if (!File.Exists(fullPath) || !MediaFileHelper.IsSupported(fullPath)) continue;
                if (_state.Library.Any(existingItem => string.Equals(existingItem.Path, fullPath, StringComparison.OrdinalIgnoreCase))) continue;

                var newItem = new MediaItem
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Path = fullPath,
                    Title = MediaFileHelper.GetDisplayTitle(fullPath),
                    MediaType = MediaFileHelper.GetMediaType(fullPath),
                    AddedUtc = DateTime.UtcNow,
                    IsFavorite = false,
                    PlayCount = 0
                };
                _state.Library.Add(newItem);
                AutoOrganizeImportedMusic(newItem);
                added++;
            }

            if (added > 0) Save();
            return added;
        }

        public int AddFolder(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath)) return 0;
            return AddFiles(SafeEnumerateFiles(folderPath));
        }

        public List<MediaItem> GetItems(LibraryView view, string playlistId, string searchText)
        {
            IEnumerable<MediaItem> query = _state.Library;

            switch (view)
            {
                case LibraryView.Music:
                    query = query.Where(item => string.Equals(item.MediaType, "Music", StringComparison.OrdinalIgnoreCase));
                    break;
                case LibraryView.Videos:
                    query = query.Where(item => string.Equals(item.MediaType, "Video", StringComparison.OrdinalIgnoreCase));
                    break;
                case LibraryView.Favorites:
                    query = query.Where(item => item.IsFavorite);
                    break;
                case LibraryView.Recent:
                    query = query.Where(item => item.LastPlayedUtc.HasValue)
                                 .OrderByDescending(item => item.LastPlayedUtc.Value);
                    break;
                case LibraryView.Playlist:
                    var playlist = _state.Playlists.FirstOrDefault(p => string.Equals(p.Id, playlistId, StringComparison.OrdinalIgnoreCase));
                    if (playlist == null) query = Enumerable.Empty<MediaItem>();
                    else
                    {
                        var ids = new HashSet<string>(playlist.MediaIds, StringComparer.OrdinalIgnoreCase);
                        query = query.Where(item => ids.Contains(item.Id));
                    }
                    break;
            }

            var search = (searchText ?? string.Empty).Trim();
            if (search.Length > 0)
            {
                query = query.Where(item =>
                    (item.Title ?? string.Empty).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (item.Path ?? string.Empty).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (item.MediaType ?? string.Empty).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (view != LibraryView.Recent)
                query = query.OrderBy(item => item.Title, StringComparer.OrdinalIgnoreCase);

            return query.ToList();
        }

        public MediaItem FindById(string id)
        {
            return _state.Library.FirstOrDefault(item => string.Equals(item.Id, id, StringComparison.OrdinalIgnoreCase));
        }

        public void ToggleFavorite(MediaItem item)
        {
            if (item == null) return;
            item.IsFavorite = !item.IsFavorite;
            Save();
        }

        public void MarkPlayed(MediaItem item)
        {
            if (item == null) return;
            item.LastPlayedUtc = DateTime.UtcNow;
            item.PlayCount++;
            Save();
        }

        public void RemoveFromLibrary(MediaItem item)
        {
            if (item == null) return;
            _state.Library.RemoveAll(x => string.Equals(x.Id, item.Id, StringComparison.OrdinalIgnoreCase));
            foreach (var playlist in _state.Playlists)
                playlist.MediaIds.RemoveAll(id => string.Equals(id, item.Id, StringComparison.OrdinalIgnoreCase));
            Save();
        }

        public Playlist CreatePlaylist(string name)
        {
            name = (name ?? string.Empty).Trim();
            if (name.Length == 0) return null;

            var existing = _state.Playlists.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
            if (existing != null) return existing;

            var playlist = new Playlist
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = name,
                CreatedUtc = DateTime.UtcNow,
                ModifiedUtc = DateTime.UtcNow
            };
            _state.Playlists.Add(playlist);
            Save();
            return playlist;
        }

        public void DeletePlaylist(Playlist playlist)
        {
            if (playlist == null) return;
            _state.Playlists.RemoveAll(p => string.Equals(p.Id, playlist.Id, StringComparison.OrdinalIgnoreCase));
            Save();
        }

        public bool AddToPlaylist(Playlist playlist, MediaItem item)
        {
            if (playlist == null || item == null) return false;
            if (playlist.MediaIds.Any(id => string.Equals(id, item.Id, StringComparison.OrdinalIgnoreCase))) return false;
            playlist.MediaIds.Add(item.Id);
            playlist.ModifiedUtc = DateTime.UtcNow;
            Save();
            return true;
        }

        public bool RemoveFromPlaylist(Playlist playlist, MediaItem item)
        {
            if (playlist == null || item == null) return false;
            var removed = playlist.MediaIds.RemoveAll(id => string.Equals(id, item.Id, StringComparison.OrdinalIgnoreCase)) > 0;
            if (removed)
            {
                playlist.ModifiedUtc = DateTime.UtcNow;
                Save();
            }
            return removed;
        }

        public void SavePlaybackSettings(int volume, bool isMuted)
        {
            _state.Volume = Math.Max(0, Math.Min(100, volume));
            _state.IsMuted = isMuted;
            Save();
        }

        public int CountMissingItems()
        {
            return _state.Library.Count(item => !item.Exists);
        }

        public int RemoveMissingItems()
        {
            var missingIds = new HashSet<string>(
                _state.Library.Where(item => !item.Exists).Select(item => item.Id),
                StringComparer.OrdinalIgnoreCase);

            if (missingIds.Count == 0) return 0;

            var removed = _state.Library.RemoveAll(item => missingIds.Contains(item.Id));
            foreach (var playlist in _state.Playlists)
                playlist.MediaIds.RemoveAll(id => missingIds.Contains(id));

            Save();
            return removed;
        }

        private void EnsureStarterPlaylists()
        {
            var changed = false;
            foreach (var name in StarterPlaylists)
            {
                if (_state.Playlists.Any(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase))) continue;
                _state.Playlists.Add(new Playlist
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = name,
                    CreatedUtc = DateTime.UtcNow,
                    ModifiedUtc = DateTime.UtcNow
                });
                changed = true;
            }
            if (changed) Save();
        }

        private void AutoOrganizeImportedMusic(MediaItem item)
        {
            if (item == null || !string.Equals(item.MediaType, "Music", StringComparison.OrdinalIgnoreCase)) return;

            var asserMix = _state.Playlists.FirstOrDefault(p => string.Equals(p.Name, "Asser Mix", StringComparison.OrdinalIgnoreCase));
            if (asserMix != null && !asserMix.MediaIds.Contains(item.Id))
                asserMix.MediaIds.Add(item.Id);

            var haystack = ((item.Title ?? string.Empty) + " " + (item.Path ?? string.Empty)).ToLowerInvariant();
            foreach (var artistName in StarterPlaylists.Where(n => !string.Equals(n, "Asser Mix", StringComparison.OrdinalIgnoreCase)))
            {
                if (haystack.IndexOf(artistName.ToLowerInvariant(), StringComparison.Ordinal) < 0) continue;
                var playlist = _state.Playlists.FirstOrDefault(p => string.Equals(p.Name, artistName, StringComparison.OrdinalIgnoreCase));
                if (playlist != null && !playlist.MediaIds.Contains(item.Id))
                    playlist.MediaIds.Add(item.Id);
            }
        }

        public void Save()
        {
            _storage.Save(_state);
        }

        private static IEnumerable<string> SafeEnumerateFiles(string root)
        {
            var pending = new Stack<string>();
            pending.Push(root);

            while (pending.Count > 0)
            {
                var current = pending.Pop();
                string[] files;
                try { files = Directory.GetFiles(current); }
                catch { files = new string[0]; }

                foreach (var file in files)
                    if (MediaFileHelper.IsSupported(file)) yield return file;

                string[] directories;
                try { directories = Directory.GetDirectories(current); }
                catch { directories = new string[0]; }

                foreach (var directory in directories)
                    pending.Push(directory);
            }
        }
    }
}
