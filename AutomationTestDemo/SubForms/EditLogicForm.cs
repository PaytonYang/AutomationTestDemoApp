using ScriptLibrary.ReadWrite;
using ScriptLibrary.Controls;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomationTestDemo.SubForms
{
    public partial class EditLogicForm : Form
    {
        public EditLogicFormResult? Result { private set; get; } = null;

        public EditLogicForm(EditLogicFormResult? defaultValue)
        {
            InitializeComponent();
            _initializePanel(defaultValue);
            this.logicTypeComboBox.SelectedIndexChanged += this.logicTypeComboBox_SelectedIndexChanged;
            this.classNameComboBox.SelectedIndexChanged += this.classNameComboBox_SelectedIndexChanged;
            this.methodComboBox.SelectedIndexChanged += this.methodComboBox_SelectedIndexChanged;
        }

        private void _initializePanel(EditLogicFormResult? defaultValue)
        {
            if (defaultValue == null)
            {
                var settings = ConfigurationManager.OpenExeConfiguration("AutomationTestDemo.exe").AppSettings.Settings;
                string? previous_logic = (settings["LogicType"] == null || settings["LogicType"].Value == "") ? null : settings["LogicType"].Value;
                string? class_name = (settings["ClassName"] == null || settings["ClassName"].Value == "") ? null : settings["ClassName"].Value;
                string? method_name = (settings["MethodName"] == null || settings["MethodName"].Value == "") ? null : settings["MethodName"].Value;
                this.logicTypeComboBox.SelectedIndex = (previous_logic == null) ? 0 : this.logicTypeComboBox.Items.IndexOf(previous_logic);
                if (previous_logic != null & class_name != null & method_name != null)
                {
                    EditLogicFormResult previous_values = new EditLogicFormResult
                    {
                        ClassName = class_name,
                        MethodName = method_name,
                        Parameters = null,
                    };
                    this.logicTypeComboBox_SelectedIndexChanged(this, new DefaultValueEventArgs(previous_values));
                }
                else
                {
                    this.logicTypeComboBox_SelectedIndexChanged(this, new EventArgs());
                }
            }
            else
            {
                switch (defaultValue.LogicType)
                {
                    case LogicType.ForLogic:
                        if ((bool)defaultValue.Parameters[0]) //Stop If Fail
                        {
                            this.logicTypeComboBox.SelectedIndex = 1; //Retry
                        }
                        else
                        {
                            this.logicTypeComboBox.SelectedIndex = 2; //Loop
                        }
                        this.logicTypeComboBox.Enabled = false;
                        break;
                    case LogicType.IfLogic:
                        this.logicTypeComboBox.SelectedIndex = 3;
                        this.logicTypeComboBox.Enabled = false;
                        break;
                    case LogicType.ActionLogic:
                        this.logicTypeComboBox.SelectedIndex = 0;
                        break;
                }
                this.logicTypeComboBox_SelectedIndexChanged(this, new DefaultValueEventArgs(defaultValue));
            }
        }

        private void logicTypeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            this.classNameComboBox.Items.Clear();
            if (this.logicTypeComboBox.SelectedItem.ToString() == "General Action")
            {
                this.classLabel.Visible = true;
                this.classNameComboBox.Visible = true;
                var names = ActionReader.ReadActions("TestLibrary", "TestLibrary");
                foreach (Tuple<string, Type> name in names)
                {
                    this.classNameComboBox.Items.Add(new ComboBoxItem(name.Item2, name.Item1));
                }
                EditLogicFormResult? default_value = e is DefaultValueEventArgs ? ((DefaultValueEventArgs)e).DefaultValue : null;
                if (default_value == null)
                {
                    this.classNameComboBox.SelectedIndex = 0;
                }
                else
                {
                    ComboBoxItem? item = this.classNameComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(x => x.Text == default_value.ClassName);
                    this.classNameComboBox.SelectedItem = item?? this.classNameComboBox.Items[0];
                    this.classNameComboBox_SelectedIndexChanged(sender, e);
                }
            }
            else
            {
                this.classLabel.Visible = false;
                this.classNameComboBox.Visible = false;
                this.methodLabel.Visible = false;
                this.methodComboBox.Visible = false;
                this.methodComboBox_SelectedIndexChanged(sender, e);
            }
        }


        private void classNameComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            this.methodLabel.Visible = true;
            this.methodComboBox.Visible = true;
            string action_name = this.classNameComboBox.SelectedItem.ToString()!;
            Type action_type = (Type)((ComboBoxItem)this.classNameComboBox.SelectedItem).Value;
            this.methodComboBox.Items.Clear();
            var names = ActionReader.ReadActionMethods(action_type);
            foreach (Tuple<string, MethodInfo> name in names)
            {
                this.methodComboBox.Items.Add(new ComboBoxItem(name.Item2, name.Item1));
            }
            EditLogicFormResult? default_value = e is DefaultValueEventArgs ? ((DefaultValueEventArgs)e).DefaultValue : null;
            if (default_value == null)
            {
                this.methodComboBox.SelectedIndex = 0;
            }
            else
            {
                ComboBoxItem? item = this.methodComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(x => x.Text == default_value.MethodName);
                this.methodComboBox.SelectedItem = item?? this.methodComboBox.Items[0];
                this.methodComboBox_SelectedIndexChanged(sender, e);
            }
        }

        private void methodComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            EditLogicFormResult? default_value = e is DefaultValueEventArgs ? ((DefaultValueEventArgs)e).DefaultValue : null;
            _updateParametersName(default_value == null ? null : default_value.Parameters);
        }

        private void _updateParametersName(List<object>? defaultParametersValue)
        {
            string? logic_type = this.logicTypeComboBox.SelectedItem.ToString();
            string? class_name = this.classNameComboBox.SelectedItem?.ToString();
            string? method_name = this.methodComboBox.SelectedItem?.ToString();
            if (logic_type == "General Action" & class_name != null & method_name != null)
            {
                this.parametersGridView.Rows.Clear();

                var parameters_name = ActionReader.ReadMethodParameters((MethodInfo)((ComboBoxItem)this.methodComboBox.SelectedItem!).Value);

                foreach (Tuple<string, string> parameter in parameters_name)
                {
                    this.parametersGridView.Rows.Add(parameter.Item1, "", parameter.Item2);
                }
                if (defaultParametersValue != null)
                {
                    int index = 0;
                    defaultParametersValue.ForEach(parameter =>
                    {
                        this.parametersGridView["parametersValue", index].Value = parameter.ToString();
                        index++;
                    });
                }
            }
            if (logic_type == "Retry" | logic_type == "Loop")
            {
                this.parametersGridView.Rows.Clear();
                this.parametersGridView.Rows.Add("Running Times", "", "Int32");
                this.parametersGridView.Columns["parametersValue"].ReadOnly = false;
                if (defaultParametersValue != null) this.parametersGridView["parametersValue", 0].Value = defaultParametersValue[1];
            }
            if (logic_type == "If Condition")
            {
                this.parametersGridView.Rows.Clear();
                this.parametersGridView.Rows.Add("Criteria", "Fail", "String");
                this.parametersGridView.Columns["parametersValue"].ReadOnly = false;
                if (defaultParametersValue != null) this.parametersGridView["parametersValue", 0].Value = (string)defaultParametersValue[0];
            }
            foreach (DataGridViewRow row in this.parametersGridView.Rows)
            {
                row.DefaultCellStyle.Font = new Font("Times New Roman", 9);
            }
        }

        private void parametersGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 & e.RowIndex >= 0)
            {
                this.parametersGridView.BeginEdit(true);
            }
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            _checkParameters();
        }

        private void _checkParameters()
        {
            List<object>? parameters_value = null;
            bool parameters_pass = true;
            if (this.parametersGridView.Rows.Count != 0)
            {
                foreach (DataGridViewRow row in this.parametersGridView.Rows)
                {
                    if (row.Cells["parametersValue"].Value.ToString() == "")
                    {
                        MessageBox.Show(this, "Parameters value cannot be empty");
                        parameters_pass = false;
                        break;
                    }
                    else
                    {
                        try
                        {
                            object data = _checkDataFormat(row.Cells["parametersValue"].Value, (string)row.Cells["parametersType"].Value);
                            if (parameters_value == null) parameters_value = new List<object>();
                            parameters_value.Add(data);
                        }
                        catch
                        {
                            MessageBox.Show(this, "Wrong parameters value format");
                            parameters_pass = false;
                            break;
                        }
                    }
                }
            }
            if (parameters_pass)
            {
                _exportEditLogic(parameters_value);
                this.Close();
            }
        }

        

        private object _checkDataFormat(object data, string format)
        {
            try
            {
                return ActionReader.DataFormat(data, format);
            }
            catch { throw; }
        }

        private void _exportEditLogic(List<object>? parameters)
        {
            Result = new EditLogicFormResult();
            string action_type = this.logicTypeComboBox.SelectedItem.ToString()!;
            if (action_type == "General Action")
            {
                Result.LogicType = LogicType.ActionLogic;
                Result.ClassName = this.classNameComboBox.SelectedItem.ToString();
                Result.MethodName = this.methodComboBox.SelectedItem.ToString()!.Replace(' ', '_');
                List<string> parameters_name = new List<string>();
                foreach (DataGridViewRow row in this.parametersGridView.Rows)
                {
                    parameters_name.Add((string)row.Cells["parametersName"].Value);
                }
                Result.ParametersName = parameters_name;
                Result.Parameters = parameters;
            }
            if (action_type == "Retry")
            {
                Result.LogicType = LogicType.ForLogic;
                parameters!.Insert(0, true);
                Result.Parameters = parameters;
            }
            if (action_type == "Loop")
            {
                Result.LogicType = LogicType.ForLogic;
                parameters!.Insert(0, false);
                Result.Parameters = parameters;
            }
            if (action_type == "If Condition")
            {
                Result.LogicType = LogicType.IfLogic;
                Result.Parameters = parameters;
            }
            _recordSelectedLogic();
        }

        private void _recordSelectedLogic()
        {
            var settings = ConfigurationManager.OpenExeConfiguration("AutomationTestDemo.exe").AppSettings.Settings;
            if (settings["LogicType"] == null) settings.Add("LogicType", "");
            if (settings["ClassName"] == null) settings.Add("ClassName", "");
            if (settings["MethodName"] == null) settings.Add("MethodName", "");
            settings["LogicType"].Value = this.logicTypeComboBox.Text;
            settings["ClassName"].Value = this.classNameComboBox.Text;
            settings["MethodName"].Value = this.methodComboBox.Text;
            settings.CurrentConfiguration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public ComboBoxItem(object value, string text)
            {
                Text = text;
                Value = value;
            }
            public override string ToString()
            {
                return Text;
            }
        }

        public class DefaultValueEventArgs : EventArgs
        {
            public EditLogicFormResult? DefaultValue { get; set; } = null;
            public DefaultValueEventArgs(EditLogicFormResult? defaultValue)
            {
                DefaultValue = defaultValue;
            }
        }

        private void comboBox_Leave(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem == null)
            {
                comboBox.SelectedIndex = 0;
            }
        }
    }
}
