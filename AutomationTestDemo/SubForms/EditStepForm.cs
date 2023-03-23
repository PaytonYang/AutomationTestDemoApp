using ScriptLibrary.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomationTestDemo.SubForms
{
    public partial class EditStepForm : Form
    {
        public EditStepFormResult? Result { private set; get; } = null;

        public EditStepForm(EditStepFormResult defaultValue)
        {
            InitializeComponent();
            if (defaultValue == null)
            {
                this.stepNameTextBox.Text = "";
                this.actionDelayTextBox.Text = "2000";
                this.stopIfFail_Yes.Checked = true;
                this.stopIfFail_No.Checked = false;
                this.normalStepRadioButton.Checked = true;
            }
            else
            {
                this.stepNameTextBox.Text = defaultValue.StepName;
                this.actionDelayTextBox.Text = defaultValue.DefaultActionDelay.ToString();
                this.stopIfFail_Yes.Checked = defaultValue.StopIfFail;
                this.stopIfFail_No.Checked = !this.stopIfFail_Yes.Checked;
                if (defaultValue.IsPreStep | defaultValue.IsFinalStep)
                {
                    this.preStepRadioButton.Checked = defaultValue.IsPreStep;
                    this.finalStepRadioButton.Checked = defaultValue.IsFinalStep;
                }
            }
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.stepNameTextBox.Text))
            {
                MessageBox.Show(this, "Step name cannot be empty");
                return;
            }
            int delay_time;
            if (!int.TryParse(this.actionDelayTextBox.Text, out delay_time))
            {
                MessageBox.Show(this, "Wrong format delay time");
                return;
            }
            Result = new EditStepFormResult
            {
                StepName = this.stepNameTextBox.Text,
                DefaultActionDelay = delay_time,
                StopIfFail = this.stopIfFail_Yes.Checked,
                IsPreStep = this.preStepRadioButton.Checked,
                IsFinalStep = this.finalStepRadioButton.Checked
            };
            this.Close();
        }

        private void EditStepForm_Shown(object sender, EventArgs e)
        {
            this.stepNameTextBox.Focus();
        }
    }
}
