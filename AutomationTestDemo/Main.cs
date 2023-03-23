using AutomationTestDemo.SubForms;
using AutomationTestDemo.TestProcess;
using ScriptLibrary;
using ScriptLibrary.Controls;
using ScriptLibrary.Executor;
using ScriptLibrary.ReadWrite;
using System.Diagnostics;
using System.Globalization;
using System.Net.Mail;
using System.Reflection;
using System.Text.Json;

namespace AutomationTestDemo;

public partial class Main : Form
{
    private ScriptTree _scriptTree = new ScriptTree();
    private ScriptTestManager? _manager = null;

    public Main()
    {
        InitializeComponent();
        _initializePanel();
    }

    private async void startButton_Click(object sender, EventArgs e)
    {
        try
        {
            Script script = this._scriptTree.GetScript();
            if (script != null)
            {
                this._scriptTree.ClearTestReulst();
                this.passTextBox.Text = ""; this.failTextBox.Text = ""; this.totalTextBox.Text = "";
                _scriptModeSwitch(true);
                _manager = new ScriptTestManager();
                _manager.ActionPositionCallBack = _onActionPosition;
                _manager.ActionCompletedCallBack = _onActionCompleted;
                _manager.StepCompletedCallBack = _onStepCompleted;
                _manager.TestCompletedCallBack = _onTestCompleted;
                _manager.ScriptCompletedCallBack = _onScriptCompleted;
                _manager.FailStopCallBack = _onFailStop;
                _manager.ErrorHandleCallBack = _onErrorStop;
                script.LoopTimes = 1;
                await Task.Run(() => _manager.RunTest(script));
                this.stopButton.Enabled = true;
                this.startButton.Enabled = false;
            }
            else { MessageBox.Show("Script is empty"); }
        }
        catch (Exception error) { MessageBox.Show(error.Message); }
    }

    private void stopButton_Click(object sender, EventArgs e)
    {
        if (_manager!= null && _manager.IsRunning)
        {
            _manager.StopTest();
        }
    }

    private void _initializePanel()
    {
        this.panel.Controls.Add(_scriptTree);
        _scriptModeSwitch(false);
        this.stopButton.Enabled = false;
    }

    private void _onActionPosition(int actionIndex)
    {
        this.Invoke(new Action(() =>
        {
            _scriptTree.SelectedIndex = actionIndex;
        }));
    }

    private void _onActionCompleted(int actionIndex, string actionResult)
    {
        this.Invoke(new Action(() =>
        {
            _scriptTree.UpdateTestResult(actionIndex, actionResult);
        }));
    }

    private void _onStepCompleted(int stepIndex, string stpeResult)
    {
        this.Invoke(new Action(() =>
        {
            _scriptTree.UpdateTestResult(stepIndex, stpeResult);
        }));
    }

    private void _onTestCompleted(int passNumber, int failNumber)
    {
        this.Invoke(new Action(() =>
        {
            this.passTextBox.Text = passNumber.ToString();
            this.failTextBox.Text = failNumber.ToString();
            this.totalTextBox.Text = (passNumber + failNumber).ToString();
            _scriptTree.Refresh();
        }));
    }

    private void _onScriptCompleted()
    {
        MessageBox.Show("Test Finish", "Notification");
        this.Invoke(new Action(() =>
        {
            _scriptModeSwitch(false);
            this.stopButton.Enabled = false;
            this.startButton.Enabled = true;
        }));
    }

    private FailDialogResult _onFailStop(string failMessage)
    {
        DialogResult result = MessageBox.Show(failMessage, "Fail", MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)
        {
            return new FailDialogResult { StopTest = false, ReallyFail = true };
        }
        else
        {
            return new FailDialogResult { StopTest = true, ReallyFail = true };
        }
    }

    private void _onErrorStop(string errorMessage)
    {
        MessageBox.Show(errorMessage, "Error");
        this.Invoke(new Action(() =>
        {
            _scriptModeSwitch(false);
            this.stopButton.Enabled = false;
            this.startButton.Enabled = true;
        }));
    }

    private void _scriptModeSwitch(bool isTesting)
    {
        if (isTesting)
        {
            _scriptTree.EditMode = ScriptTree.Mode.Viewer;
            _scriptTree.EditStepForm = null;
            _scriptTree.EditLogicForm = null;
            _scriptTree.ScriptChangedCallBack = null;
        }
        else
        {
            _scriptTree.EditMode = ScriptTree.Mode.Editor;
            _scriptTree.EditStepForm = _onCallAddStepForm;
            _scriptTree.EditLogicForm = _onCallAddLogicForm;
        }
    }

    private EditStepFormResult? _onCallAddStepForm(EditStepFormResult defaultValue)
    {
        EditStepForm form = new EditStepForm(defaultValue);
        form.ShowDialog();
        return form.Result;
    }

    private EditLogicFormResult? _onCallAddLogicForm(EditLogicFormResult defaultValue)
    {
        EditLogicForm form = new EditLogicForm(defaultValue);
        form.ShowDialog();
        return form.Result;
    }

    private void saveButton_Click(object sender, EventArgs e)
    {
        try
        {
            Script script = this._scriptTree.GetScript();
            if (script != null)
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Filter = "(*.bin)|*.bin";
                    dialog.RestoreDirectory = true;
                    DialogResult result = dialog.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.FileName))
                    {
                        ScriptFile.Write(dialog.FileName, script);
                        MessageBox.Show("Save Completed");
                    }
                }
            }
            else { MessageBox.Show("Script is empty"); }
        }
        catch (Exception error) { MessageBox.Show(error.Message); }
    }
}