using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AsserPlayer.Utilities
{
    internal static class MediaFileHelper
    {
        private static readonly HashSet<string> AudioExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".mp3", ".wav", ".flac", ".aac", ".m4a", ".ogg", ".wma", ".opus"
        };

        private static readonly HashSet<string> VideoExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".mp4", ".mkv", ".avi", ".mov", ".webm", ".m4v", ".wmv", ".mpeg", ".mpg", ".ts"
        };

        public static bool IsSupported(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return false;
            var extension = Path.GetExtension(path);
            return AudioExtensions.Contains(extension) || VideoExtensions.Contains(extension);
        }

        public static bool IsVideo(string path)
        {
            return VideoExtensions.Contains(Path.GetExtension(path ?? string.Empty));
        }

        public static string GetMediaType(string path)
        {
            return IsVideo(path) ? "Video" : "Music";
        }

        public static string GetDisplayTitle(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return "Untitled";
            try { return Path.GetFileNameWithoutExtension(path); }
            catch { return path; }
        }


        private static readonly string[] KnownArtists =
        {
            "Arthur Nery", "Chase Atlantic", "Cigarettes After Sex", "Lola Amour", "keshi",
            "The 1975", "Lauv", "LANY", "NIKI", "sombr", "SZA", "Jordan Susanto", "Adie",
            "Esremborak", "Maroon 5", "Radiohead", "fitterkarma", "Andy Grammer", "Mayonnaise",
            "Harry Styles", "The Vamps", "Conan Gray", "DNCE", "No Rome"
        };

        public static string GetArtistFromTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return "Local audio";

            var parts = SplitTitleParts(title);
            foreach (var part in parts)
            {
                var candidate = CleanArtistCandidate(part);
                var known = KnownArtists.FirstOrDefault(artist => string.Equals(artist, candidate, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrWhiteSpace(known)) return known;
            }

            if (parts.Length > 1)
            {
                var first = CleanArtistCandidate(parts[0]);
                if (!string.IsNullOrWhiteSpace(first) && !Regex.IsMatch(first, @"^\d+$")) return first;
            }

            return "Local audio";
        }

        public static string GetTrackNameFromTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return "Untitled";

            var parts = SplitTitleParts(title);
            if (parts.Length == 0) return "Untitled";

            for (var i = 0; i < parts.Length; i++)
            {
                var candidate = CleanArtistCandidate(parts[i]);
                if (!KnownArtists.Any(artist => string.Equals(artist, candidate, StringComparison.OrdinalIgnoreCase)))
                    continue;

                if (i == 0 && parts.Length > 1)
                    return CleanTrackCandidate(parts[1]);

                if (i > 0)
                {
                    if (Regex.IsMatch(parts[0], @"^\d{1,3}$") && parts.Length > i + 1)
                        return CleanTrackCandidate(parts[i + 1]);
                    return CleanTrackCandidate(parts[0]);
                }
            }

            if (parts.Length > 1)
                return CleanTrackCandidate(parts[1]);

            return CleanTrackCandidate(parts[0]);
        }

        private static string[] SplitTitleParts(string title)
        {
            var cleaned = title == null ? string.Empty : title.Trim();
            cleaned = Regex.Replace(cleaned, @"\s*[-–—]\s*\(?\d+\s*Kbps\)?\s*$", string.Empty, RegexOptions.IgnoreCase);
            return Regex.Split(cleaned, @"\s+[-–—]\s+")
                .Select(part => part.Trim())
                .Where(part => !string.IsNullOrWhiteSpace(part))
                .ToArray();
        }

        private static string CleanArtistCandidate(string value)
        {
            var cleaned = value == null ? string.Empty : value.Trim();
            cleaned = Regex.Replace(cleaned, @"\s*[\(\[](official\s+)?(lyric\s+video|lyrics?|audio|video).*?[\)\]]\s*$", string.Empty, RegexOptions.IgnoreCase);
            cleaned = Regex.Replace(cleaned, @"\s*[\(\[](320\s*Kbps|\d+\s*Kbps)[\)\]]\s*$", string.Empty, RegexOptions.IgnoreCase);
            return cleaned.Trim();
        }

        private static string CleanTrackCandidate(string value)
        {
            var cleaned = value == null ? string.Empty : value.Trim();
            cleaned = Regex.Replace(cleaned, @"\s*[-–—]\s*\(?\d+\s*Kbps\)?\s*$", string.Empty, RegexOptions.IgnoreCase);
            return cleaned.Trim();
        }


        public static string GetFormatSummary(string path, string title)
        {
            var extension = string.Empty;
            try { extension = Path.GetExtension(path ?? string.Empty); } catch { extension = string.Empty; }
            extension = string.IsNullOrWhiteSpace(extension) ? "FILE" : extension.Trim('.').ToUpperInvariant();

            var bitrate = string.Empty;
            if (!string.IsNullOrWhiteSpace(title))
            {
                var match = Regex.Match(title, @"(\d+\s*Kbps)", RegexOptions.IgnoreCase);
                if (match.Success) bitrate = match.Groups[1].Value;
            }

            return string.IsNullOrWhiteSpace(bitrate) ? extension : (extension + "  •  " + bitrate);
        }

        public static string BuildOpenFileFilter()
        {
            return "Media files|*.mp3;*.wav;*.flac;*.aac;*.m4a;*.ogg;*.wma;*.opus;*.mp4;*.mkv;*.avi;*.mov;*.webm;*.m4v;*.wmv;*.mpeg;*.mpg;*.ts|Audio files|*.mp3;*.wav;*.flac;*.aac;*.m4a;*.ogg;*.wma;*.opus|Video files|*.mp4;*.mkv;*.avi;*.mov;*.webm;*.m4v;*.wmv;*.mpeg;*.mpg;*.ts|All files|*.*";
        }
    }
}
