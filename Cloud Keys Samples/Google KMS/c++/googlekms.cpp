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

class MyGoogleKMS : public GoogleKMS {
public:
	virtual int FireKeyList(GoogleKMSKeyListEventParams* e) {
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

		fprintf(stderr, "usage: googlekms project\n");
		fprintf(stderr, "\n");
		fprintf(stderr, "  project     the Google Cloud project ID of the project to connect to\n");
		printf("\nPress enter to continue.\n");
		getchar();

	}
	else{
		MyGoogleKMS googlekms; 
		char command[LINE_LEN];     // user's command
		char *argument;             // arguments to the user's command
		char keyname[LINE_LEN], keyring[LINE_LEN], keytype[LINE_LEN], algorithm[LINE_LEN];
		char *output, input[LINE_LEN];
		int output_length;

		printf("Press Enter to Authenticate.");
		getchar();

		googlekms.SetOAuthClientId("157623334268.apps.googleusercontent.com");
		googlekms.SetOAuthClientSecret("v8R8R90hn_LchsVc0Ta6Sy0D");
		googlekms.SetOAuthAuthorizationScope("https://www.googleapis.com/auth/cloud-platform");
		googlekms.SetOAuthServerAuthURL("https://accounts.google.com/o/oauth2/auth");
		googlekms.SetOAuthServerTokenURL("https://accounts.google.com/o/oauth2/token");

		googlekms.Authorize();
		googlekms.SetGoogleProjectId(argv[1]);

		int ret_code = googlekms.GetLastErrorCode();
		if (ret_code) goto done;

		printf("Authentication Successful.\n");

		prompt("Key Ring Name (If the key ring does not exist, it will be created)", ":", "CloudKeysDemo", keyring);
		googlekms.SetKeyRing(keyring);

		googlekms.ListKeyRings();

		for (int i = 0; i < googlekms.GetKeyRingCount(); i++) {
			if (!strcmp(keyring, googlekms.GetKeyRingName(i))) {
				printf("Key Ring Found.\n");
				goto start;
			}
		}
		
		printf("Creating a New Key Ring...\n");
		googlekms.CreateKeyRing();

	  start:
		ret_code = googlekms.GetLastErrorCode();
		if (ret_code) goto done;

		while (1)
		{
			printf("\nGoogleKMS Commands:\n");
			printf("l            List Keys\n");
			printf("c            Create a Key\n");
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
				printf("\nGoogleKMS Commands:\n");
				printf("l            List Keys\n");
				printf("c            Create a Key\n");
				printf("e            Encrypts a String\n");
				printf("s            Signs a String\n");
				printf("?            Display Options\n");
				printf("q            Quit\n");

				printf("> ");
			}

			else if (!strcmp(command, "l"))
			{
				printf("Keys: \n");
				googlekms.ListKeys();
				printf("\n");
			}

			else if (!strcmp(command, "c"))
			{
				prompt("Key Name", ":", "TestKey", keyname);

				printf("Valid Purpose Values:\n");
				printf("  1: A symmetric key used for encryption and decryption.\n");
				printf("  2: An asymmetric key used for signing and verification.\n");
				printf("  3: An asymmetric key used for encryption and decryption.\n");

				prompt("Key Purpose", ":", "2", command);
				int purpose = atoi(command);

				prompt("Key Algorithm", ":", "RSA_SIGN_PSS_2048_SHA256", algorithm);

				googlekms.CreateKey(keyname, purpose, algorithm, false);

				ret_code = googlekms.GetLastErrorCode();
				if (ret_code) goto done;

				printf("\nKey Created Successfully\n");
			}

			else if (!strcmp(command, "e"))
			{
				prompt("Key Name", ":", "TestKey", keyname);
				prompt("String to Encrypt", ":", "This is an Example!", input);
				googlekms.SetInputData(input, sizeof(input));
				googlekms.Encrypt(keyname, "");

				ret_code = googlekms.GetLastErrorCode();
				if (ret_code) goto done;

				printf("Encrypted Data: ");
				googlekms.GetOutputData(output, output_length);
				printf(output);
			}

			else if (!strcmp(command, "s"))
			{
				prompt("Key Name", ":", "TestKey", keyname);
				prompt("Signature Algorithm", ":", "SHA256", algorithm);
				prompt("String to Sign", ":", "This is an Example!", input);
				googlekms.SetInputData(input, sizeof(input));

				googlekms.ListVersions(keyname);

				googlekms.Sign(keyname, googlekms.GetVersionVersionId(0), algorithm, false);

				ret_code = googlekms.GetLastErrorCode();
				if (ret_code) goto done;

				printf("Signature Data: ");
				googlekms.GetOutputData(output, output_length);
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

		  ret_code = googlekms.GetLastErrorCode();
			if (ret_code) goto done;

		}  // end of main while loop

	done:
		if (ret_code)     // Got an error.  The user is done.
		{
			printf("\nError: %d", ret_code);
			if (googlekms.GetLastError())
			{
				printf(" \"%s\"\n", googlekms.GetLastError());
			}
		}
		fprintf(stderr, "\npress <return> to continue...\n");
		exit(ret_code);
		return 0;
	}
}



