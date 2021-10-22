using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Smart_explorer
{
    public partial class Form1 : Form
    {
        static DateTime date = DateTime.Now;
        static string NewDateFormat = date.ToString("yyyy-MM-dd");
        public string ProdwritePath = @"C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Smart explorer\Smart explorer\result\result " + NewDateFormat + ".txt";
        int Ping = 0;
        public Form1()
        {
            InitializeComponent();
            Width = 642;
            Height = 150;
            this.ActiveControl = button11;
            textBox11.Focus();
            AutoCompleteStringCollection source = new AutoCompleteStringCollection() { };
            source.AddRange(ReadText(@"C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Start_EAS_Trans\save\eas all ops.txt"));
            textBox11.AutoCompleteCustomSource = source;
            textBox11.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox11.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }
        //Функция для считывания текста из текстового файла по строкам и записи его в массив, возвращает массив строк
        public string[] ReadText(string path)
        {
            string[] array;
            using (StreamReader fs = new StreamReader(path))
            {
                array = fs.ReadToEnd().Split().ToArray();
                int counter = 0;
                while (true)
                {
                    counter++;
                    string temp = fs.ReadLine();
                    if (temp == null) break;
                    array[counter] = temp;
                }
            }
            return array;
        }
        private void button11_Click(object sender, EventArgs e)
        {
            Name_DataBase_and_Server(textBox11.Text, out string server, out string status);
            ViewSmartExplorer();
        }
        private void ViewSmartExplorer()
        {           
            if (textBox11.Text.Length == 6 & Ping == 1)
            {
                textBox1.Text = @"\\" + textBox12.Text + @"\c$\Program Files (x86)\RussianPost\PostOffice";
                textBox3.Text = @"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost";
                textBox4.Text = @"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost\nsi02\data";
                textBox2.Text = @"\\" + textBox12.Text + @"\c$\Program Files(x86)\Microsoft Dynamics AX\60\Retail POS";
                textBox7.Text = @"\\" + textBox12.Text + @"\c$\Program Files(x86)\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA";
                textBox8.Text = @"\\" + textBox12.Text + @"\c$\GMMQ\Export";
                textBox9.Text = @"\\" + textBox12.Text + @"\c$\GMMQ\Import";
                textBox5.Text = @"\\10.94.1.110\c$\UFPS\EAS\ExportKatalog\Arhiv";
                textBox10.Text = @"\\D01EASFTPBKP01\Backup_in\KAL\";
                textBox14.Text = @"\\10.94.1.166\Download\";
                textBox15.Text = @"\\" + textBox12.Text + @"\c$\Program Files (x86)\Microsoft Dynamics AX\60\PosServices\GMMQ";
                textBox16.Text = @"\\" + textBox12.Text + @"\d$";
                textBox17.Text = @"\\" + textBox12.Text + @"\c$";
                textBox6.Text = @"\\10.94.1.110\c$\Users";
                textBox25.Text = @"ftp://r16ftp00.main.russianpost.ru/Obmen/";
                textBox18.Text = "Л: podpiska П: 1LKd2Oi2msO!1";
            }
            else if(Ping == 1)
            {
                textBox2.Text = @"\\" + textBox12.Text + @"\c$\Program Files(x86)\Microsoft Dynamics AX\60\Retail POS";
                textBox7.Text = @"\\" + textBox12.Text + @"\c$\Program Files(x86)\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA";
                textBox8.Text = @"\\" + textBox12.Text + @"\c$\GMMQ\Export";
                textBox9.Text = @"\\" + textBox12.Text + @"\c$\GMMQ\Import";
                textBox5.Text = @"\\10.94.1.110\c$\UFPS\EAS\ExportKatalog\Arhiv";
                textBox10.Text = @"\\D01EASFTPBKP01\Backup_in\KAL\";
                textBox14.Text = @"\\10.94.1.166\Download\";
                textBox15.Text = @"\\" + textBox12.Text + @"\c$\Program Files (x86)\Microsoft Dynamics AX\60\PosServices\GMMQ";
                textBox16.Text = @"\\" + textBox12.Text + @"\d$";
                textBox17.Text = @"\\" + textBox12.Text + @"\c$";
                textBox6.Text = @"\\10.94.1.110\c$\Users";
                textBox25.Text = @"ftp://r16ftp00.main.russianpost.ru/Obmen/";
                textBox18.Text = "Л: podpiska П: 1LKd2Oi2msO!1";
            }
        }
        public void Name_DataBase_and_Server(string numder_ops, out string server, out string status)
        {
            status = "";
            server = "";
            textBox12.Clear();
            textBox13.Clear();
            if (textBox11.Text != "")
            {
                try
                {
                    if (numder_ops != "222222")
                    {
                        IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
                        if ( numder_ops.Length > 6)
                        {
                            ipAddress = Dns.GetHostEntry("r40-" + numder_ops).AddressList[0];
                        }
                        if (numder_ops.Length == 6)
                        {
                            ipAddress = Dns.GetHostEntry("r40-" + numder_ops + "-n").AddressList[0];
                        }
                        Ping ping = new Ping();
                        PingReply pingReply = ping.Send(ipAddress);
                        textBox13.Text = pingReply.Status.ToString();
                        if (pingReply.Address != null)
                        {
                            if (pingReply.Address.ToString() != "10.94.187.117"
                                & pingReply.Address.ToString() != "10.94.209.149"
                                & pingReply.Address.ToString() != "10.94.187.101"
                                & pingReply.Address.ToString() != "10.94.185.21"
                                & pingReply.Address.ToString() != "10.94.225.101"
                                & pingReply.Address.ToString() != "10.94.205.197"
                                & pingReply.Address.ToString() != "10.94.185.85"
                                & pingReply.Address.ToString() != "10.94.218.53"
                                 & pingReply.Address.ToString() != "10.94.206.69")
                            {

                                if (pingReply.Status.ToString() != "Success")
                                {
                                    textBox12.Text = $"ОПС {textBox11.Text} не подключается";

                                }
                                else
                                {
                                    this.Ping = 1;
                                    status = pingReply.Status.ToString();
                                    server = pingReply.Address.ToString();
                                    textBox12.Text = pingReply.Address.ToString();
                                    System.Threading.Thread.Sleep(200);
                                }
                            }
                            else
                            {
                                MessageBox.Show($"\nКоманда пинг - не проходит.");
                                server = "";
                            }
                        }
                        else
                        {
                            MessageBox.Show($"\nКоманда пинг - не проходит.");
                            server = "";
                        }
                    }
                    else
                        server = @"D:\!localhost";
                }
                catch (PingException ex)
                {
                    MessageBox.Show($"\nКоманда пинг - не проходит.\n{ex.Message}");
                    server = "";
                }
                catch (SocketException)
                {
                    MessageBox.Show("\nКоманда пинг - не проходит.\nCould not resolve host name.");
                    server = "";
                }
                catch (ArgumentNullException)
                {
                    MessageBox.Show("\nКоманда пинг - не проходит.\nPlease enter the host name or IP address to ping.");
                    server = "";
                }
                catch (System.Net.NetworkInformation.NetworkInformationException)
                {
                    MessageBox.Show($"\nКоманда пинг - не проходит.\nПк ОПС {textBox11.Text} - выключен или без интернета");
                    server = "";
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show($"\nКоманда пинг - не проходит.\nПк ОПС {textBox11.Text} - выключен или без интернета");
                    server = "";
                }
            }
            else
                MessageBox.Show($"\nПоле для ввода номера ОПС - пустое\n(Метод Name_DataBase_and_Server)");
        }
        public void SeName_DataBase_and_Server(string numder_ops, out string server, out string status)
        {
            status = "";
            server = "";
            textBox12.Clear();
            textBox13.Clear();
            try
            {
                if (numder_ops != "222222")
                {
                    IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
                    ipAddress = Dns.GetHostEntry("r40-" + numder_ops + "-n").AddressList[0];
                    Ping ping = new Ping();
                    PingReply pingReply = ping.Send(ipAddress);
                    if (pingReply.Address != null)
                    {
                        if (pingReply.Address.ToString() != "10.94.187.117"
                            & pingReply.Address.ToString() != "10.94.209.149"
                            & pingReply.Address.ToString() != "10.94.187.101"
                            & pingReply.Address.ToString() != "10.94.185.21"
                            & pingReply.Address.ToString() != "10.94.225.101"
                            & pingReply.Address.ToString() != "10.94.205.197"
                            & pingReply.Address.ToString() != "10.94.185.85"
                            & pingReply.Address.ToString() != "10.94.218.53"
                             & pingReply.Address.ToString() != "10.94.206.69")
                        {
                            if (pingReply.Status.ToString() != "Success")
                            {
                                SeProdStreamWriterEAStrans(numder_ops + " - не подключается, pingReply.Status.ToString() != \"Success\"", ProdwritePath);
                            }
                            else
                            {
                                this.Ping = 1;
                                status = pingReply.Status.ToString();
                                server = pingReply.Address.ToString();
                                //textBox12.Text = pingReply.Address.ToString();
                                SeProdStreamWriterEAStrans(numder_ops + " - подключение открыто", ProdwritePath);
                                System.Threading.Thread.Sleep(100);
                            }
                        }
                        else
                        {
                            SeProdStreamWriterEAStrans(numder_ops + " - не подключается, (пк выкл, не ошибка, блок else 2, вернулся недопустимый ip)", ProdwritePath);
                            server = "";
                        }
                    }
                    else
                    {
                        SeProdStreamWriterEAStrans(numder_ops + " - не подключается, (пк выкл, не ошибка, блок else 1) pingReply.Address == null", ProdwritePath);
                        server = "";
                    }
                }
                else
                    server = @"D:\!localhost";
            }
            catch (PingException ex)
            {
                SeProdStreamWriterEAStrans(numder_ops + $" - ошибка 1, {ex}", ProdwritePath);
                server = "";
            }
            catch (SocketException)
            {
                //SeProdStreamWriterEAStrans(numder_ops + " - ошибка 2");
                server = "";
            }
            catch (ArgumentNullException)
            {
                SeProdStreamWriterEAStrans(numder_ops + " - ошибка 3", ProdwritePath);
                server = "";
            }
            catch (System.Net.NetworkInformation.NetworkInformationException)
            {
                SeProdStreamWriterEAStrans(numder_ops + " - ошибка 4", ProdwritePath);
                server = "";
            }
            catch (NullReferenceException)
            {
                SeProdStreamWriterEAStrans(numder_ops + " - ошибка 5", ProdwritePath);
                server = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProcessStart(textBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost");
        }

        private void button3_Click(object sender, EventArgs e)
        {
                 ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost\nsi02\data");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\" + textBox12.Text + @"\c$\Program Files (x86)\Microsoft Dynamics AX\60\Retail POS");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\" + textBox12.Text + @"\c$\Program Files (x86)\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA");
        }
        private void ProcessStart(string path)
        {
            try
            {
                Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\" + textBox12.Text + @"\c$\GMMQ\Export");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\" + textBox12.Text + @"\c$\GMMQ\Import");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\10.94.1.110\c$\UFPS\EAS\ExportKatalog\Arhiv");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\D01EASFTPBKP01\Backup_in\KAL\");
        }

        private void button19_Click(object sender, EventArgs e)
        {
            ProcessStart(@"C:\Users\Eduard.Karpov\Desktop\166.cmd");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\" + textBox12.Text + @"\c$\Program Files (x86)\Microsoft Dynamics AX\60\PosServices\GMMQ");
        }

        private void button22_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\" + textBox12.Text + @"\c$");
        }

        private void button21_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\" + textBox12.Text + @"\d$");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\10.94.1.110\c$\Users");
        }

        private void button24_Click(object sender, EventArgs e)
        {     
                 ProcessStart(@"D:\Обновление МПК");
        }

        private void button25_Click(object sender, EventArgs e)
        {
            ProcessStart(@"C:\");
        }

        private void button23_Click(object sender, EventArgs e)
        {
            ProcessStart(@"D:\");
        }

        private void button26_Click(object sender, EventArgs e)
        {
            ProcessStart(@"C:\Users\Eduard.Karpov\Desktop\Этот компьютер - Ярлык.lnk");
        }

        private void button32_Click(object sender, EventArgs e)
        {
            ProcessStart(@"D:\Clean");
        }

        private void button31_Click(object sender, EventArgs e)
        {
            if (textBox12.Text != "")
            {
                string path = @"D:\Clean\Clean.bat";
                string newPath = @"\\" + textBox12.Text + @"\d$\Clean.bat";
                FileInfo fileInf = new FileInfo(path);
                if (fileInf.Exists)
                {
                    fileInf.CopyTo(newPath, true);
                }
                FileInfo fileInf1 = new FileInfo(newPath);
                if (fileInf1.Exists)
                {
                    MessageBox.Show($"Clean.bat скопирован на ОПС: {textBox11.Text}"); 
                }
            }
        }

        private void button33_Click(object sender, EventArgs e)
        {
            MemoryInOps();
        }
        private void MemoryInOps()
        {
            if (textBox12.Text != "")
            {
                string command_power_shell = "Get-WmiObject -Class Win32_logicalDisk -ComputerName " + textBox12.Text + "" +
                    " | ft -Property DeviceID, {$_.FreeSpace/1Gb}";
                Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"/command {command_power_shell}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                });
                string result = process.StandardOutput.ReadToEnd();
                if (result != "")
                {
                    string freeSpace = result.Substring(56).Replace("\n", "").Replace(" ", "").Replace(":", ": ").Replace("-","");
                    MessageBox.Show($"Свободное место на дисках ОПС : \n\n{freeSpace}");
                }
                else
                {
                    MessageBox.Show($"Команда не выполнилась, метод - MemoryInOps()");
                }
            }
            else
                MessageBox.Show($"Поле IP адрес ОПС - пустое.");
        }
        private void SeMemoryOPS_C(string server, string ops, string text)
        {
            try
            {
                string command_power_shell = "Get-WmiObject -Class Win32_logicalDisk -ComputerName " + server + "" +
                        " | ft -Property DeviceID, {$_.FreeSpace/1Gb}";
                Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"/command {command_power_shell}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                });
                string result = process.StandardOutput.ReadToEnd();
                if (result != "")
                {
                    string freeSpace = result.Substring(56,25).Replace("\n", "").Replace(Environment.NewLine, "").Replace(" ", "").Replace(":", ": ").Replace("-", "").Trim();
                    string l = $" - Свободное место на дисках ОПС: {freeSpace} {text}".Replace("\n", "");
                    SeProdStreamWriterEAStrans(ops + l, ProdwritePath);
                }
                else
                {
                    SeProdStreamWriterEAStrans(ops + $" - Команда не выполнилась, метод - SeMemoryInOps(), переменная result = null. Свободное место на дисках выяснить не удалось", ProdwritePath);
                }
            }
            catch(Exception ex)
            {
                SeProdStreamWriterEAStrans(ops + $" - Ошибка: \n{ex}", ProdwritePath);
            }
        }

        private void button34_Click(object sender, EventArgs e)
        {
            if (textBox12.Text != "")
            {
                string path = @"\\10.94.1.110\c$\UFPS\EAS\ExportKatalog\Arhiv\subscriptionCatalog_2022_1_141021_170641.json.gz";
                string path2 = @"\\10.94.1.110\c$\UFPS\EAS\ExportKatalog\Arhiv\subscriptionCatalog_2021_2_111021_123245.json.gz";
                string newPath = @"\\" + textBox12.Text + @"\d$\subscriptionCatalog_2022_1_141021_170641.json.gz";
                string newPath1 = @"\\" + textBox12.Text + @"\d$\subscriptionCatalog_2021_2_111021_123245.json.gz";
                FileInfo fileInf = new FileInfo(path);
                FileInfo fileInf2 = new FileInfo(path2);
                if (fileInf.Exists & fileInf2.Exists)
                {
                    fileInf.CopyTo(newPath, true);
                    fileInf2.CopyTo(newPath1, true);
                }
                FileInfo fileInf1 = new FileInfo(newPath);
                if (fileInf1.Exists)
                {
                    MessageBox.Show($"subscriptionCatalog_2022_1_141021_170641.json.gz скопирован на ОПС: {textBox11.Text}");
                }
            }
            else
            {
                MessageBox.Show($"Поле IP адрес ОПС - пустое.");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Delete($"Удалить файлы ОПС {textBox1.Text}\nиз папки PostOffice?",
                @"\\" + textBox12.Text + @"\c$\Program Files(x86)\RussianPost\PostOffice");
        }
        private void Delete(string dialog, string path)
        {
            DialogResult result = MessageBox.Show(dialog,
                          "Сообщение",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                Name_DataBase_and_Server(textBox11.Text, out string server, out string name_database);
                if (server != "")
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    foreach (FileInfo file in di.EnumerateFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.EnumerateDirectories())
                    {
                        dir.Delete(true);
                    }
                }
            }
            if (result == DialogResult.No)
            {
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Delete($"Удалить файлы ОПС {textBox1.Text}\nиз папки Program data?",
              @"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Delete($"Удалить файлы ОПС {textBox12.Text}\nиз папки Retail POS?",
             @"\\" + textBox12.Text + @"\c$\Program Files (x86)\Microsoft Dynamics AX\60\Retail POS"/*@"D:\25"*/);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Delete($"Удалить файлы ОПС {textBox1.Text}\nиз папки GMMQ?",
           @"\\" + textBox12.Text + @"\c$\Program Files (x86)\Microsoft Dynamics AX\60\PosServices\GMMQ");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Delete($"Удалить файлы ОПС {textBox1.Text}\nиз папки GMMQ.Export?",
                    @"\\" + textBox12.Text + @"\c$\GMMQ\Export");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Delete($"Удалить файлы ОПС {textBox1.Text}\nиз папки GMMQ.Import?",
                                @"\\" + textBox12.Text + @"\c$\GMMQ\Import");
        }

        private void button35_Click(object sender, EventArgs e)
        {           
            if(Height != 497)
            {
                Height = 497;
            }
            else
            {
                Height = 150;
            }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            string[] path_out_configuration = new string[9]
                { @"\c$\ProgramData\RussianPost\nsi02\f130article-2021-10-01.json",
                  @"\c$\ProgramData\RussianPost\nsi02\f130articletype-2021-10-01.json",
                  @"\c$\ProgramData\RussianPost\nsi02\f130attributes-2021-10-01.json",
                  @"\c$\ProgramData\RussianPost\nsi02\f130easops_map-2021-10-01.json",
                  @"\c$\ProgramData\RussianPost\nsi02\f130easops_oper-2021-10-01.json",
                  @"\c$\ProgramData\RussianPost\nsi02\f130mfk_sql-2021-10-01.json",
                  @"\c$\ProgramData\RussianPost\nsi02\f130skdn-2021-10-01.json",
                  @"\c$\ProgramData\RussianPost\nsi02\f130skdncommon-2021-10-01.json",
                  @"\c$\ProgramData\RussianPost\nsi02\f130settings-2021-10-01.json"};
            string[] path_in_configuration = new string[9]
            {
                @"D:\Новая конфигурация\f130article-2021-10-01.json",
                @"D:\Новая конфигурация\f130articletype-2021-10-01.json",
                @"D:\Новая конфигурация\f130attributes-2021-10-01.json",
                @"D:\Новая конфигурация\f130easops_map-2021-10-01.json",
                @"D:\Новая конфигурация\f130easops_oper-2021-10-01.json",
                @"D:\Новая конфигурация\f130mfk_sql-2021-10-01.json",
                @"D:\Новая конфигурация\f130skdn-2021-10-01.json",
                @"D:\Новая конфигурация\f130skdncommon-2021-10-01.json",
                @"D:\Новая конфигурация\f130settings-2021-10-01.json"
            };
            for (int i = 0; i < path_out_configuration.Length; i++)
            {
                string path = path_in_configuration[i];
                string newPath = @"\\" + textBox12.Text + path_out_configuration[i];
                FileInfo fileInf = new FileInfo(path);
                if (fileInf.Exists)
                {
                    fileInf.CopyTo(newPath, true);
                    progressBar1.Value += 11;
                }
            }
            string newPath1 = @"\\" + textBox12.Text + path_out_configuration[8];
            FileInfo fileInf1 = new FileInfo(newPath1);
            if (fileInf1.Exists)
            {
                progressBar1.Value = 100;
                MessageBox.Show("Файлы конфигурации скопированы.");
                progressBar1.Value = 0;
            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            ProcessStart(@"D:\Новая конфигурация");
        }

        private void button37_Click(object sender, EventArgs e)
        {
            ProcessStart(@"D:\цхдпа");
        }

        private void button38_Click(object sender, EventArgs e)
        {
            ProcessStart(@"C:\Users\Eduard.Karpov\Desktop\Подписка");
        }
        public Form1(string number_ops)
        {
            InitializeComponent();
            textBox11.Text = number_ops;
            Width = 642;
            Height = 150;
            this.ActiveControl = button11;
            textBox11.Focus();
            AutoCompleteStringCollection source = new AutoCompleteStringCollection() { };
            source.AddRange(ReadText(@"C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Start_EAS_Trans\save\eas all ops.txt"));
            textBox11.AutoCompleteCustomSource = source;
            textBox11.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox11.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void button39_Click(object sender, EventArgs e)
        {
            ProcessStart(@"D:\Бэкапы");
        }
        private void button40_Click(object sender, EventArgs e)
        {
            if (textBox11.Text.Length != 6)
            {
                Name_DataBase_and_Server(textBox11.Text, out string server, out string status);
                ViewSmartExplorer();
            }
            else
            {
                MessageBox.Show("Длина поля соотвествует стандартному номеру ОПС для ПК начальника," +
                    " а не окна, длина поля, должна быть больше 6ти смиволов\n" +
                    "Пример ввода окна: 248001-w01");
            }
        }

        private void button41_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = @"/c cd C:\Users\Eduard.Karpov\Downloads\PSTools (1) & psexec \\" + textBox12.Text + @" -c d:\Clean\Clean.bat",
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            }).WaitForExit();
            MessageBox.Show($"Скрипт по очистке места на жестком диске С ОПС {textBox11.Text} - Выполнен \n");
        }

        async private void button42_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Smart explorer\Smart explorer\RangeClean\eas all ops.txt";
            string dialog = "Запустить скрипт для чистки места на заданном диапозоне ОПС \n" +
                @"Файл считывания диапозона ОПС: C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Smart explorer\Smart explorer\RangeClean";
            DialogResult result = MessageBox.Show(dialog,
                          "Сообщение",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {       
                    string ops = "";
                    for (int i = 0; i < ReadText(path).Length; i++)
                    {
                        progressBar1.Maximum = ReadText(path).Length;
                        ops = ReadText(path)[i];
                        SeName_DataBase_and_Server(ops, out string server, out string name_database);
                        if (server != "")
                        {
                            try
                            {
                                await Task.Run(() => SeMemoryOPS_C(server, ops, "- до очистки скриптом"));
                                await Task.Run(() => RangeScripts(ReadText(path)[i], server));
                                await Task.Run(() => SeProdStreamWriterEAStrans(ReadText(path)[i] +  " - очистка места на жестком диске С - выполнена",
                                    ProdwritePath));
                                await Task.Run(() => SeMemoryOPS_C(server, ops, "- после очистки скриптом"));
                                progressBar1.Value++;
                            }
                            catch (Exception ex)
                            {
                                progressBar1.Value = ReadText(path).Length;
                                SeProdStreamWriterEAStrans(ops + $" - Ошибка - {ex}", ProdwritePath);
                            }
                        }
                    }
                progressBar1.Value = ReadText(path).Length;
                MessageBox.Show($"Скрипт по очистке места на жестком диске С c диапозоном ОПС - Выполнен\n");
                progressBar1.Value = 0;
            }
            if (result == DialogResult.No)
            {
            }
        }
        private void RangeScripts(string ops, string server)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = @"/c cd C:\Users\Eduard.Karpov\Downloads\PSTools (1) & psexec \\" + server + @" -c d:\Clean\Clean.bat",
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            }).WaitForExit();
            SeProdStreamWriterEAStrans(ops + " - скрипт выполнен", ProdwritePath);
        }
        public void SeProdStreamWriterEAStrans(string text, string ProdwritePath)
        {
            DateTime date = DateTime.Now;
            string NewDateFormat = date.ToString("yyyy-MM-dd HH.mm.ss");
            using (StreamWriter sw = new StreamWriter(this.ProdwritePath, true, System.Text.Encoding.Default))
            {
                sw.WriteLineAsync($"[{NewDateFormat}] - ОПС " + text);
            }
        }

        private void button43_Click(object sender, EventArgs e)
        {
            File.Create(ProdwritePath);
        }

        private void button30_Click(object sender, EventArgs e)
        {
            ProcessStart("ftp://r16ftp00.main.russianpost.ru/Obmen/");
        }

        private void button28_Click(object sender, EventArgs e)
        {
            //SeMemoryOPS_C("10.94.96.194","248903", " - проверка");
        }
    }
}
