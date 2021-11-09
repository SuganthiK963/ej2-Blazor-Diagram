using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Data
{
    /// <summary>
    /// Derived HttpMessageContent class to prepare or modify the multipart type requests.
    /// Reference from the https://github.com/aspnet/AspNetWebStack/blob/master/src/System.Net.Http.Formatting/HttpMessageContent.cs to prepare a HttpContent extension.
    /// </summary>
    /// <exclude/>
    public class HttpMessageContent : HttpContent
    {
        private const string SP = " ";
        private const string ColonSP = ": ";
        private const string CRLF = "\r\n";
        private const string CommaSeparator = ", ";

        private const int DefaultHeaderAllocation = 2 * 1024;

        private const string DefaultMediaType = "application/http";

        private const string MsgTypeParameter = "msgtype";
        private const string DefaultRequestMsgType = "request";
        private const string DefaultResponseMsgType = "response";

        private static readonly HashSet<string> _singleValueHeaderFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Cookie",
            "Set-Cookie",
            "X-Powered-By",
        };

        private static readonly HashSet<string> _spaceSeparatedValueHeaderFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "User-Agent",
        };

        private bool _contentConsumed;
        private Lazy<Task<Stream>> _streamTask;

        public HttpMessageContent(HttpRequestMessage httpRequest)
        {
            HttpRequestMessage = httpRequest;
            Headers.ContentType = new MediaTypeHeaderValue(DefaultMediaType);
            Headers.ContentType.Parameters.Add(new NameValueHeaderValue(MsgTypeParameter, DefaultRequestMsgType));

            InitializeStreamTask();
        }

        public HttpMessageContent(HttpResponseMessage httpResponse)
        {
            HttpResponseMessage = httpResponse;
            Headers.ContentType = new MediaTypeHeaderValue(DefaultMediaType);
            Headers.ContentType.Parameters.Add(new NameValueHeaderValue(MsgTypeParameter, DefaultResponseMsgType));

            InitializeStreamTask();
        }

        private HttpContent Content
        {
            get { return HttpRequestMessage != null ? HttpRequestMessage.Content : HttpResponseMessage.Content; }
        }

        public HttpRequestMessage HttpRequestMessage { get; private set; }

        public HttpResponseMessage HttpResponseMessage { get; private set; }

        private void InitializeStreamTask()
        {
            _streamTask = new Lazy<Task<Stream>>(() => Content == null ? null : Content.ReadAsStreamAsync());
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            byte[] header = SerializeHeader();
            await stream?.WriteAsync(header, 0, header.Length);

            if (Content != null)
            {
                Stream readStream = await _streamTask.Value;
                ValidateStreamForReading(readStream);
                await Content.CopyToAsync(stream);
            }
        }

        // This method should return lenght of the each request content.
        // Need to ensure all headers and data is included in the length calculation.
        // If any content missed in calculation then unexpected issues may occur.
        // Ex: BLAZ-5485 - content-length missed hence above 5 records not updated.
        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            byte[] header = SerializeHeader();
            long contentLength = Content?.Headers.ContentLength ?? 0;
            if (Content != null)
            {
                length += ("Content-Length" + ColonSP + contentLength).Length;
            }

            length += header.Length + contentLength;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (HttpRequestMessage != null)
                {
                    HttpRequestMessage.Dispose();
                    HttpRequestMessage = null;
                }

                if (HttpResponseMessage != null)
                {
                    HttpResponseMessage.Dispose();
                    HttpResponseMessage = null;
                }
            }

            base.Dispose(disposing);
        }

        private static void SerializeRequestLine(StringBuilder message, HttpRequestMessage httpRequest)
        {
            Contract.Assert(message != null, "message cannot be null");
            message.Append(httpRequest.Method + SP);
            message.Append(httpRequest.RequestUri.PathAndQuery + SP);
            message.Append("HTTP/1.1" + CRLF);
        }

        private static void SerializeHeaderFields(StringBuilder message, HttpHeaders headers)
        {
            Contract.Assert(message != null, "message cannot be null");
            if (headers != null)
            {
                foreach (KeyValuePair<string, IEnumerable<string>> header in headers)
                {
                    if (_singleValueHeaderFields.Contains(header.Key))
                    {
                        foreach (string value in header.Value)
                        {
                            message.Append(header.Key + ColonSP + value + CRLF);
                        }
                    }
                    else if (_spaceSeparatedValueHeaderFields.Contains(header.Key))
                    {
                        message.Append(header.Key + ColonSP + string.Join(SP, header.Value) + CRLF);
                    }
                    else
                    {
                        message.Append(header.Key + ColonSP + string.Join(CommaSeparator, header.Value) + CRLF);
                    }
                }
            }
        }

        private byte[] SerializeHeader()
        {
            StringBuilder message = new StringBuilder(DefaultHeaderAllocation);
            HttpHeaders headers = null;
            HttpContent content = null;
            SerializeRequestLine(message, HttpRequestMessage);
            headers = HttpRequestMessage.Headers;
            content = HttpRequestMessage.Content;
            SerializeHeaderFields(message, headers);
            if (content != null)
            {
                SerializeHeaderFields(message, content.Headers);
            }

            message.Append(CRLF);
            return Encoding.UTF8.GetBytes(message.ToString());
        }

        private void ValidateStreamForReading(Stream stream)
        {
            if (_contentConsumed)
            {
                stream.Position = 0;
            }

            _contentConsumed = true;
        }
    }
}