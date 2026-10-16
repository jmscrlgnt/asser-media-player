using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AsserPlayer.Forms
{
    internal static class NativeTheme
    {
        private const int DwmwaUseImmersiveDarkMode = 20;
        private const int DwmwaUseImmersiveDarkModeOld = 19;
        private const int EmSetCueBanner = 0x1501;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        public static void ApplyDarkTitleBar(Form form)
        {
            if (form == null || !form.IsHandleCreated) return;
            try
            {
                var enabled = 1;
                var result = DwmSetWindowAttribute(form.Handle, DwmwaUseImmersiveDarkMode, ref enabled, sizeof(int));
                if (result != 0)
                    DwmSetWindowAttribute(form.Handle, DwmwaUseImmersiveDarkModeOld, ref enabled, sizeof(int));
            }
            catch
            {
            }
        }

        public static void SetCueBanner(TextBox textBox, string text)
        {
            if (textBox == null) return;

            Action apply = delegate
            {
                if (!textBox.IsHandleCreated) return;
                try { SendMessage(textBox.Handle, EmSetCueBanner, new IntPtr(1), text ?? string.Empty); }
                catch { }
            };

            if (textBox.IsHandleCreated) apply();
            textBox.HandleCreated += delegate { apply(); };
        }

        public static void ApplyDarkScrollableControls(Control root)
        {
            if (root == null) return;
            ApplyToControl(root);
            foreach (Control child in root.Controls)
                ApplyDarkScrollableControls(child);
        }

        private static void ApplyToControl(Control control)
        {
            if (control == null || !control.IsHandleCreated) return;
            if (!(control is ListBox) && !(control is ListView) && !(control is DataGridView) &&
                !(control is TextBox) && !(control is ComboBox) && !(control is ScrollBar)) return;

            try
            {
                SetWindowTheme(control.Handle, "DarkMode_Explorer", null);
            }
            catch
            {
            }
        }
    }
}
