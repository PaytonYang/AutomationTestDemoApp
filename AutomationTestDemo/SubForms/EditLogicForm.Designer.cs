namespace AutomationTestDemo.SubForms
{
    partial class EditLogicForm
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
            this.methodComboBox = new System.Windows.Forms.ComboBox();
            this.classNameComboBox = new System.Windows.Forms.ComboBox();
            this.logicTypeComboBox = new System.Windows.Forms.ComboBox();
            this.parametersGridView = new System.Windows.Forms.DataGridView();
            this.parametersName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parametersValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parametersType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.methodLabel = new System.Windows.Forms.Label();
            this.classLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.confirmButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parametersGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.methodComboBox);
            this.groupBox1.Controls.Add(this.classNameComboBox);
            this.groupBox1.Controls.Add(this.logicTypeComboBox);
            this.groupBox1.Controls.Add(this.parametersGridView);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.methodLabel);
            this.groupBox1.Controls.Add(this.classLabel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(321, 260);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Logic Information";
            // 
            // methodComboBox
            // 
            this.methodComboBox.FormattingEnabled = true;
            this.methodComboBox.Location = new System.Drawing.Point(96, 111);
            this.methodComboBox.Name = "methodComboBox";
            this.methodComboBox.Size = new System.Drawing.Size(214, 23);
            this.methodComboBox.TabIndex = 2;
            // 
            // classNameComboBox
            // 
            this.classNameComboBox.FormattingEnabled = true;
            this.classNameComboBox.Location = new System.Drawing.Point(96, 73);
            this.classNameComboBox.Name = "classNameComboBox";
            this.classNameComboBox.Size = new System.Drawing.Size(214, 23);
            this.classNameComboBox.TabIndex = 2;
            // 
            // logicTypeComboBox
            // 
            this.logicTypeComboBox.FormattingEnabled = true;
            this.logicTypeComboBox.Items.AddRange(new object[] {
            "General Action",
            "Retry",
            "Loop",
            "If Condition"});
            this.logicTypeComboBox.Location = new System.Drawing.Point(96, 35);
            this.logicTypeComboBox.Name = "logicTypeComboBox";
            this.logicTypeComboBox.Size = new System.Drawing.Size(214, 23);
            this.logicTypeComboBox.TabIndex = 2;
            // 
            // parametersGridView
            // 
            this.parametersGridView.AllowUserToAddRows = false;
            this.parametersGridView.AllowUserToDeleteRows = false;
            this.parametersGridView.AllowUserToResizeRows = false;
            this.parametersGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.parametersGridView.BackgroundColor = System.Drawing.Color.White;
            this.parametersGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.parametersGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.parametersGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.parametersName,
            this.parametersValue,
            this.parametersType});
            this.parametersGridView.Location = new System.Drawing.Point(6, 169);
            this.parametersGridView.Name = "parametersGridView";
            this.parametersGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.parametersGridView.RowHeadersVisible = false;
            this.parametersGridView.RowHeadersWidth = 51;
            this.parametersGridView.RowTemplate.Height = 25;
            this.parametersGridView.Size = new System.Drawing.Size(300, 80);
            this.parametersGridView.TabIndex = 1;
            this.parametersGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.parametersGridView_CellClick);
            // 
            // parametersName
            // 
            this.parametersName.HeaderText = "Name";
            this.parametersName.MinimumWidth = 6;
            this.parametersName.Name = "parametersName";
            this.parametersName.ReadOnly = true;
            // 
            // parametersValue
            // 
            this.parametersValue.HeaderText = "Value";
            this.parametersValue.MinimumWidth = 6;
            this.parametersValue.Name = "parametersValue";
            // 
            // parametersType
            // 
            this.parametersType.HeaderText = "Type";
            this.parametersType.MinimumWidth = 6;
            this.parametersType.Name = "parametersType";
            this.parametersType.ReadOnly = true;
            this.parametersType.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Parameters:";
            // 
            // methodLabel
            // 
            this.methodLabel.AutoSize = true;
            this.methodLabel.Location = new System.Drawing.Point(6, 114);
            this.methodLabel.Name = "methodLabel";
            this.methodLabel.Size = new System.Drawing.Size(57, 15);
            this.methodLabel.TabIndex = 0;
            this.methodLabel.Text = "Function:";
            // 
            // classLabel
            // 
            this.classLabel.AutoSize = true;
            this.classLabel.Location = new System.Drawing.Point(7, 76);
            this.classLabel.Name = "classLabel";
            this.classLabel.Size = new System.Drawing.Size(45, 15);
            this.classLabel.TabIndex = 0;
            this.classLabel.Text = "Action:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Logic Type:";
            // 
            // confirmButton
            // 
            this.confirmButton.Location = new System.Drawing.Point(243, 278);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(90, 23);
            this.confirmButton.TabIndex = 1;
            this.confirmButton.Text = "Confirm";
            this.confirmButton.UseVisualStyleBackColor = true;
            this.confirmButton.Click += new System.EventHandler(this.confirmButton_Click);
            // 
            // EditLogicForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 309);
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditLogicForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Logic";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parametersGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private ComboBox methodComboBox;
        private ComboBox classNameComboBox;
        private ComboBox logicTypeComboBox;
        private DataGridView parametersGridView;
        private DataGridViewTextBoxColumn parametersName;
        private DataGridViewTextBoxColumn parametersValue;
        private DataGridViewTextBoxColumn parametersType;
        private Label label5;
        private Label methodLabel;
        private Label classLabel;
        private Label label1;
        private Button confirmButton;
    }
}