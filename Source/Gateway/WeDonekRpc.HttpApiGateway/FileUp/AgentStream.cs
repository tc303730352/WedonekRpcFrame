using System;
using System.IO;

namespace WeDonekRpc.HttpApiGateway.FileUp
{
    internal class AgentStream : Stream
    {
        private readonly Stream _Stream;

        private readonly int _Skip = 0;

        public AgentStream(FileInfo file, int skip)
        {
            this._Stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            this._Skip = skip;
            this._Stream.Position = skip;
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => this._Stream.Position - this._Skip;

        public override long Position { get => this._Stream.Position - this._Skip; set => this._Stream.Position = value + this._Skip; }

        public override void Flush()
        {
            this._Stream.Flush();
        }
        protected override void Dispose(bool disposing)
        {
            this._Stream.Dispose();
        }
        public override void Write(ReadOnlySpan<byte> buffer)
        {
            this._Stream.Write(buffer);
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            return this._Stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            offset += this._Skip;
            return this._Stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            value += this._Skip;
            this._Stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this._Stream.Write(buffer, offset, count);
        }
    }
}
