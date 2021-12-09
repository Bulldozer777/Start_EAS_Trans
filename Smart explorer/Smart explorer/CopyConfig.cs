using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Smart_explorer
{
    public partial class CopyConfig : Form
    {
        string active_conf = "2021-12-01";
        string number_ops;
        string server;
        string appStorageFolder;
        public CopyConfig(string number_ops, string server, string appStorageFolder)
        {
            InitializeComponent();
            this.number_ops = number_ops;
            this.server = server;
            this.appStorageFolder = appStorageFolder;
            AutoCompleteStringCollection source = new AutoCompleteStringCollection()
            {
                "2021-12-01",
                "2021-10-01"
            };
            textBox1.AutoCompleteCustomSource = source;
            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }
        public void CopyConf()
        {
            try
            {
                string date_config = active_conf;
                string[] path_out_configuration = new string[9]
                    { @"\c$\ProgramData\RussianPost\nsi02\data\f130article-" + date_config + ".json",
                  @"\c$\ProgramData\RussianPost\nsi02\data\f130articletype-" + date_config + ".json",
                  @"\c$\ProgramData\RussianPost\nsi02\data\f130attributes-" + date_config + ".json",
                  @"\c$\ProgramData\RussianPost\nsi02\data\f130easops_map-" + date_config + ".json",
                  @"\c$\ProgramData\RussianPost\nsi02\data\f130easops_oper-" + date_config + ".json",
                  @"\c$\ProgramData\RussianPost\nsi02\data\f130mfk_sql-" + date_config + ".json",
                  @"\c$\ProgramData\RussianPost\nsi02\data\f130skdn-" + date_config + ".json",
                  @"\c$\ProgramData\RussianPost\nsi02\data\f130skdncommon-" + date_config + ".json",
                  @"\c$\ProgramData\RussianPost\nsi02\data\f130settings-" + date_config + ".json"};
                string[] path_in_configuration = new string[9]
                {
                @"D:\Новая конфигурация\f130article-" + date_config + ".json",
                @"D:\Новая конфигурация\f130articletype-" + date_config + ".json",
                @"D:\Новая конфигурация\f130attributes-" + date_config + ".json",
                @"D:\Новая конфигурация\f130easops_map-" + date_config + ".json",
                @"D:\Новая конфигурация\f130easops_oper-" + date_config + ".json",
                @"D:\Новая конфигурация\f130mfk_sql-" + date_config + ".json",
                @"D:\Новая конфигурация\f130skdn-" + date_config + ".json",
                @"D:\Новая конфигурация\f130skdncommon-" + date_config + ".json",
                @"D:\Новая конфигурация\f130settings-" + date_config + ".json"
                };
                for (int i = 0; i < path_out_configuration.Length; i++)
                {
                    string path = path_in_configuration[i];
                    string newPath = @"\\" + server + path_out_configuration[i];
                    FileInfo fileInf = new FileInfo(path);
                    if (fileInf.Exists)
                    {
                        fileInf.CopyTo(newPath, true);
                        progressBar1.Value += 11;
                    }
                }
                string newPath1 = @"\\" + server + path_out_configuration[8];
                FileInfo fileInf1 = new FileInfo(newPath1);
                if (fileInf1.Exists)
                {
                    progressBar1.Value = 100;
                    MessageBox.Show("Файлы конфигурации скопированы.");
                    //Process.Start(@"\\" + server + @"\c$\ProgramData\RussianPost\nsi02\data\");
                    progressBar1.Value = 0;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка {ex}");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (active_conf != "")
            {
                textBox1.Text = active_conf;
            }
            else
            {
                MessageBox.Show("Актуальная конфигурация не настроена");
            }
        }
      async private void button1_Click(object sender, EventArgs e)
        {
            await Task.Run(() => CopyConf());
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Form4.ProcessStart($"{appStorageFolder}" + @"\StaticExplorer\Новая конфигурация");
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Form4.ProcessStart(@"\\" + server + @"\c$\ProgramData\RussianPost\nsi02\data\");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(Rebut_Service_PochtaForm1);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            this.Activate();
        }
        private void Rebut_Service_PochtaForm1()
        {         
            var form = new Rebut_Service_Pochta.Form1(server);
            form.ShowDialog();
        }
    }
}
