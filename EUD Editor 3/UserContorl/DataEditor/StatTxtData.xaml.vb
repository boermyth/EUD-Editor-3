﻿Public Class StatTxtData
    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.orders

    Public Property ObjectID As Integer


    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = pjData
        ObjectID = tObjectID

        'NameBar.Init(ObjectID, DatFiles, 0)
    End Sub
    Public Sub ReLoad(DatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        ObjectID = ObjectID
        'NameBar.ReLoad(ObjectID, DatFiles, 0)
    End Sub
End Class