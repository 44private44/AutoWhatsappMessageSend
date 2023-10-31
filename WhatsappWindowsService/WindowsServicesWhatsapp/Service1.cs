using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace WindowsServicesWhatsapp
{
    public partial class Service1 : ServiceBase
    {

        System.Timers.Timer timer = new System.Timers.Timer();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 2 * 60 * 60 * 1000;
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }
        // Recall the file accordingly time period
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteToFile("Service is recall at " + DateTime.Now);
            sendWhatsappMessage();
        }
        // Write in file 
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
        public void sendWhatsappMessage()
        {
            var accountSid = "AC6e6c6e6b3a25051cca0345d72ee3641c";
            var authToken = "e3d24f10e2fc02a5059f4a4b3901cd0f";
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
              new PhoneNumber("whatsapp:+919328243252"));
            messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
            messageOptions.Body = "😊 Hey handsome, today is your day \nYou can do anything, just believe in that...\n😊 Smile and be Confident 😊";

            var message = MessageResource.Create(messageOptions);
            Console.WriteLine(message.Body);

            Console.WriteLine("Messages sent successfully!");
        }
    }
}
