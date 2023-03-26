using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Forms;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

namespace ScriptLibrary.Controls
{
    public partial class ScriptTree : UserControl
    {
        private TreeModel _model;
        private Mode _mode;

        public enum Mode
        {
            Editor = 0,
            Viewer
        }

        public Mode EditMode
        {
            set
            {
                _mode = value;
                switch (value)
                {
                    case Mode.Editor:
                        this.treeView.ItemDrag += new ItemDragEventHandler(this.treeView_ItemDrag);
                        this.treeView.NodeMouseDoubleClick += new EventHandler<TreeNodeAdvMouseEventArgs>(this.treeView_NodeMouseDoubleClick);
                        this.treeView.SelectionChanged += new EventHandler(this.treeView_SelectionChanged);
                        this.treeView.DragDrop += new DragEventHandler(this.treeView_DragDrop);
                        this.treeView.DragOver += new DragEventHandler(this.treeView_DragOver);
                        this.treeView.KeyDown += new KeyEventHandler(this.treeView_KeyDown);
                        this.scriptNodeCheckBox.EditEnabled = true;
                        break;
                    case Mode.Viewer:
                        this.treeView.ItemDrag -= new ItemDragEventHandler(this.treeView_ItemDrag);
                        this.treeView.NodeMouseDoubleClick -= new EventHandler<TreeNodeAdvMouseEventArgs>(this.treeView_NodeMouseDoubleClick);
                        this.treeView.SelectionChanged -= new EventHandler(this.treeView_SelectionChanged);
                        this.treeView.DragDrop -= new DragEventHandler(this.treeView_DragDrop);
                        this.treeView.DragOver -= new DragEventHandler(this.treeView_DragOver);
                        this.treeView.KeyDown -= new KeyEventHandler(this.treeView_KeyDown);
                        this.scriptNodeCheckBox.EditEnabled = false;
                        break;
                }
            }
            get { return _mode; }
        }

        public int SelectedIndex
        {
            set
            {
                if (value != -1)
                {
                    List<TreeNodeAdv> all_nodes = this.treeView.AllNodes.ToList();
                    this.treeView.SelectedNode = all_nodes[value];
                }
            }
            get
            {
                List<TreeNodeAdv> all_nodes = this.treeView.AllNodes.ToList();
                return (this.treeView.SelectedNode == null) ? -1 : all_nodes.IndexOf(this.treeView.SelectedNode);
            }
        }

        public Func<EditStepFormResult, EditStepFormResult> EditStepForm { set; get; } = null;
        public Func<EditLogicFormResult, EditLogicFormResult> EditLogicForm { set; get; } = null;
        public Action<LogicInfo> SelectedItemCallBack { set; get; } = null;
        public Action ScriptChangedCallBack { set; get; } = null;

        public ScriptTree()
        {
            InitializeComponent();
            _model = new TreeModel();
            this.treeView.Model = _model;
            this.scriptNodeTextBox.DrawText += nodeTextBox_DrawText;
            this.scriptNodeCheckBox.CheckStateChanged += scriptNodeCheckBox_CheckStateChanged;
            this.addMenuStep.Visible = true;
            this.addMenuAction.Visible = false;
            this.scriptColumn.Width = this.treeView.Width / 2;
            this.resultColumn.Width = this.treeView.Width / 2;
        }

        #region Add script, Get script, Clear Script and Update Test Result
        public void AddNewScript(Script script)
        {
            _model = new TreeModel();
            this.treeView.Model = _model;
            this.treeView.BeginUpdate();
            if (script != null)
            {
                //_script = script;
                script.Content.ForEach(step =>
                {
                    ScriptNode step_node = _addStepToTree(step);
                    if (step.Content != null)
                    {
                        _addStepContentToTree(step_node, step.Content);
                    }
                });
            }
            this.treeView.EndUpdate();
            this.treeView.ExpandAll();
            treeView_SelectionChanged(null, null);
        }

        public Script GetScript()
        {
            Script script = _getScriptFromTree();
            if (script != null && script.Content != null)
            {
                List<string> all_class_names = new List<string>();
                List<string> all_calibration_names = new List<string>();
                List<string> all_image_names = new List<string>();
                script.Content.ForEach(step =>
                {
                    if (step.Content != null)
                    {
                        _getClassName(step.Content, all_class_names);
                    }
                });
                script.AllClassNameInScript = all_class_names;
                return script;
            }
            else
            {
                return null;
            }
        }

        public void UpdateTestResult(int actionIndex, string result)
        {
            List<TreeNodeAdv> all_nodes = this.treeView.AllNodes.ToList();
            ScriptNode node = all_nodes[actionIndex].Tag as ScriptNode;
            node.Result = result;
        }

        public void ClearTestReulst()
        {
            this.treeView.BeginUpdate();
            this.treeView.AllNodes.ToList().ForEach(node => ((ScriptNode)node.Tag).Result = "");
            this.treeView.EndUpdate();
        }

        public void ClearScript()
        {
            if (_model != null)
            {
                this.treeView.BeginUpdate();
                _model.Nodes.Clear();
                this.treeView.EndUpdate();
            }
        }

        private Script _getScriptFromTree()
        {
            Node root = ((TreeModel)this.treeView.Model).Root;
            if (root.Nodes.Count > 0)
            {
                int index = 0;
                Script script = new Script();
                foreach (Node step_node in root.Nodes)
                {
                    if (script.Content == null) script.Content = new List<Step>();
                    Step step = (Step)DeepClone(step_node.Tag);
                    StepConfigure confgure = step.Configure;
                    confgure.StepIndex = index;
                    step.Configure = confgure;
                    index++;
                    index = _getStepContentFromTree(step, step_node.Nodes.ToList(), index);
                    script.Content.Add(step);
                }
                return script;
            }
            else
            {
                return null;
            }
        }

        private int _getStepContentFromTree(Step step, List<Node> nodes, int index)
        {
            if (nodes.Count > 0)
            {
                if (step.Content == null) step.Content = new List<LogicBase>();
                foreach (Node logic_node in nodes)
                {
                    LogicBase logic = (LogicBase)DeepClone(logic_node.Tag);
                    logic.ActionIndex = index;
                    index++;
                    if (logic_node.Nodes.Count > 0)
                    {
                        index = _getLogicFromTree(logic, logic_node.Nodes.ToList(), index);
                    }
                    step.Content.Add(logic);
                }
            }
            return index;
        }

        private int _getLogicFromTree(LogicBase parentlogic, List<Node> nodes, int index)
        {
            foreach (Node logic_node in nodes)
            {
                if (parentlogic.InternalLogic == null) parentlogic.InternalLogic = new List<LogicBase>();
                LogicBase logic = (LogicBase)DeepClone(logic_node.Tag);
                logic.ActionIndex = index;
                index++;
                if (logic_node.Nodes.Count > 0)
                {
                    index = _getLogicFromTree(logic, logic_node.Nodes.ToList(), index);
                }
                parentlogic.InternalLogic.Add(logic);
            }
            return index;
        }

        private void _getClassName(List<LogicBase> logics, List<string> allClassNames)
        {
            logics.ForEach(logic =>
            {
                if (logic.GetType() == typeof(ActionLogic))
                {
                    if (!allClassNames.Contains(((ActionLogic)logic).ClassName) & ((ActionLogic)logic).IsInclude)
                    {
                        allClassNames.Add(((ActionLogic)logic).ClassName);
                    }
                }
                if (logic.InternalLogic != null)
                {
                    _getClassName(logic.InternalLogic, allClassNames);
                }
            });
        }
        #endregion

        #region Menu operation to add steps, actions and logics
        private void treeView_SelectionChanged(object sender, EventArgs e)
        {
            if (_mode == Mode.Viewer)
            {
                this.addMenuStep.Visible = false;
                this.addMenuAction.Visible = false;
                return;
            }
            if (this.treeView.SelectedNode == null)
            {
                this.addMenuStep.Visible = true;
                this.addMenuAction.Visible = false;
                return;
            }
            if (this.treeView.SelectedNodes.Count > 1)
            {
                this.addMenuStep.Visible = false;
                this.addMenuAction.Visible = false;
                return;
            }
            if (this.treeView.SelectedNode.Level == 1)
            {
                this.addMenuStep.Visible = false;
                this.addMenuAction.Visible = true;
                return;
            }
            if (this.treeView.SelectedNode.Level > 1 & !(((ScriptNode)this.treeView.SelectedNode.Tag).Tag is ActionLogic))
            {
                this.addMenuStep.Visible = false;
                this.addMenuAction.Visible = true;
            }
            else
            {
                this.addMenuStep.Visible = false;
                this.addMenuAction.Visible = false;
            }
        }

        private void addMenuStep_Click(object sender, EventArgs e)
        {
            EditStepFormResult result = EditStepForm?.Invoke(null);
            if (result != null)
            {
                _addNewStep(result.StepName, result.StopIfFail, result.DefaultActionDelay, result.IsPreStep, result.IsFinalStep);
                ScriptChangedCallBack?.Invoke();
            }
        }

        private void addMenuAction_Click(object sender, EventArgs e)
        {
            EditLogicFormResult result = EditLogicForm?.Invoke(null);
            if (result!= null)
            {
                _addNewLogic(result.LogicType, result.ClassName, result.MethodName, result.ParametersName, result.Parameters);
                ScriptChangedCallBack?.Invoke();
            }
        }

        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeAdvMouseEventArgs e)
        {
            ScriptNode clicked_node = e.Node.Tag as ScriptNode;

            if (e.Node.Level == 1)
            {
                Step step = (Step)clicked_node.Tag;
                EditStepFormResult default_value = new EditStepFormResult
                {
                    StepName = step.Configure.StepName,
                    DefaultActionDelay = step.Configure.DefaultActionDelay,
                    StopIfFail = step.Configure.StopIfFail,
                    IsPreStep = step.Configure.IsPreStep,
                    IsFinalStep = step.Configure.IsFinalStep
                };
                EditStepFormResult result = EditStepForm?.Invoke(default_value);
                if (result != null)
                {
                    step.Configure = new StepConfigure
                    {
                        StepName = result.StepName,
                        StopIfFail = result.StopIfFail,
                        DefaultActionDelay = result.DefaultActionDelay,
                        IsInclude = step.Configure.IsInclude,
                        IsPreStep = result.IsPreStep,
                        IsFinalStep = result.IsFinalStep
                    };
                    _updateStepToTree(step);
                }
            }
            else
            {
                LogicBase logic = (LogicBase)clicked_node.Tag;
                EditLogicFormResult default_value = new EditLogicFormResult();
                if (logic.GetType() == typeof(ForLogic))
                {
                    default_value.LogicType = LogicType.ForLogic;
                    default_value.Parameters = new List<object> { ((ForLogic)logic).RetryIfFail, ((ForLogic)logic).RunningTimes };
                }
                if (logic.GetType() == typeof(IfLogic))
                {
                    default_value.LogicType = LogicType.IfLogic;
                    default_value.Parameters = new List<object> { ((IfLogic)logic).Criteria };
                }
                if (logic.GetType() == typeof(ActionLogic))
                {
                    default_value.LogicType = LogicType.ActionLogic;
                    default_value.ClassName = ((ActionLogic)logic).ClassName;
                    default_value.MethodName = ((ActionLogic)logic).MethodName;
                    default_value.ParametersName = ((ActionLogic)logic).ParametersName;
                    default_value.Parameters = ((ActionLogic)logic).Parameters;
                }
                EditLogicFormResult result = null;
                result = EditLogicForm?.Invoke(default_value);
                if (result != null)
                {
                    if (default_value.LogicType == LogicType.ForLogic)
                    {
                        ((ForLogic)logic).RetryIfFail = (bool)result.Parameters[0];
                        ((ForLogic)logic).RunningTimes = (int)result.Parameters[1];
                    }
                    if (default_value.LogicType == LogicType.IfLogic)
                    {
                        ((IfLogic)logic).Criteria = (string)result.Parameters[0];
                    }
                    if (default_value.LogicType == LogicType.ActionLogic)
                    {
                        ((ActionLogic)logic).ClassName = result.ClassName;
                        ((ActionLogic)logic).MethodName = result.MethodName;
                        ((ActionLogic)logic).ParametersName = result.ParametersName;
                        ((ActionLogic)logic).Parameters = result.Parameters;
                    }
                    _updateLogicToTree(default_value.LogicType, logic);
                }
            }
            ScriptChangedCallBack?.Invoke();
            this.treeView.ExpandAll();
        }

        private void _addNewStep(string stepName, bool stopIfFail, int defaultActionDelay, bool isPreStep, bool isFinalStep)
        {
            Step step = new Step();
            step.Configure = new StepConfigure
            {
                StepName = stepName,
                StopIfFail = stopIfFail,
                DefaultActionDelay = defaultActionDelay,
                IsInclude = true,
                IsPreStep = isPreStep,
                IsFinalStep = isFinalStep
            };
            _addStepToTree(step);
        }

        private void _addNewLogic(LogicType logicType, string className, string methodName, List<string> parametersName, List<object> parameters)
        {
            if (this.treeView.SelectedNode != null)
            {
                TreeNodeAdv node_level = this.treeView.SelectedNode;
                List<int> logic_position = new List<int>();
                for (int i = 1; i < this.treeView.SelectedNode.Level; i++)
                {
                    logic_position.Add(node_level.Index);
                    node_level = node_level.Parent;
                }
                logic_position.Reverse();
                if (node_level.Children.Count == 0)
                {
                    logic_position.Add(0);
                }
                else
                {
                    logic_position.Add(node_level.Children.Count - 1);
                }

                LogicBase logic = null;
                switch (logicType)
                {
                    case LogicType.ForLogic:
                        ForLogic for_logic = new ForLogic();
                        for_logic.RetryIfFail = (bool)parameters[0];
                        for_logic.RunningTimes = (int)parameters[1];
                        logic = for_logic;
                        break;
                    case LogicType.IfLogic:
                        IfLogic if_logic = new IfLogic();
                        if_logic.Criteria = (string)parameters[0];
                        logic = if_logic;
                        break;
                    case LogicType.ActionLogic:
                        ActionLogic action_logic = new ActionLogic();
                        action_logic.ClassName = className;
                        action_logic.MethodName = methodName;
                        action_logic.ParametersName = parametersName;
                        action_logic.Parameters = parameters;
                        logic = action_logic;
                        break;
                }
                logic.IsInclude = true;
                logic.NeedRecord = false;
                _addLogicToTree(this.treeView.SelectedNode.Tag as ScriptNode, logic);
            }
        }

        private void scriptNodeCheckBox_CheckStateChanged(object sender, TreePathEventArgs e)
        {
            ScriptNode checked_node = e.Path.LastNode as ScriptNode;
            bool isInclude = checked_node.CheckState == CheckState.Checked;
            if (this.treeView.SelectedNodes.Count > 0 && this.treeView.SelectedNodes.Select(x => this.treeView.GetPath(x).LastNode).Contains(e.Path.LastNode))
            {
                foreach (TreeNodeAdv node in this.treeView.SelectedNodes)
                {
                    TreePath path = this.treeView.GetPath(node);
                    _changeScriptNodeCheckBox(path, isInclude);
                }
            }
            else { _changeScriptNodeCheckBox(e.Path, isInclude); }
            ScriptChangedCallBack?.Invoke();
        }

        private void _changeScriptNodeCheckBox(TreePath path, bool isInclude)
        {
            ScriptNode checked_node = path.LastNode as ScriptNode;
            checked_node.CheckState = isInclude ? CheckState.Checked : CheckState.Unchecked;
            if (path.FullPath.Length == 1)
            {
                Step step = (Step)checked_node.Tag;
                StepConfigure configure = step.Configure;
                configure.IsInclude = isInclude;
                step.Configure = configure;
            }
            else
            {
                LogicBase logic = (LogicBase)checked_node.Tag;
                logic.IsInclude = isInclude;
            }
            if (checked_node.Nodes.Count != 0)
            {
                foreach (ScriptNode node in checked_node.Nodes)
                {
                    node.CheckState = isInclude ? CheckState.Checked : CheckState.Unchecked;
                    _changeScriptNodeCheckBox(new TreePath(path, node), isInclude);
                }
            }
        }
        #endregion

        #region Drag and drop operation
        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            this.treeView.DoDragDropSelectedNodes(DragDropEffects.Move);
        }

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNodeAdv[])) && treeView.DropPosition.Node != null)
            {
                TreeNodeAdv[] nodes = e.Data.GetData(typeof(TreeNodeAdv[])) as TreeNodeAdv[];
                TreeNodeAdv parent = treeView.DropPosition.Node;
                if (treeView.DropPosition.Position != NodePosition.Inside)
                    parent = parent.Parent;

                foreach (TreeNodeAdv node in nodes)
                    if (!_checkNodeParent(parent, node))
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }

                e.Effect = e.AllowedEffect;
            }
        }

        private bool _checkNodeParent(TreeNodeAdv parent, TreeNodeAdv node)
        {
            while (parent != null)
            {
                if (node == parent)
                    return false;
                else
                    parent = parent.Parent;
            }
            return true;
        }

        private void treeView_DragDrop(object sender, DragEventArgs e)
        {
            TreeNodeAdv[] nodes = (TreeNodeAdv[])e.Data.GetData(typeof(TreeNodeAdv[]));
            if (treeView.DropPosition.Node == null) return;
            ScriptNode dropNode = treeView.DropPosition.Node.Tag as ScriptNode;

            if ((this.treeView.DropPosition.Node.Level == 1 & nodes[0].Level > 1 & this.treeView.DropPosition.Position != NodePosition.Inside) | (nodes[0].Level == 1 & this.treeView.DropPosition.Node.Level > 1) | (nodes[0].Level == 1 & this.treeView.DropPosition.Node.Level == 1 & this.treeView.DropPosition.Position == NodePosition.Inside))
            {
                return;
            }

            if(dropNode.Tag is ActionLogic & this.treeView.DropPosition.Position == NodePosition.Inside) { return; }

            if (nodes[0].Level == 1) // Step Drag and Drop
            {
                int drop_index = treeView.DropPosition.Node.Index;
                TreePath drop_tree_path = this.treeView.GetPath(treeView.DropPosition.Node);

                for (int i = 0; i < nodes.Length; i++)
                {
                    _updateTreeNode(dropNode, nodes[i]);
                }
            }
            else
            {
                TreePath drop_tree_path = this.treeView.GetPath(this.treeView.DropPosition.Node);
                if (nodes[0].Parent == treeView.DropPosition.Node.Parent & treeView.DropPosition.Position != NodePosition.Inside) //Action Drag and Drop: in the same level
                {
                    _updateTreeNode(dropNode, nodes);
                }
                else if (drop_tree_path.LastNode == this.treeView.GetPath(nodes[0]).FullPath[this.treeView.GetPath(nodes[0]).FullPath.Length - 2] & treeView.DropPosition.Position == NodePosition.Inside) //Action Drag and Drop: Redundant Drag and Drop
                {
                    return;
                }
                else if (drop_tree_path.FirstNode == this.treeView.GetPath(nodes[0]).FirstNode) //Action Drag and Drop: Drag form different level but in the same step
                {
                    _updateTreeNode(dropNode, nodes);
                }
                else //Action Drag and Drop: Drag form different step
                {
                    _updateTreeNode(dropNode, nodes);
                }
            }
            this.treeView.ExpandAll();
            ScriptChangedCallBack?.Invoke();
        }

        private void _updateTreeNode(ScriptNode dropNode, TreeNodeAdv dragNode)
        {
            treeView.BeginUpdate();
            if (treeView.DropPosition.Position == NodePosition.Inside)
            {
                (dragNode.Tag as ScriptNode).Parent = dropNode;
            }
            else
            {
                Node parent = dropNode.Parent;
                Node nextItem = dropNode;
                if (treeView.DropPosition.Position == NodePosition.After) nextItem = dropNode.NextNode;
                if (nextItem == dragNode.Tag) return;
                (dragNode.Tag as Node).Parent = null;
                int index = -1;
                index = parent.Nodes.IndexOf(nextItem);
                ScriptNode item = dragNode.Tag as ScriptNode;
                if (index == -1)
                    parent.Nodes.Add(item);
                else
                {
                    parent.Nodes.Insert(index, item);
                }
            }
            this.treeView.EndUpdate();
        }

        private void _updateTreeNode(ScriptNode dropNode, TreeNodeAdv[] dragNodes)
        {
            treeView.BeginUpdate();
            if (treeView.DropPosition.Position == NodePosition.Inside)
            {
                for(int i = 0; i < dragNodes.Length; i++)
                {
                    (dragNodes[i].Tag as ScriptNode).Parent = dropNode;
                }
            }
            else
            {
                Node parent = dropNode.Parent;
                Node nextItem = dropNode;
                if (treeView.DropPosition.Position == NodePosition.After) nextItem = dropNode.NextNode;
                int index = -1;
                for (int i = 0; i < dragNodes.Length; i++)
                {
                    if (nextItem == dragNodes[i].Tag) continue;
                    (dragNodes[i].Tag as Node).Parent = null;
                    index = parent.Nodes.IndexOf(nextItem);
                    ScriptNode item = dragNodes[i].Tag as ScriptNode;
                    if (index == -1)
                    {
                        parent.Nodes.Add(item);
                    }
                    else
                    {
                        parent.Nodes.Insert(index, item);
                    }
                    index++;
                }
            }
            this.treeView.EndUpdate();
        }
        #endregion

        #region Copy, paste, delete operation
        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (this.treeView.SelectedNodes != null)
                {
                    _deleteTreeNodes();
                    ScriptChangedCallBack?.Invoke();
                }
            }
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control && this.treeView.SelectedNodes.Count != 0)
            {
                if (this.treeView.SelectedNodes.Count > 0) _copyTreeNodes();
            }
            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control && this.treeView.SelectedNode != null)
            {
                List<ScriptNode> copy_nodes = _getCopyNodes();
                if (copy_nodes != null)
                {
                    if ((this.treeView.SelectedNode.Level == 1 & copy_nodes[0].Tag is LogicBase) | (copy_nodes[0].Tag is Step & this.treeView.SelectedNode.Level > 1))
                    {
                        return;
                    }
                    _pasteTreeNodes(copy_nodes);
                    ScriptChangedCallBack?.Invoke();
                }
            }
        }

        private List<ScriptNode> _getCopyNodes()
        {
            List<ScriptNode> copy_nodes = null;
            if (Clipboard.ContainsData("Copy_Node"))
            {
                copy_nodes = new List<ScriptNode>();
                object data = Clipboard.GetData("Copy_Node");
                if (data is List<Step>)
                {
                    ((List<Step>)data).ForEach(step =>
                    {
                        ScriptNode step_node = _addStepToTree(step);
                        step_node.Parent = null;
                        if (step.Content != null)
                        {
                            _addStepContentToTree(step_node, step.Content);
                        }
                        copy_nodes.Add(step_node);
                    });
                }
                if (data is List<LogicBase>)
                {
                    copy_nodes = new List<ScriptNode>();
                    ScriptNode fake_node = new ScriptNode("fake");
                    _addStepContentToTree(fake_node, (List<LogicBase>)data);
                    copy_nodes = fake_node.Nodes.Cast<ScriptNode>().ToList();
                }
            }
            return copy_nodes;
        }

        private void _pasteTreeNodes(List<ScriptNode> copyNodes)
        {
            this.treeView.BeginUpdate();
            Node parent = (this.treeView.SelectedNode.Tag as ScriptNode).Parent;
            int index = parent.Nodes.IndexOf(this.treeView.SelectedNode.Tag as ScriptNode);
            foreach (ScriptNode copy_node in copyNodes)
            {
                ScriptNode paste_node = new ScriptNode(copy_node.Text);
                paste_node.Tag = DeepClone(copy_node.Tag);
                if (copy_node.Nodes.Count != 0)
                {
                    _createPastedChildNodes(paste_node, copy_node.Nodes.Cast<ScriptNode>().ToList());
                }
                if (index == -1)
                {
                    parent.Nodes.Add(paste_node);
                }
                else
                {
                    paste_node.CheckState = copy_node.CheckState;
                    parent.Nodes.Insert(index, paste_node);
                    index++;
                }
            }
            this.treeView.EndUpdate();
            this.treeView.ExpandAll();
        }

        private void _copyTreeNodes()
        {
            List<Step> steps = new List<Step>();
            List<LogicBase> logics = new List<LogicBase>();
            this.treeView.SelectedNodes.OrderBy(n => n.Index).ToList().ForEach(node_adv =>
            {
                if (node_adv.Level == 1) // Step
                {
                    Step step = DeepClone((Step)((ScriptNode)node_adv.Tag).Tag);
                    _getStepContentFromTree(step, ((Node)node_adv.Tag).Nodes.ToList());
                    steps.Add(step);
                }
                else // Logic
                {
                    LogicBase logic = DeepClone((LogicBase)((ScriptNode)node_adv.Tag).Tag);
                    if (((Node)node_adv.Tag).Nodes.Count > 0)
                    {
                        _getLogicFromTree(logic, ((Node)node_adv.Tag).Nodes.ToList());
                    }
                    logics.Add(logic);
                }
            });

            if ((this.treeView.SelectedNodes)[0].Level == 1)
            {
                Clipboard.SetData("Copy_Node", steps);
            }
            else
            {
                Clipboard.SetData("Copy_Node", logics);
            }
        }

        private void _deleteTreeNodes()
        {
            if (this.treeView.SelectedNodes != null && this.treeView.SelectedNodes.Count != 0)
            {
                this.treeView.SelectedNodes.ToList().ForEach(node =>
                {
                    (node.Tag as ScriptNode).Parent = null;
                });
            }
        }

        private void _createPastedChildNodes(ScriptNode pastedNode, List<ScriptNode> copyNodes)
        {
            foreach (ScriptNode copy_node in copyNodes)
            {
                ScriptNode pasted_parent_node = new ScriptNode(copy_node.Text);
                pasted_parent_node.CheckState = copy_node.CheckState;
                pasted_parent_node.Tag = DeepClone(copy_node.Tag);
                pastedNode.Nodes.Add(pasted_parent_node);
                if (copy_node.Nodes.Count != 0)
                {
                    _createPastedChildNodes((ScriptNode)pastedNode.Nodes.Last(), copy_node.Nodes.Cast<ScriptNode>().ToList());
                }
            }
        }

        private void _getStepContentFromTree(Step step, List<Node> nodes)
        {
            if (nodes.Count > 0)
            {
                if (step.Content == null) step.Content = new List<LogicBase>();
                foreach (Node logic_node in nodes)
                {
                    LogicBase logic = (LogicBase)DeepClone(logic_node.Tag);
                    if (logic_node.Nodes.Count > 0)
                    {
                        _getLogicFromTree(logic, logic_node.Nodes.ToList());
                    }
                    step.Content.Add(logic);
                }
            }
        }

        private void _getLogicFromTree(LogicBase parentlogic, List<Node> nodes)
        {
            foreach (Node logic_node in nodes)
            {
                if (parentlogic.InternalLogic == null) parentlogic.InternalLogic = new List<LogicBase>();
                LogicBase logic = (LogicBase)DeepClone(logic_node.Tag);
                if (logic_node.Nodes.Count > 0)
                {
                    _getLogicFromTree(logic, logic_node.Nodes.ToList());
                }
                parentlogic.InternalLogic.Add(logic);
            }
        }
        #endregion

        #region Script Class to Tree View
        private void _addStepContentToTree(ScriptNode parentNode, List<LogicBase> logicList)
        {
            logicList.ForEach(logic =>
            {
                ScriptNode child_node = _addLogicToTree(parentNode, logic);
                if (logic.InternalLogic != null)
                {
                    _addStepContentToTree(child_node, logic.InternalLogic);
                }
            });
        }

        private ScriptNode _addStepToTree(Step scriptStep)
        {
            string stop_if_fail = scriptStep.Configure.StopIfFail ? "(Fail Stop)" : "";
            string delay_time = string.Format("(Delay {0} ms)", scriptStep.Configure.DefaultActionDelay);
            string step_configure = scriptStep.Configure.IsPreStep ? "(Pre-Step)" : (scriptStep.Configure.IsFinalStep ? "(Final-Step)" : "");
            string step_text = "";
            if (_mode == Mode.Editor)
            {
                step_text = string.Format("Step: {0} {1} {2} {3}", scriptStep.Configure.StepName, stop_if_fail, delay_time, step_configure);
            }
            else
            {
                step_text = string.Format("Step {0}: {1} {2} {3} {4}", ((TreeModel)this.treeView.Model).Root.Nodes.Count + 1, scriptStep.Configure.StepName, stop_if_fail, delay_time, step_configure);
            }
            ScriptNode step = new ScriptNode(step_text);
            step.CheckState = (scriptStep.Configure.IsInclude) ? CheckState.Checked : CheckState.Unchecked;
            Step tag = DeepClone(scriptStep);
            tag.Content.Clear();
            step.Tag = tag;
            ((TreeModel)this.treeView.Model).Root.Nodes.Add(step);
            this.treeView.ExpandAll();
            return step;
        }

        private void _updateStepToTree(Step scriptStep)
        {
            string stop_if_fail = scriptStep.Configure.StopIfFail ? "(Fail Stop)" : "";
            string delay_time = string.Format("(Delay {0} ms)", scriptStep.Configure.DefaultActionDelay);
            string step_configure = scriptStep.Configure.IsPreStep ? "(Pre-Step)" : (scriptStep.Configure.IsFinalStep ? "(Final-Step)" : "");
            ScriptNode selected_node = this.treeView.SelectedNode.Tag as ScriptNode;
            selected_node.Text = string.Format("Step: {0} {1} {2} {3}", scriptStep.Configure.StepName, stop_if_fail, delay_time, step_configure);
            this.treeView.ExpandAll();
        }

        private ScriptNode _addLogicToTree(ScriptNode parent, LogicBase scriptLogic)
        {
            ScriptNode logic = null;
            if (scriptLogic.GetType() == typeof(ForLogic))
            {
                ForLogic for_logic = (ForLogic)scriptLogic;
                string loop_type = for_logic.RetryIfFail ? "Retry" : "Loop";
                string runnint_times = for_logic.RunningTimes.ToString();
                logic = new ScriptNode(string.Format("{0} {1} times", loop_type, runnint_times));
            }
            if (scriptLogic.GetType() == typeof(IfLogic))
            {
                IfLogic if_logic = (IfLogic)scriptLogic;
                if (if_logic.Criteria.Length > 1)
                {
                    string criteria = string.Join(", ", if_logic.Criteria);
                    logic = new ScriptNode(string.Format("If the result are {0}", criteria));
                }
                else
                {
                    logic = new ScriptNode(string.Format("If the result is {0}", if_logic.Criteria[0]));
                }
            }
            if (scriptLogic.GetType() == typeof(ActionLogic))
            {
                ActionLogic action_logic = (ActionLogic)scriptLogic;
                string parameters = "";
                if (action_logic.Parameters != null)
                {
                    for (int i = 0; i < action_logic.Parameters.Count; i++)
                    {
                        parameters += string.Format("{0}: {1}", action_logic.ParametersName[i], action_logic.Parameters[i]);
                        if (i != action_logic.Parameters.Count - 1) parameters += ", ";
                    }
                    parameters = $"({parameters})";
                }
                logic = new ScriptNode(string.Format("{0} ({1}) {2}", action_logic.MethodName.Replace('_', ' '), action_logic.ClassName, parameters));
            }
            if (logic != null)
            {
                logic.CheckState = (scriptLogic.IsInclude) ? CheckState.Checked : CheckState.Unchecked;
                LogicBase tag = DeepClone(scriptLogic);
                tag.InternalLogic = null;
                logic.Tag = tag;
                if (parent != null) parent.Nodes.Add(logic);
                this.treeView.ExpandAll();
            }
            return logic;
        }

        private void _updateLogicToTree(LogicType logicType, LogicBase scriptLogic)
        {
            ScriptNode selected_node = this.treeView.SelectedNode.Tag as ScriptNode;
            switch (logicType)
            {
                case LogicType.ForLogic:
                    ForLogic for_logic = (ForLogic)scriptLogic;
                    string loop_type = for_logic.RetryIfFail ? "Retry" : "Loop";
                    string runnint_times = for_logic.RunningTimes.ToString();
                    selected_node.Text = string.Format("{0} {1} times", loop_type, runnint_times);
                    break;
                case LogicType.IfLogic:
                    IfLogic if_logic = (IfLogic)scriptLogic;
                    if (if_logic.Criteria.Length > 1)
                    {
                        string criteria = string.Join(", ", if_logic.Criteria);
                        selected_node.Text = string.Format("If the result are {0}", criteria);
                    }
                    else
                    {
                        selected_node.Text = string.Format("If the result is {0}", if_logic.Criteria[0]);
                    }
                    break;
                case LogicType.ActionLogic:
                    ActionLogic action_logic = (ActionLogic)scriptLogic;
                    string parameters = "";
                    if (action_logic.Parameters != null)
                    {
                        for (int i = 0; i < action_logic.Parameters.Count; i++)
                        {
                            parameters += string.Format("{0}: {1}", action_logic.ParametersName[i], action_logic.Parameters[i]);
                            if (i != action_logic.Parameters.Count - 1) parameters += ", ";
                        }
                        parameters = $"({parameters})";
                    }
                    selected_node.Text = string.Format("{0} ({1}) {2}", action_logic.MethodName.Replace('_', ' '), action_logic.ClassName, parameters);
                    break;
            }
            this.treeView.ExpandAll();
        }
        #endregion

        #region Tree Node Color Display
        private void nodeTextBox_DrawText(object sender, DrawEventArgs e)
        {
            e.Font = new Font(this.treeView.Font.FontFamily, 9, FontStyle.Regular);
            if ((e.Node.Tag as ScriptNode).Text.StartsWith("Retry") | (e.Node.Tag as ScriptNode).Text.StartsWith("Loop"))
            {
                e.TextColor = Color.Green;
                e.Font = new Font(this.treeView.Font.FontFamily, 9, FontStyle.Regular);
            }
            if ((e.Node.Tag as ScriptNode).Text.StartsWith("If"))
            {
                e.TextColor = Color.Red;
                e.Font = new Font(this.treeView.Font.FontFamily, 9, FontStyle.Regular);
            }
            if ((e.Node.Tag as ScriptNode).Text.StartsWith("Step"))
            {
                e.TextColor = Color.Blue;
                e.Font = new Font(this.treeView.Font.FontFamily, 9, FontStyle.Bold);
            }
            if (e.Node.IsSelected)
            {
                e.BackgroundBrush = Brushes.Chocolate;
                e.TextColor = Color.White;
            }
        }
        #endregion

        private static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }

    class ScriptNode : Node
    {
        public ScriptNode(string text) : base(text) { }
        public string Result { set; get; }
    }

    public class EditStepFormResult
    {
        public string StepName;
        public int DefaultActionDelay;
        public bool StopIfFail;
        public bool IsPreStep;
        public bool IsFinalStep;
    }

    public class EditLogicFormResult
    {
        public LogicType LogicType;
        public string ClassName;
        public string MethodName;
        public List<string> ParametersName;
        public List<object> Parameters;
    }

    public enum LogicType
    {
        StepGroup = 0,
        ForLogic,
        IfLogic,
        ActionLogic
    }

    public class LogicInfo
    {
        public int ActionIndex;
        public bool IsReocrd;
    }
}
