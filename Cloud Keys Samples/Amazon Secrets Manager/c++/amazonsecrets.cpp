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

class MyAmazonSecrets : public AmazonSecrets {
public:
	virtual int FireSecretList(AmazonSecretsSecretListEventParams* e) {
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

	if (argc < 1) {

		fprintf(stderr, "usage: amazonsecrets \n");
		fprintf(stderr, "\n");
		printf("\nPress enter to continue.\n");
		getchar();

	}
	else{
		MyAmazonSecrets amazonsecrets; 
		char command[LINE_LEN];     // user's command
		char *argument;             // arguments to the user's command
		char secretId[LINE_LEN], secretDescription[LINE_LEN], data[LINE_LEN];
		char versionId[LINE_LEN], stagingLabel[LINE_LEN];
		char *output, input[LINE_LEN];
		int output_length;

		prompt("Access Key", ":", "", command);
		amazonsecrets.SetAccessKey(command);
		prompt("Secret Key", ":", "", command);
		amazonsecrets.SetSecretKey(command);

		int ret_code = amazonsecrets.GetLastErrorCode();
		if (ret_code) goto done;

		while (1)
		{
			printf("\nAmazonSecrets Commands:\n");
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
				printf("\nAmazonSecrets Commands:\n");
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
				amazonsecrets.ListSecrets();
				printf("\n");
			}

			else if (!strcmp(command, "c"))
			{
				prompt("Secret Id", ":", "", secretId);
				prompt("Secret Description", ":", "Test", secretDescription);
				prompt("Secret Data", ":", "Test", data);

				printf("Secret Id: %s\n", secretId);
				printf("Secret desc: %s\n", secretDescription);
				printf("Data: %s\n", data);
				amazonsecrets.SetSecretData(data, sizeof(data));
				amazonsecrets.CreateSecret(secretId, secretDescription);

				ret_code = amazonsecrets.GetLastErrorCode();
				if (ret_code) goto done;

				printf("\nSecret Created Successfully\n");
			}

			else if (!strcmp(command, "d"))
			{
				prompt("Secret Id", ":", "", secretId);
				amazonsecrets.DeleteSecret(secretId,7);

				ret_code = amazonsecrets.GetLastErrorCode();
				if (ret_code) goto done;

				printf("\nSecret Deleted Successfully\n");
			}

			else if (!strcmp(command, "v"))
			{
				prompt("Secret Name", ":", "TestSecret", secretId);
				prompt("Version Id", ":","", versionId);
				prompt("Staging Label", ":","", stagingLabel);
				amazonsecrets.GetSecret(secretId,versionId,stagingLabel);

				ret_code = amazonsecrets.GetLastErrorCode();
				if (ret_code) goto done;

				printf("Secret Data: ");
				amazonsecrets.GetSecretData(output, output_length);
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

		  ret_code = amazonsecrets.GetLastErrorCode();
			if (ret_code) goto done;

		}  // end of main while loop

	done:
		if (ret_code)     // Got an error.  The user is done.
		{
			printf("\nError: %d", ret_code);
			if (amazonsecrets.GetLastError())
			{
				printf(" \"%s\"\n", amazonsecrets.GetLastError());
			}
		}
		fprintf(stderr, "\npress <return> to continue...\n");
		exit(ret_code);
		return 0;
	}
}

