namespace WinFlash
{
    partial class MainMenu
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.deckBox = new System.Windows.Forms.ListBox();
            this.studyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // deckBox
            // 
            this.deckBox.FormattingEnabled = true;
            this.deckBox.ItemHeight = 15;
            this.deckBox.Location = new System.Drawing.Point(12, 12);
            this.deckBox.Name = "deckBox";
            this.deckBox.Size = new System.Drawing.Size(204, 259);
            this.deckBox.TabIndex = 0;
            this.deckBox.SelectedValueChanged += new System.EventHandler(this.deckBox_SelectedValueChanged);
            // 
            // studyButton
            // 
            this.studyButton.Enabled = false;
            this.studyButton.Location = new System.Drawing.Point(141, 277);
            this.studyButton.Name = "studyButton";
            this.studyButton.Size = new System.Drawing.Size(75, 23);
            this.studyButton.TabIndex = 1;
            this.studyButton.Text = "Study";
            this.studyButton.UseVisualStyleBackColor = true;
            this.studyButton.Click += new System.EventHandler(this.studyButton_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 311);
            this.Controls.Add(this.studyButton);
            this.Controls.Add(this.deckBox);
            this.Name = "MainMenu";
            this.Text = "WinFlash";
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ListBox deckBox;
        private Button studyButton;
    }
}