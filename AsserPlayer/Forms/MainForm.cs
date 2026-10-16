using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AsserPlayer.Models;
using AsserPlayer.Services;
using AsserPlayer.Utilities;

namespace AsserPlayer.Forms
{
    public partial class MainForm : Form
    {
        private enum RepeatMode { Off, All, One }

        private const float NormalTopBarHeight = 72F;
        private const float NormalTransportHeight = 116F;
        private const float FullscreenTransportHeight = 92F;
        private const float SidebarWidth = 216F;
        private const float QueueWidth = 300F;

        private readonly MediaLibraryService _library;
        private readonly List<MediaItem> _queue = new List<MediaItem>();
        private readonly Random _random = new Random();
        private PlaybackService _playback;
        private int _queueIndex = -1;
        private LibraryView _currentView = LibraryView.All;
        private string _currentPlaylistId;
        private bool _shuffle;
        private RepeatMode _repeatMode = RepeatMode.Off;
        private bool _draggingProgress;
        private float _playbackRate = 1F;
        private bool _fullScreen;
        private FormBorderStyle _previousBorderStyle;
        private FormWindowState _previousWindowState;
        private WaveformVisualizer _audioVisualizer;
        private MediaItem _currentMediaItem;
        private int _volumeBeforeMute = 70;
        private bool _syncingVolumeUi;
        private Panel _searchShell;
        private Label _searchGlyph;
        private bool _searchFocused;
        private DarkVScrollBar _libraryScrollBar;
        private Label _lblPlaybackSection;
        private Label _lblPlaybackStatus;
        private Label _lblPlaybackPosition;
        private Label _lblPlaybackMode;
        private Panel _detailsDivider;

        public MainForm()
        {
            InitializeComponent();
            // Keep the normal window compact so the content/right panel gets more vertical room.
            rootLayout.RowStyles[0].Height = NormalTopBarHeight;
            rootLayout.RowStyles[2].Height = NormalTransportHeight;
            DoubleBuffered = true;
            KeyPreview = true;

            _library = new MediaLibraryService();
            ApplyTheme();
            ConfigureTopSearch();
            ConfigureAudioExperience();
            ConfigureLibraryGrid();
            ConfigureLibraryScrollBar();
            RefineNowPlayingPanel();
            WireEvents();
            LoadPlaylists();
            RestorePlaybackSettings();
            SetLibraryView(LibraryView.All, null);
            audioOverlay.BringToFront();
            InitializePlayback();
            UpdateNowPlayingDetails(null);
            UpdateQueueUi();
            ReflowLayout();
        }

        private void ConfigureTopSearch()
        {
            topBar.Controls.Remove(txtSearch);

            _searchShell = new Panel
            {
                Height = 44,
                Width = 500,
                BackColor = UiTheme.Input,
                Padding = new Padding(1),
                TabStop = false
            };
            _searchShell.Paint += delegate(object sender, PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var color = _searchFocused ? UiTheme.AccentSoft : UiTheme.Border;
                var rect = new Rectangle(1, 1, Math.Max(1, _searchShell.ClientSize.Width - 3), Math.Max(1, _searchShell.ClientSize.Height - 3));
                using (var path = CreateRoundedRectangle(rect, 20))
                using (var pen = new Pen(color, _searchFocused ? 1.4F : 1F))
                    e.Graphics.DrawPath(pen, path);
            };
            _searchShell.Resize += delegate { ApplyRoundedRegion(_searchShell, 21); };

            _searchGlyph = new Label
            {
                AutoSize = false,
                Text = "\uE721",
                Font = new Font("Segoe MDL2 Assets", 12F, FontStyle.Regular),
                ForeColor = UiTheme.MutedText,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(10, 7),
                Size = new Size(32, 30),
                TabStop = false
            };

            txtSearch.BorderStyle = BorderStyle.None;
            txtSearch.BackColor = UiTheme.Input;
            txtSearch.ForeColor = UiTheme.Text;
            txtSearch.Font = new Font("Segoe UI", 10.5F);
            txtSearch.Location = new Point(47, 11);
            txtSearch.Size = new Size(438, 24);
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.Margin = Padding.Empty;

            _searchShell.Controls.Add(_searchGlyph);
            _searchShell.Controls.Add(txtSearch);
            topBar.Controls.Add(_searchShell);
            _searchShell.BringToFront();
            ApplyRoundedRegion(_searchShell, 21);

            txtSearch.Enter += delegate
            {
                _searchFocused = true;
                _searchShell.Invalidate();
            };
            txtSearch.Leave += delegate
            {
                _searchFocused = false;
                _searchShell.Invalidate();
            };

            NativeTheme.SetCueBanner(txtSearch, "What do you want to play?");
        }

        private static System.Drawing.Drawing2D.GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            var diameter = Math.Max(2, radius * 2);
            var arc = new Rectangle(bounds.X, bounds.Y, diameter, diameter);
            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.X;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static void ApplyRoundedRegion(Control control, int radius)
        {
            if (control == null || control.Width <= 1 || control.Height <= 1) return;
            using (var path = CreateRoundedRectangle(new Rectangle(0, 0, control.Width, control.Height), radius))
            {
                var old = control.Region;
                control.Region = new Region(path);
                if (old != null) old.Dispose();
            }
        }

        private void ConfigureLibraryScrollBar()
        {
            gridLibrary.ScrollBars = ScrollBars.None;
            gridLibrary.Dock = DockStyle.None;

            _libraryScrollBar = new DarkVScrollBar
            {
                Width = 11,
                BackColor = UiTheme.Surface,
                Visible = false
            };

            libraryPanel.Controls.Add(_libraryScrollBar);
            _libraryScrollBar.BringToFront();

            _libraryScrollBar.ValueChanged += delegate
            {
                if (gridLibrary.RowCount == 0) return;
                var target = Math.Max(0, Math.Min(gridLibrary.RowCount - 1, _libraryScrollBar.Value));
                try { gridLibrary.FirstDisplayedScrollingRowIndex = target; } catch { }
            };

            gridLibrary.MouseWheel += delegate(object sender, MouseEventArgs e)
            {
                if (!_libraryScrollBar.Visible) return;
                var step = e.Delta > 0 ? -3 : 3;
                _libraryScrollBar.Value += step;
                var handled = e as HandledMouseEventArgs;
                if (handled != null) handled.Handled = true;
            };

            gridLibrary.Scroll += delegate { SyncLibraryScrollBarFromGrid(); };
            gridLibrary.SizeChanged += delegate { UpdateLibraryScrollBar(); };
            libraryPanel.Resize += delegate { LayoutLibraryGrid(); };
        }

        private void LayoutLibraryGrid()
        {
            if (_libraryScrollBar == null) return;

            var top = libraryHeader.Bottom;
            var height = Math.Max(0, libraryPanel.ClientSize.Height - top - 1);
            var barWidth = _libraryScrollBar.Visible ? _libraryScrollBar.Width + 2 : 1;

            gridLibrary.Location = new Point(1, top);
            gridLibrary.Size = new Size(Math.Max(50, libraryPanel.ClientSize.Width - barWidth - 1), height);

            _libraryScrollBar.Location = new Point(Math.Max(0, libraryPanel.ClientSize.Width - _libraryScrollBar.Width - 1), top + 1);
            _libraryScrollBar.Height = Math.Max(40, height - 2);
            _libraryScrollBar.BringToFront();
        }

        private void UpdateLibraryScrollBar()
        {
            if (_libraryScrollBar == null) return;

            var displayed = Math.Max(1, gridLibrary.DisplayedRowCount(false));
            var maximum = Math.Max(0, gridLibrary.RowCount - displayed);
            _libraryScrollBar.LargeChange = displayed;
            _libraryScrollBar.Maximum = maximum;
            _libraryScrollBar.Visible = maximum > 0;
            SyncLibraryScrollBarFromGrid();
            LayoutLibraryGrid();
        }

        private void SyncLibraryScrollBarFromGrid()
        {
            if (_libraryScrollBar == null || !_libraryScrollBar.Visible || gridLibrary.RowCount == 0) return;
            try
            {
                var first = gridLibrary.FirstDisplayedScrollingRowIndex;
                if (first >= 0) _libraryScrollBar.Value = Math.Min(_libraryScrollBar.Maximum, first);
            }
            catch { }
        }

        private void ConfigureAudioExperience()
        {
            lblAudioIcon.Visible = false;
            lblAudioTitle.Visible = false;
            lblAudioHint.Visible = false;

            _audioVisualizer = new WaveformVisualizer();
            _audioVisualizer.Dock = DockStyle.Fill;
            _audioVisualizer.SetArtwork(null);
            _audioVisualizer.DoubleClick += delegate { ToggleFullscreen(); };
            audioOverlay.Controls.Add(_audioVisualizer);
            _audioVisualizer.BringToFront();
        }

        private void RefineNowPlayingPanel()
        {
            // The right panel is responsive: at smaller window heights the artwork/header
            // becomes more compact so ABOUT THIS TRACK + PLAYBACK remain fully visible.
            queueLayout.RowStyles[1].Height = 280F;
            picQueueBanner.Height = 190;
            picQueueBanner.SizeMode = PictureBoxSizeMode.Zoom;

            trackDetailsPanel.Padding = new Padding(14, 10, 14, 8);
            lblDetailsSection.Text = "ABOUT THIS TRACK";

            _detailsDivider = new Panel
            {
                BackColor = UiTheme.Border,
                Height = 1,
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };
            trackDetailsPanel.Controls.Add(_detailsDivider);

            _lblPlaybackSection = new Label
            {
                AutoSize = true,
                Text = "PLAYBACK",
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                ForeColor = UiTheme.AccentSoft,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Left | AnchorStyles.Top
            };
            _lblPlaybackStatus = CreateRightInfoLabel();
            _lblPlaybackPosition = CreateRightInfoLabel();
            _lblPlaybackMode = CreateRightInfoLabel();
            _lblPlaybackMode.Visible = false;

            trackDetailsPanel.Controls.Add(_lblPlaybackSection);
            trackDetailsPanel.Controls.Add(_lblPlaybackStatus);
            trackDetailsPanel.Controls.Add(_lblPlaybackPosition);
            trackDetailsPanel.Controls.Add(_lblPlaybackMode);

            AdjustRightPanelHeight();
            LayoutNowPlayingHero();
            LayoutNowPlayingDetails();
            queueLayout.Resize += delegate { AdjustRightPanelHeight(); LayoutNowPlayingHero(); LayoutNowPlayingDetails(); };
            queueHero.Resize += delegate { LayoutNowPlayingHero(); };
            trackDetailsPanel.Resize += delegate { LayoutNowPlayingDetails(); };
            UpdatePlaybackInfo();
        }

        private Label CreateRightInfoLabel()
        {
            return new Label
            {
                AutoEllipsis = true,
                Font = new Font("Segoe UI", 8.75F),
                ForeColor = UiTheme.MutedText,
                BackColor = Color.Transparent,
                Height = 22,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };
        }

        private void AdjustRightPanelHeight()
        {
            if (queueLayout == null || queueLayout.RowStyles.Count < 4) return;

            var available = queueLayout.ClientSize.Height;
            float heroHeight;

            if (available <= 650)
                heroHeight = 220F;
            else if (available <= 740)
                heroHeight = 245F;
            else if (available <= 820)
                heroHeight = 275F;
            else
                heroHeight = 318F;

            queueLayout.RowStyles[1].Height = heroHeight;

            // Reserve enough space for the details/playback block and the fixed action row.
            var detailsHeight = Math.Max(290F, available - 38F - heroHeight - 58F);
            queueLayout.RowStyles[2].SizeType = SizeType.Absolute;
            queueLayout.RowStyles[2].Height = detailsHeight;
        }

        private void LayoutNowPlayingHero()
        {
            var availableHeroHeight = Math.Max(180, queueHero.ClientSize.Height);
            var maxImageByHeight = Math.Max(128, availableHeroHeight - 106);
            var imageHeight = Math.Min(210, Math.Min(maxImageByHeight, Math.Max(148, queueHero.ClientSize.Width - 92)));
            picQueueBanner.Height = imageHeight;
            var top = picQueueBanner.Bottom + 10;
            lblQueueBannerTitle.Location = new Point(0, top);
            lblQueueBannerMeta.Location = new Point(0, top + 34);
            lblQueueBannerHint.Location = new Point(0, top + 59);
            lblQueueBannerTitle.Width = queueHero.ClientSize.Width;
            lblQueueBannerMeta.Width = queueHero.ClientSize.Width;
            lblQueueBannerHint.Width = queueHero.ClientSize.Width;
        }

        private void LayoutNowPlayingDetails()
        {
            if (_detailsDivider == null) return;

            var width = Math.Max(80, trackDetailsPanel.ClientSize.Width - 28);
            lblDetailsSection.Location = new Point(14, 10);
            lblDetailsType.Location = new Point(14, 42);
            lblDetailsPlays.Location = new Point(14, 68);
            lblDetailsAdded.Location = new Point(14, 94);
            lblDetailsLocation.Location = new Point(14, 120);
            lblDetailsLocation.Size = new Size(width, 42);
            lblDetailsFavorite.Location = new Point(14, 170);
            lblDetailsFavorite.Width = width;

            _detailsDivider.Location = new Point(14, 202);
            _detailsDivider.Width = width;

            _lblPlaybackSection.Location = new Point(14, 214);
            _lblPlaybackStatus.Location = new Point(14, 240);
            _lblPlaybackPosition.Location = new Point(14, 264);
            _lblPlaybackStatus.Width = width;
            _lblPlaybackPosition.Width = width;
            _lblPlaybackMode.Width = width;
        }

        private void UpdatePlaybackInfo()
        {
            if (_lblPlaybackStatus == null) return;
            var state = _playback == null ? "Unavailable" : (_playback.IsPlaying ? "Playing" : (_currentMediaItem == null ? "Idle" : "Paused"));
            var repeat = _repeatMode == RepeatMode.Off ? "Off" : (_repeatMode == RepeatMode.All ? "All" : "One");
            _lblPlaybackStatus.Text = state + "  •  " + lblElapsed.Text + " / " + lblDuration.Text;
            _lblPlaybackPosition.Text = _playbackRate.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + "x  •  Repeat " + repeat + "  •  Shuffle " + (_shuffle ? "On" : "Off");
        }

        private void InitializePlayback()
        {
            try
            {
                _playback = new PlaybackService(videoView);
                _playback.Playing += delegate { Ui(delegate { btnPlayPause.Text = "Ⅱ"; lblStatus.Text = "Playing"; if (_audioVisualizer != null) _audioVisualizer.SetPlaying(true); UpdatePlaybackInfo(); }); };
                _playback.Paused += delegate { Ui(delegate { btnPlayPause.Text = "▶"; lblStatus.Text = "Paused"; if (_audioVisualizer != null) _audioVisualizer.SetPlaying(false); UpdatePlaybackInfo(); }); };
                _playback.Stopped += delegate { Ui(delegate { btnPlayPause.Text = "▶"; lblStatus.Text = "Stopped"; if (_audioVisualizer != null) _audioVisualizer.SetPlaying(false); ResetProgress(); UpdatePlaybackInfo(); }); };
                _playback.EndReached += delegate { Ui(HandleTrackEnded); };
                _playback.PlaybackError += delegate { Ui(delegate { lblStatus.Text = "Playback error"; MessageBox.Show(this, "The selected media could not be played. Try another file or verify that it is not damaged.", "Playback Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); }); };
                _playback.PositionChanged += delegate(long time, long length) { Ui(delegate { UpdateProgress(time, length); }); };
                _playback.SetVolume(_volumeBeforeMute);
                _playback.SetMute(_library.IsMuted);
                UpdateMuteButtonVisual(_library.IsMuted);
            }
            catch (Exception ex)
            {
                _playback = null;
                lblStatus.Text = "Playback engine unavailable";
                MessageBox.Show(this,
                    "not.asser opened, but the LibVLC playback engine could not initialize.\n\n" +
                    "In Visual Studio, restore NuGet packages, rebuild the solution, and make sure you are running the x64 configuration.\n\nDetails: " + ex.Message,
                    "Playback Engine",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void ApplyTheme()
        {
            BackColor = UiTheme.Background;
            ForeColor = UiTheme.Text;

            rootLayout.BackColor = UiTheme.Background;
            topBar.BackColor = UiTheme.TopBar;
            contentLayout.BackColor = UiTheme.Background;
            sidebar.BackColor = UiTheme.Sidebar;
            queuePanel.BackColor = UiTheme.Sidebar;
            transportPanel.BackColor = UiTheme.TopBar;
            mainSplit.BackColor = UiTheme.Background;
            videoHost.BackColor = UiTheme.Border;
            stageToolbar.BackColor = UiTheme.Surface;
            videoSurface.BackColor = Color.Black;
            audioOverlay.BackColor = UiTheme.PlayerSurface;
            libraryPanel.BackColor = UiTheme.Border;
            libraryHeader.BackColor = UiTheme.Surface;
            queueActions.BackColor = UiTheme.Sidebar;
            queueHeader.BackColor = UiTheme.Sidebar;
            queueHero.BackColor = UiTheme.Sidebar;
            trackDetailsPanel.BackColor = UiTheme.Surface;
            picQueueBanner.BackColor = UiTheme.PlayerSurface;
            centerTransportPanel.BackColor = UiTheme.TopBar;
            nowPlayingPanel.BackColor = UiTheme.TopBar;
            volumePanel.BackColor = UiTheme.TopBar;

            foreach (var label in ControlsRecursive<Label>(this))
                label.ForeColor = UiTheme.Text;

            lblBrandMark.Visible = false;
            lblBrandMark.BackColor = UiTheme.Accent;
            lblBrandMark.ForeColor = Color.White;
            lblCurrentView.ForeColor = UiTheme.MutedText;
            lblLibrarySection.ForeColor = UiTheme.SubtleText;
            lblPlaylistSection.ForeColor = UiTheme.SubtleText;
            lblLibraryCount.ForeColor = UiTheme.MutedText;
            lblQueueCount.ForeColor = UiTheme.MutedText;
            lblQueueBannerTitle.ForeColor = UiTheme.Text;
            lblQueueBannerMeta.ForeColor = UiTheme.MutedText;
            lblQueueBannerHint.ForeColor = UiTheme.SubtleText;
            lblDetailsSection.ForeColor = UiTheme.AccentSoft;
            lblDetailsType.ForeColor = UiTheme.MutedText;
            lblDetailsPlays.ForeColor = UiTheme.MutedText;
            lblDetailsAdded.ForeColor = UiTheme.MutedText;
            lblDetailsLocation.ForeColor = UiTheme.MutedText;
            lblDetailsFavorite.ForeColor = UiTheme.Text;
            lblNowPlayingCaption.ForeColor = UiTheme.SubtleText;
            lblNowPlayingMeta.ForeColor = UiTheme.MutedText;
            lblStatus.ForeColor = UiTheme.AccentSoft;
            lblAudioIcon.ForeColor = UiTheme.AccentSoft;
            lblAudioHint.ForeColor = UiTheme.MutedText;
            lblElapsed.ForeColor = UiTheme.MutedText;
            lblDuration.ForeColor = UiTheme.MutedText;
            lblVolume.ForeColor = UiTheme.MutedText;
            lblStageTitle.ForeColor = UiTheme.SubtleText;
            lblStageHint.ForeColor = UiTheme.MutedText;

            txtSearch.BackColor = UiTheme.Input;
            txtSearch.ForeColor = UiTheme.Text;
            txtSearch.BorderStyle = BorderStyle.FixedSingle;

            UiTheme.StylePrimaryButton(btnAddFiles);
            UiTheme.StyleSecondaryButton(btnAddFolder);
            UiTheme.StyleNavButton(btnAll);
            UiTheme.StyleNavButton(btnMusic);
            UiTheme.StyleNavButton(btnVideos);
            UiTheme.StyleNavButton(btnFavorites);
            UiTheme.StyleNavButton(btnRecent);
            UiTheme.StyleSecondaryButton(btnNewPlaylist);
            UiTheme.StyleSecondaryButton(btnPlaylistAddSelected);
            UiTheme.StyleSecondaryButton(btnPlaylistRemoveSelected);
            UiTheme.StyleDangerButton(btnDeletePlaylist);
            UiTheme.StyleSecondaryButton(btnQueueUp);
            UiTheme.StyleSecondaryButton(btnQueueDown);
            UiTheme.StyleSecondaryButton(btnQueueRemove);
            UiTheme.StyleSecondaryButton(btnQueueClear);
            UiTheme.StyleSecondaryButton(btnPrevious);
            UiTheme.StylePrimaryButton(btnPlayPause);
            UiTheme.StyleSecondaryButton(btnStop);
            UiTheme.StyleSecondaryButton(btnNext);
            UiTheme.StyleSecondaryButton(btnShuffle);
            UiTheme.StyleSecondaryButton(btnRepeat);
            UiTheme.StyleSecondaryButton(btnMute);
            UiTheme.StyleSecondaryButton(btnFullscreen);
            btnFullscreen.Text = string.Empty;
            btnFullscreen.Size = new Size(44, 40);
            btnFullscreen.BackColor = UiTheme.TopBar;
            btnFullscreen.FlatAppearance.BorderSize = 0;
            btnFullscreen.AccessibleName = "Enter full screen";
            UiTheme.StyleSecondaryButton(btnCleanMissing);

            UiTheme.StyleComboBox(cmbSort);
            UiTheme.StyleComboBox(cmbSpeed);

            lstPlaylists.BackColor = UiTheme.Sidebar;
            lstPlaylists.ForeColor = UiTheme.Text;
            lstPlaylists.BorderStyle = BorderStyle.None;

            listQueue.BackColor = UiTheme.Sidebar;
            listQueue.ForeColor = UiTheme.Text;
            listQueue.BorderStyle = BorderStyle.FixedSingle;

            UiTheme.StyleGrid(gridLibrary);
        }

        private void ConfigureLibraryGrid()
        {
            gridLibrary.Columns.Clear();
            gridLibrary.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Favorite",
                HeaderText = "",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 34,
                MinimumWidth = 34
            });
            gridLibrary.Columns.Add(new DataGridViewImageColumn
            {
                Name = "Artwork",
                HeaderText = "",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 54,
                MinimumWidth = 54,
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    NullValue = null,
                    Padding = new Padding(6, 4, 6, 4),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
            gridLibrary.Columns.Add(new DataGridViewTextBoxColumn { Name = "Title", HeaderText = "TITLE", FillWeight = 31F, MinimumWidth = 180 });
            gridLibrary.Columns.Add(new DataGridViewTextBoxColumn { Name = "Type", HeaderText = "TYPE", FillWeight = 10F, MinimumWidth = 66 });
            gridLibrary.Columns.Add(new DataGridViewTextBoxColumn { Name = "Artist", HeaderText = "ARTIST", FillWeight = 25F, MinimumWidth = 150 });
            gridLibrary.Columns.Add(new DataGridViewTextBoxColumn { Name = "Added", HeaderText = "ADDED", FillWeight = 12F, MinimumWidth = 86 });
            gridLibrary.Columns.Add(new DataGridViewTextBoxColumn { Name = "Plays", HeaderText = "PLAYS", FillWeight = 8F, MinimumWidth = 54 });
            gridLibrary.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "STATUS", FillWeight = 10F, MinimumWidth = 72 });
            gridLibrary.RowTemplate.Height = 48;

            var menu = new ContextMenuStrip();
            menu.BackColor = UiTheme.Surface;
            menu.ForeColor = UiTheme.Text;
            menu.Items.Add("Play now", null, delegate { PlaySelectedLibraryItem(); });
            menu.Items.Add("Play next", null, delegate { PlaySelectedNext(); });
            menu.Items.Add("Add to queue", null, delegate { AddSelectedLibraryItemToQueue(); });
            menu.Items.Add("Favorite / Unfavorite", null, delegate { ToggleSelectedFavorite(); });
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Add to selected playlist", null, delegate { AddSelectedToCurrentPlaylist(); });
            menu.Items.Add("Remove from selected playlist", null, delegate { RemoveSelectedFromCurrentPlaylist(); });
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Open containing folder", null, delegate { OpenContainingFolder(); });
            menu.Items.Add("Copy file path", null, delegate { CopySelectedPath(); });
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Remove from library", null, delegate { RemoveSelectedFromLibrary(); });
            gridLibrary.ContextMenuStrip = menu;
        }

        private void WireEvents()
        {
            btnAll.Click += delegate { SetLibraryView(LibraryView.All, null); };
            btnMusic.Click += delegate { SetLibraryView(LibraryView.Music, null); };
            btnVideos.Click += delegate { SetLibraryView(LibraryView.Videos, null); };
            btnFavorites.Click += delegate { SetLibraryView(LibraryView.Favorites, null); };
            btnRecent.Click += delegate { SetLibraryView(LibraryView.Recent, null); };
            txtSearch.TextChanged += delegate { RefreshLibrary(); };
            cmbSort.SelectedIndexChanged += delegate { RefreshLibrary(); };
            btnAddFiles.Click += delegate { ImportFiles(); };
            btnAddFolder.Click += delegate { ImportFolder(); };
            btnCleanMissing.Click += delegate { CleanMissingItems(); };
            gridLibrary.CellDoubleClick += delegate(object sender, DataGridViewCellEventArgs e) { if (e.RowIndex >= 0) PlaySelectedLibraryItem(); };

            btnNewPlaylist.Click += delegate { CreatePlaylist(); };
            lstPlaylists.DrawMode = DrawMode.OwnerDrawFixed;
            lstPlaylists.ItemHeight = 36;
            lstPlaylists.DrawItem += DrawPlaylistItem;
            lstPlaylists.SelectedIndexChanged += delegate { SelectPlaylistView(); };
            btnPlaylistAddSelected.Click += delegate { AddSelectedToCurrentPlaylist(); };
            btnPlaylistRemoveSelected.Click += delegate { RemoveSelectedFromCurrentPlaylist(); };
            btnDeletePlaylist.Click += delegate { DeleteSelectedPlaylist(); };

            listQueue.OwnerDraw = true;
            listQueue.DrawColumnHeader += DrawQueueColumnHeader;
            listQueue.DrawItem += DrawQueueItem;
            listQueue.DrawSubItem += DrawQueueSubItem;

            picQueueBanner.DoubleClick += delegate { ChooseCurrentArtwork(); };
            picQueueBanner.Paint += DrawArtworkPlaceholder;
            picQueueBanner.Cursor = Cursors.Hand;
            var artworkMenu = new ContextMenuStrip();
            artworkMenu.BackColor = UiTheme.Surface;
            artworkMenu.ForeColor = UiTheme.Text;
            artworkMenu.Items.Add("Choose artwork", null, delegate { ChooseCurrentArtwork(); });
            artworkMenu.Items.Add("Remove artwork", null, delegate { ClearCurrentArtwork(); });
            picQueueBanner.ContextMenuStrip = artworkMenu;

            btnQueueUp.Click += delegate { ToggleCurrentFavorite(); };
            btnQueueDown.Click += delegate { ChooseCurrentArtwork(); };
            btnQueueRemove.Click += delegate { OpenCurrentMediaFolder(); };
            btnQueueClear.Click += delegate { CopyCurrentMediaPath(); };

            btnPlayPause.Click += delegate { TogglePlayPause(); };
            btnStop.Click += delegate { if (_playback != null) _playback.Stop(); };
            btnPrevious.Click += delegate { PreviousTrack(); };
            btnNext.Click += delegate { NextTrack(true); };
            btnShuffle.Click += delegate { ToggleShuffle(); };
            btnRepeat.Click += delegate { CycleRepeatMode(); };
            btnMute.Click += delegate { ToggleMute(); };
            btnMute.Paint += DrawMuteButtonIcon;
            btnFullscreen.Click += delegate { ToggleFullscreen(); };
            btnFullscreen.Paint += DrawFullscreenButtonIcon;
            cmbSpeed.SelectedIndexChanged += delegate { ChangePlaybackRate(); };
            videoView.DoubleClick += delegate { ToggleFullscreen(); };
            audioOverlay.DoubleClick += delegate { ToggleFullscreen(); };
            trackVolume.ValueChanged += delegate { SetVolumeFromUi(); };
            trackProgress.MouseDown += delegate { _draggingProgress = true; };
            trackProgress.MouseUp += delegate { SeekFromProgressBar(); _draggingProgress = false; };

            DragEnter += MainForm_DragEnter;
            DragDrop += MainForm_DragDrop;
            FormClosing += MainForm_FormClosing;
            Resize += delegate { ReflowLayout(); LayoutNowPlayingHero(); };
            gridLibrary.HandleCreated += delegate { NativeTheme.ApplyDarkScrollableControls(gridLibrary); };
            lstPlaylists.HandleCreated += delegate { NativeTheme.ApplyDarkScrollableControls(lstPlaylists); };
            cmbSort.HandleCreated += delegate { NativeTheme.ApplyDarkScrollableControls(cmbSort); };
            Shown += delegate { ReflowLayout(); LayoutNowPlayingHero(); UpdateLibraryScrollBar(); NativeTheme.ApplyDarkScrollableControls(this); };
        }

        private void DrawPlaylistItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= lstPlaylists.Items.Count) return;

            var playlist = lstPlaylists.Items[e.Index] as Playlist;
            var selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            var backgroundColor = selected ? UiTheme.SurfaceAlt : UiTheme.Sidebar;

            using (var background = new SolidBrush(backgroundColor))
                e.Graphics.FillRectangle(background, e.Bounds);

            if (selected)
            {
                using (var accent = new SolidBrush(UiTheme.Accent))
                    e.Graphics.FillRectangle(accent, e.Bounds.X, e.Bounds.Y + 5, 3, e.Bounds.Height - 10);
            }

            var numberText = (e.Index + 1).ToString("00");
            var nameText = playlist == null ? lstPlaylists.Items[e.Index].ToString() : playlist.Name;
            var countText = playlist == null ? string.Empty : playlist.MediaIds.Count.ToString();

            using (var numberFont = new Font("Segoe UI Semibold", 8F, FontStyle.Bold))
            using (var nameFont = new Font("Segoe UI", 9F, selected ? FontStyle.Bold : FontStyle.Regular))
            using (var numberBrush = new SolidBrush(selected ? UiTheme.AccentSoft : UiTheme.SubtleText))
            using (var nameBrush = new SolidBrush(selected ? UiTheme.Text : UiTheme.MutedText))
            using (var countBrush = new SolidBrush(UiTheme.SubtleText))
            {
                e.Graphics.DrawString(numberText, numberFont, numberBrush, e.Bounds.X + 10, e.Bounds.Y + 9);
                e.Graphics.DrawString(nameText, nameFont, nameBrush, new RectangleF(e.Bounds.X + 40, e.Bounds.Y + 7, e.Bounds.Width - 80, e.Bounds.Height - 8));

                if (!string.IsNullOrWhiteSpace(countText))
                {
                    var countSize = e.Graphics.MeasureString(countText, numberFont);
                    e.Graphics.DrawString(countText, numberFont, countBrush, e.Bounds.Right - countSize.Width - 8, e.Bounds.Y + 9);
                }
            }

            if (selected) e.DrawFocusRectangle();
        }

        private void DrawQueueColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var background = new SolidBrush(UiTheme.SurfaceAlt)) e.Graphics.FillRectangle(background, e.Bounds);
            using (var brush = new SolidBrush(UiTheme.SubtleText)) e.Graphics.DrawString(e.Header.Text, new Font("Segoe UI Semibold", 8.5F), brush, e.Bounds.X + 8, e.Bounds.Y + 6);
            using (var pen = new Pen(UiTheme.Border)) e.Graphics.DrawLine(pen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
        }

        private void DrawQueueItem(object sender, DrawListViewItemEventArgs e)
        {
            if (listQueue.View != View.Details) e.DrawDefault = true;
        }

        private void DrawQueueSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            var selected = e.Item.Selected;
            using (var background = new SolidBrush(selected ? Color.FromArgb(48, 40, 94) : UiTheme.Sidebar)) e.Graphics.FillRectangle(background, e.Bounds);
            using (var brush = new SolidBrush(selected ? UiTheme.Text : UiTheme.MutedText)) e.Graphics.DrawString(e.SubItem.Text, listQueue.Font, brush, e.Bounds.X + 8, e.Bounds.Y + 5);
        }

        private void ReflowLayout()
        {
            if (!_fullScreen && topBar.ClientSize.Width > 0)
            {
                var right = topBar.ClientSize.Width - 20;
                btnAddFolder.Left = right - btnAddFolder.Width;
                btnAddFiles.Left = btnAddFolder.Left - 10 - btnAddFiles.Width;

                if (_searchShell != null)
                {
                    var desiredWidth = Math.Min(520, Math.Max(360, topBar.ClientSize.Width / 3));
                    var centeredLeft = (topBar.ClientSize.Width - desiredWidth) / 2;
                    var maxRight = btnAddFiles.Left - 22;
                    if (centeredLeft + desiredWidth > maxRight)
                        centeredLeft = Math.Max(310, maxRight - desiredWidth);
                    _searchShell.Width = desiredWidth;
                    _searchShell.Left = centeredLeft;
                    _searchShell.Top = 18;
                    txtSearch.Width = Math.Max(100, _searchShell.ClientSize.Width - 58);
                }
            }

            if (libraryHeader.ClientSize.Width > 0)
            {
                btnCleanMissing.Left = Math.Max(310, libraryHeader.ClientSize.Width - btnCleanMissing.Width - 10);
                cmbSort.Left = Math.Max(164, btnCleanMissing.Left - cmbSort.Width - 8);
            }

            if (queuePanel.ClientSize.Width > 0)
            {
                lblQueueCount.Left = Math.Max(lblQueueTitle.Right + 12, queueHeader.ClientSize.Width - lblQueueCount.Width);
                AdjustRightPanelHeight();
                LayoutNowPlayingHero();
                LayoutNowPlayingDetails();
            }

            LayoutLibraryGrid();

            if (stageToolbar.ClientSize.Width > 0)
                lblStageHint.Left = Math.Max(lblStageTitle.Right + 20, stageToolbar.ClientSize.Width - lblStageHint.Width - 12);

            LayoutTransportBar();
        }

        private void LayoutTransportBar()
        {
            if (transportLayout.ColumnStyles.Count >= 3)
            {
                transportLayout.ColumnStyles[0].Width = _fullScreen ? 27F : 31F;
                transportLayout.ColumnStyles[1].Width = _fullScreen ? 50F : 44F;
                transportLayout.ColumnStyles[2].Width = _fullScreen ? 23F : 25F;
            }

            LayoutNowPlayingTransportText();
            LayoutCenterTransportControls();
            LayoutVolumeControls();
        }

        private void LayoutNowPlayingTransportText()
        {
            if (nowPlayingPanel.ClientSize.Width <= 0) return;

            var usableWidth = Math.Max(80, nowPlayingPanel.ClientSize.Width - 20);
            lblNowPlayingTitle.Width = usableWidth;
            lblNowPlayingMeta.Width = usableWidth;
            lblStatus.Width = usableWidth;

            if (_fullScreen)
            {
                lblNowPlayingCaption.Visible = false;
                lblStatus.Visible = false;
                lblNowPlayingTitle.Top = 20;
                lblNowPlayingMeta.Top = 51;
            }
            else
            {
                lblNowPlayingCaption.Visible = true;
                lblStatus.Visible = true;
                lblNowPlayingCaption.Top = 5;
                lblNowPlayingTitle.Top = 34;
                lblNowPlayingMeta.Top = 65;
                lblStatus.Top = 88;
            }
        }

        private void LayoutCenterTransportControls()
        {
            if (centerTransportPanel.ClientSize.Width <= 0) return;

            var compact = centerTransportPanel.ClientSize.Width < 610;
            btnShuffle.Visible = !compact;
            btnRepeat.Visible = !compact;

            lblElapsed.Top = _fullScreen ? 7 : 10;
            lblDuration.Top = lblElapsed.Top;
            trackProgress.Top = _fullScreen ? -3 : 0;

            lblDuration.Left = Math.Max(48, centerTransportPanel.ClientSize.Width - lblDuration.Width - 2);
            trackProgress.Left = lblElapsed.Right + 8;
            trackProgress.Width = Math.Max(80, lblDuration.Left - 8 - trackProgress.Left);

            var controls = new List<Control>();
            if (btnShuffle.Visible) controls.Add(btnShuffle);
            controls.Add(btnPrevious);
            controls.Add(btnPlayPause);
            controls.Add(btnStop);
            controls.Add(btnNext);
            if (btnRepeat.Visible) controls.Add(btnRepeat);

            var spacing = _fullScreen ? 10 : 8;
            var total = controls.Sum(c => c.Width) + (spacing * Math.Max(0, controls.Count - 1));
            var x = Math.Max(4, (centerTransportPanel.ClientSize.Width - total) / 2);
            var rowTop = _fullScreen ? 39 : 37;

            foreach (var control in controls)
            {
                control.Left = x;
                control.Top = rowTop + Math.Max(0, (48 - control.Height) / 2);
                x += control.Width + spacing;
            }
        }

        private void LayoutVolumeControls()
        {
            if (volumePanel.ClientSize.Width <= 0) return;

            btnFullscreen.Size = new Size(44, 40);
            btnFullscreen.Left = Math.Max(4, volumePanel.ClientSize.Width - btnFullscreen.Width - 6);
            btnFullscreen.Top = _fullScreen ? 31 : 46;

            if (_fullScreen)
            {
                cmbSpeed.Visible = false;
                lblVolume.Visible = false;
                btnMute.Top = 31;
                trackVolume.Top = 30;
                btnMute.Left = 8;
                trackVolume.Left = btnMute.Right + 8;
                trackVolume.Width = Math.Max(70, btnFullscreen.Left - trackVolume.Left - 12);
            }
            else
            {
                cmbSpeed.Visible = true;
                lblVolume.Visible = true;
                btnMute.Top = 22;
                trackVolume.Top = 21;
                btnMute.Left = 80;
                lblVolume.Left = Math.Max(btnMute.Right + 82, btnFullscreen.Left - lblVolume.Width - 10);
                lblVolume.Top = 25;
                trackVolume.Left = btnMute.Right + 6;
                trackVolume.Width = Math.Max(52, lblVolume.Left - trackVolume.Left - 8);
                cmbSpeed.Left = Math.Max(trackVolume.Left, btnFullscreen.Left - cmbSpeed.Width - 10);
                cmbSpeed.Top = 60;
            } 
        }

        private void DrawFullscreenButtonIcon(object sender, PaintEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var color = button.Enabled ? UiTheme.Text : UiTheme.SubtleText;
            using (var pen = new Pen(color, 2F))
            {
                var cx = button.ClientSize.Width / 2;
                var cy = button.ClientSize.Height / 2;
                var size = 16;
                var left = cx - size / 2;
                var top = cy - size / 2;
                var right = cx + size / 2;
                var bottom = cy + size / 2;
                var arm = 5;

                if (!_fullScreen)
                {
                    e.Graphics.DrawLine(pen, left, top + arm, left, top);
                    e.Graphics.DrawLine(pen, left, top, left + arm, top);
                    e.Graphics.DrawLine(pen, right - arm, top, right, top);
                    e.Graphics.DrawLine(pen, right, top, right, top + arm);
                    e.Graphics.DrawLine(pen, left, bottom - arm, left, bottom);
                    e.Graphics.DrawLine(pen, left, bottom, left + arm, bottom);
                    e.Graphics.DrawLine(pen, right - arm, bottom, right, bottom);
                    e.Graphics.DrawLine(pen, right, bottom - arm, right, bottom);
                }
                else
                {
                    e.Graphics.DrawLine(pen, left, top, left + arm, top);
                    e.Graphics.DrawLine(pen, left + arm, top, left + arm, top + arm);
                    e.Graphics.DrawLine(pen, right - arm, top, right, top);
                    e.Graphics.DrawLine(pen, right - arm, top, right - arm, top + arm);
                    e.Graphics.DrawLine(pen, left, bottom, left + arm, bottom);
                    e.Graphics.DrawLine(pen, left + arm, bottom - arm, left + arm, bottom);
                    e.Graphics.DrawLine(pen, right - arm, bottom - arm, right - arm, bottom);
                    e.Graphics.DrawLine(pen, right - arm, bottom, right, bottom);
                }
            }
        }

        private void RestorePlaybackSettings()
        {
            _volumeBeforeMute = Math.Max(1, Math.Min(trackVolume.Maximum, _library.Volume <= 0 ? 70 : _library.Volume));
            _syncingVolumeUi = true;
            trackVolume.Value = _library.IsMuted ? 0 : _volumeBeforeMute;
            _syncingVolumeUi = false;
            lblVolume.Text = trackVolume.Value + "%";
            UpdateMuteButtonVisual(_library.IsMuted);
        }

        private void SetLibraryView(LibraryView view, string playlistId)
        {
            _currentView = view;
            _currentPlaylistId = playlistId;
            if (view != LibraryView.Playlist) lstPlaylists.ClearSelected();
            UpdateNavigationState();
            RefreshLibrary();
        }

        private void UpdateNavigationState()
        {
            UiTheme.SetNavActive(btnAll, _currentView == LibraryView.All);
            UiTheme.SetNavActive(btnMusic, _currentView == LibraryView.Music);
            UiTheme.SetNavActive(btnVideos, _currentView == LibraryView.Videos);
            UiTheme.SetNavActive(btnFavorites, _currentView == LibraryView.Favorites);
            UiTheme.SetNavActive(btnRecent, _currentView == LibraryView.Recent);

            var title = "All media";
            if (_currentView == LibraryView.Music) title = "Music";
            else if (_currentView == LibraryView.Videos) title = "Videos";
            else if (_currentView == LibraryView.Favorites) title = "Favorites";
            else if (_currentView == LibraryView.Recent) title = "Recently played";
            else if (_currentView == LibraryView.Playlist)
            {
                var playlist = SelectedPlaylist();
                title = playlist == null ? "Playlist" : playlist.Name;
            }

            lblCurrentView.Text = title;
            lblLibraryTitle.Text = title;
        }

        private void RefreshLibrary()
        {
            var selected = SelectedLibraryItem();
            var selectedId = selected == null ? null : selected.Id;
            IEnumerable<MediaItem> items = _library.GetItems(_currentView, _currentPlaylistId, txtSearch.Text);

            var sort = cmbSort.SelectedItem == null ? "Title" : cmbSort.SelectedItem.ToString();
            if (sort == "Recently played")
                items = items.OrderByDescending(item => item.LastPlayedUtc ?? DateTime.MinValue).ThenBy(item => item.Title, StringComparer.OrdinalIgnoreCase);
            else if (sort == "Most played")
                items = items.OrderByDescending(item => item.PlayCount).ThenBy(item => item.Title, StringComparer.OrdinalIgnoreCase);
            else if (sort == "Newest added")
                items = items.OrderByDescending(item => item.AddedUtc).ThenBy(item => item.Title, StringComparer.OrdinalIgnoreCase);
            else
                items = items.OrderBy(item => item.Title, StringComparer.OrdinalIgnoreCase);

            var materialized = items.ToList();
            DisposeLibraryThumbnails();
            gridLibrary.Rows.Clear();

            foreach (var item in materialized)
            {
                var rowIndex = gridLibrary.Rows.Add(
                    item.IsFavorite ? "★" : string.Empty,
                    CreateLibraryThumbnail(item),
                    item.Title,
                    item.MediaType,
                    MediaFileHelper.GetArtistFromTitle(item.Title),
                    item.AddedUtc == default(DateTime) ? "—" : item.AddedUtc.ToLocalTime().ToString("yyyy-MM-dd"),
                    item.PlayCount,
                    item.Exists ? "Ready" : "Missing");

                var row = gridLibrary.Rows[rowIndex];
                row.Tag = item;
                if (!item.Exists) row.DefaultCellStyle.ForeColor = UiTheme.Danger;
                if (item.Id == selectedId) row.Selected = true;
            }

            lblLibraryCount.Text = materialized.Count + (materialized.Count == 1 ? " item" : " items");
            UpdateLibraryScrollBar();
        }

        private void DisposeLibraryThumbnails()
        {
            if (!gridLibrary.Columns.Contains("Artwork")) return;

            foreach (DataGridViewRow row in gridLibrary.Rows)
            {
                var image = row.Cells["Artwork"].Value as Image;
                if (image != null)
                {
                    row.Cells["Artwork"].Value = null;
                    image.Dispose();
                }
            }
        }

        private Image CreateLibraryThumbnail(MediaItem item)
        {
            const int size = 38;
            var thumbnail = new Bitmap(size, size);

            using (var g = Graphics.FromImage(thumbnail))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.Clear(UiTheme.PlayerSurface);

                var bounds = new Rectangle(0, 0, size, size);
                var artworkLoaded = false;

                if (item != null && !string.IsNullOrWhiteSpace(item.ArtworkPath) && File.Exists(item.ArtworkPath))
                {
                    try
                    {
                        using (var source = Image.FromFile(item.ArtworkPath))
                        {
                            var destination = FitImageRectangle(source.Size, bounds);
                            g.DrawImage(source, destination);
                            artworkLoaded = true;
                        }
                    }
                    catch
                    {
                        artworkLoaded = false;
                    }
                }

                if (!artworkLoaded)
                {
                    using (var background = new System.Drawing.Drawing2D.LinearGradientBrush(
                        bounds,
                        Color.FromArgb(78, 61, 154),
                        Color.FromArgb(27, 31, 54),
                        45F))
                    {
                        g.FillRectangle(background, bounds);
                    }

                    var symbol = item != null && MediaFileHelper.IsVideo(item.Path) ? "▶" : "♪";
                    using (var font = new Font("Segoe UI Symbol", 16F, FontStyle.Regular, GraphicsUnit.Pixel))
                    using (var brush = new SolidBrush(UiTheme.AccentSoft))
                    {
                        var format = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };
                        g.DrawString(symbol, font, brush, bounds, format);
                    }
                }

                using (var border = new Pen(Color.FromArgb(80, UiTheme.Border), 1F))
                    g.DrawRectangle(border, 0, 0, size - 1, size - 1);
            }

            return thumbnail;
        }

        private static Rectangle FitImageRectangle(Size imageSize, Rectangle bounds)
        {
            if (imageSize.Width <= 0 || imageSize.Height <= 0) return bounds;

            var ratio = Math.Min((float)bounds.Width / imageSize.Width, (float)bounds.Height / imageSize.Height);
            var width = Math.Max(1, (int)Math.Round(imageSize.Width * ratio));
            var height = Math.Max(1, (int)Math.Round(imageSize.Height * ratio));
            return new Rectangle(
                bounds.X + (bounds.Width - width) / 2,
                bounds.Y + (bounds.Height - height) / 2,
                width,
                height);
        }

        private void LoadPlaylists(string selectId = null)
        {
            lstPlaylists.Items.Clear();
            foreach (var playlist in _library.Playlists)
                lstPlaylists.Items.Add(playlist);

            NativeTheme.ApplyDarkScrollableControls(lstPlaylists);

            if (!string.IsNullOrWhiteSpace(selectId))
            {
                for (var i = 0; i < lstPlaylists.Items.Count; i++)
                {
                    var playlist = lstPlaylists.Items[i] as Playlist;
                    if (playlist != null && string.Equals(playlist.Id, selectId, StringComparison.OrdinalIgnoreCase))
                    {
                        lstPlaylists.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void SelectPlaylistView()
        {
            var playlist = SelectedPlaylist();
            if (playlist == null) return;
            _currentView = LibraryView.Playlist;
            _currentPlaylistId = playlist.Id;
            UpdateNavigationState();
            RefreshLibrary();
        }

        private Playlist SelectedPlaylist()
        {
            return lstPlaylists.SelectedItem as Playlist;
        }

        private MediaItem SelectedLibraryItem()
        {
            if (gridLibrary.SelectedRows.Count == 0) return null;
            return gridLibrary.SelectedRows[0].Tag as MediaItem;
        }

        private void ImportFiles()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = MediaFileHelper.BuildOpenFileFilter();
                dialog.Multiselect = true;
                dialog.Title = "Add media to not.asser";
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                var count = _library.AddFiles(dialog.FileNames);
                RefreshLibrary();
                lblStatus.Text = count == 0 ? "No new media added" : "Added " + count + " media file(s)";
            }
        }

        private void ImportFolder()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Choose a folder. not.asser will scan it and its subfolders for supported media.";
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                Cursor = Cursors.WaitCursor;
                try
                {
                    var count = _library.AddFolder(dialog.SelectedPath);
                    RefreshLibrary();
                    lblStatus.Text = count == 0 ? "No new media found" : "Added " + count + " media file(s)";
                }
                finally { Cursor = Cursors.Default; }
            }
        }

        private void PlaySelectedLibraryItem()
        {
            var item = SelectedLibraryItem();
            if (item != null) PlayMedia(item, true);
        }

        private void PlayMedia(MediaItem item, bool ensureQueued)
        {
            if (item == null) return;
            if (!item.Exists)
            {
                MessageBox.Show(this, "This file is no longer available:\n\n" + item.Path, "Missing Media", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_playback == null)
            {
                MessageBox.Show(this, "Playback is unavailable because the LibVLC engine did not initialize. Restore NuGet packages and rebuild the x64 solution.", "Playback Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ensureQueued)
            {
                var existing = _queue.FindIndex(x => string.Equals(x.Id, item.Id, StringComparison.OrdinalIgnoreCase));
                if (existing < 0)
                {
                    _queue.Add(item);
                    _queueIndex = _queue.Count - 1;
                }
                else _queueIndex = existing;
                UpdateQueueUi();
            }

            try
            {
                _playback.Play(item.Path);
                _playback.SetRate(_playbackRate);
                _library.MarkPlayed(item);
                _currentMediaItem = item;
                lblNowPlayingTitle.Text = item.Title;
                lblNowPlayingMeta.Text = item.MediaType + "  •  " + Path.GetFileName(item.Path);
                UpdateNowPlayingDetails(item);
                var isVideo = MediaFileHelper.IsVideo(item.Path);
                audioOverlay.Visible = !isVideo;
                if (!isVideo)
                {
                    var artist = MediaFileHelper.GetArtistFromTitle(item.Title);
                    var trackName = MediaFileHelper.GetTrackNameFromTitle(item.Title);
                    if (_audioVisualizer != null)
                    {
                        _audioVisualizer.SetTrack(trackName, artist);
                        _audioVisualizer.SetArtwork(item.ArtworkPath);
                        _audioVisualizer.SetPlaying(true);
                    }
                    audioOverlay.BringToFront();
                }
                else
                {
                    videoView.BringToFront();
                }
                ResetProgress();
                RefreshLibrary();
                HighlightCurrentQueueItem();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "not.asser could not open this file.\n\n" + ex.Message, "Playback Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TogglePlayPause()
        {
            if (_playback == null) return;
            if (_queueIndex < 0 && _queue.Count > 0)
            {
                _queueIndex = 0;
                PlayMedia(_queue[0], false);
                return;
            }
            if (_queueIndex < 0)
            {
                PlaySelectedLibraryItem();
                return;
            }
            _playback.TogglePause();
        }

        private void AddSelectedLibraryItemToQueue()
        {
            var item = SelectedLibraryItem();
            if (item == null) return;
            if (_queue.Any(x => string.Equals(x.Id, item.Id, StringComparison.OrdinalIgnoreCase)))
            {
                lblStatus.Text = "Already in queue";
                return;
            }
            _queue.Add(item);
            UpdateQueueUi();
            lblStatus.Text = "Added to queue";
        }

        private void UpdateQueueUi()
        {
            // Queue remains an internal playback structure. The right panel is intentionally
            // dedicated to the media that is currently playing.
            listQueue.BeginUpdate();
            listQueue.Items.Clear();
            for (var i = 0; i < _queue.Count; i++)
            {
                var item = _queue[i];
                var row = new ListViewItem(item.Title);
                row.SubItems.Add(item.MediaType);
                row.Tag = i;
                listQueue.Items.Add(row);
            }
            listQueue.EndUpdate();
        }

        private void UpdateNowPlayingDetails(MediaItem item)
        {
            _currentMediaItem = item;

            if (item == null)
            {
                lblQueueCount.Text = "LOCAL";
                lblQueueBannerTitle.Text = "Nothing playing";
                lblQueueBannerMeta.Text = "Choose a song or video from your library.";
                lblQueueBannerHint.Text = "Double-click artwork to personalize it";
                lblDetailsType.Text = "Artist  •  —";
                lblDetailsPlays.Text = "Format  •  —";
                lblDetailsAdded.Text = "Added  •  —";
                lblDetailsLocation.Text = "Source  •  —" + Environment.NewLine + "File  •  —" + Environment.NewLine + "Plays  •  —";
                lblDetailsFavorite.Text = "♡ Not in favorites";
                lblDetailsFavorite.ForeColor = UiTheme.Text;
                btnQueueUp.Text = "♡";
                btnQueueUp.BackColor = UiTheme.SurfaceAlt;
                LoadCurrentArtwork(null);
                if (_audioVisualizer != null) _audioVisualizer.SetArtwork(null);
                return;
            }

            var artist = MediaFileHelper.GetArtistFromTitle(item.Title);
            var trackName = MediaFileHelper.GetTrackNameFromTitle(item.Title);
            var folderName = string.Empty;
            try { folderName = new DirectoryInfo(item.Folder).Name; } catch { folderName = item.Folder; }
            var fileFormat = MediaFileHelper.GetFormatSummary(item.Path, item.Title);

            lblQueueCount.Text = (item.MediaType ?? "Media").ToUpperInvariant();
            lblQueueBannerTitle.Text = string.IsNullOrWhiteSpace(trackName) ? item.Title : trackName;
            lblQueueBannerMeta.Text = string.IsNullOrWhiteSpace(artist) ? "Local media" : artist;
            lblQueueBannerHint.Text = string.IsNullOrWhiteSpace(fileFormat) ? Path.GetFileName(item.Path) : fileFormat;
            lblDetailsType.Text = "Artist  •  " + (string.IsNullOrWhiteSpace(artist) ? "Local audio" : artist);
            lblDetailsPlays.Text = "Format  •  " + fileFormat;
            lblDetailsAdded.Text = "Added  •  " + (item.AddedUtc == default(DateTime) ? "—" : item.AddedUtc.ToLocalTime().ToString("MMM d, yyyy"));
            lblDetailsLocation.Text = "Source  •  " + (string.IsNullOrWhiteSpace(folderName) ? item.Folder : folderName) +
                                      Environment.NewLine + "File  •  " + Path.GetFileName(item.Path) +
                                      Environment.NewLine + "Plays  •  " + item.PlayCount;
            lblDetailsFavorite.Text = item.IsFavorite ? "♥  Saved to favorites" : "♡  Not in favorites";
            lblDetailsFavorite.ForeColor = item.IsFavorite ? UiTheme.AccentSoft : UiTheme.Text;
            btnQueueUp.Text = item.IsFavorite ? "♥" : "♡";
            btnQueueUp.BackColor = item.IsFavorite ? UiTheme.Accent : UiTheme.SurfaceAlt;
            LoadCurrentArtwork(item);
            if (_audioVisualizer != null) _audioVisualizer.SetArtwork(item.ArtworkPath);
            UpdatePlaybackInfo();
        }

        private void ChooseCurrentArtwork()
        {
            if (_currentMediaItem == null)
            {
                MessageBox.Show(this, "Play a song or video first, then choose artwork for it.", "Artwork", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Choose artwork for " + _currentMediaItem.Title;
                dialog.Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp;*.gif|All files|*.*";
                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                var savedPath = _library.SetArtwork(_currentMediaItem, dialog.FileName);
                if (string.IsNullOrWhiteSpace(savedPath))
                {
                    MessageBox.Show(this, "The artwork image could not be saved.", "Artwork", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LoadCurrentArtwork(_currentMediaItem);
                lblStatus.Text = "Artwork updated";
            }
        }

        private void ClearCurrentArtwork()
        {
            if (_currentMediaItem == null) return;
            _library.ClearArtwork(_currentMediaItem);
            LoadCurrentArtwork(_currentMediaItem);
            lblStatus.Text = "Artwork removed";
        }

        private void LoadCurrentArtwork(MediaItem item)
        {
            if (picQueueBanner.Image != null)
            {
                var oldImage = picQueueBanner.Image;
                picQueueBanner.Image = null;
                oldImage.Dispose();
            }

            if (item == null || string.IsNullOrWhiteSpace(item.ArtworkPath) || !File.Exists(item.ArtworkPath))
            {
                if (_audioVisualizer != null) _audioVisualizer.SetArtwork(null);
                picQueueBanner.Invalidate();
                return;
            }

            try
            {
                using (var source = Image.FromFile(item.ArtworkPath))
                    picQueueBanner.Image = new Bitmap(source);
            }
            catch
            {
                picQueueBanner.Image = null;
            }

            if (_audioVisualizer != null) _audioVisualizer.SetArtwork(item.ArtworkPath);
            picQueueBanner.Invalidate();
        }

        private void DrawArtworkPlaceholder(object sender, PaintEventArgs e)
        {
            if (picQueueBanner.Image != null) return;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var bounds = picQueueBanner.ClientRectangle;
            using (var bg = new SolidBrush(UiTheme.PlayerSurface)) e.Graphics.FillRectangle(bg, bounds);
            using (var accent = new SolidBrush(Color.FromArgb(44, UiTheme.Accent)))
            {
                var size = Math.Min(bounds.Width, bounds.Height) / 2;
                var box = new Rectangle((bounds.Width - size) / 2, (bounds.Height - size) / 2 - 16, size, size);
                e.Graphics.FillRectangle(accent, box);
            }
            using (var iconFont = new Font("Segoe UI Symbol", 28F, FontStyle.Regular))
            using (var textFont = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold))
            using (var iconBrush = new SolidBrush(UiTheme.AccentSoft))
            using (var textBrush = new SolidBrush(UiTheme.SubtleText))
            {
                var icon = "♫";
                var iconSize = e.Graphics.MeasureString(icon, iconFont);
                e.Graphics.DrawString(icon, iconFont, iconBrush, (bounds.Width - iconSize.Width) / 2, (bounds.Height - iconSize.Height) / 2 - 25);
                var textValue = "DOUBLE-CLICK TO ADD ARTWORK";
                var textSize = e.Graphics.MeasureString(textValue, textFont);
                e.Graphics.DrawString(textValue, textFont, textBrush, (bounds.Width - textSize.Width) / 2, bounds.Height - 36);
            }
        }

        private void ToggleCurrentFavorite()
        {
            if (_currentMediaItem == null) return;
            _library.ToggleFavorite(_currentMediaItem);
            UpdateNowPlayingDetails(_currentMediaItem);
            RefreshLibrary();
            lblStatus.Text = _currentMediaItem.IsFavorite ? "Added to favorites" : "Removed from favorites";
        }

        private void OpenCurrentMediaFolder()
        {
            if (_currentMediaItem == null || string.IsNullOrWhiteSpace(_currentMediaItem.Path)) return;
            try
            {
                if (!File.Exists(_currentMediaItem.Path)) return;
                System.Diagnostics.Process.Start("explorer.exe", "/select,\"" + _currentMediaItem.Path + "\"");
            }
            catch { lblStatus.Text = "Could not open file location"; }
        }

        private void CopyCurrentMediaPath()
        {
            if (_currentMediaItem == null || string.IsNullOrWhiteSpace(_currentMediaItem.Path)) return;
            try { Clipboard.SetText(_currentMediaItem.Path); lblStatus.Text = "File path copied"; }
            catch { lblStatus.Text = "Could not copy file path"; }
        }

        private void HighlightCurrentQueueItem()
        {
            UpdateQueueUi();
        }

        private void PlaySelectedQueueItem()
        {
            if (listQueue.SelectedItems.Count == 0) return;
            var index = (int)listQueue.SelectedItems[0].Tag;
            if (index < 0 || index >= _queue.Count) return;
            _queueIndex = index;
            PlayMedia(_queue[index], false);
        }

        private void MoveQueueItem(int direction)
        {
            if (listQueue.SelectedItems.Count == 0) return;
            var index = (int)listQueue.SelectedItems[0].Tag;
            var target = index + direction;
            var minimumMovableIndex = _queueIndex >= 0 ? _queueIndex + 1 : 0;
            if (target < minimumMovableIndex || target >= _queue.Count) return;

            var item = _queue[index];
            _queue.RemoveAt(index);
            _queue.Insert(target, item);

            if (_queueIndex == index) _queueIndex = target;
            else if (_queueIndex == target) _queueIndex = index;

            UpdateQueueUi();
            foreach (ListViewItem row in listQueue.Items)
            {
                if ((int)row.Tag != target) continue;
                row.Selected = true;
                row.Focused = true;
                row.EnsureVisible();
                break;
            }
        }

        private void RemoveSelectedQueueItem()
        {
            if (listQueue.SelectedItems.Count == 0) return;
            var index = (int)listQueue.SelectedItems[0].Tag;
            if (index < 0 || index >= _queue.Count) return;

            var removingCurrent = index == _queueIndex;
            _queue.RemoveAt(index);
            if (_queue.Count == 0) _queueIndex = -1;
            else if (index < _queueIndex) _queueIndex--;
            else if (_queueIndex >= _queue.Count) _queueIndex = _queue.Count - 1;

            UpdateQueueUi();
            if (removingCurrent) lblStatus.Text = "Current item removed from queue";
        }

        private void ClearQueue()
        {
            _queue.Clear();
            _queueIndex = -1;
            UpdateQueueUi();
            lblStatus.Text = "Queue cleared";
        }

        private void NextTrack(bool userInitiated)
        {
            if (_queue.Count == 0) return;

            if (_shuffle && _queue.Count > 1)
            {
                var next = _queueIndex;
                while (next == _queueIndex) next = _random.Next(_queue.Count);
                _queueIndex = next;
            }
            else
            {
                var next = _queueIndex + 1;
                if (next >= _queue.Count)
                {
                    if (_repeatMode == RepeatMode.All || userInitiated) next = 0;
                    else return;
                }
                _queueIndex = next;
            }

            PlayMedia(_queue[_queueIndex], false);
        }

        private void PreviousTrack()
        {
            if (_queue.Count == 0) return;
            if (_playback != null && _playback.Time > 5000)
            {
                _playback.SeekToRatio(0);
                return;
            }
            _queueIndex--;
            if (_queueIndex < 0) _queueIndex = _queue.Count - 1;
            PlayMedia(_queue[_queueIndex], false);
        }

        private void HandleTrackEnded()
        {
            if (_queueIndex < 0 || _queue.Count == 0) return;
            if (_repeatMode == RepeatMode.One)
            {
                PlayMedia(_queue[_queueIndex], false);
                return;
            }
            NextTrack(false);
        }

        private void ToggleShuffle()
        {
            _shuffle = !_shuffle;
            btnShuffle.BackColor = _shuffle ? UiTheme.Accent : UiTheme.SurfaceAlt;
            lblStatus.Text = _shuffle ? "Shuffle on" : "Shuffle off";
            UpdatePlaybackInfo();
        }

        private void CycleRepeatMode()
        {
            if (_repeatMode == RepeatMode.Off) _repeatMode = RepeatMode.All;
            else if (_repeatMode == RepeatMode.All) _repeatMode = RepeatMode.One;
            else _repeatMode = RepeatMode.Off;

            btnRepeat.Text = _repeatMode == RepeatMode.Off ? "Repeat: Off" : (_repeatMode == RepeatMode.All ? "Repeat: All" : "Repeat: One");
            btnRepeat.BackColor = _repeatMode == RepeatMode.Off ? UiTheme.SurfaceAlt : UiTheme.Accent;
            UpdatePlaybackInfo();
        }

        private void SetVolumeFromUi()
        {
            lblVolume.Text = trackVolume.Value + "%";
            btnMute.Invalidate();
            if (_syncingVolumeUi) return;

            if (trackVolume.Value > 0) _volumeBeforeMute = trackVolume.Value;

            if (_playback != null)
            {
                if (_playback.IsMuted && trackVolume.Value > 0)
                {
                    _playback.SetMute(false);
                    UpdateMuteButtonVisual(false);
                }
                _playback.SetVolume(trackVolume.Value);
            }
        }

        private void ToggleMute()
        {
            if (_playback == null) return;

            var willMute = !_playback.IsMuted;
            if (willMute)
            {
                if (trackVolume.Value > 0) _volumeBeforeMute = trackVolume.Value;
                _playback.SetMute(true);
                _syncingVolumeUi = true;
                trackVolume.Value = 0;
                _syncingVolumeUi = false;
                lblVolume.Text = "0%";
                lblStatus.Text = "Muted";
            }
            else
            {
                var restoreVolume = Math.Max(1, Math.Min(100, _volumeBeforeMute));
                _playback.SetVolume(restoreVolume);
                _playback.SetMute(false);
                _syncingVolumeUi = true;
                trackVolume.Value = restoreVolume;
                _syncingVolumeUi = false;
                lblVolume.Text = restoreVolume + "%";
                lblStatus.Text = "Sound on";
            }

            UpdateMuteButtonVisual(willMute);
        }

        private void UpdateMuteButtonVisual(bool muted)
        {
            btnMute.Tag = muted;
            btnMute.Text = string.Empty;
            btnMute.AccessibleName = muted ? "Unmute" : "Mute";
            btnMute.Invalidate();
        }

        private void DrawMuteButtonIcon(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var muted = (btnMute.Tag is bool && (bool)btnMute.Tag) || trackVolume.Value == 0;
            var color = btnMute.Enabled ? UiTheme.Text : UiTheme.SubtleText;

            using (var pen = new Pen(color, 1.8F))
            using (var brush = new SolidBrush(color))
            {
                var cx = btnMute.ClientSize.Width / 2;
                var cy = btnMute.ClientSize.Height / 2;
                var speaker = new Point[]
                {
                    new Point(cx - 10, cy - 4), new Point(cx - 5, cy - 4),
                    new Point(cx + 1, cy - 10), new Point(cx + 1, cy + 10),
                    new Point(cx - 5, cy + 4), new Point(cx - 10, cy + 4)
                };
                e.Graphics.FillPolygon(brush, speaker);

                if (muted)
                {
                    e.Graphics.DrawLine(pen, cx + 7, cy - 6, cx + 17, cy + 6);
                    e.Graphics.DrawLine(pen, cx + 17, cy - 6, cx + 7, cy + 6);
                }
                else
                {
                    e.Graphics.DrawArc(pen, cx - 1, cy - 7, 15, 14, -55, 110);
                    e.Graphics.DrawArc(pen, cx - 1, cy - 11, 23, 22, -50, 100);
                }
            }
        }

        private void UpdateProgress(long time, long length)
        {
            lblElapsed.Text = FormatTime(time);
            lblDuration.Text = FormatTime(length);
            UpdatePlaybackInfo();
            if (_draggingProgress || length <= 0) return;
            var ratio = Math.Max(0, Math.Min(1, time / (double)length));
            trackProgress.Value = Math.Max(trackProgress.Minimum, Math.Min(trackProgress.Maximum, (int)(ratio * trackProgress.Maximum)));
            if (_audioVisualizer != null && audioOverlay.Visible) _audioVisualizer.SetProgress(ratio);
        }

        private void SeekFromProgressBar()
        {
            if (_playback == null || trackProgress.Maximum <= 0) return;
            _playback.SeekToRatio(trackProgress.Value / (double)trackProgress.Maximum);
        }

        private void ResetProgress()
        {
            trackProgress.Value = 0;
            lblElapsed.Text = "00:00";
            lblDuration.Text = "00:00";
            if (_audioVisualizer != null) _audioVisualizer.SetProgress(0);
            UpdatePlaybackInfo();
        }

        private static string FormatTime(long milliseconds)
        {
            if (milliseconds < 0) milliseconds = 0;
            var time = TimeSpan.FromMilliseconds(milliseconds);
            return time.TotalHours >= 1 ? time.ToString(@"hh\:mm\:ss") : time.ToString(@"mm\:ss");
        }

        private void PlaySelectedNext()
        {
            var item = SelectedLibraryItem();
            if (item == null) return;

            var existing = _queue.FindIndex(x => string.Equals(x.Id, item.Id, StringComparison.OrdinalIgnoreCase));
            if (existing >= 0)
            {
                _queue.RemoveAt(existing);
                if (existing < _queueIndex) _queueIndex--;
            }

            var insertAt = _queueIndex < 0 ? 0 : Math.Min(_queueIndex + 1, _queue.Count);
            _queue.Insert(insertAt, item);
            UpdateQueueUi();
            lblStatus.Text = "Queued to play next";
        }

        private void OpenContainingFolder()
        {
            var item = SelectedLibraryItem();
            if (item == null || string.IsNullOrWhiteSpace(item.Path)) return;

            try
            {
                if (!File.Exists(item.Path))
                {
                    MessageBox.Show(this, "The media file is missing, so its location cannot be opened.", "Missing Media", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                System.Diagnostics.Process.Start("explorer.exe", "/select,\"" + item.Path + "\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Windows could not open the file location.\n\n" + ex.Message, "Open Location", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CopySelectedPath()
        {
            var item = SelectedLibraryItem();
            if (item == null || string.IsNullOrWhiteSpace(item.Path)) return;

            try
            {
                Clipboard.SetText(item.Path);
                lblStatus.Text = "File path copied";
            }
            catch
            {
                lblStatus.Text = "Could not copy file path";
            }
        }

        private void CleanMissingItems()
        {
            var missing = _library.CountMissingItems();
            if (missing == 0)
            {
                lblStatus.Text = "No missing files found";
                return;
            }

            if (MessageBox.Show(this,
                "Remove " + missing + " missing media entr" + (missing == 1 ? "y" : "ies") + " from the library?\n\nYour actual files are never deleted.",
                "Clean Missing Media",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes) return;

            var removed = _library.RemoveMissingItems();
            _queue.RemoveAll(item => !item.Exists);
            if (_queueIndex >= _queue.Count) _queueIndex = _queue.Count - 1;
            RefreshLibrary();
            UpdateQueueUi();
            lblStatus.Text = "Removed " + removed + " missing entr" + (removed == 1 ? "y" : "ies");
        }

        private void ChangePlaybackRate()
        {
            if (_playback == null || cmbSpeed.SelectedItem == null) return;
            var raw = cmbSpeed.SelectedItem.ToString().Replace("x", string.Empty);
            float rate;
            if (!float.TryParse(raw, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out rate))
                rate = 1F;

            _playbackRate = rate;
            _playback.SetRate(rate);
            lblStatus.Text = "Playback speed " + rate.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + "x";
            UpdatePlaybackInfo();
        }

        private void ToggleSelectedFavorite()
        {
            var item = SelectedLibraryItem();
            if (item == null) return;
            _library.ToggleFavorite(item);
            if (_currentMediaItem != null && string.Equals(_currentMediaItem.Id, item.Id, StringComparison.OrdinalIgnoreCase))
                UpdateNowPlayingDetails(_currentMediaItem);
            RefreshLibrary();
            lblStatus.Text = item.IsFavorite ? "Added to favorites" : "Removed from favorites";
        }

        private void RemoveSelectedFromLibrary()
        {
            var item = SelectedLibraryItem();
            if (item == null) return;
            if (MessageBox.Show(this, "Remove \"" + item.Title + "\" from not.asser's library?\n\nThe actual media file will NOT be deleted.", "Remove from Library", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            _library.RemoveFromLibrary(item);
            if (_currentMediaItem != null && string.Equals(_currentMediaItem.Id, item.Id, StringComparison.OrdinalIgnoreCase))
            {
                _currentMediaItem = null;
                UpdateNowPlayingDetails(null);
            }
            _queue.RemoveAll(x => string.Equals(x.Id, item.Id, StringComparison.OrdinalIgnoreCase));
            if (_queueIndex >= _queue.Count) _queueIndex = _queue.Count - 1;
            RefreshLibrary();
            UpdateQueueUi();
        }

        private void CreatePlaylist()
        {
            var name = TextPromptForm.Show(this, "New Playlist", "Playlist name", string.Empty);
            if (string.IsNullOrWhiteSpace(name)) return;
            var playlist = _library.CreatePlaylist(name);
            LoadPlaylists(playlist == null ? null : playlist.Id);
        }

        private void AddSelectedToCurrentPlaylist()
        {
            var playlist = SelectedPlaylist();
            var item = SelectedLibraryItem();
            if (playlist == null)
            {
                MessageBox.Show(this, "Select a playlist in the sidebar first.", "Playlist", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (item == null)
            {
                MessageBox.Show(this, "Select a media item from the library first.", "Playlist", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            lblStatus.Text = _library.AddToPlaylist(playlist, item) ? "Added to " + playlist.Name : "Already in " + playlist.Name;
            if (_currentView == LibraryView.Playlist) RefreshLibrary();
        }

        private void RemoveSelectedFromCurrentPlaylist()
        {
            var playlist = SelectedPlaylist();
            var item = SelectedLibraryItem();
            if (playlist == null || item == null) return;
            if (_library.RemoveFromPlaylist(playlist, item))
            {
                lblStatus.Text = "Removed from " + playlist.Name;
                RefreshLibrary();
            }
        }

        private void DeleteSelectedPlaylist()
        {
            var playlist = SelectedPlaylist();
            if (playlist == null) return;
            if (MessageBox.Show(this, "Delete playlist \"" + playlist.Name + "\"?\n\nMedia files remain in your library.", "Delete Playlist", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            _library.DeletePlaylist(playlist);
            _currentView = LibraryView.All;
            _currentPlaylistId = null;
            LoadPlaylists();
            UpdateNavigationState();
            RefreshLibrary();
        }

        private void ToggleFullscreen()
        {
            SuspendLayout();
            rootLayout.SuspendLayout();
            contentLayout.SuspendLayout();

            try
            {
                if (!_fullScreen)
                {
                    _previousBorderStyle = FormBorderStyle;
                    _previousWindowState = WindowState;

                    // A hidden control does not collapse a TableLayoutPanel row/column by itself.
                    // Collapse those layout slots too so the video can actually use the full screen.
                    rootLayout.RowStyles[0].Height = 0F;
                    rootLayout.RowStyles[2].Height = FullscreenTransportHeight;
                    contentLayout.ColumnStyles[0].Width = 0F;
                    contentLayout.ColumnStyles[2].Width = 0F;

                    topBar.Visible = false;
                    sidebar.Visible = false;
                    queuePanel.Visible = false;
                    mainSplit.Panel2Collapsed = true;
                    stageToolbar.Visible = false;
                    videoHost.Padding = Padding.Empty;
                    videoSurface.Padding = Padding.Empty;

                    _fullScreen = true;
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                    btnFullscreen.AccessibleName = "Exit full screen";
                    btnFullscreen.Invalidate();
                }
                else
                {
                    rootLayout.RowStyles[0].Height = NormalTopBarHeight;
                    rootLayout.RowStyles[2].Height = NormalTransportHeight;
                    contentLayout.ColumnStyles[0].Width = SidebarWidth;
                    contentLayout.ColumnStyles[2].Width = QueueWidth;

                    topBar.Visible = true;
                    sidebar.Visible = true;
                    queuePanel.Visible = true;
                    mainSplit.Panel2Collapsed = false;
                    stageToolbar.Visible = true;
                    videoHost.Padding = new Padding(1);
                    videoSurface.Padding = new Padding(10);

                    _fullScreen = false;
                    FormBorderStyle = _previousBorderStyle;
                    WindowState = _previousWindowState;
                    btnFullscreen.AccessibleName = "Enter full screen";
                    btnFullscreen.Invalidate();
                }
            }
            finally
            {
                contentLayout.ResumeLayout(true);
                rootLayout.ResumeLayout(true);
                ResumeLayout(true);
                ReflowLayout();
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            NativeTheme.ApplyDarkTitleBar(this);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F11) { ToggleFullscreen(); return true; }
            if (keyData == Keys.Escape && _fullScreen) { ToggleFullscreen(); return true; }
            if (keyData == (Keys.Control | Keys.O)) { ImportFiles(); return true; }
            if (keyData == (Keys.Control | Keys.Shift | Keys.O)) { ImportFolder(); return true; }
            if (keyData == (Keys.Control | Keys.F)) { txtSearch.Focus(); txtSearch.SelectAll(); return true; }

            var searchFocused = txtSearch.Focused;
            if (!searchFocused && keyData == Keys.Space) { TogglePlayPause(); return true; }
            if (!searchFocused && keyData == Keys.M) { ToggleMute(); return true; }
            if (!searchFocused && keyData == Keys.Right && _playback != null) { _playback.SeekBySeconds(5); return true; }
            if (!searchFocused && keyData == Keys.Left && _playback != null) { _playback.SeekBySeconds(-5); return true; }
            if (!searchFocused && keyData == Keys.Up) { trackVolume.Value = Math.Min(100, trackVolume.Value + 5); return true; }
            if (!searchFocused && keyData == Keys.Down) { trackVolume.Value = Math.Max(0, trackVolume.Value - 5); return true; }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null || !e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (paths == null) return;

            var files = new List<string>();
            var added = 0;
            foreach (var path in paths)
            {
                if (File.Exists(path)) files.Add(path);
                else if (Directory.Exists(path)) added += _library.AddFolder(path);
            }
            added += _library.AddFiles(files);
            RefreshLibrary();
            lblStatus.Text = added == 0 ? "No new supported media found" : "Added " + added + " media file(s)";
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeLibraryThumbnails();
            var muted = _playback != null ? _playback.IsMuted : _library.IsMuted;
            _library.SavePlaybackSettings(muted ? _volumeBeforeMute : trackVolume.Value, muted);
            if (_playback != null) _playback.Dispose();
        }

        private void Ui(Action action)
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                try { BeginInvoke(action); } catch { }
            }
            else action();
        }

        private static IEnumerable<T> ControlsRecursive<T>(Control parent) where T : Control
        {
            foreach (Control child in parent.Controls)
            {
                var typed = child as T;
                if (typed != null) yield return typed;
                foreach (var nested in ControlsRecursive<T>(child)) yield return nested;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            NativeTheme.ApplyDarkTitleBar(this);
            NativeTheme.ApplyDarkScrollableControls(this);
        }

        private void lblAppTitle_Click(object sender, EventArgs e)
        {

        }

        private void trackVolume_Scroll(object sender, EventArgs e)
        {

        }

        private void lblVolume_Click(object sender, EventArgs e)
        {

        }

        private void btnMute_Click(object sender, EventArgs e)
        {

        }
    }
}
