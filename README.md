azuqua.cs
=========

Azuqua c# library
-----------------

The makefile provided uses the Mono compiler to compile the Azuqua.dll.

```
mcs -target:library -out:Azuqua.dll -r:./Newtonsoft.Json.dll Azuqua/Azuqua.cs
```

The Azuqua.dll has one dependency: the **Newtonsoft.Json Version 6.0.8** package. The Newtonsoft.Json package can be installed via Nuget.

Usage
-----

There's three ways to access the Azuqua API:

	1. Username and Password
	2. Org's Access Key and Access Secret
	3. JSON Config File

### Username and Password

```
using Azuqua = AzuquaCS.Azuqua;

public class MainClass {
	public static void Main (string[] args)
	{
		// grab credentials from the environment variables
		string EMAIL = Environment.GetEnvironmentVariable ("AZUQUA_EMAIL");
		string PASSWORD = Environment.GetEnvironmentVariable ("AZUQUA_PASSWORD");

		// Login with user credentials.
		// Azuqua.Login returns a list of Org objects
		List<Org> orgs = Azuqua.Login (EMAIL, PASSWORD);
		foreach (Org org in orgs) {
			foreach (Flo flo in org.GetFlos()) {
				// Call each flo method.
				Console.WriteLine (flo.Disable ());
				Console.WriteLine (flo.Enable ());
				Console.WriteLine (flo.Read ());
				Console.WriteLine (flo.Invoke ("{\"a\":\"Test Data\"}"));
			}
		}
	}
}

```

### Org's Access Key and Access Secret

```
using Azuqua = AzuquaCS.Azuqua;

public class MainClass {
	public static void Main (string[] args)
	{
		// grab org key, and org secret from the environment variables
		string KEY = Environment.GetEnvironmentVariable ("ACCESS_KEY");
		string SECRET = Environment.GetEnvironmentVariable ("ACCESS_SECRET");

		Org org = new Org(KEY, SECRET);
		foreach (Flo flo in org.GetFlos()) {
			// Call each flo method.
			Console.WriteLine (flo.Disable ());
			Console.WriteLine (flo.Enable ());
			Console.WriteLine (flo.Read ());
			Console.WriteLine (flo.Invoke ("{\"a\":\"Test Data\"}"));
		}
	}
}

```

### JSON Config File

The JSON Config File needs to have the following format:

```
{
	"name": "Name of Org",
	"access_key": "Org Key",
	"access_secret": "Org Secret"
}
```

The code is as follows:

```
using Azuqua = AzuquaCS.Azuqua;

public class MainClass {
	public static void Main (string[] args)
	{
		// Create an org with a config JSON file
		Org orgViaConfigFile = new Org("/tmp/config.json");	
		foreach (Flo flo in org.GetFlos()) {
			// Call each flo method.
			Console.WriteLine (flo.Disable ());
			Console.WriteLine (flo.Enable ());
			Console.WriteLine (flo.Read ());
			Console.WriteLine (flo.Invoke ("{\"a\":\"Test Data\"}"));
		}
	}
}
```
