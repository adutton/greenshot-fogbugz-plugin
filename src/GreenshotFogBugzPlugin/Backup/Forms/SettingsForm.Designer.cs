namespace GreenshotFogBugzPlugin.Forms 
{
    partial class SettingsForm
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
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
        	this.InitialPanel = new System.Windows.Forms.Panel();
        	this.QuitButton = new System.Windows.Forms.Button();
        	this.LoginButton = new System.Windows.Forms.Button();
        	this.PasswordTextBox = new System.Windows.Forms.TextBox();
        	this.label4 = new System.Windows.Forms.Label();
        	this.EmailTextBox = new System.Windows.Forms.TextBox();
        	this.label3 = new System.Windows.Forms.Label();
        	this.ServerUrlTextBox = new System.Windows.Forms.TextBox();
        	this.label2 = new System.Windows.Forms.Label();
        	this.InitialLabel = new System.Windows.Forms.Label();
        	this.EstablishedPanel = new System.Windows.Forms.Panel();
        	this.label1 = new System.Windows.Forms.Label();
        	this.FogBugzEmailLabel = new System.Windows.Forms.Label();
        	this.FogBugzServerUrlLabel = new System.Windows.Forms.Label();
        	this.ClearButton = new System.Windows.Forms.Button();
        	this.OkButton = new System.Windows.Forms.Button();
        	this.EstablishedLabel = new System.Windows.Forms.Label();
        	this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
        	this.InitialPanel.SuspendLayout();
        	this.EstablishedPanel.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// InitialPanel
        	// 
        	this.InitialPanel.Controls.Add(this.QuitButton);
        	this.InitialPanel.Controls.Add(this.LoginButton);
        	this.InitialPanel.Controls.Add(this.PasswordTextBox);
        	this.InitialPanel.Controls.Add(this.label4);
        	this.InitialPanel.Controls.Add(this.EmailTextBox);
        	this.InitialPanel.Controls.Add(this.label3);
        	this.InitialPanel.Controls.Add(this.ServerUrlTextBox);
        	this.InitialPanel.Controls.Add(this.label2);
        	this.InitialPanel.Controls.Add(this.InitialLabel);
        	this.InitialPanel.Controls.Add(this.flowLayoutPanel1);
        	this.InitialPanel.Location = new System.Drawing.Point(1, 0);
        	this.InitialPanel.Name = "InitialPanel";
        	this.InitialPanel.Size = new System.Drawing.Size(275, 188);
        	this.InitialPanel.TabIndex = 9;
        	// 
        	// QuitButton
        	// 
        	this.QuitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        	this.QuitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        	this.QuitButton.Location = new System.Drawing.Point(183, 153);
        	this.QuitButton.Name = "QuitButton";
        	this.QuitButton.Size = new System.Drawing.Size(75, 23);
        	this.QuitButton.TabIndex = 17;
        	this.QuitButton.Text = "&Cancel";
        	this.QuitButton.UseVisualStyleBackColor = true;
        	// 
        	// LoginButton
        	// 
        	this.LoginButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        	this.LoginButton.Location = new System.Drawing.Point(102, 153);
        	this.LoginButton.Name = "LoginButton";
        	this.LoginButton.Size = new System.Drawing.Size(75, 23);
        	this.LoginButton.TabIndex = 16;
        	this.LoginButton.Text = "&Login";
        	this.LoginButton.UseVisualStyleBackColor = true;
        	this.LoginButton.Click += new System.EventHandler(this.LoginButtonClick);
        	// 
        	// PasswordTextBox
        	// 
        	this.PasswordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.PasswordTextBox.Location = new System.Drawing.Point(10, 127);
        	this.PasswordTextBox.Name = "PasswordTextBox";
        	this.PasswordTextBox.Size = new System.Drawing.Size(248, 20);
        	this.PasswordTextBox.TabIndex = 15;
        	this.PasswordTextBox.UseSystemPasswordChar = true;
        	// 
        	// label4
        	// 
        	this.label4.AutoSize = true;
        	this.label4.Location = new System.Drawing.Point(10, 110);
        	this.label4.Name = "label4";
        	this.label4.Size = new System.Drawing.Size(53, 13);
        	this.label4.TabIndex = 14;
        	this.label4.Text = "Password";
        	// 
        	// EmailTextBox
        	// 
        	this.EmailTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.EmailTextBox.Location = new System.Drawing.Point(11, 86);
        	this.EmailTextBox.Name = "EmailTextBox";
        	this.EmailTextBox.Size = new System.Drawing.Size(247, 20);
        	this.EmailTextBox.TabIndex = 13;
        	// 
        	// label3
        	// 
        	this.label3.AutoSize = true;
        	this.label3.Location = new System.Drawing.Point(11, 69);
        	this.label3.Name = "label3";
        	this.label3.Size = new System.Drawing.Size(73, 13);
        	this.label3.TabIndex = 12;
        	this.label3.Text = "Email Address";
        	// 
        	// ServerUrlTextBox
        	// 
        	this.ServerUrlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.ServerUrlTextBox.Location = new System.Drawing.Point(11, 46);
        	this.ServerUrlTextBox.Name = "ServerUrlTextBox";
        	this.ServerUrlTextBox.Size = new System.Drawing.Size(247, 20);
        	this.ServerUrlTextBox.TabIndex = 11;
        	// 
        	// label2
        	// 
        	this.label2.AutoSize = true;
        	this.label2.Location = new System.Drawing.Point(11, 29);
        	this.label2.Name = "label2";
        	this.label2.Size = new System.Drawing.Size(118, 13);
        	this.label2.TabIndex = 10;
        	this.label2.Text = "URL of FogBugz server";
        	// 
        	// InitialLabel
        	// 
        	this.InitialLabel.AutoSize = true;
        	this.InitialLabel.Location = new System.Drawing.Point(11, 9);
        	this.InitialLabel.Name = "InitialLabel";
        	this.InitialLabel.Size = new System.Drawing.Size(152, 13);
        	this.InitialLabel.TabIndex = 9;
        	this.InitialLabel.Text = "For the initial setup of FogBugz";
        	// 
        	// EstablishedPanel
        	// 
        	this.EstablishedPanel.Controls.Add(this.label1);
        	this.EstablishedPanel.Controls.Add(this.FogBugzEmailLabel);
        	this.EstablishedPanel.Controls.Add(this.FogBugzServerUrlLabel);
        	this.EstablishedPanel.Controls.Add(this.ClearButton);
        	this.EstablishedPanel.Controls.Add(this.OkButton);
        	this.EstablishedPanel.Controls.Add(this.EstablishedLabel);
        	this.EstablishedPanel.Location = new System.Drawing.Point(1, 194);
        	this.EstablishedPanel.Name = "EstablishedPanel";
        	this.EstablishedPanel.Size = new System.Drawing.Size(275, 171);
        	this.EstablishedPanel.TabIndex = 10;
        	// 
        	// label1
        	// 
        	this.label1.AutoSize = true;
        	this.label1.Location = new System.Drawing.Point(3, 52);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(77, 13);
        	this.label1.TabIndex = 5;
        	this.label1.Text = "FogBugz Email";
        	// 
        	// FogBugzEmailLabel
        	// 
        	this.FogBugzEmailLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.FogBugzEmailLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.FogBugzEmailLabel.Location = new System.Drawing.Point(3, 68);
        	this.FogBugzEmailLabel.Name = "FogBugzEmailLabel";
        	this.FogBugzEmailLabel.Size = new System.Drawing.Size(264, 23);
        	this.FogBugzEmailLabel.TabIndex = 4;
        	this.FogBugzEmailLabel.Text = "FogBugzEmailLabel";
        	// 
        	// FogBugzServerUrlLabel
        	// 
        	this.FogBugzServerUrlLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.FogBugzServerUrlLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        	this.FogBugzServerUrlLabel.Location = new System.Drawing.Point(3, 28);
        	this.FogBugzServerUrlLabel.Name = "FogBugzServerUrlLabel";
        	this.FogBugzServerUrlLabel.Size = new System.Drawing.Size(264, 24);
        	this.FogBugzServerUrlLabel.TabIndex = 3;
        	this.FogBugzServerUrlLabel.Text = "FogBugzServerUrlLabel";
        	// 
        	// ClearButton
        	// 
        	this.ClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.ClearButton.Location = new System.Drawing.Point(111, 136);
        	this.ClearButton.Name = "ClearButton";
        	this.ClearButton.Size = new System.Drawing.Size(75, 23);
        	this.ClearButton.TabIndex = 2;
        	this.ClearButton.Text = "&Clear";
        	this.ClearButton.UseVisualStyleBackColor = true;
        	this.ClearButton.Click += new System.EventHandler(this.ClearButtonClick);
        	// 
        	// OkButton
        	// 
        	this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
        	this.OkButton.Location = new System.Drawing.Point(192, 136);
        	this.OkButton.Name = "OkButton";
        	this.OkButton.Size = new System.Drawing.Size(75, 23);
        	this.OkButton.TabIndex = 1;
        	this.OkButton.Text = "&Ok";
        	this.OkButton.UseVisualStyleBackColor = true;
        	this.OkButton.Click += new System.EventHandler(this.OkButtonClick);
        	// 
        	// EstablishedLabel
        	// 
        	this.EstablishedLabel.AutoSize = true;
        	this.EstablishedLabel.Location = new System.Drawing.Point(3, 15);
        	this.EstablishedLabel.Name = "EstablishedLabel";
        	this.EstablishedLabel.Size = new System.Drawing.Size(83, 13);
        	this.EstablishedLabel.TabIndex = 0;
        	this.EstablishedLabel.Text = "FogBugz Server";
        	// 
        	// flowLayoutPanel1
        	// 
        	this.flowLayoutPanel1.Location = new System.Drawing.Point(-10, -11);
        	this.flowLayoutPanel1.Name = "flowLayoutPanel1";
        	this.flowLayoutPanel1.Size = new System.Drawing.Size(302, 383);
        	this.flowLayoutPanel1.TabIndex = 11;
        	// 
        	// SettingsForm
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(305, 384);
        	this.Controls.Add(this.InitialPanel);
        	this.Controls.Add(this.EstablishedPanel);
        	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        	this.Name = "SettingsForm";
        	this.Text = "FogBugz Settings";
        	this.InitialPanel.ResumeLayout(false);
        	this.InitialPanel.PerformLayout();
        	this.EstablishedPanel.ResumeLayout(false);
        	this.EstablishedPanel.PerformLayout();
        	this.ResumeLayout(false);
        }
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label FogBugzEmailLabel;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label FogBugzServerUrlLabel;

        #endregion

        private System.Windows.Forms.Panel InitialPanel;
        private System.Windows.Forms.Panel EstablishedPanel;
        private System.Windows.Forms.Label EstablishedLabel;
        private System.Windows.Forms.Button QuitButton;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox EmailTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ServerUrlTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label InitialLabel;
        private System.Windows.Forms.Button OkButton;

    }
}