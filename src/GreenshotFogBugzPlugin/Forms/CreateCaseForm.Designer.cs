namespace GreenshotFogBugzPlugin.Forms
{
    partial class CreateCaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateCaseForm));
            this.lbCaseTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblProject = new System.Windows.Forms.Label();
            this.lbProject = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lbCaseTitle
            // 
            this.lbCaseTitle.AutoSize = true;
            this.lbCaseTitle.Location = new System.Drawing.Point(12, 9);
            this.lbCaseTitle.Name = "lbCaseTitle";
            this.lbCaseTitle.Size = new System.Drawing.Size(27, 13);
            this.lbCaseTitle.TabIndex = 0;
            this.lbCaseTitle.Text = "Title";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(12, 25);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(552, 20);
            this.txtTitle.TabIndex = 1;
            this.txtTitle.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lblProject
            // 
            this.lblProject.AutoSize = true;
            this.lblProject.Location = new System.Drawing.Point(12, 57);
            this.lblProject.Name = "lblProject";
            this.lblProject.Size = new System.Drawing.Size(40, 13);
            this.lblProject.TabIndex = 2;
            this.lblProject.Text = "Project";
            // 
            // lbProject
            // 
            this.lbProject.AllowDrop = true;
            this.lbProject.FormattingEnabled = true;
            this.lbProject.Location = new System.Drawing.Point(12, 74);
            this.lbProject.Name = "lbProject";
            this.lbProject.Size = new System.Drawing.Size(105, 17);
            this.lbProject.TabIndex = 3;
            // 
            // CreateCaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 369);
            this.Controls.Add(this.lbProject);
            this.Controls.Add(this.lblProject);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lbCaseTitle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CreateCaseForm";
            this.Text = "Fogbugz New Case";
            this.Load += new System.EventHandler(this.CreateCaseForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbCaseTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblProject;
        private System.Windows.Forms.ListBox lbProject;
    }
}