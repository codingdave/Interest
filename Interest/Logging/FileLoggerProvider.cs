using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;

namespace Interest.Logger
{
    internal class FileLoggerProvider : ILoggerProvider
    {
        public FileLoggerProvider(IOptions<FileLoggerOptions> options)
        {
            Options = options.Value;

            if (!Directory.Exists(Options.FolderPath))
            {
                Directory.CreateDirectory(Options.FolderPath);
            }
        }

        public FileLoggerOptions Options { get; }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(this);
        }

        public void Dispose()
        {
        }
    }
}