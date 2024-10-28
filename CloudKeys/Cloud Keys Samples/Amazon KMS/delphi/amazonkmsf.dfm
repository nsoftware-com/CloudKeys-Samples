object FormAmazonkms: TFormAmazonkms
  Left = 0
  Top = 0
  Caption = 'Amazon KMS Demo'
  ClientHeight = 513
  ClientWidth = 734
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
    Top = 16
    Width = 688
    Height = 39
    Caption = 
      'The Amazon KMS Manager demo is designed to show you how to use t' +
      'he AmazonKMS component to create, manage, and use Customer Maste' +
      'r Keys (CMKs) in Amazon Key Management Service. To begin, provid' +
      'e an access key and secret key to use for authentication, and ch' +
      'oose a region to make requests against.'
    Color = clSkyBlue
    ParentColor = False
    WordWrap = True
  end
  object Panel1: TPanel
    Left = 16
    Top = 65
    Width = 697
    Height = 89
    Alignment = taLeftJustify
    Caption = 'Authentication and Region'
    TabOrder = 0
    VerticalAlignment = taAlignTop
    object Label2: TLabel
      Left = 8
      Top = 27
      Width = 58
      Height = 13
      Caption = 'Access Key:'
    end
    object Label3: TLabel
      Left = 10
      Top = 59
      Width = 56
      Height = 13
      Caption = 'Secret Key:'
    end
    object Label4: TLabel
      Left = 382
      Top = 27
      Width = 37
      Height = 13
      Caption = 'Region:'
    end
    object accessKey: TEdit
      Left = 72
      Top = 24
      Width = 289
      Height = 21
      TabOrder = 0
      Text = ''
      OnChange = accessKeyChange
    end
    object secretKey: TEdit
      Left = 72
      Top = 59
      Width = 291
      Height = 21
      TabOrder = 1
      Text = ''
      OnChange = secretKeyChange
    end
    object region: TEdit
      Left = 425
      Top = 24
      Width = 177
      Height = 21
      TabOrder = 2
      Text = 'us-east-1'
      OnChange = regionChange
    end
  end
  object Panel5: TPanel
    Left = 16
    Top = 160
    Width = 553
    Height = 49
    Alignment = taLeftJustify
    Caption = 'Key Controls'
    TabOrder = 1
    VerticalAlignment = taAlignTop
    object Label5: TLabel
      Left = 214
      Top = 24
      Width = 66
      Height = 13
      Caption = 'Days to Wait:'
    end
    object createKey: TButton
      Left = 16
      Top = 16
      Width = 81
      Height = 25
      Caption = 'Create Key'
      TabOrder = 0
      OnClick = createKeyClick
    end
    object scheduleDelete: TButton
      Left = 103
      Top = 16
      Width = 105
      Height = 25
      Caption = 'Schedule Deletion'
      TabOrder = 1
      OnClick = scheduleDeleteClick
    end
    object Edit4: TEdit
      Left = 286
      Top = 16
      Width = 27
      Height = 21
      TabOrder = 2
      Text = '7'
    end
    object cancelDelete: TButton
      Left = 327
      Top = 16
      Width = 92
      Height = 25
      Caption = 'Cancel Deletion'
      TabOrder = 3
      OnClick = cancelDeleteClick
    end
    object Button7: TButton
      Left = 437
      Top = 16
      Width = 97
      Height = 25
      Caption = 'List Keys'
      TabOrder = 4
      OnClick = Button7Click
    end
  end
  object Panel7: TPanel
    Left = 16
    Top = 224
    Width = 697
    Height = 273
    Alignment = taLeftJustify
    Caption = 'Customer Master Keys'
    TabOrder = 2
    VerticalAlignment = taAlignTop
    object ListView1: TListView
      Left = 8
      Top = 24
      Width = 681
      Height = 241
      Columns = <
        item
          Caption = 'Id'
          MaxWidth = 200
          Width = 200
        end
        item
          Caption = 'Key Spec'
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
          Caption = 'Description'
          MaxWidth = 175
        end>
      TabOrder = 0
      ViewStyle = vsReport
    end
  end
  object ckAmazonKMS1: TckAmazonKMS
    Region = 'us-east-1'
    SSLCertStore = 'MY'
    OnKeyList = ckAmazonKMS1KeyList
    Left = 632
    Top = 168
  end
end


