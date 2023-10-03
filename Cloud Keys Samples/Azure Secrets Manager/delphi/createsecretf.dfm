object FormCreatesecret: TFormCreatesecret
  Left = 0
  Top = 0
  Caption = 'Create Secret'
  ClientHeight = 127
  ClientWidth = 345
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 16
    Top = 8
    Width = 34
    Height = 13
    Caption = 'Name: '
  end
  object Label2: TLabel
    Left = 16
    Top = 38
    Width = 62
    Height = 13
    Caption = 'Secret Type:'
  end
  object Label3: TLabel
    Left = 16
    Top = 65
    Width = 61
    Height = 13
    Caption = 'Secret Data:'
  end
  object Button1: TButton
    Left = 254
    Top = 89
    Width = 75
    Height = 25
    Caption = 'OK'
    Default = True
    ModalResult = 1
    TabOrder = 0
  end
  object Button2: TButton
    Left = 173
    Top = 89
    Width = 75
    Height = 25
    Caption = 'Cancel'
    ModalResult = 2
    TabOrder = 1
  end
  object Edit1: TEdit
    Left = 84
    Top = 8
    Width = 245
    Height = 21
    TabOrder = 2
    Text = 'TestSecret'
  end
  object Edit2: TEdit
    Left = 84
    Top = 35
    Width = 245
    Height = 21
    TabOrder = 3
    Text = 'Test'
  end
  object Edit3: TEdit
    Left = 84
    Top = 62
    Width = 245
    Height = 21
    TabOrder = 4
    Text = 'Test'
  end
end
