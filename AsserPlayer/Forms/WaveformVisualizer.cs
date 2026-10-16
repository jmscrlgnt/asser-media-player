using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace AsserPlayer.Forms
{
    internal sealed class WaveformVisualizer : Control
    {
        private string _trackTitle = "Your media library is ready";
        private string _artist = "Add local music or video files to start listening";
        private double _progress;
        private bool _playing;
        private Image _artwork;
        private readonly Timer _animationTimer;
        private int _seed = 17;

        public WaveformVisualizer()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            BackColor = UiTheme.PlayerSurface;
            _animationTimer = new Timer { Interval = 45 };
            _animationTimer.Tick += delegate
            {
                if (_playing) Invalidate();
            };
        }

        public void SetTrack(string title, string artist)
        {
            _trackTitle = string.IsNullOrWhiteSpace(title) ? "Untitled" : title.Trim();
            _artist = string.IsNullOrWhiteSpace(artist) ? "Local audio" : artist.Trim();
            _progress = 0;

            unchecked
            {
                _seed = 17;
                foreach (var ch in (_trackTitle + "|" + _artist))
                    _seed = (_seed * 31) + ch;
            }

            Invalidate();
        }

        public void SetArtwork(string artworkPath)
        {
            if (_artwork != null)
            {
                _artwork.Dispose();
                _artwork = null;
            }

            if (!string.IsNullOrWhiteSpace(artworkPath) && File.Exists(artworkPath))
            {
                try
                {
                    using (var source = Image.FromFile(artworkPath))
                        _artwork = new Bitmap(source);
                }
                catch
                {
                    _artwork = null;
                }
            }

            Invalidate();
        }

        public void SetPlaying(bool playing)
        {
            _playing = playing;
            if (playing) _animationTimer.Start();
            else _animationTimer.Stop();
            Invalidate();
        }

        public void SetProgress(double progress)
        {
            _progress = Math.Max(0, Math.Min(1, progress));
            Invalidate();
        }

        public void ResetView()
        {
            _trackTitle = "Your media library is ready";
            _artist = "Add local music or video files to start listening";
            _progress = 0;
            _playing = false;
            _animationTimer.Stop();
            SetArtwork(null);
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _animationTimer.Stop();
                _animationTimer.Dispose();
                if (_artwork != null) _artwork.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            var w = ClientSize.Width;
            var h = ClientSize.Height;
            if (w <= 40 || h <= 40) return;

            DrawBackground(g, w, h);
            DrawWaveField(g, w, h);
            DrawArtwork(g, w, h);
            DrawTrackText(g, w, h);
        }

        private void DrawBackground(Graphics g, int w, int h)
        {
            using (var brush = new LinearGradientBrush(
                new Rectangle(0, 0, w, h),
                Color.FromArgb(5, 10, 22),
                Color.FromArgb(8, 11, 18),
                90F))
            {
                g.FillRectangle(brush, 0, 0, w, h);
            }

            var glowRect = new Rectangle(w / 8, h / 5, w * 3 / 4, h * 2 / 3);
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(glowRect);
                using (var glow = new PathGradientBrush(path))
                {
                    glow.CenterColor = Color.FromArgb(42, 40, 92, 180);
                    glow.SurroundColors = new[] { Color.FromArgb(0, 23, 40, 78) };
                    g.FillEllipse(glow, glowRect);
                }
            }
        }

        private void DrawWaveField(Graphics g, int w, int h)
        {
            var left = Math.Max(34, w / 12);
            var right = w - left;
            var width = Math.Max(80, right - left);
            var centerY = Math.Max(h / 2 + 18, Math.Min(h - 125, h * 61 / 100));
            var baseAmplitude = Math.Max(26F, Math.Min(78F, h * 0.16F));
            var phase = (_playing ? Environment.TickCount / 620.0 : 0.0) + ((_seed & 0xFF) / 60.0);

            // Soft luminous core line.
            using (var glowPen = new Pen(Color.FromArgb(28, 116, 195, 255), 7F))
            using (var corePen = new Pen(Color.FromArgb(170, 190, 216, 255), 1.35F))
            {
                var core = BuildWavePath(left, width, centerY, baseAmplitude * 0.56F, phase, 0.0, 0F);
                g.DrawPath(glowPen, core);
                g.DrawPath(corePen, core);
                core.Dispose();
            }

            // Layered smooth strands inspired by abstract audio-wave art.
            const int layers = 13;
            for (var layer = 0; layer < layers; layer++)
            {
                var centered = layer - ((layers - 1) / 2F);
                var offset = centered * 2.7F;
                var amplitude = baseAmplitude * (0.80F - Math.Abs(centered) * 0.035F);
                var localPhase = phase + centered * 0.095;
                var secondary = centered * 0.12;

                var path = BuildWavePath(left, width, centerY + offset, amplitude, localPhase, secondary, centered);

                var alpha = (int)Math.Max(36, 122 - Math.Abs(centered) * 9);
                var r = (int)(132 + Math.Max(0, centered) * 4);
                var gg = (int)(165 + Math.Max(0, -centered) * 4);
                var b = 255;
                using (var pen = new Pen(Color.FromArgb(alpha, Math.Min(220, r), Math.Min(230, gg), b), 1.1F))
                    g.DrawPath(pen, path);

                path.Dispose();
            }

            // Progress glow across the lower edge of the wave field.
            var progressWidth = (int)(width * _progress);
            if (progressWidth > 0)
            {
                var y = centerY + baseAmplitude + 24;
                using (var progressBrush = new LinearGradientBrush(
                    new RectangleF(left, y, Math.Max(1, progressWidth), 2F),
                    UiTheme.Accent,
                    Color.FromArgb(90, 112, 214, 255),
                    0F))
                {
                    g.FillRectangle(progressBrush, left, y, progressWidth, 2F);
                }
            }
        }

        private GraphicsPath BuildWavePath(int left, int width, float centerY, float amplitude, double phase, double secondaryPhase, float layerOffset)
        {
            var path = new GraphicsPath();
            const int points = 160;
            PointF? previous = null;

            for (var i = 0; i < points; i++)
            {
                var t = i / (double)(points - 1);
                var x = left + (float)(t * width);
                var envelope = Math.Pow(Math.Sin(Math.PI * t), 0.72);
                var first = Math.Sin((t * Math.PI * 5.1) + phase + secondaryPhase);
                var second = Math.Sin((t * Math.PI * 10.4) - phase * 0.55 + (_seed % 17) * 0.035);
                var third = Math.Cos((t * Math.PI * 2.1) + phase * 0.25 + layerOffset * 0.06);
                var y = centerY + (float)(envelope * amplitude * ((first * 0.62) + (second * 0.24) + (third * 0.14)));
                var current = new PointF(x, y);

                if (previous.HasValue)
                    path.AddLine(previous.Value, current);

                previous = current;
            }

            return path;
        }

        private void DrawArtwork(Graphics g, int w, int h)
        {
            var tile = Math.Max(118, Math.Min(190, Math.Min(w / 5, h / 2 - 42)));
            var x = (w - tile) / 2;
            var y = Math.Max(28, h / 12);
            var rect = new Rectangle(x, y, tile, tile);

            using (var shadow = new SolidBrush(Color.FromArgb(86, 0, 0, 0)))
                g.FillRectangle(shadow, rect.X + 8, rect.Y + 10, rect.Width, rect.Height);

            using (var background = new LinearGradientBrush(rect, Color.FromArgb(58, 44, 126), Color.FromArgb(24, 24, 54), 45F))
                g.FillRectangle(background, rect);

            if (_artwork != null)
                DrawImageContain(g, _artwork, rect);
            else
                DrawMusicMark(g, rect);

            using (var pen = new Pen(Color.FromArgb(78, 156, 171, 255), 1F))
                g.DrawRectangle(pen, rect);
        }

        private static void DrawImageContain(Graphics g, Image image, Rectangle bounds)
        {
            var ratio = Math.Min(bounds.Width / (double)image.Width, bounds.Height / (double)image.Height);
            var width = Math.Max(1, (int)Math.Round(image.Width * ratio));
            var height = Math.Max(1, (int)Math.Round(image.Height * ratio));
            var x = bounds.X + (bounds.Width - width) / 2;
            var y = bounds.Y + (bounds.Height - height) / 2;
            g.DrawImage(image, new Rectangle(x, y, width, height));
        }

        private static void DrawMusicMark(Graphics g, Rectangle rect)
        {
            using (var noteFont = new Font("Segoe UI Symbol", rect.Width * 0.30F, FontStyle.Regular, GraphicsUnit.Pixel))
            using (var noteBrush = new SolidBrush(Color.FromArgb(232, 216, 210, 255)))
            {
                var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString("♪", noteFont, noteBrush, rect, format);
            }
        }

        private void DrawTrackText(Graphics g, int w, int h)
        {
            var titleY = h - 78;
            using (var titleFont = new Font("Segoe UI Semibold", 15F, FontStyle.Bold))
            using (var metaFont = new Font("Segoe UI", 9.5F))
            using (var titleBrush = new SolidBrush(UiTheme.Text))
            using (var metaBrush = new SolidBrush(UiTheme.MutedText))
            {
                var format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter
                };

                g.DrawString(_trackTitle, titleFont, titleBrush, new RectangleF(34, titleY, w - 68, 28), format);
                g.DrawString(_artist, metaFont, metaBrush, new RectangleF(34, titleY + 31, w - 68, 24), format);
            }
        }
    }
}
