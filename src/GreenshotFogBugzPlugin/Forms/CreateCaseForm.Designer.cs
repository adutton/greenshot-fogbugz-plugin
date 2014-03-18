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
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbProject = new System.Windows.Forms.ComboBox();
            this.cbMilestone = new System.Windows.Forms.ComboBox();
            this.lblMilestone = new System.Windows.Forms.Label();
            this.lblArea = new System.Windows.Forms.Label();
            this.cbArea = new System.Windows.Forms.ComboBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.lblAssignedTo = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
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
            this.txtTitle.Size = new System.Drawing.Size(495, 20);
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
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(433, 334);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 4;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(352, 334);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cbProject
            // 
            this.cbProject.FormattingEnabled = true;
            this.cbProject.Location = new System.Drawing.Point(12, 73);
            this.cbProject.Name = "cbProject";
            this.cbProject.Size = new System.Drawing.Size(161, 21);
            this.cbProject.TabIndex = 6;
            // 
            // cbMilestone
            // 
            this.cbMilestone.FormattingEnabled = true;
            this.cbMilestone.Location = new System.Drawing.Point(346, 73);
            this.cbMilestone.Name = "cbMilestone";
            this.cbMilestone.Size = new System.Drawing.Size(161, 21);
            this.cbMilestone.TabIndex = 8;
            // 
            // lblMilestone
            // 
            this.lblMilestone.AutoSize = true;
            this.lblMilestone.Location = new System.Drawing.Point(346, 57);
            this.lblMilestone.Name = "lblMilestone";
            this.lblMilestone.Size = new System.Drawing.Size(52, 13);
            this.lblMilestone.TabIndex = 7;
            this.lblMilestone.Text = "Milestone";
            // 
            // lblArea
            // 
            this.lblArea.AutoSize = true;
            this.lblArea.Location = new System.Drawing.Point(179, 57);
            this.lblArea.Name = "lblArea";
            this.lblArea.Size = new System.Drawing.Size(29, 13);
            this.lblArea.TabIndex = 7;
            this.lblArea.Text = "Area";
            // 
            // cbArea
            // 
            this.cbArea.FormattingEnabled = true;
            this.cbArea.Location = new System.Drawing.Point(179, 73);
            this.cbArea.Name = "cbArea";
            this.cbArea.Size = new System.Drawing.Size(161, 21);
            this.cbArea.TabIndex = 8;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(12, 105);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(49, 13);
            this.lblCategory.TabIndex = 2;
            this.lblCategory.Text = "Category";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 121);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(161, 21);
            this.comboBox1.TabIndex = 6;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(346, 105);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "Status";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(346, 121);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(161, 21);
            this.comboBox2.TabIndex = 8;
            // 
            // lblAssignedTo
            // 
            this.lblAssignedTo.AutoSize = true;
            this.lblAssignedTo.Location = new System.Drawing.Point(179, 105);
            this.lblAssignedTo.Name = "lblAssignedTo";
            this.lblAssignedTo.Size = new System.Drawing.Size(66, 13);
            this.lblAssignedTo.TabIndex = 7;
            this.lblAssignedTo.Text = "Assigned To";
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(179, 121);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(161, 21);
            this.comboBox3.TabIndex = 8;
            // 
            // textBox1
            // 
            this.textBox1.AcceptsReturn = true;
            this.textBox1.AcceptsTab = true;
            this.textBox1.Location = new System.Drawing.Point(12, 155);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(495, 169);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // CreateCaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 369);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.cbArea);
            this.Controls.Add(this.lblAssignedTo);
            this.Controls.Add(this.lblArea);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.cbMilestone);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblMilestone);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.cbProject);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.lblProject);
            this.Controls.Add(this.textBox1);
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
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbProject;
        private System.Windows.Forms.ComboBox cbMilestone;
        private System.Windows.Forms.Label lblMilestone;
        private System.Windows.Forms.Label lblArea;
        private System.Windows.Forms.ComboBox cbArea;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label lblAssignedTo;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.TextBox textBox1;
    }
}