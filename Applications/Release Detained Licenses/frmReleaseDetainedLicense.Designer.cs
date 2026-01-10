namespace DVLD_Project_Mine_
{
    partial class frmReleaseDetainedLicense
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
            this.label1 = new System.Windows.Forms.Label();
            this.ctrlLIcenseInfoWithFilter1 = new DVLD_Project_Mine_.ctrlLIcenseInfoWithFilter();
            this.btnClose = new System.Windows.Forms.Button();
            this.ctrlReleaseDetainedInfo1 = new DVLD_Project_Mine_.ctrlReleaseDetainedInfo();
            this.btnRelease = new System.Windows.Forms.Button();
            this.llblShowPersonLicensesHistory = new System.Windows.Forms.LinkLabel();
            this.llblShowLicenseDetails = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Mongolian Baiti", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(145, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(322, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Release Detained License";
            // 
            // ctrlLIcenseInfoWithFilter1
            // 
            this.ctrlLIcenseInfoWithFilter1.Location = new System.Drawing.Point(12, 41);
            this.ctrlLIcenseInfoWithFilter1.Name = "ctrlLIcenseInfoWithFilter1";
            this.ctrlLIcenseInfoWithFilter1.Size = new System.Drawing.Size(692, 384);
            this.ctrlLIcenseInfoWithFilter1.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Mongolian Baiti", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(408, 619);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(145, 37);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ctrlReleaseDetainedInfo1
            // 
            this.ctrlReleaseDetainedInfo1.Location = new System.Drawing.Point(12, 431);
            this.ctrlReleaseDetainedInfo1.Name = "ctrlReleaseDetainedInfo1";
            this.ctrlReleaseDetainedInfo1.Size = new System.Drawing.Size(692, 182);
            this.ctrlReleaseDetainedInfo1.TabIndex = 3;
            // 
            // btnRelease
            // 
            this.btnRelease.Font = new System.Drawing.Font("Mongolian Baiti", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRelease.Location = new System.Drawing.Point(559, 619);
            this.btnRelease.Name = "btnRelease";
            this.btnRelease.Size = new System.Drawing.Size(145, 37);
            this.btnRelease.TabIndex = 4;
            this.btnRelease.Text = "Release";
            this.btnRelease.UseVisualStyleBackColor = true;
            this.btnRelease.Click += new System.EventHandler(this.btnRelease_Click);
            // 
            // llblShowPersonLicensesHistory
            // 
            this.llblShowPersonLicensesHistory.AutoSize = true;
            this.llblShowPersonLicensesHistory.Location = new System.Drawing.Point(0, 631);
            this.llblShowPersonLicensesHistory.Name = "llblShowPersonLicensesHistory";
            this.llblShowPersonLicensesHistory.Size = new System.Drawing.Size(150, 13);
            this.llblShowPersonLicensesHistory.TabIndex = 5;
            this.llblShowPersonLicensesHistory.TabStop = true;
            this.llblShowPersonLicensesHistory.Text = "Show Person Licenses History";
            this.llblShowPersonLicensesHistory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblShowPersonLicensesHistory_LinkClicked);
            // 
            // llblShowLicenseDetails
            // 
            this.llblShowLicenseDetails.AutoSize = true;
            this.llblShowLicenseDetails.Enabled = false;
            this.llblShowLicenseDetails.Location = new System.Drawing.Point(168, 631);
            this.llblShowLicenseDetails.Name = "llblShowLicenseDetails";
            this.llblShowLicenseDetails.Size = new System.Drawing.Size(109, 13);
            this.llblShowLicenseDetails.TabIndex = 6;
            this.llblShowLicenseDetails.TabStop = true;
            this.llblShowLicenseDetails.Text = "Show License Details";
            this.llblShowLicenseDetails.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblShowLicenseDetails_LinkClicked);
            // 
            // frmReleaseDetainedLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 668);
            this.Controls.Add(this.llblShowLicenseDetails);
            this.Controls.Add(this.llblShowPersonLicensesHistory);
            this.Controls.Add(this.btnRelease);
            this.Controls.Add(this.ctrlReleaseDetainedInfo1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.ctrlLIcenseInfoWithFilter1);
            this.Controls.Add(this.label1);
            this.Name = "frmReleaseDetainedLicense";
            this.Text = "frmReleaseDetainedLicense";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private ctrlLIcenseInfoWithFilter ctrlLIcenseInfoWithFilter1;
        private System.Windows.Forms.Button btnClose;
        private ctrlReleaseDetainedInfo ctrlReleaseDetainedInfo1;
        private System.Windows.Forms.Button btnRelease;
        private System.Windows.Forms.LinkLabel llblShowPersonLicensesHistory;
        private System.Windows.Forms.LinkLabel llblShowLicenseDetails;
    }
}