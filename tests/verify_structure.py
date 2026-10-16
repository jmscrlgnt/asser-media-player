from pathlib import Path
import re
import sys
import xml.etree.ElementTree as ET

root = Path(__file__).resolve().parents[1]

required = [
    'AsserPlayer.sln',
    'AsserPlayer/AsserPlayer.csproj',
    'AsserPlayer/Program.cs',
    'AsserPlayer/App.config',
    'AsserPlayer/Assets/AsserPlayer.ico',
    'AsserPlayer/Properties/AssemblyInfo.cs',
    'AsserPlayer/Forms/MainForm.cs',
    'AsserPlayer/Forms/MainForm.Designer.cs',
    'AsserPlayer/Forms/MainForm.resx',
    'AsserPlayer/Forms/UiTheme.cs',
    'AsserPlayer/Forms/NativeTheme.cs',
    'AsserPlayer/Forms/DarkVScrollBar.cs',
    'AsserPlayer/Forms/WaveformVisualizer.cs',
    'AsserPlayer/Services/PlaybackService.cs',
    'AsserPlayer/Services/MediaLibraryService.cs',
    'AsserPlayer/Services/AppStorage.cs',
    'AsserPlayer/Models/MediaItem.cs',
    'AsserPlayer/Models/Playlist.cs',
    'README.md',
    'LICENSE',
    'CHANGELOG.md',
    'docs/ARCHITECTURE.md',
    'docs/USER_GUIDE.md',
    '.github/workflows/build.yml',
]

missing = [item for item in required if not (root / item).exists()]
if missing:
    print('Missing required files:')
    for item in missing:
        print(' -', item)
    sys.exit(1)

csproj_path = root / 'AsserPlayer/AsserPlayer.csproj'
ET.parse(csproj_path)
csproj = csproj_path.read_text(encoding='utf-8')
for token in [
    '<TargetFrameworkVersion>v4.8</TargetFrameworkVersion>',
    '<PlatformTarget>x64</PlatformTarget>',
    'LibVLCSharp.WinForms',
    'VideoLAN.LibVLC.Windows',
    'System.Xml',
    'Assets\\AsserPlayer.ico',
    'Forms\\NativeTheme.cs',
    'Forms\\WaveformVisualizer.cs',
    'Forms\\DarkVScrollBar.cs',
]:
    assert token in csproj, token

main = (root / 'AsserPlayer/Forms/MainForm.cs').read_text(encoding='utf-8')
for token in [
    'ToggleFullscreen()',
    'ConfigureAudioExperience()',
    'NativeTheme.ApplyDarkTitleBar(this)',
    'DrawPlaylistItem',
    'DrawQueueColumnHeader',
    '_audioVisualizer.SetTrack',
    'ConfigureTopSearch()',
    'ConfigureLibraryScrollBar()',
    'What do you want to play?',
]:
    assert token in main, token

library = (root / 'AsserPlayer/Services/MediaLibraryService.cs').read_text(encoding='utf-8')
for token in ['Asser Mix', 'EnsureStarterPlaylists()', 'AutoOrganizeImportedMusic(newItem)']:
    assert token in library, token

storage = (root / 'AsserPlayer/Services/AppStorage.cs').read_text(encoding='utf-8')
assert '"not.asser"' in storage
assert '"NovaPlayer"' in storage  # legacy migration path

for path in root.rglob('*'):
    if not path.is_file() or path.suffix.lower() not in {'.cs', '.md', '.config', '.csproj', '.yml', '.yaml'}:
        continue
    content = path.read_text(encoding='utf-8', errors='ignore')
    assert not re.search(r'C:\\Users\\[^\\\s]+', content, re.IGNORECASE), f'Local user path found in {path}'

print('not.asser v2.3 static repository checks passed.')
