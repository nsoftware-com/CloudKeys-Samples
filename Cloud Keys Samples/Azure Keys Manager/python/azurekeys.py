# 
# Cloud Keys 2024 Python Edition - Sample Project
# 
# This sample project demonstrates the usage of Cloud Keys in a 
# simple, straightforward way. It is not intended to be a complete 
# application. Error handling and other checks are simplified for clarity.
# 
# www.nsoftware.com/cloudkeys
# 
# This code is subject to the terms and conditions specified in the 
# corresponding product license agreement which outlines the authorized 
# usage and restrictions.
# 

import sys
import string
from cloudkeys import *

input = sys.hexversion < 0x03000000 and raw_input or input


def ensureArg(args, prompt, index):
    if len(args) <= index:
        while len(args) <= index:
            args.append(None)
        args[index] = input(prompt)
    elif args[index] is None:
        args[index] = input(prompt)


def fireKeyListEvent(e):
  print("  " + e.name)

def display_menu():
  print("\nAzureKeys Commands:")
  print("l            List Keys")
  print("c            Create a Key")
  print("d            Delete a Key")
  print("e            Encrypts a String")
  print("s            Signs a String")
  print("?            Display Options")
  print("q            Quit")

def prompt(message, default):
  ret = input(message + " [" + default + "]: ")
  if ret == "":
    return default
  return ret

azurekeys = AzureKeys()
azurekeys.on_key_list = fireKeyListEvent

try:
  input("Press Enter to Authenticate.")

  azurekeys.set_o_auth_client_id("ce5c0f06-1f2b-4f98-8abf-73f8aaa2592c")
  azurekeys.set_o_auth_client_secret("3KqXE.3tm~0-1A~~V6AjSA1Y8a1FI.Fgec")
  azurekeys.set_o_auth_authorization_scope("offline_access https://vault.azure.net/user_impersonation")
  azurekeys.set_o_auth_server_auth_url("https://login.microsoftonline.com/common/oauth2/v2.0/authorize")
  azurekeys.set_o_auth_server_token_url("https://login.microsoftonline.com/common/oauth2/v2.0/token")
  azurekeys.config("OAuthWebServerPort=7777")
  
  azurekeys.authorize()

  print("Authentication Successful.")

  azurekeys.set_vault(input("AzureKeys Vault Name: "))

  while True:
    display_menu()
    command = input("> ")

    if command == "l":
      print("Keys: ")
      azurekeys.list_keys()
      print()

    elif command == "c":
      name = prompt("Key Name", "TestKey")
      type = prompt("Key Type", "RSA_2048")
      
      if name.startswith("EC"):
        azurekeys.create_key(name, type, "sign, verify")
      else:
        azurekeys.create_key(name, type, "encrypt, decrypt, sign, verify")

      print("Key Created Successfully.")

    elif command == "d":
      name = prompt("Key Name", "TestKey")

      azurekeys.delete_key(name)

      print("Key Deleted Successfully.")

    elif command == "e":
      name = prompt("Key Name", "TestKey")
      
      algorithm = prompt("Encryption Algorithm", "RSA-OAEP")
      azurekeys.set_input_data(prompt("String to Encrypt", "This is an Example!"))
      
      azurekeys.encrypt(name, algorithm)

      print("Encrypted Data: " + str(azurekeys.get_output_data()))

    elif command == "s":
      name = prompt("Key Name", "TestKey")

      algorithm = prompt("Signature Algorithm: ", "PS256")
      azurekeys.set_input_data(prompt("String to Sign: ", "This is an Example!"))

      azurekeys.sign(name, algorithm, False)

      print("Signature Data: " + str(azurekeys.get_output_data()))

    elif command == "?":
      continue
    elif command == "q":
      break
    else:
      print("Command not recognized.")

  
except CloudKeysError as e:
    print("ERROR %s"%e.message)
    sys.exit()  


