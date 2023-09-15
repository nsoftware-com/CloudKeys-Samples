/*
 * Cloud Keys 2022 .NET Edition - Sample Project
 *
 * This sample project demonstrates the usage of Cloud Keys in a 
 * simple, straightforward way. It is not intended to be a complete 
 * application. Error handling and other checks are simplified for clarity.
 *
 * www.nsoftware.com/cloudkeys
 *
 * This code is subject to the terms and conditions specified in the 
 * corresponding product license agreement which outlines the authorized 
 * usage and restrictions.
 * 
 */

using System.Collections.Generic;
﻿using System;
using System.Threading.Tasks;
using nsoftware.async.CloudKeys;

class azuresecretsDemo
{
  private static Azuresecrets azuresecrets = new nsoftware.async.CloudKeys.Azuresecrets();

  static async Task Main(string[] args)
  {
    try
    {
      azuresecrets.OnSecretList += azuresecrets_OnSecretList;
      azuresecrets.OnError += azuresecrets_OnError;

      // Prompt for authentication information.
      Console.Write("Enter your OAuth client ID: "); // Can use ce5c0f06-1f2b-4f98-8abf-73f8aaa2592c for testing purposes.
      azuresecrets.OAuth.ClientId = Console.ReadLine();
      Console.Write("Enter your OAuth client secret: "); // Can use 3KqXE.3tm~0-1A~~V6AjSA1Y8a1FI.Fgec for testing purposes.
      azuresecrets.OAuth.ClientSecret = Console.ReadLine();
      Console.Write("Enter your Azure tenant ID: ");
      string tenantID = Console.ReadLine();

      azuresecrets.OAuth.ServerAuthURL = "https://login.microsoftonline.com/" + tenantID + "/oauth2/v2.0/authorize";
      azuresecrets.OAuth.ServerTokenURL = "https://login.microsoftonline.com/" + tenantID + "/oauth2/v2.0/token";
      azuresecrets.OAuth.AuthorizationScope = "offline_access https://vault.azure.net/user_impersonation";

      Console.Write("Enter a redirect URI port: ");
      await azuresecrets.Config("OAuthWebServerPort=" + Console.ReadLine());
      await azuresecrets.Config("OAuthBrowserResponseTimeout=60");
      await azuresecrets.Authorize();

      Console.Write("Enter the Azure Key vault to select: ");
      azuresecrets.Vault = Console.ReadLine();

      // Process user commands.
      Console.WriteLine("Type \"?\" or \"help\" for a list of commands.");
      Console.Write("azuresecrets> ");
      string command;
      string[] arguments;

      while (true)
      {
        command = Console.ReadLine();
        arguments = command.Split();

        if (arguments[0] == "?" || arguments[0] == "help")
        {
          Console.WriteLine("Commands: ");
          Console.WriteLine("  ?                                        display the list of valid commands");
          Console.WriteLine("  help                                     display the list of valid commands");
          Console.WriteLine("  ls                                       list secrets");
          Console.WriteLine("  create <name> <type>                     create a new secret");
          Console.WriteLine("    ex. create testsecret contenttype");
          Console.WriteLine("  del <name>                               delete the secret with the specified name");
          Console.WriteLine("  view <name>                              view a secret's data");
          Console.WriteLine("  quit                                     exit the application");
        }
        else if (arguments[0] == "ls")
        {
          await azuresecrets.ListSecrets();
        }
        else if (arguments[0] == "create")
        {
          if (arguments.Length > 2)
          {
            Console.Write("Enter the secret data: ");
            azuresecrets.SecretData = Console.ReadLine();

            await azuresecrets.CreateSecret(arguments[1], arguments[2]);
            Console.WriteLine("Secret created successfully.");
          }
        }
        else if (arguments[0] == "del")
        {
          if (arguments.Length > 1)
          {
            await azuresecrets.DeleteSecret(arguments[1]);
            Console.WriteLine("Secret deleted successfully.");
          }
        }
        else if (arguments[0] == "view")
        {
          if (arguments.Length > 1)
          {
            await azuresecrets.GetSecret(arguments[1]);
            Console.WriteLine("Secret data: " + azuresecrets.SecretData);
          }
        }
        else if (arguments[0] == "quit")
        {
          break;
        }
        else if (arguments[0] == "")
        {
          // Do nothing.
        }
        else
        {
          Console.WriteLine("Invalid command.");
        } // End of command checking.

        Console.Write("azuresecrets> ");
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
    }
  }

  private static void azuresecrets_OnSecretList(object sender, AzuresecretsSecretListEventArgs e)
  {
    Console.WriteLine("   " + e.Name);
  }

  private static void azuresecrets_OnError(object sender, AzuresecretsErrorEventArgs e)
  {
    Console.WriteLine("Error [" + e.ErrorCode + "]: " + e.Description);
  }
}


class ConsoleDemo
{
  public static Dictionary<string, string> ParseArgs(string[] args)
  {
    Dictionary<string, string> dict = new Dictionary<string, string>();

    for (int i = 0; i < args.Length; i++)
    {
      // If it starts with a "/" check the next argument.
      // If the next argument does NOT start with a "/" then this is paired, and the next argument is the value.
      // Otherwise, the next argument starts with a "/" and the current argument is a switch.

      // If it doesn't start with a "/" then it's not paired and we assume it's a standalone argument.

      if (args[i].StartsWith("/"))
      {
        // Either a paired argument or a switch.
        if (i + 1 < args.Length && !args[i + 1].StartsWith("/"))
        {
          // Paired argument.
          dict.Add(args[i].TrimStart('/'), args[i + 1]);
          // Skip the value in the next iteration.
          i++;
        }
        else
        {
          // Switch, no value.
          dict.Add(args[i].TrimStart('/'), "");
        }
      }
      else
      {
        // Standalone argument. The argument is the value, use the index as a key.
        dict.Add(i.ToString(), args[i]);
      }
    }
    return dict;
  }

  public static string Prompt(string prompt, string defaultVal)
  {
    Console.Write(prompt + (defaultVal.Length > 0 ? " [" + defaultVal + "]": "") + ": ");
    string val = Console.ReadLine();
    if (val.Length == 0) val = defaultVal;
    return val;
  }
}