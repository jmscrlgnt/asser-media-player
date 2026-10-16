using System;
using System.Drawing;
using System.Windows.Forms;

namespace AsserPlayer.Forms
{
    internal sealed class DarkVScrollBar : Control
    {
        private int _maximum;
        private int _largeChange = 1;
        private int _value;
        private bool _dragging;
        private int _dragOffset;
        private bool _hoveringThumb;

        public DarkVScrollBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
            Width = 10;
            MinimumSize = new Size(8, 40);
            Cursor = Cursors.Default;
            TabStop = false;
        }

        public event EventHandler ValueChanged;

        public int Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = Math.Max(0, value);
                Value = Math.Min(_value, _maximum);
                Invalidate();
            }
        }

        public int LargeChange
        {
            get { return _largeChange; }
            set
            {
                _largeChange = Math.Max(1, value);
                Invalidate();
            }
        }

        public int Value
        {
            get { return _value; }
            set
            {
                var next = Math.Max(0, Math.Min(_maximum, value));
                if (next == _value) return;
                _value = next;
                Invalidate();
                var handler = ValueChanged;
                if (handler != null) handler(this, EventArgs.Empty);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(UiTheme.Surface);

            using (var trackBrush = new SolidBrush(Color.FromArgb(12, 17, 27)))
                e.Graphics.FillRectangle(trackBrush, ClientRectangle);

            var thumb = GetThumbRectangle();
            var thumbColor = _hoveringThumb || _dragging
                ? Color.FromArgb(89, 101, 126)
                : Color.FromArgb(57, 68, 88);

            using (var brush = new SolidBrush(thumbColor))
                e.Graphics.FillRectangle(brush, thumb);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var thumb = GetThumbRectangle();
            var hovering = thumb.Contains(e.Location);
            if (hovering != _hoveringThumb)
            {
                _hoveringThumb = hovering;
                Invalidate();
            }

            if (!_dragging || _maximum <= 0) return;

            var thumbHeight = thumb.Height;
            var trackTravel = Math.Max(1, ClientSize.Height - thumbHeight);
            var y = Math.Max(0, Math.Min(trackTravel, e.Y - _dragOffset));
            Value = (int)Math.Round((y / (double)trackTravel) * _maximum);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left || _maximum <= 0) return;

            var thumb = GetThumbRectangle();
            if (thumb.Contains(e.Location))
            {
                _dragging = true;
                _dragOffset = e.Y - thumb.Top;
                Capture = true;
                Invalidate();
                return;
            }

            Value += e.Y < thumb.Top ? -LargeChange : LargeChange;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _dragging = false;
            Capture = false;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_dragging) return;
            _hoveringThumb = false;
            Invalidate();
        }

        private Rectangle GetThumbRectangle()
        {
            if (ClientSize.Height <= 0)
                return Rectangle.Empty;

            if (_maximum <= 0)
                return new Rectangle(2, 2, Math.Max(2, ClientSize.Width - 4), Math.Max(20, ClientSize.Height - 4));

            var rangeSize = _maximum + _largeChange;
            var ratio = Math.Max(0.08, Math.Min(1.0, _largeChange / (double)Math.Max(1, rangeSize)));
            var thumbHeight = Math.Max(28, (int)Math.Round(ClientSize.Height * ratio));
            thumbHeight = Math.Min(ClientSize.Height, thumbHeight);

            var travel = Math.Max(0, ClientSize.Height - thumbHeight);
            var top = _maximum == 0 ? 0 : (int)Math.Round((_value / (double)_maximum) * travel);
            return new Rectangle(2, top, Math.Max(2, ClientSize.Width - 4), thumbHeight);
        }
    }
}
