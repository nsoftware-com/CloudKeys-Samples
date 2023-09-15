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
using System.Text;
using System.Threading.Tasks;
using nsoftware.async.CloudKeys;

class amazonsecretsDemo
{
  private static Amazonsecrets amazonsecrets = new nsoftware.async.CloudKeys.Amazonsecrets();

  static async Task Main(string[] args)
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
          await amazonsecrets.ListSecrets();
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

            await amazonsecrets.CreateSecret(arguments[1], desc);
            Console.WriteLine("Secret created successfully.");
          }
        }
        else if (arguments[0] == "del")
        {
          if (arguments.Length > 1)
          {
            await amazonsecrets.DeleteSecret(arguments[1], 0);
            Console.WriteLine("Secret deleted successfully.");
          }
        }
        else if (arguments[0] == "view")
        {
          if (arguments.Length > 3)
          {
            await amazonsecrets.GetSecret(arguments[1], arguments[2], arguments[3]);
            Console.WriteLine("Secret data: " + Encoding.UTF8.GetString(amazonsecrets.SecretData));
          }
          else if (arguments.Length > 2)
          {
            await amazonsecrets.GetSecret(arguments[1], "", arguments[2]);
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

  private static void amazonsecrets_OnSecretList(object sender, AmazonsecretsSecretListEventArgs e)
  {
    Console.WriteLine("   " + e.Name);
  }

  private static void amazonsecrets_OnError(object sender, AmazonsecretsErrorEventArgs e)
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