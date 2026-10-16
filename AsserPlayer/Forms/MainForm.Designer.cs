using System.Drawing;
using System.Windows.Forms;
using LibVLCSharp.WinForms;

namespace AsserPlayer.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private TableLayoutPanel rootLayout;
        private Panel topBar;
        private Label lblBrandMark;
        private Label lblAppTitle;
        private Label lblCurrentView;
        private TextBox txtSearch;
        private Button btnAddFiles;
        private Button btnAddFolder;

        private TableLayoutPanel contentLayout;
        private Panel sidebar;
        private Label lblLibrarySection;
        private Button btnAll;
        private Button btnMusic;
        private Button btnVideos;
        private Button btnFavorites;
        private Button btnRecent;
        private Label lblPlaylistSection;
        private Button btnNewPlaylist;
        private ListBox lstPlaylists;
        private Button btnPlaylistAddSelected;
        private Button btnPlaylistRemoveSelected;
        private Button btnDeletePlaylist;

        private SplitContainer mainSplit;
        private Panel videoHost;
        private Panel stageToolbar;
        private Label lblStageTitle;
        private Label lblStageHint;
        private Panel videoSurface;
        private VideoView videoView;
        private Panel audioOverlay;
        private Label lblAudioIcon;
        private Label lblAudioTitle;
        private Label lblAudioHint;

        private Panel libraryPanel;
        private Panel libraryHeader;
        private Label lblLibraryTitle;
        private Label lblLibraryCount;
        private ComboBox cmbSort;
        private Button btnCleanMissing;
        private DataGridView gridLibrary;

        private Panel queuePanel;
        private TableLayoutPanel queueLayout;
        private Panel queueHeader;
        private Label lblQueueTitle;
        private Label lblQueueCount;
        private Panel queueHero;
        private PictureBox picQueueBanner;
        private Label lblQueueBannerTitle;
        private Label lblQueueBannerMeta;
        private Label lblQueueBannerHint;
        private ListView listQueue;
        private ColumnHeader queueTitleColumn;
        private ColumnHeader queueTypeColumn;
        private Panel trackDetailsPanel;
        private Label lblDetailsSection;
        private Label lblDetailsType;
        private Label lblDetailsPlays;
        private Label lblDetailsAdded;
        private Label lblDetailsLocation;
        private Label lblDetailsFavorite;
        private Panel queueActions;
        private Button btnQueueUp;
        private Button btnQueueDown;
        private Button btnQueueRemove;
        private Button btnQueueClear;

        private Panel transportPanel;
        private TableLayoutPanel transportLayout;
        private Panel nowPlayingPanel;
        private Label lblNowPlayingCaption;
        private Label lblNowPlayingTitle;
        private Label lblNowPlayingMeta;
        private Label lblStatus;
        private Panel centerTransportPanel;
        private TrackBar trackProgress;
        private Label lblElapsed;
        private Label lblDuration;
        private Button btnShuffle;
        private Button btnPrevious;
        private Button btnPlayPause;
        private Button btnStop;
        private Button btnNext;
        private Button btnRepeat;
        private Panel volumePanel;
        private Button btnMute;
        private TrackBar trackVolume;
        private Label lblVolume;
        private ComboBox cmbSpeed;
        private Button btnFullscreen;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.rootLayout = new System.Windows.Forms.TableLayoutPanel();
            this.topBar = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblBrandMark = new System.Windows.Forms.Label();
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.lblCurrentView = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnAddFiles = new System.Windows.Forms.Button();
            this.btnAddFolder = new System.Windows.Forms.Button();
            this.contentLayout = new System.Windows.Forms.TableLayoutPanel();
            this.sidebar = new System.Windows.Forms.Panel();
            this.lblLibrarySection = new System.Windows.Forms.Label();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnMusic = new System.Windows.Forms.Button();
            this.btnVideos = new System.Windows.Forms.Button();
            this.btnFavorites = new System.Windows.Forms.Button();
            this.btnRecent = new System.Windows.Forms.Button();
            this.lblPlaylistSection = new System.Windows.Forms.Label();
            this.btnNewPlaylist = new System.Windows.Forms.Button();
            this.lstPlaylists = new System.Windows.Forms.ListBox();
            this.btnPlaylistAddSelected = new System.Windows.Forms.Button();
            this.btnPlaylistRemoveSelected = new System.Windows.Forms.Button();
            this.btnDeletePlaylist = new System.Windows.Forms.Button();
            this.mainSplit = new System.Windows.Forms.SplitContainer();
            this.videoHost = new System.Windows.Forms.Panel();
            this.videoSurface = new System.Windows.Forms.Panel();
            this.audioOverlay = new System.Windows.Forms.Panel();
            this.lblAudioHint = new System.Windows.Forms.Label();
            this.lblAudioTitle = new System.Windows.Forms.Label();
            this.lblAudioIcon = new System.Windows.Forms.Label();
            this.videoView = new LibVLCSharp.WinForms.VideoView();
            this.stageToolbar = new System.Windows.Forms.Panel();
            this.lblStageTitle = new System.Windows.Forms.Label();
            this.lblStageHint = new System.Windows.Forms.Label();
            this.libraryPanel = new System.Windows.Forms.Panel();
            this.gridLibrary = new System.Windows.Forms.DataGridView();
            this.libraryHeader = new System.Windows.Forms.Panel();
            this.lblLibraryTitle = new System.Windows.Forms.Label();
            this.lblLibraryCount = new System.Windows.Forms.Label();
            this.cmbSort = new System.Windows.Forms.ComboBox();
            this.btnCleanMissing = new System.Windows.Forms.Button();
            this.queuePanel = new System.Windows.Forms.Panel();
            this.queueLayout = new System.Windows.Forms.TableLayoutPanel();
            this.queueHeader = new System.Windows.Forms.Panel();
            this.lblQueueTitle = new System.Windows.Forms.Label();
            this.lblQueueCount = new System.Windows.Forms.Label();
            this.queueHero = new System.Windows.Forms.Panel();
            this.lblQueueBannerHint = new System.Windows.Forms.Label();
            this.lblQueueBannerMeta = new System.Windows.Forms.Label();
            this.lblQueueBannerTitle = new System.Windows.Forms.Label();
            this.picQueueBanner = new System.Windows.Forms.PictureBox();
            this.trackDetailsPanel = new System.Windows.Forms.Panel();
            this.lblDetailsFavorite = new System.Windows.Forms.Label();
            this.lblDetailsLocation = new System.Windows.Forms.Label();
            this.lblDetailsAdded = new System.Windows.Forms.Label();
            this.lblDetailsPlays = new System.Windows.Forms.Label();
            this.lblDetailsType = new System.Windows.Forms.Label();
            this.lblDetailsSection = new System.Windows.Forms.Label();
            this.queueActions = new System.Windows.Forms.Panel();
            this.btnQueueUp = new System.Windows.Forms.Button();
            this.btnQueueDown = new System.Windows.Forms.Button();
            this.btnQueueRemove = new System.Windows.Forms.Button();
            this.btnQueueClear = new System.Windows.Forms.Button();
            this.transportPanel = new System.Windows.Forms.Panel();
            this.transportLayout = new System.Windows.Forms.TableLayoutPanel();
            this.nowPlayingPanel = new System.Windows.Forms.Panel();
            this.lblNowPlayingCaption = new System.Windows.Forms.Label();
            this.lblNowPlayingTitle = new System.Windows.Forms.Label();
            this.lblNowPlayingMeta = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.centerTransportPanel = new System.Windows.Forms.Panel();
            this.btnShuffle = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnPlayPause = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnRepeat = new System.Windows.Forms.Button();
            this.trackProgress = new System.Windows.Forms.TrackBar();
            this.lblElapsed = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.volumePanel = new System.Windows.Forms.Panel();
            this.lblVolume = new System.Windows.Forms.Label();
            this.cmbSpeed = new System.Windows.Forms.ComboBox();
            this.btnMute = new System.Windows.Forms.Button();
            this.trackVolume = new System.Windows.Forms.TrackBar();
            this.btnFullscreen = new System.Windows.Forms.Button();
            this.listQueue = new System.Windows.Forms.ListView();
            this.queueTitleColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.queueTypeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rootLayout.SuspendLayout();
            this.topBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contentLayout.SuspendLayout();
            this.sidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplit)).BeginInit();
            this.mainSplit.Panel1.SuspendLayout();
            this.mainSplit.Panel2.SuspendLayout();
            this.mainSplit.SuspendLayout();
            this.videoHost.SuspendLayout();
            this.videoSurface.SuspendLayout();
            this.audioOverlay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.videoView)).BeginInit();
            this.stageToolbar.SuspendLayout();
            this.libraryPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLibrary)).BeginInit();
            this.libraryHeader.SuspendLayout();
            this.queuePanel.SuspendLayout();
            this.queueLayout.SuspendLayout();
            this.queueHeader.SuspendLayout();
            this.queueHero.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picQueueBanner)).BeginInit();
            this.trackDetailsPanel.SuspendLayout();
            this.queueActions.SuspendLayout();
            this.transportPanel.SuspendLayout();
            this.transportLayout.SuspendLayout();
            this.nowPlayingPanel.SuspendLayout();
            this.centerTransportPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackProgress)).BeginInit();
            this.volumePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // rootLayout
            // 
            this.rootLayout.ColumnCount = 1;
            this.rootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayout.Controls.Add(this.topBar, 0, 0);
            this.rootLayout.Controls.Add(this.contentLayout, 0, 1);
            this.rootLayout.Controls.Add(this.transportPanel, 0, 2);
            this.rootLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootLayout.Location = new System.Drawing.Point(0, 0);
            this.rootLayout.Margin = new System.Windows.Forms.Padding(0);
            this.rootLayout.Name = "rootLayout";
            this.rootLayout.RowCount = 3;
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 145F));
            this.rootLayout.Size = new System.Drawing.Size(1750, 1106);
            this.rootLayout.TabIndex = 0;
            // 
            // topBar
            // 
            this.topBar.Controls.Add(this.pictureBox1);
            this.topBar.Controls.Add(this.lblBrandMark);
            this.topBar.Controls.Add(this.lblAppTitle);
            this.topBar.Controls.Add(this.lblCurrentView);
            this.topBar.Controls.Add(this.txtSearch);
            this.topBar.Controls.Add(this.btnAddFiles);
            this.topBar.Controls.Add(this.btnAddFolder);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topBar.Location = new System.Drawing.Point(4, 4);
            this.topBar.Margin = new System.Windows.Forms.Padding(4);
            this.topBar.Name = "topBar";
            this.topBar.Padding = new System.Windows.Forms.Padding(22, 12, 22, 12);
            this.topBar.Size = new System.Drawing.Size(1742, 82);
            this.topBar.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(15, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(68, 59);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // lblBrandMark
            // 
            this.lblBrandMark.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblBrandMark.Location = new System.Drawing.Point(25, 19);
            this.lblBrandMark.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBrandMark.Name = "lblBrandMark";
            this.lblBrandMark.Size = new System.Drawing.Size(52, 52);
            this.lblBrandMark.TabIndex = 0;
            this.lblBrandMark.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAppTitle
            // 
            this.lblAppTitle.AutoSize = true;
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 15F, System.Drawing.FontStyle.Bold);
            this.lblAppTitle.Location = new System.Drawing.Point(92, 16);
            this.lblAppTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAppTitle.Name = "lblAppTitle";
            this.lblAppTitle.Size = new System.Drawing.Size(165, 35);
            this.lblAppTitle.TabIndex = 1;
            this.lblAppTitle.Text = "Asser Playlist";
            this.lblAppTitle.Click += new System.EventHandler(this.lblAppTitle_Click);
            // 
            // lblCurrentView
            // 
            this.lblCurrentView.AutoSize = true;
            this.lblCurrentView.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCurrentView.Location = new System.Drawing.Point(95, 54);
            this.lblCurrentView.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentView.Name = "lblCurrentView";
            this.lblCurrentView.Size = new System.Drawing.Size(73, 20);
            this.lblCurrentView.TabIndex = 2;
            this.lblCurrentView.Text = "All media";
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location = new System.Drawing.Point(1023, 28);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(374, 30);
            this.txtSearch.TabIndex = 3;
            // 
            // btnAddFiles
            // 
            this.btnAddFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFiles.Location = new System.Drawing.Point(1414, 22);
            this.btnAddFiles.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddFiles.Name = "btnAddFiles";
            this.btnAddFiles.Size = new System.Drawing.Size(145, 42);
            this.btnAddFiles.TabIndex = 4;
            this.btnAddFiles.Text = "+ Add files";
            // 
            // btnAddFolder
            // 
            this.btnAddFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFolder.Location = new System.Drawing.Point(1572, 22);
            this.btnAddFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Size = new System.Drawing.Size(145, 42);
            this.btnAddFolder.TabIndex = 5;
            this.btnAddFolder.Text = "Add folder";
            // 
            // contentLayout
            // 
            this.contentLayout.ColumnCount = 3;
            this.contentLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 270F));
            this.contentLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.contentLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 375F));
            this.contentLayout.Controls.Add(this.sidebar, 0, 0);
            this.contentLayout.Controls.Add(this.mainSplit, 1, 0);
            this.contentLayout.Controls.Add(this.queuePanel, 2, 0);
            this.contentLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentLayout.Location = new System.Drawing.Point(0, 90);
            this.contentLayout.Margin = new System.Windows.Forms.Padding(0);
            this.contentLayout.Name = "contentLayout";
            this.contentLayout.RowCount = 1;
            this.contentLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.contentLayout.Size = new System.Drawing.Size(1750, 871);
            this.contentLayout.TabIndex = 1;
            // 
            // sidebar
            // 
            this.sidebar.Controls.Add(this.lblLibrarySection);
            this.sidebar.Controls.Add(this.btnAll);
            this.sidebar.Controls.Add(this.btnMusic);
            this.sidebar.Controls.Add(this.btnVideos);
            this.sidebar.Controls.Add(this.btnFavorites);
            this.sidebar.Controls.Add(this.btnRecent);
            this.sidebar.Controls.Add(this.lblPlaylistSection);
            this.sidebar.Controls.Add(this.btnNewPlaylist);
            this.sidebar.Controls.Add(this.lstPlaylists);
            this.sidebar.Controls.Add(this.btnPlaylistAddSelected);
            this.sidebar.Controls.Add(this.btnPlaylistRemoveSelected);
            this.sidebar.Controls.Add(this.btnDeletePlaylist);
            this.sidebar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sidebar.Location = new System.Drawing.Point(4, 4);
            this.sidebar.Margin = new System.Windows.Forms.Padding(4);
            this.sidebar.Name = "sidebar";
            this.sidebar.Padding = new System.Windows.Forms.Padding(15, 18, 15, 15);
            this.sidebar.Size = new System.Drawing.Size(262, 863);
            this.sidebar.TabIndex = 0;
            // 
            // lblLibrarySection
            // 
            this.lblLibrarySection.AutoSize = true;
            this.lblLibrarySection.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblLibrarySection.Location = new System.Drawing.Point(22, 22);
            this.lblLibrarySection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLibrarySection.Name = "lblLibrarySection";
            this.lblLibrarySection.Size = new System.Drawing.Size(66, 20);
            this.lblLibrarySection.TabIndex = 0;
            this.lblLibrarySection.Text = "LIBRARY";
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(15, 55);
            this.btnAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(240, 45);
            this.btnAll.TabIndex = 1;
            this.btnAll.Text = "All media";
            // 
            // btnMusic
            // 
            this.btnMusic.Location = new System.Drawing.Point(15, 105);
            this.btnMusic.Margin = new System.Windows.Forms.Padding(4);
            this.btnMusic.Name = "btnMusic";
            this.btnMusic.Size = new System.Drawing.Size(240, 45);
            this.btnMusic.TabIndex = 2;
            this.btnMusic.Text = "Music";
            // 
            // btnVideos
            // 
            this.btnVideos.Location = new System.Drawing.Point(15, 155);
            this.btnVideos.Margin = new System.Windows.Forms.Padding(4);
            this.btnVideos.Name = "btnVideos";
            this.btnVideos.Size = new System.Drawing.Size(240, 45);
            this.btnVideos.TabIndex = 3;
            this.btnVideos.Text = "Videos";
            // 
            // btnFavorites
            // 
            this.btnFavorites.Location = new System.Drawing.Point(15, 205);
            this.btnFavorites.Margin = new System.Windows.Forms.Padding(4);
            this.btnFavorites.Name = "btnFavorites";
            this.btnFavorites.Size = new System.Drawing.Size(240, 45);
            this.btnFavorites.TabIndex = 4;
            this.btnFavorites.Text = "Favorites";
            // 
            // btnRecent
            // 
            this.btnRecent.Location = new System.Drawing.Point(15, 255);
            this.btnRecent.Margin = new System.Windows.Forms.Padding(4);
            this.btnRecent.Name = "btnRecent";
            this.btnRecent.Size = new System.Drawing.Size(240, 45);
            this.btnRecent.TabIndex = 5;
            this.btnRecent.Text = "Recently played";
            // 
            // lblPlaylistSection
            // 
            this.lblPlaylistSection.AutoSize = true;
            this.lblPlaylistSection.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPlaylistSection.Location = new System.Drawing.Point(22, 329);
            this.lblPlaylistSection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPlaylistSection.Name = "lblPlaylistSection";
            this.lblPlaylistSection.Size = new System.Drawing.Size(78, 20);
            this.lblPlaylistSection.TabIndex = 6;
            this.lblPlaylistSection.Text = "PLAYLISTS";
            // 
            // btnNewPlaylist
            // 
            this.btnNewPlaylist.Location = new System.Drawing.Point(160, 316);
            this.btnNewPlaylist.Margin = new System.Windows.Forms.Padding(4);
            this.btnNewPlaylist.Name = "btnNewPlaylist";
            this.btnNewPlaylist.Size = new System.Drawing.Size(95, 35);
            this.btnNewPlaylist.TabIndex = 7;
            this.btnNewPlaylist.Text = "+ New";
            // 
            // lstPlaylists
            // 
            this.lstPlaylists.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstPlaylists.IntegralHeight = false;
            this.lstPlaylists.ItemHeight = 20;
            this.lstPlaylists.Location = new System.Drawing.Point(15, 368);
            this.lstPlaylists.Margin = new System.Windows.Forms.Padding(4);
            this.lstPlaylists.Name = "lstPlaylists";
            this.lstPlaylists.Size = new System.Drawing.Size(232, 323);
            this.lstPlaylists.TabIndex = 8;
            // 
            // btnPlaylistAddSelected
            // 
            this.btnPlaylistAddSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlaylistAddSelected.Location = new System.Drawing.Point(15, 705);
            this.btnPlaylistAddSelected.Margin = new System.Windows.Forms.Padding(4);
            this.btnPlaylistAddSelected.Name = "btnPlaylistAddSelected";
            this.btnPlaylistAddSelected.Size = new System.Drawing.Size(232, 40);
            this.btnPlaylistAddSelected.TabIndex = 9;
            this.btnPlaylistAddSelected.Text = "Add selected";
            // 
            // btnPlaylistRemoveSelected
            // 
            this.btnPlaylistRemoveSelected.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlaylistRemoveSelected.Location = new System.Drawing.Point(15, 752);
            this.btnPlaylistRemoveSelected.Margin = new System.Windows.Forms.Padding(4);
            this.btnPlaylistRemoveSelected.Name = "btnPlaylistRemoveSelected";
            this.btnPlaylistRemoveSelected.Size = new System.Drawing.Size(232, 40);
            this.btnPlaylistRemoveSelected.TabIndex = 10;
            this.btnPlaylistRemoveSelected.Text = "Remove from playlist";
            // 
            // btnDeletePlaylist
            // 
            this.btnDeletePlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeletePlaylist.Location = new System.Drawing.Point(15, 800);
            this.btnDeletePlaylist.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeletePlaylist.Name = "btnDeletePlaylist";
            this.btnDeletePlaylist.Size = new System.Drawing.Size(232, 40);
            this.btnDeletePlaylist.TabIndex = 11;
            this.btnDeletePlaylist.Text = "Delete playlist";
            // 
            // mainSplit
            // 
            this.mainSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.mainSplit.Location = new System.Drawing.Point(274, 4);
            this.mainSplit.Margin = new System.Windows.Forms.Padding(4);
            this.mainSplit.Name = "mainSplit";
            this.mainSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mainSplit.Panel1
            // 
            this.mainSplit.Panel1.Controls.Add(this.videoHost);
            this.mainSplit.Panel1.Padding = new System.Windows.Forms.Padding(18, 18, 18, 0);
            this.mainSplit.Panel1MinSize = 260;
            // 
            // mainSplit.Panel2
            // 
            this.mainSplit.Panel2.Controls.Add(this.libraryPanel);
            this.mainSplit.Panel2.Padding = new System.Windows.Forms.Padding(18, 0, 18, 15);
            this.mainSplit.Panel2MinSize = 220;
            this.mainSplit.Size = new System.Drawing.Size(1097, 863);
            this.mainSplit.SplitterDistance = 603;
            this.mainSplit.SplitterWidth = 10;
            this.mainSplit.TabIndex = 1;
            // 
            // videoHost
            // 
            this.videoHost.Controls.Add(this.videoSurface);
            this.videoHost.Controls.Add(this.stageToolbar);
            this.videoHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoHost.Location = new System.Drawing.Point(18, 18);
            this.videoHost.Margin = new System.Windows.Forms.Padding(4);
            this.videoHost.Name = "videoHost";
            this.videoHost.Padding = new System.Windows.Forms.Padding(1);
            this.videoHost.Size = new System.Drawing.Size(1061, 585);
            this.videoHost.TabIndex = 0;
            // 
            // videoSurface
            // 
            this.videoSurface.Controls.Add(this.audioOverlay);
            this.videoSurface.Controls.Add(this.videoView);
            this.videoSurface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoSurface.Location = new System.Drawing.Point(1, 53);
            this.videoSurface.Margin = new System.Windows.Forms.Padding(4);
            this.videoSurface.Name = "videoSurface";
            this.videoSurface.Padding = new System.Windows.Forms.Padding(12);
            this.videoSurface.Size = new System.Drawing.Size(1059, 531);
            this.videoSurface.TabIndex = 0;
            // 
            // audioOverlay
            // 
            this.audioOverlay.Controls.Add(this.lblAudioHint);
            this.audioOverlay.Controls.Add(this.lblAudioTitle);
            this.audioOverlay.Controls.Add(this.lblAudioIcon);
            this.audioOverlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.audioOverlay.Location = new System.Drawing.Point(12, 12);
            this.audioOverlay.Margin = new System.Windows.Forms.Padding(4);
            this.audioOverlay.Name = "audioOverlay";
            this.audioOverlay.Size = new System.Drawing.Size(1035, 507);
            this.audioOverlay.TabIndex = 0;
            // 
            // lblAudioHint
            // 
            this.lblAudioHint.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAudioHint.Location = new System.Drawing.Point(0, 290);
            this.lblAudioHint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAudioHint.Name = "lblAudioHint";
            this.lblAudioHint.Size = new System.Drawing.Size(1035, 45);
            this.lblAudioHint.TabIndex = 0;
            this.lblAudioHint.Text = "Add music or video files, then double-click an item to play.";
            this.lblAudioHint.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblAudioTitle
            // 
            this.lblAudioTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAudioTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 17F, System.Drawing.FontStyle.Bold);
            this.lblAudioTitle.Location = new System.Drawing.Point(0, 225);
            this.lblAudioTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAudioTitle.Name = "lblAudioTitle";
            this.lblAudioTitle.Size = new System.Drawing.Size(1035, 65);
            this.lblAudioTitle.TabIndex = 1;
            this.lblAudioTitle.Text = "Your media library is ready";
            this.lblAudioTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAudioIcon
            // 
            this.lblAudioIcon.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAudioIcon.Font = new System.Drawing.Font("Segoe UI Symbol", 72F);
            this.lblAudioIcon.Location = new System.Drawing.Point(0, 0);
            this.lblAudioIcon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAudioIcon.Name = "lblAudioIcon";
            this.lblAudioIcon.Size = new System.Drawing.Size(1035, 225);
            this.lblAudioIcon.TabIndex = 2;
            this.lblAudioIcon.Text = "♪";
            this.lblAudioIcon.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // videoView
            // 
            this.videoView.BackColor = System.Drawing.Color.Black;
            this.videoView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoView.Location = new System.Drawing.Point(12, 12);
            this.videoView.Margin = new System.Windows.Forms.Padding(4);
            this.videoView.MediaPlayer = null;
            this.videoView.Name = "videoView";
            this.videoView.Size = new System.Drawing.Size(1035, 507);
            this.videoView.TabIndex = 1;
            // 
            // stageToolbar
            // 
            this.stageToolbar.Controls.Add(this.lblStageTitle);
            this.stageToolbar.Controls.Add(this.lblStageHint);
            this.stageToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.stageToolbar.Location = new System.Drawing.Point(1, 1);
            this.stageToolbar.Margin = new System.Windows.Forms.Padding(4);
            this.stageToolbar.Name = "stageToolbar";
            this.stageToolbar.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.stageToolbar.Size = new System.Drawing.Size(1059, 52);
            this.stageToolbar.TabIndex = 1;
            // 
            // lblStageTitle
            // 
            this.lblStageTitle.AutoSize = true;
            this.lblStageTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblStageTitle.Location = new System.Drawing.Point(15, 15);
            this.lblStageTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStageTitle.Name = "lblStageTitle";
            this.lblStageTitle.Size = new System.Drawing.Size(110, 20);
            this.lblStageTitle.TabIndex = 0;
            this.lblStageTitle.Text = "NOW PLAYING";
            // 
            // lblStageHint
            // 
            this.lblStageHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStageHint.AutoSize = true;
            this.lblStageHint.Location = new System.Drawing.Point(667, 15);
            this.lblStageHint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStageHint.Name = "lblStageHint";
            this.lblStageHint.Size = new System.Drawing.Size(0, 20);
            this.lblStageHint.TabIndex = 1;
            // 
            // libraryPanel
            // 
            this.libraryPanel.Controls.Add(this.gridLibrary);
            this.libraryPanel.Controls.Add(this.libraryHeader);
            this.libraryPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.libraryPanel.Location = new System.Drawing.Point(18, 0);
            this.libraryPanel.Margin = new System.Windows.Forms.Padding(4);
            this.libraryPanel.Name = "libraryPanel";
            this.libraryPanel.Padding = new System.Windows.Forms.Padding(1);
            this.libraryPanel.Size = new System.Drawing.Size(1061, 235);
            this.libraryPanel.TabIndex = 0;
            // 
            // gridLibrary
            // 
            this.gridLibrary.AllowUserToAddRows = false;
            this.gridLibrary.AllowUserToDeleteRows = false;
            this.gridLibrary.AllowUserToResizeRows = false;
            this.gridLibrary.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridLibrary.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridLibrary.ColumnHeadersHeight = 29;
            this.gridLibrary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridLibrary.Location = new System.Drawing.Point(1, 66);
            this.gridLibrary.Margin = new System.Windows.Forms.Padding(4);
            this.gridLibrary.MultiSelect = false;
            this.gridLibrary.Name = "gridLibrary";
            this.gridLibrary.ReadOnly = true;
            this.gridLibrary.RowHeadersVisible = false;
            this.gridLibrary.RowHeadersWidth = 51;
            this.gridLibrary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridLibrary.Size = new System.Drawing.Size(1059, 168);
            this.gridLibrary.TabIndex = 0;
            // 
            // libraryHeader
            // 
            this.libraryHeader.Controls.Add(this.lblLibraryTitle);
            this.libraryHeader.Controls.Add(this.lblLibraryCount);
            this.libraryHeader.Controls.Add(this.cmbSort);
            this.libraryHeader.Controls.Add(this.btnCleanMissing);
            this.libraryHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.libraryHeader.Location = new System.Drawing.Point(1, 1);
            this.libraryHeader.Margin = new System.Windows.Forms.Padding(4);
            this.libraryHeader.Name = "libraryHeader";
            this.libraryHeader.Padding = new System.Windows.Forms.Padding(15, 10, 12, 8);
            this.libraryHeader.Size = new System.Drawing.Size(1059, 65);
            this.libraryHeader.TabIndex = 1;
            // 
            // lblLibraryTitle
            // 
            this.lblLibraryTitle.AutoSize = true;
            this.lblLibraryTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblLibraryTitle.Location = new System.Drawing.Point(15, 20);
            this.lblLibraryTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLibraryTitle.Name = "lblLibraryTitle";
            this.lblLibraryTitle.Size = new System.Drawing.Size(90, 25);
            this.lblLibraryTitle.TabIndex = 0;
            this.lblLibraryTitle.Text = "All media";
            // 
            // lblLibraryCount
            // 
            this.lblLibraryCount.AutoSize = true;
            this.lblLibraryCount.Location = new System.Drawing.Point(131, 22);
            this.lblLibraryCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLibraryCount.Name = "lblLibraryCount";
            this.lblLibraryCount.Size = new System.Drawing.Size(57, 20);
            this.lblLibraryCount.TabIndex = 1;
            this.lblLibraryCount.Text = "0 items";
            // 
            // cmbSort
            // 
            this.cmbSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSort.Items.AddRange(new object[] {
            "Title",
            "Recently played",
            "Most played",
            "Newest added"});
            this.cmbSort.Location = new System.Drawing.Point(691, 15);
            this.cmbSort.Margin = new System.Windows.Forms.Padding(4);
            this.cmbSort.Name = "cmbSort";
            this.cmbSort.Size = new System.Drawing.Size(174, 28);
            this.cmbSort.TabIndex = 2;
            // 
            // btnCleanMissing
            // 
            this.btnCleanMissing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCleanMissing.Location = new System.Drawing.Point(877, 12);
            this.btnCleanMissing.Margin = new System.Windows.Forms.Padding(4);
            this.btnCleanMissing.Name = "btnCleanMissing";
            this.btnCleanMissing.Size = new System.Drawing.Size(140, 38);
            this.btnCleanMissing.TabIndex = 3;
            this.btnCleanMissing.Text = "Clean missing";
            // 
            // queuePanel
            // 
            this.queuePanel.Controls.Add(this.queueLayout);
            this.queuePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queuePanel.Location = new System.Drawing.Point(1379, 4);
            this.queuePanel.Margin = new System.Windows.Forms.Padding(4);
            this.queuePanel.Name = "queuePanel";
            this.queuePanel.Padding = new System.Windows.Forms.Padding(15, 14, 15, 12);
            this.queuePanel.Size = new System.Drawing.Size(367, 863);
            this.queuePanel.TabIndex = 2;
            // 
            // queueLayout
            // 
            this.queueLayout.ColumnCount = 1;
            this.queueLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.queueLayout.Controls.Add(this.queueHeader, 0, 0);
            this.queueLayout.Controls.Add(this.queueHero, 0, 1);
            this.queueLayout.Controls.Add(this.trackDetailsPanel, 0, 2);
            this.queueLayout.Controls.Add(this.queueActions, 0, 3);
            this.queueLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queueLayout.Location = new System.Drawing.Point(15, 14);
            this.queueLayout.Margin = new System.Windows.Forms.Padding(0);
            this.queueLayout.Name = "queueLayout";
            this.queueLayout.RowCount = 4;
            this.queueLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.queueLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 382F));
            this.queueLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.queueLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.queueLayout.Size = new System.Drawing.Size(337, 837);
            this.queueLayout.TabIndex = 0;
            // 
            // queueHeader
            // 
            this.queueHeader.Controls.Add(this.lblQueueTitle);
            this.queueHeader.Controls.Add(this.lblQueueCount);
            this.queueHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queueHeader.Location = new System.Drawing.Point(0, 0);
            this.queueHeader.Margin = new System.Windows.Forms.Padding(0);
            this.queueHeader.Name = "queueHeader";
            this.queueHeader.Size = new System.Drawing.Size(337, 38);
            this.queueHeader.TabIndex = 0;
            // 
            // lblQueueTitle
            // 
            this.lblQueueTitle.AutoSize = true;
            this.lblQueueTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblQueueTitle.Location = new System.Drawing.Point(0, 5);
            this.lblQueueTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblQueueTitle.Name = "lblQueueTitle";
            this.lblQueueTitle.Size = new System.Drawing.Size(120, 25);
            this.lblQueueTitle.TabIndex = 0;
            this.lblQueueTitle.Text = "Now playing";
            // 
            // lblQueueCount
            // 
            this.lblQueueCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQueueCount.AutoSize = true;
            this.lblQueueCount.Location = new System.Drawing.Point(283, 7);
            this.lblQueueCount.Margin = new System.Windows.Forms.Padding(0);
            this.lblQueueCount.Name = "lblQueueCount";
            this.lblQueueCount.Size = new System.Drawing.Size(52, 20);
            this.lblQueueCount.TabIndex = 1;
            this.lblQueueCount.Text = "LOCAL";
            // 
            // queueHero
            // 
            this.queueHero.Controls.Add(this.lblQueueBannerHint);
            this.queueHero.Controls.Add(this.lblQueueBannerMeta);
            this.queueHero.Controls.Add(this.lblQueueBannerTitle);
            this.queueHero.Controls.Add(this.picQueueBanner);
            this.queueHero.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queueHero.Location = new System.Drawing.Point(0, 38);
            this.queueHero.Margin = new System.Windows.Forms.Padding(0);
            this.queueHero.Name = "queueHero";
            this.queueHero.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.queueHero.Size = new System.Drawing.Size(337, 382);
            this.queueHero.TabIndex = 1;
            // 
            // lblQueueBannerHint
            // 
            this.lblQueueBannerHint.AutoEllipsis = true;
            this.lblQueueBannerHint.Location = new System.Drawing.Point(0, 348);
            this.lblQueueBannerHint.Margin = new System.Windows.Forms.Padding(0);
            this.lblQueueBannerHint.Name = "lblQueueBannerHint";
            this.lblQueueBannerHint.Size = new System.Drawing.Size(337, 22);
            this.lblQueueBannerHint.TabIndex = 3;
            this.lblQueueBannerHint.Text = "Double-click artwork to personalize it";
            // 
            // lblQueueBannerMeta
            // 
            this.lblQueueBannerMeta.AutoEllipsis = true;
            this.lblQueueBannerMeta.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblQueueBannerMeta.Location = new System.Drawing.Point(0, 320);
            this.lblQueueBannerMeta.Margin = new System.Windows.Forms.Padding(0);
            this.lblQueueBannerMeta.Name = "lblQueueBannerMeta";
            this.lblQueueBannerMeta.Size = new System.Drawing.Size(337, 24);
            this.lblQueueBannerMeta.TabIndex = 2;
            this.lblQueueBannerMeta.Text = "Choose a song or video from your library.";
            // 
            // lblQueueBannerTitle
            // 
            this.lblQueueBannerTitle.AutoEllipsis = true;
            this.lblQueueBannerTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.lblQueueBannerTitle.Location = new System.Drawing.Point(0, 286);
            this.lblQueueBannerTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblQueueBannerTitle.Name = "lblQueueBannerTitle";
            this.lblQueueBannerTitle.Size = new System.Drawing.Size(337, 32);
            this.lblQueueBannerTitle.TabIndex = 1;
            this.lblQueueBannerTitle.Text = "Nothing playing";
            // 
            // picQueueBanner
            // 
            this.picQueueBanner.BackColor = System.Drawing.Color.Black;
            this.picQueueBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this.picQueueBanner.Location = new System.Drawing.Point(0, 0);
            this.picQueueBanner.Margin = new System.Windows.Forms.Padding(0);
            this.picQueueBanner.Name = "picQueueBanner";
            this.picQueueBanner.Size = new System.Drawing.Size(337, 275);
            this.picQueueBanner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picQueueBanner.TabIndex = 0;
            this.picQueueBanner.TabStop = false;
            // 
            // trackDetailsPanel
            // 
            this.trackDetailsPanel.Controls.Add(this.lblDetailsFavorite);
            this.trackDetailsPanel.Controls.Add(this.lblDetailsLocation);
            this.trackDetailsPanel.Controls.Add(this.lblDetailsAdded);
            this.trackDetailsPanel.Controls.Add(this.lblDetailsPlays);
            this.trackDetailsPanel.Controls.Add(this.lblDetailsType);
            this.trackDetailsPanel.Controls.Add(this.lblDetailsSection);
            this.trackDetailsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackDetailsPanel.Location = new System.Drawing.Point(0, 428);
            this.trackDetailsPanel.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.trackDetailsPanel.Name = "trackDetailsPanel";
            this.trackDetailsPanel.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.trackDetailsPanel.Size = new System.Drawing.Size(337, 351);
            this.trackDetailsPanel.TabIndex = 2;
            // 
            // lblDetailsFavorite
            // 
            this.lblDetailsFavorite.AutoEllipsis = true;
            this.lblDetailsFavorite.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblDetailsFavorite.Location = new System.Drawing.Point(12, 202);
            this.lblDetailsFavorite.Name = "lblDetailsFavorite";
            this.lblDetailsFavorite.Size = new System.Drawing.Size(308, 24);
            this.lblDetailsFavorite.TabIndex = 5;
            this.lblDetailsFavorite.Text = "♡ Not in favorites";
            // 
            // lblDetailsLocation
            // 
            this.lblDetailsLocation.AutoEllipsis = true;
            this.lblDetailsLocation.Location = new System.Drawing.Point(12, 148);
            this.lblDetailsLocation.Name = "lblDetailsLocation";
            this.lblDetailsLocation.Size = new System.Drawing.Size(308, 44);
            this.lblDetailsLocation.TabIndex = 4;
            this.lblDetailsLocation.Text = "Location  —";
            // 
            // lblDetailsAdded
            // 
            this.lblDetailsAdded.AutoEllipsis = true;
            this.lblDetailsAdded.Location = new System.Drawing.Point(12, 116);
            this.lblDetailsAdded.Name = "lblDetailsAdded";
            this.lblDetailsAdded.Size = new System.Drawing.Size(308, 24);
            this.lblDetailsAdded.TabIndex = 3;
            this.lblDetailsAdded.Text = "Added  —";
            // 
            // lblDetailsPlays
            // 
            this.lblDetailsPlays.AutoEllipsis = true;
            this.lblDetailsPlays.Location = new System.Drawing.Point(12, 84);
            this.lblDetailsPlays.Name = "lblDetailsPlays";
            this.lblDetailsPlays.Size = new System.Drawing.Size(308, 24);
            this.lblDetailsPlays.TabIndex = 2;
            this.lblDetailsPlays.Text = "Plays  —";
            // 
            // lblDetailsType
            // 
            this.lblDetailsType.AutoEllipsis = true;
            this.lblDetailsType.Location = new System.Drawing.Point(12, 52);
            this.lblDetailsType.Name = "lblDetailsType";
            this.lblDetailsType.Size = new System.Drawing.Size(308, 24);
            this.lblDetailsType.TabIndex = 1;
            this.lblDetailsType.Text = "Type  —";
            // 
            // lblDetailsSection
            // 
            this.lblDetailsSection.AutoSize = true;
            this.lblDetailsSection.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblDetailsSection.Location = new System.Drawing.Point(12, 12);
            this.lblDetailsSection.Name = "lblDetailsSection";
            this.lblDetailsSection.Size = new System.Drawing.Size(113, 20);
            this.lblDetailsSection.TabIndex = 0;
            this.lblDetailsSection.Text = "TRACK DETAILS";
            // 
            // queueActions
            // 
            this.queueActions.Controls.Add(this.btnQueueUp);
            this.queueActions.Controls.Add(this.btnQueueDown);
            this.queueActions.Controls.Add(this.btnQueueRemove);
            this.queueActions.Controls.Add(this.btnQueueClear);
            this.queueActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queueActions.Location = new System.Drawing.Point(0, 779);
            this.queueActions.Margin = new System.Windows.Forms.Padding(0);
            this.queueActions.Name = "queueActions";
            this.queueActions.Size = new System.Drawing.Size(337, 58);
            this.queueActions.TabIndex = 3;
            // 
            // btnQueueUp
            // 
            this.btnQueueUp.Location = new System.Drawing.Point(0, 9);
            this.btnQueueUp.Margin = new System.Windows.Forms.Padding(0);
            this.btnQueueUp.Name = "btnQueueUp";
            this.btnQueueUp.Size = new System.Drawing.Size(46, 40);
            this.btnQueueUp.TabIndex = 0;
            this.btnQueueUp.Text = "♡";
            // 
            // btnQueueDown
            // 
            this.btnQueueDown.Location = new System.Drawing.Point(52, 9);
            this.btnQueueDown.Margin = new System.Windows.Forms.Padding(0);
            this.btnQueueDown.Name = "btnQueueDown";
            this.btnQueueDown.Size = new System.Drawing.Size(86, 40);
            this.btnQueueDown.TabIndex = 1;
            this.btnQueueDown.Text = "Artwork";
            // 
            // btnQueueRemove
            // 
            this.btnQueueRemove.Location = new System.Drawing.Point(144, 9);
            this.btnQueueRemove.Margin = new System.Windows.Forms.Padding(0);
            this.btnQueueRemove.Name = "btnQueueRemove";
            this.btnQueueRemove.Size = new System.Drawing.Size(84, 40);
            this.btnQueueRemove.TabIndex = 2;
            this.btnQueueRemove.Text = "Folder";
            // 
            // btnQueueClear
            // 
            this.btnQueueClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQueueClear.Location = new System.Drawing.Point(234, 9);
            this.btnQueueClear.Margin = new System.Windows.Forms.Padding(0);
            this.btnQueueClear.Name = "btnQueueClear";
            this.btnQueueClear.Size = new System.Drawing.Size(103, 40);
            this.btnQueueClear.TabIndex = 3;
            this.btnQueueClear.Text = "Copy path";
            // 
            // transportPanel
            // 
            this.transportPanel.Controls.Add(this.transportLayout);
            this.transportPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.transportPanel.Location = new System.Drawing.Point(4, 965);
            this.transportPanel.Margin = new System.Windows.Forms.Padding(4);
            this.transportPanel.Name = "transportPanel";
            this.transportPanel.Padding = new System.Windows.Forms.Padding(20, 12, 20, 12);
            this.transportPanel.Size = new System.Drawing.Size(1742, 137);
            this.transportPanel.TabIndex = 2;
            // 
            // transportLayout
            // 
            this.transportLayout.ColumnCount = 3;
            this.transportLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31F));
            this.transportLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44F));
            this.transportLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.transportLayout.Controls.Add(this.nowPlayingPanel, 0, 0);
            this.transportLayout.Controls.Add(this.centerTransportPanel, 1, 0);
            this.transportLayout.Controls.Add(this.volumePanel, 2, 0);
            this.transportLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.transportLayout.Location = new System.Drawing.Point(20, 12);
            this.transportLayout.Margin = new System.Windows.Forms.Padding(4);
            this.transportLayout.Name = "transportLayout";
            this.transportLayout.RowCount = 1;
            this.transportLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.transportLayout.Size = new System.Drawing.Size(1702, 113);
            this.transportLayout.TabIndex = 0;
            // 
            // nowPlayingPanel
            // 
            this.nowPlayingPanel.Controls.Add(this.lblNowPlayingCaption);
            this.nowPlayingPanel.Controls.Add(this.lblNowPlayingTitle);
            this.nowPlayingPanel.Controls.Add(this.lblNowPlayingMeta);
            this.nowPlayingPanel.Controls.Add(this.lblStatus);
            this.nowPlayingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nowPlayingPanel.Location = new System.Drawing.Point(4, 4);
            this.nowPlayingPanel.Margin = new System.Windows.Forms.Padding(4);
            this.nowPlayingPanel.Name = "nowPlayingPanel";
            this.nowPlayingPanel.Padding = new System.Windows.Forms.Padding(2, 0, 15, 0);
            this.nowPlayingPanel.Size = new System.Drawing.Size(519, 105);
            this.nowPlayingPanel.TabIndex = 0;
            // 
            // lblNowPlayingCaption
            // 
            this.lblNowPlayingCaption.AutoSize = true;
            this.lblNowPlayingCaption.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblNowPlayingCaption.Location = new System.Drawing.Point(2, 5);
            this.lblNowPlayingCaption.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNowPlayingCaption.Name = "lblNowPlayingCaption";
            this.lblNowPlayingCaption.Size = new System.Drawing.Size(110, 20);
            this.lblNowPlayingCaption.TabIndex = 0;
            this.lblNowPlayingCaption.Text = "NOW PLAYING";
            // 
            // lblNowPlayingTitle
            // 
            this.lblNowPlayingTitle.AutoEllipsis = true;
            this.lblNowPlayingTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblNowPlayingTitle.Location = new System.Drawing.Point(2, 34);
            this.lblNowPlayingTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNowPlayingTitle.Name = "lblNowPlayingTitle";
            this.lblNowPlayingTitle.Size = new System.Drawing.Size(462, 28);
            this.lblNowPlayingTitle.TabIndex = 1;
            this.lblNowPlayingTitle.Text = "Nothing playing";
            // 
            // lblNowPlayingMeta
            // 
            this.lblNowPlayingMeta.AutoEllipsis = true;
            this.lblNowPlayingMeta.Location = new System.Drawing.Point(2, 65);
            this.lblNowPlayingMeta.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNowPlayingMeta.Name = "lblNowPlayingMeta";
            this.lblNowPlayingMeta.Size = new System.Drawing.Size(462, 24);
            this.lblNowPlayingMeta.TabIndex = 2;
            this.lblNowPlayingMeta.Text = "Import media to build your local library";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoEllipsis = true;
            this.lblStatus.Location = new System.Drawing.Point(2, 88);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(462, 22);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Ready";
            // 
            // centerTransportPanel
            // 
            this.centerTransportPanel.Controls.Add(this.btnShuffle);
            this.centerTransportPanel.Controls.Add(this.btnPrevious);
            this.centerTransportPanel.Controls.Add(this.btnPlayPause);
            this.centerTransportPanel.Controls.Add(this.btnStop);
            this.centerTransportPanel.Controls.Add(this.btnNext);
            this.centerTransportPanel.Controls.Add(this.btnRepeat);
            this.centerTransportPanel.Controls.Add(this.trackProgress);
            this.centerTransportPanel.Controls.Add(this.lblElapsed);
            this.centerTransportPanel.Controls.Add(this.lblDuration);
            this.centerTransportPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.centerTransportPanel.Location = new System.Drawing.Point(531, 4);
            this.centerTransportPanel.Margin = new System.Windows.Forms.Padding(4);
            this.centerTransportPanel.Name = "centerTransportPanel";
            this.centerTransportPanel.Size = new System.Drawing.Size(740, 105);
            this.centerTransportPanel.TabIndex = 1;
            // 
            // btnShuffle
            // 
            this.btnShuffle.Location = new System.Drawing.Point(71, 37);
            this.btnShuffle.Margin = new System.Windows.Forms.Padding(4);
            this.btnShuffle.Name = "btnShuffle";
            this.btnShuffle.Size = new System.Drawing.Size(98, 40);
            this.btnShuffle.TabIndex = 3;
            this.btnShuffle.Text = "Shuffle";
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(183, 34);
            this.btnPrevious.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(55, 48);
            this.btnPrevious.TabIndex = 4;
            this.btnPrevious.Text = "|◀";
            // 
            // btnPlayPause
            // 
            this.btnPlayPause.Location = new System.Drawing.Point(250, 27);
            this.btnPlayPause.Margin = new System.Windows.Forms.Padding(4);
            this.btnPlayPause.Name = "btnPlayPause";
            this.btnPlayPause.Size = new System.Drawing.Size(65, 60);
            this.btnPlayPause.TabIndex = 5;
            this.btnPlayPause.Text = "▶";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(324, 34);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(55, 48);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "■";
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(391, 34);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(55, 48);
            this.btnNext.TabIndex = 7;
            this.btnNext.Text = "▶|";
            // 
            // btnRepeat
            // 
            this.btnRepeat.Location = new System.Drawing.Point(461, 37);
            this.btnRepeat.Margin = new System.Windows.Forms.Padding(4);
            this.btnRepeat.Name = "btnRepeat";
            this.btnRepeat.Size = new System.Drawing.Size(120, 40);
            this.btnRepeat.TabIndex = 8;
            this.btnRepeat.Text = "Repeat: Off";
            // 
            // trackProgress
            // 
            this.trackProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackProgress.Location = new System.Drawing.Point(52, 0);
            this.trackProgress.Margin = new System.Windows.Forms.Padding(4);
            this.trackProgress.Maximum = 1000;
            this.trackProgress.Name = "trackProgress";
            this.trackProgress.Size = new System.Drawing.Size(626, 56);
            this.trackProgress.TabIndex = 0;
            this.trackProgress.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // lblElapsed
            // 
            this.lblElapsed.AutoSize = true;
            this.lblElapsed.Location = new System.Drawing.Point(0, 10);
            this.lblElapsed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblElapsed.Name = "lblElapsed";
            this.lblElapsed.Size = new System.Drawing.Size(44, 20);
            this.lblElapsed.TabIndex = 1;
            this.lblElapsed.Text = "00:00";
            // 
            // lblDuration
            // 
            this.lblDuration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(696, 10);
            this.lblDuration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(44, 20);
            this.lblDuration.TabIndex = 2;
            this.lblDuration.Text = "00:00";
            // 
            // volumePanel
            // 
            this.volumePanel.Controls.Add(this.lblVolume);
            this.volumePanel.Controls.Add(this.cmbSpeed);
            this.volumePanel.Controls.Add(this.btnMute);
            this.volumePanel.Controls.Add(this.trackVolume);
            this.volumePanel.Controls.Add(this.btnFullscreen);
            this.volumePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.volumePanel.Location = new System.Drawing.Point(1279, 4);
            this.volumePanel.Margin = new System.Windows.Forms.Padding(4);
            this.volumePanel.Name = "volumePanel";
            this.volumePanel.Size = new System.Drawing.Size(419, 105);
            this.volumePanel.TabIndex = 2;
            // 
            // lblVolume
            // 
            this.lblVolume.AutoSize = true;
            this.lblVolume.Location = new System.Drawing.Point(231, 23);
            this.lblVolume.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(37, 20);
            this.lblVolume.TabIndex = 2;
            this.lblVolume.Text = "70%";
            this.lblVolume.Click += new System.EventHandler(this.lblVolume_Click);
            // 
            // cmbSpeed
            // 
            this.cmbSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSpeed.Items.AddRange(new object[] {
            "0.75x",
            "1.0x",
            "1.25x",
            "1.5x",
            "2.0x"});
            this.cmbSpeed.Location = new System.Drawing.Point(194, 48);
            this.cmbSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.cmbSpeed.Name = "cmbSpeed";
            this.cmbSpeed.Size = new System.Drawing.Size(96, 28);
            this.cmbSpeed.TabIndex = 3;
            // 
            // btnMute
            // 
            this.btnMute.Location = new System.Drawing.Point(58, 22);
            this.btnMute.Margin = new System.Windows.Forms.Padding(4);
            this.btnMute.Name = "btnMute";
            this.btnMute.Size = new System.Drawing.Size(22, 22);
            this.btnMute.TabIndex = 0;
            this.btnMute.Click += new System.EventHandler(this.btnMute_Click);
            // 
            // trackVolume
            // 
            this.trackVolume.Location = new System.Drawing.Point(62, 21);
            this.trackVolume.Margin = new System.Windows.Forms.Padding(4);
            this.trackVolume.Maximum = 100;
            this.trackVolume.Name = "trackVolume";
            this.trackVolume.Size = new System.Drawing.Size(170, 56);
            this.trackVolume.TabIndex = 1;
            this.trackVolume.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackVolume.Value = 70;
            this.trackVolume.Scroll += new System.EventHandler(this.trackVolume_Scroll);
            // 
            // btnFullscreen
            // 
            this.btnFullscreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFullscreen.Location = new System.Drawing.Point(307, 40);
            this.btnFullscreen.Margin = new System.Windows.Forms.Padding(4);
            this.btnFullscreen.Name = "btnFullscreen";
            this.btnFullscreen.Size = new System.Drawing.Size(47, 42);
            this.btnFullscreen.TabIndex = 4;
            // 
            // listQueue
            // 
            this.listQueue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listQueue.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.queueTitleColumn,
            this.queueTypeColumn});
            this.listQueue.FullRowSelect = true;
            this.listQueue.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listQueue.HideSelection = false;
            this.listQueue.Location = new System.Drawing.Point(0, 0);
            this.listQueue.Name = "listQueue";
            this.listQueue.Size = new System.Drawing.Size(1, 1);
            this.listQueue.TabIndex = 9;
            this.listQueue.UseCompatibleStateImageBehavior = false;
            this.listQueue.View = System.Windows.Forms.View.Details;
            this.listQueue.Visible = false;
            // 
            // queueTitleColumn
            // 
            this.queueTitleColumn.Text = "Title";
            // 
            // queueTypeColumn
            // 
            this.queueTypeColumn.Text = "Type";
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1750, 1106);
            this.Controls.Add(this.rootLayout);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1470, 913);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "not.asser music/video player";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.rootLayout.ResumeLayout(false);
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contentLayout.ResumeLayout(false);
            this.sidebar.ResumeLayout(false);
            this.sidebar.PerformLayout();
            this.mainSplit.Panel1.ResumeLayout(false);
            this.mainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplit)).EndInit();
            this.mainSplit.ResumeLayout(false);
            this.videoHost.ResumeLayout(false);
            this.videoSurface.ResumeLayout(false);
            this.audioOverlay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.videoView)).EndInit();
            this.stageToolbar.ResumeLayout(false);
            this.stageToolbar.PerformLayout();
            this.libraryPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridLibrary)).EndInit();
            this.libraryHeader.ResumeLayout(false);
            this.libraryHeader.PerformLayout();
            this.queuePanel.ResumeLayout(false);
            this.queueLayout.ResumeLayout(false);
            this.queueHeader.ResumeLayout(false);
            this.queueHeader.PerformLayout();
            this.queueHero.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picQueueBanner)).EndInit();
            this.trackDetailsPanel.ResumeLayout(false);
            this.trackDetailsPanel.PerformLayout();
            this.queueActions.ResumeLayout(false);
            this.transportPanel.ResumeLayout(false);
            this.transportLayout.ResumeLayout(false);
            this.nowPlayingPanel.ResumeLayout(false);
            this.nowPlayingPanel.PerformLayout();
            this.centerTransportPanel.ResumeLayout(false);
            this.centerTransportPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackProgress)).EndInit();
            this.volumePanel.ResumeLayout(false);
            this.volumePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackVolume)).EndInit();
            this.ResumeLayout(false);

        }

        private PictureBox pictureBox1;
    }
}
