using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

Console.WriteLine("Server is running");
TcpListener listener = new TcpListener(IPAddress.Any, 7); 
listener.Start(); //begynder at lytte. Reserveret og venter på klienter
Console.WriteLine("Server is waiting for connection..");


while (true)
{
    //Serveren Accepterer en ny klient-forbindelse
    TcpClient socket = listener.AcceptTcpClient();
    Console.WriteLine("Client connected!");
    // Opret en ny tråd (Task) til at håndtere den specifikke klient
    Task.Run(() => HandleClient(socket));

}
  
void HandleClient(TcpClient socket)
{
    NetworkStream ns = socket.GetStream();
    StreamReader reader = new StreamReader(ns);
    StreamWriter writer = new StreamWriter(ns) { AutoFlush = true };


    while (socket.Connected)
    {

        string? message = reader.ReadLine(); // Read message from the client
        if (message == null)
        {
            Console.WriteLine("Client has disconnected");
            break;
        }
        Console.WriteLine($"Received command: {message}");


        switch (message)
        {
            case "Random":
                writer.WriteLine("Input numbers");
                string? numbers = reader.ReadLine();
                var randomResult = GetRandomInRange(numbers);
                writer.WriteLine(randomResult);
                break;

            case "Add":
                writer.WriteLine("Input numbers");
                numbers = reader.ReadLine();
                var addResult = AddNumbers(numbers);
                writer.WriteLine(addResult);
                break;

            case "Subtract":
                writer.WriteLine("Input numbers");
                numbers = reader.ReadLine();
                var subtractResult = SubtractNumbers(numbers);
                writer.WriteLine(subtractResult);
                break;

            default:
                writer.WriteLine("Unknown command");
                break;
        }
    }
}
static int GetRandomInRange(string input)
{
    string[] parts = input.Split(' ');
    int num1 = int.Parse(parts[0]);
    int num2 = int.Parse(parts[1]);
    Random rand = new Random();
    return rand.Next(num1, num2 + 1); 
}

static int AddNumbers(string input)
{
    string[] parts = input.Split(' ');
    int num1 = int.Parse(parts[0]);
    int num2 = int.Parse(parts[1]);
    return num1 + num2;
}

static int SubtractNumbers(string input)
{
    string[] parts = input.Split(' ');
    int num1 = int.Parse(parts[0]);
    int num2 = int.Parse(parts[1]);
    return num1 - num2;
}

