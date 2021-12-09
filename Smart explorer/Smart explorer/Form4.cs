using NLog;
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Smart_explorer
{
    public partial class Form4 : Form
    {       
        readonly static DateTime date = DateTime.Now;
        readonly static string NewDateFormat = date.ToString("yyyy-MM-dd");     
        int Ping = 0;
        readonly static string baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        readonly static string appStorageFolder = Path.Combine(baseFolder, "Start_EAS_Trans");
        static public string ProdwritePath = appStorageFolder + @"\data\path\Smart explorer\result\result " + NewDateFormat + ".txt";
        readonly private static Logger logger = LogManager.GetCurrentClassLogger();
        int globalclean = 0;
        public Form4()
        {
            InitializeComponent();
            Width = 629;
            Height = 150;
            this.ActiveControl = button11;
            textBox11.Focus();
            AutoCompleteStringCollection source = new AutoCompleteStringCollection() { };
            source.AddRange(ReadText(appStorageFolder + @"\data\path\Start_EAS_Trans\save\eas all ops.txt"));
            textBox11.AutoCompleteCustomSource = source;
            textBox11.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox11.AutoCompleteSource = AutoCompleteSource.CustomSource;         
        }
        //Функция для считывания текста из текстового файла по строкам и записи его в массив, возвращает массив строк
        public string[] ReadText(string path)
        {
            int count = File.ReadAllLines(path).Length;
            string[] array = new string[count];
            using (StreamReader fs = new StreamReader(path))
            {
                int counter = 0;
                while (true)
                {
                    counter++;
                    string temp = fs.ReadLine();
                    if (temp == null) break;
                    array[counter - 1] = temp;
                }
            }
            return array;
        }
       async private void button11_Click(object sender, EventArgs e)
        {
            if (textBox11.Text != "")
            {
                await Task.Run(() => Name_DataBase_and_Server(textBox11.Text, out string server, out string status));
                await Task.Run(() => ViewSmartExplorer());
            }
            else
                MessageBox.Show($"\nПоле для ввода номера ОПС - пустое\n(Метод Name_DataBase_and_Server)");
        }
        private void ViewSmartExplorer()
        {           
            if (textBox11.Text.Length == 6 & Ping == 1)
            {
                textBox1.Text = @"\\" + textBox12.Text + @"\c$\Program Files (x86)\RussianPost\PostOffice";
                textBox3.Text = @"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost";
                textBox2.Text = @"\\" + textBox12.Text + @"\c$\Program Files(x86)\Microsoft Dynamics AX\60\Retail POS";

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
                textBox23.Text = @"\\" + textBox12.Text + @"\c$\ProgramData\POS\Logs";
                textBox19.Text = @"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox11.Text + @"\In";
                textBox20.Text = @"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox11.Text + @"\Out";
                textBox21.Text = "<add name=\"server1\" url=\"tcp://D01EASOPSGMMQ.main.russianpost.ru:10401\" />";
            }
            else if(Ping == 1)
            {
                textBox2.Text = @"\\" + textBox12.Text + @"\c$\Program Files(x86)\Microsoft Dynamics AX\60\Retail POS";
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
                textBox21.Text = "<add name=\"server1\" url=\"tcp://D01EASOPSGMMQ.main.russianpost.ru:10401\" />";
            }
        }
        public void Name_DataBase_and_Server(string numder_ops, out string server, out string status)
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
                    if (numder_ops.Length > 6)
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
                            & pingReply.Address.ToString() != "10.94.206.69"
                            & pingReply.Address.ToString() != "10.94.207.245"
                            & pingReply.Address.ToString() != "10.94.201.245")
                        {
                            if (pingReply.Status.ToString() != "Success")
                            {
                                logger.Trace("сообщение 1 {0}", this.Text);
                                textBox12.Text = $"ОПС {textBox11.Text} не подключается";
                            }
                            else
                            {
                                this.Ping = 1;
                                status = pingReply.Status.ToString();
                                server = pingReply.Address.ToString();
                                textBox12.Text = pingReply.Address.ToString();
                                DBForm dB = new DBForm();
                                dB.DBSmartExplorersWrite("Подключение", numder_ops, status, server, DateTime.Now);
                                System.Threading.Thread.Sleep(200);
                                logger.Trace("log {0}", this.Text);
                                logger.Debug("log {0}", this.Text);
                                logger.Info("log {0}", this.Text);
                                logger.Warn("log {0}", this.Text);
                                logger.Error("log {0}", this.Text);
                                logger.Fatal("log {0}", this.Text);
                            }
                        }
                        else
                        {
                            logger.Trace("сообщение 1 {0}", this.Text);
                            MessageBox.Show($"\nКоманда пинг - не проходит.");
                            server = "";
                        }
                    }
                    else
                    {
                        logger.Trace("сообщение 1 {0}", this.Text);
                        MessageBox.Show($"\nКоманда пинг - не проходит.");
                        server = "";
                    }
                }
                else
                    server = @"D:\!localhost";
            }
            catch (PingException ex)
            {
                logger.Trace("сообщение 1 {0}", this.Text);
                MessageBox.Show($"\nКоманда пинг - не проходит.\n{ex.Message}");
                server = "";
            }
            catch (SocketException)
            {
                logger.Trace("сообщение 1 {0}", this.Text);
                MessageBox.Show("\nКоманда пинг - не проходит.\nCould not resolve host name.");
                server = "";
            }
            catch (ArgumentNullException)
            {
                logger.Trace("сообщение 1 {0}", this.Text);
                MessageBox.Show("\nКоманда пинг - не проходит.\nPlease enter the host name or IP address to ping.");
                server = "";
            }
            catch (System.Net.NetworkInformation.NetworkInformationException)
            {
                logger.Trace("сообщение 1 {0}", this.Text);
                MessageBox.Show($"\nКоманда пинг - не проходит.\nПк ОПС {textBox11.Text} - выключен или без интернета");
                server = "";
            }
            catch (NullReferenceException)
            {
                logger.Trace("сообщение 1 {0}", this.Text);
                MessageBox.Show($"\nКоманда пинг - не проходит.\nПк ОПС {textBox11.Text} - выключен или без интернета");
                server = "";
            }
        }
        public void SeName_DataBase_and_Server(string numder_ops, out string server, out string status)
        {
            status = "";
            server = "";
            if (globalclean == 0)
            {
                textBox12.Clear();
                textBox13.Clear();
            }
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
                            & pingReply.Address.ToString() != "10.94.206.69"
                            & pingReply.Address.ToString() != "10.94.207.245")
                        {
                            if (pingReply.Status.ToString() != "Success")
                            {
                                SeProdStreamWriterEAStrans(numder_ops + " - не подключается, pingReply.Status.ToString() != \"Success\"");
                            }
                            else
                            {
                                this.Ping = 1;
                                status = pingReply.Status.ToString();
                                server = pingReply.Address.ToString();
                                SeProdStreamWriterEAStrans(numder_ops + " - подключение открыто");
                                System.Threading.Thread.Sleep(100);
                            }
                        }
                        else
                        {
                            SeProdStreamWriterEAStrans(numder_ops + " - не подключается, (пк выкл, не ошибка, блок else 2, вернулся недопустимый ip)");
                            server = "";
                        }
                    }
                    else
                    {
                        SeProdStreamWriterEAStrans(numder_ops + " - не подключается, (пк выкл, не ошибка, блок else 1) pingReply.Address == null");
                        server = "";
                    }
                }
                else
                    server = @"D:\!localhost";
            }
            catch (PingException ex)
            {
                SeProdStreamWriterEAStrans(numder_ops + $" - ошибка 1, {ex}");
                server = "";
            }
            catch (SocketException)
            {
                //SeProdStreamWriterEAStrans(numder_ops + " - ошибка 2");
                server = "";
            }
            catch (ArgumentNullException)
            {
                SeProdStreamWriterEAStrans(numder_ops + " - ошибка 3");
                server = "";
            }
            catch (System.Net.NetworkInformation.NetworkInformationException)
            {
                SeProdStreamWriterEAStrans(numder_ops + " - ошибка 4");
                server = "";
            }
            catch (NullReferenceException)
            {
                SeProdStreamWriterEAStrans(numder_ops + " - ошибка 5");
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
            try
            {
                Process.Start(@"\\" + textBox12.Text + @"\c$\Program Files (x86)\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA");
            }
            catch
            {
                try
                {
                    Process.Start(@"\\" + textBox12.Text + @"\c$\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA");
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Ошибка: \n{ex}");
                }
            }
        }
        public static void ProcessStart(string path)
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

        /*async*/ private void button19_Click(object sender, EventArgs e)
        {
            //string lockal = "\"";
            //await Task.Run(() => Process.Start(new ProcessStartInfo
            //{
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = @"/c net use * \\10.94.1.166\Download """" /user:guest /persistent:no"
                //UseShellExecute = false,
                //CreateNoWindow = true,
                //RedirectStandardOutput = true
            });
            MessageBox.Show(@"/c net use * \\10.94.1.166\Download """" /user:guest /persistent:no");
            //await Task.Run(() => Process.Start(new ProcessStartInfo
            //{
            //    FileName = "cmd",
            //    Arguments = @"/c cd " + appStorageFolder + @"\data\bin\PSTools (1) & psexec \\" + lockal + @" -c " + appStorageFolder + @"\data\bin\166.cmd",
            //    //Arguments = @"/c cd C:\Users\Eduard.Karpov\Downloads\PSTools (1) & psexec \\" + textBox12.Text + @" -c d:\Clean\Clean.bat",
            //    UseShellExecute = false,
            //    WindowStyle = ProcessWindowStyle.Hidden,
            //    CreateNoWindow = true,
            //    RedirectStandardOutput = true
            //}).WaitForExit());      
            //Process.Start(appStorageFolder + @"\data\bin\166.cmd");
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
                 ProcessStart($"{appStorageFolder}" + @"\StaticExplorer\Обновление МПК");
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
            ProcessStart($"{appStorageFolder}" + @"\StaticExplorer\Clean");
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

       async private void button33_Click(object sender, EventArgs e)
        {
            await Task.Run(() => MemoryInOps());
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
                    SeProdStreamWriterEAStrans(ops + l);
                }
                else
                {
                    SeProdStreamWriterEAStrans(ops + $" - Команда не выполнилась, метод - SeMemoryInOps(), переменная result = null. Свободное место на дисках выяснить не удалось");
                }
            }
            catch(Exception ex)
            {
                SeProdStreamWriterEAStrans(ops + $" - Ошибка: \n{ex}");
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
            if(Height != 632)
            {
                Height = 632;
            }
            else
            {
                Height = 150;
            }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(CopyConfig);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            this.Activate();
            
        }
        private void CopyConfig()
        {
            string number_ops = textBox11.Text;
            string server = textBox12.Text;
            if (textBox11.Text != "" & textBox12.Text != "")
            {
                var form = new Smart_explorer.CopyConfig(number_ops, server, appStorageFolder);
                form.ShowDialog();
            }
            else
                MessageBox.Show("Введите номер ОПС и нажмите \"Сохранить\"");
        }
        private void button36_Click(object sender, EventArgs e)
        {
            ProcessStart($"{appStorageFolder}" + @"\StaticExplorer\Новая конфигурация");
        }

        private void button37_Click(object sender, EventArgs e)
        {
            ProcessStart($"{appStorageFolder}" + @"\StaticExplorer\цхдпа");
        }

        private void button38_Click(object sender, EventArgs e)
        {
            ProcessStart($"{appStorageFolder}" + @"\StaticExplorer\Подписка");
        }
        public Form4(string number_ops)
        {
            InitializeComponent();
            textBox11.Text = number_ops;
            Width = 642;
            Height = 150;
            this.ActiveControl = button11;
            textBox11.Focus();
            AutoCompleteStringCollection source = new AutoCompleteStringCollection() { };
            source.AddRange(ReadText(appStorageFolder + @"\data\path\Start_EAS_Trans\save\eas all ops.txt"));
            textBox11.AutoCompleteCustomSource = source;
            textBox11.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox11.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void button39_Click(object sender, EventArgs e)
        {
            ProcessStart(@"D:\Бэкапы");
        }
       async private void button40_Click(object sender, EventArgs e)
        {
            if (textBox11.Text.Length != 6)
            {
                await Task.Run(() => Name_DataBase_and_Server(textBox11.Text, out string server, out string status));
                ViewSmartExplorer();
            }
            else
            {
                MessageBox.Show("Длина поля соотвествует стандартному номеру ОПС для ПК начальника," +
                    " а не окна, длина поля, должна быть больше 6ти смиволов\n" +
                    "Пример ввода окна: 248001-w01");
            }
        }

      async private void button41_Click(object sender, EventArgs e)
        {
            await Task.Run(() => Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = @"/c cd " + appStorageFolder + @"\data\bin\PSTools (1) & psexec \\" + textBox12.Text + @" -c " + appStorageFolder + @"\data\bin\Clean.bat",
                //Arguments = @"/c cd C:\Users\Eduard.Karpov\Downloads\PSTools (1) & psexec \\" + textBox12.Text + @" -c d:\Clean\Clean.bat",
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            }).WaitForExit());
            MessageBox.Show($"Скрипт по очистке места на жестком диске С ОПС {textBox11.Text} - Выполнен \n");
        }

        async private void button42_Click(object sender, EventArgs e)
        {
            string path = appStorageFolder + @"\data\path\Smart explorer\eas all ops.txt";
            string dialog = "Запустить скрипт для чистки места на заданном диапозоне ОПС \n" +
                @"Файл считывания диапозона ОПС: " + appStorageFolder + @"\data\path\Smart explorer\RangeClean";
            DialogResult result = MessageBox.Show(dialog,
                          "Сообщение",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                string ops = "";
                string[] array = ReadText(path);
                for (int i = 0; i < array.Length; i++)
                {
                    globalclean = 1;
                    progressBar1.Maximum = array.Length;
                    ops = array[i];
                    label9.Text = $"Выполняется чистка диска С ОПС: {ops}\n({i} из {array.Length})";
                    SeName_DataBase_and_Server(ops, out string server, out string name_database);
                    if (server != "")
                    {
                        try
                        {
                            await Task.Run(() => SeMemoryOPS_C(server, ops, "- до очистки скриптом"));
                            await Task.Run(() => RangeScripts(ops, server));
                            await Task.Run(() => SeProdStreamWriterEAStrans(ops + " - очистка места на жестком диске С - выполнена"));
                            await Task.Run(() => SeMemoryOPS_C(server, ops, "- после очистки скриптом"));
                            progressBar1.Value++;
                        }
                        catch (Exception ex)
                        {
                            progressBar1.Value = array.Length;
                            SeProdStreamWriterEAStrans(ops + $" - Ошибка - {ex}");
                        }
                    }
                }
                globalclean = 0;
                progressBar1.Value = array.Length;
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
                Arguments = @"/c cd " + appStorageFolder + @"\data\bin\PSTools (1) & psexec \\" + textBox12.Text + @" -c " + appStorageFolder + @"\data\bin\Clean.bat",
                //Arguments = @"/c cd C:\Users\Eduard.Karpov\Downloads\PSTools (1) & psexec \\" + server + @" -c d:\Clean\Clean.bat",
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            }).WaitForExit();
            SeProdStreamWriterEAStrans(ops + " - скрипт выполнен");
        }
        public void SeProdStreamWriterEAStrans(string text)
        {
            DateTime date = DateTime.Now;
            string NewDateFormat = date.ToString("yyyy-MM-dd HH.mm.ss");
            using (StreamWriter sw = new StreamWriter(Form4.ProdwritePath, true, System.Text.Encoding.Default))
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
            ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\POS\Logs");
        }

        private void button29_Click(object sender, EventArgs e)
        {        
            ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\POS\Plugins");
        }

        private void button27_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox11.Text + @"\In");
        }

        private void button44_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox11.Text + @"\Out");
        }

        private void button45_Click(object sender, EventArgs e)
        {
            ProcessStart($"{appStorageFolder}" + @"\StaticExplorer\166 дистр");
        }

        private void button46_Click(object sender, EventArgs e)
        {
            ProcessStart(@"D:\GMMQ.Client.Service");
        }

        private void button47_Click(object sender, EventArgs e)
        {
            ProcessStart($"{appStorageFolder}" + @"\StaticExplorer\ЕАС дистр");
        }

        private void button48_Click(object sender, EventArgs e)
        {
            ProcessStart(@"C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Smart explorer\Smart explorer\result\");
        }

        private void button51_Click(object sender, EventArgs e)
        {      
            ProcessStart($"{appStorageFolder}" + @"\StaticExplorer\ЕАС скрипты\!Скрипты");
        }

        private void button52_Click(object sender, EventArgs e)
        {       
            ProcessStart($"{appStorageFolder}" + @"\StaticExplorer\МПК скрипты");
        }

        private void button49_Click(object sender, EventArgs e)
        {
            ProcessStart($"{appStorageFolder}" + @"\StaticExplorer\МПК инструкции");
        }
        private void button54_Click(object sender, EventArgs e)
        {
            //ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost\trans02\log\errors.LOG");
        }

       async private void button58_Click(object sender, EventArgs e)
        {
            await Task.Run(() => ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost\trans02\log\errors.LOG"));
        }

       async private void button57_Click(object sender, EventArgs e)
        {
            await Task.Run(() => ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost\trans02\log\main.LOG"));
        }

        private void button56_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost\trans02\data\" + textBox11.Text);
        }
        private void button60_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(Rebut_Service_PochtaForm1);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            this.Activate();
        }
        private void Rebut_Service_PochtaForm1()
        {
            string server = textBox12.Text;
            var form = new Rebut_Service_Pochta.Form1(server);
            form.ShowDialog();
        }

        private void button61_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(Copy_Log_MPKCopy_Log_MPK);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            this.Activate();
        }
        private void Copy_Log_MPKCopy_Log_MPK()
        {
            string number_ops = textBox11.Text;
            var form = new Copy_Log_MPK.Copy_Log_MPK(number_ops);
            form.ShowDialog();
        }

        private void button59_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(Version_FileForm1);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            this.Activate();
        }
        private void Version_FileForm1()
        {
            string number_ops = textBox11.Text;
            var form = new Version_File.Form1(number_ops);
            form.ShowDialog();
        }

        private void button62_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(Copy_EAS_LogForm1);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            this.Activate();
        }
        private void Copy_EAS_LogForm1()
        {
            string number_ops = textBox11.Text;
            var form = new Copy_EAS_Log.Form1(number_ops);
            form.ShowDialog();
        }

        private void button63_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(PhoneForm5);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            this.Activate();
        }
        private void PhoneForm5()
        {
            string path = appStorageFolder;
            var form = new Smart_explorer.Form5(appStorageFolder);
            form.ShowDialog();
        }

        private void button64_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(ScriptrepeatedsendForm6);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            this.Activate();
        }
        private void ScriptrepeatedsendForm6()
        {
            string number_ops = textBox11.Text;
            string server = textBox12.Text;
            if (textBox11.Text != "" & textBox12.Text != "")
            {
                var form = new Smart_explorer.Form6(number_ops, server);
                form.ShowDialog();
            }
            else
                MessageBox.Show("Введите номер ОПС и нажмите \"Сохранить\"");
        }

        private void button65_Click(object sender, EventArgs e)
        {
            ProcessStart($"{appStorageFolder}" + @"\StaticExplorer\Перезапуск ОПС");
        }

        private void button55_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\" + textBox12.Text + @"\c$\Program Files (x86)\RussianPost\PostUnit");
        }

        private void button53_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\" + textBox12.Text + @"\c$\Windows\System32\drivers\etc\hosts");
        }

        private void button68_Click(object sender, EventArgs e)
        {
            StartRemoteAccess("DNTU", @"D:\DWare\DNTU.exe");
        }

        public void ShowNormal(Process proc)
        {
            WinAPI.ShowWindow(proc.MainWindowHandle, WinAPI.Consts.SHOWWINDOW.SW_SHOWNORMAL);
        }
        private void StartRemoteAccess(string nameProcess, string path)
        {
            bool count = true;
            foreach (Process pr in Process.GetProcessesByName(nameProcess))
            {
                count = false;
                ShowNormal(pr);
            }
            if (count)
                ProcessStart(path);
        }

        public void SetFocus(Process proc)
        {
            WinAPI.SetForegroundWindow(proc.MainWindowHandle);
        }
        /// <summary>
        /// Набор WinAPI функций, как есть (Или почти как есть)
        /// </summary>
        public static class WinAPI
        {

            /// <summary>
            ///  Устанавливает состояние показа определяемого окна.
            ///  Если функция завершилась успешно, возвращается значение
            ///  отличное от нуля. Если функция потерпела неудачу,
            ///  возвращаемое значение - ноль.
            /// </summary>
            /// <param name="hWnd">Дескриптор окна</param>
            /// <param name="nCmdShow">Определяет, как окно должно быть показано.</param>
            /// <returns>
            ///  Если функция завершилась успешно, возвращается значение
            ///  отличное от нуля. Если функция потерпела неудачу,
            ///  возвращаемое значение - ноль.
            ///  </returns>
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            /// <summary>
            ///  Устанавливает состояние показа определяемого окна.
            ///  Если функция завершилась успешно, возвращается значение
            ///  отличное от нуля. Если функция потерпела неудачу,
            ///  возвращаемое значение - ноль.
            /// </summary>
            /// <param name="hWnd">Дескриптор окна</param>
            /// <param name="nCmdShow">Определяет, как окно должно быть показано.</param>
            /// <returns>
            ///  Если функция завершилась успешно, возвращается значение
            ///  отличное от нуля. Если функция потерпела неудачу,
            ///  возвращаемое значение - ноль.
            ///  </returns>
            public static bool ShowWindow(IntPtr hWnd, Consts.SHOWWINDOW nCmdShow)
            {
                return ShowWindow(hWnd, (int)nCmdShow);
            }

            /// <summary>
            /// Установить окно на передний план
            /// </summary>
            /// <param name="hWnd">Handle окна</param>
            /// <returns>Удачность</returns>
            [DllImport("user32.dll")]
            public static extern bool SetForegroundWindow(IntPtr hWnd);


            /// <summary>
            /// Набор констант
            /// </summary>
            public static class Consts
            {

                /// <summary>
                /// Параметры к функции ShowWindow. 
                /// Внимание! Некоторые параметры имеют одинаковое значение
                /// (Почему? За ответом к дяде Биллу)
                /// </summary>
                public enum SHOWWINDOW : uint
                {
                    /// <summary>
                    /// Скрывает окно и активизирует другое окно
                    /// </summary>
                    SW_HIDE = 0,
                    /// <summary>
                    /// Активизирует и отображает окно.
                    /// Если окно свернуто или развернуто,
                    /// Windows восстанавливает его в 
                    /// первоначальном размере и позиции. 
                    /// Прикладная программа должна установить 
                    /// этот флажок при отображении окна впервые
                    /// </summary>
                    SW_SHOWNORMAL = 1,
                    SW_NORMAL = 1,
                    /// <summary>
                    /// Активизирует окно и отображает его как свернутое окно
                    /// </summary>
                    SW_SHOWMINIMIZED = 2,
                    /// <summary>
                    /// Активизирует окно и отображает его как развернутое окно
                    /// </summary>
                    SW_SHOWMAXIMIZED = 3,
                    /// <summary>
                    /// Развертывает определяемое окно
                    /// </summary>
                    SW_MAXIMIZE = 3,
                    /// <summary>
                    /// Отображает окно в его самом современном размере и позиции. 
                    /// Активное окно остается активным
                    /// </summary>
                    SW_SHOWNOACTIVATE = 4,
                    /// <summary>
                    /// Активизирует окно и отображает его текущие размеры и позицию
                    /// </summary>
                    SW_SHOW = 5,
                    /// <summary>
                    /// Свертывает определяемое окно и активизирует следующее окно 
                    /// верхнего уровня в Z-последовательности
                    /// </summary>
                    SW_MINIMIZE = 6,
                    /// <summary>
                    /// Отображает окно как свернутое окно. Активное окно остается активным
                    /// </summary>
                    SW_SHOWMINNOACTIVE = 7,
                    /// <summary>
                    /// Отображает окно в его текущем состоянии. Активное окно остается активным
                    /// </summary>
                    SW_SHOWNA = 8,
                    /// <summary>
                    /// Активизирует и отображает окно. 
                    /// Если окно свернуто или развернуто, 
                    /// Windows восстанавливает в его первоначальных 
                    /// размерах и позиции. Прикладная программа должна 
                    /// установить этот флажок при восстановлении свернутого окна
                    /// </summary>
                    SW_RESTORE = 9,
                    /// <summary>
                    /// Устанавливает состояние показа, основанное на флажке SW_
                    /// , определенном в структуре STARTUPINFO, 
                    /// переданной в функцию CreateProcess программой, 
                    /// которая запустила прикладную программу
                    /// </summary>
                    SW_SHOWDEFAULT = 10,
                    /// <summary>
                    /// Windows 2000/XP: Свертывает окно, даже если поток,
                    /// который владеет окном, зависает. Этот флажок должен 
                    /// быть использоваться только при свертывании окон 
                    /// другого потока
                    /// </summary>
                    SW_FORCEMINIMIZE = 11,
                    SW_MAX = 11,
                }
            }
        }
        private void button67_Click(object sender, EventArgs e)
        {
            StartRemoteAccess("mstsc", @"C:\WINDOWS\system32\mstsc.exe");
        }

        private void button66_Click(object sender, EventArgs e)
        {
            StartRemoteAccess("CmRcViewer", @"D:\2020 год папка загрузки на С\RC SCCM\CmRcViewer.exe");
        }

       async private void Button69_Click(object sender, EventArgs e)
        {
            await Task.Delay(100);
            Thread th = new Thread(DBRun);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            this.Activate();
        }
        private void DBRun()
        {
            var form = new Smart_explorer.DBForm();
            form.ShowDialog();
        }

       async private void button70_Click(object sender, EventArgs e)
        {
            await Task.Run(() => ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost\config03\log\main.LOG"));
        }

       async private void button71_Click(object sender, EventArgs e)
        {
            await Task.Run(() => ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost\config03\log\errors.LOG"));
        }

       async private void button72_Click(object sender, EventArgs e)
        {
            await Task.Run(() => ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost\sdo02\log\main.LOG"));
        }

       async private void button73_Click(object sender, EventArgs e)
        {
            await Task.Run(() => ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost\sdo02\log\errors.LOG"));
        }

       async private void button74_Click(object sender, EventArgs e)
        {
            await Task.Run(() => ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost\nsi02\log\main.LOG"));
        }

       async private void button75_Click(object sender, EventArgs e)
        {
            await Task.Run(() => ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost\nsi02\log\errors.LOG"));
        }

        async private void button76_Click(object sender, EventArgs e)
        {
            await Task.Run(() => ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost\update02\log\main.LOG"));
        }

        async private void button77_Click(object sender, EventArgs e)
        {
            await Task.Run(() => ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost\update02\log\errors.LOG"));
        }

       async private void button78_Click(object sender, EventArgs e)
        {
            await Task.Run(() => ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost\user21\log\main.LOG"));
        }

       async private void button79_Click(object sender, EventArgs e)
        {
            await Task.Run(() => ProcessStart(@"\\" + textBox12.Text + @"\c$\ProgramData\RussianPost\user21\log\errors.LOG"));
        }

        private void button80_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\" + textBox12.Text + @"\c$\Windows\System32\drivers\etc");
        }

        private void button82_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox20.Text);
        }

        private void button81_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox19.Text);
        }

        private void button85_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox14.Text);
        }

        private void button84_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox10.Text);
        }

        private void button83_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox25.Text);
        }

        private void button54_Click_1(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox6.Text);
        }

        private void button86_Click(object sender, EventArgs e)
        {
            ProcessStart(@"\\10.94.1.166\Download\");
        }
    }
}
