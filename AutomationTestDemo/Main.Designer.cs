namespace AutomationTestDemo
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel = new System.Windows.Forms.Panel();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.passTextBox = new System.Windows.Forms.TextBox();
            this.failTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.totalTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Location = new System.Drawing.Point(12, 16);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(868, 436);
            this.panel.TabIndex = 0;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(892, 175);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(100, 25);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(892, 211);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(100, 25);
            this.stopButton.TabIndex = 3;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // passTextBox
            // 
            this.passTextBox.Location = new System.Drawing.Point(892, 79);
            this.passTextBox.Name = "passTextBox";
            this.passTextBox.ReadOnly = true;
            this.passTextBox.Size = new System.Drawing.Size(100, 23);
            this.passTextBox.TabIndex = 4;
            // 
            // failTextBox
            // 
            this.failTextBox.Location = new System.Drawing.Point(892, 126);
            this.failTextBox.Name = "failTextBox";
            this.failTextBox.ReadOnly = true;
            this.failTextBox.Size = new System.Drawing.Size(100, 23);
            this.failTextBox.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(892, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Pass";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(892, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Fail";
            // 
            // totalTextBox
            // 
            this.totalTextBox.Location = new System.Drawing.Point(892, 36);
            this.totalTextBox.Name = "totalTextBox";
            this.totalTextBox.ReadOnly = true;
            this.totalTextBox.Size = new System.Drawing.Size(100, 23);
            this.totalTextBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(892, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Total";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(892, 320);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(100, 25);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Save Script";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(893, 289);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(100, 25);
            this.loadButton.TabIndex = 3;
            this.loadButton.Text = "Load Script";
            this.loadButton.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 466);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.failTextBox);
            this.Controls.Add(this.totalTextBox);
            this.Controls.Add(this.passTextBox);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.panel);
            this.Name = "Main";
            this.Text = "Automation Test Demo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel panel;
        private Button startButton;
        private Button stopButton;
        private TextBox passTextBox;
        private TextBox failTextBox;
        private Label label1;
        private Label label2;
        private TextBox totalTextBox;
        private Label label3;
        private Button saveButton;
        private Button loadButton;
    }
}