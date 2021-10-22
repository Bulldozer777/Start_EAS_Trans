using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Start_EAS_Trans
{
    public partial class Form3 : Form1
    {
        public Form3()
        {
            InitializeComponent();
            button34.Click += CounterEventNewForm;
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            Width = 844;
            Height = 574;
        }
        private void CounterEventNewForm(object sender, EventArgs e)
        {
            i = close + ++i;
        }
        private void CounterEventClozeForm(object sender, EventArgs e)
        {
            i--;
        }
        private void Form1_Closing(Object sender, CancelEventArgs e)
        {
            if (!isDataSaved)
            {
                e.Cancel = true;
                MessageBox.Show("Дождитесь завершения выполнения скрипта.");
            }
            else
            {
                e.Cancel = false;
                close--;
                MessageBox.Show("Досвидания!");
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Checked == true)
            {
                streamwriteEAStrans = 1;
                MessageBox.Show($"Запись в файл по пути:\n{writePath}\nВключена");
            }
            else
            {
                streamwriteEAStrans = 0;
                MessageBox.Show($"Запись в файл по пути:\n{writePath}\nВыключена");
            }
        }
        private void button34_Click(object sender, EventArgs e)
        {
            Form1 newForm = new Form1(streamwriteEAStrans);
            newForm.Text = $"Новая задача №{i}";
            newForm.Show();
        }

        private void button35_Click(object sender, EventArgs e)
        {
            if (pathAutoEASTrans != "")
            {
                for (int i = 0; i < ReadText(pathAutoEASTrans).Length; i++)
                {
                    ProdStreamWriterEAStrans("ОПС номер - " + ReadText(pathAutoEASTrans)[i]);
                    AutoStartExport(ReadText(pathAutoEASTrans)[i]);
                }
            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog autoPath = new OpenFileDialog() { Filter = "All files|*.*", ValidateNames = true, Multiselect = false })
                if (autoPath.ShowDialog() == DialogResult.OK)
                {
                    textBox9.Text = autoPath.FileName;
                    pathAutoEASTrans = autoPath.FileName;
                }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Checked == true)
            {
                pathAutoEASTrans = @"C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Start_EAS_Trans\autoreader in\транспорт.txt";
                MessageBox.Show($"Cчитывание номеров ОПС для автоматической работы программы\n" +
                    $"\nВключено по умолчанию.\n\nПуть по умолчанию: {pathAutoEASTrans}");
            }
            else
            {
                pathAutoEASTrans = "";
                MessageBox.Show($"Cчитывание номеров ОПС для автоматической работы программы\n - Выключено");
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            AddStramwriter();
        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Checked == true)
            {
                streamwriteEAStrans = 1;
                MessageBox.Show($"Запись в файл по пути:\n{writePath}\nВключена");
            }
            else
            {
                streamwriteEAStrans = 0;
                MessageBox.Show($"Запись в файл по пути:\n{writePath}\nВыключена");
            }
        }
        private void button37_Click(object sender, EventArgs e)
        {
            Process.Start(writePath);
        }

        private void button31_Click(object sender, EventArgs e)
        {
            Process.Start(@"C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Start_EAS_Trans\autoreader out");
        }
    }
}
