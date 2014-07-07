using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TesterClient.ServiceReference1;

namespace TesterClient
{
    public partial class MainForm : Form
    {
        EnterForm enterForm = null;
        Service1Client client = null;
        List<TestsTheme> tests = null;
        Test currTest = null;
        List<UserGroup> studentGroups = null;
        User currUser = null;
        int currTask = 0;

        public MainForm(EnterForm enterForm, User user, List<UserGroup> _studentGroups)
        {
            InitializeComponent();
            //client.GetIRT(tests[0].Tests[0]);
            this.enterForm = enterForm;
            client = enterForm.client;
            currUser = user;
            tests = client.GetAllTests();
            Text = "Пользователь: " + user.Name;
            listBoxThemes.DataSource = tests.Select(a => a.Name).ToList();
            studentGroups = _studentGroups;
            for (int i = 0; i < studentGroups.Count; i++)
            {
                List<TreeNode> childs = new List<TreeNode>();
                for (int j = 0; j < studentGroups[i].Users.Count; j++)
                {
                    var tr = new TreeNode(studentGroups[i].Users[j].Name);
                    tr.Tag = studentGroups[i].Users[j];
                    childs.Add(tr);
                }
                var tn = new TreeNode(studentGroups[i].Name, childs.ToArray());
                treeViewStudents.Nodes.Add(tn);
            }
            panelTestFront.Hide();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            enterForm.Close();
        }

        private void listBoxThemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxThemes.SelectedIndex > -1)
            {
                listBoxTests.DataSource = tests[listBoxThemes.SelectedIndex].Tests.Select(a => a.Name).ToList();
            }
        }

        private void listBoxTests_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxTests.SelectedIndex > -1)
            {
                buttonBeginTest.Visible = true;           
            }
            else
            {
                listBoxTests.SelectedIndex = -1;
                buttonBeginTest.Visible = false;
            }
        }

        private void ShowTask()
        {
            SetLabelTaskNum();
            textBoxTask.Text = currTest.Tasks[currTask].Text;
            listBoxAnswers.Items.Clear();
            for (int i = 0; i < currTest.Tasks[currTask].Answers.Count; i++)
            {
                /*listBoxAnswers.Items.Add(new RadioButton() 
                                        {
                                            Text = currTestPass.Test.Tasks[currTestPass.LastTask].Answers[i], 
                                            Parent = listBoxAnswers, 
                                            Height = 30,
                                            Name = "r"+i.ToString(),
                                        });*/
                listBoxAnswers.Items.Add(currTest.Tasks[currTask].Answers[i]);
            }
        }

        private void buttonEnd_Click(object sender, EventArgs e)
        {
            panelTestFront.Hide();
            panel1.Enabled = true;
        }

        private void buttoNext_Click(object sender, EventArgs e)
        {
            if (currTask < currTest.Tasks.Count - 1)
            {
                currTask++;
                ShowTask();
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            if (currTask > 0)
            {
                currTask--;
                ShowTask();
            }
        }

        private void listBoxAnswers_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void SetLabelTaskNum()
        {
            labelTaskNum.Text = string.Format("Вопрос {0} из {1}", currTask + 1, currTest.Tasks.Count);
        }

        private void buttonBeginTest_Click(object sender, EventArgs e)
        {
            panelTestFront.Show();
            panel1.Enabled = false;
            currTest = tests[listBoxThemes.SelectedIndex].Tests[listBoxTests.SelectedIndex];
            ShowTask();  
        }
    } 
    
}
