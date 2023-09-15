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

class googlekmsDemo
{
  private static Googlekms googlekms = new nsoftware.async.CloudKeys.Googlekms();

  static async Task Main(string[] args)
  {
    try
    {
      googlekms.OnKeyList += googlekms_OnKeyList;
      googlekms.OnError += googlekms_OnError;

      // Prompt for authentication information.
      Console.Write("Enter your OAuth client ID: "); // Can use 157623334268.apps.googleusercontent.com for testing purposes.
      googlekms.OAuth.ClientId = Console.ReadLine();
      Console.Write("Enter your OAuth client secret: "); // Can use v8R8R90hn_LchsVc0Ta6Sy0D for testing purposes.
      googlekms.OAuth.ClientSecret = Console.ReadLine();

      googlekms.OAuth.ServerAuthURL = "https://accounts.google.com/o/oauth2/auth";
      googlekms.OAuth.ServerTokenURL = "https://accounts.google.com/o/oauth2/token";
      googlekms.OAuth.AuthorizationScope = "https://www.googleapis.com/auth/cloud-platform";

      await googlekms.Config("OAuthBrowserResponseTimeout=60");
      await googlekms.Authorize();

      Console.Write("Enter your Google Cloud project ID: ");
      googlekms.GoogleProjectId = Console.ReadLine();

      Console.Write("Enter a key ring name (if the key ring does not exist, it will be created): ");
      googlekms.KeyRing = Console.ReadLine();
      await googlekms.ListKeyRings();

      bool exists = false;
      foreach (GoogleKeyRing keyring in googlekms.KeyRings)
      {
        if (googlekms.KeyRing == keyring.Name)
        {
          Console.WriteLine("Key ring found.");
          exists = true;
        }
      }

      if (!exists)
      {
        Console.WriteLine("Creating a new key ring...");
        await googlekms.CreateKeyRing();
      }

      // Process user commands.
      Console.WriteLine("Type \"?\" or \"help\" for a list of commands.");
      Console.Write("googlekms> ");
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
          Console.WriteLine("  create <name> <purpose> <alg>     create a new key");
          Console.WriteLine("    ex. create testkey1 1 GOOGLE_SYMMETRIC_ENCRYPTION");
          Console.WriteLine("    ex. create testkey2 2 EC_SIGN_P256_SHA256");
          Console.WriteLine("    ex. create testkey3 3 RSA_DECRYPT_OAEP_2048_SHA256");
          Console.WriteLine("  encrypt <name> <string>           encrypt a string");
          Console.WriteLine("    ex. encrypt testkey1 test");
          Console.WriteLine("  sign <name> <alg> <string>        sign a string");
          Console.WriteLine("    ex. sign testkey2 SHA256 test");
          Console.WriteLine("  quit                              exit the application");
        }
        else if (arguments[0] == "ls")
        {
          await googlekms.ListKeys();
        }
        else if (arguments[0] == "create")
        {
          if (arguments.Length > 3)
          {
            await googlekms.CreateKey(arguments[1], int.Parse(arguments[2]), arguments[3], false);
            Console.WriteLine("Key created successfully.");
          }
          else
          {
            Console.WriteLine("Valid purpose values:");
            Console.WriteLine("  1: A symmetric key used for encryption and decryption.");
            Console.WriteLine("  2: An asymmetric key used for signing and verification.");
            Console.WriteLine("  3: An asymmetric key used for encryption and decryption.");
          }
        }
        else if (arguments[0] == "encrypt")
        {
          if (arguments.Length > 2)
          {
            string text = "";
            for (int i = 2; i < arguments.Length; i++)
            {
              text += arguments[i] + " ";
            }
            googlekms.InputData = text;

            await googlekms.Encrypt(arguments[1], "");
            Console.WriteLine("Encrypted data: ");
            Console.WriteLine(googlekms.OutputData);
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
            googlekms.InputData = text;

            await googlekms.ListVersions(arguments[1]);

            await googlekms.Sign(arguments[1], googlekms.Versions[0].VersionId, arguments[2], false);
            Console.WriteLine("Signature data: ");
            Console.WriteLine(googlekms.OutputData);
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

        Console.Write("googlekms> ");
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
    }
  }

  private static void googlekms_OnKeyList(object sender, GooglekmsKeyListEventArgs e)
  {
    Console.WriteLine("   " + e.Name);
  }

  private static void googlekms_OnError(object sender, GooglekmsErrorEventArgs e)
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