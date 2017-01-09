using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Reflection;
using MPE.SS.Logic.Configurations;
using MPE.SS.Logic.Extensions;
using NLog;

namespace MPE.SS.Logic
{
    internal class PowerShellService
    {
        private static object _lock = new object();
        private string _server;
        private string _username;
        private string _password;
        private string _key;

        private static ConcurrentDictionary<string, PowerShell> _shells;
        private static ConcurrentDictionary<string, object> _locks;


        public PowerShellService(
            string server,
            string username,
            string password)
        {
            _server = server;
            _username = username;
            _password = password;
            _key = string.Format("{0}-{1}-{2}", _server, _username, _password);

            Init();
        }

        public PowerShellService()
        {
            Init();
        }

        private void Init()
        {
            lock (_lock)
            {
                if (_shells == null)
                {
                    _shells = new ConcurrentDictionary<string, PowerShell>();
                }

                if (_locks == null)
                {
                    _locks = new ConcurrentDictionary<string, object>();
                }
            }
        }

        public static void Dispose()
        {
            if (_shells != null)
            {
                foreach (var powerShell in _shells)
                {
                    powerShell.Value.Runspace.Disconnect();
                    powerShell.Value.Runspace.Close();
                    powerShell.Value.Runspace.Dispose();
                    powerShell.Value.Dispose();
                }
                _shells.Clear();
                _locks.Clear();
            }
        }

        public void Invoke(Action<PowerShell> action, bool isRemote = true)
        {
            object lockObj;
            lock (_lock)
            {
                if (!_locks.TryGetValue(_key, out lockObj))
                    lockObj = _locks.AddOrUpdate(_key, new {key = _key}, (s, o) =>
                    {
                        if (o != null)
                        {
                            lockObj = o;
                            return o;
                        }
                        o = lockObj;

                        return o;
                    });
            }
            lock (lockObj)
            {
                var shell = GetShell();

                try
                {
                    if (shell != null)
                    {
                        shell.AddScript(LoadScripts(new List<string> { "SS.ps1" }).First());
                        shell.AddScript("Setup-Env");
                        var result = shell.Invoke();
                    }

                    action.Invoke(shell);

                    if (shell != null)
                    {
                        shell.Commands.Clear();
                        shell.AddScript("[gc]::collect()");
                        shell.Invoke();
                    }
                    else
                    {
                        throw new Exception("Invoking cleanup");
                    }
                }
                catch (Exception e)
                {
                    _shells.TryRemove(_key, out shell);
                    AppConfiguration.Logger.Log(LogLevel.Fatal, e);
                }
            }
        }

        private PowerShell GetShell()
        {
            PowerShell shell;
            if (_shells.ContainsKey(_key))
            {
                shell = _shells[_key];
                return shell;
            }
            else
            {
                try
                {
                    var connectionInfo = new WSManConnectionInfo();
                    connectionInfo.ComputerName = _server;
                    connectionInfo.Credential = new PSCredential(_username, _password.ToSecureString());

                    var env = RunspaceFactory.CreateRunspace(connectionInfo);
                    env.ConnectionInfo.IdleTimeout = 60000;
                    env.Open();
                    shell = PowerShell.Create();
                    shell.Runspace = env;
                    _shells.AddOrUpdate(_key, shell, (s, o) =>
                    {
                        if (o != null)
                            return o;
                        o = shell;

                        return o;
                    });
                    return shell;
                }
                catch (PSRemotingTransportException e)
                {
                    AppConfiguration.Logger.Log(LogLevel.Fatal, e);
                }
            }
            return null;
        }

        public void LoadFilesIntoShell(PowerShell shell, List<string> files)
        {
            if (files != null && files.Any())
            {
                var scripts = LoadScripts(files);
                foreach (var scriptFile in scripts)
                {
                    shell.AddScript(scriptFile);
                }
            }
        }

        public List<string> GetErrorsFromShell(PowerShell shell)
        {
            return shell.Streams.Error.Select(x => x.Exception.ToString()).ToList();
        }

        public List<string> LoadScripts(List<string> modules)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var scripts = new List<string>();
            foreach (var module in modules)
            {
                using (Stream stream = assembly.GetManifestResourceStream(Constants.SystemConstant.PowerShellScriptPath + module))
                using (StreamReader reader = new StreamReader(stream))
                {
                    scripts.Add(reader.ReadToEnd());
                }
            }

            return scripts;
        }

        public List<T> ConvertResult<T>(
            List<PSObject> objects,
            Func<PSObject, T> func)
        {
            var result = new List<T>();
            foreach (var psObject in objects)
            {
                result.Add(func.Invoke(psObject));
            }
            return result;
        }
    }
}
