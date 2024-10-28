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
unit azurekeysf;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics,
  Vcl.Controls, Vcl.Forms, Vcl.Dialogs, Vcl.ExtCtrls, Vcl.StdCtrls, ckcore,
  cktypes, ckazurekeys, ckoauth, Vcl.Menus, Vcl.ComCtrls;

type
  TFormAzurekeys = class(TForm)
    Label1: TLabel;
    Panel1: TPanel;
    Panel5: TPanel;
    Panel7: TPanel;
    createKey: TButton;
    deleteKey: TButton;
    listKeys: TButton;
    ListView1: TListView;
    logIn: TButton;
    Label2: TLabel;
    vaultEdit: TEdit;
    Panel2: TPanel;
    Panel3: TPanel;
    Panel4: TPanel;
    Label3: TLabel;
    encryptionAlgorithm: TComboBox;
    Label4: TLabel;
    encryptData: TButton;
    Label5: TLabel;
    signatureAlgorithm: TComboBox;
    signData: TButton;
    plaintext: TMemo;
    data: TMemo;
    ckAzureKeys1: TckAzureKeys;
    procedure listKeysClick(Sender: TObject);
    procedure createKeyClick(Sender: TObject);
    procedure deleteKeyClick(Sender: TObject);
    procedure logInClick(Sender: TObject);
    procedure ckAzureKeys1KeyList(Sender: TObject;
      const Name: String; const VersionId: String; const KeyType: String; const KeyOps: String;
      Enabled: Boolean; CreationDate: Int64; UpdateDate: Int64; DeletionDate: Int64;
      PurgeDate: Int64; const PublicKey: String);
    procedure vaultEditChange(Sender: TObject);
    procedure encryptDataClick(Sender: TObject);
    procedure signDataClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
    procedure RefreshKeys();
  end;

var
  FormAzurekeys: TFormAzurekeys;

implementation

{$R *.DFM}

uses createkeyf;

procedure TFormAzurekeys.RefreshKeys();
begin
  ListView1.Items.Clear();

  ckAzureKeys1.ListKeys();
end;

procedure TFormAzurekeys.signDataClick(Sender: TObject);
begin
  try
    ckAzureKeys1.InputData := plaintext.Text;
    ckAzureKeys1.Sign(ListView1.Selected.Caption, signatureAlgorithm.Text, false);
    data.Text := ckAzureKeys1.OutputData;
  except on E:ECloudKeys do
    ShowMessage(E.Message)
  end;
end;

procedure TFormAzurekeys.vaultEditChange(Sender: TObject);
begin
  ckAzureKeys1.Vault := vaultEdit.Text;
end;

procedure TFormAzurekeys.logInClick(Sender: TObject);
begin
  ckAzureKeys1.OAuth.ClientId := 'ce5c0f06-1f2b-4f98-8abf-73f8aaa2592c';
  ckAzureKeys1.OAuth.ClientSecret := '3KqXE.3tm~0-1A~~V6AjSA1Y8a1FI.Fgec';
  ckAzureKeys1.OAuth.AuthorizationScope := 'offline_access https://vault.azure.net/user_impersonation';
  ckAzureKeys1.OAuth.ServerAuthURL := 'https://login.microsoftonline.com/common/oauth2/v2.0/authorize';
  ckAzureKeys1.OAuth.ServerTokenURL := 'https://login.microsoftonline.com/common/oauth2/v2.0/token';
  ckAzureKeys1.Config('OAuthWebServerPort=7777');

  ckAzureKeys1.Authorize();
  logIn.Enabled := false;
end;

procedure TFormAzurekeys.listKeysClick(Sender: TObject);
begin
  RefreshKeys();
end;

procedure TFormAzurekeys.createKeyClick(Sender: TObject);
begin
  try
    if FormCreatekey.ShowModal() = mrOk then
    begin
      if FormCreatekey.CheckBox1.Checked then
        ckAzureKeys1.CreateKey(FormCreatekey.Edit1.Text,
                                FormCreatekey.ComboBox1.Text,
                                'encrypt, decrypt, sign, verify')
      else
        ckAzureKeys1.CreateKey(FormCreatekey.Edit1.Text,
                                FormCreatekey.ComboBox1.Text,
                                'sign, verify');
      RefreshKeys();
    end;
  except on E:ECloudKeys do
    ShowMessage(E.Message)
  end;
end;

procedure TFormAzurekeys.deleteKeyClick(Sender: TObject);
begin
  ckAzureKeys1.DeleteKey(ListView1.Selected.Caption);
  RefreshKeys();
end;

procedure TFormAzurekeys.encryptDataClick(Sender: TObject);
begin
  try
    ckAzureKeys1.InputData := 'Test';
    ckAzureKeys1.Encrypt(ListView1.Selected.Caption, encryptionAlgorithm.Text);
    data.Text := ckAzureKeys1.OutputData;
  except on E:ECloudKeys do
    ShowMessage(E.Message)
  end;
end;

procedure TFormAzurekeys.ckAzureKeys1KeyList(Sender: TObject; const Name: String;
  const VersionId: String; const KeyType: String; const KeyOps: String;
  Enabled: Boolean; CreationDate: Int64; UpdateDate: Int64; DeletionDate: Int64;
  PurgeDate: Int64; const PublicKey: String);
var
  Itm: TListItem;
begin
  ListView1.Items.BeginUpdate;
  Itm := ListView1.Items.Add;
  Itm.Caption := Name;
  Itm.SubItems.Add(VersionId);

  Itm.SubItems.Add(KeyType);
  if Enabled then
    Itm.SubItems.Add('Enabled')
  else
    Itm.SubItems.Add('Disabled');

  Itm.SubItems.Add(KeyOps);

  ListView1.Items.EndUpdate;
end;

end.



