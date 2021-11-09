using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.ComponentModel;

namespace Syncfusion.Blazor.Inputs.Internal
{
    internal class UploaderStreamReader
    {
        /// <summary>
        /// Constructor for initializing the component adaptor.
        /// </summary>
        /// <param name="parent">Uploader arguments.</param>
        /// <exclude/>
        public UploaderStreamReader(SfUploader parent)
        {
            Parent = parent;
        }

        private SfUploader Parent { get; set; }

        internal async Task<UploadData> UploadFileAsync(int fileIndex, byte[] bufferSize, long position, long offset, int totalCount)
        {
            var result = await ReadFileBase64String(fileIndex, position, totalCount);
            var byteFile = 0;
            if (!string.IsNullOrEmpty(result))
            {
                var byteResult = Convert.FromBase64String(result);
                Parent.LocalStream = new MemoryStream(byteResult);
                byteFile = byteResult.Length;
                Array.Copy(byteResult, 0, bufferSize, offset, byteFile);
            }

            return new UploadData() { fileByte = byteFile, result = result };
        }

        internal async Task<string> ReadFileBase64String(int fileIndex, long position, int totalCount)
        {
            var data = await Parent.JSRuntime.InvokeAsync<string>("sfBlazor.Uploader.serverReadFileBase64", new object[] { Parent.FileElement, fileIndex, position, totalCount });

            // var data = await parent.InvokeMethod<string>("sfBlazor.Uploader.serverReadFileBase64", false, new object[] { parent.fileElement, fileIndex, position, totalCount });
            return data;
        }

        public class UploaderFileStream : Stream
        {
            private readonly int fileIndex;
            private readonly long length;
            private readonly UploaderStreamReader uploadFileInterop;
            private long position;

            public UploaderFileStream(int index, long length, UploaderStreamReader uploadFileInterop)
            {
                fileIndex = index;
                this.length = length;
                this.uploadFileInterop = uploadFileInterop;
            }

            public override bool CanRead => true;

            public override bool CanSeek => false;

            public override bool CanWrite => false;

            public override long Length => length;

            /// <exclude/>
            public override long Position
            {
                get => position;
                set
                {
                    position = value;
                }
            }

            public override void Flush()
                => throw new NotSupportedException();

            public override int Read(byte[] buffer, int offset, int count)
                => throw new NotSupportedException("Synchronous reads are not supported");

            public override long Seek(long offset, SeekOrigin origin)
                => throw new NotSupportedException();

            public override void SetLength(long value)
                => throw new NotSupportedException();

            public override void Write(byte[] buffer, int offset, int count)
                => throw new NotSupportedException();

            public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                UploadData uploadByte = await uploadFileInterop.UploadFileAsync(fileIndex, buffer, Position, (long)offset, count);
                Position += (long)uploadByte.fileByte;
                return uploadByte.fileByte;
            }
        }

        internal async Task<Stream> GetFileStream(ElementReference eleRef, int index)
        {
            var fileInfo = await GetFileInfo(eleRef, index);
            return new UploaderFileStream(await GetFileRead(eleRef, index), (long)fileInfo.Size, this);
        }

        internal async Task<FileInfo> GetFileInfo(ElementReference eleRef, int index)
        {
            var fileData = await Parent.InvokeMethod<FileInfo>("sfBlazor.Uploader.getFileInfo", false, new object[] { eleRef, index });
            return fileData;
        }

        internal async Task<int> UploadFileCount(ElementReference ele)
        {
            return await Parent.InvokeMethod<int>("sfBlazor.Uploader.uploadFileCount", false, new object[] { ele });
        }

        internal async Task<int> GetFileRead(ElementReference ele, int index)
        {
            return await Parent.InvokeMethod<int>("sfBlazor.Uploader.getFileRead", false, new object[] { ele, index });
        }

        /// <exclude/>
        internal class UploadData
        {
            public int fileByte { get; set; }

            public string result { get; set; }
        }
    }

    internal class UploadFileList : IUploadFileList
    {
        internal UploadFileList(ElementReference fileElement, UploaderStreamReader uploaderFileInterop)
        {
            FileElement = fileElement;
            UploaderFileInterop = uploaderFileInterop;
        }

        public async Task<IEnumerable<IUploadReadFile>> FileListData(ElementReference ele) =>
            Enumerable.Range(0, Math.Max(0, await UploaderFileInterop.UploadFileCount(ele)))
                .Select(index =>
                (IUploadReadFile)new UploadReadFile(this, index));

        public ElementReference FileElement { get; private set; }

        /// <exclude/>
        public UploaderStreamReader UploaderFileInterop { get; set; }
    }

    internal class UploadReadFile : IUploadReadFile
    {
        private readonly UploadFileList UploadFileListRef;

        public readonly int Index;

        internal UploadReadFile(UploadFileList uploadFileListRef, int index)
        {
            UploadFileListRef = uploadFileListRef;
            Index = index;
        }

        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Task<Stream> GetFileReader()
        {
            return UploadFileListRef.UploaderFileInterop.GetFileStream(UploadFileListRef.FileElement, Index);
        }
    }

    /// <exclude/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IUploadReadFile
    {
        /// <summary>
        /// Opens a stream to read the file.
        /// </summary>
        /// <returns>Task.</returns>
        Task<Stream> GetFileReader();
    }

    /// <summary>
    /// Specifies the interface for upload file list.
    /// </summary>
    /// <exclude/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IUploadFileList
    {
        /// <summary>
        /// Register for drop events on the source element.
        /// </summary>
        /// <param name="ele">Specifies the element reference.</param>
        /// <returns>Task.</returns>
        Task<IEnumerable<IUploadReadFile>> FileListData(ElementReference ele);
    }

    /// <summary>
    /// Specifies the upload file status.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class UploadFileDetails : FileInfo
    {
        /// <summary>
        /// Specifies the chunk size.
        /// </summary>
        public long chunksize { get; set; }

        /// <summary>
        /// Specifies the total chunk size.
        /// </summary>
        public long totalChunksize { get; set; }
    }
}