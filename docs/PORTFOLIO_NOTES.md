# Portfolio Notes

## 30-second project explanation

AsserPlayer is a Windows local media player I built with C# WinForms and LibVLCSharp. I separated the project into a UI layer, library service, playback service, persistence service, models, and media utilities instead of putting all logic into one form. The app supports music and video playback, searchable local-library management, playlists, favorites, a reorderable playback queue, full-screen playback, speed controls, drag-and-drop imports, and persistent local state.

## Problems the project solves

- Keeps local music and video organized in one library.
- Avoids a cluttered desktop layout by using dedicated navigation, player, library, queue, and transport regions.
- Handles libraries larger than the visible window through bounded scrolling.
- Protects users from stale paths through missing-file detection and cleanup.
- Keeps actual media files untouched when users manage the AsserPlayer library.

## Engineering decisions worth discussing

**LibVLCSharp instead of Windows Media Player ActiveX** — gives the app a dedicated playback engine with broad format support and keeps the project independent from the older WMP control.

**Service separation** — playback, storage, and library operations are behind application-specific services, making MainForm primarily an orchestrator.

**IDs inside playlists** — playlists store `MediaItem.Id` values rather than copying complete media objects, reducing duplicated state.

**Safe folder scanning** — inaccessible folders are skipped rather than crashing an import.

**Local-first persistence** — the app saves JSON under LocalAppData and never uploads media or metadata.

## Suggested GitHub repository description

> Polished local-first Windows music/video player built with C# WinForms and LibVLCSharp, featuring searchable media libraries, playlists, favorites, queue management, playback speed, and full-screen video.

## Suggested topics

`csharp` `winforms` `dotnet-framework` `libvlc` `libvlcsharp` `media-player` `music-player` `video-player` `desktop-app` `portfolio-project`
