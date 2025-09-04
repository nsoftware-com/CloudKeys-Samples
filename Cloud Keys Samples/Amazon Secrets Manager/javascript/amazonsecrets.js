/*
 * Cloud Keys 2024 JavaScript Edition - Sample Project
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
 
const readline = require("readline");
const cloudkeys = require("@nsoftware/cloudkeys");

if(!cloudkeys) {
  console.error("Cannot find cloudkeys.");
  process.exit(1);
}
let rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

main();

async function main() {
  const amazonsecrets = new cloudkeys.amazonsecrets();

  amazonsecrets.on('Error', (e) => (console.log(`Error [${e.errorCode}] ${e.description}`)))
  .on('SecretList', (e) => (console.log(`   ${e.name}`)));

  prompt("accesskey", "AWS Access Key", ":", "");

  rl.on('line', async (line) => {
    
    const args = line.split(/\s+/);
    const cmd = args[0];
    switch (lastPrompt){
      case "accesskey":
        amazonsecrets.setAccessKey(line);
        prompt("secretkey", "AWS Secret Key", ":", "");
      break;
      case "secretkey":
        amazonsecrets.setSecretKey(line);
        lastPrompt = "start";
    }

    switch (cmd){
      case 'q':
        console.log('Quitting');
        process.exit();
      case 'l':
        console.log("Secrets: ")
        try{
          await amazonsecrets.listSecrets();
        } catch (e) {
          console.log(e);
        }
        
        process.stdout.write('> ');
        break;
      case 'c':
        if(args.length !== 4){
          console.log("Usage: c SecretName SecretDescription SecretData")
          process.stdout.write('> ');
          break;
        }
        try{
          amazonsecrets.setSecretData(args[3]);
          await amazonsecrets.createSecret(args[1], args[2]).catch(e => (console.log(e)));
        } catch (e) {
          console.log(e);
        }
        console.log("Secret Created Successfully");
        process.stdout.write('> ');
        break;
      case 'd':
        if(args.length !== 2){
          console.log("Usage: d SecretName")
          process.stdout.write('> ');
          break;
        }
        amazonsecrets.deleteSecret(args[1], 7);
        console.log("Secret Deleted Successfully")
        process.stdout.write('> ');
        break;
      case 'v':
        if(args.length !== 2){
          console.log("Usage: v SecretId")
          process.stdout.write('> ');
          break;
        }
        try{
          await amazonsecrets.getSecret(args[1], "", "");
          console.log(`Secret Data: ${amazonsecrets.getSecretData().toString()}`);
        } catch (e) {
          console.log(e);
        }
        
  
        process.stdout.write('> ');
        break;
      case '?':
        DisplayMenu();
        process.stdout.write('> ');
        break;
      default:
        if(lastPrompt === "secretkey"){
          break;
        } else if(lastPrompt === "start"){
          DisplayMenu();
          process.stdout.write('> ');
        } else {
          console.log("Invalid Command");
          DisplayMenu();
          process.stdout.write('> ');
        }
    }
	});
  
}

function DisplayMenu(){
  console.log("\AmazonSecrets Commands:");
  console.log("l            List Secrets");
  console.log("c            Create a Secret");
  console.log("d            Delete a Secret");
  console.log("v            View a Secret's Data");
  console.log("?            Display Options");
  console.log("q            Quit");
  lastPrompt = "";
}



function prompt(promptName, label, punctuation, defaultVal)
{
  lastPrompt = promptName;
  lastDefault = defaultVal;
  process.stdout.write(`${label} [${defaultVal}]${punctuation} `);
}
