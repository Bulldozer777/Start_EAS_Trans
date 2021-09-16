using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Start_EAS_Trans
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Width = 844;
            Height = 518;
            //listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            AutoCompleteStringCollection source = new AutoCompleteStringCollection()
        {
                 "248000","248001","248002","248003","248007","248008","248009","248010","248012","248016","248017","248018","248021","248022","248023","248025","248028","248029","248030","248031",
                           "248032",
                           "248033",
                           "248035",
                           "248036",
                           "248037",
                           "248038",
                           "248039",
                           "248901",
                           "248903",
                           "248911",
                           "248912",
                           "248915",
                           "248918",
                           "248920",
                           "248921",
                           "248926",
                           "249000",
                           "249001",
                           "249004",
                           "249007",
                           "249008",
                           "249009",
                           "249010",
                           "249011",
                           "249013",
                           "249015",
                           "249017",
                           "249018",
                           "249020",
                           "249021",
                           "249022",
                           "249023",
                           "249024",
                           "249025",
                           "249026",
                           "249027",
                           "249028",
                           "249031",
                           "249032",
                           "249033",
                           "249034",
                           "249035",
                           "249037",
                           "249038",
                           "249039",
                           "249051",
                           "249052",
                           "249053",
                           "249054",
                           "249056",
                           "249060",
                           "249061",
                           "249062",
                           "249064",
                           "249070",
                           "249071",
                           "249073",
                           "249076",
                           "249080",
                           "249081",
                           "249082",
                           "249086",
                           "249087",
                           "249091",
                           "249092",
                           "249093",
                           "249094",
                           "249096",
                           "249100",
                           "249101",
                           "249103",
                           "249104",
                           "249105",
                           "249106",
                           "249107",
                           "249108",
                           "249109",
                           "249111",
                           "249120",
                           "249122",
                           "249123",
                           "249124",
                           "249127",
                           "249130",
                           "249134",
                           "249135",
                           "249136",
                           "249137",
                           "249139",
                           "249140",
                           "249141",
                           "249142",
                           "249143",
                           "249144",
                           "249160",
                           "249161",
                           "249162",
                           "249163",
                           "249164",
                           "249165",
                           "249166",
                           "249167",
                           "249168",
                           "249172",
                           "249173",
                           "249174",
                           "249180",
                           "249181",
                           "249183",
                           "249184",
                           "249185",
                           "249187",
                           "249191",
                           "249192",
                           "249200",
                           "249201",
                           "249205",
                           "249210",
                           "249212",
                           "249213",
                           "249214",
                           "249215",
                           "249217",
                           "249221",
                           "249222",
                           "249223",
                           "249224",
                           "249225",
                           "249230",
                           "249231",
                           "249232",
                           "249235",
                           "249236",
                           "249240",
                           "249242",
                           "249243",
                           "249246",
                           "249247",
                           "249250",
                           "249251",
                           "249252",
                           "249254",
                           "249255",
                           "249261",
                           "249264",
                           "249265",
                           "249266",
                           "249267",
                           "249268",
                           "249271",
                           "249273",
                           "249274",
                           "249275",
                           "249276",
                           "249277",
                           "249278",
                           "249280",
                           "249281",
                           "249282",
                           "249284",
                           "249286",
                           "249290",
                           "249292",
                           "249296",
                           "249300",
                           "249301",
                           "249303",
                           "249304",
                           "249305",
                           "249306",
                           "249310",
                           "249311",
                           "249312",
                           "249313",
                           "249314",
                           "249315",
                           "249320",
                           "249322",
                           "249332",
                           "249333",
                           "249340",
                           "249342",
                           "249343",
                           "249349",
                           "249350",
                           "249352",
                           "249356",
                           "249357",
                           "249360",
                           "249363",
                           "249364",
                           "249365",
                           "249370",
                           "249371",
                           "249372",
                           "249373",
                           "249376",
                           "249377",
                           "249378",
                           "249381",
                           "249382",
                           "249383",
                           "249401",
                           "249402",
                           "249403",
                           "249405",
                           "249406",
                           "249410",
                           "249411",
                           "249412",
                           "249413",
                           "249414",
                           "249415",
                           "249417",
                           "249418",
                           "249419",
                           "249425",
                           "249431",
                           "249432",
                           "249433",
                           "249434",
                           "249435",
                           "249436",
                           "249438",
                           "249440",
                           "249441",
                           "249442",
                           "249443",
                           "249444",
                           "249448",
                           "249450",
                           "249451",
                           "249452",
                           "249453",
                           "249454",
                           "249455",
                           "249456",
                           "249457",
                           "249458",
                           "249459",
                           "249500",
                           "249502",
                           "249503",
                           "249505",
                           "249510",
                           "249511",
                           "249513",
                           "249515",
                           "249516",
                           "249517",
                           "249518",
                           "249519",
                           "249600",
                           "249601",
                           "249603",
                           "249610",
                           "249611",
                           "249616",
                           "249620",
                           "249621",
                           "249622",
                           "249623",
                           "249625",
                           "249627",
                           "249630",
                           "249640",
                           "249650",
                           "249651",
                           "249652",
                           "249654",
                           "249655",
                           "249656",
                           "249658",
                           "249660",
                           "249662",
                           "249664",
                           "249665",
                           "249701",
                           "249703",
                           "249705",
                           "249706",
                           "249708",
                           "249710",
                           "249711",
                           "249712",
                           "249713",
                           "249716",
                           "249720",
                           "249722",
                           "249723",
                           "249725",
                           "249726",
                           "249730",
                           "249732",
                           "249736",
                           "249750",
                           "249757",
                           "249760",
                           "249764",
                           "249766",
                           "249771",
                           "249774",
                           "249775",
                           "249800",
                           "249802",
                           "249803",
                           "249804",
                           "249806",
                           "249808",
                           "249811",
                           "249812",
                           "249815",
                           "249831",
                           "249832",
                           "249833",
                           "249834",
                           "249840",
                           "249841",
                           "249842",
                           "249844",
                           "249845",
                           "249846",
                           "249849",
                           "249850",
                           "249851",
                           "249852",
                           "249855",
                           "249856",
                           "249857",
                           "249858",
                           "249859",
                           "249860",
                           "249861",
                           "249862",
                           "249863",
                           "249864",
                           "249865",
                           "249866",
                           "249867",
                           "249868",
                           "249870",
                           "249873",
                           "249875",
                           "249880",
                           "249882",
                           "249884",
                           "249890",
                           "249891",
                           "249892",
                           "249901",
                           "249902",
                           "249903",
                           "249910",
                           "249911",
                           "249912",
                           "249913",
                           "249920",
                           "249921",
                           "249922",
                           "249923",
                           "249924",
                           "249925",
                           "249930",
                           "249932",
                           "249933",
                           "249934",
                           "249936",
                           "249941",
                           "249942",
                           "249946",
                           "249950",
                           "249951",
                           "249953",
                           "249954",
                           "249955",
                           "249960",
                           "249961",
                           "249962",
                           "249963",
                           "249966",
                           "249967",
                           "249968",
        };
            textBox1.AutoCompleteCustomSource = source;
            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                this.timer1.Start();
                try
                {
                    DialogResult result = MessageBox.Show(
        $"Толкнуть транспорт скриптом,\nна ОПС {textBox1.Text} ? ",
        "Сообщение",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Information,
        MessageBoxDefaultButton.Button1,
        MessageBoxOptions.DefaultDesktopOnly);

                    if (result == DialogResult.Yes)
                    {
                        CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                                 // отменяет отслеживание ошибок,
                                                                 // но дает передать компоненты формы в другой поток 

                        Thread thread = new Thread(
                            () =>
                            {
                            //Action action = () =>
                            //{
                            Start_Transport(progressBar1);
                            //label22_Click(sender,e);
                            //};
                            //if (InvokeRequired)
                            //    Invoke(action);
                            //else
                            //    action(); /*реализация через делегат action*/

                            // Invoke((MethodInvoker)(() =>
                            //{
                            //    Rebut_MPK_Service(progressBar10);
                            //}));

                        });
                        thread.Start();
                    }
                    if (result == DialogResult.No)
                    {
                        this.timer1.Stop();
                        this.TopMost = false;
                    }
                    this.TopMost = true;


                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: \n{ex}");
                }
            }
            else
                MessageBox.Show("Поле для ввода номера ОПС - пустое");
        }

        async public void Start_Transport(ProgressBar progressBar)
        {
            if (textBox1.Text != "")
            {
                string server = "";
                string name_database = "";
                Name_DataBase_and_Server(textBox1.Text, out server, out name_database);
                string connectionString = $"Server={server};Database={name_database};Persist Security Info=False;User ID=sa;Password=QweAsd123;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    textBox2.Text = "Подключение открыто";
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "exec ReplicaExport 0";
                    command.Connection = connection;
                    command.CommandTimeout = 500; //увеличено время на выполнение команды
                                                  //progressBar.Value = 10;
                    string path = @"\\" + server + @"\c$\GMMQ\Export";
                    string[] files = Directory.GetFiles(path);
                    //Progress_Bar_Async(progressBar1, path);
                    progressBar.Value = files.Length;
                    if (progressBar1.Value == 98)
                    {
                        this.timer1.Stop();
                    }
                    await Task.Run(() => command.ExecuteNonQueryAsync());
                    //int i = 0;
                    //while (command.ExecuteNonQueryAsync().Status.ToString() == "Running")
                    //{
                    //    progressBar.Value += 5;
                    //    i++;
                    //}
                    textBox2.Text = "Скрипт выполнен";
                    //textBox2.TextChanged += "";


                    progressBar.Value = 100;
                    MessageBox.Show($"Запрос в SQL: \"{command.CommandText}\" - успешно отработан");
                    progressBar.Value = 0;
                    textBox9.Text = textBox1.Text + " - толкнул скриптом +";
                }
            }
            else
                MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
        }
        public void L()
        {

        }
        public void P(SqlCommand command, ProgressBar progressBar, string path)
        {
            string[] files = Directory.GetFiles(path);
            command.ExecuteNonQueryAsync();
            progressBar.Value = files.Length;
        }
        async public void Progress_Bar_Async(ProgressBar progressBar, string path)
        {
            await Task.Run(() => Progress_Bar(progressBar, path));
        }
        public void Progress_Bar(ProgressBar progressBar, string path)
        {
            string[] files = Directory.GetFiles(path);
            int qt = files.Length;

            for (int i = qt; i > 0; i--)
            {
                label2.Text = files[i - 1];
            }
            for (int i = 0; i < 90; i++)
            {
                if (progressBar.Value > 99)
                {
                    progressBar.Value = 11;
                    progressBar.Value++;
                    Thread.Sleep(1000);
                }
                else
                    break;
            }
        }            
        public void Name_DataBase_and_Server(string numder_ops, out string server, out string name_database)
        {
            name_database = "DB" + numder_ops;
            server = "";
            if (textBox1.Text != "")
            {

                try
                {
                    IPAddress ipAddress = Dns.GetHostEntry("r40-" + numder_ops + "-n").AddressList[0];
                    Ping ping = new Ping();
                    PingReply pingReply = ping.Send(ipAddress);
                    if (pingReply.Address != null)
                    {
                        if (pingReply.Address.ToString() != "10.94.187.117")
                        {
                            server = pingReply.Address.ToString();
                            System.Threading.Thread.Sleep(100);
                        }
                        else
                        {
                            MessageBox.Show($"\nКоманда пинг - не проходит.");
                            textBox9.Text = textBox1.Text + " - не подключается";
                        }
                    }
                    else
                    {
                        MessageBox.Show($"\nКоманда пинг - не проходит.");
                        textBox9.Text = textBox1.Text + " - не подключается";
                    }

                }
                catch (PingException ex)
                {
                    MessageBox.Show($"\nКоманда пинг - не проходит.\n{ex.Message}");
                }
                catch (SocketException)
                {
                    MessageBox.Show("\nКоманда пинг - не проходит.\nCould not resolve host name.");
                }

                catch (ArgumentNullException)
                {
                    MessageBox.Show("\nКоманда пинг - не проходит.\nPlease enter the host name or IP address to ping.");
                }
                catch (System.Net.NetworkInformation.NetworkInformationException)
                {
                    MessageBox.Show($"\nКоманда пинг - не проходит.\nПк ОПС {textBox1.Text} - выключен или без интернета");
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show($"\nКоманда пинг - не проходит.\nПк ОПС {textBox1.Text} - выключен или без интернета");
                }
            }
            else
                MessageBox.Show($"\nПоле для ввода номера ОПС - пустое\n(Метод Name_DataBase_and_Server)");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //textBox2.Text += command.ExecuteNonQueryAsync().Status.ToString();
        }

    
      
        public string[] Get_Files(string path)
        {
            try
            {
                FileInfo fileInf_1 = new FileInfo(path + "*.DAT");
                FileInfo fileInf_2 = new FileInfo(path + "*.FE");
                FileInfo fileInf_3 = new FileInfo(path + "*.Fi");
                if (fileInf_1.Exists)
                {
                    string[] files = new string[200];
                    files[0] = " 1 ";
                    files = Directory.GetFiles(path);
                    //Directory.GetLastAccessTime(" ")

                    if (files[0] == " 1 ")
                        return new string[1] { "0" };
                    else
                        return files;
                }
                if (fileInf_2.Exists)
                {
                    string[] files = new string[200];
                    files[0] = " 1 ";
                    files = Directory.GetFiles(path);
                    //Directory.GetLastAccessTime(" ")

                    if (files[0] == " 1 ")
                        return new string[1] { "0" };
                    else
                        return files;
                }
                if (fileInf_3.Exists)
                {
                    string[] files = new string[200];
                    files[0] = " 1 ";
                    files = Directory.GetFiles(path);
                    //Directory.GetLastAccessTime(" ")

                    if (files[0] == " 1 ")
                        return new string[1] { "0" };
                    else
                        return files;
                }
                return new string[1] { "0" };
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
                string[] files = new string[2] { "                            Папка пустая", "                            Папка пустая" };
                return files;
            }
        }
        public string Get_Files_Time(string path)
        {
            if (textBox1.Text != "")
            {
                try
                {
                    DateTime dateTime = new DateTime();
                    dateTime = Directory.GetLastAccessTime(path);
                    string files_1 = dateTime.ToString();
                    return files_1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: \n{ex}");
                    string h = "";
                    return h;
                }
            }
            else
                MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
            string e = "";
            return e;
        }

        public DateTime Get_Date_Time(string path)
        {
            if (textBox1.Text != "")
            {
                try
                {
                    DateTime dateTime = new DateTime();
                    dateTime = Directory.GetLastAccessTime(path);
                    return dateTime;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: \n{ex}");
                    DateTime h = new DateTime(2015, 7, 20, 15, 30, 25);
                    return h;
                }
            }
            else
                MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
            DateTime e = new DateTime(2015, 7, 20, 15, 30, 25);
            return e;
        }


        public void FR(string path, string server, out List<string> files_list )
        {
            
                files_list = new List<string>();
                List<string> files_list_1 = new List<string>();
                List<string> files_list_2 = new List<string>();
            if (textBox1.Text != "")
            {
                try
                {
                    CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                             // отменяет отслеживание ошибок,
                                                             // но дает передать компоненты формы в другой поток 


                    if (path == @"\\" + server + @"\c$\GMMQ\Import" | path == @"\\" + server + @"\c$\GMMQ\Export")
                    {
                        string[] catalog = Directory.GetDirectories(path);
                        if (catalog.Length != 0)
                        {
                            for (int i = 0; i < catalog.Length; i++)
                            {
                                listBox1.Items.Add(catalog[i].Substring(18 + server.Length));
                                files_list_1.Add(catalog[i].Substring(18 + server.Length));
                            }
                        }
                    }
                    else
                    {
                        string[] catalog = Directory.GetDirectories(path);
                        if (catalog.Length != 0)
                        {
                            for (int i = 0; i < catalog.Length; i++)
                            {
                                listBox1.Items.Add(catalog[i].Substring(42));
                                files_list_2.Add(catalog[i].Substring(42));
                            }
                        }
                    }
                    string[] mask = new string[6] { "*.dat", "*.fi", "*.fe", "*.lock", "*.xml", "*.end" };
                    for (int i = 0; i < mask.Length; i++)
                    {
                        Find(path, mask[i]);
                    }
                    for (int i = 0; i < files_list_1.Count + files_list_2.Count; i++)
                    {
                        if (i < files_list_1.Count)
                            files_list.Add(files_list_1[i]);
                        if (i >= files_list_1.Count & i < files_list_1.Count + files_list_2.Count)
                            files_list.Add(files_list_2[i]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"\nОшибка: \n{ex.Message}");
                }
            }
            else
            MessageBox.Show($"\nПоле для ввода номера ОПС - пустое\n(Метод FR)");
        }
        public void Find(string path, string mask)
        {
            if (textBox1.Text != "")
            {
                CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                         // отменяет отслеживание ошибок,
                                                         // но дает передать компоненты формы в другой поток 
                DirectoryInfo dir = new DirectoryInfo(path);
                foreach (FileInfo file in dir.GetFiles(mask))
                {
                    listBox1.Items.Add(file.ToString());
                }
            }
            else
            MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
        }
        async private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                         // отменяет отслеживание ошибок,
                                                         // но дает передать компоненты формы в другой поток 
                string server = "";
                string name_database = "";
                Name_DataBase_and_Server(textBox1.Text, out server, out name_database);
                string path = @"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox1.Text + @"\In";
                listBox1.Items.Clear();
                await Task.Run(() => FR(path, server, out List<string> files_list));
                string[] files = Directory.GetFiles(path);
                label11.Text = files.Length.ToString();
                string[] catalog = Directory.GetDirectories(path);
                label15.Text = catalog.Length.ToString();
                label21.Text = $"Всего элементов: {files.Length + catalog.Length}";
                }
                else
                    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex.Message}");
            }
            //listBox1.Items.AddRange(Get_Files(path).Select(line => line.Substring(31))
            //.ToArray());
        }

        async private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    string server = "";
                string name_database = "";
                Name_DataBase_and_Server(textBox1.Text, out server, out name_database);
                string path = @"\\" + server + @"\c$\GMMQ\Import";
                listBox1.Items.Clear();
                await Task.Run(() => FR(path, server, out List<string> files_list));
                string[] files = Directory.GetFiles(path);
                label11.Text = files.Length.ToString();
                string[] catalog = Directory.GetDirectories(path);
                label15.Text = catalog.Length.ToString();
                label21.Text = $"Всего элементов: {files.Length + catalog.Length}";
                }
                else
                    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex.Message}");
            }
            //    listBox1.Items.AddRange(Get_Files(path).Select(line => line.Substring(11 + server.Length))
            //.ToArray());
        }
        async private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    string server = "";
                    string name_database = "";
                    Name_DataBase_and_Server(textBox1.Text, out server, out name_database);
                    string path = @"\\" + server + @"\c$\GMMQ\Export";
                    listBox1.Items.Clear();
                    await Task.Run(() => FR(path, server, out List<string> files_list));
                    string[] files = Directory.GetFiles(path);
                    label11.Text = files.Length.ToString();
                    string[] catalog = Directory.GetDirectories(path);
                    label15.Text = catalog.Length.ToString();
                    //while(files.Length > 0)
                    //{
                    //    listBox1.BeginUpdate();
                    //    FR(path, server, out List<string> files_list);
                    //    listBox1.EndUpdate();
                    //}
                    label21.Text = $"Всего элементов: {files.Length + catalog.Length}";
                    //button2_Click(sender, e);
                    //await Task.Delay(500);
                }
                else
                    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex.Message}");
            }
        }

        async private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    string server = "";
                    string name_database = "";
                    Name_DataBase_and_Server(textBox1.Text, out server, out name_database);
                    string path = @"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox1.Text + @"\Out";
                    listBox1.Items.Clear();
                    await Task.Run(() => FR(path, server, out List<string> files_list));
                    string[] files = Directory.GetFiles(path);
                    label11.Text = files.Length.ToString();
                    string[] catalog = Directory.GetDirectories(path);
                    label15.Text = catalog.Length.ToString();
                    label21.Text = $"Всего элементов: {files.Length + catalog.Length}";
                }
                else
                    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex.Message}");
            }
        }

        async private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    string server = "";
                string name_database = "";
                Name_DataBase_and_Server(textBox1.Text, out server, out name_database);
                string path = @"\\" + server + @"\c$\GMMQ\Export";
                textBox4.Clear();
                await Task.Run(() => Get_Files_Time(path));
                textBox4.Text = "Export - " + (Get_Files_Time(path));
                }
                else
                    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex.Message}");
            }
        }

        async private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    string server = "";
                string name_database = "";
                Name_DataBase_and_Server(textBox1.Text, out server, out name_database);
                string path = @"\\" + server + @"\c$\GMMQ\Import";
                textBox5.Clear();
                await Task.Run(() => Get_Files_Time(path));
                textBox5.Text = "Import - " + (Get_Files_Time(path));
                }
                else
                    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex.Message}");
            }
        }

        async private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    string server = "";
                string name_database = "";
                Name_DataBase_and_Server(textBox1.Text, out server, out name_database);
                string path = @"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox1.Text + @"\In";
                textBox7.Clear();
                await Task.Run(() => Get_Files_Time(path));
                textBox7.Text = "In - " + (Get_Files_Time(path));
                }
                else
                    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex.Message}");
            }
        }

        async private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    string server = "";
                    string name_database = "";
                    Name_DataBase_and_Server(textBox1.Text, out server, out name_database);
                    string path = @"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox1.Text + @"\Out";
                    textBox6.Clear();
                    await Task.Run(() => Get_Files_Time(path));
                    textBox6.Text = "Out - " + (Get_Files_Time(path));
                }
                else
                    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex.Message}");
            }
        }

        async private void button10_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                try
                {
                    string server = "";
                    string name_database = "";
                    Name_DataBase_and_Server(textBox1.Text, out server, out name_database);
                    string path = @"\\" + server + @"\c$\GMMQ\Export";
                    string path_1 = @"\\" + server + @"\c$\GMMQ\Import";
                    textBox4.Clear();
                    await Task.Run(() => Get_Files_Time(path));
                    textBox4.Text = "Export - " + (Get_Files_Time(path));
                    textBox5.Clear();
                    await Task.Run(() => Get_Files_Time(path_1));
                    textBox5.Text = "Import - " + (Get_Files_Time(path_1));
                    string path_2 = @"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox1.Text + @"\In";
                    string path_3 = @"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox1.Text + @"\Out";
                    textBox7.Clear();
                    await Task.Run(() => Get_Files_Time(path_2));
                    textBox7.Text = "In - " + (Get_Files_Time(path_2));
                    textBox6.Clear();
                    await Task.Run(() => Get_Files_Time(path_3));
                    textBox6.Text = "Out - " + (Get_Files_Time(path_3));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"\nОшибка: \n{ex.Message}");
                }
            }
            else
                MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
        }

        async private void button11_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                try
                {
                    CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                             // отменяет отслеживание ошибок,
                                                             // но дает передать компоненты формы в другой поток 
                    Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
                    await Task.Run(() => textBox8.Text = server);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"\nОшибка: \n{ex.Message}");
                }
            }
            else
                MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
        }

         private void button12_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (Width != 844)
                {
                    Width = 844;
                }
                else
                    Width = 1215;
                Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
                label20.Text = "IP Адрес удленного ПК: " + server;
            }
            else
                MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
        }

       async public void Async_View_Files(ListBox listBox, Label all_files, Label all_catalogs, Label all_elemetnts, string path)
        {
            string server = "";
            string name_database = "";
            Name_DataBase_and_Server(textBox1.Text, out server, out name_database);
            listBox.Items.Clear();
            await Task.Run(() => FR(path, server, out List<string> files_list));
            string[] files = Directory.GetFiles(path);
            all_files.Text = files.Length.ToString();
            string[] catalog = Directory.GetDirectories(path);
            all_catalogs.Text = catalog.Length.ToString();
            //while(files.Length > 0)
            //{
            //    listBox1.BeginUpdate();
            //    FR(path, server, out List<string> files_list);
            //    listBox1.EndUpdate();
            //}
            all_elemetnts.Text = $"Всего элементов: {files.Length + catalog.Length}";
        }

      
        public void Delete_File(string path)
        {
            if (textBox1.Text != "")
            {
                try
                {
                    string[] files = Directory.GetFiles(path);
                    string[] catalog_1 = Directory.GetDirectories(path);
                    if (files.Length + catalog_1.Length != 0)
                    {
                        string[] catalog = Directory.GetDirectories(path);
                        if (catalog.Length != 0)
                        {
                            for (int i = 0; i < catalog.Length; i++)
                            {
                                DirectoryInfo dir = new DirectoryInfo(path + catalog[i]);
                                //foreach (DirectoryInfo file in dir.GetDirectories(path + catalog[i]))
                                //{
                                dir.Delete(true); //значение true для удаления папки со всеми содержащимися в ней файлами
                                                  //}
                            }
                        }
                        string[] mask = new string[6] { "*.dat", "*.fi", "*.fe", "*.lock", "*.xml", "*.end" };
                        for (int i = 0; i < mask.Length; i++)
                        {
                            Delete_File(path, mask[i]);
                        }
                        //MessageBox.Show($"\nФайлы из папки Export - удалены");
                    }
                    else
                        MessageBox.Show($"\nПапка пустая, элементов в папке - 0\nудаление - не произведено");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"\nОшибка: \n{ex.Message}");
                }
            }
            else
                MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
        }

      

      async private void button17_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                         // отменяет отслеживание ошибок,
                                                         // но дает передать компоненты формы в другой поток 
                DateTime now = new DateTime();
                now = DateTime.Now;
                Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
                Get_Date_Time(@"\\" + server + @"\c$\GMMQ\Export");
                DateTime export = Get_Date_Time(@"\\" + server + @"\c$\GMMQ\Export").AddHours(3);
                DateTime inport = Get_Date_Time(@"\\" + server + @"\c$\GMMQ\Import").AddHours(3);
                DateTime In = Get_Date_Time(@"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox1.Text + @"\In").AddHours(3);
                DateTime Out = Get_Date_Time(@"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox1.Text + @"\Out").AddHours(3);
                //if (now < export & now < inport & now < In & now < Out)
                    if (now < In & now < Out)
                {
                    textBox3.Text = "Время обновления папок актуальное";
                    await Task.Delay(200);
                    if (server != "")
                    {
                        if (textBox1.Text != "248000" & textBox1.Text != "249406")
                        {
                            string ip = server;
                            string result = "";
                            await Task.Run(() => Power_Shell_1("get-service -" +
                                               "DisplayName \"GMMQ\"" +
                                               " -ComputerName " + ip + "" +
                                               " | format-table Status -autosize", out result));
                            textBox3.Text = "Служба GMMQ - " + result.Replace("\r\n", "");
                            await Task.Delay(400);
                            if (result.Replace("\r\n", "") == "Running")
                            {
                                if (server != "")
                                {
                                    await Task.Run(() => Power_Shell_1("get-service -" +
                                                       "DisplayName \"GM_SchedulerSvc\"" +
                                                       " -ComputerName " + ip + "" +
                                                       " | format-table Status -autosize", out result));
                                    textBox3.Text = "Служба GM_Scheduler - " + result.Replace("\r\n", "");
                                    await Task.Delay(500);
                                    if (result.Replace("\r\n", "") == "Running")
                                    {
                                        textBox3.Text = "Службы транспорта работают";
                                        await Task.Delay(1000);
                                        textBox3.Text = "Транспорт в рабочем состоянии";
                                    }
                                    else
                                    {
                                        MessageBox.Show("Время обновления файлов в папках более 3 часов\nТранспорт не работает более 3 часов");
                                        textBox3.Text = "Транспорт не работает более 3 часов";
                                    }
                                }
                                else
                                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
                            }
                            else
                            {
                                MessageBox.Show("Служба GMMQ - не работает");
                                textBox3.Text = "Транспорт не работает";
                            }
                        }
                        else
                            await Task.Run(() => textBox3.Text = "Транспорт в рабочем состоянии");
                    }
                    else
                        MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
                }
                else
                {
                    MessageBox.Show("Время обновления файлов в папках:\nExport, Import на ОПС и In, Out в DAX - более 3 часов\nТранспорт не работает более 3 часов");
                    textBox3.Text = "Транспорт не работает";
                }
            }
            else
                MessageBox.Show("Поле для ввода номера ОПС - пустое");        
    }
        public void Delete_File(string path, string mask)
        {
            if (textBox1.Text != "")
            {
                try
                {
                    CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                             // отменяет отслеживание ошибок,
                                                             // но дает передать компоненты формы в другой поток 
                    DirectoryInfo dir = new DirectoryInfo(path);
                    foreach (FileInfo file in dir.GetFiles(mask))
                    {
                        file.Delete();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"\nОшибка: \n{ex.Message}");
                }
            }
            else
                MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");

        }
        async private void button16_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                try
                {

                    Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
                    string path = @"\\" + server + @"\c$\GMMQ\Export";
                    DialogResult result = MessageBox.Show(
           $"Удалить файлы ОПС {textBox1.Text}\nиз папки \\" + server + @"\c$\GMMQ\Export?",
           "Сообщение",
           MessageBoxButtons.YesNo,
           MessageBoxIcon.Information,
           MessageBoxDefaultButton.Button1,
           MessageBoxOptions.DefaultDesktopOnly);

                    if (result == DialogResult.Yes)
                    {
                        await Task.Run(() => Delete_File(path));
                        Async_View_Files(listBox1, label11, label15, label21, path);
                    }
                    if (result == DialogResult.No)
                    {
                        this.TopMost = false;
                        //Async_View_Files(listBox1, label11, label15, label21, path);
                    }
                    this.TopMost = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"\nОшибка: \n{ex.Message}");
                }
            }
            else
                MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
        }
        async private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
                string path = @"\\" + server + @"\c$\GMMQ\Import";

                    DialogResult result = MessageBox.Show(
           $"Удалить файлы ОПС {textBox1.Text}\nиз папки \\" + server + @"\c$\GMMQ\Import?",
           "Сообщение",
           MessageBoxButtons.YesNo,
           MessageBoxIcon.Information,
           MessageBoxDefaultButton.Button1,
           MessageBoxOptions.DefaultDesktopOnly);

                    if (result == DialogResult.Yes)
                    {
                        await Task.Run(() => Delete_File(path));
                        Async_View_Files(listBox1, label11, label15, label21, path);
                    }
                    if (result == DialogResult.No)
                    {
                        //Async_View_Files(listBox1, label11, label15, label21, path);
                    }
                    this.TopMost = true;
                }
                else
                    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
            }
            catch (Exception ex)

            {
                MessageBox.Show($"\nОшибка: \n{ex.Message}");
            }
        }

        async private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    string path = @"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox1.Text + @"\In";
                    DialogResult result = MessageBox.Show(
       $"Удалить файлы ОПС {textBox1.Text}\nиз папки \\KAL\\{textBox1.Text}\\In?",
       "Сообщение",
       MessageBoxButtons.YesNo,
       MessageBoxIcon.Information,
       MessageBoxDefaultButton.Button1,
       MessageBoxOptions.DefaultDesktopOnly);

                    if (result == DialogResult.Yes)
                    {                   
                        await Task.Run(() => Delete_File(path));
                        Async_View_Files(listBox1, label11, label15, label21, path);
                    }
                    if (result == DialogResult.No)
                    {
                        //Async_View_Files(listBox1, label11, label15, label21, path);
                    }
                    this.TopMost = true;
                }
                else
                    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex.Message}");
            }
        }

        async private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    string path = @"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox1.Text + @"\Out";
                    DialogResult result = MessageBox.Show(
        $"Удалить файлы ОПС {textBox1.Text}\nиз папки \\KAL\\{textBox1.Text}\\Out?",
        "Сообщение",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Information,
        MessageBoxDefaultButton.Button1,
        MessageBoxOptions.DefaultDesktopOnly);

                    if (result == DialogResult.Yes)
                    {
                        await Task.Run(() => Delete_File(path));
                        Async_View_Files(listBox1, label11, label15, label21, path);
                    }
                    if (result == DialogResult.No)
                    {
                        //Async_View_Files(listBox1, label11, label15, label21, path);
                    }
                    this.TopMost = true;

                }
                else
                    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex.Message}");
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (Width != 844)
            {
                Width = 844;
            }
            else
                Width = 1200;
        }
        // Остановка служб GMMQ и Sheduller 
        async private void button19_Click_1(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            try
            {
                Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
                if (server != "")
                {
                    string result = "";
                    string name_service = "GMMQ";
                    string ip = server;
                    progressBar2.Value = 40;
                    string action_service = "Stopped";
                    await Task.Run(() => Powershell_service_Force(ip, action_service, name_service, ""));

                    //Служба GMMQ останавливается с помощью другой команды т к имеет зависимые службы,
                    //поэтому для нее особы метод создан

                    await Task.Run(() => Power_Shell_1("get-service -" +
                                       "DisplayName \"GMMQ\"" +
                                       " -ComputerName " + server + "" +
                                       " | format-table Status -autosize", out result));

                    //запрос на состояние службы и потом проверка,
                    //работа с службой GMMQ специально разделена на 2 метода
                    //первый метод Powershell_service_Force - не возвращает после отработки состояние службы, только ее останавливает
                    //второй метод Power_Shell_1 - получает просто состояние службы в переменную, записанную в выходные параметры метода
                    //далее логика работы программы, в зависимости от полученного значения состояния, из метода Power_Shell_1

                    textBox10.Text = result.Replace("\r\n", "");
                    if (result == $"\r\n{action_service}\r\n\r\n\r\n")
                    {
                        progressBar2.Value = 100;
                        MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service)}");
                        progressBar2.Value = 0;
                        textBox10.Text = result.Replace("\r\n", "");
                    }
                    if (result == "Блок еlse")
                    {
                        MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - {Action_service_info(action_service)}");
                    }
                    if (result == "Нет данных")
                    {
                        progressBar2.Value = 0;
                    }
                }
                else
                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        async private void button28_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            try
            {
                Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
                if (server != "")
                {
                    string ip = server;
                    string name_service = "GM_SchedulerSvc";
                    progressBar3.Value = 40;
                    string action_service = "Stopped";
                    string result = "";
                    await Task.Run(() => Power_Shell("Get-Service -Computer " + ip + " -Name " + name_service + " | Stop-Service -Force"));
                    await Task.Run(() => Power_Shell_1("get-service -" +
                                      "DisplayName \"GM_SchedulerSvc\"" +
                                      " -ComputerName " + ip + "" +
                                      " | format-table Status -autosize", out result));
                    textBox11.Text = result.Replace("\r\n", "");
                    if (result == $"\r\n{action_service}\r\n\r\n\r\n")
                    {
                        progressBar3.Value = 100;
                        MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service)}");
                        progressBar3.Value = 0;
                        textBox11.Text = result.Replace("\r\n", "");
                    }
                    if (result == "Блок еlse")
                    {
                        MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - {Action_service_info(action_service)}");
                    }
                    if (result == "Нет данных")
                    {
                        progressBar3.Value = 0;
                    }
                }
                else
                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        //Метод представляющий процесс, запускающий power_shell с командами, передаваемыми через параметр string action_power_shell
        //Метод ожидает своей отработки, т е пока поток не отработает, основная программа не будет выполнять код, ниже которого вызван этот метод
        public void Power_Shell(string action_power_shell)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = $"/command {action_power_shell}",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            }).WaitForExit();
        }
        public void Powershell_service_Force(string ip, string action, string name_service, string action_more)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = "/command Get-Service -Computer " + ip + " -Name " + name_service + " | Stop-Service -Force" + action_more,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            }).WaitForExit();
        }

        // Запуск "GMMQ" и "GM_SchedulerSvc"
        async private void button20_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            try
            {
                Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
                if (server != "")
                {
                    string ip = server;
                    string name_service = "GMMQ";
                    string action_service = "Running";
                    progressBar2.Value = 40;
                    await Task.Run(() => Async_Power_Shell_Service(ip, name_service, action_service, progressBar2, textBox10));
                }
                else
                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        async private void button27_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            try
            {
                Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
                if (server != "")
                {
                    string ip = server;
                    string result = "";
                    string name_service = "GM_SchedulerSvc";
                    string action_service = "Running";
                    progressBar3.Value = 40;
                    await Task.Run(() => Power_Shell($"set-service {name_service} -ComputerName {ip} -Status {action_service} -PassThru | format-table Status -autosize"));
                    await Task.Run(() => Power_Shell_1("get-service -" +
                                      "DisplayName \"GM_SchedulerSvc\"" +
                                      " -ComputerName " + ip + "" +
                                      " | format-table Status -autosize", out result));
                    textBox11.Text = result.Replace("\r\n", "");
                    if (result == $"\r\n{action_service}\r\n\r\n\r\n")
                    {
                        progressBar3.Value = 100;
                        MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service)}");
                        progressBar3.Value = 0;
                        textBox11.Text = result.Replace("\r\n", "");
                    }
                    if(result == "Нет данных")
                    {
                        progressBar3.Value = 0;
                    }
                    else if (result == "Блок еlse")
                        MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - {Action_service_info(action_service)}");
                }
                else
                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        // Состояние служб
        async private void button22_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
            if (server != "")
            {
                string ip = server;
                string result = "";
                await Task.Run(() => Power_Shell_1("get-service -" +
                                   "DisplayName \"GMMQ\"" +
                                   " -ComputerName " + ip + "" +
                                   " | format-table Status -autosize", out result));
                textBox10.Text = result.Replace("\r\n", "");
            }
            else
                MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
        }

        async private void button25_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
            if (server != "")
            {
                string ip = server;
                string result = "";
                await Task.Run(() => Power_Shell_1("get-service -" +
                                   "DisplayName \"GM_SchedulerSvc\"" +
                                   " -ComputerName " + ip + "" +
                                   " | format-table Status -autosize", out result));
                textBox11.Text = result.Replace("\r\n", "");
                if (result == "Нет данных")
                {
                    textBox11.Text = result;
                }
            }
            else
                MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
        }

        // Перезапуск 
        async private void button21_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            try
            {
                Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
                if (server != "")
                {
                    string ip = server;
                    string result = "";
                    string name_service = "GMMQ";
                    progressBar2.Value = 40;
                    string action_service_1 = "Stopped";
                    string action_service_2 = "Running";
                    await Task.Run(() => Powershell_service_Force(ip, action_service_1, name_service, ""));
                    progressBar2.Value = 70;
                    await Task.Run(() => Power_Shell_1($"set-service {name_service} " +
                     $"-ComputerName {ip} " +
                     $"-Status {action_service_2} -PassThru " +
                     "| format-table Status -autosize", out result));
                    progressBar2.Value = 90;
                    if (result == $"\r\n{action_service_2}\r\n\r\n\r\n")
                    {
                        progressBar2.Value = 100;
                        MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Перезапущена");
                        progressBar2.Value = 0;
                        textBox10.Text = result.Replace("\r\n", "");
                    }
                    if (result == "Блок еlse")
                    {
                        MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Перезапущена");
                    }
                    if (result == "Нет данных")
                    {
                        textBox10.Text = result;
                        progressBar2.Value = 0;
                    }
                }
                else
                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        async private void button26_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            try
            {
                Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
                if (server != "")
                {
                    string ip = server;
                    string result = "";
                    string name_service = "GM_SchedulerSvc";
                    progressBar3.Value = 40;
                    string action_service_1 = "Stopped";
                    string action_service_2 = "Running";
                    await Task.Run(() => Powershell_service_Force(ip, action_service_1, name_service, ""));
                    progressBar3.Value = 60;
                    await Task.Run(() => Power_Shell($"set-service {name_service} " +
                     $"-ComputerName {ip} " +
                     $"-Status {action_service_2} -PassThru " +
                     "| format-table Status -autosize"));
                    progressBar3.Value = 70;
                    await Task.Run(() => Power_Shell_1("get-service -" +
                               "DisplayName \"GM_SchedulerSvc\"" +
                               " -ComputerName " + ip + "" +
                               " | format-table Status -autosize", out result));

                    //используется 3 метода, т к программа не успевает отлавливать состояние службы, т к запуск ее долгий

                    textBox11.Text = result.Replace("\r\n", "");
                    progressBar3.Value = 90;
                    if (result == $"\r\n{action_service_2}\r\n\r\n\r\n")
                    {
                        progressBar3.Value = 100;
                        MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Перезапущена");
                        progressBar3.Value = 0;
                        textBox11.Text = result.Replace("\r\n", "");
                    }
                    if (result == "Блок еlse")
                    {
                        MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Перезапущена");
                    }
                    if(result == "Нет данных")
                    {
                        textBox11.Text = result;
                        progressBar3.Value = 0;
                    }
                }
                else
                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }
        public void Power_Shell_1(string command_power_shell, out string result)
        {
            try
            {
                Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"/command {command_power_shell}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                });
                result = process.StandardOutput.ReadToEnd().Substring(18);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка из метода Power_Shell_1 \nПк выключен или нет интернета\nИли запрашиваемая служба отсутствует на данном ПК\n\n\nОписание:\n\n{ex}");
                result = "Нет данных";
            }
        }
        public void Async_Power_Shell_Service(string ip, string name_service, string action_service, ProgressBar progressBar, TextBox textBox)
        {
            string check_action;
            if (textBox1.Text != "")
            {
                try
                {
                    Powershell_service(ip, action_service, name_service, "", out check_action);
                    Thread.Sleep(500);
                    if (check_action == $"\r\n{action_service}\r\n\r\n\r\n")
                    {
                        progressBar.Value = 100;
                        MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service)}");
                        progressBar.Value = 0;
                        textBox.Text = check_action.Replace("\r\n", "");
                    }
                    if (check_action == "Блок еlse")
                    {
                        MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - {Action_service_info(action_service)}");
                        progressBar.Value = 0;
                    }
                    if (check_action == "Нет данных")
                    {
                        textBox.Text = check_action;
                        progressBar.Value = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка из метода Async_Power_Shell_Service \nПк выключен или нет интернета\nИли запрашиваемая служба отсутствует на данном ПК\n\n\nОписание:\n\n{ex}");
                }
            }
            MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
        }

        //Метод для вывода состояния службы в MessageBox, исходя из значения переменной action_service
        public string Action_service_info(string action_service)
        {
            if (action_service == "Stopped")
                return "Остановлена";
            if (action_service == "Running")
                return "Запущена";
            else
                return $"\nОшибка: action_service = {action_service}\nМетод Async_Power_Shell_Service далее Action_service_info";
        }

        //Метод представляющий процесс, запускающий power_shell с командами,
        //редактируемыми по параметрам метода и служащими для управления службами на удаленных пк, в сетке, где находится сама программа
        //Удаленные пк задаются в виде их ip (айпи адреса), в самом Power_shell, в его команде, после объявления командлета "-ComputerName" 
        public void Powershell_service(string ip, string action, string name_service, string action_more, out string p)
        {
            p = "";
            try
            {
                Process process1 = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = "/command set-service " + name_service + " -ComputerName " + ip + " -Status " + action + " -PassThru | format-table Status -autosize" + action_more,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                });
                Thread.Sleep(500);
                string h = process1.StandardOutput.ReadToEnd();
                string k = h.Substring(18);
                Thread.Sleep(500);
                if (textBox1.Text.Length == 6 & k != "\r\nStopped\r\n\r\n\r\n" & k != "\r\nRunning\r\n\r\n\r\n")
                {
                    p = "Нет данных";
                }
                if (k == "\r\nStopped\r\n\r\n\r\n")
                {
                    p = k;
                }
                if (k == "\r\nRunning\r\n\r\n\r\n")  //else выполниться, исходя из значения последнего невыполненого if,
                                                     //на значения других if конструкция не распространяется
                {
                    p = k;
                }
                else
                    p = "Блок еlse";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка из метода Powershell_service \nПк выключен или нет интернета\nИли запрашиваемая служба отсутствует на данном ПК\n\n\nОписание:\n\n{ex}");
                p = "Нет данных";
            }
        }
        private void button18_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 6 & Width > 1000)
            { 
                Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
                label20.Text = "IP Адрес удленного ПК: " + server;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value < 98)
            {
                this.progressBar1.Increment(1);
            }
            else
                this.timer1.Stop();
            //else
            //    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
        }

      async  private void label22_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
            string path = @"\\" + server + @"\c$\GMMQ\Export";
            string[] files = Directory.GetFiles(path);
            string[] mass_check = new string[1];
            //int j = 0;
            if (files.Length > 0)
            {
                DateTime dateTime = new DateTime();
                dateTime = Directory.GetLastAccessTime(path);
                if (dateTime == DateTime.Now)
                {
                    //if (files.Length > mass_check.Length)
                    //{
                    mass_check = new string[files.Length];
                    for (int i = 0; i < files.Length; i++)
                    {
                        label22.Text = files[i];
                        await Task.Delay(200);
                    }
                }
                //}
            }
            else
                await Task.Delay(500);
            //label22_Click(sender, e);

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
        //private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //button2_Click(sender, e);
        //}
    }
}
