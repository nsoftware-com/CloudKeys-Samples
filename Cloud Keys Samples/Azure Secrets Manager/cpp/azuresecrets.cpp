/*
 * Cloud Keys 2022 C++ Edition - Sample Project
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

class MyAzureSecrets : public AzureSecrets {
public:
	virtual int FireSecretList(AzureSecretsSecretListEventParams* e) {
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

		fprintf(stderr, "usage: azuresecrets vault\n");
		fprintf(stderr, "\n");
		fprintf(stderr, "  vault     the vault name of the vault to connect to\n");
		printf("\nPress enter to continue.\n");
		getchar();

	}
	else{
		MyAzureSecrets azuresecrets; 
		char command[LINE_LEN];     // user's command
		char *argument;             // arguments to the user's command
		char secretname[LINE_LEN], secrettype[LINE_LEN], data[LINE_LEN];
		char *output, input[LINE_LEN];
		int output_length;

		printf("Press Enter to Authenticate.");
		getchar();

		azuresecrets.SetOAuthClientId("ce5c0f06-1f2b-4f98-8abf-73f8aaa2592c");
		azuresecrets.SetOAuthClientSecret("3KqXE.3tm~0-1A~~V6AjSA1Y8a1FI.Fgec");
		azuresecrets.SetOAuthAuthorizationScope("offline_access https://vault.azure.net/user_impersonation");
		azuresecrets.SetOAuthServerAuthURL("https://login.microsoftonline.com/common/oauth2/v2.0/authorize");
		azuresecrets.SetOAuthServerTokenURL("https://login.microsoftonline.com/common/oauth2/v2.0/token");
		azuresecrets.Config("OAuthWebServerPort=7777");

		azuresecrets.Authorize();
		azuresecrets.SetVault(argv[1]);

		int ret_code = azuresecrets.GetLastErrorCode();
		if (ret_code) goto done;

		printf("Authentication Successful.\n");

		while (1)
		{
			printf("\nAzureSecrets Commands:\n");
			printf("l            List Secrets\n");
			printf("c            Create a Secret\n");
			printf("d            Delete a Secret\n");
			printf("v            View a Secret's Data\n");
			printf("?            Display Options\n");
			printf("q            Quit\n");

			printf("> ");
			fgets(command, LINE_LEN, stdin);
			command[strlen(command) - 1] = '\0';
			argument = strtok(command, " \t\n");

			if (!strcmp(command, "?"))
			{
				printf("\nAzureSecrets Commands:\n");
				printf("l            List Secrets\n");
				printf("c            Create a Secret\n");
				printf("d            Delete a Secret\n");
				printf("v            View a Secret's Data\n");
				printf("?            Display Options\n");
				printf("q            Quit\n");

				printf("> ");
			}

			else if (!strcmp(command, "l"))
			{
				printf("Secrets: \n");
				azuresecrets.ListSecrets();
				printf("\n");
			}

			else if (!strcmp(command, "c"))
			{
				prompt("Secret Name", ":", "TestSecret", secretname);
				prompt("Secret Type", ":", "Test", secrettype);
				prompt("Secret Data", ":", "123", data);

				azuresecrets.SetSecretData(data, sizeof(data));
				azuresecrets.CreateSecret(secretname, secrettype);

				ret_code = azuresecrets.GetLastErrorCode();
				if (ret_code) goto done;

				printf("\nSecret Created Successfully\n");
			}

			else if (!strcmp(command, "d"))
			{
				prompt("Secret Name", ":", "TestSecret", secretname);
				azuresecrets.DeleteSecret(secretname);

				ret_code = azuresecrets.GetLastErrorCode();
				if (ret_code) goto done;

				printf("\nSecret Deleted Successfully\n");
			}

			else if (!strcmp(command, "v"))
			{
				prompt("Secret Name", ":", "TestSecret", secretname);
				azuresecrets.GetSecret(secretname);

				printf("Secret Data: ");
				azuresecrets.GetSecretData(output, output_length);
				printf("%s\n", output);
			}

			else if (!strcmp(command, "q")) 
			{
				goto done;
			}

			else
			{
				printf("Command Not Recognized.\n");
			}

		  ret_code = azuresecrets.GetLastErrorCode();
			if (ret_code) goto done;

		}  // end of main while loop

	done:
		if (ret_code)     // Got an error.  The user is done.
		{
			printf("\nError: %d", ret_code);
			if (azuresecrets.GetLastError())
			{
				printf(" \"%s\"\n", azuresecrets.GetLastError());
			}
		}
		fprintf(stderr, "\npress <return> to continue...\n");
		exit(ret_code);
		return 0;
	}
}



