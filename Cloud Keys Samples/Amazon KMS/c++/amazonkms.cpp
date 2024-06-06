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

class MyAmazonKMS : public AmazonKMS {
public:
	virtual int FireKeyList(AmazonKMSKeyListEventParams* e) {
		printf("  %s\n", e->Id);
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
	int ret_code;

	if (argc < 1) {

		fprintf(stderr, "usage: amazonkms\n");
		fprintf(stderr, "\n");
		printf("\nPress enter to continue.\n");
		getchar();

	}
	else{
		MyAmazonKMS amazonkms; 
		char command[LINE_LEN];     // user's command
		char *argument;             // arguments to the user's command
		char keyname[LINE_LEN], keytype[LINE_LEN], algorithm[LINE_LEN];
		char *output, input[LINE_LEN];
		char* arn;
		int output_length;

		prompt("Access Key", ":", "", command);
		amazonkms.SetAccessKey(command);
		prompt("Secret Key", ":", "", command);
		amazonkms.SetSecretKey(command);

		while (1)
		{
			printf("\nAmazonKMS Commands:\n");
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
				printf("\nAmazonKMS Commands:\n");
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
				amazonkms.ListKeys();
				printf("\n");
			}

			else if (!strcmp(command, "c"))
			{
				prompt("Key Spec", ":", "RSA_2048", keyname);
				prompt("For signing", "?", "y", keytype);
				if (keytype[1] == 'y') {
					arn = amazonkms.CreateKey(keyname, true, "sign, verify");
				}
				else {
					arn = amazonkms.CreateKey(keyname, false, "encrypt, decrypt, sign, verify");
				}

				ret_code = amazonkms.GetLastErrorCode();
				if (ret_code) goto done;

				printf("\nKey Created Successfully\n");
				printf("Key ARN: %s\n",arn);
			}

			else if (!strcmp(command, "d"))
			{
				prompt("Key Id", ":", "", keyname);
				amazonkms.ScheduleKeyDeletion(keyname,7);

				ret_code = amazonkms.GetLastErrorCode();
				if (ret_code) goto done;

				printf("\nKey Deleted Successfully\n");
			}

			else if (!strcmp(command, "e"))
			{
				prompt("Key Id", ":", "", keyname);
				prompt("Encryption Algorithm", ":", "RSAES_OAEP_SHA_256", algorithm);
				prompt("String to Encrypt", ":", "This is an Example!", input);
				amazonkms.SetInputData(input, sizeof(input));
				amazonkms.Encrypt(keyname, algorithm);

				printf("Encrypted Data: ");
				amazonkms.GetOutputData(output, output_length);
				printf(output);
			}

			else if (!strcmp(command, "s"))
			{
				prompt("Key Id", ":", "", keyname);
				prompt("Signature Algorithm", ":", "ECDSA_SHA_256", algorithm);
				prompt("String to Sign", ":", "This is an Example!", input);
				amazonkms.SetInputData(input, sizeof(input));
				amazonkms.Sign(keyname, algorithm, false);
				printf("Signature Data: ");
				amazonkms.GetOutputData(output, output_length);
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

		  ret_code = amazonkms.GetLastErrorCode();
			if (ret_code) goto done;

		}  // end of main while loop

	done:
		if (ret_code)     // Got an error.  The user is done.
		{
			printf("\nError: %d", ret_code);
			if (amazonkms.GetLastError())
			{
				printf(" \"%s\"\n", amazonkms.GetLastError());
			}
		}
		fprintf(stderr, "\npress <return> to continue...\n");
		exit(ret_code);
		return 0;
	}
}

