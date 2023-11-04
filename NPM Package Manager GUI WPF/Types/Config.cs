using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPM_Package_Manager_GUI_WPF.Types
{
    public class Config
    {
        public PackageJson LastOpenedPackageFile { get; set; } = new PackageJson();
        public string LastOpenedPackagePath { get; set; } = string.Empty;
        public bool AllowDepricatedPackageVersion {  get; set; } = false;
    }
}
