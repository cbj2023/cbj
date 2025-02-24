﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.IO;
using System.Security.Cryptography;//ecb
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SUTAIMES;
using SUTAIMES.Common;

namespace SUTAIMES.Http
{

    public class HttpProcessor
    {
        public TcpClient socket;
        public HttpServer srv;

        private Stream inputStream;
        public StreamWriter outputStream;

        public String http_method;
        public String http_url;
        public String http_protocol_versionstring;
        public Hashtable httpHeaders = new Hashtable();


        private static int MAX_POST_SIZE = 10 * 1024 * 1024; // 10MB

        Program tSystem;
        public HttpProcessor(TcpClient s, HttpServer srv, Program tSys)
        {
            tSystem = tSys;
            this.socket = s;
            this.srv = srv;
        }


        private string streamReadLine(Stream inputStream)
        {
            int next_char;
            string data = "";
            while (true)
            {
                next_char = inputStream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\r') { continue; }
                if (next_char == -1) { Thread.Sleep(1); continue; };
                data += Convert.ToChar(next_char);
            }
            return data;
        }
        public void process()
        {
            // we can't use a StreamReader for input, because it buffers up extra data on us inside it's
            // "processed" view of the world, and we want the data raw after the headers
            inputStream = new BufferedStream(socket.GetStream());

            // we probably shouldn't be using a streamwriter for all output from handlers either
            outputStream = new StreamWriter(new BufferedStream(socket.GetStream()));
            try
            {
                parseRequest();
                readHeaders();
                if (http_method.Equals("GET"))
                {
                    handleGETRequest();
                }
                else if (http_method.Equals("POST"))
                {
                    handlePOSTRequest();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
                writeFailure();
            }
            outputStream.Flush();
            // bs.Flush(); // flush any remaining output
            inputStream = null; outputStream = null; // bs = null;            
            socket.Close();
        }

        public void parseRequest()
        {
            String request = streamReadLine(inputStream);
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception("invalid http request line");
            }
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_versionstring = tokens[2];

            Console.WriteLine("starting: " + request);
        }

        public void readHeaders()
        {
            Console.WriteLine("readHeaders()");
            String line;
            while ((line = streamReadLine(inputStream)) != null)
            {
                if (line.Equals(""))
                {
                    Console.WriteLine("got headers");
                    return;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++; // strip any spaces
                }

                string value = line.Substring(pos, line.Length - pos);
                Console.WriteLine("header: {0}:{1}", name, value);
                httpHeaders[name] = value;
            }
        }

        public void handleGETRequest()
        {
            srv.handleGETRequest(this);
        }

        private const int BUF_SIZE = 4096;
        public void handlePOSTRequest()
        {
            // this post data processing just reads everything into a memory stream.
            // this is fine for smallish things, but for large stuff we should really
            // hand an input stream to the request processor. However, the input stream 
            // we hand him needs to let him see the "end of the stream" at this content 
            // length, because otherwise he won't know when he's seen it all! 

            Console.WriteLine("get post data start");
            int content_len = 0;
            MemoryStream ms = new MemoryStream();
            if (this.httpHeaders.ContainsKey("Content-Length"))
            {
                content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
                if (content_len > MAX_POST_SIZE)
                {
                    throw new Exception(
                        String.Format("POST Content-Length({0}) too big for this simple server",
                          content_len));
                }
                byte[] buf = new byte[BUF_SIZE];
                int to_read = content_len;
                while (to_read > 0)
                {
                    Console.WriteLine("starting Read, to_read={0}", to_read);

                    int numread = this.inputStream.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
                    Console.WriteLine("read finished, numread={0}", numread);
                    if (numread == 0)
                    {
                        if (to_read == 0)
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception("client disconnected during post");
                        }
                    }
                    to_read -= numread;
                    ms.Write(buf, 0, numread);
                }
                ms.Seek(0, SeekOrigin.Begin);
            }
            Console.WriteLine("get post data end");
            //StreamReader read = new StreamReader(fs, Encoding.Default)
            srv.handlePOSTRequest(this, new StreamReader(ms, Encoding.GetEncoding("utf-8")));//utf-8  //GB2312

        }

        public void writeSuccess()
        {
            outputStream.WriteLine("HTTP/1.0 200 OK");
            outputStream.WriteLine("Content-Type: text/html");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }

        public void writeFailure()
        {
            outputStream.WriteLine("HTTP/1.0 404 File not found");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }
    }

    public abstract class HttpServer
    {

        protected int port;
        TcpListener listener;
        bool is_active = true;

        Program tSystem;

        public HttpServer(int port, Program tSys)
        {
            tSystem = tSys;
            this.port = port;

        }

        public void listen()
        {
            System.Net.IPAddress _locIp = IPAddress.Parse("192.168.0.163");//"127.0.0.1");10.7.45.100
            IPEndPoint ipEndpoint = new IPEndPoint(_locIp, port);
            //listener = new TcpListener(port);
            listener = new TcpListener(port);
            listener.Start();
            while (is_active)
            {
                TcpClient s = listener.AcceptTcpClient();
                HttpProcessor processor = new HttpProcessor(s, this, tSystem);
                Thread thread = new Thread(new ThreadStart(processor.process));
                thread.Start();
                Thread.Sleep(1);
            }
        }

        public abstract void handleGETRequest(HttpProcessor p);
        public abstract void handlePOSTRequest(HttpProcessor p, StreamReader inputData);
    }

    public class MyHttpServer : HttpServer
    {
        Program tSystem;
        public MyHttpServer(int port, Program tSys)
            : base(port, tSys)
        {
            tSystem = tSys;
        }
        public override void handleGETRequest(HttpProcessor p)
        {
            Console.WriteLine("request: {0}", p.http_url);
            p.writeSuccess();
            p.outputStream.WriteLine("<html><body><h1>test server</h1>");
            p.outputStream.WriteLine("Current Time: " + DateTime.Now.ToString());
            p.outputStream.WriteLine("url : {0}", p.http_url);

            p.outputStream.WriteLine("<form method=post action=/form>");
            p.outputStream.WriteLine("<input type=text name=foo value=foovalue>");
            p.outputStream.WriteLine("<input type=submit name=bar value=barvalue>");
            p.outputStream.WriteLine("</form>");
        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            //StreamReader read = new StreamReader(fs, Encoding.Default);
            string tAcepStr = "";
            Console.WriteLine("POST request: {0}", p.http_url);

            string data = inputData.ReadToEnd();




            tAcepStr = OperReturnData(p.http_url, data);
            p.writeSuccess();
            p.outputStream.WriteLine("{0}", tAcepStr);

        }

        public string OperReturnData(string tHttpUrl, string tInputStr)
        {
            string tRstr = "", tMsg = "",tStatus="1";
            try
            {

                string tOptimize = "";
                string tRetStr = "",tRetData="";
                tOptimize = tInputStr.Trim();
                GetData.ClsDispostErp tDispostErp = new GetData.ClsDispostErp(tSystem);
                tDispostErp.GetErpData(tOptimize, ref tRetStr);
                if (tRetStr.ToUpper() == "OK")
                {
                    tSystem.mClsDBUPdate.ExecuteTvpOptimize_batch("Pro_Optimize_batch", tOptimize, "1", ref tRetStr, ref tRetData);
                    tSystem.mClsDBUPdate.DBClose();
                }
                else
                {
                    tStatus = "0";
                    tMsg = "获取优化单基础数据失败！";
                }


                StringWriter tsw = new StringWriter();
                JsonWriter twriter = new JsonTextWriter(tsw);
                twriter.WriteStartObject();
                twriter.WritePropertyName("result");
                twriter.WriteValue(tStatus);

                twriter.WritePropertyName("msg");
                twriter.WriteValue(tMsg);


                twriter.WriteEndObject();
                twriter.Flush();

                string tPostStr = "";
                tPostStr = tsw.GetStringBuilder().ToString();

                return tPostStr;
            }
            catch (Exception ex)
            {
                tMsg = "处理语法错误" + ex.StackTrace + " 错误信息：" + ex.Message;
                return tRstr;

            }
        }


        ////////////////////
    }
}
