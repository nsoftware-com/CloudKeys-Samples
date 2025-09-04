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

public class googlesecrets extends ConsoleDemo{

	static String command; // user's command
	static String buffer; // text buffer
	static String[] argument; // arguments following command

	static String secretName;
	static String secretType;
	static String data;

	public static void main(String[] args) {

		GoogleSecrets googlesecrets = new GoogleSecrets();
		try {
			googlesecrets.addGoogleSecretsEventListener(new DefaultGoogleSecretsEventListener(){

				@Override
				public void error(GoogleSecretsErrorEvent e) {
					System.out.println("Error [" + e.errorCode + "]: " + e.description);
				}

				@Override
				public void secretList(GoogleSecretsSecretListEvent e) {
					System.out.println("   " + e.name);
				}
			});

			System.out.println("Press Enter to Authenticate.");
			input();

			googlesecrets.getOAuth().setClientId("157623334268.apps.googleusercontent.com");
			googlesecrets.getOAuth().setClientSecret("v8R8R90hn_LchsVc0Ta6Sy0D");
			googlesecrets.getOAuth().setAuthorizationScope("https://www.googleapis.com/auth/cloud-platform");
			googlesecrets.getOAuth().setServerAuthURL("https://accounts.google.com/o/oauth2/auth");
			googlesecrets.getOAuth().setServerTokenURL("https://accounts.google.com/o/oauth2/token");

			googlesecrets.authorize();
			System.out.println("Authentication Successful.");

			googlesecrets.setGoogleProjectId(prompt("Google Cloud Project ID"));

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
						googlesecrets.listSecrets();
						System.out.println();
						break;
					case 'c':
						secretName = prompt("Secret Name", ":", "TestSecret");
						data = prompt("Secret Data", ":", "Test");

						googlesecrets.setSecretData(data);
						googlesecrets.createSecret(secretName);

						System.out.println("Secret Created Successfully");
						break;
					case 'd':
						secretName = prompt("Secret Name", ":", "TestSecret");
						googlesecrets.deleteSecret(secretName);
						System.out.println("Secret Deleted Successfully");
						break;
					case 'v':
						secretName = prompt("Secret Name", ":", "TestSecret");
						googlesecrets.getSecret(secretName, "");

						System.out.println("Secret Data: " + new String(googlesecrets.getSecretData()));
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
			System.out.println("\nGoogleSecrets Commands:");
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



