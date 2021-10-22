﻿using System;
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
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        int timerCounter = 0; //счётчик для таймера
        int action = 0;
        int СheckTransport = 0;
        static DateTime currentDate = DateTime.Now;
        static string NewDateFormat = currentDate.ToString("dd-MM");
        public string writePath = @"C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Start_EAS_Trans\autoreader out\транспорт " + NewDateFormat + ".txt";
        public int streamwriteEAStrans = 0;
        public int i = 1;
        public string pathAutoEASTrans = @"C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Start_EAS_Trans\autoreader in\транспорт.txt";
        public bool isDataSaved;
        public int close = 0;
        public string ProdwritePath = @"C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Start_EAS_Trans\Prod\prod.txt";
        public int Automodul = 0;
        public string ops = "";

        public Form1(int stream)
        {
            streamwriteEAStrans = stream;
            BaseConstructor();
        }
        private void BaseConstructor()
        {
            try
            {
                InitializeComponent();
                Width = 844;
                Height = 574;
                this.ActiveControl = button11;
                textBox11.Focus();

                AutoCompleteStringCollection source = new AutoCompleteStringCollection() { };
                source.AddRange(ReadText(@"C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Start_EAS_Trans\save\eas all ops.txt"));
                textBox1.AutoCompleteCustomSource = source;
                textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

                this.Closing += new CancelEventHandler(this.Form1_Closing);
                this.isDataSaved = true;

                timer.Interval = 1000; //интервал между срабатываниями 1000 миллисекунд
                timer.Tick += new EventHandler(timer_Tick); //подписываемся на события Tick

                Button[] Mass_Button_View_Ops = new Button[2] { button2, button4 };
                Button[] Mass_Button_View_Stage = new Button[2] { button3, button5 };
                Button[] Mass_Button_View_Stage_In = new Button[1] { button3 };
                Button[] Mass_Button_View_Stage_Out = new Button[1] { button5 };
                Button[] Mass_Button_View = new Button[Mass_Button_View_Ops.Length + Mass_Button_View_Stage.Length];
                for (int i = 0; i < Mass_Button_View.Length; i++)
                {
                    if (i > -1 & i < 2)
                        Mass_Button_View[i] = Mass_Button_View_Ops[i];
                    if (i > 1 & i < 4)
                        Mass_Button_View[i] = Mass_Button_View_Stage[i - 2];
                }
                Button[] Mass_Button_Delete = new Button[4] { button16, button15, button14, button13 };
                Button[] Mass_Button = new Button[Mass_Button_View.Length + Mass_Button_Delete.Length];
                string[] path_mass = new string[4]
                {
                 @"\c$\GMMQ\Export",
                 @"\c$\GMMQ\Import",
                 "\\\\D01eascl02fskal\\gmmq\\EAS\\KAL\\1",
                 "\\\\D01eascl02fskal\\gmmq\\EAS\\KAL\\2",
                };
                for (int i = 0; i < Mass_Button.Length; i++)
                {
                    if (i > -1 & i < 4)
                        Mass_Button[i] = Mass_Button_View[i];
                    if (i > 3 & i < 8)
                        Mass_Button[i] = Mass_Button_Delete[i - 4];
                }
                foreach (Button button in Mass_Button_Delete)
                {
                    CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                             // отменяет отслеживание ошибок,
                                                             // но дает передать компоненты формы в другой поток 
                    button.Click += async (s, e) =>
                    {
                        for (int i = 0; i < Mass_Button_Delete.Length; i++)
                        {
                            if (button == Mass_Button_Delete[i])
                            {
                                if (textBox1.Text != "")
                                {
                                    string[] files = Directory.GetFiles(Replace_path(path_mass[i]));
                                    string[] catalog = Directory.GetDirectories(Replace_path(path_mass[i]));
                                    int all_elemetnts = files.Length + catalog.Length;
                                    if (all_elemetnts > 0)
                                    {
                                        DialogResult result = MessageBox.Show($"Удалить файлы ОПС {textBox1.Text}\nиз папки {Replace_path(path_mass[i])}?",
                                                        "Сообщение",
                                                         MessageBoxButtons.YesNo,
                                                         MessageBoxIcon.Information);
                                        if (result == DialogResult.Yes)
                                        {
                                            Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
                                            if (server != "")
                                            {
                                                await Task.Run(() => Delete_File(Replace_path(path_mass[i])));
                                                Async_View_Files(listBox1, label11, label15, label21, Replace_path(path_mass[i]), textBox12, server);
                                                Update_Files_and_Catolog_lenght(Replace_path(path_mass[i]));
                                            }
                                        }
                                        if (result == DialogResult.No)
                                        {
                                        }
                                    }
                                    else
                                        MessageBox.Show("Папка пустая");
                                }
                                else
                                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex}");
            }
        }
        public Form1() 
        {
            BaseConstructor();
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

        //Событие, описывающее логику работы таймера времени выполнения запроса
        void timer_Tick(object sender, EventArgs e)
        {
            string minutes = "";
            int int_minutes = 0;
            if (timerCounter < 61)
            {
                minutes = "";
                this.label23.Text = $"Время выполнения запроса: {minutes} {(++timerCounter) - 1 } сек.";
            }
            if (timerCounter >= 61)
            {
                for (int i = 1; i < 1000; i++)
                {
                    if (i == 31)
                    {
                        break;
                    }
                    if (timerCounter >= 60 * i + i & timerCounter < 60 * (i + 1) + (i + 1))
                    {
                        int y = 1;
                        int_minutes = i;
                        if (i == 1)
                        {
                            minutes = $"{int_minutes} минута";
                        }
                        if (i >= 2 & i <= 4)
                        {
                            minutes = $"{int_minutes} минуты";
                        }
                        if (i >= 5 & i <= 20)
                        {
                            minutes = $"{int_minutes} минут";
                        }
                        if (i == 10 + 10 * y + 1)
                        {
                            minutes = $"{int_minutes} минута";
                        }
                        if (i >= 12 + 10 * y & i <= 14 + 10 * y)
                        {
                            minutes = $"{int_minutes} минуты";
                        }
                        if (i >= 15 + 10 * y & i <= 20 + 10 * y)
                        {
                            minutes = $"{int_minutes} минут";
                        }
                        this.label23.Text = $"Время выполнения запроса: {minutes} {(++timerCounter) - (60 * i + (i + 1))} сек.";
                    }
                }
            }
        }

        //Кнопка толкнуть транспорт скриптом
        //Работа осуществляется в отдельном потоке от основной программы,
        //Сам поток начинается с того, что постоянно ищет файл meta.xml, в папке отделения,
        //Сигнализирующий о завершении SQL запроса "exec ReplicaExport 0", 
        //если файл найден, методу View_files(progressBar1, command, action);
        //передается переменная action, которая записана в его параметры, уже с другим значением и,
        //исходя из этого значения, метод View_files прекращает свою работу
        //Так же в потоке есть еще один метод, который именно и запускает SQL запрос "exec ReplicaExport 0"
        //Этот метод - Start_Transport(progressBar1, command);
        private void button1_Click(object sender, EventArgs e)
        {
            this.isDataSaved = false;
            if (textBox1.Text != "")
            {
                Name_DataBase_and_Server(textBox1.Text, out string server1, out string name_database1);
                string file = "gmmq.packege.end";
                string file_2 = "meta.xml";
                string path = @"\\" + server1 + @"\c$\GMMQ\Export\" + file;
                string path_2 = @"\\" + server1 + @"\c$\GMMQ\Export\" + file_2;
                FileInfo fileInf = new FileInfo(path);
                FileInfo fileInf_2 = new FileInfo(path_2);
                while (fileInf.Exists | fileInf_2.Exists)
                {
                    progressBar1.Value = 0;
                    textBox2.Clear();
                    textBox2.Text = textBox1.Text + " - выполнен скрипт exec ReplicaExport 0 ";
                    timerCounter = 0;
                    label22.Text = "Количество файлов реплики: ";
                }
                if(fileInf.Exists | fileInf_2.Exists)
                {
                    progressBar1.Value = 0;
                    textBox2.Clear();
                    textBox2.Text = textBox1.Text + " - выполнен скрипт exec ReplicaExport 0 ";
                    timerCounter = 0;
                    label22.Text = "Количество файлов реплики: ";
                }
                else
                {
                    timerCounter = 0;
                    try
                    {
                        DialogResult result = MessageBox.Show(
            $"Толкнуть транспорт скриптом,\nна ОПС {textBox1.Text} ? ",
            "Сообщение",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);

                        if (result == DialogResult.Yes)
                        {
                            this.timer.Start();
                            action = 0;
                            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                                     // отменяет отслеживание ошибок,
                                                                     // но дает передать компоненты формы в другой поток 

                            Thread thread = new Thread(
                                () =>
                                {                                                                    
                                    string ops = textBox1.Text;                                  
                                    SqlCommand command = new SqlCommand();                                  
                                    Start_Transport(progressBar1, command, ops, writePath);
                                    if (action != 1)
                                    {
                                        View_files(progressBar1, command, action, ops);
                                    }
                                });
                            thread.Start();
                            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                                     // отменяет отслеживание ошибок,
                                                                     // но дает передать компоненты формы в другой поток 
                        }
                        if (result == DialogResult.No)
                        {
                            this.timer.Stop();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: \n{ex}");
                        progressBar1.Value = 0;
                        this.timer.Stop();
                        textBox2.Text = textBox1.Text + " - ошибка";
                    }
                    textBox1.Focus();
                }
            }
            else
                MessageBox.Show("Поле для ввода номера ОПС - пустое");
        }
        public void AutoStartExport(string ops)
        {
            Automodul = 1;
            DateTime currentDate = DateTime.Now;
            string NewDateFormat = currentDate.ToString("dd-MM HH.mm.ss");
            string NewDateFormat1 = currentDate.ToString("HH.mm.ss");
            string writePath = @"C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Start_EAS_Trans\autoreader out\транспорт " + NewDateFormat + ".txt";
            textBox1.Text = "auto start export";
            Prod_Name_DataBase_and_Server(ops,  out string server1, out string name_database1);
            if (server1 != "")
            {
                СheckEasTransport(server1);
                if (СheckTransport != 1)
                {
                    string file = "gmmq.packege.end";
                    string file_2 = "meta.xml";
                    string path = @"\\" + server1 + @"\c$\GMMQ\Export\" + file;
                    string path_2 = @"\\" + server1 + @"\c$\GMMQ\Export\" + file_2;
                    FileInfo fileInf = new FileInfo(path);
                    FileInfo fileInf_2 = new FileInfo(path_2);
                    File.Create(@"C:\Users\Eduard.Karpov\source\repos\Start_EAS_Trans\Start_EAS_Trans\autoreader out\транспорт " + NewDateFormat + ".txt");
                    //while (fileInf.Exists | fileInf_2.Exists)
                    //{
                    //    progressBar1.Value = 0;
                    //    textBox2.Clear();
                    //    string text = $"{ops} - толкнул транспорт";
                    //    StreamWriterEAStrans(text);
                    //    timerCounter = 0;
                    //    label22.Text = "Количество файлов реплики: ";
                    //}
                    if (fileInf.Exists | fileInf_2.Exists)
                    {
                        progressBar1.Value = 0;
                        textBox2.Clear();
                        string text = $"{ops} - толкнул транспорт";
                        StreamWriterEAStrans(text);
                        timerCounter = 0;
                        label22.Text = "Количество файлов реплики: ";
                    }
                    else
                    {

                        try
                        {
                            DialogResult result = MessageBox.Show(
                $"Толкнуть транспорт скриптом,\nна ОПС {textBox1.Text} ? ",
                "Сообщение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);

                            if (result == DialogResult.Yes)
                            {
                                this.timer.Start();
                                action = 0;
                                CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                                         // отменяет отслеживание ошибок,
                                                                         // но дает передать компоненты формы в другой поток 

                                Thread thread = new Thread(
                                    () =>
                                    {
                                        SqlCommand command = new SqlCommand();
                                        Prod_Start_Transport(progressBar1, command, ops, writePath);
                                        //if (action != 1)
                                        //{
                                        //    View_files(progressBar1, command, action, ops);
                                        //}
                                    });
                                thread.Start();
                                CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                                         // отменяет отслеживание ошибок,
                                                                         // но дает передать компоненты формы в другой поток 
                            }
                            if (result == DialogResult.No)
                            {
                                this.timer.Stop();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка: \n{ex}");
                            progressBar1.Value = 0;
                            this.timer.Stop();
                            textBox2.Text = textBox1.Text + " - ошибка";
                        }
                        textBox1.Focus();
                    }
                }
            }
        }


        //Метод, использующий класс SqlConnection для создания запроса "exec ReplicaExport 0" на базу отделения 
        //connectionString = $"Server={server};Database={name_database};Persist Security Info=False;User ID=sa;Password=QweAsd123;";
        //
        async public void Start_Transport(ProgressBar progressBar, SqlCommand command, string ops, string writePath)
        {
            int action = 0;
            try
            {
                if (textBox1.Text != "")
                {
                    if (ops != "")
                    {
                        textBox1.Text = ops;
                    }                       
                    textBox2.Clear();
                    string server = "";
                    string name_database = "";
                    if (Automodul == 1)
                    {
                        Prod_Name_DataBase_and_Server(textBox1.Text, out server, out name_database);
                    }
                    if (Automodul == 0)
                    {
                        Name_DataBase_and_Server(textBox1.Text, out server, out name_database);
                    }
                    string connectionString = $"Server={server};Database={name_database};Persist Security Info=False;User ID=sa;Password=QweAsd123;";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();
                        textBox2.Text = "Подключение открыто";
                    }
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();
                        command.CommandText = "exec ReplicaExport 0";
                        command.Connection = connection;
                        command.CommandTimeout = 5000; //увеличено время на выполнение команды
                        await Task.Run(() => command.ExecuteNonQueryAsync());
                        action = 1;
                        Action(action);
                        this.action = 1;
                        this.timer.Stop();
                        this.isDataSaved = true;
                        textBox2.Text = "Скрипт выполнен";
                        progressBar.Value = 100;
                        MessageBox.Show($"Запрос в SQL: \"{command.CommandText}\" - успешно отработан");
                        string text = $"{ops} - толкнул скриптом +";
                        StreamWriterEAStrans(text);
                        progressBar.Value = 0;
                        textBox2.Clear();
                        textBox2.Text = textBox1.Text + " - выполнен скрипт exec ReplicaExport 0 ";
                        timerCounter = 0;
                        label22.Text = "Количество файлов реплики: ";
                    }
                }
                else
                    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
                action = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
                progressBar.Value = 0;
                this.timer.Stop();
                textBox2.Text = textBox1.Text + " - ошибка";
            }
        }
        async public void Prod_Start_Transport(ProgressBar progressBar, SqlCommand command, string ops, string writePath)
        {
            int action = 0;
            try
            {
                if (textBox1.Text != "")
                {
                    if (ops != "")
                    {
                        textBox1.Text = ops;
                    }
                    textBox2.Clear();
                    string server = "";
                    string name_database = "";
                    if (Automodul == 1)
                    {
                        Prod_Name_DataBase_and_Server(textBox1.Text, out server, out name_database);
                    }
                    string connectionString = $"Server={server};Database={name_database};Persist Security Info=False;User ID=sa;Password=QweAsd123;";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();
                        ProdStreamWriterEAStrans("Подключение открыто");
                    }
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();
                        command.CommandText = "exec ReplicaExport 0";
                        command.Connection = connection;
                        command.CommandTimeout = 5000; //увеличено время на выполнение команды
                        await Task.Run(() => command.ExecuteNonQueryAsync());
                        action = 1;
                        Action(action);
                        this.action = 1;
                        this.timer.Stop();
                        this.isDataSaved = true;
                        ProdStreamWriterEAStrans("Скрипт выполнен");
                        //progressBar.Value = 100;
                        ProdStreamWriterEAStrans($"Запрос в SQL: \"{command.CommandText}\" - успешно отработан");
                        string text = $"{ops} - толкнул скриптом +";
                        StreamWriterEAStrans(text);
                        //progressBar.Value = 0;
                        //textBox2.Clear();
                        //textBox2.Text = textBox1.Text + " - выполнен скрипт exec ReplicaExport 0 ";
                        timerCounter = 0;
                        //label22.Text = "Количество файлов реплики: ";
                    }
                }
                else
                    ProdStreamWriterEAStrans($"\nПоле для ввода номера ОПС - пустое");
                action = 1;
            }
            catch (Exception ex)
            {
                this.timer.Stop();
                ProdStreamWriterEAStrans( textBox1.Text + $" - ошибка \n{ex}");
            }
        }
        public int Action(int action)
        {
            if (action == 1)
            {
                int p = action;
                return p;
            }
            else
            {
                int p = action;
                return p;
            }
        }

        //Метод, следящий за появлением новых файлов в папке отделения, по фиксированному пути 
        //@"\\" + server + @"\c$\GMMQ\Export"
        async public void View_files(ProgressBar progressBar, SqlCommand command, int action, string ops)
        {
            string server = "";
            try
            {
                CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                         // отменяет отслеживание ошибок,
                                                         // но дает передать компоненты формы в другой поток 

                if (ops != "")
                {
                    textBox1.Text = ops;
                }
                if (Automodul == 1)
                {
                    Prod_Name_DataBase_and_Server(textBox1.Text, out string serverEAS, out string name_database);
                    server = serverEAS;
                }
                if (Automodul == 0)
                {
                    Name_DataBase_and_Server(textBox1.Text, out string serverEAS, out string name_database);
                    server = serverEAS;
                }
                string path = @"\\" + server + @"\c$\GMMQ\Export";
                string[] files = Directory.GetFiles(path);
                string[] files1 = Directory.GetFiles(path);
                string[] catalog = Directory.GetDirectories(path);
                int count = 1;
                int u = 0;
                textBox12.Text = path;
                string[] mass_2 = new string[count];
                if (action != 1 & this.action != 1)
                {
                    while (files.Length == 0)
                    {
                        catalog = Directory.GetDirectories(path);
                        if (catalog.Length == 0)
                        {
                            files = Directory.GetFiles(path);
                            for (int i = 0; i < files.Length; i++)
                            {
                                if (files[i] != "meta.xml" | files[i] != "gmmq.packege.end")
                                
                                {
                                    files = Directory.GetFiles(path); //перезаписывает переменную, для обновления количества файлов в папке
                                    label22.Text = "Количество файлов реплики: 0";
                                }
                                else
                                {
                                    //await Task.Delay(200000000);
                                    //action = 1;
                                    //break;
                                }
                            }
                            await Task.Delay(200);
                        }
                        else
                        {
                            action = 1;
                            //await Task.Delay(200000000);
                        }
                    }
                    while (files.Length != 0)
                    {
                        if (action != 1 & this.action != 1)
                        {
                            count++;
                            string[] files_1 = Directory.GetFiles(path);
                            int file3 = files.Length;
                            DateTime dateTime = new DateTime();
                            dateTime = Directory.GetLastWriteTime(path); //перезаписывает переменную, для обновления даты изменения папки
                            FileInfo file = new FileInfo(path);
                            if (dateTime.AddSeconds(3) > DateTime.Now)
                            {
                                catalog = Directory.GetDirectories(path);
                                if (catalog.Length == 0)
                                {
                                    string[] mass = new string[files_1.Length];
                                    if (u == 0 & this.action != 1)
                                    {
                                        u++;
                                        mass = files_1;
                                        listBox2.Items.Clear();
                                        for (int i = 0; i < files_1.Length; i++)
                                        {
                                            if (files_1[i] != "meta.xml" | files_1[i] != "gmmq.packege.end")
                                            {
                                                label22.Text = "Количество файлов реплики: " + files_1.Length + "\nПо пути: " + files_1[i];
                                                listBox2.Items.Add(files_1[i].Substring(18 + server.Length));
                                                if (progressBar.Value >= 0 & progressBar.Value < 99)
                                                {
                                                    double p = files_1.Length;
                                                    double b = p / 1.8;
                                                    if (b < 99)
                                                    {
                                                        progressBar.Value = Convert.ToInt32(b);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                textBox2.Text = textBox1.Text + " - выполнен скрипт exec ReplicaExport 0 ";
                                                action = 1;
                                                break;
                                            }
                                        }
                                    }
                                    if (u == 1 & this.action != 1)
                                    {   
                                        listBox2.BeginUpdate();
                                        listBox2.Items.Clear();
                                        for (int i = 0; i < files_1.Length; i++)
                                        {
                                            if (files_1[i] != "meta.xml" | files_1[i] != "gmmq.packege.end")
                                            {
                                                listBox2.Items.Add(files_1[i].Substring(18 + server.Length));                                              
                                            }
                                            else
                                            {
                                                textBox2.Text = textBox1.Text + " - выполнен скрипт exec ReplicaExport 0 ";
                                                action = 1;
                                                break;
                                            }
                                        }
                                        listBox2.EndUpdate();
                                        for (int i = 0; i < files_1.Length; i++)
                                        {
                                            if (progressBar.Value >= 0 & progressBar.Value < 99)
                                            {
                                                double p = files_1.Length;
                                                double b = p / 1.8;
                                                if (b < 99)
                                                {
                                                    progressBar.Value = Convert.ToInt32(b);
                                                }
                                            }
                                            if (mass[i] != files_1[i])
                                            {
                                                mass = Addition_Mass(mass, files_1);
                                                label22.Text = "Количество файлов реплики: " + files_1.Length + "\nПо пути: " + files_1[i];
                                            }
                                        }
                                        }
                                        Update_Files_and_Catolog_lenght(path, label25,label26,label27);
                                }
                                else
                                    await Task.Delay(40000000);
                            }
                            await Task.Delay(400);
                        }
                        else
                        {
                            progressBar.Value = 0;
                            this.timer.Stop();
                            textBox2.Text = textBox1.Text + " - выполнен скрипт exec ReplicaExport 0 ";
                            break;
                        }
                    }
                }
                else
                {
                    progressBar.Value = 0;
                    this.timer.Stop();
                    textBox2.Text = textBox1.Text + " - выполнен скрипт exec ReplicaExport 0 ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex}");
                progressBar.Value = 0;
                this.timer.Stop();
                textBox2.Text = textBox1.Text + " - ошибка";
            }
        }

        public string[] Addition_Mass(string[] mass1, string[] mass2)
        {
            string[] mass3 = new string[mass1.Length + mass2.Length];
            for (int i = 0; i < mass3.Length; i++)
            {
                if (i > -1 & i < mass1.Length)
                    mass3[i] = mass1[i];
                if (i > (mass1.Length-1) & i < mass3.Length)
                    mass3[i] = mass2[i - mass1.Length];
            }
            return mass3;
        }

        public string[] Checked(string[] mass_1, int count, string[] mass_2, out string[] mass_3)
        {
            mass_3 = new string[0];

            if (count == 0)
            {
                mass_2 = mass_1;
                return mass_2;
            }
            if (count > 0)
            {
                return (string[])mass_1.Except(mass_2);
            }
            return mass_1;
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

        //Метод, находящий по номеру отделения, его ip адрес и имя его основной базы данных ЕАС 
        public void Name_DataBase_and_Server(string numder_ops, out string server, out string name_database)
        {
            ops = textBox1.Text;
            name_database = "DB" + numder_ops;
            server = "";
            string text = $"{numder_ops} - не подключается";
            FileInfo fileInf = new FileInfo(writePath);
            if (fileInf.Exists)
            {              
                if (textBox1.Text != "")
                {
                    try
                    {
                        if (numder_ops != "222222") //Проверка на тестовый контур 
                        {
                            IPAddress ipAddress = Dns.GetHostEntry("r40-" + numder_ops + "-n").AddressList[0];
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
                                    & pingReply.Address.ToString() != "10.94.206.69")
                                {
                                    server = pingReply.Address.ToString();
                                    System.Threading.Thread.Sleep(200);
                                }
                                else
                                {
                                    MessageBox.Show($"\nКоманда пинг - не проходит.");
                                    textBox2.Text = textBox1.Text + " - не подключается";
                                    server = "";
                                    StreamWriterEAStrans(text);
                                }
                            }
                            else
                            {
                                MessageBox.Show($"\nКоманда пинг - не проходит.");
                                textBox2.Text = textBox1.Text + " - не подключается";
                                server = "";
                                StreamWriterEAStrans(text);
                            }
                        }
                        else
                            server = @"D:\!localhost";
                    }
                    catch (PingException ex)
                    {
                        MessageBox.Show($"\nКоманда пинг - не проходит.\n{ex.Message}");
                        textBox2.Text = textBox1.Text + " - не подключается";
                        server = "";
                        StreamWriterEAStrans(text);
                    }
                    catch (SocketException)
                    {
                        MessageBox.Show("\nКоманда пинг - не проходит.\nCould not resolve host name.");
                        textBox2.Text = textBox1.Text + " - не подключается";
                        server = "";
                        StreamWriterEAStrans(text);
                    }

                    catch (ArgumentNullException)
                    {
                        MessageBox.Show("\nКоманда пинг - не проходит.\nPlease enter the host name or IP address to ping.");
                        textBox2.Text = textBox1.Text + " - не подключается";
                        server = "";
                        StreamWriterEAStrans(text);
                    }
                    catch (System.Net.NetworkInformation.NetworkInformationException)
                    {
                        MessageBox.Show($"\nКоманда пинг - не проходит.\nПк ОПС {textBox1.Text} - выключен или без интернета");
                        textBox2.Text = textBox1.Text + " - не подключается";
                        server = "";
                        StreamWriterEAStrans(text);
                    }
                    catch (NullReferenceException)
                    {
                        MessageBox.Show($"\nКоманда пинг - не проходит.\nПк ОПС {textBox1.Text} - выключен или без интернета");
                        textBox2.Text = textBox1.Text + " - не подключается";
                        server = "";
                        StreamWriterEAStrans(text);
                    }
                }
                else
                    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое\n(Метод Name_DataBase_and_Server)");
            }
            else
            {
                File.Create(writePath);
                MessageBox.Show($"\nСоздался файл записи\nПо пути: {writePath}\n(Метод Name_DataBase_and_Server)");
            }
        }

        public void StreamWriterEAStrans(string text)
        {
            if (streamwriteEAStrans == 1)
            {
                using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    sw.WriteLineAsync(text);
                }
            }
        }
        public void ProdStreamWriterEAStrans(string text)
        {
            //if (streamwriteEAStrans == 1)
            //{
                using (StreamWriter sw = new StreamWriter(ProdwritePath, true, System.Text.Encoding.Default))
                {
                    sw.WriteLineAsync(text);
                }
            //}
        }
        public void Prod_Name_DataBase_and_Server(string number_ops, out string server, out string name_database)
        {
            string text = $"{number_ops} - не подключается";
            name_database = "DB" + number_ops;
            server = "";
            if (textBox1.Text != "")
            {

                try
                {
                    if (textBox1.Text == "auto start export")
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
                                    & pingReply.Address.ToString() != "10.94.206.69")
                                {
                                server = pingReply.Address.ToString();
                                System.Threading.Thread.Sleep(200);
                            }
                            else
                            {
                                ProdStreamWriterEAStrans(text);
                                StreamWriterEAStrans(text);
                                server = "";
                            }

                        }
                        else
                        {
                            ProdStreamWriterEAStrans(text);
                            StreamWriterEAStrans(text);
                            server = "";
                        }

                    }
                }
                catch (PingException)
                {
                    ProdStreamWriterEAStrans(text);
                    StreamWriterEAStrans(text);
                    server = "";
                }
                catch (SocketException)
                {
                    ProdStreamWriterEAStrans(text);
                    StreamWriterEAStrans(text);
                    server = "";
                }

                catch (ArgumentNullException)
                {
                    ProdStreamWriterEAStrans(text);
                    StreamWriterEAStrans(text);
                    server = "";
                }
                catch (System.Net.NetworkInformation.NetworkInformationException)
                {
                    ProdStreamWriterEAStrans(text);
                    StreamWriterEAStrans(text);
                    server = "";
                }
                catch (NullReferenceException)
                {
                    ProdStreamWriterEAStrans(text);
                    StreamWriterEAStrans(text);
                    server = "";
                }
            }
            else
                MessageBox.Show($"\nПоле для ввода номера ОПС - пустое\n(Метод Name_DataBase_and_Server)");
        }

        //Функция возвращающая дату последнего доступа к папке по заданному пути,
        //в виде строки  
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

        //Функция возвращающая дату последнего доступа к папке по заданному пути,
        //в виде переменной, типа dateTime
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

        //Метод добавляющий имена папок в листбокс, в зависимости от заданного пути и ip сервера 
        //Метод имеет выходной параметр типа List, в который записывает результат,
        //Метод использует метод Find, который уже в свою очередь добавляет файлы в листбокс
        public void FR(string path, string server, out List<string> files_list)
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

        //Метод добавляющий файлы в листбокс по заданному пути и маске
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



        //Просмотр даты обновления папок на ОПС
         private void button6_Click(object sender, EventArgs e)
        {
            Async_Date_View_Ops(@"\c$\GMMQ\Export", textBox4);
        }
         private void button7_Click(object sender, EventArgs e)
        {
            Async_Date_View_Ops(@"\c$\GMMQ\Import", textBox5);        
        }

        // Просмотр даты обновления папок на Даксе(стейдже)
         private void button9_Click(object sender, EventArgs e)
        {
            Async_Date_View_Stage(@"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox1.Text + @"\In", textBox7);          
        }
         private void button8_Click(object sender, EventArgs e)
        {
            Async_Date_View_Stage(@"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox1.Text + @"\Out", textBox6);          
        }

        //Метод просмотра даты обновления папок на отделении
       async public void Async_Date_View_Ops(string path, TextBox view)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    string server = "";
                    string name_database = "";
                    Name_DataBase_and_Server(textBox1.Text, out server, out name_database);
                    if (server != "")
                    {
                        string path_1 = @"\\" + server + path;
                        view.Clear();
                        view.Text = $"{path.Substring(9)} - " + (Get_Files_Time(path_1));
                        await Task.Delay(100);
                    }
                }
                else
                    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex.Message}");
            }
        }

        //перегрузка Метода просмотра даты обновления папок на отделении
        async public void Async_Date_View_Ops(string path, TextBox view, int action)
        {
            try
            {
                if (action == 1)
                {
                    if (textBox1.Text != "")
                    {
                        string server = "";
                        string name_database = "";
                        Name_DataBase_and_Server(textBox1.Text,  out server, out name_database);
                        if (server != "")
                        {
                            string path_1 = @"\\" + server + path;
                            view.Clear();
                            view.Text = $"{path.Substring(9)} - " + (Get_Files_Time(path_1));
                            await Task.Delay(100);
                        }
                    }
                    else
                        MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex.Message}");
            }
        }

        //Метод просмотра даты обновления папок на даксе
        async public void Async_Date_View_Stage(string path, TextBox view)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    view.Clear();
                    view.Text = $"{path.Substring(38)} - " + (Get_Files_Time(path));
                    await Task.Delay(100);
                }
                else
                    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex.Message}");
            }
        }

        //перегрузка Метода просмотра даты обновления папок на даксе
        async public void Async_Date_View_Stage(string path, TextBox view, int action)
        {
            try
            {
                if (action == 1)
                {
                    if (textBox1.Text != "")
                    {
                        view.Clear();
                        view.Text = $"{path.Substring(38)} - " + (Get_Files_Time(path));
                        await Task.Delay(100);
                    }
                    else
                        MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex.Message}");
            }
        }

        //Просмотр обновления сразу всех папок
        async private void button10_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
                if (server != "")
                {
                    Async_Date_View_Ops(@"\c$\GMMQ\Export", textBox4,1);
                    Async_Date_View_Ops(@"\c$\GMMQ\Import", textBox5,1);
                }
                await Task.Delay(100);
                if (server == "")
                {
                    textBox4.Clear();
                    textBox5.Clear();
                }
                Async_Date_View_Stage(@"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox1.Text + @"\In", textBox7,1);
                Async_Date_View_Stage(@"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox1.Text + @"\Out", textBox6,1);
            }
            else
                MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
        }

        //Кнопка PING
        async private void button11_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                try
                {
                    textBox8.Clear();
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

        //Службы ЕАС
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

        //Метод для просмотра определенных файлов и попок по маске и задонному пути,
        //так же определяет их количество, количество папок, количество всех элементов по заданному пути
        //Добавляет их в листбокс и количество выводит в лабел
        async public void Async_View_Files(ListBox listBox, Label all_files, Label all_catalogs, Label all_elemetnts, string path, TextBox way, string server)
        {
            try
            {
                way.Clear();
                if (server != "")
                {
                    way.Text = path;
                    listBox.Items.Clear();
                    await Task.Run(() => FR(path, server, out List<string> files_list));
                    string[] files = Directory.GetFiles(path);
                    all_files.Text = "Всего файлов: " + files.Length.ToString();
                    string[] catalog = Directory.GetDirectories(path);
                    all_catalogs.Text = "Всего папок: " + catalog.Length.ToString();
                    all_elemetnts.Text = $"Всего элементов: {files.Length + catalog.Length}";
                }
                if (server == "")
                {
                    if (path == "\\\\D01eascl02fskal\\gmmq\\EAS\\KAL\\" + textBox1.Text + "\\In" | path == "\\\\D01eascl02fskal\\gmmq\\EAS\\KAL\\" + textBox1.Text + "\\Out")
                    {
                        way.Text = path;
                        listBox.Items.Clear();
                        await Task.Run(() => FR(path, server, out List<string> files_list));
                        string[] files = Directory.GetFiles(path);
                        all_files.Text = "Всего файлов: " + files.Length.ToString();
                        string[] catalog = Directory.GetDirectories(path);
                        all_catalogs.Text = "Всего папок: " + catalog.Length.ToString();
                        all_elemetnts.Text = $"Всего элементов: {files.Length + catalog.Length}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex}");
            }
        }

        //Метод для нахождения количеста файлов, количества папок, количества всех элементов по заданному пути
        public void Update_Files_and_Catolog_lenght(string path)
        {
            string[] files = Directory.GetFiles(path);
            label10.Text = "Всего файлов: " + files.Length.ToString();
            string[] catalog = Directory.GetDirectories(path);
            label14.Text = "Всего папок: " + catalog.Length.ToString();
            label21.Text = $"Всего элементов: {files.Length + catalog.Length}";
        }
        public void Update_Files_and_Catolog_lenght(string path, Label labelallfile , Label allcatalog , Label allelement)
        {
            string[] files = Directory.GetFiles(path);
            labelallfile.Text = "Файлов: " + files.Length.ToString();
            string[] catalog = Directory.GetDirectories(path);
            allcatalog.Text = "Папок: " + catalog.Length.ToString();
            allelement.Text = $"Элементов: {files.Length + catalog.Length}";
        }

        //Функция позволяющая выводить откорректированный путь, для дальнейшего его использования в методах
        public string Replace_path(string path)
        {
            Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
            if (server == @"D:\!localhost")
            {
                if (path == @"\c$\GMMQ\Export")
                    return server + @"\Export";
                if (path == @"\c$\GMMQ\Import")
                    return server + @"\Import";
                if (path == "\\\\D01eascl02fskal\\gmmq\\EAS\\KAL\\1")
                    return server + @"\In";
                if (path == "\\\\D01eascl02fskal\\gmmq\\EAS\\KAL\\2")
                    return server + @"\Out";
            }
            if (server != @"D:\!localhost")
            {
                if (path == @"\c$\GMMQ\Export" | path == @"\c$\GMMQ\Import")
                    return @"\\" + server + path;
                else
                {
                    if (path == "\\\\D01eascl02fskal\\gmmq\\EAS\\KAL\\1")
                    {
                        string path_1 = "\\\\D01eascl02fskal\\gmmq\\EAS\\KAL\\" + textBox1.Text + "\\In";
                        return path_1;
                    }
                    if (path == "\\\\D01eascl02fskal\\gmmq\\EAS\\KAL\\2")
                    {
                        string path_1 = "\\\\D01eascl02fskal\\gmmq\\EAS\\KAL\\" + textBox1.Text + "\\Out";
                        return path_1;
                    }
                }
                return path;
            }
            return path;
        }

        //Метод, удаляющий файлы и папки по заданному пути и маске,
        //использует свою вторую версию, которая удаляет только файлы по маске и пути
        public void Delete_File(string path)
        {
            if (textBox1.Text != "")
            {
                try
                {
                    string[] files = new string[1];
                    string[] catalog_1 = Directory.GetDirectories(path);
                    if (files.Length + catalog_1.Length != 0)
                    {
                        string[] catalog = Directory.GetDirectories(path);
                        if (catalog.Length != 0)
                        {
                            for (int i = 0; i < catalog.Length; i++)
                            {
                                DirectoryInfo dir = new DirectoryInfo(catalog[i]);
                                dir.Delete(true); //значение true для удаления папки со всеми содержащимися в ней файлами
                                                  
                            }
                        }
                        string[] mask = new string[6] { "*.dat", "*.fi", "*.fe", "*.lock", "*.xml", "*.end" };
                        for (int i = 0; i < mask.Length; i++)
                        {
                            Delete_File(path, mask[i]);
                        }
                    }
                    else
                        MessageBox.Show($"\nПапка пустая, элементов в папке - 0\nудаление - не произведено");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"\nОшибка: \n{ex}");
                }
            }
            else
                MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
        }
        public void DeleteFileReplicaAll()
        {
            string[] path_mass = new string[4]
                {
                 @"\c$\GMMQ\Export",
                 @"\c$\GMMQ\Import",
                 "\\\\D01eascl02fskal\\gmmq\\EAS\\KAL\\1",
                 "\\\\D01eascl02fskal\\gmmq\\EAS\\KAL\\2",
                };
            for (int i = 0; i < path_mass.Length; i++)
            {
                Delete_File(path_mass[i]);
            }
            
        }

        //Метод удаляет только файлы по заданной маске и пути
        //Вторая версия метода  Delete_File, отличающаяся по параметрам
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


        //Кнопка проверка работы транспорта, представлена логикой условий и рассмотрения различных вариантов поведения,
        //зависит от состояния служб транспорта, времени обновления в папках, подключения к отделению
        async private void button17_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                         // отменяет отслеживание ошибок,
                                                         // но дает передать компоненты формы в другой поток 
                DateTime now = new DateTime();
                now = DateTime.Now;
                Name_DataBase_and_Server(textBox1.Text,out string server, out string name_database);
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
      async public void СheckEasTransport(string server)
        {
            СheckTransport = 1;
            if (textBox1.Text != "")
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
                Get_Date_Time(@"\\" + server + @"\c$\GMMQ\Export");
                DateTime export = Get_Date_Time(@"\\" + server + @"\c$\GMMQ\Export").AddHours(3);
                DateTime inport = Get_Date_Time(@"\\" + server + @"\c$\GMMQ\Import").AddHours(3);
                DateTime In = Get_Date_Time(@"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox1.Text + @"\In").AddHours(3);
                DateTime Out = Get_Date_Time(@"\\D01eascl02fskal\gmmq\EAS\KAL\" + textBox1.Text + @"\Out").AddHours(3);
                //if (now < export & now < inport & now < In & now < Out)
                if (now < In & now < Out)
                {
                    string text = "Время обновления папок актуальное";
                    ProdStreamWriterEAStrans(text);
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
                            //textBox3.Text = "Служба GMMQ - " + result.Replace("\r\n", "");
                            ProdStreamWriterEAStrans("Служба GMMQ - " + result.Replace("\r\n", ""));
                            if (result.Replace("\r\n", "") == "Running")
                            {
                                if (server != "")
                                {
                                    await Task.Run(() => Power_Shell_1("get-service -" +
                                                       "DisplayName \"GM_SchedulerSvc\"" +
                                                       " -ComputerName " + ip + "" +
                                                       " | format-table Status -autosize", out result));
                                    ProdStreamWriterEAStrans("Служба GM_Scheduler - " + result.Replace("\r\n", ""));
                                    if (result.Replace("\r\n", "") == "Running")
                                    {
                                        ProdStreamWriterEAStrans("Службы транспорта работают");
                                        ProdStreamWriterEAStrans("Транспорт в рабочем состоянии");
                                        СheckTransport = 1;
                                    }
                                    else
                                    {
                                        ProdStreamWriterEAStrans("Служба GM_SchedulerSvc - не работает");
                                        if (server != "")
                                        {
                                            textBox11.Clear();
                                            string name_service = "GM_SchedulerSvc";
                                            string action_service = "Running";
                                            await Task.Run(() => Prod_Async_Power_Shell_Service(server, name_service, action_service));
                                        }
                                        ProdStreamWriterEAStrans("Запуск GM_SchedulerSvc осуществлен");
                                        СheckTransport = 1;
                                    }
                                }
                                else
                                    ProdStreamWriterEAStrans("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
                            }
                            else
                            {
                                ProdStreamWriterEAStrans("Служба GMMQ - не работает");
                                if (server != "")
                                {
                                    textBox10.Clear();                    
                                    string name_service = "GMMQ";
                                    string action_service = "Running";
                                    await Task.Run(() => Prod_Async_Power_Shell_Service(server, name_service, action_service));
                                }
                                ProdStreamWriterEAStrans("Запуск GMMQ осуществлен");
                                if (server != "")
                                {
                                    await Task.Run(() => Power_Shell_1("get-service -" +
                                                       "DisplayName \"GM_SchedulerSvc\"" +
                                                       " -ComputerName " + ip + "" +
                                                       " | format-table Status -autosize", out result));
                                    ProdStreamWriterEAStrans("Служба GM_Scheduler - " + result.Replace("\r\n", ""));
                                    if (result.Replace("\r\n", "") == "Running")
                                    {
                                        ProdStreamWriterEAStrans("Службы транспорта работают");
                                        ProdStreamWriterEAStrans("Транспорт в рабочем состоянии");
                                        СheckTransport = 1;
                                    }
                                    else
                                    {
                                        ProdStreamWriterEAStrans("Служба GM_SchedulerSvc - не работает");
                                        if (server != "")
                                        {
                                            textBox11.Clear();
                                            string name_service = "GM_SchedulerSvc";
                                            string action_service = "Running";
                                            await Task.Run(() => Prod_Async_Power_Shell_Service(server, name_service, action_service));
                                        }
                                        ProdStreamWriterEAStrans("Запуск GM_SchedulerSvc осуществлен");
                                        СheckTransport = 1;
                                    }
                                }
                                else
                                    ProdStreamWriterEAStrans("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
                            }
                        }
                        else
                            ProdStreamWriterEAStrans("Транспорт в рабочем состоянии");
                    }
                    else
                        ProdStreamWriterEAStrans("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
                }
                else
                {
                    ProdStreamWriterEAStrans("Время обновления файлов в папках:\nExport, Import на ОПС и In, Out в DAX - более 3 часов\nТранспорт не работает более 3 часов");
                    if (textBox1.Text != "248000" & textBox1.Text != "249406")
                    {
                        string ip = server;
                        string result = "";
                        await Task.Run(() => Power_Shell_1("get-service -" +
                                           "DisplayName \"GMMQ\"" +
                                           " -ComputerName " + ip + "" +
                                           " | format-table Status -autosize", out result));
                        //textBox3.Text = "Служба GMMQ - " + result.Replace("\r\n", "");
                        ProdStreamWriterEAStrans("Служба GMMQ - " + result.Replace("\r\n", ""));
                        if (result.Replace("\r\n", "") == "Running")
                        {
                            if (server != "")
                            {
                                await Task.Run(() => Power_Shell_1("get-service -" +
                                                   "DisplayName \"GM_SchedulerSvc\"" +
                                                   " -ComputerName " + ip + "" +
                                                   " | format-table Status -autosize", out result));
                                ProdStreamWriterEAStrans("Служба GM_Scheduler - " + result.Replace("\r\n", ""));
                                if (result.Replace("\r\n", "") == "Running")
                                {
                                    ProdStreamWriterEAStrans("Службы транспорта работают");
                                    ProdStreamWriterEAStrans("Файлы старой реплики удалены");
                                    DeleteFileReplicaAll();
                                    СheckTransport = 0;
                                }
                                else
                                {
                                    ProdStreamWriterEAStrans("Служба GM_SchedulerSvc - не работает");
                                    if (server != "")
                                    {
                                        textBox11.Clear();
                                        string name_service = "GM_SchedulerSvc";
                                        string action_service = "Running";
                                        await Task.Run(() => Prod_Async_Power_Shell_Service(server, name_service, action_service));
                                    }
                                    ProdStreamWriterEAStrans("Запуск GM_SchedulerSvc осуществлен");
                                    ProdStreamWriterEAStrans("Службы транспорта работают");
                                    ProdStreamWriterEAStrans("Файлы старой реплики удалены");
                                    DeleteFileReplicaAll();
                                    СheckTransport = 0;
                                    //textBox3.Text = "Транспорт не работает";
                                }
                            }
                            else
                                ProdStreamWriterEAStrans("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
                        }
                        else
                        {
                            ProdStreamWriterEAStrans("Служба GMMQ - не работает");
                            if (server != "")
                            {
                                textBox10.Clear();
                                string name_service = "GMMQ";
                                string action_service = "Running";
                                await Task.Run(() => Prod_Async_Power_Shell_Service(server, name_service, action_service));
                            }
                            ProdStreamWriterEAStrans("Запуск GMMQ осуществлен");
                            if (server != "")
                            {
                                await Task.Run(() => Power_Shell_1("get-service -" +
                                                   "DisplayName \"GM_SchedulerSvc\"" +
                                                   " -ComputerName " + ip + "" +
                                                   " | format-table Status -autosize", out result));
                                ProdStreamWriterEAStrans("Служба GM_Scheduler - " + result.Replace("\r\n", ""));
                                if (result.Replace("\r\n", "") == "Running")
                                {
                                    ProdStreamWriterEAStrans("Службы транспорта работают");
                                    ProdStreamWriterEAStrans("Файлы старой реплики удалены");
                                    DeleteFileReplicaAll();
                                    СheckTransport = 0;
                                }
                                else
                                {
                                    ProdStreamWriterEAStrans("Служба GM_SchedulerSvc - не работает");
                                    if (server != "")
                                    {
                                        textBox11.Clear();
                                        string name_service = "GM_SchedulerSvc";
                                        string action_service = "Running";
                                        await Task.Run(() => Prod_Async_Power_Shell_Service(server, name_service, action_service));
                                    }
                                    ProdStreamWriterEAStrans("Запуск GM_SchedulerSvc осуществлен");
                                    ProdStreamWriterEAStrans("Файлы старой реплики удалены");
                                    DeleteFileReplicaAll();
                                    СheckTransport = 0;
                                }
                            }
                            else
                                ProdStreamWriterEAStrans("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
                        }
                    }
                    else
                    {
                        ProdStreamWriterEAStrans("Транспорт в рабочем состоянии");
                        СheckTransport = 1;
                    }
                      
                }
            }
            else
                ProdStreamWriterEAStrans("Поле для ввода номера ОПС - пустое");
        }



        //Кнопка Обновить
        private void button18_Click(object sender, EventArgs e)
        {
            Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
            label20.Text = "IP Адрес удленного ПК: " + server;
        }

        // Остановка служб "GMMQ" и "GM_SchedulerSvc" 
        // Остановка службы "GMMQ"
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
                    textBox10.Clear();
                    //string result = "";
                    string name_service = "GMMQ";
                    string ip = server;
                    progressBar2.Value = 40;
                    string action_service = "Stopped";
                    await Task.Run(() => Async_Power_Shell_Service(server, name_service, action_service, progressBar2, textBox10));
                    //await Task.Run(() => Powershell_service_Force(ip, action_service, name_service, ""));
                    //    await Task.Run(() => Power_Shell("Get-Service -Computer " + ip + " -Name " + name_service + " | Stop-Service -Force"));
                    //    //Служба GMMQ останавливается с помощью другой команды т к имеет зависимые службы,
                    //    //поэтому для нее особы метод создан

                    //    await Task.Run(() => Power_Shell_1("get-service -" +
                    //                       "DisplayName \"GMMQ\"" +
                    //                       " -ComputerName " + server + "" +
                    //                       " | format-table Status -autosize", out result));

                    //    //запрос на состояние службы и потом проверка,
                    //    //работа с службой GMMQ специально разделена на 2 метода
                    //    //первый метод Powershell_service_Force - не возвращает после отработки состояние службы, только ее останавливает
                    //    //второй метод Power_Shell_1 - получает просто состояние службы в переменную, записанную в выходные параметры метода
                    //    //далее логика работы программы, в зависимости от полученного значения состояния, из метода Power_Shell_1

                    //    textBox10.Text = result.Replace("\r\n", "");
                    //    if (result == $"\r\n{action_service}\r\n\r\n\r\n")
                    //    {
                    //        progressBar2.Value = 100;
                    //        MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service)}");
                    //        progressBar2.Value = 0;
                    //        textBox10.Text = result.Replace("\r\n", "");
                    //    }
                    //    if (result == "Блок еlse")
                    //    {
                    //        MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - {Action_service_info(action_service)}");
                    //    }
                    //    if (result == "Нет данных")
                    //    {
                    //        progressBar2.Value = 0;
                    //    }
                    //}
                    //else
                    //    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        // Остановка службы "GM_SchedulerSvc"
        async private void button28_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            try
            {
                Name_DataBase_and_Server(textBox1.Text,  out string server, out string name_database);
                if (server != "")
                {
                    textBox11.Clear();
                    string ip = server;
                    string name_service = "GM_SchedulerSvc";
                    progressBar3.Value = 40;
                    string action_service = "Stopped";
                    await Task.Run(() => Async_Power_Shell_Service(server, name_service, action_service, progressBar3, textBox11));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }


        // Запуск "GMMQ" и "GM_SchedulerSvc"
        // Запуск "GMMQ"
        async private void button20_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            try
            {
                if (textBox1.Text != "")
                {
                    Name_DataBase_and_Server(textBox1.Text,  out string server, out string name_database);
                    if (server != "")
                    {
                        textBox10.Clear();
                        string name_service = "GMMQ";
                        string action_service = "Running";
                        await Task.Run(() => Async_Power_Shell_Service(server, name_service, action_service, progressBar2, textBox10));
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

        // Запуск "GM_SchedulerSvc"
        async private void button27_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            try
            {
                if (textBox1.Text != "")
                {
                    
                    Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
                   
                    if (server != "")
                    {
                        textBox11.Clear();                  
                        string name_service = "GM_SchedulerSvc";
                        string action_service = "Running";
                       await Task.Run(() => Async_Power_Shell_Service(server, name_service, action_service, progressBar3, textBox11));
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

        // Состояние служб "GMMQ" и "GM_SchedulerSvc"
        // Состояние службы "GMMQ"
        async private void button22_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            if (textBox1.Text != "")
            {
                Name_DataBase_and_Server(textBox1.Text,  out string server, out string name_database);
                if (server != "")
                {
                    textBox10.Clear();
                    string ip = server;
                    string result = "";
                    await Task.Run(() => Power_Shell_1("get-service -" +
                                       "DisplayName \"GMMQ\"" +
                                       " -ComputerName " + ip + "" +
                                       " | format-table Status -autosize", out result));
                    textBox10.Text = result.Replace("\r\n", "");
                    if (result == "Нет данных")
                    {
                        textBox10.Text = result;
                    }
                }
            }
            else
                MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
            
        }

        // Состояние службы "GM_SchedulerSvc"
        async private void button25_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            if (textBox1.Text != "")
            {
                Name_DataBase_and_Server(textBox1.Text,  out string server, out string name_database);
                if (server != "")
                {
                    textBox11.Clear();
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
            }
            else
                MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
        }

        // Перезапуск "GMMQ" и "GM_SchedulerSvc"
        // Перезапуск "GMMQ"
        async private void button21_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            try
            {
                if (textBox1.Text != "")
                {
                    Name_DataBase_and_Server(textBox1.Text,  out string server, out string name_database);
                    if (server != "")
                    {
                        textBox10.Clear();
                        string ip = server;
                        string name_service = "GMMQ";
                        string action_service = "Restart";
                        await Task.Run(() => Async_Power_Shell_Service(ip, name_service, action_service, progressBar2, textBox10));
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

        // Перезапуск "GM_SchedulerSvc"
        async private void button26_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            try
            {
                if (textBox1.Text != "")
                {
                    Name_DataBase_and_Server(textBox1.Text,  out string server, out string name_database);
                    if (server != "")
                    {
                        textBox11.Clear();
                        string action_service = "Restart";
                        string ip = server;
                        string name_service = "GM_SchedulerSvc";
                        await Task.Run(() => Async_Power_Shell_Service(ip, name_service, action_service, progressBar3, textBox11));
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

        //Метод, запускающий командную строку с определенный командой,
        //записанной в параметр метода и выводящиЙ в выходной параметр результат выполнения команды
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

        //Метод представляющий процесс, запускающий power_shell с фиксированной командой, заточенной под остановку службы на удаленном пк в вашей сети,
        //Метод ожидает своей отработки .WaitForExit(); 
        public void Powershell_service_Force(string ip, string name_service, string action_more)
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

        //Метод для контроля результата выполения метода Powershell_service,
        //исходя из результата Powershell_service, метод движется по разным сценариям
      async public void Async_Power_Shell_Service(string ip, string name_service, string action_service, ProgressBar progressBar, TextBox textBox)
        {
            if (textBox1.Text != "")
            {
                try
                {
                    if (action_service == "Running")
                    {
                        progressBar.Value = 40;
                        string action_power_shell = $"set-service {name_service} -ComputerName {ip} -Status {action_service} -PassThru | format-table Status -autosize";
                        Power_Shell(action_power_shell);
                        Thread.Sleep(500);
                        progressBar.Value = 70;
                        Powershell_service_checked(ip, name_service, out string check_action);
                        progressBar.Value = 90;
                        Check_Service(check_action, ip, name_service, action_service, progressBar, textBox, out string result);
                    }
                    if (action_service == "Stopped")
                    {
                        progressBar.Value = 40;
                        string action_more = "";
                        Powershell_service_Force(ip, name_service, action_more);
                        Thread.Sleep(500);
                        progressBar.Value = 70;
                        Powershell_service_checked(ip, name_service, out string check_action);
                        progressBar.Value = 90;
                        Check_Service(check_action, ip, name_service, action_service, progressBar, textBox, out string result);
                    }
                    if (action_service == "Restart")
                    {
                        action_service = "R-Stopped";
                        textBox.Text = action_service.Substring(2);
                        progressBar.Value = 15;
                        string action_more = "";
                        Powershell_service_Force(ip, name_service, action_more);
                        progressBar.Value = 30;
                        string check_action = "";
                        string check_action_1 = "";
                        await Task.Run(() => Powershell_service_checked(ip, name_service, out check_action));
                        progressBar.Value = 45;
                        Check_Service(check_action, ip, name_service, action_service, progressBar, textBox, out string result_1);
                        if (result_1 == "\r\nStopped\r\n\r\n\r\n")
                        {
                            action_service = "R-Running";
                            progressBar.Value = 60;
                            string action_power_shell = $"set-service {name_service} -ComputerName {ip} -Status {action_service.Substring(2)} -PassThru | format-table Status -autosize";
                            Power_Shell(action_power_shell);
                            progressBar.Value = 75;
                            await Task.Run(() => Powershell_service_checked(ip, name_service, out check_action_1));
                            progressBar.Value = 90;
                            Check_Service(check_action_1, ip, name_service, action_service, progressBar, textBox, out string result);
                            if (result == "\r\nRunning\r\n\r\n\r\n")
                            {
                                progressBar.Value = 100;
                                MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Перезапущена");
                                progressBar.Value = 0;
                                textBox.Text = result.Replace("\r\n", "");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка из метода Async_Power_Shell_Service \nПк выключен или нет интернета\nИли запрашиваемая служба отсутствует на данном ПК\n\n\nОписание:\n\n{ex}");
                }
            }
            else
            MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
        }
        async private void Prod_Async_Power_Shell_Service(string ip, string name_service, string action_service)
        {
            string service_prod = "[Service] - ";
            if (textBox1.Text != "")
            {
                try
                {
                    if (action_service == "Running")
                    {
                        string action_power_shell = $"set-service {name_service} -ComputerName {ip} -Status {action_service} -PassThru | format-table Status -autosize";
                        Power_Shell(action_power_shell);
                        Thread.Sleep(500);
                        Powershell_service_checked(ip, name_service, out string check_action);
                        Prod_Check_Service(check_action, ip, name_service, action_service, out string result);
                    }
                    if (action_service == "Stopped")
                    {
                        string action_more = "";
                        Powershell_service_Force(ip, name_service, action_more);
                        Thread.Sleep(500);
                        Powershell_service_checked(ip, name_service, out string check_action);
                        Prod_Check_Service(check_action, ip, name_service, action_service, out string result);
                    }
                    if (action_service == "Restart")
                    {
                        action_service = "R-Stopped";
                        string action_more = "";
                        Powershell_service_Force(ip, name_service, action_more);
                        string check_action = "";
                        string check_action_1 = "";
                        await Task.Run(() => Powershell_service_checked(ip, name_service, out check_action));
                        Prod_Check_Service(check_action, ip, name_service, action_service, out string result_1);
                        if (result_1 == "\r\nStopped\r\n\r\n\r\n")
                        {
                            action_service = "R-Running";
                            string action_power_shell = $"set-service {name_service} -ComputerName {ip} -Status {action_service.Substring(2)} -PassThru | format-table Status -autosize";
                            Power_Shell(action_power_shell);
                            await Task.Run(() => Powershell_service_checked(ip, name_service, out check_action_1));
                            Prod_Check_Service(check_action_1, ip, name_service, action_service, out string result);
                            if (result == "\r\nRunning\r\n\r\n\r\n")
                            {
                                ProdStreamWriterEAStrans(service_prod + $"Служба \"{name_service}\"\nНа компьютере {ip} - Перезапущена");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ProdStreamWriterEAStrans(service_prod + $"Ошибка из метода Async_Power_Shell_Service \nПк выключен или нет интернета\nИли запрашиваемая служба отсутствует на данном ПК\n\n\nОписание:\n\n{ex}");
                }
            }
            else
                ProdStreamWriterEAStrans($"\nПоле для ввода номера ОПС - пустое");
        }

        public void Check_Service(string check_action, string ip, string name_service, string action_service, ProgressBar progressBar, TextBox textBox, out string result)
        {
            if (action_service != "R-Stopped" | action_service != "R-Running")
            {
                result = check_action;
                if (check_action == $"\r\n{action_service}\r\n\r\n\r\n")
                {
                    result = check_action;
                    progressBar.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service)}");
                    progressBar.Value = 0;
                    textBox.Text = check_action.Replace("\r\n", "");
                }
                 if (check_action == "Блок еlse")
                {
                    result = check_action;
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - {Action_service_info(action_service)}");
                    progressBar.Value = 0;
                }
                 if (check_action == "Нет данных")
                {
                    result = check_action;
                    textBox.Text = check_action;
                    progressBar.Value = 0;
                }
                if (check_action == $"\r\n{Inversion_Action_service(action_service)}\r\n\r\n\r\n")
                {
                    result = check_action;
                    progressBar.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service + "-Error")}");
                    textBox.Text = Inversion_Action_service(action_service);
                    progressBar.Value = 0;
                }
            }
            else
            {
                result = "";
                if (check_action == $"\r\nStopped\r\n\r\n\r\n")
                {
                    result = check_action;
                    textBox.Text = check_action.Replace("\r\n", "");
                }
                 if (check_action == "Блок еlse")
                {
                    result = check_action;
                    textBox.Text = $"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - {Action_service_info(action_service)}";
                }
                 if (check_action == "Нет данных")
                {
                    result = check_action;
                    textBox.Text = check_action;
                }
                 if (check_action == $"\r\n{Inversion_Action_service(action_service)}\r\n\r\n\r\n")
                {
                    result = check_action;
                    progressBar.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service + "-Error")}");
                    textBox.Text = Inversion_Action_service(action_service);
                    progressBar.Value = 0;
                }
            }
        }
        public void Prod_Check_Service(string check_action, string ip, string name_service, string action_service, out string result)
        {
            string service_prod = "[Service] - ";
            if (action_service != "R-Stopped" | action_service != "R-Running")
            {
                result = check_action;
                if (check_action == $"\r\n{action_service}\r\n\r\n\r\n")
                {
                    result = check_action;
                    ProdStreamWriterEAStrans(service_prod + $"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service)}");
                    ProdStreamWriterEAStrans(service_prod + check_action.Replace("\r\n", ""));
                }
                if (check_action == "Блок еlse")
                {
                    result = check_action;
                    ProdStreamWriterEAStrans(service_prod + $"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - {Action_service_info(action_service)}");
                }
                if (check_action == "Нет данных")
                {
                    result = check_action;
                    ProdStreamWriterEAStrans(service_prod + check_action);
                }
                if (check_action == $"\r\n{Inversion_Action_service(action_service)}\r\n\r\n\r\n")
                {
                    result = check_action;
                    ProdStreamWriterEAStrans(service_prod + $"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service + "-Error")}");
                    ProdStreamWriterEAStrans(service_prod + Inversion_Action_service(action_service));
                }
            }
            else
            {
                result = "";
                if (check_action == $"\r\nStopped\r\n\r\n\r\n")
                {
                    result = check_action;
                    ProdStreamWriterEAStrans(service_prod + check_action.Replace("\r\n", ""));
                }
                if (check_action == "Блок еlse")
                {
                    result = check_action;
                    ProdStreamWriterEAStrans(service_prod + $"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - {Action_service_info(action_service)}");
                }
                if (check_action == "Нет данных")
                {
                    result = check_action;
                    ProdStreamWriterEAStrans(service_prod + check_action);
                }
                if (check_action == $"\r\n{Inversion_Action_service(action_service)}\r\n\r\n\r\n")
                {
                    result = check_action;
                    ProdStreamWriterEAStrans(service_prod + $"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service + "-Error")}");
                    ProdStreamWriterEAStrans(service_prod + Inversion_Action_service(action_service));
                }
            }
        }
        public string Inversion_Action_service(string action_service)
        {
            if (action_service == "Stopped")
                return "Running";
            if (action_service == "Running")
                return "Stopped";
            else
                return $"\nОшибка: action_service = {action_service}\nМетод Async_Power_Shell_Service далее Action_service_info";
        }


        //Метод для вывода состояния службы в MessageBox, исходя из значения переменной action_service
        public string Action_service_info(string action_service)
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

        //Метод представляющий процесс, запускающий power_shell с командами,
        //редактируемыми по параметрам метода и служащими для управления службами на удаленных пк, в сетке, где находится сама программа
        //Удаленные пк задаются в виде их ip (айпи адреса), в самом Power_shell, в его команде, после объявления командлета "-ComputerName" 
        public void Powershell_service_checked(string ip, string name_service, out string check_action)
        {
            //check_action = "";
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
                if (textBox1.Text.Length == 6 & k != "\r\nStopped\r\n\r\n\r\n" & k != "\r\nRunning\r\n\r\n\r\n")
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
                MessageBox.Show($"Ошибка из метода Powershell_service \nПк выключен или нет интернета\nИли запрашиваемая служба отсутствует на данном ПК\n\n\nОписание:\n\n{ex}");
                check_action = "Нет данных";
            }
        }

        //События кнопок "Посмотреть"
        private void button3_Click(object sender, EventArgs e)
        {
            View_Stage("\\\\D01eascl02fskal\\gmmq\\EAS\\KAL\\" + textBox1.Text + "\\In");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            View_Stage("\\\\D01eascl02fskal\\gmmq\\EAS\\KAL\\" + textBox1.Text + "\\Out");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            View_Ops(@"\c$\GMMQ\Export");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            View_Ops(@"\c$\GMMQ\Import");
        }

        //Метод кнопок посмотреть на отделении
        public void View_Ops(string path)
        {
            if (textBox1.Text != "")
            {
                    Name_DataBase_and_Server(textBox1.Text,  out string server, out string name_database);
                    if (server != "")
                    {
                        string par = @"\\" + server + path;
                        Async_View_Files(listBox1, label10, label14, label21, par, textBox12, server);
                    }
            }
            else
                MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
        }

        //Метод кнопок посмотреть на даксе
        public void View_Stage(string path)
        {
            if (textBox1.Text != "")
            {
                    string server = "";
                    Async_View_Files(listBox1, label10, label14, label21, path, textBox12, server);
            }
            else
                MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
        }

        //Событие timer1 - пока не используется
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label23.Text = "Время выполнения запроса: " + (++timerCounter).ToString() + " сек.";


            //if (progressBar1.Value < 98)
            //{
            //    this.progressBar1.Increment(1);
            //}
            //else
            //    this.timer1.Stop();


            //else
            //    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
        }

        async private void label22_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            Name_DataBase_and_Server(textBox1.Text,  out string server, out string name_database);
            string path = @"\\" + server + @"\c$\GMMQ\Export";
            string[] files = Directory.GetFiles(path);
            string[] mass_check = new string[1];
            if (files.Length > 0)
            {
                DateTime dateTime = new DateTime();
                dateTime = Directory.GetLastAccessTime(path);
                if (dateTime == DateTime.Now)
                {
                    mass_check = new string[files.Length];
                    for (int i = 0; i < files.Length; i++)
                    {
                        label22.Text = files[i];
                        await Task.Delay(200);
                    }
                }
            }
            else
                await Task.Delay(500);
        }


        //Кнопка "Проводник"
        private void button24_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (textBox12.Text != "")
                {
                    Process.Start($"{textBox12.Text}");
                }
                else
                    MessageBox.Show("Поле \"Путь\" - пустое.\nНажмите на кнопку \"Посмотреть\"\nВ разделе \"Файлы на ОПС\" или \"Файлы в DAX\"\nТогда поле \"Путь\" заполниться и \"проводник\" по данному откроется  ");
            }
            else
                MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
        }


    
        private void button23_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Name_DataBase_and_Server(textBox1.Text,  out string server1, out string name_database1);
                string file = "gmmq.packege.end";
                string file_2 = "meta.xml";
                string path = @"\\" + server1 + @"\c$\GMMQ\Import\" + file;
                string path_2 = @"\\" + server1 + @"\c$\GMMQ\Import\" + file_2;
                FileInfo fileInf = new FileInfo(path);
                FileInfo fileInf_2 = new FileInfo(path_2);
                //while (fileInf.Exists | fileInf_2.Exists)
                //{
                //    progressBar1.Value = 0;
                //    textBox2.Clear();
                //    textBox9.Text = textBox1.Text + " - выполнен скрипт exec ReplicaImport 0 ";
                //    timerCounter = 0;
                //    label22.Text = "Количество файлов реплики: ";
                //}
                //if (fileInf.Exists | fileInf_2.Exists)
                //{
                //    progressBar1.Value = 0;
                //    textBox2.Clear();
                //    textBox9.Text = textBox1.Text + " - выполнен скрипт exec ReplicaImport 0 ";
                //    timerCounter = 0;
                //    label22.Text = "Количество файлов реплики: ";
                //}
                //else
                //{
                if (fileInf.Exists | fileInf_2.Exists)
                {
                    try
                    {
                        DialogResult result = MessageBox.Show(
            $"Выполнить скрипт \"exec ReplicaImport 0\",\nна ОПС {textBox1.Text} ? ",
            "Сообщение",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Information);

                        if (result == DialogResult.Yes)
                        {
                            this.timer.Start();
                            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                                     // отменяет отслеживание ошибок,
                                                                     // но дает передать компоненты формы в другой поток 

                            Thread thread = new Thread(
                                () =>
                                {
                                    //Action action = () =>
                                    //{
                                    //int action = 0;
                                    SqlCommand command = new SqlCommand();
                                    //int action = 0;
                                    Start_Transport_Import(progressBar1, command);
                                    //View_files_Import(progressBar1, command, action);
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
                            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                                     // отменяет отслеживание ошибок,
                                                                     // но дает передать компоненты формы в другой поток 
                                                                     //Name_DataBase_and_Server(textBox1.Text, out string server1, out string name_database1);
                                                                     //string path1 = @"\\" + server1 + @"\c$\GMMQ\Export";
                                                                     //string[] files = Directory.GetFiles(path1);
                                                                     //while (files.Length != 0)
                                                                     //{
                                                                     //    DateTime dateTime = new DateTime();
                                                                     //    dateTime = Directory.GetLastWriteTime(path1); //перезаписывает переменную, для обновления даты изменения папки
                                                                     //    if (dateTime.AddSeconds(3) > DateTime.Now)
                                                                     //    {
                                                                     //        string[] files_2 = Directory.GetFiles(path1);//перезаписывает переменную, для обновления количества файлов в папке                   
                                                                     //        for (int i = 0; i < files_2.Length; i++)
                                                                     //        {
                                                                     //            if (files_2[i] == "meta.xml" | files_2[i] == "gmmq.packege.end")
                                                                     //            {
                                                                     //                textBox9.Text = textBox1.Text + " - толкнул скриптом +";
                                                                     //                thread.Abort();
                                                                     //            }
                                                                     //        }
                                                                     //    }
                                                                     //}
                        }
                        if (result == DialogResult.No)
                        {
                            this.timer.Stop();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: \n{ex}");
                        progressBar1.Value = 0;
                        this.timer.Stop();
                        textBox2.Text = textBox1.Text + " - ошибка";
                    }
                    textBox1.Focus();
                    //}
                }
                else
                {
                    MessageBox.Show($"Нет файлов: {file} \n{file_2} \nВ папке: {path} \n{path_2}"); 
                }
            }
            else
                MessageBox.Show("Поле для ввода номера ОПС - пустое");
        }
        async public void Start_Transport_Import(ProgressBar progressBar, SqlCommand command)
        {
            int action = 0;
            try
            {
                if (textBox1.Text != "")
                {

                    textBox2.Clear();
                    string server = "";
                    string name_database = "";
                    Name_DataBase_and_Server(textBox1.Text,  out server, out name_database);
                    string connectionString = $"Server={server};Database={name_database};Persist Security Info=False;User ID=sa;Password=QweAsd123;";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();
                        textBox2.Text = "Подключение открыто";
                    }
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();
                        command.CommandText = "exec ReplicaImport 0";
                        command.Connection = connection;
                        command.CommandTimeout = 5000; //увеличено время на выполнение команды
                        await Task.Run(() => command.ExecuteNonQueryAsync());
                        action = 1;
                        Action(action);
                        this.timer.Stop();
                        textBox2.Text = "Скрипт выполнен";
                        progressBar.Value = 100;
                        MessageBox.Show($"Запрос в SQL: \"{command.CommandText}\" - успешно отработан");
                        progressBar.Value = 0;
                        textBox2.Clear();
                        textBox2.Text = textBox1.Text + " - выполнен скрипт exec ReplicaImport 0 ";
                        timerCounter = 0;
                        label22.Text = "Количество файлов реплики: ";
                    }
                }
                else
                    MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
                action = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
                progressBar.Value = 0;
                this.timer.Stop();
                textBox2.Text = textBox1.Text + " - ошибка";
            }
        }

        //Метод, следящий за появлением новых файлов в папке отделения, по фиксированному пути 
        //@"\\" + server + @"\c$\GMMQ\Export"
        async public void View_files_Import(ProgressBar progressBar, SqlCommand command, int action)
        {
            try
            {
                CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                         // отменяет отслеживание ошибок,
                                                         // но дает передать компоненты формы в другой поток 
                Name_DataBase_and_Server(textBox1.Text,  out string server, out string name_database);
                string path = @"\\" + server + @"\c$\GMMQ\Import";
                string[] files = Directory.GetFiles(path);
                string[] files1 = Directory.GetFiles(path);
                string[] catalog = Directory.GetDirectories(path);
                int count = 1;
                int u = 0;
                textBox12.Text = path;
                string[] mass_2 = new string[count];
                if (action != 1)
                {
                    
                    while (files.Length != 0)
                    {
                        if (action != 1)
                        {
                            count++;
                            string[] files_1 = Directory.GetFiles(path);
                            int file3 = files.Length;
                            DateTime dateTime = new DateTime();
                            dateTime = Directory.GetLastWriteTime(path); //перезаписывает переменную, для обновления даты изменения папки
                            FileInfo file = new FileInfo(path);
                            if (dateTime.AddSeconds(3) > DateTime.Now)
                            {
                                catalog = Directory.GetDirectories(path);
                                if (catalog.Length == 0)
                                {
                                    //string[] files_2 = Directory.GetFiles(path);//перезаписывает переменную, для обновления количества файлов в папке
                                    string[] mass = new string[files_1.Length];
                                    if (u == 0)
                                    {
                                        u++;
                                        mass = files_1;
                                        listBox1.Items.Clear();
                                        //listBox1.BeginUpdate();
                                        for (int i = 0; i < files_1.Length; i++)
                                        {
                                            //if (files_1[i] != "meta.xml" | files_1[i] != "gmmq.packege.end")
                                            //{
                                                label22.Text = "Количество файлов реплики: " + files_1.Length + "\nПо пути: " + files_1[i];
                                                listBox1.Items.Add(files_1[i].Substring(18 + server.Length));
                                                if (progressBar.Value >= 0 & progressBar.Value < 99)
                                                {
                                                    double p = files_1.Length/200;
                                                    double b = (1 - p)*100 ;
                                                    if (b < 99)
                                                    {
                                                        progressBar.Value = Convert.ToInt32(b);
                                                    }
                                            }
                                        //}
                                            //else
                                            //{
                                            //    textBox9.Text = textBox1.Text + " - выполнен скрипт exec ReplicaImport 0 ";
                                            //    action = 1;
                                            //    break;
                                            //}
                                        }
                                        //listBox1.EndUpdate();
                                    }
                                    if (u == 1)
                                    {
                                        listBox1.BeginUpdate();
                                        listBox1.Items.Clear();
                                        for (int i = 0; i < files_1.Length; i++)
                                        {
                                            //if (files_1[i] != "meta.xml" | files_1[i] != "gmmq.packege.end")
                                            //{
                                                listBox1.Items.Add(files_1[i].Substring(18 + server.Length));
                                            //}
                                            //else
                                            //{
                                            //    textBox9.Text = textBox1.Text + " - выполнен скрипт exec ReplicaImport 0 ";
                                            //    action = 1;
                                            //    break;
                                            //}
                                        }
                                        listBox1.EndUpdate();
                                        for (int i = 0; i < files_1.Length; i++)
                                        {
                                            if (progressBar.Value >= 0 & progressBar.Value < 99)
                                            {
                                                double p = files_1.Length;
                                                double b = p / 1.8;
                                                if (b < 99)
                                                {
                                                    progressBar.Value = Convert.ToInt32(b);
                                                }
                                            }
                                            //if (mass[i] != files_1[i])
                                            //{
                                                mass = Addition_Mass(mass, files_1);
                                                label22.Text = "Количество файлов реплики: " + files_1.Length + "\nПо пути: " + files_1[i];
                                            //}
                                        }
                                    }
                                    Update_Files_and_Catolog_lenght(path);
                                }
                                else
                                    await Task.Delay(40000000);
                            }
                            await Task.Delay(400);
                        }
                        else
                        {
                            progressBar.Value = 0;
                            this.timer.Stop();
                            textBox2.Text = textBox1.Text + " - выполнен скрипт exec ReplicaImport 0 ";
                            break;
                        }
                    }
                    if (files.Length == 0)
                    {
                        //catalog = Directory.GetDirectories(path);
                        //if (catalog.Length == 0)
                        //{
                            //files = Directory.GetFiles(path);
                            //for (int i = 0; i < files.Length; i++)
                            //{
                            //    if (files[i] != "meta.xml" | files[i] != "gmmq.packege.end")

                            //    {
                                progressBar.Value = 0;
                                this.timer.Stop();
                                textBox2.Text = textBox1.Text + " - выполнен скрипт exec ReplicaImport 0 ";
                                //files = Directory.GetFiles(path); //перезаписывает переменную, для обновления количества файлов в папке
                                    label22.Text = "Количество файлов реплики: 0";
                            //    }
                            //    else
                            //    {
                            //        await Task.Delay(200000000);
                            //        action = 1;
                            //        break;
                            //    }
                            //}
                        //    await Task.Delay(200);
                        //}
                        //else
                        //{
                        //    action = 1;
                        //    await Task.Delay(200000000);
                        //}
                    }
                }
                else
                {
                    progressBar.Value = 0;
                    this.timer.Stop();
                    textBox2.Text = textBox1.Text + " - выполнен скрипт exec ReplicaImport 0 ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex}");
                progressBar.Value = 0;
                this.timer.Stop();
                textBox2.Text = textBox1.Text + " - ошибка";
            }
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
            }
        }
        public void AddStramwriter()
        {
            if (textBox1.Text != "" & streamwriteEAStrans == 1)
            {
                StreamWriterEAStrans(textBox1.Text + " - транспорт работает +");
            }
            else
            {
                MessageBox.Show($"Поле для ввода номера ОПС пустое\nНе включена функия - \"Запись в файл\"");
            }
        }
        public void AddStreamwriterDistrict(string district)
        {
            if (textBox1.Text != "" & streamwriteEAStrans == 1)
            {
                StreamWriterEAStrans($"Транспорт по {district} почтамту:"); 
            }
            else
            {
                MessageBox.Show($"Поле для ввода номера ОПС пустое\nНе включена функия - \"Запись в файл\"");
            }
        }
        public void AddStreamwriterDistrict()
        {
            if (textBox1.Text != "" & streamwriteEAStrans == 1)
            {
                StreamWriterEAStrans($"Больше остановок транспорта не зафиксировано");
            }
            else
            {
                MessageBox.Show($"Поле для ввода номера ОПС пустое\nНе включена функия - \"Запись в файл\"");
            }
        }
        private void button29_Click(object sender, EventArgs e)
        {
             AddStramwriter();
        }
        private void button30_Click(object sender, EventArgs e)
        {
            string number_ops = textBox1.Text;  
            var form = new Smart_explorer.Form1(number_ops);
            form.ShowDialog();
        }




        //Тестовый контур 
        async private void button23_Click(object sender, EventArgs e)
        {
            //string[] mass_1 = new string[5] { @"10.94\QWER1", @"10.94\QWER2", @"10.94\QWER3", @"10.94\QWER4", @"10.94\QWER5" };
            //string[] mass_2 = new string[7] { @"10.94\QWER1", @"10.94\QWER2", @"10.94\QWER3",@"10.94\QWER4",@"10.94\QWER5", @"10.94\QWER6" , @"10.94\QWER7" };

            //var result = mass_2.Except(mass_1);

            //foreach (var item in result)
            //{             
            //    label23.Text = item;
            //    await Task.Delay(700);
            //}

            this.timer.Start();

            await Task.Run(() => View_files_Test(progressBar1));

            //string path = @"D:\Новая папка (2)";
            //Delete_File(path);
        }

        //Тестовая версия метода
        async public void View_files_Test(ProgressBar progressBar)
        {
            try
            {
                CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                         // отменяет отслеживание ошибок,
                                                         // но дает передать компоненты формы в другой поток 
                                                         //Name_DataBase_and_Server(textBox1.Text, out string server, out string name_database);
                                                         //string path = @"\\" + server + @"\c$\GMMQ\Export";
                string path = @"D:\!localhost\Export";
                string[] files = Directory.GetFiles(path);
                string[] files1 = Directory.GetFiles(path);
                int count = 1;
                string[] mass_2 = new string[count];
                while (files.Length == 0)
                {
                    files = Directory.GetFiles(path); //перезаписывает переменную, для обновления количества файлов в папке
                    label22.Text = "Количество файлов реплики: 0";
                    await Task.Delay(200);
                }
                while (files.Length != 0)
                {
                    count++;
                    //mass_2 = new string[count];
                    string[] files_1 = Directory.GetFiles(path);
                    DateTime dateTime = new DateTime();
                    dateTime = Directory.GetLastWriteTime(path);
                    //int file3 = files.Length;
                    //DateTime dateTime = new DateTime();
                    //dateTime = Directory.Get; //перезаписывает переменную, для обновления даты изменения папки
                    //FileInfo file = new FileInfo(path);
                    if (dateTime.AddSeconds(2) > DateTime.Now)
                    {
                        //    string[] files_2 = Directory.GetFiles(path);//перезаписывает переменную, для обновления количества файлов в папке
                        //                                                //string[] mass_2 = files_2;
                        //                                                //Checked(files_2, count, mass_2, out string[] mass_3);
                        listBox1.BeginUpdate();
                        listBox1.Items.Clear();

                        //if (files_2.Length > file3)
                        //{
                        //int raznich = files_2.Length - files.Length;

                        //string[] mass_1 = new string[5] { @"10.94\QWER1", @"10.94\QWER2", @"10.94\QWER3", @"10.94\QWER4", @"10.94\QWER5" };
                        //string[] mass_2 = new string[7] { @"10.94\QWER1", @"10.94\QWER2", @"10.94\QWER3", @"10.94\QWER4", @"10.94\QWER5", @"10.94\QWER6", @"10.94\QWER7" };

                        //var result = files_2.Except(files_1);

                        //foreach (var item in result)
                        //{
                        //label23.Text = item;
                        //await Task.Delay(700);


                        //foreach (var item in result)
                        //{
                        //    label22.Text = "Количество файлов реплики: " + files_2.Length + "\nПо пути: " + item;
                        //    listBox1.Items.Add(item);
                        //    //.Substring(18 + server.Length)
                        //    if (progressBar.Value >= 0 & progressBar.Value < 99)
                        //    {
                        //        double p = files.Length;
                        //        double b = p / 1.8;
                        //        if (b < 99)
                        //        {
                        //            progressBar.Value = Convert.ToInt32(b);
                        //        }
                        //    }
                        //}



                        for (int i = 0; i < files_1.Length; i++)
                        {
                            label22.Text = "Количество файлов реплики: " + dateTime + "\nПо пути: " + files_1[i];
                            listBox1.Items.Add(files_1[i].Substring(21));
                            if (progressBar.Value >= 0 & progressBar.Value < 99)
                            {
                                double p = files_1.Length;
                                double b = p / 1.8;
                                if (b < 99)
                                {
                                    progressBar.Value = Convert.ToInt32(b);
                                }
                            }
                        }
                        listBox1.EndUpdate();

                        //for (int i = 0; i < files_2.Length; i++)
                        //{
                        //    if(count == 1)
                        //    {
                        //        mass_2[count - 1] = files_2[files_2.Length - 1];
                        //    }
                        //    if (mass_2[count - 1] != files_2[files_2.Length - 1])
                        //    {
                        //        label22.Text = "Количество файлов реплики: " + files_2.Length + "\nПо пути: " + files_2[files_2.Length - 1];
                        //        mass_2[count - 1] = files_2[files_2.Length - 1];
                        //        listBox1.Items.Add(files_2[i]);
                        //        //.Substring(18 + server.Length)
                        //        if (progressBar.Value >= 0 & progressBar.Value < 99)
                        //        {
                        //            double p = files.Length;
                        //            double b = p / 1.8;
                        //            if (b < 99)
                        //            {
                        //                progressBar.Value = Convert.ToInt32(b);
                        //            }
                        //        }
                        //    }
                        //}



                        //}
                        //}
                        //}
                        label11.Text = files_1.Length.ToString();
                        string[] catalog = Directory.GetDirectories(path);
                        label15.Text = catalog.Length.ToString();
                        label21.Text = $"Всего элементов: {files_1.Length + catalog.Length}";

                    }
                    await Task.Delay(1200);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"\nОшибка: \n{ex}");
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
            AddStreamwriterDistrict(button32.Text);
        }

        private void button33_Click(object sender, EventArgs e)
        {
            AddStreamwriterDistrict(button33.Text);
        }

        private void button34_Click(object sender, EventArgs e)
        {
            AddStreamwriterDistrict(button34.Text);
        }

        private void button35_Click(object sender, EventArgs e)
        {
            AddStreamwriterDistrict(button35.Text);
        }

        private void button36_Click(object sender, EventArgs e)
        {
            AddStreamwriterDistrict(button36.Text);
        }

        private void button37_Click(object sender, EventArgs e)
        {
            AddStreamwriterDistrict(button37.Text);
        }

        private void button38_Click(object sender, EventArgs e)
        {
            AddStreamwriterDistrict(button38.Text);
        }

        private void button39_Click(object sender, EventArgs e)
        {
            AddStreamwriterDistrict(button39.Text);
        }

        private void button40_Click(object sender, EventArgs e)
        {
            AddStreamwriterDistrict(button40.Text);
        }

        private void button41_Click(object sender, EventArgs e)
        {
            AddStreamwriterDistrict(button41.Text);
        }

        private void button42_Click(object sender, EventArgs e)
        {
            AddStreamwriterDistrict();
        }

        private void button31_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (Width != 844)
                {
                    Width = 844;
                }
                else
                {
                    Width = 1215;
                }
            }
            else
                MessageBox.Show($"\nПоле для ввода номера ОПС - пустое");
        }
    }     
}
