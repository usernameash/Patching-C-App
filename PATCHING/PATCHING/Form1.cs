using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PATCHING
{
    public partial class Form1 : Form
    {
        // get these from config
        string username;
        string password;
        List<serverConfig> Servers = new List<serverConfig>();
        public List<string> Addresses_to_check { get; set; } = new List<string>();



        // Make a config file for crednetials



        public Form1()
        {
            // 

            var settings = ConfigurationManager.AppSettings;

            foreach(var line in settings.AllKeys)
            {

                if (line.StartsWith("addr,"))
                {
                    var address = line.Split(',')[1];
                    var servertype = settings.Get(line);
                    Servers.Add(new serverConfig { 
                        Address = address,
                        commands = settings.Get(servertype).Split(';')}
                    );

                }
            }

             username = settings.Get("username");
             password = settings.Get("password");




            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var list = textBox1.Text;
            // trim input strings 
            Addresses_to_check = list.Split('\n').ToList();
            Char[] charsToTrim = { '\r' };

            // Start connecting to the server/node
            startcheckin();

            

        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Enter the field of servers you are wanting to check or to be checked
        }

       private void startcheckin ()
        {
            foreach (var address in Addresses_to_check)
            
            {
                serverConfig config = Servers.First(server => server.Address == address);


                var Client = new Renci.SshNet.SshClient(address, username, password);
                Client.Connect();
                foreach (string command in config.commands)
                {
                   var cmd = Client.RunCommand(command);
                    textBox2.Text +=($"{cmd.Result}\r\n");
                }
                Client.Disconnect();
                // find statements 
                // write command response to text field
                // execute commands
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}