/*
 * Cloud Keys 2022 Java Edition - Sample Project
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


public class azurekeys extends ConsoleDemo{

	static String command; // user's command
	static String buffer; // text buffer
	static String[] argument; // arguments following command

	static String keyName;
	static String keyType;
	static String algorithm;

	public static void main(String[] args) {

		Azurekeys azurekeys = new Azurekeys();
		try {
			azurekeys.addAzurekeysEventListener(new DefaultAzurekeysEventListener(){

				@Override
				public void error(AzurekeysErrorEvent e) {
					System.out.println("Error [" + e.errorCode + "]: " + e.description);
				}

				@Override
				public void keyList(AzurekeysKeyListEvent e) {
					System.out.println("   " + e.name);
				}
			});

			System.out.println("Press Enter to Authenticate.");
			input();

			azurekeys.getOAuth().setClientId("ce5c0f06-1f2b-4f98-8abf-73f8aaa2592c");
			azurekeys.getOAuth().setClientSecret("3KqXE.3tm~0-1A~~V6AjSA1Y8a1FI.Fgec");
			azurekeys.getOAuth().setAuthorizationScope("offline_access https://vault.azure.net/user_impersonation");
			azurekeys.getOAuth().setServerAuthURL("https://login.microsoftonline.com/common/oauth2/v2.0/authorize");
			azurekeys.getOAuth().setServerTokenURL("https://login.microsoftonline.com/common/oauth2/v2.0/token");
			azurekeys.config("OAuthWebServerPort=7777");

			azurekeys.authorize();
			System.out.println("Authentication Successful.");

			azurekeys.setVault(prompt("AzureKey Vault Name"));

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
						azurekeys.listKeys();
						System.out.println();
						break;
					case 'c':
						keyName = prompt("Key Name", ":", "TestKey");
						keyType = prompt("Key Type", ":", "RSA_2048");
						if (keyName.startsWith("EC")){
							azurekeys.createKey(keyName, keyType, "sign, verify");
						}
						else{
							azurekeys.createKey(keyName, keyType, "encrypt, decrypt, sign, verify");
						}
						System.out.println("Key Created Successfully");
						break;
					case 'd':
						keyName = prompt("Key Name", ":", "TestKey");
						azurekeys.deleteKey(keyName);
						System.out.println("Key Deleted Successfully");
						break;
					case 'e':
						keyName = prompt("Key Name", ":", "TestKey");
						algorithm = prompt("Encryption Algorithm", ":", "RSA-OAEP");
						azurekeys.setInputData(prompt("String to Encrypt", ":", "This is an Example!"));
						azurekeys.encrypt(keyName, algorithm);
						System.out.println("Encrypted Data: ");
						System.out.println(new String(azurekeys.getOutputData()));
						break;
					case 's':
						keyName = prompt("Key Name", ":", "TestKey");
						algorithm = prompt("Signature Algorithm", ":", "PS256");
						azurekeys.setInputData(prompt("String to Sign", ":", "This is an Example!"));
						azurekeys.sign(keyName, algorithm, false);
						System.out.println("Signature Data: ");
						System.out.println(new String(azurekeys.getOutputData()));
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
		System.out.println("\nAzureKeys Commands:");
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

  static String prompt(String label, String punctuation, String defaultVal)
  {
	System.out.print(label + " [" + defaultVal + "] " + punctuation + " ");
	String response = input();
	if(response.equals(""))
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
}



