using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPM_Package_Manager_GUI_WPF.Actions
{
    public static class CheckProjectState
    {
        public static bool HasAllPackagesInstalled(Types.PackageJson project, string path)
        {
            if(Directory.Exists(path))
            {
                foreach (var package in project.Dependencies)
                {
                    if(!Directory.Exists(path + "\\" + package.Key))
                    {
                        return false;
                    }
                }
            } else
            {
                return false;
            }

            return true;
        }

        public static bool SinglePackageInstalled(string name, string path)
        {
            if (Directory.Exists(path + "\\" + name)) return true;
            else return false;
        }
    }
}
