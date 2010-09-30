namespace json_merge
{
    partial class MergeVisualForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MergeVisualForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.aTextBox = new System.Windows.Forms.RichTextBox();
            this.bTextBox = new System.Windows.Forms.RichTextBox();
            this.cTextBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cTextBox);
            this.splitContainer1.Size = new System.Drawing.Size(840, 722);
            this.splitContainer1.SplitterDistance = 397;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.aTextBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.bTextBox);
            this.splitContainer2.Size = new System.Drawing.Size(840, 397);
            this.splitContainer2.SplitterDistance = 415;
            this.splitContainer2.TabIndex = 0;
            // 
            // aTextBox
            // 
            this.aTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aTextBox.Font = new System.Drawing.Font("Consolas", 9F);
            this.aTextBox.Location = new System.Drawing.Point(0, 0);
            this.aTextBox.Name = "aTextBox";
            this.aTextBox.Size = new System.Drawing.Size(415, 397);
            this.aTextBox.TabIndex = 0;
            this.aTextBox.Text = "";
            this.aTextBox.WordWrap = false;
            this.aTextBox.VScroll += new System.EventHandler(this.aTextBox_Scroll);
            this.aTextBox.HScroll += new System.EventHandler(this.aTextBox_Scroll);
            // 
            // bTextBox
            // 
            this.bTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bTextBox.Font = new System.Drawing.Font("Consolas", 9F);
            this.bTextBox.Location = new System.Drawing.Point(0, 0);
            this.bTextBox.Name = "bTextBox";
            this.bTextBox.Size = new System.Drawing.Size(421, 397);
            this.bTextBox.TabIndex = 1;
            this.bTextBox.Text = "";
            this.bTextBox.WordWrap = false;
            this.bTextBox.VScroll += new System.EventHandler(this.bTextBox_Scroll);
            this.bTextBox.HScroll += new System.EventHandler(this.bTextBox_Scroll);
            // 
            // cTextBox
            // 
            this.cTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cTextBox.Font = new System.Drawing.Font("Consolas", 9F);
            this.cTextBox.Location = new System.Drawing.Point(0, 0);
            this.cTextBox.Name = "cTextBox";
            this.cTextBox.Size = new System.Drawing.Size(840, 321);
            this.cTextBox.TabIndex = 2;
            this.cTextBox.Text = "";
            this.cTextBox.WordWrap = false;
            this.cTextBox.VScroll += new System.EventHandler(this.cTextBox_Scroll);
            this.cTextBox.HScroll += new System.EventHandler(this.cTextBox_Scroll);
            // 
            // MergeVisualForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 722);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MergeVisualForm";
            this.Text = "Json Merge";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.RichTextBox aTextBox;
        private System.Windows.Forms.RichTextBox bTextBox;
        private System.Windows.Forms.RichTextBox cTextBox;
    }
}