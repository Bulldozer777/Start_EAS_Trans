using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Start_EAS_Trans
{
    public partial class Form3 : Form1
    {
        public ListBox listBox3;
        public Form3()
        {
            InitializeComponent();
            button34.Click += CounterEventNewForm;
        }
        public Form3(ListBox listBox)
        {
            InitializeComponent();
            listBox3 = listBox;
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
        private void button34_Click(object sender, EventArgs e)
        {
            Form1 newForm = new Form1(streamwriteEAStrans);
            newForm.Text = $"Новая задача №{i}";
            newForm.Show();
        }


        async private void button35_Click(object sender, EventArgs e)
        {
            if (pathAutoEASTrans != "")
            {
                Form3 form3 = new Form3(listBox2);
                Methods methods = new Methods(form3.listBox3);
                string[] enum_ops1 = methods.ReadText(pathAutoEASTrans);
                if (enum_ops1[0] != "")
                {
                    string ops = "";
                    List<string> enum_ops = new List<string>();
                    for (int i = 0; i < enum_ops1.Length; i++)
                    {
                        if (enum_ops1[i] != "" & enum_ops1[i] != null)
                        {
                            enum_ops.Add(enum_ops1[i]);
                        }
                    }
                    for (int i = 0; i < enum_ops.Count; i++)
                    {
                        if (enum_ops[i] != "")
                        {
                            ops = enum_ops[i];
                            string server = "";
                            await Task.Run(() => methods.Prod_Name_DataBase_and_Server(ops, out server, out string namedb));
                            if (server != "")
                            {
                                await Task.Run(() => methods.СheckEasTransport(server, ops));
                                while (methods.action != 1)
                                {
                                    await Task.Delay(400);
                                }
                                await Task.Delay(4000);
                            }
                            await Task.Run(() => label30.Text = $"Выполнено: {i + 1} из {enum_ops.Count} ОПС: {ops}");
                            methods.action = 0;
                            action = 0;
                            if (streamwriteEAStrans == 1)
                            {
                                methods.EndOperationDistrict(out string[] DefinitionOps);
                                for (int k = 0; k < DefinitionOps.Length; k++)
                                {
                                    if (ops == DefinitionOps[k])
                                    {
                                        methods.AddStreamwriterEndOperationDistrict();
                                    }
                                }
                            }
                        }
                    }
                    await Task.Run(() => label30.Text = $"Выполнено: {enum_ops.Count} из {enum_ops.Count} ОПС: {enum_ops[enum_ops.Count - 1]}");
                    MessageBox.Show($"Выполнение инструкций завершено\n");
                    if (streamwriteEAStrans == 1)
                    {
                        Smart_explorer.Form4.ProcessStart(@"C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Start_EAS_Trans\autoreader out");
                    }
                }
                else
                {
                    MessageBox.Show($"Пустой файл AutoMode\n");
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
            Smart_explorer.Form4.ProcessStart(writePath);
        }
        private void button31_Click(object sender, EventArgs e)
        {
            Smart_explorer.Form4.ProcessStart(@"C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Start_EAS_Trans\autoreader out");
        }
        private void button43_Click(object sender, EventArgs e)
        {
            Smart_explorer.Form4.ProcessStart(@"C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Start_EAS_Trans\Prod");
        }

        private void button46_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            label25.Text = "Файлов:";
            label26.Text = "Папок:";
            label27.Text = "Элементов:";
        }

        private void button48_Click(object sender, EventArgs e)
        {
            Smart_explorer.Form4.ProcessStart(@"C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Start_EAS_Trans\autoreader in\транспорт.txt");
        }
    }
}
