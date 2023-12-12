using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace SteamgameInfo
{    
    public class clsSteam
    {
        private static clsSteam selfInstance = null;
        public static clsSteam getInstance
        {
            get
            {
                if (selfInstance == null) selfInstance = new clsSteam();
                return selfInstance;
            }
        }

        public clsSteam()
        { 
        }

        ~clsSteam()
        {
            steamItem.Clear();
        }

        private List<SteamItem> steamItem = new List<SteamItem>();
        public List<SteamItem> STEAM_ITEM
        {
            get { return steamItem; }           
        }

        public string GetSteamPath(string skey)
        {
            string strValue = "";
            try
            {                
                RegistryKey Reg = Registry.CurrentUser.OpenSubKey("Software\\Valve\\Steam", true);
                if (Reg == null)
                {
                    return "";
                }

                strValue = Reg.GetValue(skey).ToString();
                Reg.Close();
            }
            catch
            {
                return "";
            }
            return strValue;  
        }

        public void KillSteamProcess()
        {
            Process[] processes = Process.GetProcessesByName("Steam");
            foreach (var process in processes)
            {
                process.Kill();
            }
        }
        
        public bool StartSteam(string strUserName, string strPassword)
        {
            KillSteamProcess();

            string strPath = string.Format("{0}\\Steam.exe", GetSteamPath("SteamPath"));
            if (File.Exists(strPath))
            {
                Process.Start(strPath, string.Format("-login {0} {1}", strUserName, strPassword));
                return true;
            }
            return false;
        }

        public bool StopSteam()
        {
            string strPath = string.Format("{0}\\Steam.exe", GetSteamPath("SteamPath"));
            if (File.Exists(strPath))
            {                
                Process.Start(strPath, "-shutdown");
                return true;
            }
            return false;
        }

        private void ReadSteamGameInfo(string strPath)
        {
            string strAppid = "";
            string strInstallDir = "";

            StreamReader reader = new StreamReader(strPath);
            using (reader)
            {  
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line.Contains("appid"))
                        strAppid = line.Replace("appid", "").Replace("\"", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                    else if (line.Contains("installdir"))
                        strInstallDir = line.Replace("installdir", "").Replace("\"", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                 
                    line = reader.ReadLine();
                }
            }

            if (strAppid != "" && strInstallDir != "")
            {
                SteamItem Item = new SteamItem(strAppid, strInstallDir);
                steamItem.Add(Item);
            }
        }

        public bool GetGameInstallList()
        {
            string strPath = string.Format("{0}\\steamapps", GetSteamPath("SteamPath"));

            DirectoryInfo directoryInfo = new DirectoryInfo(strPath);
            if (directoryInfo.Exists == false)
                return false;

            FileInfo[] gameFiles = directoryInfo.GetFiles("*.acf");
            foreach (FileInfo fileinfo in gameFiles)
            {
                ReadSteamGameInfo(fileinfo.FullName);
            }
            return true;
        }
    }
    
    public class SteamItem
    {
        public SteamItem(string appid, string installdir)
        {
            APPID = appid;
            INSTALLDIR = installdir;
        }

        public string APPID { get; set; }
        public string INSTALLDIR { get; set; }
    }
}
