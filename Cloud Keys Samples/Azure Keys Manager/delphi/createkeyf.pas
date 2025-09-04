unit createkeyf;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics,
  Vcl.Controls, Vcl.Forms, Vcl.Dialogs, Vcl.StdCtrls;

type
  TFormCreatekey = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    CheckBox1: TCheckBox;
    Button1: TButton;
    ComboBox1: TComboBox;
    Button2: TButton;
    Edit1: TEdit;
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  FormCreatekey: TFormCreatekey;

implementation

{$R *.dfm}

end.
