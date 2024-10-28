/*
 * Cloud Keys 2024 C++ Edition - Sample Project
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

#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include "../../include/cloudkeys.h"

#define LINE_LEN 80

class MyAzureKeys : public AzureKeys {
public:
	virtual int FireKeyList(AzureKeysKeyListEventParams* e) {
		printf("  %s\n", e->Name);
		return 0;
	}
};

void prompt(char* command, char* punctuation, char* default_str, char(&ret)[LINE_LEN]) {
	printf("%s [%s] %s ", command, default_str, punctuation);
	fgets(ret, LINE_LEN, stdin);
	if (strlen(ret) == 1) {
		strcpy(ret, default_str);
		return;
	}
	ret[strlen(ret) - 1] = '\0';
}

int main(int argc, char* argv[])
{

	if (argc < 2) {

		fprintf(stderr, "usage: azurekeys vault\n");
		fprintf(stderr, "\n");
		fprintf(stderr, "  vault     the vault name of the vault to connect to\n");
		printf("\nPress enter to continue.\n");
		getchar();

	}
	else{
		MyAzureKeys azurekeys; 
		char command[LINE_LEN];     // user's command
		char *argument;             // arguments to the user's command
		char keyname[LINE_LEN], keytype[LINE_LEN], algorithm[LINE_LEN];
		char *output, input[LINE_LEN];
		int output_length;

		printf("Press Enter to Authenticate.");
		getchar();

		azurekeys.SetOAuthClientId("ce5c0f06-1f2b-4f98-8abf-73f8aaa2592c");
		azurekeys.SetOAuthClientSecret("3KqXE.3tm~0-1A~~V6AjSA1Y8a1FI.Fgec");
		azurekeys.SetOAuthAuthorizationScope("offline_access https://vault.azure.net/user_impersonation");
		azurekeys.SetOAuthServerAuthURL("https://login.microsoftonline.com/common/oauth2/v2.0/authorize");
		azurekeys.SetOAuthServerTokenURL("https://login.microsoftonline.com/common/oauth2/v2.0/token");
		azurekeys.Config("OAuthWebServerPort=7777");

		azurekeys.Authorize();
		azurekeys.SetVault(argv[1]);

		int ret_code = azurekeys.GetLastErrorCode();
		if (ret_code) goto done;

		printf("Authentication Successful.\n");

		while (1)
		{
			printf("\nAzureKeys Commands:\n");
			printf("l            List Keys\n");
			printf("c            Create a Key\n");
			printf("d            Delete a Key\n");
			printf("e            Encrypts a String\n");
			printf("s            Signs a String\n");
			printf("?            Display Options\n");
			printf("q            Quit\n");

			printf("> ");
			fgets(command, LINE_LEN, stdin);
			command[strlen(command) - 1] = '\0';
			argument = strtok(command, " \t\n");

			if (!strcmp(command, "?"))
			{
				printf("\nAzureKeys Commands:\n");
				printf("l            List Keys\n");
				printf("c            Create a Key\n");
				printf("d            Delete a Key\n");
				printf("e            Encrypts a String\n");
				printf("s            Signs a String\n");
				printf("?            Display Options\n");
				printf("q            Quit\n");

				printf("> ");
			}

			else if (!strcmp(command, "l"))
			{
				printf("Keys: \n");
				azurekeys.ListKeys();
				printf("\n");
			}

			else if (!strcmp(command, "c"))
			{
				prompt("Key Name", ":", "TestKey", keyname);
				prompt("Key Type", ":", "RSA_2048", keytype);
				if (keyname[0] == 'E' && keyname[1] == 'C') {
					azurekeys.CreateKey(keyname, keytype, "sign, verify");
				}
				else {
					azurekeys.CreateKey(keyname, keytype, "encrypt, decrypt, sign, verify");
				}

				ret_code = azurekeys.GetLastErrorCode();
				if (ret_code) goto done;

				printf("\nKey Created Successfully\n");
			}

			else if (!strcmp(command, "d"))
			{
				prompt("Key Name", ":", "TestKey", keyname);
				azurekeys.DeleteKey(keyname);

				ret_code = azurekeys.GetLastErrorCode();
				if (ret_code) goto done;

				printf("\nKey Deleted Successfully\n");
			}

			else if (!strcmp(command, "e"))
			{
				prompt("Key Name", ":", "TestKey", keyname);
				prompt("Encryption Algorithm", ":", "RSA-OAEP", algorithm);
				prompt("String to Encrypt", ":", "This is an Example!", input);
				azurekeys.SetInputData(input, sizeof(input));
				azurekeys.Encrypt(keyname, algorithm);

				printf("Encrypted Data: ");
				azurekeys.GetOutputData(output, output_length);
				printf(output);
			}

			else if (!strcmp(command, "s"))
			{
				prompt("Key Name", ":", "TestKey", keyname);
				prompt("Signature Algorithm", ":", "PS256", algorithm);
				prompt("String to Sign", ":", "This is an Example!", input);
				azurekeys.SetInputData(input, sizeof(input));
				azurekeys.Sign(keyname, algorithm, false);
				printf("Signature Data: ");
				azurekeys.GetOutputData(output, output_length);
				printf(output);
			}

			else if (!strcmp(command, "q")) 
			{
				goto done;
			}

			else
			{
				printf("Command Not Recognized.\n");
			}

		  ret_code = azurekeys.GetLastErrorCode();
			if (ret_code) goto done;

		}  // end of main while loop

	done:
		if (ret_code)     // Got an error.  The user is done.
		{
			printf("\nError: %d", ret_code);
			if (azurekeys.GetLastError())
			{
				printf(" \"%s\"\n", azurekeys.GetLastError());
			}
		}
		fprintf(stderr, "\npress <return> to continue...\n");
		exit(ret_code);
		return 0;
	}
}







