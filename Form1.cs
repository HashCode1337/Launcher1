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
using System.Net;
using System.Net.Sockets;

namespace Launcher
{
    public partial class Form1 : Form
    {
        const string clientid = "1";
        const int port = 11517;
        const string ipaddr = "127.0.0.1";
        const string okchar = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.,-_:";
        
        public string protocol { get; set; }
        public string link { get; set; }
        public string ftpip { get; set; }
        public string ftpport { get; set; }
        public string ftpuser { get; set; }
        public string ftppwd { get; set; }
        public string ftpfolder { get; set; }
        public string gameip { get; set; }
        public string gameport { get; set; }
        public string gamepwd { get; set; }


        public Form1()
        {
            InitializeComponent();
        }

        private string CleanString(string input)
        {
            string output = "";

            foreach (char ch in input)
            {
                bool isCharCorrect = okchar.Contains(ch);
                if (isCharCorrect)
                {
                    output += ch;
                }
            }
            return output;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.AppendText($"Загрузка данных...\r\n");
            //label1.BackColor = Color.FromArgb(1,1,1,1);

            TcpClient client = new TcpClient(ipaddr, port);
            NetworkStream stream = client.GetStream();
            StringBuilder builder = new StringBuilder();

            SendData(stream, clientid);

            byte[] answerBuffer = new byte[128];

            do
            {
                stream.Read(answerBuffer,0,answerBuffer.Length);
                builder.Append(Encoding.Unicode.GetString(answerBuffer));
            } while (stream.DataAvailable);

            string answer = builder.ToString();
            string[] answerArray = answer.Split(':');

            answer = answerArray[1];
            string correctAnswer = CleanString(answer);

            label1.Text = correctAnswer;

            bool gettingData;

            switch (correctAnswer)
            {
                case "ok":
                    gettingData = true;
                    break;
                default:
                    gettingData = false;
                    break;
            }

            builder.Clear();

            if (gettingData)
            {
                label1.Text = "Начинаю загрузку";
                FullPrepare(stream,builder);
            }
            else
            {
                label1.Text = "Отмена";
            }

        }
        private bool IsPathesExistInReg() 
        {
            bool pathesOk = true;



            return pathesOk;
        }
        private void FullPrepare(NetworkStream stream, StringBuilder builder)
        {
            byte[] answerBuffer = new byte[16];

            do
            {
                stream.Read(answerBuffer, 0, answerBuffer.Length);
                builder.Append(Encoding.Unicode.GetString(answerBuffer));
            } while (stream.DataAvailable);

            string answer = CleanString(builder.ToString());
            string[] answerArray = answer.Split(':');

            //textBox1.AppendText($"\r\n{answer}");

            protocol = answerArray[0];
            link = answerArray[1];
            ftpip = answerArray[2];
            ftpport = answerArray[3];
            ftpuser = answerArray[4];
            ftppwd = answerArray[5];
            ftpfolder = answerArray[6];
            gameip = answerArray[7];
            gameport = answerArray[8];
            gamepwd = answerArray[9];

            textBox1.AppendText($"\r\n{protocol}");
            textBox1.AppendText($"\r\n{link}");
            textBox1.AppendText($"\r\n{ftpip}");
            textBox1.AppendText($"\r\n{ftpport}");
            textBox1.AppendText($"\r\n{ftpuser}");
            textBox1.AppendText($"\r\n{ftppwd}");
            textBox1.AppendText($"\r\n{ftpfolder}");
            textBox1.AppendText($"\r\n{gameip}");
            textBox1.AppendText($"\r\n{gameport}");
            textBox1.AppendText($"\r\n{gamepwd}");


        }
        private void SendData(NetworkStream stream, string info)
        {
            byte[] data = Encoding.Unicode.GetBytes(info);
            stream.Write(data, 0, data.Length);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}


/*
 StringBuilder StrBuilder = new StringBuilder();

            byte[] data = Encoding.Unicode.GetBytes(clientid);
            byte[] buffer = new byte[256];

            stream.Write(data, 0, data.Length);

            stream.Flush();

            int incoming = 0;
            byte[] incomingData = new byte[256];
            while (stream.DataAvailable)
            {
                incoming = stream.Read(buffer, 0, buffer.Length);
                string str = Encoding.Unicode.GetString(incomingData, 0, incoming);
                StrBuilder.Append(str);
            }
            textBox1.Text += StrBuilder.ToString()+"\r\n";* 


           private string GetData(NetworkStream stream)
        {
            string result;

            StringBuilder StrBuilder = new StringBuilder();

            int bytes;
            byte[] data = new byte[256];

            do
            {
                bytes = stream.Read(data, 0, data.Length);
                StrBuilder.Append(Encoding.UTF8.GetString(data, 0, bytes));
            } while (stream.DataAvailable);

            result = StrBuilder.ToString();

            return result;
        }

 */
