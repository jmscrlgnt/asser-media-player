# not.asser — Music / Video Player

A portfolio-ready **local-first Windows media player** built with C# WinForms, .NET Framework 4.8, LibVLCSharp, and the VLC runtime.

`not.asser` is designed as a personal desktop media library rather than a streaming-service clone: you import media you legally own or have permission to use, organize it into playlists, and play music or video locally.

> Current version: **2.3.0 — Personalized UI Refinement**

## What's new in v2.3

- Preserves the personalized **Asser Playlist** header and **not.asser music/video player** window title.
- Rebuilt the audio-only player as a **smooth multi-line neon waveform visualizer** inspired by modern music-player visuals. It is drawn by the app itself rather than using the reference images as backgrounds.
- Custom per-track artwork now appears in the center of the audio visualizer as well as in the right-side Now Playing panel.
- Replaced the native DataGridView vertical scrollbar with a **fully custom dark scrollbar**, avoiding the white Windows scrollbar that kept returning.
- Centered the top search field and added an in-field search icon plus the cue text **“What do you want to play?”**.
- Reorganized the right-side Now Playing area so its space is actively used by artwork, track identity, About This Track information, playback status, position, speed, repeat, shuffle, favorite state, and quick actions.
- Changed the main library's **LOCATION** column to **ARTIST** for a more useful music-library presentation.
- Improved filename parsing for common personal-library names, including leading track numbers and several preferred artists.
- Kept mute/unmute synchronized with the 0% volume behavior and custom speaker icon.
- Retained numbered owner-drawn playlists with song counts and selected-row accent styling.

## Starter playlists

The app creates empty playlist templates for the user's preferred artists, including:

Arthur Nery, Chase Atlantic, Cigarettes After Sex, Lola Amour, keshi, The 1975, Lauv, LANY, NIKI, Sombr, SZA, Jordan Susanto, Adie, Esremborak, Maroon 5, Radiohead, Fitterkarma, Andy Grammer, and Mayonnaise.

**No commercial songs are bundled with this repository.** Those recordings are copyrighted. Add music you legally own or have permission to use; matching local files are then organized into the relevant starter playlist automatically.

## Core features

- Audio and video playback through LibVLC.
- Three-column layout: library navigation, player/library, and a dedicated Now Playing information panel.
- Full-screen video mode with collapsed side panels.
- Music visualization mode for audio-only tracks.
- Add individual files or recursively scan folders.
- Drag-and-drop import.
- All Media, Music, Videos, Favorites, Recently Played, and custom playlists.
- Search and sorting by title, recently played, most played, and newest added.
- Internal playback queue with Play Next, previous/next navigation, shuffle, and repeat behavior.
- Previous, play/pause, stop, next, shuffle, and Repeat Off/All/One.
- Seek bar, elapsed/total time, volume, mute, and playback speed.
- Missing-file detection and cleanup.
- Open containing folder and copy file path actions.
- Persistent local JSON library metadata.
- Keyboard shortcuts.
- GitHub Actions Windows build workflow.

## Audio experience

When a video is playing, the center stage is dedicated to video. When an audio file is playing, the app switches to a branded audio stage containing:

- smooth layered neon audio-wave curves,
- subtle playback-progress accenting,
- custom per-track artwork in the center,
- track title and parsed artist name,
- a dark blue/black visual stage generated entirely in code.

The attached wave images were used only as visual direction; the application draws its own waveform instead of embedding those references as backgrounds.

## Dark Windows chrome

WinForms normally follows the native Windows title-bar theme independently from a form's `BackColor`. `not.asser` uses the Desktop Window Manager dark-mode attribute for the title bar and applies Windows' dark explorer theme to native scrollable controls where the operating system supports it.

This means the application remains native while matching the dark interface much more closely.

## Tech stack

- C#
- WinForms
- .NET Framework 4.8
- LibVLCSharp.WinForms 3.10.1
- VideoLAN.LibVLC.Windows 3.0.24
- JSON persistence with `DataContractJsonSerializer`

## Repository structure

```text
Asser-Media-Player-v2.3.0-Personalized/
├── AsserPlayer.sln
├── AsserPlayer/
│   ├── Assets/
│   │   └── AsserPlayer.ico
│   ├── Forms/
│   │   ├── MainForm.cs
│   │   ├── MainForm.Designer.cs
│   │   ├── MainForm.resx
│   │   ├── NativeTheme.cs
│   │   ├── DarkVScrollBar.cs
│   │   ├── UiTheme.cs
│   │   ├── WaveformVisualizer.cs
│   │   └── TextPromptForm.cs
│   ├── Models/
│   ├── Services/
│   ├── Utilities/
│   ├── Properties/
│   ├── App.config
│   └── AsserPlayer.csproj
├── docs/
├── tests/
├── .github/workflows/build.yml
├── CHANGELOG.md
├── CONTRIBUTING.md
├── LICENSE
└── README.md
```

## Run locally

1. Clone or download the repository.
2. Open `AsserPlayer.sln` in Visual Studio.
3. Select **x64**.
4. Restore NuGet packages.
5. Build the solution.
6. Run with `F5`.

If Visual Studio has cached an older project version, close Visual Studio and delete `.vs`, `bin`, and `obj`, then reopen the solution and rebuild.

## Existing-user migration

Earlier builds stored data in:

```text
%LOCALAPPDATA%\NovaPlayer\Data\state.json
```

v2.1 stores data in:

```text
%LOCALAPPDATA%\not.asser\Data\state.json
```

On first run, if the new state does not exist but the older NovaPlayer state does, the app copies the old state automatically.

## Useful shortcuts

| Shortcut | Action |
|---|---|
| `Space` | Play / pause |
| `Ctrl + O` | Add media files |
| `Ctrl + Shift + O` | Add folder |
| `Ctrl + F` | Focus search |
| `M` | Mute / unmute |
| `Left / Right` | Seek ±5 seconds |
| `Up / Down` | Volume ±5% |
| `F11` | Full screen |
| `Esc` | Exit full screen |

## Portfolio notes

This project demonstrates desktop UI organization, event-driven programming, media-engine integration, persistence, queue state, playlist management, native Windows theming, custom drawing, backwards-compatible data migration, and separation between UI, services, models, and utilities.

A good GitHub repository description is:

> Local-first C# WinForms music/video player with LibVLC playback, custom audio waveform visualization, playlists, queue management, dark Windows theming, and fullscreen video.

Suggested topics: `csharp`, `winforms`, `dotnet-framework`, `libvlcsharp`, `media-player`, `music-player`, `video-player`, `desktop-app`, `portfolio-project`.

## License

MIT. See [LICENSE](LICENSE).
