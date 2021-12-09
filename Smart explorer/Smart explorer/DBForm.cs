using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Smart_explorer
{
    public partial class DBForm : Form
    {
        // This delegate enables asynchronous calls for setting  
        // the text property on a TextBox control.  
        delegate void StringArgReturningVoidDelegate(string text);
        private Thread demoThread = null;

        public int Progresscount = 0;
        static EventWaitHandle waithandler = new AutoResetEvent(false);
        private readonly string Action;
        private readonly string Description;
        private readonly DateTime DateTimeAction;
        private ApplicationContext db;
        public DBForm()
        {
            InitializeComponent();
            db = new ApplicationContext();
            db.DBSmartExplorers.Load();           
            dataGridView1.DataSource = db.DBSmartExplorers.Local.ToBindingList();
            dataGridView1.Columns[0].Width = 30;
            dataGridView1.Columns[1].Width = 150;
            dataGridView1.Columns[2].Width = 70;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 85;
            dataGridView1.Columns[5].Width = 158;
        }
        public DBForm(string Action, string Description, DateTime DateTimeAction)
        {
            InitializeComponent();
            db = new ApplicationContext();
            db.DBSmartExplorers.Load();
            dataGridView1.DataSource = db.DBSmartExplorers.Local.ToBindingList();
            this.Action = Action;
            this.Description = Description;
            this.DateTimeAction = DateTimeAction;
        }

        async public void DBSmartExplorersWrite(string Action, string NumberOps, string StatusConnection,string IpConnection, DateTime DateTimeAction)
        {
            //ApplicationContext i = new ApplicationContext();
            DBSmartExplorer DBSmartExplorers = new DBSmartExplorer();
            DBSmartExplorers.Action = Action;
            DBSmartExplorers.NumberOps = NumberOps;
            DBSmartExplorers.StatusConnection = StatusConnection;
            DBSmartExplorers.IpConnection = IpConnection;
            DBSmartExplorers.DateTimeAction = DateTimeAction;
            db.DBSmartExplorers.Add(DBSmartExplorers);
            //db.SaveChanges();
            await Task.Run(() => db.SaveChanges());
        }

        private void DBForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DBFormChange DBFormChange = new DBFormChange();
            DialogResult result = DBFormChange.ShowDialog(this);
            if (result == DialogResult.Cancel)
                return;
            DBSmartExplorer DBSmartExplorers = new DBSmartExplorer();
            DBSmartExplorers.Action = DBFormChange.comboBox1.SelectedItem.ToString();
            DBSmartExplorers.NumberOps = DBFormChange.textBox3.Text;
            DBSmartExplorers.StatusConnection = DBFormChange.comboBox2.SelectedItem.ToString();
            DBSmartExplorers.IpConnection = DBFormChange.textBox1.Text;
            DBSmartExplorers.DateTimeAction = DBFormChange.dateTimePicker1.Value;
            db.DBSmartExplorers.Add(DBSmartExplorers);
            db.SaveChanges();
            MessageBox.Show("Новый объект добавлен");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int index = dataGridView1.SelectedRows[0].Index;
                    int id = 0;
                    bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                    if (converted == false)
                        return;
                    DBSmartExplorer DBSmartExplorer = db.DBSmartExplorers.Find(id);
                    DBFormChange DBFormChange = new DBFormChange();
                    DBFormChange.comboBox1.SelectedItem = DBSmartExplorer.Action;
                    DBFormChange.textBox3.Text = DBSmartExplorer.NumberOps;
                    DBFormChange.comboBox2.SelectedItem = DBSmartExplorer.StatusConnection;
                    DBFormChange.textBox1.Text = DBSmartExplorer.IpConnection;
                    DBFormChange.dateTimePicker1.Value = DBSmartExplorer.DateTimeAction;
                    DialogResult result = DBFormChange.ShowDialog(this);
                    if (result == DialogResult.Cancel)
                        return;
                    DBSmartExplorer.Action = DBFormChange.comboBox1.SelectedItem.ToString();
                    DBSmartExplorer.NumberOps = DBFormChange.textBox3.Text;
                    DBSmartExplorer.StatusConnection = DBFormChange.comboBox2.SelectedItem.ToString();
                    DBSmartExplorer.IpConnection = DBFormChange.textBox1.Text;
                    DBSmartExplorer.DateTimeAction = DBFormChange.dateTimePicker1.Value;
                    db.SaveChanges();
                    dataGridView1.Refresh(); // обновляем грид
                    MessageBox.Show("Объект обновлен");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка {ex}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;
                DBSmartExplorer DBSmartExplorer = db.DBSmartExplorers.Find(id);
                db.DBSmartExplorers.Remove(DBSmartExplorer);
                db.SaveChanges();
                MessageBox.Show("Объект удален");
            }
        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public void Progressincrement()
        {

            waithandler.WaitOne();
            while (CheckForInternetConnection() == true)
            {
                if (Progresscount == 100)

                {
                    break;
                }
                SetLabel("Connected");
                Progresscount += 1;

                SetProgress(Progresscount.ToString());
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            if (Progresscount < 100)
            {
                Startthread();
            }
            SetLabel("Completed");


        }

        public void Startthread()
        {

            this.demoThread = new Thread(new ThreadStart(Progressincrement));
            this.demoThread.Start();
            SetLabel("Waiting for connection");
            while (CheckForInternetConnection() == false) ;

            waithandler.Set();
        }
        private void SetLabel(string text)
        {
            // InvokeRequired required compares the thread ID of the  
            // calling thread to the thread ID of the creating thread.  
            // If these threads are different, it returns true.  
            if (this.label1.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetLabel);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.label1.Text = text;
            }
        }
        private void SetProgress(string Value)
        {
            // InvokeRequired required compares the thread ID of the  
            // calling thread to the thread ID of the creating thread.  
            // If these threads are different, it returns true.  
            if (this.progressBar1.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetProgress);
                this.Invoke(d, new object[] { Value });
            }
            else
            {
                this.progressBar1.Value = Convert.ToInt32(Value);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //Startthread();
        }
    }
}
