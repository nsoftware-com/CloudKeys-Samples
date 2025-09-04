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
using System.Text;
using nsoftware.CloudKeys;

class amazonsecretsDemo
{
  private static AmazonSecrets amazonsecrets = new nsoftware.CloudKeys.AmazonSecrets();

  static void Main(string[] args)
  {
    try
    {
      amazonsecrets.OnSecretList += amazonsecrets_OnSecretList;
      amazonsecrets.OnError += amazonsecrets_OnError;

      // Prompt for authentication information.
      Console.Write("Enter your access key: ");
      amazonsecrets.AccessKey = Console.ReadLine();
      Console.Write("Enter your secret key: ");
      amazonsecrets.SecretKey = Console.ReadLine();

      // Process user commands.
      Console.WriteLine("Type \"?\" or \"help\" for a list of commands.");
      Console.Write("amazonsecrets> ");
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
          Console.WriteLine("  create <id> <desc>                       create a new secret");
          Console.WriteLine("    ex. create testsecret a test secret");
          Console.WriteLine("  del <id>                                 delete the secret with the specified id");
          Console.WriteLine("  view <id> [<versionid>] <staginglabel>   view a secret's data");
          Console.WriteLine("    ex. view testsecret a1b2c3d4-5678-90ab-cdef-EXAMPLE11111 AWSCURRENT");
          Console.WriteLine("    ex. view testsecret AWSCURRENT");
          Console.WriteLine("  quit                                     exit the application");
        }
        else if (arguments[0] == "ls")
        {
          amazonsecrets.ListSecrets();
        }
        else if (arguments[0] == "create")
        {
          if (arguments.Length > 2)
          {
            Console.Write("Enter the secret data: ");
            string data = Console.ReadLine();
            amazonsecrets.SecretData = Encoding.UTF8.GetBytes(data);

            string desc = "";
            for (int i = 2; i < arguments.Length; i++)
            {
              desc += arguments[i] + " ";
            }

            amazonsecrets.CreateSecret(arguments[1], desc);
            Console.WriteLine("Secret created successfully.");
          }
        }
        else if (arguments[0] == "del")
        {
          if (arguments.Length > 1)
          {
            amazonsecrets.DeleteSecret(arguments[1], 0);
            Console.WriteLine("Secret deleted successfully.");
          }
        }
        else if (arguments[0] == "view")
        {
          if (arguments.Length > 3)
          {
            amazonsecrets.GetSecret(arguments[1], arguments[2], arguments[3]);
            Console.WriteLine("Secret data: " + Encoding.UTF8.GetString(amazonsecrets.SecretData));
          }
          else if (arguments.Length > 2)
          {
            amazonsecrets.GetSecret(arguments[1], "", arguments[2]);
            Console.WriteLine("Secret data: " + Encoding.UTF8.GetString(amazonsecrets.SecretData));
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

        Console.Write("amazonsecrets> ");
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
    }
  }

  private static void amazonsecrets_OnSecretList(object sender, AmazonSecretsSecretListEventArgs e)
  {
    Console.WriteLine("   " + e.Name);
  }

  private static void amazonsecrets_OnError(object sender, AmazonSecretsErrorEventArgs e)
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