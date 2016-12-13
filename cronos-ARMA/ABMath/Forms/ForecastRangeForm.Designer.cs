namespace ABMath.Forms
{
    partial class ForecastRangeForm
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
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.skipWeekendsCheckBox = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.daysUpDown = new System.Windows.Forms.NumericUpDown();
            this.hoursUpDown = new System.Windows.Forms.NumericUpDown();
            this.minutesUpDown = new System.Windows.Forms.NumericUpDown();
            this.secondsUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.daysUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hoursUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minutesUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondsUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            //this.dateTimePicker1.Location = new System.Drawing.Point(152, 36);
            this.dateTimePicker1.Name = "dateTimePicker1";
           // this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // dateTimePicker2
            // 
            //this.dateTimePicker2.Location = new System.Drawing.Point(152, 69);
            this.dateTimePicker2.Name = "dateTimePicker2";
            //this.dateTimePicker2.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            //this.label1.Location = new System.Drawing.Point(43, 36);
            this.label1.Name = "label1";
            //this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Start Time";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            //this.label2.Location = new System.Drawing.Point(43, 69);
            this.label2.Name = "label2";
            //this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "End Time";
            // 
            // skipWeekendsCheckBox
            // 
            this.skipWeekendsCheckBox.AutoSize = true;
            //this.skipWeekendsCheckBox.Location = new System.Drawing.Point(174, 123);
            this.skipWeekendsCheckBox.Name = "skipWeekendsCheckBox";
            //this.skipWeekendsCheckBox.Size = new System.Drawing.Size(102, 17);
            this.skipWeekendsCheckBox.TabIndex = 5;
            this.skipWeekendsCheckBox.Text = "Skip Weekends";
            this.skipWeekendsCheckBox.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            //this.okButton.Location = new System.Drawing.Point(277, 299);
            this.okButton.Name = "okButton";
            //this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            //this.cancelButton.Location = new System.Drawing.Point(46, 299);
            this.cancelButton.Name = "cancelButton";
            //this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // daysUpDown
            // 
            //this.daysUpDown.Location = new System.Drawing.Point(198, 19);
            this.daysUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.daysUpDown.Name = "daysUpDown";
           // this.daysUpDown.Size = new System.Drawing.Size(78, 20);
            this.daysUpDown.TabIndex = 8;
            // 
            // hoursUpDown
            // 
            //this.hoursUpDown.Location = new System.Drawing.Point(198, 45);
            this.hoursUpDown.Name = "hoursUpDown";
            //this.hoursUpDown.Size = new System.Drawing.Size(78, 20);
            this.hoursUpDown.TabIndex = 9;
            // 
            // minutesUpDown
            // 
            //this.minutesUpDown.Location = new System.Drawing.Point(198, 71);
            this.minutesUpDown.Name = "minutesUpDown";
            //this.minutesUpDown.Size = new System.Drawing.Size(78, 20);
            this.minutesUpDown.TabIndex = 10;
            // 
            // secondsUpDown
            // 
            //this.secondsUpDown.Location = new System.Drawing.Point(198, 97);
            this.secondsUpDown.Name = "secondsUpDown";
            //this.secondsUpDown.Size = new System.Drawing.Size(78, 20);
            this.secondsUpDown.TabIndex = 11;
            // 
            // label4
            // 
            //this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            //this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 3;
            // 
            // label5
            // 
           // this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            //this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 2;
            // 
            // label6
            // 
            //this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            //this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 1;
            // 
            // label7
            // 
            //this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            //this.label7.Size = new System.Drawing.Size(100, 23);
            this.label7.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.skipWeekendsCheckBox);
            this.groupBox1.Controls.Add(this.daysUpDown);
            this.groupBox1.Controls.Add(this.hoursUpDown);
            this.groupBox1.Controls.Add(this.minutesUpDown);
            this.groupBox1.Controls.Add(this.secondsUpDown);
            //this.groupBox1.Location = new System.Drawing.Point(46, 109);
            this.groupBox1.Name = "groupBox1";
            //this.groupBox1.Size = new System.Drawing.Size(306, 156);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sampling";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            //this.label10.Location = new System.Drawing.Point(97, 99);
            this.label10.Name = "label10";
            //this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Seconds";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            //this.label9.Location = new System.Drawing.Point(97, 21);
            this.label9.Name = "label9";
            //this.label9.Size = new System.Drawing.Size(31, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Days";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            //this.label8.Location = new System.Drawing.Point(97, 73);
            this.label8.Name = "label8";
            //this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Minutes";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            //this.label3.Location = new System.Drawing.Point(97, 47);
            this.label3.Name = "label3";
            //this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Hours";
            // 
            // ForecastRangeForm
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.ClientSize = new System.Drawing.Size(393, 342);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.groupBox1);
            this.Name = "ForecastRangeForm";
            this.Text = "Forecast Horizon";
            ((System.ComponentModel.ISupportInitialize)(this.daysUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hoursUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minutesUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.secondsUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox skipWeekendsCheckBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.NumericUpDown daysUpDown;
        private System.Windows.Forms.NumericUpDown hoursUpDown;
        private System.Windows.Forms.NumericUpDown minutesUpDown;
        private System.Windows.Forms.NumericUpDown secondsUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
    }
}