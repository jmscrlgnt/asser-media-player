using System.Drawing;
using System.Windows.Forms;

namespace AsserPlayer.Forms
{
    internal static class UiTheme
    {
        public static readonly Color Background = Color.FromArgb(8, 11, 18);
        public static readonly Color Sidebar = Color.FromArgb(11, 15, 24);
        public static readonly Color TopBar = Color.FromArgb(13, 18, 29);
        public static readonly Color Surface = Color.FromArgb(16, 22, 34);
        public static readonly Color PlayerSurface = Color.FromArgb(10, 14, 23);
        public static readonly Color SurfaceAlt = Color.FromArgb(24, 31, 47);
        public static readonly Color Input = Color.FromArgb(12, 17, 27);
        public static readonly Color Border = Color.FromArgb(34, 42, 60);
        public static readonly Color Accent = Color.FromArgb(111, 89, 255);
        public static readonly Color AccentHover = Color.FromArgb(132, 113, 255);
        public static readonly Color AccentSoft = Color.FromArgb(167, 154, 255);
        public static readonly Color Text = Color.FromArgb(242, 245, 249);
        public static readonly Color MutedText = Color.FromArgb(151, 162, 181);
        public static readonly Color SubtleText = Color.FromArgb(108, 120, 141);
        public static readonly Color Success = Color.FromArgb(56, 189, 128);
        public static readonly Color Danger = Color.FromArgb(248, 113, 113);
        public static readonly Color DangerSurface = Color.FromArgb(52, 28, 34);

        public static void StylePrimaryButton(Button button)
        {
            button.BackColor = Accent;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = AccentHover;
            button.FlatAppearance.MouseDownBackColor = Accent;
            button.Cursor = Cursors.Hand;
        }

        public static void StyleSecondaryButton(Button button)
        {
            button.BackColor = SurfaceAlt;
            button.ForeColor = Text;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Border;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(31, 40, 60);
            button.Cursor = Cursors.Hand;
        }

        public static void StyleDangerButton(Button button)
        {
            StyleSecondaryButton(button);
            button.BackColor = DangerSurface;
            button.ForeColor = Color.FromArgb(255, 189, 189);
        }

        public static void StyleNavButton(Button button)
        {
            button.BackColor = Sidebar;
            button.ForeColor = MutedText;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Surface;
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.Padding = new Padding(14, 0, 0, 0);
            button.Cursor = Cursors.Hand;
        }

        public static void SetNavActive(Button button, bool active)
        {
            button.BackColor = active ? SurfaceAlt : Sidebar;
            button.ForeColor = active ? Text : MutedText;
        }

        public static void StyleComboBox(ComboBox combo)
        {
            combo.BackColor = Input;
            combo.ForeColor = Text;
            combo.FlatStyle = FlatStyle.Flat;
        }

        public static void StyleGrid(DataGridView grid)
        {
            grid.BackgroundColor = Surface;
            grid.BorderStyle = BorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(19, 25, 39);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = SubtleText;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(19, 25, 39);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            grid.ColumnHeadersHeight = 38;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.DefaultCellStyle.BackColor = Surface;
            grid.DefaultCellStyle.ForeColor = Text;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(48, 40, 94);
            grid.DefaultCellStyle.SelectionForeColor = Text;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9.25F);
            grid.DefaultCellStyle.Padding = new Padding(5, 2, 5, 2);
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(13, 18, 29);
            grid.GridColor = Border;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.RowTemplate.Height = 38;
        }
    }
}
