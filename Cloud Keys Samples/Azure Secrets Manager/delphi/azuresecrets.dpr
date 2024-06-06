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

program azuresecrets;

uses
  Forms,
  createsecretf in 'createsecretf.pas'   {FormCreatesecretf},
  azuresecretsf in 'azuresecretsf.pas' {FormAzuresecrets};

begin
  Application.Initialize;

  Application.CreateForm(TFormAzuresecrets, FormAzuresecrets);
  Application.CreateForm(TFormCreatesecret, FormCreatesecret);

  Application.Run;
end.


         
