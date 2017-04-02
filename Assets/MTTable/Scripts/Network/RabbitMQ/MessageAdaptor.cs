using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
// using RabbitMQ.Client.

public class MessageAdaptor
{
    // public static string RABBITMQ_HOSTNAME = "localhost";
    public static string RABBITMQ_HOSTNAME = "127.0.0.1";
    public static string RABBITMQ_EXCHANGE_NAME = "default";
    public static string RABBITMQ_DEFAULT_QUEUE = "default_queue";

    public static string RABBITMQ_ROUTING_KEY_WILDCARD = "#";

    
    public class EventType
    {
        public const string Speech = "speech";
        public const string Hotword = "hotword";
        public const string MidAirTokens = "mid_air_token";
        
    }


    public static class CustomSocketFactory
    {
        public static System.Net.Sockets.TcpClient GetSocket(System.Net.Sockets.AddressFamily addressFamily)
        {
            System.Net.Sockets.TcpClient tcpClient = new System.Net.Sockets.TcpClient(System.Net.Sockets.AddressFamily.InterNetwork);
            tcpClient.NoDelay = true;
            return tcpClient;
        }
    }

    public delegate void RabbitMQMessageHandler(EventMessage message);

	ConnectionFactory cf; 
	IModel channel; 
    EventingBasicConsumer consumer;
 

    Dictionary<string, RabbitMQMessageHandler> eventHandlers
        = new Dictionary<string, RabbitMQMessageHandler>();
    
    public class EventMessage {};

    [Serializable]
    public class SpeechMessage : EventMessage
    {
        public string sentence;
        public float confidence; 
        public string entity;
        public string intent; 
    }

    [Serializable]
    public class HotwordMessage : EventMessage
    {
        public string hotword; 
    }

    [Serializable]
    public class MidAirTokenMessage: EventMessage
    {
        public string fromTokenId; // The Mack Addresses of the token
        public string toTokenId; // The Mac Addresses of the token
        public int distance; 

    }

    public MessageAdaptor()
    {
        cf = new ConnectionFactory();
		cf.SocketFactory = new ConnectionFactory.ObtainSocket(CustomSocketFactory.GetSocket);
		cf.HostName = RABBITMQ_HOSTNAME;
        cf.UserName = "yujen";
        cf.Password = "yujen"; 
        


        IConnection connection = null;

        try
		{
			connection = cf.CreateConnection();
		}
		catch (Exception e)
		{
			Debug.Log("connection - Error has Occured");
			Debug.Log("Exception: " + e.ToString());
			Debug.Log("Stack Trace: " + e.StackTrace.ToString());
			Debug.Log("Message: " + e.Message);
			Debug.Log("Inner Exception: " + e.InnerException.ToString()); 
            return;
		}

        Debug.Log("connection Created");

        this.channel = connection.CreateModel();
        this.channel.ExchangeDeclare(RABBITMQ_EXCHANGE_NAME, ExchangeType.Topic);

        string randomQueueName = System.Guid.NewGuid().ToString();
        this.channel.QueueDeclare(randomQueueName, false, true, false, null);
        this.channel.QueueBind(randomQueueName, RABBITMQ_EXCHANGE_NAME,
            RABBITMQ_ROUTING_KEY_WILDCARD, null);
        
        this.consumer = new EventingBasicConsumer(this.channel);

        this.consumer.Received += this.onMessage;
        // this.consumer.Shutdown += this.onShutdown;

        string consumerTag = this.channel.BasicConsume(
            queue: randomQueueName, 
            noAck: true, 
            consumer: this.consumer);

    
    }

    private void onMessage (IBasicConsumer sender, 
        BasicDeliverEventArgs args) 
    
   {
        var body = args.Body;
        var message = Encoding.UTF8.GetString(body);
        // Debug.Log (message);
        if(this.eventHandlers.ContainsKey(args.RoutingKey)) {

            // var body = args.Body;
            // var message = Encoding.UTF8.GetString(body);
            // Debug.Log (message);

            switch(args.RoutingKey) 
            {
                case EventType.Speech:
                    SpeechMessage spMsg = JsonUtility.FromJson<SpeechMessage>(message);
                    this.eventHandlers[args.RoutingKey](spMsg);
                    break;
                case EventType.Hotword:
                    HotwordMessage hwMsg = JsonUtility.FromJson<HotwordMessage>(message);
                    this.eventHandlers[args.RoutingKey](hwMsg);
                    break;
                case EventType.MidAirTokens:
                    MidAirTokenMessage midAirTokenMessage = 
                        JsonUtility.FromJson<MidAirTokenMessage>(message);
                    this.eventHandlers[args.RoutingKey](midAirTokenMessage);
                    break;
                default:
                    break;
            }
        }

        // if(this.eventHandlers.ContainsKey(args.RoutingKey)) {
        //     this.eventHandlers[args.RoutingKey](sender, args);
        // } 
    
    }

    // private void onShutdown (IBasicConsumer sender, 
    //     BasicDeliverEventArgs args) {
    //     Debug.Log ("onShutdown"); 
    // }


    
    public void subscribe(string eventType, RabbitMQMessageHandler handler)
    {
        switch(eventType) 
        {
            case EventType.Speech:
                this.eventHandlers[eventType] = handler;
                break;
            case EventType.Hotword:
                this.eventHandlers[eventType] = handler;
                break;
            case EventType.MidAirTokens:
                this.eventHandlers[eventType] = handler;
                break;
            default:
                break;
        }

    }

}
 