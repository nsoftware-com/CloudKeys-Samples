object FormAzuresecrets: TFormAzuresecrets
  Left = 0
  Top = 0
  Caption = 'Azure Secrets Demo'
  ClientHeight = 416
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
    Width = 905
    Height = 26
    Caption = 
      'The Azure Secrets Manager demo is designed to show you how to us' +
      'e the AzureSecrets component to create, manage, and use secrets ' +
      'in Azure Secret Vault. To begin, log in using OAuth, and specify' +
      ' the name of the Azure Secret Vault to make requests against.'
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
    Caption = 'Secret Controls'
    TabOrder = 1
    VerticalAlignment = taAlignTop
    object createSecret: TButton
      Left = 16
      Top = 16
      Width = 81
      Height = 25
      Caption = 'Create Secret'
      TabOrder = 0
      OnClick = createSecretClick
    end
    object deleteSecret: TButton
      Left = 103
      Top = 16
      Width = 105
      Height = 25
      Caption = 'Delete Secret'
      TabOrder = 1
      OnClick = deleteSecretClick
    end
    object listSecrets: TButton
      Left = 214
      Top = 16
      Width = 97
      Height = 25
      Caption = 'List Secrets'
      TabOrder = 2
      OnClick = listSecretsClick
    end
  end
  object Panel7: TPanel
    Left = 16
    Top = 104
    Width = 578
    Height = 305
    Alignment = taLeftJustify
    Caption = 'Secrets'
    TabOrder = 2
    VerticalAlignment = taAlignTop
    object ListView1: TListView
      Left = 8
      Top = 24
      Width = 561
      Height = 273
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
        end>
      ColumnClick = False
      HideSelection = False
      RowSelect = True
      TabOrder = 0
      ViewStyle = vsReport
      OnClick = ListView1Click
    end
  end
  object Panel2: TPanel
    Left = 600
    Top = 104
    Width = 325
    Height = 305
    Alignment = taLeftJustify
    Caption = 'Secret Details'
    TabOrder = 3
    VerticalAlignment = taAlignTop
    object secretDetails: TMemo
      Left = 8
      Top = 24
      Width = 305
      Height = 273
      ReadOnly = True
      TabOrder = 0
    end
  end
  object ckAzureSecrets1: TckAzureSecrets
    SSLCertStore = 'MY'
    OnSecretList = ckAzureSecrets1SecretList
    Left = 480
    Top = 88
  end
end


