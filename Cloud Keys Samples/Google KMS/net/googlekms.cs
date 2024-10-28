/*
 * Cloud Keys 2024 .NET Edition - Sample Project
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

﻿using System;
using nsoftware.CloudKeys;

class googlekmsDemo
{
  private static GoogleKMS googlekms = new nsoftware.CloudKeys.GoogleKMS();

  static void Main(string[] args)
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

      googlekms.Config("OAuthBrowserResponseTimeout=60");
      googlekms.Authorize();

      Console.Write("Enter your Google Cloud project ID: ");
      googlekms.GoogleProjectId = Console.ReadLine();

      Console.Write("Enter a key ring name (if the key ring does not exist, it will be created): ");
      googlekms.KeyRing = Console.ReadLine();
      googlekms.ListKeyRings();

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
        googlekms.CreateKeyRing();
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
          googlekms.ListKeys();
        }
        else if (arguments[0] == "create")
        {
          if (arguments.Length > 3)
          {
            googlekms.CreateKey(arguments[1], int.Parse(arguments[2]), arguments[3], false);
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

            googlekms.Encrypt(arguments[1], "");
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

            googlekms.ListVersions(arguments[1]);

            googlekms.Sign(arguments[1], googlekms.Versions[0].VersionId, arguments[2], false);
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

  private static void googlekms_OnKeyList(object sender, GoogleKMSKeyListEventArgs e)
  {
    Console.WriteLine("   " + e.Name);
  }

  private static void googlekms_OnError(object sender, GoogleKMSErrorEventArgs e)
  {
    Console.WriteLine("Error [" + e.ErrorCode + "]: " + e.Description);
  }
}




class ConsoleDemo
{
  /// <summary>
  /// Takes a list of switch arguments or name-value arguments and turns it into a dictionary.
  /// </summary>
  public static System.Collections.Generic.Dictionary<string, string> ParseArgs(string[] args)
  {
    System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>();

    for (int i = 0; i < args.Length; i++)
    {
      // Add a key to the dictionary for each argument.
      if (args[i].StartsWith("/"))
      {
        // If the next argument does NOT start with a "/", then it is a value.
        if (i + 1 < args.Length && !args[i + 1].StartsWith("/"))
        {
          // Save the value and skip the next entry in the list of arguments.
          dict.Add(args[i].ToLower().TrimStart('/'), args[i + 1]);
          i++;
        }
        else
        {
          // If the next argument starts with a "/", then we assume the current one is a switch.
          dict.Add(args[i].ToLower().TrimStart('/'), "");
        }
      }
      else
      {
        // If the argument does not start with a "/", store the argument based on the index.
        dict.Add(i.ToString(), args[i].ToLower());
      }
    }
    return dict;
  }
  /// <summary>
  /// Asks for user input interactively and returns the string response.
  /// </summary>
  public static string Prompt(string prompt, string defaultVal)
  {
    Console.Write(prompt + (defaultVal.Length > 0 ? " [" + defaultVal + "]": "") + ": ");
    string val = Console.ReadLine();
    if (val.Length == 0) val = defaultVal;
    return val;
  }
}