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



def DisplayMenu():
  print("\nAmazonKMS Commands:")
  print("l            List Keys")
  print("c            Create a Key")
  print("d            Delete a Key")
  print("e            Encrypts a String")
  print("s            Signs a String")
  print("?            Display Options")
  print("q            Quit")



if len(sys.argv) != 1:
  print("usage: amazonkms.py")
  print("")
  print("  host  the IP address (IP number in dotted internet format) or Domain Name of the remote host")
  print("\r\nExample: ping.py")
else:
  kms = AmazonKMS()

  def fireKeyList(e):
    print("Id: "+e.id)

  def fireError(e):
    print(e.message)

  
  kms.on_key_list = fireKeyList
  kms.on_error = fireError

  try:
    kms.set_access_key(input("Access key: "))
    kms.set_secret_key(input("Secret key: "))

    while (True):
      DisplayMenu()
      command = input("> ")
      if (command == "l"):
        print("Keys: ")
        kms.list_keys()
      elif (command == "c"):
        forSigning = input("For signing? (y/n): ")
        if (forSigning == "y"):
          keyId = kms.create_key(input("Key Spec: "),True,"sign, verify")
        else:
          keyId = kms.create_key(input("Key Spec: "),False,"encrypt, decrypt")
        print("New key id: "+keyId)
      elif (command == "d"):
        kms.schedule_key_deletion(input("Key Id: "),7)
      elif (command == "e"):
        kms.set_input_data(input("Input string: "))
        keyId = input("Key id: ")
        algorithm = input("Algorithm: ")
        kms.encrypt(keyId,algorithm)
      elif (command == "s"):
        kms.set_input_data(input("Input string: "))
        keyId = input("Key id: ")
        algorithm = input("Algorithm: ")
        kms.sign(keyId,algorithm,False)
      elif (command == "?"):
        DisplayMenu()
      elif (command == "q"):
        break
      else:
        print("command not recognized")   

  except CloudKeysError as e:
    print("ERROR %s" %e.message)

