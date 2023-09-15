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

class amazonkmsDemo
{
  private static Amazonkms amazonkms = new nsoftware.async.CloudKeys.Amazonkms();
  private static string keyId = "";

  static async Task Main(string[] args)
  {
    try
    {
      amazonkms.OnKeyList += amazonkms_OnKeyList;
      amazonkms.OnError += amazonkms_OnError;

      // Prompt for authentication information.
      Console.Write("Enter your access key: ");
      amazonkms.AccessKey = Console.ReadLine();
      Console.Write("Enter your secret key: ");
      amazonkms.SecretKey = Console.ReadLine();

      // Process user commands.
      Console.WriteLine("Type \"?\" or \"help\" for a list of commands.");
      Console.Write("amazonkms> ");
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
          Console.WriteLine("  create <type> <forsigning>        create a new key");
          Console.WriteLine("    ex. create RSA_2048 y");
          Console.WriteLine("    ex. create SYMMETRIC_DEFAULT n");
          Console.WriteLine("  del <id> <days>                   schedule the key with the specified id to be deleted in the specified number of days");
          Console.WriteLine("  encrypt <id> <alg> <string>       encrypt a string");
          Console.WriteLine("    ex. encrypt 1234abcd-12ab-34cd-56ef-1234567890ab SYMMETRIC_DEFAULT test");
          Console.WriteLine("  sign <id> <alg> <string>          sign a string");
          Console.WriteLine("    ex. sign 1234abcd-12ab-34cd-56ef-1234567890ab RSASSA_PSS_SHA_256 test");
          Console.WriteLine("  quit                              exit the application");
        }
        else if (arguments[0] == "ls")
        {
          await amazonkms.ListKeys();
        }
        else if (arguments[0] == "create")
        {
          if (arguments.Length > 2)
          {
            bool forSigning = arguments[2].ToLower() == "y";
            keyId = await amazonkms.CreateKey(arguments[1], forSigning, "sign, verify");
            Console.WriteLine("Key created successfully.");
            Console.WriteLine("New key id: " + keyId);
          }
        }
        else if (arguments[0] == "del")
        {
          if (arguments.Length > 2)
          {
            await amazonkms.ScheduleKeyDeletion(arguments[1], int.Parse(arguments[2]));
            Console.WriteLine("Key scheduled for deletion.");
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
            amazonkms.InputData = text;

            await amazonkms.Encrypt(arguments[1], arguments[2]);
            Console.WriteLine("Encrypted data: ");
            Console.WriteLine(amazonkms.OutputData);
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
            amazonkms.InputData = text;

            await amazonkms.Sign(arguments[1], arguments[2], false);
            Console.WriteLine("Signature data: ");
            Console.WriteLine(amazonkms.OutputData);
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

        Console.Write("amazonkms> ");
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
    }
  }

  private static void amazonkms_OnKeyList(object sender, AmazonkmsKeyListEventArgs e)
  {
    Console.WriteLine("   " + e.ARN);
  }

  private static void amazonkms_OnError(object sender, AmazonkmsErrorEventArgs e)
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