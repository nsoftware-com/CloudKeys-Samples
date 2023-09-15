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

class azurekeysDemo
{
  private static Azurekeys azurekeys = new nsoftware.async.CloudKeys.Azurekeys();

  static async Task Main(string[] args)
  {
    try
    {
      azurekeys.OnKeyList += azurekeys_OnKeyList;
      azurekeys.OnError += azurekeys_OnError;

      // Prompt for authentication information.
      Console.Write("Enter your OAuth client ID: "); // Can use ce5c0f06-1f2b-4f98-8abf-73f8aaa2592c for testing purposes.
      azurekeys.OAuth.ClientId = Console.ReadLine();
      Console.Write("Enter your OAuth client secret: "); // Can use 3KqXE.3tm~0-1A~~V6AjSA1Y8a1FI.Fgec for testing purposes.
      azurekeys.OAuth.ClientSecret = Console.ReadLine();
      Console.Write("Enter your Azure tenant ID: ");
      string tenantID = Console.ReadLine();

      azurekeys.OAuth.ServerAuthURL = "https://login.microsoftonline.com/" + tenantID + "/oauth2/v2.0/authorize";
      azurekeys.OAuth.ServerTokenURL = "https://login.microsoftonline.com/" + tenantID + "/oauth2/v2.0/token";
      azurekeys.OAuth.AuthorizationScope = "offline_access https://vault.azure.net/user_impersonation";

      Console.Write("Enter a redirect URI port: ");
      await azurekeys.Config("OAuthWebServerPort=" + Console.ReadLine());
      await azurekeys.Config("OAuthBrowserResponseTimeout=60");
      await azurekeys.Authorize();

      Console.Write("Enter the Azure Key vault to select: ");
      azurekeys.Vault = Console.ReadLine();

      // Process user commands.
      Console.WriteLine("Type \"?\" or \"help\" for a list of commands.");
      Console.Write("azurekeys> ");
      string command;
      string[] arguments;

      while (true)
      {
        command = Console.ReadLine();
        arguments = command.Split();

        if (arguments[0] == "?" || arguments[0] == "help")
        {
          Console.WriteLine("Commands: ");
          Console.WriteLine("  ?                                 display the list of valid commands");
          Console.WriteLine("  help                              display the list of valid commands");
          Console.WriteLine("  ls                                list keys");
          Console.WriteLine("  create <name> <type>              create a new key");
          Console.WriteLine("    ex. create testkey1 EC_P256");
          Console.WriteLine("    ex. create testkey2 RSA_2048");
          Console.WriteLine("  del <name>                        delete the key with the specified name");
          Console.WriteLine("  encrypt <name> <alg> <string>     encrypt a string");
          Console.WriteLine("    ex. encrypt testkey2 RSA1_5 test");
          Console.WriteLine("  sign <name> <alg> <string>        sign a string");
          Console.WriteLine("    ex. sign testkey1 ES256 test");
          Console.WriteLine("  quit                              exit the application");
        }
        else if (arguments[0] == "ls")
        {
          await azurekeys.ListKeys();
        }
        else if (arguments[0] == "create")
        {
          if (arguments.Length > 2)
          {
            if (arguments[2].ToLower().StartsWith("ec"))
            {
              await azurekeys.CreateKey(arguments[1], arguments[2], "sign, verify");
            }
            else
            {
              await azurekeys.CreateKey(arguments[1], arguments[2], "encrypt, decrypt");
            }
            Console.WriteLine("Key created successfully.");
          }
        }
        else if (arguments[0] == "del")
        {
          if (arguments.Length > 1)
          {
            await azurekeys.DeleteKey(arguments[1]);
            Console.WriteLine("Key deleted successfully.");
          }
        }
        else if (arguments[0] == "encrypt")
        {
          if (arguments.Length > 3)
          {
            string text = "";
            for (int i = 3; i < arguments.Length; i++)
            {
              text += arguments[i] + " ";
            }
            azurekeys.InputData = text;

            await azurekeys.Encrypt(arguments[1], arguments[2]);
            Console.WriteLine("Encrypted data: ");
            Console.WriteLine(azurekeys.OutputData);
          }
        }
        else if (arguments[0] == "sign")
        {
          if (arguments.Length > 3)
          {
            string text = "";
            for (int i = 3; i < arguments.Length; i++)
            {
              text += arguments[i] + " ";
            }
            azurekeys.InputData = text;

            await azurekeys.Sign(arguments[1], arguments[2], false);
            Console.WriteLine("Signature data: ");
            Console.WriteLine(azurekeys.OutputData);
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

        Console.Write("azurekeys> ");
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
    }
  }

  private static void azurekeys_OnKeyList(object sender, AzurekeysKeyListEventArgs e)
  {
    Console.WriteLine("   " + e.Name);
  }

  private static void azurekeys_OnError(object sender, AzurekeysErrorEventArgs e)
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