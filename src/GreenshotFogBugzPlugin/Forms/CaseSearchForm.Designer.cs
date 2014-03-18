/*
 * Created by SharpDevelop.
 * User: aarond
 * Date: 3/17/2012
 * Time: 11:56 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace GreenshotFogBugzPlugin.Forms
{
	partial class CaseSearchForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaseSearchForm));
            this.label1 = new System.Windows.Forms.Label();
            this.KeywordsTextBox = new System.Windows.Forms.TextBox();
            this.ResultsListBox = new System.Windows.Forms.ListBox();
            this.CaptionLabel = new System.Windows.Forms.Label();
            this.CaptionTextBox = new System.Windows.Forms.TextBox();
            this.SendToButton = new System.Windows.Forms.Button();
            this.QuitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search for case";
            // 
            // KeywordsTextBox
            // 
            this.KeywordsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.KeywordsTextBox.Location = new System.Drawing.Point(12, 27);
            this.KeywordsTextBox.Name = "KeywordsTextBox";
            this.KeywordsTextBox.Size = new System.Drawing.Size(552, 20);
            this.KeywordsTextBox.TabIndex = 1;
            this.KeywordsTextBox.TextChanged += new System.EventHandler(this.KeywordsTextBoxTextChanged);
            this.KeywordsTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeywordsTextBoxKeyDown);
            // 
            // ResultsListBox
            // 
            this.ResultsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsListBox.FormattingEnabled = true;
            this.ResultsListBox.Location = new System.Drawing.Point(12, 53);
            this.ResultsListBox.Name = "ResultsListBox";
            this.ResultsListBox.Size = new System.Drawing.Size(552, 186);
            this.ResultsListBox.TabIndex = 2;
            this.ResultsListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ResultsListBoxMouseDoubleClick);
            // 
            // CaptionLabel
            // 
            this.CaptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CaptionLabel.Location = new System.Drawing.Point(12, 249);
            this.CaptionLabel.Name = "CaptionLabel";
            this.CaptionLabel.Size = new System.Drawing.Size(100, 17);
            this.CaptionLabel.TabIndex = 3;
            this.CaptionLabel.Text = "Caption (optional)";
            // 
            // CaptionTextBox
            // 
            this.CaptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CaptionTextBox.Location = new System.Drawing.Point(12, 269);
            this.CaptionTextBox.Name = "CaptionTextBox";
            this.CaptionTextBox.Size = new System.Drawing.Size(552, 20);
            this.CaptionTextBox.TabIndex = 4;
            // 
            // SendToButton
            // 
            this.SendToButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SendToButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SendToButton.Location = new System.Drawing.Point(489, 295);
            this.SendToButton.Name = "SendToButton";
            this.SendToButton.Size = new System.Drawing.Size(75, 23);
            this.SendToButton.TabIndex = 5;
            this.SendToButton.Text = "&Send To";
            this.SendToButton.UseVisualStyleBackColor = true;
            this.SendToButton.Click += new System.EventHandler(this.SendToButtonClick);
            // 
            // QuitButton
            // 
            this.QuitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.QuitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.QuitButton.Location = new System.Drawing.Point(408, 295);
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.Size = new System.Drawing.Size(75, 23);
            this.QuitButton.TabIndex = 6;
            this.QuitButton.Text = "&Cancel";
            this.QuitButton.UseVisualStyleBackColor = true;
            // 
            // CaseSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 326);
            this.Controls.Add(this.QuitButton);
            this.Controls.Add(this.SendToButton);
            this.Controls.Add(this.CaptionTextBox);
            this.Controls.Add(this.CaptionLabel);
            this.Controls.Add(this.ResultsListBox);
            this.Controls.Add(this.KeywordsTextBox);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CaseSearchForm";
            this.Text = "FogBugz Case Search";
            this.Load += new System.EventHandler(this.CaseSearchFormLoad);
            this.Shown += new System.EventHandler(this.CaseSearchFormShown);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Button QuitButton;
		private System.Windows.Forms.Button SendToButton;
		private System.Windows.Forms.TextBox CaptionTextBox;
		private System.Windows.Forms.Label CaptionLabel;
		private System.Windows.Forms.ListBox ResultsListBox;
		private System.Windows.Forms.TextBox KeywordsTextBox;
		private System.Windows.Forms.Label label1;
	}
}
