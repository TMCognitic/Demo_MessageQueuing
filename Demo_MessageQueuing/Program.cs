using System;
using System.Messaging; //Ajouter la référence System.Messaging

namespace Demo_MessageQueuing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (MessageQueue messageQueue = new MessageQueue(@".\Private$\demo_queue"))
            {
                Console.WriteLine($"Id : {messageQueue.Id} ");
                Console.WriteLine($"Label : {messageQueue.Label}");                
                Console.WriteLine($"Created Date : {messageQueue.CreateTime}");
                Console.WriteLine($"Readable : {messageQueue.CanRead}");
                Console.WriteLine($"Writable : {messageQueue.CanWrite}");
                Console.WriteLine($"Machine Name : {messageQueue.MachineName}");
                Console.WriteLine($"Path : {messageQueue.Path}");
                Console.WriteLine($"Queue Name : {messageQueue.QueueName}");
                
                if (messageQueue.CanWrite)
                {
                    Demo demo = new Demo() { Text = "Exemple de message..." };
                    Message message = new Message(demo);
                    messageQueue.Send(message);
                }

                if (messageQueue.CanRead)
                {
                    messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(Demo) });
                    Message[] messages = messageQueue.GetAllMessages();

                    Console.WriteLine($"il y a {messages.Length} dans la queue...");

                    foreach (Message message in messages)
                    {
                        Demo demo = (Demo)message.Body;
                        Console.WriteLine(demo.Text);
                        messageQueue.ReceiveById(message.Id); //Retire le message de la queue
                    }
                }

                Console.ReadLine();
            }
        }
    }

    public class Demo
    {
        public string Text { get; set; }
    }
}


