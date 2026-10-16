using System;
using System.Drawing;
using System.Windows.Forms;

namespace AsserPlayer.Forms
{
    internal sealed class TextPromptForm : Form
    {
        private readonly TextBox _textBox;

        private TextPromptForm(string title, string labelText, string initialValue)
        {
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(430, 170);
            BackColor = UiTheme.Surface;
            ForeColor = UiTheme.Text;
            Font = new Font("Segoe UI", 9.5F);

            var label = new Label
            {
                Text = labelText,
                AutoSize = true,
                Left = 24,
                Top = 24,
                ForeColor = UiTheme.MutedText
            };

            _textBox = new TextBox
            {
                Left = 24,
                Top = 52,
                Width = 380,
                Text = initialValue ?? string.Empty,
                BackColor = UiTheme.Background,
                ForeColor = UiTheme.Text,
                BorderStyle = BorderStyle.FixedSingle
            };

            var okButton = new Button
            {
                Text = "Create",
                Left = 224,
                Top = 105,
                Width = 86,
                Height = 34,
                DialogResult = DialogResult.OK
            };
            UiTheme.StylePrimaryButton(okButton);

            var cancelButton = new Button
            {
                Text = "Cancel",
                Left = 318,
                Top = 105,
                Width = 86,
                Height = 34,
                DialogResult = DialogResult.Cancel
            };
            UiTheme.StyleSecondaryButton(cancelButton);

            Controls.Add(label);
            Controls.Add(_textBox);
            Controls.Add(okButton);
            Controls.Add(cancelButton);
            AcceptButton = okButton;
            CancelButton = cancelButton;
        }

        public static string Show(IWin32Window owner, string title, string labelText, string initialValue)
        {
            using (var form = new TextPromptForm(title, labelText, initialValue))
            {
                if (form.ShowDialog(owner) != DialogResult.OK) return null;
                return form._textBox.Text.Trim();
            }
        }
    }
}
