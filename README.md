# ClickOnce.RuntimeVersion

Example usage:

```

  using ClickOnce.RuntimeVersion;

  // initialise with the main app assembly name using reflection
  ClickOnceDeployment cod = new ClickOnceDeployment(Assembly.GetExecutingAssembly().GetName().Name);

  // Check if we are network deployed
  if (cod.IsNetworkDeployed())
  {
      // Get the version as a string in the form 1.0.0.0
      string VersionString = cod.GetVersionString();

    // Get the version as separate
    // Major, Minor, Build, Revision
    ClickOnceVersion AppVersion = cod.GetVersion();
  }
  else
  {
      // we are not network deployed
  }
  ```



    public class ClickOnceVersion
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public int Revision { get; set; }
        public string ManifestPath { get; set; }
    }

