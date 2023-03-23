namespace ScriptLibrary.Controls
{
    partial class ScriptTree
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
            this.components = new System.ComponentModel.Container();
            this.treeView = new Aga.Controls.Tree.TreeViewAdv();
            this.scriptColumn = new Aga.Controls.Tree.TreeColumn();
            this.resultColumn = new Aga.Controls.Tree.TreeColumn();
            this.addMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addMenuStep = new System.Windows.Forms.ToolStripMenuItem();
            this.addMenuAction = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptNodeCheckBox = new Aga.Controls.Tree.NodeControls.NodeCheckBox();
            this.scriptNodeTextBox = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.resultNodeTextBox = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.addMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.AllowDrop = true;
            this.treeView.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeView.Columns.Add(this.scriptColumn);
            this.treeView.Columns.Add(this.resultColumn);
            this.treeView.ContextMenuStrip = this.addMenuStrip;
            this.treeView.DefaultToolTipProvider = null;
            this.treeView.DisplayDraggingNodes = true;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeView.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView.GridLineStyle = ((Aga.Controls.Tree.GridLineStyle)((Aga.Controls.Tree.GridLineStyle.Horizontal | Aga.Controls.Tree.GridLineStyle.Vertical)));
            this.treeView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.treeView.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeView.LoadOnDemand = true;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Margin = new System.Windows.Forms.Padding(0);
            this.treeView.Model = null;
            this.treeView.Name = "treeView";
            this.treeView.NodeControls.Add(this.scriptNodeCheckBox);
            this.treeView.NodeControls.Add(this.scriptNodeTextBox);
            this.treeView.NodeControls.Add(this.resultNodeTextBox);
            this.treeView.SelectedNode = null;
            this.treeView.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.MultiSameParent;
            this.treeView.Size = new System.Drawing.Size(900, 450);
            this.treeView.TabIndex = 0;
            this.treeView.Text = "treeView";
            this.treeView.UseColumns = true;
            // 
            // scriptColumn
            // 
            this.scriptColumn.Header = "Script";
            this.scriptColumn.SortOrder = System.Windows.Forms.SortOrder.None;
            this.scriptColumn.TooltipText = null;
            this.scriptColumn.Width = 650;
            // 
            // resultColumn
            // 
            this.resultColumn.Header = "Result";
            this.resultColumn.SortOrder = System.Windows.Forms.SortOrder.None;
            this.resultColumn.TooltipText = null;
            this.resultColumn.Width = 250;
            // 
            // addMenuStrip
            // 
            this.addMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.addMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMenuStep,
            this.addMenuAction});
            this.addMenuStrip.Name = "addMenuStrip";
            this.addMenuStrip.Size = new System.Drawing.Size(138, 48);
            // 
            // addMenuStep
            // 
            this.addMenuStep.Name = "addMenuStep";
            this.addMenuStep.Size = new System.Drawing.Size(137, 22);
            this.addMenuStep.Text = "Add Step";
            this.addMenuStep.Click += new System.EventHandler(this.addMenuStep_Click);
            // 
            // addMenuAction
            // 
            this.addMenuAction.Name = "addMenuAction";
            this.addMenuAction.Size = new System.Drawing.Size(137, 22);
            this.addMenuAction.Text = "Add Action";
            this.addMenuAction.Click += new System.EventHandler(this.addMenuAction_Click);
            // 
            // scriptNodeCheckBox
            // 
            this.scriptNodeCheckBox.DataPropertyName = "CheckState";
            this.scriptNodeCheckBox.EditEnabled = true;
            this.scriptNodeCheckBox.LeftMargin = 0;
            this.scriptNodeCheckBox.ParentColumn = this.scriptColumn;
            // 
            // scriptNodeTextBox
            // 
            this.scriptNodeTextBox.DataPropertyName = "Text";
            this.scriptNodeTextBox.IncrementalSearchEnabled = true;
            this.scriptNodeTextBox.LeftMargin = 3;
            this.scriptNodeTextBox.ParentColumn = this.scriptColumn;
            // 
            // resultNodeTextBox
            // 
            this.resultNodeTextBox.DataPropertyName = "Result";
            this.resultNodeTextBox.IncrementalSearchEnabled = true;
            this.resultNodeTextBox.LeftMargin = 3;
            this.resultNodeTextBox.ParentColumn = this.resultColumn;
            // 
            // ScriptTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ScriptTree";
            this.Size = new System.Drawing.Size(900, 450);
            this.addMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Aga.Controls.Tree.TreeViewAdv treeView;
        private Aga.Controls.Tree.NodeControls.NodeTextBox scriptNodeTextBox;
        private Aga.Controls.Tree.NodeControls.NodeCheckBox scriptNodeCheckBox;
        private System.Windows.Forms.ContextMenuStrip addMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addMenuStep;
        private System.Windows.Forms.ToolStripMenuItem addMenuAction;
        private Aga.Controls.Tree.TreeColumn scriptColumn;
        private Aga.Controls.Tree.TreeColumn resultColumn;
        private Aga.Controls.Tree.NodeControls.NodeTextBox resultNodeTextBox;
    }
}
