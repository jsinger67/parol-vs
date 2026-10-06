using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace parol_vs
{
    [Export(typeof(ILanguageClient))]
    [ContentType("parol")]
    [Name("parol Language Client")]
    internal sealed class ParolLanguageClient : ILanguageClient
    {
        private const string LanguageServerPathVariable = "PAROL_LS_PATH";
        private const string LanguageServerArgsVariable = "PAROL_LS_ARGS";

        private Process? languageServerProcess;

        public string Name => "parol Language Client";

        public IEnumerable<string> ConfigurationSections => Array.Empty<string>();

        public object? InitializationOptions => null;

        public IEnumerable<string> FilesToWatch => Array.Empty<string>();

        public bool ShowNotificationOnInitializeFailed => true;

        public event AsyncEventHandler<EventArgs>? StartAsync;

        public event AsyncEventHandler<EventArgs>? StopAsync;

        public async Task<Connection> ActivateAsync(CancellationToken token)
        {
            await Task.Yield();

            string fileName = Environment.GetEnvironmentVariable(LanguageServerPathVariable) ?? "parol-ls";
            string arguments = Environment.GetEnvironmentVariable(LanguageServerArgsVariable) ?? "--stdio";

            var processStartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };

            this.languageServerProcess = Process.Start(processStartInfo);
            if (this.languageServerProcess == null)
            {
                throw new InvalidOperationException("Failed to start parol language server process.");
            }

            _ = Task.Run(async () =>
            {
                try
                {
                    using var reader = this.languageServerProcess!.StandardError;
                    var logPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"parol-ls-stderr_{this.languageServerProcess.Id}.log");
                    using var writer = new System.IO.StreamWriter(logPath, append: true);
                    string? line;
                    while ((line = await reader.ReadLineAsync()) != null && !token.IsCancellationRequested)
                    {
                        System.Diagnostics.Debug.WriteLine($"parol-ls stderr: {line}");
                        await writer.WriteLineAsync(line);
                        await writer.FlushAsync();
                    }
                }
                catch { }
            }, token);

            return new Connection(this.languageServerProcess.StandardOutput.BaseStream, this.languageServerProcess.StandardInput.BaseStream);
        }

        public Task OnLoadedAsync()
        {
            return this.StartAsync?.InvokeAsync(this, EventArgs.Empty) ?? Task.CompletedTask;
        }

        public Task OnServerInitializedAsync()
        {
            return Task.CompletedTask;
        }

        public Task<InitializationFailureContext?> OnServerInitializeFailedAsync(ILanguageClientInitializationInfo initializationState)
        {
            this.TryStopLanguageServerProcess();
            return Task.FromResult<InitializationFailureContext?>(null);
        }

        private void TryStopLanguageServerProcess()
        {
            if (this.languageServerProcess == null)
            {
                return;
            }

            if (!this.languageServerProcess.HasExited)
            {
                this.languageServerProcess.Kill();
            }

            this.languageServerProcess.Dispose();
            this.languageServerProcess = null;
        }
    }
}
