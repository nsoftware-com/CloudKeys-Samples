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
unit amazonkmsf;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics,
  Vcl.Controls, Vcl.Forms, Vcl.Dialogs, Vcl.ExtCtrls, Vcl.StdCtrls, ckcore,
  cktypes, ckamazonkms, Vcl.Menus, Vcl.ComCtrls;

type
  TFormAmazonkms = class(TForm)
    Label1: TLabel;
    Panel1: TPanel;
    Panel5: TPanel;
    Panel7: TPanel;
    Label2: TLabel;
    accessKey: TEdit;
    Label3: TLabel;
    secretKey: TEdit;
    Label4: TLabel;
    region: TEdit;
    createKey: TButton;
    scheduleDelete: TButton;
    Label5: TLabel;
    Edit4: TEdit;
    cancelDelete: TButton;
    Button7: TButton;
    ListView1: TListView;
    ckAmazonKMS1: TckAmazonKMS;
    procedure Button7Click(Sender: TObject);
    procedure ckAmazonKMS1KeyList(Sender: TObject; const ARN, Id, AccountId,
      Description: string; Enabled, AWSManaged, ForSigning: Boolean;
      const KeySpec, Algorithms: string; State: Integer; const CreationDate,
      DeletionDate: string);
    procedure accessKeyChange(Sender: TObject);
    procedure secretKeyChange(Sender: TObject);
    procedure regionChange(Sender: TObject);
    procedure createKeyClick(Sender: TObject);
    procedure scheduleDeleteClick(Sender: TObject);
    procedure cancelDeleteClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
    procedure RefreshKeys();
  end;

var
  FormAmazonkms: TFormAmazonkms;

implementation

{$R *.DFM}

procedure TFormAmazonkms.ckAmazonKMS1KeyList(Sender: TObject; const ARN, Id, AccountId,
  Description: string; Enabled, AWSManaged, ForSigning: Boolean; const KeySpec,
  Algorithms: string; State: Integer; const CreationDate, DeletionDate: string);
var
  Itm: TListItem;
begin
    ListView1.Items.BeginUpdate;
    Itm := ListView1.Items.Add;
    Itm.Caption := Id;
    Itm.SubItems.Add(KeySpec);
    if ForSigning
    then Itm.SubItems.Add('Signing')
    else Itm.SubItems.Add('Encryption');
    case State of
      0: Itm.SubItems.Add('Enabled');
      1: Itm.SubItems.Add('Disabled');
      2: Itm.SubItems.Add('Pending Deletion');
      3: Itm.SubItems.Add('Pending Import');
      4: Itm.SubItems.Add('Unavailable');
    else ShowMessage('Unknown state');
    end;
    Itm.SubItems.Add(Description);

    ListView1.Items.EndUpdate;
end;

procedure TFormAmazonkms.RefreshKeys();
begin
  ListView1.Items.Clear();
  ckAmazonKMS1.IncludeKeyDetails:=True;
  try
      ckAmazonKMS1.ListKeys();
  finally

  end;
end;


procedure TFormAmazonkms.regionChange(Sender: TObject);
begin
  ckAmazonKMS1.Region := region.Text;
end;

procedure TFormAmazonkms.accessKeyChange(Sender: TObject);
begin
  ckAmazonKMS1.AccessKey := accessKey.Text;
end;

procedure TFormAmazonkms.Button7Click(Sender: TObject);
begin
  RefreshKeys();
end;

procedure TFormAmazonkms.cancelDeleteClick(Sender: TObject);
begin
   ckAmazonKMS1.CancelKeyDeletion(ListView1.Selected.Caption);
  RefreshKeys();
end;

procedure TFormAmazonkms.createKeyClick(Sender: TObject);
begin
  ckAmazonKMS1.CreateKey('SYMMETRIC_DEFAULT', false, 'new key');
  RefreshKeys();
end;

procedure TFormAmazonkms.scheduleDeleteClick(Sender: TObject);
begin
  ckAmazonKMS1.ScheduleKeyDeletion(ListView1.Selected.Caption, strtoint(Edit4.Text));
  RefreshKeys();
end;

procedure TFormAmazonkms.secretKeyChange(Sender: TObject);
begin
  ckAmazonKMS1.SecretKey := secretKey.Text;
end;

end.

