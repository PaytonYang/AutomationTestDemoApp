namespace AutomationTestDemo.SubForms
{
    partial class EditStepForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.stopIfFail_No = new System.Windows.Forms.RadioButton();
            this.stopIfFail_Yes = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.actionDelayTextBox = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.stepNameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.finalStepRadioButton = new System.Windows.Forms.RadioButton();
            this.preStepRadioButton = new System.Windows.Forms.RadioButton();
            this.normalStepRadioButton = new System.Windows.Forms.RadioButton();
            this.confirmButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.actionDelayTextBox)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.stopIfFail_No);
            this.groupBox1.Controls.Add(this.stopIfFail_Yes);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.actionDelayTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.stepNameTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(315, 106);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Step Information";
            // 
            // stopIfFail_No
            // 
            this.stopIfFail_No.AutoSize = true;
            this.stopIfFail_No.Location = new System.Drawing.Point(175, 83);
            this.stopIfFail_No.Name = "stopIfFail_No";
            this.stopIfFail_No.Size = new System.Drawing.Size(41, 19);
            this.stopIfFail_No.TabIndex = 4;
            this.stopIfFail_No.Text = "No";
            this.stopIfFail_No.UseVisualStyleBackColor = true;
            // 
            // stopIfFail_Yes
            // 
            this.stopIfFail_Yes.AutoSize = true;
            this.stopIfFail_Yes.Checked = true;
            this.stopIfFail_Yes.Location = new System.Drawing.Point(111, 83);
            this.stopIfFail_Yes.Name = "stopIfFail_Yes";
            this.stopIfFail_Yes.Size = new System.Drawing.Size(42, 19);
            this.stopIfFail_Yes.TabIndex = 4;
            this.stopIfFail_Yes.TabStop = true;
            this.stopIfFail_Yes.Text = "Yes";
            this.stopIfFail_Yes.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Step Name:";
            // 
            // actionDelayTextBox
            // 
            this.actionDelayTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.actionDelayTextBox.Location = new System.Drawing.Point(175, 50);
            this.actionDelayTextBox.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.actionDelayTextBox.Name = "actionDelayTextBox";
            this.actionDelayTextBox.Size = new System.Drawing.Size(120, 23);
            this.actionDelayTextBox.TabIndex = 3;
            this.actionDelayTextBox.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Default Action Delay(ms):";
            // 
            // stepNameTextBox
            // 
            this.stepNameTextBox.Location = new System.Drawing.Point(100, 19);
            this.stepNameTextBox.Name = "stepNameTextBox";
            this.stepNameTextBox.Size = new System.Drawing.Size(195, 23);
            this.stepNameTextBox.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Stop If Fail:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.finalStepRadioButton);
            this.groupBox2.Controls.Add(this.preStepRadioButton);
            this.groupBox2.Controls.Add(this.normalStepRadioButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 136);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(315, 62);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Step Configure";
            // 
            // finalStepRadioButton
            // 
            this.finalStepRadioButton.AutoSize = true;
            this.finalStepRadioButton.Location = new System.Drawing.Point(211, 30);
            this.finalStepRadioButton.Name = "finalStepRadioButton";
            this.finalStepRadioButton.Size = new System.Drawing.Size(78, 19);
            this.finalStepRadioButton.TabIndex = 0;
            this.finalStepRadioButton.Text = "Final-Step";
            this.finalStepRadioButton.UseVisualStyleBackColor = true;
            // 
            // preStepRadioButton
            // 
            this.preStepRadioButton.AutoSize = true;
            this.preStepRadioButton.Location = new System.Drawing.Point(111, 30);
            this.preStepRadioButton.Name = "preStepRadioButton";
            this.preStepRadioButton.Size = new System.Drawing.Size(70, 19);
            this.preStepRadioButton.TabIndex = 0;
            this.preStepRadioButton.Text = "Pre-Step";
            this.preStepRadioButton.UseVisualStyleBackColor = true;
            // 
            // normalStepRadioButton
            // 
            this.normalStepRadioButton.AutoSize = true;
            this.normalStepRadioButton.Checked = true;
            this.normalStepRadioButton.Location = new System.Drawing.Point(20, 30);
            this.normalStepRadioButton.Name = "normalStepRadioButton";
            this.normalStepRadioButton.Size = new System.Drawing.Size(65, 19);
            this.normalStepRadioButton.TabIndex = 0;
            this.normalStepRadioButton.TabStop = true;
            this.normalStepRadioButton.Text = "Normal";
            this.normalStepRadioButton.UseVisualStyleBackColor = true;
            // 
            // confirmButton
            // 
            this.confirmButton.Location = new System.Drawing.Point(242, 204);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(85, 23);
            this.confirmButton.TabIndex = 2;
            this.confirmButton.Text = "Confirm";
            this.confirmButton.UseVisualStyleBackColor = true;
            this.confirmButton.Click += new System.EventHandler(this.confirmButton_Click);
            // 
            // EditStepForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 232);
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditStepForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Step";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.EditStepForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.actionDelayTextBox)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private RadioButton stopIfFail_No;
        private RadioButton stopIfFail_Yes;
        private Label label1;
        private NumericUpDown actionDelayTextBox;
        private Label label2;
        private TextBox stepNameTextBox;
        private Label label3;
        private GroupBox groupBox2;
        private RadioButton finalStepRadioButton;
        private RadioButton preStepRadioButton;
        private RadioButton normalStepRadioButton;
        private Button confirmButton;
    }
}