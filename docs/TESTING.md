# Testing Checklist

Use this checklist after restoring packages and building the x64 solution on Windows.

## Build

- [ ] Visual Studio restores `LibVLCSharp.WinForms` and `VideoLAN.LibVLC.Windows`.
- [ ] Debug x64 builds with 0 errors.
- [ ] Release x64 builds with 0 errors.
- [ ] The application icon appears in the built executable/window.

## Library

- [ ] Add one supported audio file.
- [ ] Add one supported video file.
- [ ] Add multiple files at once.
- [ ] Add a folder containing nested media folders.
- [ ] Drag files/folders onto the window.
- [ ] Adding the same file twice does not duplicate it.
- [ ] Search filters the current view.
- [ ] All four sort modes work.
- [ ] Favorite/unfavorite updates the Favorites view.
- [ ] Recently Played updates after playback starts.
- [ ] Removing a library item does not delete the file from disk.

## Missing media

- [ ] Move or rename an imported file outside AsserPlayer.
- [ ] Reopen/refresh the library and confirm it shows as Missing.
- [ ] Clean Missing removes only stale library entries.

## Playlists

- [ ] Create a playlist.
- [ ] Add a selected item.
- [ ] Prevent duplicate playlist entries.
- [ ] Remove an item from a playlist.
- [ ] Delete a playlist without deleting media files.

## Queue

- [ ] Add a library item to queue.
- [ ] Play Next inserts immediately after the current queue item.
- [ ] Move queue items up/down.
- [ ] Double-click a queued item to play it.
- [ ] Remove one item.
- [ ] Clear queue.
- [ ] Queue stays scrollable with many items.

## Playback

- [ ] Play/pause/stop.
- [ ] Previous and next.
- [ ] Seek by progress slider.
- [ ] Left/right keyboard seek.
- [ ] Volume slider and up/down shortcuts.
- [ ] Mute button and `M` shortcut.
- [ ] Shuffle.
- [ ] Repeat Off, All, and One.
- [ ] 0.75x, 1.0x, 1.25x, 1.5x, and 2.0x speed selections.

## Video/full screen

- [ ] Video renders in the center stage.
- [ ] Double-click video enters full screen.
- [ ] F11 toggles full screen.
- [ ] Escape exits full screen.
- [ ] Sidebar, queue, library, and top bar collapse in full screen.
- [ ] Bottom transport remains usable.

## Persistence

- [ ] Close and reopen the app.
- [ ] Library persists.
- [ ] Playlists persist.
- [ ] Favorites persist.
- [ ] Play counts persist.
- [ ] Volume and mute state persist.

## Repository checks

Run:

```bash
python tests/verify_structure.py
```

The script should print `AsserPlayer v2.0 static repository checks passed.`
