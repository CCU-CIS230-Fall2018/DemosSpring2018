using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StreamsIONetworking
{
    class Program
    {
        static void Main(string[] args)
        {
            MemoryStreamDemo();

            //FileStreamDemo();

            //StreamReaderWriterDemo();

            //StreamCompressionDemo();

            //UriDemo();

            //WebClientDemo();

            //HttpClientDemo();

            //HttpListenerDemo();
        }

        private static void MemoryStreamDemo()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Console.WriteLine($"CanRead: {stream.CanRead}");
                Console.WriteLine($"CanWrite: {stream.CanWrite}");
                Console.WriteLine($"CanSeek: {stream.CanSeek}");

                for (int i = 0; i < 10; i++)
                {
                    stream.WriteByte((byte)i);
                }

                // Must move back to the start of the stream to read from it. This is allowed
                // because the stream CanSeek.
                stream.Position = 0;

                int byteValue;
                while ((byteValue = stream.ReadByte()) != -1)
                {
                    Console.WriteLine(byteValue);
                }
            }
        }

        private static void FileStreamDemo()
        {
            using (FileStream stream = new FileStream("file.txt", FileMode.Create))
            {
                Console.WriteLine($"CanRead: {stream.CanRead}");
                Console.WriteLine($"CanWrite: {stream.CanWrite}");
                Console.WriteLine($"CanSeek: {stream.CanSeek}");

                for (int i = 0; i < 10; i++)
                {
                    stream.WriteByte((byte)i);
                }

                // Must move back to the start of the stream to read from it. This is allowed
                // because the stream CanSeek.
                stream.Position = 0;

                int byteValue;
                while ((byteValue = stream.ReadByte()) != -1)
                {
                    Console.WriteLine(byteValue);
                }
            }
        }

        private static void StreamReaderWriterDemo()
        {
            using (FileStream stream = new FileStream("file.txt", FileMode.Create))
            using (StreamWriter writer = new StreamWriter(stream))
            // Or, simpler but less file options: using (StreamWriter writer = new StreamWriter("file.txt", false))
            {
                writer.Write(true);
                writer.Write('b');
                writer.Write(45.23M);
                writer.WriteLine("The cow jumped over the moon");
            }

            using (StreamReader reader = new StreamReader("file.txt"))
            {
                Console.WriteLine(reader.ReadLine());
            }
        }

        private static void StreamCompressionDemo()
        {
            using (FileStream stream = new FileStream("file.gz", FileMode.Create))
            using (GZipStream zipStream = new GZipStream(stream, CompressionMode.Compress))
            using (StreamWriter writer = new StreamWriter(zipStream))
            {
                writer.Write(true);
                writer.Write('b');
                writer.Write(45.23M);

                for (int i = 0; i < 1000; i++)
                {
                    writer.WriteLine("The cow jumped over the moon");
                }
            }

            using (FileStream stream = new FileStream("file.gz", FileMode.Open))
            using (GZipStream zipStream = new GZipStream(stream, CompressionMode.Decompress))
            using (StreamReader reader = new StreamReader(zipStream))
            {
                Console.WriteLine(reader.ReadToEnd());
            }
        }

        private static void UriDemo()
        {
            Console.WriteLine("Uri:");
            Uri link = new Uri("https://msdn.microsoft.com/en-us/library/system.uri(v=vs.110).aspx?query=abc");
            Console.WriteLine($"Scheme: {link.Scheme}");
            Console.WriteLine($"Port: {link.Port}");
            Console.WriteLine($"Host: {link.Host}");
            Console.WriteLine($"PathAndQuery: {link.PathAndQuery}");

            Console.WriteLine();
            Console.WriteLine("UriBuilder:");
            UriBuilder linkBuilder = new UriBuilder("https://msdn.microsoft.com/en-us/library/system.uri(v=vs.110).aspx?query=abc");
            Console.WriteLine($"Scheme: {linkBuilder.Scheme}");
            Console.WriteLine($"Port: {linkBuilder.Port}");
            Console.WriteLine($"Host: {linkBuilder.Host}");
            Console.WriteLine($"Path: {linkBuilder.Path}");
            Console.WriteLine($"Query: {linkBuilder.Query}");
        }

        private static void WebClientDemo()
        {
            string searchUrl = "https://www.google.com/search?q=c%23+string+interpolation&rlz=1C1CHBF_enUS725US725&oq=c%23+string+interpolation&aqs=chrome..69i57j69i58j0l4.3723j0j8&sourceid=chrome&ie=UTF-8";

            using (WebClient client = new WebClient())
            {
                Console.WriteLine(client.DownloadString(searchUrl));
            }

            using (WebClient client = new WebClient())
            using (FileStream fileStream = File.Open("webClientSearch.html", FileMode.Create))
            {
                Stream searchStream = client.OpenRead(searchUrl);
                searchStream.CopyTo(fileStream);
            }
        }

        private static void HttpClientDemo()
        {
            string searchUrl = "https://www.google.com/search?q=c%23+string+interpolation&rlz=1C1CHBF_enUS725US725&oq=c%23+string+interpolation&aqs=chrome..69i57j69i58j0l4.3723j0j8&sourceid=chrome&ie=UTF-8";

            using (HttpClient client = new HttpClient())
            {
                Console.WriteLine(client.GetStringAsync(searchUrl).Result);
            }

            //using (HttpClient client = new HttpClient())
            //using (FileStream fileStream = File.Open("httpClientSearch.html", FileMode.Create))
            //{
            //    Stream searchStream = client.GetStreamAsync(searchUrl).Result;
            //    searchStream.CopyTo(fileStream);
            //}
        }

        private static void HttpListenerDemo()
        {
            int portNumber = 8001;
            UriBuilder uri = new UriBuilder("http", "localhost", portNumber);

            // Create a listener.
            using (HttpListener listener = new HttpListener())
            {
                listener.Prefixes.Add(uri.ToString());
                listener.Start();

                Console.WriteLine("Listening...");

                // Note: The GetContext method blocks while waiting for a request. 
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                // Obtain a response object.
                HttpListenerResponse response = context.Response;

                // Construct a response.
                string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                // You must close the output stream.
                output.Close();
                listener.Stop();
            }

            //ListenAsync(portNumber);

            //Console.WriteLine($"Server running on port {portNumber}. Press Enter to stop.");
            //Console.ReadLine();
        }

        async static void ListenAsync(int portNumber)
        {
            UriBuilder uri = new UriBuilder("http", "localhost", portNumber);

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(uri.ToString());
            listener.Start();

            while (true)
            {
                try
                {
                    var context = await listener.GetContextAsync();
                    await Task.Run(() => ProcessRequestAsync(context));
                }
                catch (HttpListenerException) { break; }
                catch (InvalidOperationException) { break; }
            }

            listener.Stop();
        }

        private static void ProcessRequestAsync(HttpListenerContext context)
        {
            string x = context.Request.QueryString["x"];
            string y = context.Request.QueryString["y"];

            if (!string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(y))
            {
                string result = x + y;

                context.Response.ContentLength64 = Encoding.UTF8.GetByteCount(result.ToString());
                context.Response.StatusCode = (int)HttpStatusCode.OK;

                using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                {
                    writer.Write(result);
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.OutputStream.Close();
            }
        }
    }
}
