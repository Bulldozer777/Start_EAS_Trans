using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Smart_explorer
{
    public partial class Form6 : Form
    {
        string number_ops;
        string server;
        public Form6()
        {
            InitializeComponent();
        }
        public Form6(string number_ops, string server)
        {
            InitializeComponent();
            label3.Text = "Номер ОПС: " + number_ops;
            this.number_ops = number_ops;
            this.server = server;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now; ;
            string EAS_DateTime_Now = dateTime.ToString("MMdd");
            textBox12.Text = EAS_DateTime_Now;
        }

       async private void button1_Click(object sender, EventArgs e)
        {
            string server = this.server;
            string name_database = "sdo02";
            string connectionString = $"Server={server};Database={name_database};Persist Security Info=False;User ID=sa;Password=QweAsd123;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                textBox1.Text = "Подключение открыто";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                await connection.OpenAsync();
                command.CommandText = $"update [sdo02].[dbo].[sdo_cdb_messages] set send_at = null where form_id = '{number_ops}.2021{textBox12.Text}'";
                command.Connection = connection;
                command.CommandTimeout = 5000; //увеличено время на выполнение команды
                await Task.Run(() => command.ExecuteNonQueryAsync());
                textBox1.Text = "Скрипт выполнен";
                MessageBox.Show("Скрипт выполнен\nДата 2021"+ textBox12.Text + "\nОПС: " + number_ops +"\n");
            }
        }
    }
}
