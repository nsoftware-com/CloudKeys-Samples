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


public class azuresecrets extends ConsoleDemo{

	static String command; // user's command
	static String buffer; // text buffer
	static String[] argument; // arguments following command

	static String secretName;
	static String secretType;
	static String data;

	public static void main(String[] args) {

		AzureSecrets azuresecrets = new AzureSecrets();
		try {
			azuresecrets.addAzureSecretsEventListener(new DefaultAzureSecretsEventListener(){

				@Override
				public void error(AzureSecretsErrorEvent e) {
					System.out.println("Error [" + e.errorCode + "]: " + e.description);
				}

				@Override
				public void secretList(AzureSecretsSecretListEvent e) {
					System.out.println("   " + e.name);
				}
			});

			System.out.println("Press Enter to Authenticate.");
			input();

			azuresecrets.getOAuth().setClientId("ce5c0f06-1f2b-4f98-8abf-73f8aaa2592c");
			azuresecrets.getOAuth().setClientSecret("3KqXE.3tm~0-1A~~V6AjSA1Y8a1FI.Fgec");
			azuresecrets.getOAuth().setAuthorizationScope("offline_access https://vault.azure.net/user_impersonation");
			azuresecrets.getOAuth().setServerAuthURL("https://login.microsoftonline.com/common/oauth2/v2.0/authorize");
			azuresecrets.getOAuth().setServerTokenURL("https://login.microsoftonline.com/common/oauth2/v2.0/token");
			azuresecrets.config("OAuthWebServerPort=7777");

			azuresecrets.authorize();
			System.out.println("Authentication Successful.");

			azuresecrets.setVault(prompt("AzureSecrets Vault Name"));

			while (true) {
				DisplayMenu();
				System.out.print("> ");
				command = input();
				argument = command.split("\\s");
				if (argument.length == 0)
					continue;
				switch (argument[0].charAt(0)) {
					case 'l':
						System.out.println("Secrets: ");
						azuresecrets.listSecrets();
						System.out.println();
						break;
					case 'c':
						secretName = prompt("Secret Name", ":", "TestSecret");
						secretType = prompt("Secret Type", ":", "Test");
						data = prompt("Secret Data", ":", "Test");

						azuresecrets.setSecretData(data);
						azuresecrets.createSecret(secretName, secretType);

						System.out.println("Secret Created Successfully");
						break;
					case 'd':
						secretName = prompt("Secret Name", ":", "TestSecret");
						azuresecrets.deleteSecret(secretName);
						System.out.println("Secret Deleted Successfully");
						break;
					case 'v':
						secretName = prompt("Secret Name", ":", "TestSecret");
						azuresecrets.getSecret(secretName);

						System.out.println("Secret Data: " + new String(azuresecrets.getSecretData()));
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
			System.out.println("\nAzureSecrets Commands:");
			System.out.println("l            List Secrets");
			System.out.println("c            Create a Secret");
			System.out.println("d            Delete a Secret");
			System.out.println("v            View a Secret's Data");
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



