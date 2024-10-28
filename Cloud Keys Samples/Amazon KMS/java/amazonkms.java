/*
 * Cloud Keys 2024 Java Edition - Sample Project
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
 */

import java.io.*;
import cloudkeys.*;


public class amazonkms extends ConsoleDemo{

	static String command; // user's command
	static String buffer; // text buffer
	static String[] argument; // arguments following command

	static String keySpec;
	static String keyId;
	static String type;

	public static void main(String[] args) {

		AmazonKMS kms = new AmazonKMS();
		try {
			kms.addAmazonKMSEventListener(new DefaultAmazonKMSEventListener(){

				@Override
				public void error(AmazonKMSErrorEvent e) {
					System.out.println("Error [" + e.errorCode + "]: " + e.description);
				}

				@Override
				public void keyList(AmazonKMSKeyListEvent e) {
					System.out.println("   " + e.ARN);
				}
			});

			kms.setAccessKey(prompt("Access key",":",""));
			kms.setSecretKey(prompt("Secret key",":",""));

			while (true) {
				DisplayMenu();
				System.out.print("> ");
				command = input();
				argument = command.split("\\s");
				if (argument.length == 0)
					continue;
				switch (argument[0].charAt(0)) {
					case 'l':
						System.out.println("Keys: ");
						kms.listKeys();
						System.out.println();
						break;
					case 'c':
						keySpec = prompt("Key Spec", ":", "RSA_2048");
						type = prompt("For Signing", ":", "y");
						if (type == "y" || type =="Y") {
							keyId = kms.createKey(keySpec, true, "sign, verify");
						}
						else{
							keyId = kms.createKey(keySpec, false, "encrypt, decrypt");
						}
						System.out.println("Key Created Successfully");
						System.out.println("New key id: "+keyId);
						break;
					case 'd':
						String keyId = prompt("Key Id", ":", "");
						String days = prompt("Days to wait",":","7");
						kms.scheduleKeyDeletion(keyId,Integer.parseInt(days));
						System.out.println("Key Scheduled for Deletion");
						break;
					case 'e':
						keyId = prompt("Key Id", ":", "");
						String algorithm = prompt("Encryption Algorithm", ":", "SYMMETRIC_DEFAULT");
						kms.setInputData(prompt("String to Encrypt", ":", "This is an Example!"));
						kms.encrypt(keyId, algorithm);
						System.out.println("Encrypted Data: ");
						System.out.println(new String(kms.getOutputData()));
						break;
					case 's':
						keyId = prompt("Key Id", ":", "");
						algorithm = prompt("Signature Algorithm", ":", "PS256");
						kms.setInputData(prompt("String to Sign", ":", "This is an Example!"));
						kms.sign(keyId, algorithm, false);
						System.out.println("Signature Data: ");
						System.out.println(new String(kms.getOutputData()));
						break;
					case '?':
						DisplayMenu();
						break;
					case 'q':
						return;
					default:
						System.out.println("Command Not Recognized.");
				}
			}

		} catch (Exception ex) {
			System.out.println(ex.getMessage());
		}
	}

	private static void DisplayMenu() {
		System.out.println("\nAmazonKMS Commands:");
		System.out.println("l            List Keys");
		System.out.println("c            Create a Key");
		System.out.println("d            Delete a Key");
		System.out.println("e            Encrypts a String");
		System.out.println("s            Signs a String");
		System.out.println("?            Display Options");
		System.out.println("q            Quit");
	}
}
class ConsoleDemo {
  private static BufferedReader bf = new BufferedReader(new InputStreamReader(System.in));

  static String input() {
    try {
      return bf.readLine();
    } catch (IOException ioe) {
      return "";
    }
  }
  static char read() {
    return input().charAt(0);
  }

  static String prompt(String label) {
    return prompt(label, ":");
  }
  static String prompt(String label, String punctuation) {
    System.out.print(label + punctuation + " ");
    return input();
  }
  static String prompt(String label, String punctuation, String defaultVal) {
      System.out.print(label + " [" + defaultVal + "]" + punctuation + " ");
      String response = input();
      if (response.equals(""))
        return defaultVal;
      else
        return response;
  }

  static char ask(String label) {
    return ask(label, "?");
  }
  static char ask(String label, String punctuation) {
    return ask(label, punctuation, "(y/n)");
  }
  static char ask(String label, String punctuation, String answers) {
    System.out.print(label + punctuation + " " + answers + " ");
    return Character.toLowerCase(read());
  }

  static void displayError(Exception e) {
    System.out.print("Error");
    if (e instanceof CloudKeysException) {
      System.out.print(" (" + ((CloudKeysException) e).getCode() + ")");
    }
    System.out.println(": " + e.getMessage());
    e.printStackTrace();
  }

  /**
   * Takes a list of switch arguments or name-value arguments and turns it into a map.
   */
  static java.util.Map<String, String> parseArgs(String[] args) {
    java.util.Map<String, String> map = new java.util.HashMap<String, String>();
    
    for (int i = 0; i < args.length; i++) {
      // Add a key to the map for each argument.
      if (args[i].startsWith("-")) {
        // If the next argument does NOT start with a "-" then it is a value.
        if (i + 1 < args.length && !args[i + 1].startsWith("-")) {
          // Save the value and skip the next entry in the list of arguments.
          map.put(args[i].toLowerCase().replaceFirst("^-+", ""), args[i + 1]);
          i++;
        } else {
          // If the next argument starts with a "-", then we assume the current one is a switch.
          map.put(args[i].toLowerCase().replaceFirst("^-+", ""), "");
        }
      } else {
        // If the argument does not start with a "-", store the argument based on the index.
        map.put(Integer.toString(i), args[i].toLowerCase());
      }
    }
    return map;
  }
}



