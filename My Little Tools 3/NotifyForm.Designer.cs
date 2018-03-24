namespace MyLittleTools3
{
    partial class NotifyForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NotifyText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // NotifyText
            // 
            this.NotifyText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NotifyText.Location = new System.Drawing.Point(0, 0);
            this.NotifyText.Multiline = true;
            this.NotifyText.Name = "NotifyText";
            this.NotifyText.Size = new System.Drawing.Size(284, 161);
            this.NotifyText.TabIndex = 0;
            // 
            // NotifyForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 161);
            this.Controls.Add(this.NotifyText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NotifyForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Notification";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox NotifyText;
    }
}