using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            using (NamedPipeServerStream pipeServer = new NamedPipeServerStream(
                "TestPipe", PipeDirection.In, 1,
                PipeTransmissionMode.Byte, PipeOptions.Asynchronous))
            {
                Console.WriteLine("Ожидание подключения отправителя...");
                await pipeServer.WaitForConnectionAsync();
                Console.WriteLine("Отправитель подключился.");

                byte[] buffer = new byte[1024];
                List<byte> messageBytes = new List<byte>();

                while (true)
                {
                    int bytesRead = await pipeServer.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        break;
                    }

                    for (int i = 0; i < bytesRead; i++)
                    {
                        if (buffer[i] == 10)
                        {
                            string message = Encoding.UTF8.GetString(messageBytes.ToArray());
                            Console.WriteLine("Получено: " + message);
                            messageBytes.Clear();
                        }
                        else
                        {
                            messageBytes.Add(buffer[i]);
                        }
                    }
                }

                Console.WriteLine("Отправитель отключился.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }
}
