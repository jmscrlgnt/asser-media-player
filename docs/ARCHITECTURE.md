# Architecture

AsserPlayer uses a small layered desktop architecture so media playback, library state, persistence, and UI behavior do not all live in one form.

## Main layers

### Forms

`MainForm` is the application shell. It owns interaction state such as the current library view, queue order, repeat mode, shuffle mode, current queue index, and full-screen state. It delegates durable data work and playback work to services.

`MainForm.Designer.cs` contains layout and control creation only. `UiTheme` centralizes the visual palette and common control styling. `TextPromptForm` is a small reusable dialog for playlist names.

### Models

`MediaItem` represents one imported file and stores its title, path, media type, favorite state, play count, and playback history.

`Playlist` stores playlist metadata and a list of media IDs rather than duplicating media records.

`AppState` is the serialized root object for library data and playback preferences.

### Services

`MediaLibraryService` owns library queries, file imports, folder scanning, playlists, favorites, play history, missing-item cleanup, and saved playback preferences.

`PlaybackService` wraps LibVLCSharp. The UI never talks directly to the LibVLC `MediaPlayer`; it uses a smaller application-specific playback API instead.

`AppStorage` serializes `AppState` to JSON beneath `%LOCALAPPDATA%` and normalizes older or partially missing state on load.

### Utilities

`MediaFileHelper` owns supported file extensions and derives display titles/media types from paths.

## Request flow examples

### Play a library item

```text
User double-clicks row
        ↓
MainForm resolves MediaItem
        ↓
PlaybackService.Play(path)
        ↓
LibVLCSharp / LibVLC
        ↓
Playback events update MainForm
        ↓
MediaLibraryService.MarkPlayed(item)
        ↓
AppStorage saves state.json
```

### Import a folder

```text
User selects folder
        ↓
MainForm.ImportFolder
        ↓
MediaLibraryService.AddFolder
        ↓
Safe recursive filesystem enumeration
        ↓
Supported media filtered by MediaFileHelper
        ↓
New MediaItem records created
        ↓
AppStorage persists updated library
```

## Why this structure matters

The separation makes each part easier to understand and extend. A future database, metadata provider, or different playback engine can be introduced without rewriting the entire window. It also makes the project easier to discuss in interviews because each class has a clear responsibility.

## v2.1 additions

### NativeTheme
`Forms/NativeTheme.cs` isolates Windows-specific theming calls. It applies immersive dark mode to the native window title bar and requests the Windows dark explorer theme for scrollable native controls.

### WaveformVisualizer
`Forms/WaveformVisualizer.cs` is a custom double-buffered WinForms control used for audio-only playback. It generates a deterministic visual waveform from the track identity, shows playback progress, and keeps copyrighted album art out of the repository.

### Starter playlist organization
`MediaLibraryService` seeds the personal starter playlists and automatically adds imported local music to `Asser Mix`. If an imported filename/path includes a configured artist name, that track is also added to the corresponding artist playlist.
