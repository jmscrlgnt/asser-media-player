# Changelog

## v2.3.0 — Personalized UI refinement

- Replaced bar-based audio visualization with smooth layered neon wave curves drawn in code.
- Kept custom track artwork centered inside the audio visualizer.
- Added a custom dark DataGridView scrollbar to eliminate the recurring white native scrollbar.
- Centered the top search control and added a search icon plus “What do you want to play?” cue text.
- Rebalanced the right-side Now Playing panel and added live playback information.
- Changed LOCATION to ARTIST in the main library grid.
- Improved filename/artist parsing for personal-library naming patterns.
- Preserved “Asser Playlist” and “not.asser music/video player” branding.

## 2.2.0 — Personalized Player UI

- Replaced the visible queue panel with a dedicated Now Playing details panel.
- Added per-track custom artwork that can be assigned by double-clicking the artwork area.
- Added current-track type, play count, date added, location, and favorite status.
- Added quick actions for favorite, artwork, file location, and copy path.
- Reworked mute into a drawn speaker icon; muting now moves the volume slider to 0% and unmuting restores the previous level.
- Restyled playlists with numbering, selection accent, and per-playlist track counts.
- Kept the internal playback queue for next/previous/shuffle behavior without using the right panel for queue management.


## 2.1.3 — Queue panel redesign

- Rebuilt the Up Next panel using a fixed four-row layout so it stays aligned in the WinForms Designer and at runtime.
- Added a dedicated user-selectable queue banner image area. Double-click it to choose an image; right-click to replace or remove it.
- Persisted the chosen queue banner under the app's local data folder.
- Removed the ListView column header to eliminate the bright header/scrollbar corner artifact.
- Up Next now shows only future queue items instead of already-played/current items.
- Fixed queue button placement so Up, Down, Remove, and Clear remain visible.

## 2.1.0 — not.asser Personal Edition

- Rebranded the app and executable project to AsserPlayer / not.asser.
- Added dark native Windows title-bar support.
- Added dark native theming hooks for scrollable controls.
- Added owner-drawn dark playlist rows and queue surfaces.
- Added a custom audio waveform visualizer with per-track generated waveform and progress state.
- Added artist/title parsing for common `Artist - Track` filenames.
- Added starter artist playlist templates and `Asser Mix`.
- Added automatic local-track organization into matching starter playlists.
- Added migration from the previous `%LOCALAPPDATA%\NovaPlayer` state path.
- Preserved the v2.0.1 Visual Studio Designer compatibility fix.

## 2.0.1

- Fixed WinForms Designer loading by removing non-designer helper calls from `InitializeComponent()`.

## 2.0.0

- Portfolio-oriented redesign with library, playlists, queue, search, full-screen playback, favorites, history, sorting, missing-media cleanup, playback speed, and documentation.

## v2.2.1 - Now Playing Polish
- Audio visualizer now supports stylized wave backgrounds and center artwork
- Library grid shows ARTIST instead of LOCATION
- Now Playing panel is tighter and more informative
- Reapplied dark-scrollable theming to library grid and lists

## v2.3.3 - Responsive Right Panel
- Right Now Playing panel now adapts its artwork/header height to the current window height.
- ABOUT THIS TRACK and PLAYBACK remain visible in normal/minimum window sizes.
- Normal top and bottom bars use compact runtime heights to preserve more content space.

## v2.3.4 - Library Artwork + Fullscreen Transport Polish
- Added per-track artwork thumbnails beside every library title, with music/video fallback icons.
- Reused each track's custom right-panel artwork in the library list.
- Replaced the Full screen text button with a custom fullscreen/exit-fullscreen icon.
- Improved responsive bottom transport layout in fullscreen and narrow windows.
- Simplified fullscreen right-side controls by hiding speed/percentage labels while retaining mute, volume, and fullscreen controls.
- Simplified fullscreen now-playing text to title + media metadata to prevent clipping.
