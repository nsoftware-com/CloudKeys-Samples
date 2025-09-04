object FormCreatekey: TFormCreatekey
  Left = 0
  Top = 0
  Caption = 'Create Key'
  ClientHeight = 108
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
    Width = 49
    Height = 13
    Caption = 'Key Type:'
  end
  object CheckBox1: TCheckBox
    Left = 16
    Top = 72
    Width = 113
    Height = 17
    Caption = 'Encrypt / Decrypt'
    Checked = True
    ParentShowHint = False
    ShowHint = False
    State = cbChecked
    TabOrder = 0
  end
  object Button1: TButton
    Left = 254
    Top = 68
    Width = 75
    Height = 25
    Caption = 'OK'
    Default = True
    ModalResult = 1
    TabOrder = 1
  end
  object ComboBox1: TComboBox
    Left = 80
    Top = 35
    Width = 249
    Height = 21
    TabOrder = 2
    Text = 'RSA_2048'
    Items.Strings = (
      'RSA_2048'
      'RSA_HSM_2048'
      'RSA_3072'
      'RSA_HSM_3072'
      'RSA_4096'
      'RSA_HSM_4096'
      'EC_P256'
      'EC_HSM_P256'
      'EC_P256K'
      'EC_HSM_P256K'
      'EC_P384'
      'EC_HSM_P384'
      'EC_P521'
      'EC_HSM_P521')
  end
  object Button2: TButton
    Left = 173
    Top = 68
    Width = 75
    Height = 25
    Caption = 'Cancel'
    ModalResult = 2
    TabOrder = 3
  end
  object Edit1: TEdit
    Left = 80
    Top = 8
    Width = 249
    Height = 21
    TabOrder = 4
    Text = 'TestKey'
  end
end
