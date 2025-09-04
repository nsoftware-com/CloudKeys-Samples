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

class googlesecretsDemo
{
  private static GoogleSecrets googlesecrets = new nsoftware.CloudKeys.GoogleSecrets();

  static void Main(string[] args)
  {
    try
    {
      googlesecrets.OnSecretList += googlesecrets_OnSecretList;
      googlesecrets.OnError += googlesecrets_OnError;

      // Prompt for authentication information.
      Console.Write("Enter your OAuth client ID: "); // Can use 157623334268.apps.googleusercontent.com for testing purposes.
      googlesecrets.OAuth.ClientId = Console.ReadLine();
      Console.Write("Enter your OAuth client secret: "); // Can use v8R8R90hn_LchsVc0Ta6Sy0D for testing purposes.
      googlesecrets.OAuth.ClientSecret = Console.ReadLine();

      googlesecrets.OAuth.ServerAuthURL = "https://accounts.google.com/o/oauth2/auth";
      googlesecrets.OAuth.ServerTokenURL = "https://accounts.google.com/o/oauth2/token";
      googlesecrets.OAuth.AuthorizationScope = "https://www.googleapis.com/auth/cloud-platform";

      googlesecrets.Config("OAuthBrowserResponseTimeout=60");
      googlesecrets.Authorize();

      Console.Write("Enter your Google Cloud project ID: ");
      googlesecrets.GoogleProjectId = Console.ReadLine();

      // Process user commands.
      Console.WriteLine("Type \"?\" or \"help\" for a list of commands.");
      Console.Write("googlesecrets> ");
      string command;
      string[] arguments;

      while (true)
      {
        command = Console.ReadLine();
        arguments = command.Split();

        if (arguments[0] == "?" || arguments[0] == "help")
        {
          Console.WriteLine("Commands: ");
          Console.WriteLine("  ?                               display the list of valid commands");
          Console.WriteLine("  help                            display the list of valid commands");
          Console.WriteLine("  ls                              list secrets");
          Console.WriteLine("  create <name>                   create a new secret with the specified name");
          Console.WriteLine("  del <name>                      delete the secret with the specified name");
          Console.WriteLine("  view <name>                     view a secret's data");
          Console.WriteLine("  quit                            exit the application");
        }
        else if (arguments[0] == "ls")
        {
          googlesecrets.ListSecrets();
        }
        else if (arguments[0] == "create")
        {
          if (arguments.Length > 1)
          {
            Console.Write("Enter the secret data: ");
            googlesecrets.SecretData = Console.ReadLine();

            googlesecrets.CreateSecret(arguments[1]);
            Console.WriteLine("Secret created successfully.");
          }
        }
        else if (arguments[0] == "del")
        {
          if (arguments.Length > 1)
          {
            googlesecrets.DeleteSecret(arguments[1]);
            Console.WriteLine("Secret deleted successfully.");
          }
        }
        else if (arguments[0] == "view")
        {
          if (arguments.Length > 1)
          {
            googlesecrets.GetSecret(arguments[1], "");
            Console.WriteLine("Secret data: " + googlesecrets.SecretData);
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

        Console.Write("googlesecrets> ");
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
    }
  }

  private static void googlesecrets_OnSecretList(object sender, GoogleSecretsSecretListEventArgs e)
  {
    Console.WriteLine("   " + e.Name);
  }

  private static void googlesecrets_OnError(object sender, GoogleSecretsErrorEventArgs e)
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