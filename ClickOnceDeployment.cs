using System;
using System.IO;
using System.Reflection;

namespace ClickOnce.RuntimeVersion
{
    public class ClickOnceDeployment
    {
        readonly string Application;
              
        public ClickOnceDeployment(string assemblyName)
        {
            Application = assemblyName;
        }

        /// <summary>
        /// Check to see if the manifest file exists, 
        /// if so, assume we are network deployed
        /// </summary>
        /// <returns>True / False</returns>
        public bool IsNetworkDeployed()
        {
            string ManifestPath = GetManifestPath();

            // And empty path is an error so assume not network deployed
            if (string.IsNullOrEmpty(ManifestPath))
                return false;

            return File.Exists(ManifestPath);
        }

        /// <summary>
        /// Get the version from the manifest and return as a string
        /// </summary>
        /// <returns>The version as a string (or null)</returns>
        public string GetVersionString()
        {
            string ManifestPath = GetManifestPath();
            if (string.IsNullOrEmpty(ManifestPath))
            {
                return null;
            }

            return GetVersionData(ManifestPath);
        }

        /// <summary>
        /// Get the version from the manifest and return as a ClickOnceVersion object
        /// </summary>
        /// <returns>The version as a ClickOnceVersion object</returns>
        public ClickOnceVersion GetVersion()
        {
            ClickOnceVersion cov = new ClickOnceVersion()
            {
                ManifestPath = GetManifestPath()
            };

            if (string.IsNullOrEmpty(cov.ManifestPath) == false)
            {
                string VersionString = GetVersionData(cov.ManifestPath);

                if (string.IsNullOrEmpty(VersionString) == false)
                {
                    var VersionArray = VersionString.Split('.');

                    if (VersionArray.Length > 0)
                    {
                        cov.Major = int.Parse(VersionArray[0]);
                    }

                    if (VersionArray.Length > 1)
                    {
                        cov.Minor = int.Parse(VersionArray[1]);
                    }

                    if (VersionArray.Length > 2)
                    {
                        cov.Build = int.Parse(VersionArray[2]);
                    }

                    if (VersionArray.Length > 3)
                    {
                        cov.Revision = int.Parse(VersionArray[3]);
                    }
                }
            }
            return cov;
        }

        /// <summary>
        /// Get the version data from the manifest file
        /// </summary>
        /// <param name="ManifestPath"></param>
        /// <returns>The version in string format</returns>
        private string GetVersionData(string ManifestPath)
        {
            string VersionNumber = "";
            if (String.IsNullOrEmpty(ManifestPath) == false)
            {
                string Manifest = File.ReadAllText(ManifestPath);
                int AssembyIdentityStart = Manifest.IndexOf("asmv1:assemblyIdentity");
                string VersionText = "version=\"";
                int VersionStart = Manifest.IndexOf(VersionText, AssembyIdentityStart);
                int NumberStart = VersionStart + VersionText.Length;
                int NumberEnd = Manifest.IndexOf("\"", NumberStart);
                VersionNumber = Manifest.Substring(NumberStart, NumberEnd - NumberStart);
            }
            return VersionNumber;
        }

        /// <summary>
        /// Build the manifest file path for a network deployed application
        /// </summary>
        /// <returns>The full file path</returns>
        public string GetManifestPath()
        {
            string BaseDir = AppContext.BaseDirectory;
            string AssemblyName = string.IsNullOrEmpty(Application) ? Assembly.GetExecutingAssembly().GetName().Name : Application;

            if (String.IsNullOrEmpty(AssemblyName) == false)
            {
                string ManifestPath = Path.Combine(BaseDir, $"{AssemblyName}.exe.manifest");
                return ManifestPath;
            }
            else
            {
                return null;
            }
        }
    }
}
