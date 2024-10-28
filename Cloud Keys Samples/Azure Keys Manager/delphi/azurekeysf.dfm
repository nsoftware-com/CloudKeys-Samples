object FormAzurekeys: TFormAzurekeys
  Left = 0
  Top = 0
  Caption = 'Azure Keys Demo'
  ClientHeight = 613
  ClientWidth = 933
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 16
    Top = 16
    Width = 908
    Height = 26
    Caption = 
      'The Azure Keys Manager demo is designed to show you how to use t' +
      'he AzureKeys component to create, manage, and use keys in Azure ' +
      'Key Vault. To begin, log in using OAuth, and specify the name of' +
      ' the Azure Key Vault to make requests against.'
    Color = clBtnFace
    ParentColor = False
    WordWrap = True
  end
  object Panel1: TPanel
    Left = 16
    Top = 48
    Width = 578
    Height = 50
    Alignment = taLeftJustify
    Caption = 'Authentication and Vault'
    TabOrder = 0
    VerticalAlignment = taAlignTop
    object Label2: TLabel
      Left = 89
      Top = 20
      Width = 58
      Height = 13
      Caption = 'Vault Name:'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clWindowText
      Font.Height = -11
      Font.Name = 'Tahoma'
      Font.Style = []
      ParentFont = False
    end
    object logIn: TButton
      Left = 8
      Top = 16
      Width = 75
      Height = 25
      Caption = 'Log In'
      TabOrder = 0
      OnClick = logInClick
    end
    object vaultEdit: TEdit
      Left = 153
      Top = 16
      Width = 416
      Height = 21
      TabOrder = 1
      OnChange = vaultEditChange
    end
  end
  object Panel5: TPanel
    Left = 600
    Top = 48
    Width = 324
    Height = 50
    Alignment = taLeftJustify
    Caption = 'Key Controls'
    TabOrder = 1
    VerticalAlignment = taAlignTop
    object createKey: TButton
      Left = 16
      Top = 16
      Width = 81
      Height = 25
      Caption = 'Create Key'
      TabOrder = 0
      OnClick = createKeyClick
    end
    object deleteKey: TButton
      Left = 103
      Top = 16
      Width = 105
      Height = 25
      Caption = 'Delete Key'
      TabOrder = 1
      OnClick = deleteKeyClick
    end
    object listKeys: TButton
      Left = 214
      Top = 16
      Width = 97
      Height = 25
      Caption = 'List Keys'
      TabOrder = 2
      OnClick = listKeysClick
    end
  end
  object Panel7: TPanel
    Left = 16
    Top = 104
    Width = 908
    Height = 273
    Alignment = taLeftJustify
    Caption = 'Keys'
    TabOrder = 2
    VerticalAlignment = taAlignTop
    object ListView1: TListView
      Left = 8
      Top = 24
      Width = 889
      Height = 241
      Columns = <
        item
          Caption = 'Name'
          MaxWidth = 200
          Width = 200
        end
        item
          Caption = 'Version Id'
          MaxWidth = 125
          Width = 125
        end
        item
          Caption = 'Type'
          MaxWidth = 75
          Width = 75
        end
        item
          Caption = 'State'
          MaxWidth = 110
          Width = 110
        end
        item
          AutoSize = True
          Caption = 'Supported Operations'
          MaxWidth = 175
        end>
      ColumnClick = False
      HideSelection = False
      RowSelect = True
      TabOrder = 0
      ViewStyle = vsReport
    end
  end
  object Panel2: TPanel
    Left = 16
    Top = 383
    Width = 909
    Height = 218
    Alignment = taLeftJustify
    Caption = 'Cryptographic Operations'
    TabOrder = 3
    VerticalAlignment = taAlignTop
    object Label3: TLabel
      Left = 8
      Top = 21
      Width = 654
      Height = 13
      Caption = 
        'The controls below allow you to perform any cryptographic operat' +
        'ions supported by the currently-selected key (assuming it is ena' +
        'bled). '
    end
    object Label4: TLabel
      Left = 327
      Top = 53
      Width = 103
      Height = 13
      Caption = 'Encryption Algorithm:'
    end
    object Label5: TLabel
      Left = 327
      Top = 130
      Width = 98
      Height = 13
      Caption = 'Signature Algorithm:'
    end
    object Panel3: TPanel
      Left = 8
      Top = 40
      Width = 313
      Height = 169
      Alignment = taLeftJustify
      Caption = 'Plaintext / Message to Sign'
      TabOrder = 0
      VerticalAlignment = taAlignTop
    end
    object Panel4: TPanel
      Left = 582
      Top = 40
      Width = 315
      Height = 169
      Alignment = taLeftJustify
      Caption = 'Encrypted Data / Signature Data'
      TabOrder = 1
      VerticalAlignment = taAlignTop
    end
    object encryptionAlgorithm: TComboBox
      Left = 327
      Top = 72
      Width = 249
      Height = 21
      TabOrder = 2
      Text = 'RSA1_5'
      Items.Strings = (
        'RSA1_5'
        'RSA-OAEP'
        'RSA-OAEP-256')
    end
    object encryptData: TButton
      Left = 327
      Top = 99
      Width = 249
      Height = 25
      Caption = '--> Encrypt Data -->'
      TabOrder = 3
      OnClick = encryptDataClick
    end
    object signatureAlgorithm: TComboBox
      Left = 327
      Top = 149
      Width = 249
      Height = 21
      TabOrder = 4
      Text = 'RS256'
      Items.Strings = (
        'ES256'
        'ES256K'
        'ES384'
        'ES512'
        'PS256'
        'PS384'
        'PS512'
        'RS256'
        'RS384'
        'RS512')
    end
    object signData: TButton
      Left = 327
      Top = 176
      Width = 249
      Height = 25
      Caption = '--> Sign Data -->'
      TabOrder = 5
      OnClick = signDataClick
    end
    object plaintext: TMemo
      Left = 8
      Top = 56
      Width = 313
      Height = 153
      Lines.Strings = (
        'This is some test data.')
      TabOrder = 6
    end
    object data: TMemo
      Left = 582
      Top = 56
      Width = 315
      Height = 153
      TabOrder = 7
    end
  end
  object ckAzureKeys1: TckAzureKeys
    SSLCertStore = 'MY'
    OnKeyList = ckAzureKeys1KeyList
    Left = 808
    Top = 383
  end
end


