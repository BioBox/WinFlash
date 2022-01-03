namespace WinFlash
{
    partial class CardSide
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.viewPort = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // buttonPanel
            // 
            this.buttonPanel.Location = new System.Drawing.Point(75, 323);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(324, 29);
            this.buttonPanel.TabIndex = 0;
            // 
            // viewPort
            // 
            this.viewPort.Location = new System.Drawing.Point(18, 14);
            this.viewPort.Name = "viewPort";
            this.viewPort.Size = new System.Drawing.Size(443, 303);
            this.viewPort.TabIndex = 1;
            this.viewPort.Paint += new System.Windows.Forms.PaintEventHandler(this.viewPort_Paint);
            // 
            // CardSide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.viewPort);
            this.Controls.Add(this.buttonPanel);
            this.Name = "CardSide";
            this.Size = new System.Drawing.Size(480, 365);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel buttonPanel;
        private Panel viewPort;
    }
}
