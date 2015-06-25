namespace MyClockIn
{
    partial class InputCredentials
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DomainTextBox = new System.Windows.Forms.TextBox();
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.CancelLoginButton = new System.Windows.Forms.Button();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.ClockingInRadioButton = new System.Windows.Forms.RadioButton();
            this.ClockingOutRadioButton = new System.Windows.Forms.RadioButton();
            this.SessionRadioButton = new System.Windows.Forms.RadioButton();
            this.SessionTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(255, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please login to begin your MyClockIn session.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Domain";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Username";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Password";
            // 
            // DomainTextBox
            // 
            this.DomainTextBox.Location = new System.Drawing.Point(76, 35);
            this.DomainTextBox.Name = "DomainTextBox";
            this.DomainTextBox.Size = new System.Drawing.Size(196, 20);
            this.DomainTextBox.TabIndex = 7;
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.Location = new System.Drawing.Point(76, 61);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(196, 20);
            this.UsernameTextBox.TabIndex = 0;
            // 
            // SubmitButton
            // 
            this.SubmitButton.Location = new System.Drawing.Point(116, 136);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(75, 23);
            this.SubmitButton.TabIndex = 5;
            this.SubmitButton.Text = "Submit";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // CancelLoginButton
            // 
            this.CancelLoginButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelLoginButton.Location = new System.Drawing.Point(197, 136);
            this.CancelLoginButton.Name = "CancelLoginButton";
            this.CancelLoginButton.Size = new System.Drawing.Size(75, 23);
            this.CancelLoginButton.TabIndex = 6;
            this.CancelLoginButton.Text = "Cancel";
            this.CancelLoginButton.UseVisualStyleBackColor = true;
            this.CancelLoginButton.Click += new System.EventHandler(this.CancelLoginButton_Click);
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(76, 87);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(196, 20);
            this.PasswordTextBox.TabIndex = 1;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            // 
            // ClockingInRadioButton
            // 
            this.ClockingInRadioButton.AutoSize = true;
            this.ClockingInRadioButton.Location = new System.Drawing.Point(108, 113);
            this.ClockingInRadioButton.Name = "ClockingInRadioButton";
            this.ClockingInRadioButton.Size = new System.Drawing.Size(75, 17);
            this.ClockingInRadioButton.TabIndex = 3;
            this.ClockingInRadioButton.Text = "ClockingIn";
            this.ClockingInRadioButton.UseVisualStyleBackColor = true;
            this.ClockingInRadioButton.CheckedChanged += new System.EventHandler(this.ClockingInRadioButton_CheckedChanged);
            // 
            // ClockingOutRadioButton
            // 
            this.ClockingOutRadioButton.AutoSize = true;
            this.ClockingOutRadioButton.Location = new System.Drawing.Point(189, 113);
            this.ClockingOutRadioButton.Name = "ClockingOutRadioButton";
            this.ClockingOutRadioButton.Size = new System.Drawing.Size(83, 17);
            this.ClockingOutRadioButton.TabIndex = 4;
            this.ClockingOutRadioButton.Text = "ClockingOut";
            this.ClockingOutRadioButton.UseVisualStyleBackColor = true;
            this.ClockingOutRadioButton.CheckedChanged += new System.EventHandler(this.ClockingOutRadioButton_CheckedChanged);
            // 
            // SessionRadioButton
            // 
            this.SessionRadioButton.AutoSize = true;
            this.SessionRadioButton.Checked = true;
            this.SessionRadioButton.Location = new System.Drawing.Point(40, 113);
            this.SessionRadioButton.Name = "SessionRadioButton";
            this.SessionRadioButton.Size = new System.Drawing.Size(62, 17);
            this.SessionRadioButton.TabIndex = 2;
            this.SessionRadioButton.TabStop = true;
            this.SessionRadioButton.Text = "Session";
            this.SessionRadioButton.UseVisualStyleBackColor = true;
            this.SessionRadioButton.CheckedChanged += new System.EventHandler(this.SessionRadioButton_CheckedChanged);
            // 
            // SessionTimer
            // 
            this.SessionTimer.Interval = 1000;
            this.SessionTimer.Tick += new System.EventHandler(this.SessionTimer_Tick);
            // 
            // InputCredentials
            // 
            this.AcceptButton = this.SubmitButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelLoginButton;
            this.ClientSize = new System.Drawing.Size(284, 171);
            this.ControlBox = false;
            this.Controls.Add(this.SessionRadioButton);
            this.Controls.Add(this.ClockingOutRadioButton);
            this.Controls.Add(this.ClockingInRadioButton);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.CancelLoginButton);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.UsernameTextBox);
            this.Controls.Add(this.DomainTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputCredentials";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MyClockIn Login";
            this.Load += new System.EventHandler(this.InputCredentials_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox DomainTextBox;
        private System.Windows.Forms.TextBox UsernameTextBox;
        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.Button CancelLoginButton;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.RadioButton ClockingInRadioButton;
        private System.Windows.Forms.RadioButton ClockingOutRadioButton;
        private System.Windows.Forms.RadioButton SessionRadioButton;
        private System.Windows.Forms.Timer SessionTimer;
    }
}