(*
 * Cloud Keys 2024 Delphi Edition - Sample Project
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
 *)

program azurekeys;

uses
  Forms,
  createkeyf in 'createkeyf.pas'   {FormCreatekeyf},
  azurekeysf in 'azurekeysf.pas' {FormAzurekeys};

begin
  Application.Initialize;

  Application.CreateForm(TFormAzurekeys, FormAzurekeys);
  Application.CreateForm(TFormCreatekey, FormCreatekey);

  Application.Run;
end.


         
