using System;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(
                ".", "TestPipe", PipeDirection.Out, PipeOptions.Asynchronous))
            {
                Console.WriteLine("Подключение к получателю...");
                await pipeClient.ConnectAsync();
                Console.WriteLine("Подключение установлено.");

                while (true)
                {
                    Console.Write("Введите сообщение (exit - выход): ");
                    string message = Console.ReadLine();

                    if (message == null || message == "exit")
                    {
                        break;
                    }

                    byte[] buffer = Encoding.UTF8.GetBytes(message + "\n");
                    await pipeClient.WriteAsync(buffer, 0, buffer.Length);
                    Console.WriteLine("Сообщение отправлено.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }
}
