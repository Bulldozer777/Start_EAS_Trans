using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Smart_explorer
{
    public partial class DBFormChange : Form
    {
        readonly static string baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        readonly static string appStorageFolder = Path.Combine(baseFolder, "Start_EAS_Trans");
        public DBFormChange()
        {
            InitializeComponent();
            Form4 form4 = new Form4();
            AutoCompleteStringCollection source = new AutoCompleteStringCollection() { };
            source.AddRange(form4.ReadText(appStorageFolder + @"\data\path\Start_EAS_Trans\save\eas all ops.txt"));
            textBox3.AutoCompleteCustomSource = source;
            textBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection source1 = new AutoCompleteStringCollection() { };
            source1.AddRange(form4.ReadText(appStorageFolder + @"\data\path\Start_EAS_Trans\save\Ip.txt"));
            this.textBox1.AutoCompleteCustomSource = source1;
            this.textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                Form4 form4 = new Form4();
                form4.Name_DataBase_and_Server(textBox3.Text, out string server, out string number_ops);
                textBox1.Text = server;
            }
            else
                MessageBox.Show("Поле для ввода номера ОПС - пустое");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
        }
    }
}
