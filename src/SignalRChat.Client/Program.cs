using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace SignalRChat.Client
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:2020/chathub")
                    .WithAutomaticReconnect()
                    .AddJsonProtocol()
                    .Build();

                connection.On<string>("ReceiveMessage", Console.WriteLine);

                await connection.StartAsync();

                Console.Write("Input group name: ");
                var groupName = Console.ReadLine();
                
                await connection.InvokeAsync("AddToGroup", groupName);
                
                while (true)
                {
                    await connection.InvokeAsync("Send", groupName, Console.ReadLine());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}