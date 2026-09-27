namespace DVLD_Project.Applications.Local_Driving_License
{
    partial class ctrlDrivingLicenseApplicationInfo
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
            this.label2 = new System.Windows.Forms.Label();
            this.lblLDLAppID = new System.Windows.Forms.Label();
            this.lblLicenseClassName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblPassedTests = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnShowLicenseInfo = new System.Windows.Forms.Button();
            this.gbDrivingLicenseApplicationInfo = new System.Windows.Forms.GroupBox();
            this.ctrlApplicationBasicInfo1 = new DVLD_Project.Applications.Controls.ctrlApplicationBasicInfo();
            this.gbDrivingLicenseApplicationInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(52, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(177, 40);
            this.label2.TabIndex = 0;
            this.label2.Text = "Local Driving License \r\nApplication ID : ";
            // 
            // lblLDLAppID
            // 
            this.lblLDLAppID.AutoSize = true;
            this.lblLDLAppID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLDLAppID.ForeColor = System.Drawing.Color.Green;
            this.lblLDLAppID.Location = new System.Drawing.Point(249, 78);
            this.lblLDLAppID.Name = "lblLDLAppID";
            this.lblLDLAppID.Size = new System.Drawing.Size(45, 20);
            this.lblLDLAppID.TabIndex = 1;
            this.lblLDLAppID.Text = "0000";
            // 
            // lblLicenseClassName
            // 
            this.lblLicenseClassName.AutoSize = true;
            this.lblLicenseClassName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLicenseClassName.Location = new System.Drawing.Point(689, 78);
            this.lblLicenseClassName.Name = "lblLicenseClassName";
            this.lblLicenseClassName.Size = new System.Drawing.Size(101, 20);
            this.lblLicenseClassName.TabIndex = 2;
            this.lblLicenseClassName.Text = "Class Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(492, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(173, 20);
            this.label5.TabIndex = 3;
            this.label5.Text = "Applied For License : ";
            // 
            // lblPassedTests
            // 
            this.lblPassedTests.AutoSize = true;
            this.lblPassedTests.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassedTests.Location = new System.Drawing.Point(249, 133);
            this.lblPassedTests.Name = "lblPassedTests";
            this.lblPassedTests.Size = new System.Drawing.Size(32, 20);
            this.lblPassedTests.TabIndex = 4;
            this.lblPassedTests.Text = "0/3";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(52, 133);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(127, 20);
            this.label7.TabIndex = 5;
            this.label7.Text = "Passed Tests : ";
            // 
            // btnShowLicenseInfo
            // 
            this.btnShowLicenseInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowLicenseInfo.Location = new System.Drawing.Point(486, 121);
            this.btnShowLicenseInfo.Name = "btnShowLicenseInfo";
            this.btnShowLicenseInfo.Size = new System.Drawing.Size(179, 45);
            this.btnShowLicenseInfo.TabIndex = 39;
            this.btnShowLicenseInfo.Text = "Show License Info";
            this.btnShowLicenseInfo.UseVisualStyleBackColor = true;
            // 
            // gbDrivingLicenseApplicationInfo
            // 
            this.gbDrivingLicenseApplicationInfo.Controls.Add(this.btnShowLicenseInfo);
            this.gbDrivingLicenseApplicationInfo.Controls.Add(this.label7);
            this.gbDrivingLicenseApplicationInfo.Controls.Add(this.lblPassedTests);
            this.gbDrivingLicenseApplicationInfo.Controls.Add(this.label5);
            this.gbDrivingLicenseApplicationInfo.Controls.Add(this.lblLicenseClassName);
            this.gbDrivingLicenseApplicationInfo.Controls.Add(this.lblLDLAppID);
            this.gbDrivingLicenseApplicationInfo.Controls.Add(this.label2);
            this.gbDrivingLicenseApplicationInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbDrivingLicenseApplicationInfo.Location = new System.Drawing.Point(0, 0);
            this.gbDrivingLicenseApplicationInfo.Name = "gbDrivingLicenseApplicationInfo";
            this.gbDrivingLicenseApplicationInfo.Size = new System.Drawing.Size(1000, 181);
            this.gbDrivingLicenseApplicationInfo.TabIndex = 0;
            this.gbDrivingLicenseApplicationInfo.TabStop = false;
            this.gbDrivingLicenseApplicationInfo.Text = "Driving License Application Info";
            // 
            // ctrlApplicationBasicInfo1
            // 
            this.ctrlApplicationBasicInfo1.Location = new System.Drawing.Point(0, 187);
            this.ctrlApplicationBasicInfo1.Name = "ctrlApplicationBasicInfo1";
            this.ctrlApplicationBasicInfo1.Size = new System.Drawing.Size(1000, 229);
            this.ctrlApplicationBasicInfo1.TabIndex = 1;
            // 
            // ctrlDrivingLicenseApplicationInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctrlApplicationBasicInfo1);
            this.Controls.Add(this.gbDrivingLicenseApplicationInfo);
            this.Name = "ctrlDrivingLicenseApplicationInfo";
            this.Size = new System.Drawing.Size(1002, 419);
            this.gbDrivingLicenseApplicationInfo.ResumeLayout(false);
            this.gbDrivingLicenseApplicationInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblLDLAppID;
        private System.Windows.Forms.Label lblLicenseClassName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblPassedTests;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnShowLicenseInfo;
        private System.Windows.Forms.GroupBox gbDrivingLicenseApplicationInfo;
        private Controls.ctrlApplicationBasicInfo ctrlApplicationBasicInfo1;
    }
}
