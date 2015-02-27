using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.IO;
using System.Diagnostics;

namespace CustomActions
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult BackupHelper(Session session)
        {            
            //Debugger.Break();
            session.Log("Begin BackupHelper");
            
            try
            {
                string installDir = session["INSTALLDIR"];
                if (Directory.Exists(installDir))
                {
                    //A version is already installed; backup to folder:
                    string filename = installDir + "TestingApplication.exe";
                    if (File.Exists(filename))
                    {
                        var fileVersion = FileVersionInfo.GetVersionInfo(filename).FileVersion;

                        string backupDir = installDir + "\\Previous Versions\\v" + fileVersion + "\\" + DateTime.Now.ToString("dd-MM-yyyy  HHmm");
                        if (!Directory.Exists(backupDir)) { Directory.CreateDirectory(backupDir); }

                        string backupFilename = backupDir + "\\TestingApplication.exe";
                        if (!File.Exists(backupFilename)) { File.Copy(filename, backupFilename); }

                        //Copy all necessary files here

                        //TODO: Copy the Installer into the correct previous version folder, so that you will always have somewhere to install the previous version from
                    }
                }
            }
            catch (Exception ex)
            {
                session.Log("ERROR in BackupHelper: " + ex.Message);
                return ActionResult.Failure; //If the backup does not succeed, the installation/uninstallation will fail. This may or may not be the desired outcome.
            }

            session.Log("Complete BackupHelper");
            return ActionResult.Success;
        }
    }
}
