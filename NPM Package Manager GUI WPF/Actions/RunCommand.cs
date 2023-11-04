using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPM_Package_Manager_GUI_WPF.Actions
{
    public static class RunCommand
    {
        public static void RunInstall(string Path, bool UseYarn, bool IsDebug)
        {
            //Environment.CurrentDirectory = Path;

            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = UseYarn ? "yarn" : "npm",
                    Arguments = UseYarn ? "" : "install",
                    CreateNoWindow = !IsDebug,
                    WindowStyle = IsDebug ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden,
                    WorkingDirectory = Path,
                    UseShellExecute = true
                }
            };

            process.Start();
            process.WaitForExit();
        }

        public static void RunInstallSpecific(string Path, bool UseYarn, bool IsDebug, string PackageName)
        {
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = UseYarn ? "yarn" : "npm",
                    Arguments = UseYarn ? $"add {PackageName}" : $"install {PackageName}",
                    CreateNoWindow = !IsDebug,
                    WindowStyle = IsDebug ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden,
                    WorkingDirectory = Path,
                    UseShellExecute = true
                }
            };

            process.Start();
            process.WaitForExit();
        }

        public static void RunRemoveSpecific(string Path, bool UseYarn, bool IsDebug, string PackageName)
        {
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = UseYarn ? "yarn" : "npm",
                    Arguments = UseYarn ? $"remove {PackageName}" : $"uninstall {PackageName}",
                    CreateNoWindow = !IsDebug,
                    WindowStyle = IsDebug ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden,
                    WorkingDirectory = Path,
                    UseShellExecute = true
                }
            };

            process.Start();
            process.WaitForExit();
        }
    }
}
