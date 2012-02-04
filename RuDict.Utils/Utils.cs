using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;

namespace RuDict.Utils
{
    public static class Utils
    {
        public static string GetDefaultBrowserPath()
        {
            string key = @"htmlfile\shell\open\command";
            RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(key, false);
            return ((string)registryKey.GetValue(null, null)).Split('"')[1];
        }

        public static void OpenInDefaultBrowser(string url)
        {
            string path = GetDefaultBrowserPath();
            Process.Start(path, url);
        }

        public static string ReplaceHrefBlank(string html)
        {
            return html.Replace("<a href=", "<a target='_blank' href=");
        }

        public static string GetUserAppDataPath()
        {
            string path = string.Empty;
            Assembly assm;
            Type at;
            object[] r;

            // Get the .EXE assembly
            assm = Assembly.GetEntryAssembly();
            // Get a 'Type' of the AssemblyCompanyAttribute
            at = typeof(AssemblyCompanyAttribute);
            // Get a collection of custom attributes from the .EXE assembly
            r = assm.GetCustomAttributes(at, false);
            // Get the Company Attribute
            AssemblyCompanyAttribute ct = ((AssemblyCompanyAttribute)(r[0]));
            // Build the User App Data Path
            path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path += @"\" + ct.Company;
            path += @"\" + assm.GetName().Version.ToString();

            return path;
        }
    }
}
