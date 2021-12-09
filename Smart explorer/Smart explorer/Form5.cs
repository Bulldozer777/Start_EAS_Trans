using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Smart_explorer
{
    public partial class Form5 : Form
    {
        private readonly string appStorageFolder;
        public Form5()
        {
            InitializeComponent();
        }
        public Form5(string appStorageFolder)
        {
            InitializeComponent();
            this.appStorageFolder = appStorageFolder;
        }
        private void button63_Click(object sender, EventArgs e)
        {
            Form4.ProcessStart($"{appStorageFolder}" + @"\othertxt\Отдел контроля телефоны.txt");
        }
        private void button64_Click(object sender, EventArgs e)
        {
            Form4.ProcessStart($"{appStorageFolder}" + @"\othertxt\телефоны УФПС.txt");
        }
    }
}
