using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
  public class Methods : Form1
    {
        readonly static string baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        readonly static string appStorageFolder = Path.Combine(baseFolder, "Start_EAS_Trans");
        public Methods(string prodwritePath, int Action, int checkTransport, int automodul)
        {
            ProdwritePath = prodwritePath;
            action = Action;
            СheckTransport = checkTransport;
            Automodul = automodul;
        }
        public Methods(int streamwriteEAStrans, string writePath, List<Control> l)
        {
            stream = streamwriteEAStrans;
            path = writePath;
            listBox3 = (ListBox)l[0];
        }
        public Methods(ListBox listBox)
        {
            listBox3 = listBox;
        }
        public int stream;
        public string path; 
        public  new int action = 0;
        public  new int СheckTransport = 0;
        public  new string ProdwritePath = appStorageFolder + @"\data\path\Start_EAS_Trans\Prod\prod.txt";
        public  new int Automodul = 0;
        public  new string ops = "";
        public  new string i = "";
        public  int u = 1;
        public  List<string> listDistrict = new List<string>();
        public  ListBox listBox3;

        //Функция для считывания текста из текстового файла по строкам и записи его в массив, возвращает массив строк
        public new string[] ReadText(string path)
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
     public void ProdStreamWriterEAStrans(string text, string ops, string ProdwritePath)
        {
            using (StreamWriter sw = new StreamWriter(ProdwritePath, true, System.Text.Encoding.Default))
            {
                DateTime date = DateTime.Now;
                string NewDateFormat = date.ToString("yyyy-MM-dd HH.mm.ss");
                  sw.WriteLineAsync("[" + NewDateFormat + "] - ОПС " + ops + " " + text);
            }
            if (ProdwritePath != writePath)
            {
                listBox3.Items.Add("ОПС " + ops + " " + text);
            }
        }
        async public  void ProdStreamWriterEAStrans(string text, string ops, string ProdwritePath, ListBox listBox)
        {
            Stream myStream;
            using (myStream = File.Open(ProdwritePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                myStream.Position = 0;
                StreamWriter sw = new StreamWriter(myStream);
                DateTime date = DateTime.Now;
                string NewDateFormat = date.ToString("yyyy-MM-dd HH.mm.ss");
                await sw.WriteLineAsync("[" + NewDateFormat + "] - ОПС " + ops + " " + text);
                if (ProdwritePath != writePath)
                {
                    await Task.Run(() => listBox.Items.Add("ОПС " + ops + " " + text));
                }
            }
        }
        async public  void ProdStreamWriterEAStrans(string text, string ProdwritePath)
        {
            using (StreamWriter sw = new StreamWriter(ProdwritePath, true, System.Text.Encoding.Default))
            {
                DateTime date = DateTime.Now;
                string NewDateFormat = date.ToString("yyyy-MM-dd HH.mm.ss");
                await sw.WriteLineAsync("[" + NewDateFormat + "] " + text);
            }
        }
       
     public void Prod_Name_DataBase_and_Server(string number_ops, out string server, out string name_database)
        {
            string text = $"- не подключается";
            name_database = "DB" + number_ops;
            server = "";
            if (number_ops != "")
            {
                try
                {
                    IPAddress ipAddress = Dns.GetHostEntry("r40-" + number_ops + "-n").AddressList[0];
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
                          & pingReply.Address.ToString() != "10.94.206.69"
                          & pingReply.Address.ToString() != "10.94.219.165"
                          & pingReply.Address.ToString() != "10.94.185.117"
                          & pingReply.Address.ToString() != "10.94.218.53"
                          & pingReply.Address.ToString() != "10.94.201.245"
                          & pingReply.Address.ToString() != "10.94.207.245")
                        {
                            if (pingReply.Status.ToString() != "Success")
                            {
                                EndOperation(number_ops, text);
                                server = "";
                            }
                            else
                            {
                                server = pingReply.Address.ToString();
                                System.Threading.Thread.Sleep(200);
                            }                          
                        }
                        else
                        {
                            EndOperation(number_ops, text);
                            server = "";
                        }
                    }
                    else
                    {
                        EndOperation(number_ops, text);
                        server = "";
                    }
                }
                catch (PingException)
                {
                    EndOperation(number_ops, text);
                    server = "";
                }
                catch (SocketException)
                {
                    EndOperation(number_ops, text);
                    server = "";
                }

                catch (ArgumentNullException)
                {
                    EndOperation(number_ops, text);
                    server = "";
                }
                catch (System.Net.NetworkInformation.NetworkInformationException)
                {
                    EndOperation(number_ops, text);
                    server = "";
                }
                catch (NullReferenceException)
                {
                    EndOperation(number_ops, text);
                    server = "";
                }
            }
        }
        public void EndOperation(string number_ops, string text)
        {
            DefinitionDistrict(number_ops, out List<string> newDistrict);     
            if (streamwriteEAStrans == 1)
            {
                ProdStreamWriterEAStrans(text, number_ops, writePath);
            }
            ProdStreamWriterEAStrans(text, number_ops, ProdwritePath);
        }
        public  void DefinitionDistrict(string number_ops, out List<string> newDistrict)
        {
            bool key = true;
            string[] District = new string[10] { "Бабынискому", "Жуковскому",
                "Калужскому", "Кировскому","Козельскому",
                "Кондровскому","Людиновскому", "Обнинскому" ,"Сухиничскому", "Юхновскому"};
            string[] massOps = ReadText(writePath);
            for (int i = 0; i < massOps.Length; i++)
            {
                for (int k = 0; k < District.Length; k++)
                {
                    if (massOps[i].Contains("Транспорт по " + District[k] + " почтамту:"))
                    {
                        key = false;
                        break;
                    }                  
                }             
            }
            newDistrict = new List<string>();
            if (streamwriteEAStrans == 1)
            {
                if (key)
                {
                    if (Convert.ToInt32(number_ops) >= 249000
                    & Convert.ToInt32(number_ops) <= 249096
                    & Convert.ToInt32(number_ops) != 249080
                    & Convert.ToInt32(number_ops) != 249082
                    & Convert.ToInt32(number_ops) != 249086
                    & Convert.ToInt32(number_ops) != 249087)
                    {
                        if (!i.Contains(District[7]))
                        {
                            AddStreamwriterDistrict(District[7]);
                            i += District[7];
                            newDistrict.Add(District[7]);
                        }
                    }
                    if (Convert.ToInt32(number_ops) >= 249200
                    & Convert.ToInt32(number_ops) <= 249255
                    | Convert.ToInt32(number_ops) == 249142
                    | Convert.ToInt32(number_ops) == 249143
                    | Convert.ToInt32(number_ops) == 249139
                    | Convert.ToInt32(number_ops) == 249134
                    | Convert.ToInt32(number_ops) == 249120)
                    {
                        if (!i.Contains(District[0]))
                        {
                            AddStreamwriterDistrict(District[0]);
                            i += District[0];
                            newDistrict.Add(District[0]);
                        }
                    }
                    if (Convert.ToInt32(number_ops) >= 249100
                    & Convert.ToInt32(number_ops) <= 249111
                    | Convert.ToInt32(number_ops) == 249160
                    & Convert.ToInt32(number_ops) == 249192)
                    {
                        if (!i.Contains(District[1]))
                        {
                            AddStreamwriterDistrict(District[1]);
                            i += District[1];
                            newDistrict.Add(District[0]);
                        }
                    }
                    if (Convert.ToInt32(number_ops) >= 249261
                    & Convert.ToInt32(number_ops) <= 249322)
                    {
                        if (!i.Contains(District[8]))
                        {
                            AddStreamwriterDistrict(District[8]);
                            i += District[8];
                            newDistrict.Add(District[0]);
                        }
                    }
                }
            }
        }
        public void EndOperationDistrict(out string[] DefinitionOps)
        {
            DefinitionOps = new string[10];
            string[] massOps = ReadText(pathAutoEASTrans);
            try
            {
                int max1 = int.MinValue;
                int max2 = int.MinValue;
                int max3 = int.MinValue;
                int max4 = int.MinValue;
                for (int i = 0; i < massOps.Length; i++)
                {
                    if (massOps[i] != "")
                    {
                        if (Convert.ToInt32(massOps[i]) >= 249000
                        & Convert.ToInt32(massOps[i]) <= 249096
                        & Convert.ToInt32(massOps[i]) != 249080
                        & Convert.ToInt32(massOps[i]) != 249082
                        & Convert.ToInt32(massOps[i]) != 249086
                        & Convert.ToInt32(massOps[i]) != 249087)
                        {
                            if (Convert.ToInt32(massOps[i]) > max1)
                            {
                                max1 = Convert.ToInt32(massOps[i]);
                            }
                        }
                        if (Convert.ToInt32(massOps[i]) >= 249200
                        & Convert.ToInt32(massOps[i]) <= 249255
                        | Convert.ToInt32(massOps[i]) == 249142
                        | Convert.ToInt32(massOps[i]) == 249143
                        | Convert.ToInt32(massOps[i]) == 249139
                        | Convert.ToInt32(massOps[i]) == 249134
                        | Convert.ToInt32(massOps[i]) == 249120)
                        {
                            if (Convert.ToInt32(massOps[i]) > max2)
                            {
                                max2 = Convert.ToInt32(massOps[i]);
                            }
                        }
                        if (Convert.ToInt32(massOps[i]) >= 249100
                        & Convert.ToInt32(massOps[i]) <= 249111
                        | Convert.ToInt32(massOps[i]) == 249160
                        & Convert.ToInt32(massOps[i]) == 249192)
                        {
                            if (Convert.ToInt32(massOps[i]) > max3)
                            {
                                max3 = Convert.ToInt32(massOps[i]);
                            }
                        }
                        if (Convert.ToInt32(massOps[i]) >= 249261
                        & Convert.ToInt32(massOps[i]) <= 249322)
                        {
                            if (Convert.ToInt32(massOps[i]) > max4)
                            {
                                max4 = Convert.ToInt32(massOps[i]);
                            }
                        }
                    }
                }
                DefinitionOps[0] = Convert.ToString(max1);
                DefinitionOps[1] = Convert.ToString(max2);
                DefinitionOps[2] = Convert.ToString(max3);
                DefinitionOps[3] = Convert.ToString(max4);
            }
            catch (Exception ex)
            {
                ProdStreamWriterEAStrans(ops + $" - ошибка \n{ex}", ops, ProdwritePath);
            }
        }
        new public void AddStreamwriterDistrict(string district)
        {
            if (streamwriteEAStrans == 1)
            {
                DateTime date = DateTime.Now;
                string NewDateFormat = date.ToString("yyyy-MM-dd HH.mm.ss");
                StreamWriterEAStrans($"[{NewDateFormat}] Транспорт по {district} почтамту:"); 
            }
            else
            {
                MessageBox.Show($"Поле для ввода номера ОПС пустое\nНе включена функия - \"Запись в файл\"");
            }
        }
         public  void AddStreamwriterEndOperationDistrict()
        {
            if (streamwriteEAStrans == 1)
            {
                DateTime date = DateTime.Now;
                string NewDateFormat = date.ToString("yyyy-MM-dd HH.mm.ss");
                StreamWriterEAStrans($"[{NewDateFormat}] Больше остановок транспорта не зафиксировано");
            }
            else
            {
                MessageBox.Show($"Поле для ввода номера ОПС пустое\nНе включена функия - \"Запись в файл\"");
            }
        }
        async public void Prod_Start_Transport(SqlCommand command, string ops, string writePath)
        {
            action = 0;
            try
            {
                if (ops != "")
                {
                    Prod_Name_DataBase_and_Server(ops, out string server, out string name_database);
                    string connectionString = $"Server={server};Database={name_database};Persist Security Info=False;User ID=sa;Password=QweAsd123;";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();
                        ProdStreamWriterEAStrans("Подключение открыто", ops, ProdwritePath);
                    }
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();
                        command.CommandText = "exec ReplicaExport 0";
                        command.Connection = connection;
                        command.CommandTimeout = 5000; //увеличено время на выполнение команды
                        await Task.Run(() => command.ExecuteNonQueryAsync());
                        ProdStreamWriterEAStrans("Скрипт выполнен", ops, ProdwritePath);
                        ProdStreamWriterEAStrans($"Запрос в SQL: \"{command.CommandText}\" - успешно отработан", ops, ProdwritePath);
                        string text = $"- толкнул скриптом +";
                        EndOperation(ops, text);                      
                        action = 1;
                        ProdAction(action);
                    }
                }
                else
                    ProdStreamWriterEAStrans($"\nПоле для ввода номера ОПС - пустое", ops, ProdwritePath);
                action = 1;
            }
            catch (Exception ex)
            {
                ProdStreamWriterEAStrans(ops + $" - ошибка \n{ex}", ops, ProdwritePath);
            }
        }
        public int ProdAction(int action)
        {
            return action;
        }
        async public void СheckEasTransport(string server, string ops)
        {
            try
            {
                ProdStreamWriterEAStrans(" ", ops, ProdwritePath);
                СheckTransport = 1;
                if (server != "")
                {
                    CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                             // отменяет отслеживание ошибок,
                                                             // но дает передать компоненты формы в другой поток 
                    DateTime now = new DateTime();
                    now = DateTime.Now;
                    string path = @"\\" + server + @"\c$\GMMQ\Export";
                    string[] files = Directory.GetFiles(path);
                    string[] files1 = Directory.GetFiles(path);
                    string[] catalog = Directory.GetDirectories(path);
                    Get_Date_Time(@"\\" + server + @"\c$\GMMQ\Export", ops, ProdwritePath);
                    DateTime export = Get_Date_Time(@"\\" + server + @"\c$\GMMQ\Export", ops, ProdwritePath).AddHours(3);
                    DateTime import = Get_Date_Time(@"\\" + server + @"\c$\GMMQ\Import", ops, ProdwritePath).AddHours(3);
                    DateTime In = Get_Date_Time(@"\\D01eascl02fskal\gmmq\EAS\KAL\" + ops + @"\In", ops, ProdwritePath).AddHours(3);
                    DateTime Out = Get_Date_Time(@"\\D01eascl02fskal\gmmq\EAS\KAL\" + ops + @"\Out", ops, ProdwritePath).AddHours(3);
                    if (now < In & now < Out)
                    {
                        string text = "Время обновления папок актуальное";
                        ProdStreamWriterEAStrans(text, ops, ProdwritePath);
                        if (server != "")
                        {
                            if (ops != "248000" & ops != "249406")
                            {
                                string ip = server;
                                string result = "";
                                await Task.Run(() => Power_Shell_1("get-service -" +
                                                   "DisplayName \"GMMQ\"" +
                                                   " -ComputerName " + ip + "" +
                                                   " | format-table Status -autosize", out result));
                                ProdStreamWriterEAStrans("1 Служба GMMQ - " + result.Replace("\r\n", ""), ops, ProdwritePath);
                                if (result.Replace("\r\n", "") == "Running")
                                {
                                    if (server != "")
                                    {
                                        await Task.Run(() => Power_Shell_1("get-service -" +
                                                           "DisplayName \"GM_SchedulerSvc\"" +
                                                           " -ComputerName " + ip + "" +
                                                           " | format-table Status -autosize", out result));
                                        ProdStreamWriterEAStrans("2 Служба GM_Scheduler - " + result.Replace("\r\n", ""), ops, ProdwritePath);
                                        if (result.Replace("\r\n", "") == "Running")
                                        {
                                            ProdStreamWriterEAStrans("3 Службы транспорта работают", ops, ProdwritePath);
                                            ProdStreamWriterEAStrans("4 Транспорт в рабочем состоянии", ops, ProdwritePath);
                                            EndOperation(ops, "- транспорт работает +");
                                            СheckTransport = 1;
                                            action = 1;

                                        }
                                        else
                                        {
                                            ProdStreamWriterEAStrans("5 Служба GM_SchedulerSvc - не работает", ops, ProdwritePath);
                                            if (server != "")
                                            {
                                                string name_service = "GM_SchedulerSvc";
                                                string action_service = "Running";
                                                await Task.Run(() => Prod_Async_Power_Shell_Service(server, name_service, action_service, ops));
                                            }
                                            ProdStreamWriterEAStrans("6 Запуск GM_SchedulerSvc осуществлен", ops, ProdwritePath);
                                            ProdStreamWriterEAStrans("7 Транспорт в рабочем состоянии", ops, ProdwritePath);
                                            EndOperation(ops, "- транспорт работает +");
                                            СheckTransport = 1;
                                            action = 1;

                                        }
                                    }
                                    else
                                        ProdStreamWriterEAStrans("Поле для ввода пустое.\nВведите ip отделения почтовой связи.", ops, ProdwritePath);
                                }
                                else
                                {
                                    ProdStreamWriterEAStrans("8 Служба GMMQ - не работает", ops, ProdwritePath);
                                    if (server != "")
                                    {
                                        string name_service = "GMMQ";
                                        string action_service = "Running";
                                        await Task.Run(() => Prod_Async_Power_Shell_Service(server, name_service, action_service, ops));
                                    }
                                    ProdStreamWriterEAStrans("9 Запуск GMMQ осуществлен", ops, ProdwritePath);
                                    if (server != "")
                                    {
                                        await Task.Run(() => Power_Shell_1("get-service -" +
                                                           "DisplayName \"GM_SchedulerSvc\"" +
                                                           " -ComputerName " + ip + "" +
                                                           " | format-table Status -autosize", out result));
                                        ProdStreamWriterEAStrans("10 Служба GM_Scheduler - " + result.Replace("\r\n", ""), ops, ProdwritePath);
                                        if (result.Replace("\r\n", "") == "Running")
                                        {
                                            ProdStreamWriterEAStrans("11 Службы транспорта работают", ops, ProdwritePath);
                                            ProdStreamWriterEAStrans("12 Транспорт в рабочем состоянии", ops, ProdwritePath);
                                            EndOperation(ops, "- транспорт работает +");
                                            СheckTransport = 1;
                                            action = 1;

                                        }
                                        else
                                        {
                                            ProdStreamWriterEAStrans("13 Служба GM_SchedulerSvc - не работает", ops, ProdwritePath);
                                            if (server != "")
                                            {
                                                string name_service = "GM_SchedulerSvc";
                                                string action_service = "Running";
                                                await Task.Run(() => Prod_Async_Power_Shell_Service(server, name_service, action_service, ops));
                                            }
                                            ProdStreamWriterEAStrans("14 Запуск GM_SchedulerSvc осуществлен", ops, ProdwritePath);
                                            ProdStreamWriterEAStrans("15 Транспорт в рабочем состоянии", ops, ProdwritePath);
                                            EndOperation(ops, "- транспорт работает +");
                                            СheckTransport = 1;
                                            action = 1;

                                        }
                                    }
                                    else
                                        ProdStreamWriterEAStrans("16 Поле для ввода пустое.\nВведите ip отделения почтовой связи.", ops, ProdwritePath);
                                }
                            }
                            else
                                ProdStreamWriterEAStrans("17 Транспорт в рабочем состоянии", ops, ProdwritePath);
                        }
                        else
                            ProdStreamWriterEAStrans("18 Поле для ввода пустое.\nВведите ip отделения почтовой связи.", ops, ProdwritePath);
                    }
                    else
                    {
                        ProdStreamWriterEAStrans("19 Время обновления файлов в папках:\nExport, Import на ОПС и In, Out в DAX - более 3 часов\nТранспорт не работает более 3 часов", ops, ProdwritePath);
                        if (ops != "248000" & ops != "249406")
                        {
                            string ip = server;
                            string result = "";
                            await Task.Run(() => Power_Shell_1("get-service -" +
                                               "DisplayName \"GMMQ\"" +
                                               " -ComputerName " + ip + "" +
                                               " | format-table Status -autosize", out result));
                            ProdStreamWriterEAStrans("20 Служба GMMQ - " + result.Replace("\r\n", ""), ops, ProdwritePath);
                            if (result.Replace("\r\n", "") == "Running")
                            {
                                if (server != "")
                                {
                                    await Task.Run(() => Power_Shell_1("get-service -" +
                                                       "DisplayName \"GM_SchedulerSvc\"" +
                                                       " -ComputerName " + ip + "" +
                                                       " | format-table Status -autosize", out result));
                                    ProdStreamWriterEAStrans("21 Служба GM_Scheduler - " + result.Replace("\r\n", ""), ops, ProdwritePath);
                                    if (result.Replace("\r\n", "") == "Running")
                                    {
                                        ProdStreamWriterEAStrans("22 Службы транспорта работают", ops, ProdwritePath);
                                        ProdStreamWriterEAStrans("23 Файлы старой реплики удалены", ops, ProdwritePath);
                                        DeleteFileReplicaAll(server, ops);
                                        СheckTransport = 0;
                                        ProdStreamWriterEAStrans("Запущен метод AutoStartExport(ops);", ops, ProdwritePath);
                                        AutoStartExport(ops, server);
                                    }
                                    else
                                    {
                                        ProdStreamWriterEAStrans("24 Служба GM_SchedulerSvc - не работает", ops, ProdwritePath);
                                        if (server != "")
                                        {
                                            string name_service = "GM_SchedulerSvc";
                                            string action_service = "Running";
                                            await Task.Run(() => Prod_Async_Power_Shell_Service(server, name_service, action_service, ops));
                                        }
                                        ProdStreamWriterEAStrans("25 Запуск GM_SchedulerSvc осуществлен", ops, ProdwritePath);
                                        ProdStreamWriterEAStrans("26 Службы транспорта работают", ops, ProdwritePath);
                                        ProdStreamWriterEAStrans("27 Файлы старой реплики удалены", ops, ProdwritePath);
                                        DeleteFileReplicaAll(server, ops);
                                        СheckTransport = 0;
                                        ProdStreamWriterEAStrans("Запущен метод AutoStartExport(ops);", ops, ProdwritePath);
                                        AutoStartExport(ops, server);
                                    }
                                }
                                else
                                    ProdStreamWriterEAStrans("28 Поле для ввода пустое.\nВведите ip отделения почтовой связи.", ops, ProdwritePath);
                            }
                            else
                            {
                                ProdStreamWriterEAStrans("29 Служба GMMQ - не работает", ops, ProdwritePath);
                                if (server != "")
                                {
                                    string name_service = "GMMQ";
                                    string action_service = "Running";
                                    await Task.Run(() => Prod_Async_Power_Shell_Service(server, name_service, action_service, ops));
                                }
                                ProdStreamWriterEAStrans("30 Запуск GMMQ осуществлен", ops, ProdwritePath);
                                if (server != "")
                                {
                                    await Task.Run(() => Power_Shell_1("get-service -" +
                                                       "DisplayName \"GM_SchedulerSvc\"" +
                                                       " -ComputerName " + ip + "" +
                                                       " | format-table Status -autosize", out result));
                                    ProdStreamWriterEAStrans("31 Служба GM_Scheduler - " + result.Replace("\r\n", ""), ops, ProdwritePath);
                                    if (result.Replace("\r\n", "") == "Running")
                                    {
                                        ProdStreamWriterEAStrans("32 Службы транспорта работают", ops, ProdwritePath);
                                        ProdStreamWriterEAStrans("33 Файлы старой реплики удалены", ops, ProdwritePath);
                                        DeleteFileReplicaAll(server, ops);
                                        СheckTransport = 0;
                                        ProdStreamWriterEAStrans("Запущен метод AutoStartExport(ops);", ops, ProdwritePath);
                                        AutoStartExport(ops, server);
                                    }
                                    else
                                    {
                                        ProdStreamWriterEAStrans("34 Служба GM_SchedulerSvc - не работает", ops, ProdwritePath);
                                        if (server != "")
                                        {
                                            string name_service = "GM_SchedulerSvc";
                                            string action_service = "Running";
                                            await Task.Run(() => Prod_Async_Power_Shell_Service(server, name_service, action_service, ops));
                                        }
                                        ProdStreamWriterEAStrans("35 Запуск GM_SchedulerSvc осуществлен", ops, ProdwritePath);
                                        ProdStreamWriterEAStrans("36 Файлы старой реплики удалены", ops, ProdwritePath);
                                        DeleteFileReplicaAll(server, ops);
                                        СheckTransport = 0;
                                        ProdStreamWriterEAStrans("Запущен метод AutoStartExport(ops);", ops, ProdwritePath);
                                        AutoStartExport(ops, server);
                                    }
                                }
                                else
                                    ProdStreamWriterEAStrans("37 Поле для ввода пустое.\nВведите ip отделения почтовой связи.", ops, ProdwritePath);
                            }
                        }
                        else
                        {
                            ProdStreamWriterEAStrans("38 Транспорт в рабочем состоянии", ops, ProdwritePath);
                            СheckTransport = 1;
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                ProdStreamWriterEAStrans("Ошибка", ops, ProdwritePath);
                ProdStreamWriterEAStrans("Ошибка, метод СheckEasTransport " + ex, ops, ProdwritePath);
                EndOperation(ops, "- ошибка, метод СheckEasTransport");
                СheckTransport = 1;
                action = 1;
            }
        }
        async public  void Prod_Async_Power_Shell_Service(string ip, string name_service, string action_service, string ops)
        {
            string service_prod = "[Service] - ";
            try
            {
                if (action_service == "Running")
                {
                    string action_power_shell = $"set-service {name_service} -ComputerName {ip} -Status {action_service} -PassThru | format-table Status -autosize";
                    Power_Shell(action_power_shell);
                    await Task.Delay(500);
                    Powershell_service_checked(ip, name_service, out string check_action, ops);
                    await Task.Delay(500);
                    Prod_Check_Service(check_action, ip, name_service, action_service, out string result, ops);
                    await Task.Delay(500);
                }
                if (action_service == "Stopped")
                {
                    string action_more = "";
                    Powershell_service_Force(ip, name_service, action_more);
                    await Task.Delay(500);
                    Powershell_service_checked(ip, name_service, out string check_action, ops);
                    await Task.Delay(500);
                    Prod_Check_Service(check_action, ip, name_service, action_service, out string result, ops);
                    await Task.Delay(500);
                }
                if (action_service == "Restart")
                {
                    action_service = "R-Stopped";
                    string action_more = "";
                    Powershell_service_Force(ip, name_service, action_more);
                    await Task.Delay(500);
                    string check_action = "";
                    string check_action_1 = "";
                    await Task.Run(() => Powershell_service_checked(ip, name_service, out check_action, ops));
                    await Task.Delay(500);
                    Prod_Check_Service(check_action, ip, name_service, action_service, out string result_1, ops);
                    await Task.Delay(500);
                    if (result_1 == "\r\nStopped\r\n\r\n\r\n")
                    {
                        action_service = "R-Running";
                        string action_power_shell = $"set-service {name_service} -ComputerName {ip} -Status {action_service.Substring(2)} -PassThru | format-table Status -autosize";
                        Power_Shell(action_power_shell);
                        await Task.Delay(500);
                        await Task.Run(() => Powershell_service_checked(ip, name_service, out check_action_1, ops));
                        await Task.Delay(500);
                        Prod_Check_Service(check_action_1, ip, name_service, action_service, out string result, ops);
                        await Task.Delay(500);
                        if (result == "\r\nRunning\r\n\r\n\r\n")
                        {
                            ProdStreamWriterEAStrans(service_prod + $"Служба \"{name_service}\"\nНа компьютере {ip} - Перезапущена", ops, ProdwritePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ProdStreamWriterEAStrans(service_prod + $"Ошибка из метода Async_Power_Shell_Service \nПк выключен или нет интернета\nИли запрашиваемая служба отсутствует на данном ПК\n\n\nОписание:\n\n{ex}", ops, ProdwritePath);
            }
        }
        public  void Powershell_service_checked(string ip, string name_service, out string check_action, string ops)
        {
            try
            {
                Process process1 = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = "/command get-service " +
                                      $"-DisplayName \"{name_service}\"" +
                                      " -ComputerName " + ip + "" +
                                      " | format-table Status -autosize",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                });
                string h = process1.StandardOutput.ReadToEnd();
                string k = h.Substring(18);
                if (k != "\r\nStopped\r\n\r\n\r\n" & k != "\r\nRunning\r\n\r\n\r\n")
                {
                    check_action = "Нет данных";
                }
                else if (k == "\r\nStopped\r\n\r\n\r\n")
                {
                    check_action = k;
                }
                else if (k == "\r\nRunning\r\n\r\n\r\n")  //else выполниться, исходя из значения последнего невыполненого if,
                                                          //на значения других if конструкция не распространяется
                {
                    check_action = k;
                }
                else
                    check_action = "Блок еlse";
            }
            catch (Exception ex)
            {
                ProdStreamWriterEAStrans($"Ошибка из метода Powershell_service \nПк выключен или нет интернета\nИли запрашиваемая служба отсутствует на данном ПК\n\n\nОписание:\n\n{ex}", ops, ProdwritePath);
                check_action = "Нет данных";
            }
        }
        public  void DeleteFileReplicaAll(string server, string ops)
        {
            try
            {
                string[] path_mass = new string[4]
                    {
                 @"\\" + server + @"\c$\GMMQ\Export",
                 @"\\" + server + @"\c$\GMMQ\Import",
                 "\\\\D01eascl02fskal\\gmmq\\EAS\\KAL\\" + ops + @"\In",
                 "\\\\D01eascl02fskal\\gmmq\\EAS\\KAL\\" + ops + @"\Out",
                    };
                for (int i = 0; i < path_mass.Length; i++)
                {
                    DirectoryInfo di = new DirectoryInfo(path_mass[i]);
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
            catch(Exception ex)
            {
                ProdStreamWriterEAStrans($"Ошибка из метода DeleteFileReplicaAll \n{ex}", ops, ProdwritePath);
            }

        }
        public  DateTime Get_Date_Time(string path, string ops, string ProdwritePath)
        {
                try
                {
                    DateTime dateTime = new DateTime();
                    dateTime = Directory.GetLastAccessTime(path);
                    return dateTime;
                }
                catch (Exception ex)
                {
                    ProdStreamWriterEAStrans($"Ошибка: \n{ex}", ops, ProdwritePath);
                    DateTime h = new DateTime(2015, 7, 20, 15, 30, 25);
                    return h;
                }
        }
        public  void Prod_Check_Service(string check_action, string ip, string name_service, string action_service, out string result, string ops)
        {
            string service_prod = "[Service] - ";
            if (action_service != "R-Stopped" | action_service != "R-Running")
            {
                result = check_action;
                if (check_action == $"\r\n{action_service}\r\n\r\n\r\n")
                {
                    result = check_action;
                    ProdStreamWriterEAStrans(service_prod + $"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service, ops)}", ProdwritePath);
                    ProdStreamWriterEAStrans(service_prod + check_action.Replace("\r\n", ""), ProdwritePath);
                }
                if (check_action == "Блок еlse")
                {
                    result = check_action;
                    ProdStreamWriterEAStrans(service_prod + $"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - {Action_service_info(action_service, ops)}", ProdwritePath);
                }
                if (check_action == "Нет данных")
                {
                    result = check_action;
                    ProdStreamWriterEAStrans(service_prod + check_action, ProdwritePath);
                }
                if (check_action == $"\r\n{Inversion_Action_service(action_service, ops)}\r\n\r\n\r\n")
                {
                    result = check_action;
                    ProdStreamWriterEAStrans(service_prod + $"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service + "-Error", ops)}", ProdwritePath);
                    ProdStreamWriterEAStrans(service_prod + Inversion_Action_service(action_service, ops), ProdwritePath);
                }
            }
            else
            {
                result = "";
                if (check_action == $"\r\nStopped\r\n\r\n\r\n")
                {
                    result = check_action;
                    ProdStreamWriterEAStrans(service_prod + check_action.Replace("\r\n", ""), ProdwritePath);
                }
                if (check_action == "Блок еlse")
                {
                    result = check_action;
                    ProdStreamWriterEAStrans(service_prod + $"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - {Action_service_info(action_service, ops)}", ProdwritePath);
                }
                if (check_action == "Нет данных")
                {
                    result = check_action;
                    ProdStreamWriterEAStrans(service_prod + check_action, ProdwritePath);
                }
                if (check_action == $"\r\n{Inversion_Action_service(action_service, ops)}\r\n\r\n\r\n")
                {
                    result = check_action;
                    ProdStreamWriterEAStrans(service_prod + $"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service + "-Error", ops)}", ProdwritePath);
                    ProdStreamWriterEAStrans(service_prod + Inversion_Action_service(action_service, ops), ProdwritePath);
                }
            }
        }
        public  string Inversion_Action_service(string action_service, string ops)
        {
            if (action_service == "Stopped")
                return "Running";
            if (action_service == "Running")
                return "Stopped";
            else
                return $"\nОшибка: action_service = {action_service}\nМетод Async_Power_Shell_Service далее Action_service_info";
        }


        //Метод для вывода состояния службы в MessageBox, исходя из значения переменной action_service
        public  string Action_service_info(string action_service, string ops)
        {
            if (action_service == "Stopped")
                return "Остановлена";
            if (action_service == "Stopped-Error")
                return "не остановилась\nОшибка остановки службы";
            if (action_service == "Running")
                return "Запущена";
            if (action_service == "Running-Error")
                return "не запустилась\nОшибка запуска службы";
            else
                return $"\nОшибка: action_service = {action_service}\nМетод Async_Power_Shell_Service далее Action_service_info";
        }
        public void AutoStartExport(string ops, string server1)
        {
            action = 0;
            Automodul = 1;
            if (server1 != "")
            {
                string file = "gmmq.packege.end";
                string file_2 = "meta.xml";
                string path = @"\\" + server1 + @"\c$\GMMQ\Export\" + file;
                string path_2 = @"\\" + server1 + @"\c$\GMMQ\Export\" + file_2;
                FileInfo fileInf = new FileInfo(path);
                FileInfo fileInf_2 = new FileInfo(path_2);
                if (fileInf.Exists | fileInf_2.Exists)
                {
                    string text = $"{ops} - толкнул транспорт";
                    ProdStreamWriterEAStrans(text, ops, ProdwritePath);
                }
                else
                {
                    action = 0;
                    Thread thread = new Thread(
                        () =>
                        {
                            SqlCommand command = new SqlCommand();
                            Prod_Start_Transport(command, ops, writePath);
                        });
                    thread.Start();
                    CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                }
            }
        }
    }
}
