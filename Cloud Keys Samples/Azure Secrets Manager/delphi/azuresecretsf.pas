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
unit azuresecretsf;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics,
  Vcl.Controls, Vcl.Forms, Vcl.Dialogs, Vcl.ExtCtrls, Vcl.StdCtrls, ckcore,
  cktypes, ckazuresecrets, ckoauth, Vcl.Menus, Vcl.ComCtrls;

type
  TFormAzuresecrets = class(TForm)
    Label1: TLabel;
    Panel1: TPanel;
    Panel5: TPanel;
    Panel7: TPanel;
    createSecret: TButton;
    deleteSecret: TButton;
    listSecrets: TButton;
    ListView1: TListView;
    logIn: TButton;
    Label2: TLabel;
    vaultEdit: TEdit;
    Panel2: TPanel;
    secretDetails: TMemo;
    ckAzureSecrets1: TckAzureSecrets;
    procedure listSecretsClick(Sender: TObject);
    procedure createSecretClick(Sender: TObject);
    procedure deleteSecretClick(Sender: TObject);
    procedure logInClick(Sender: TObject);
    procedure vaultEditChange(Sender: TObject);
    procedure ckAzureSecrets1SecretList(Sender: TObject;
      const Name: String; const VersionId: String; const ContentType: String;
      Enabled: Boolean; CreationDate: Int64; UpdateDate: Int64; DeletionDate: Int64;
      PurgeDate: Int64);
    procedure ListView1Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
    procedure RefreshSecrets();
  end;

var
  FormAzuresecrets: TFormAzuresecrets;

implementation

{$R *.DFM}

uses createsecretf;

procedure TFormAzuresecrets.RefreshSecrets();
begin
  ListView1.Items.Clear();

  ckAzureSecrets1.ListSecrets();
end;

procedure TFormAzuresecrets.vaultEditChange(Sender: TObject);
begin
  ckAzureSecrets1.Vault := vaultEdit.Text;
end;

procedure TFormAzuresecrets.logInClick(Sender: TObject);
begin
  ckAzureSecrets1.OAuth.ClientId := 'ce5c0f06-1f2b-4f98-8abf-73f8aaa2592c';
  ckAzureSecrets1.OAuth.ClientSecret := '3KqXE.3tm~0-1A~~V6AjSA1Y8a1FI.Fgec';
  ckAzureSecrets1.OAuth.AuthorizationScope := 'offline_access https://vault.azure.net/user_impersonation';
  ckAzureSecrets1.OAuth.ServerAuthURL := 'https://login.microsoftonline.com/common/oauth2/v2.0/authorize';
  ckAzureSecrets1.OAuth.ServerTokenURL := 'https://login.microsoftonline.com/common/oauth2/v2.0/token';
  ckAzureSecrets1.Config('OAuthWebServerPort=7777');

  ckAzureSecrets1.Authorize();
  logIn.Enabled := false;
end;

procedure TFormAzuresecrets.listSecretsClick(Sender: TObject);
begin
  RefreshSecrets();
end;

procedure TFormAzuresecrets.ListView1Click(Sender: TObject);
var selected: integer;
begin
  if ListView1.SelCount = 1 then
  begin
    selected := ListView1.Selected.Index;
    ckAzureSecrets1.GetSecret(ListView1.Selected.Caption);
    secretDetails.Clear();
    secretDetails.Lines.Add('Name:');
    secretDetails.Lines.Add(ckAzureSecrets1.SecretName[0]);
    secretDetails.Lines.Add(' ');
    secretDetails.Lines.Add('Version Id:');
    secretDetails.Lines.Add(ckAzureSecrets1.SecretVersionId[0]);
    secretDetails.Lines.Add(' ');
    secretDetails.Lines.Add('Content Type:');
    secretDetails.Lines.Add(ckAzureSecrets1.SecretContentType[0]);
    secretDetails.Lines.Add(' ');
    secretDetails.Lines.Add('Data:');
    secretDetails.Lines.Add(ckAzureSecrets1.SecretData);
    RefreshSecrets();
    ListView1.Selected := ListView1.Items[selected];
  end;
end;

procedure TFormAzuresecrets.createSecretClick(Sender: TObject);
begin
  try
    if FormCreatesecret.ShowModal() = mrOk then
    begin
      ckAzureSecrets1.SecretData := FormCreatesecret.Edit3.Text;
      ckAzureSecrets1.CreateSecret(FormCreatesecret.Edit1.Text, FormCreatesecret.Edit2.Text);
      RefreshSecrets();
    end;
  except on E:ECloudKeys do
    ShowMessage(E.Message)
  end;
end;

procedure TFormAzuresecrets.deleteSecretClick(Sender: TObject);
begin
  ckAzureSecrets1.DeleteSecret(ListView1.Selected.Caption);
  RefreshSecrets();
  secretDetails.Clear();
end;


procedure TFormAzuresecrets.ckAzureSecrets1SecretList(Sender: TObject;
  const Name: String; const VersionId: String; const ContentType: String;
  Enabled: Boolean; CreationDate: Int64; UpdateDate: Int64; DeletionDate: Int64;
  PurgeDate: Int64);
var
  Itm: TListItem;
begin
  ListView1.Items.BeginUpdate;
  Itm := ListView1.Items.Add;
  Itm.Caption := Name;
  Itm.SubItems.Add(VersionId);

  Itm.SubItems.Add(ContentType);
  if Enabled then
    Itm.SubItems.Add('Enabled')
  else
    Itm.SubItems.Add('Disabled');

  ListView1.Items.EndUpdate;
end;

end.




