﻿Public Class StatusInforData
    Private Const UnitDatPage As Integer = 6

    Private DatFiles As SCDatFiles.DatFiles = SCDatFiles.DatFiles.statusinfor

    Public Property ObjectID As Integer


    Public Sub New(tObjectID As Integer)
        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        DataContext = pjData
        ObjectID = tObjectID

        NameBar.Init(ObjectID, SCDatFiles.DatFiles.units, UnitDatPage)

        JOINER.Init(DatFiles, ObjectID, JOINER.Tag)
        ST.Init(DatFiles, ObjectID, ST.Tag)
        DI.Init(DatFiles, ObjectID, DI.Tag)

        BS.Init(SCDatFiles.DatFiles.ButtonSet, ObjectID, BS.Tag)
        WI.Init(SCDatFiles.DatFiles.wireframe, ObjectID, WI.Tag)
        GR.Init(SCDatFiles.DatFiles.wireframe, ObjectID, GR.Tag)
        TR.Init(SCDatFiles.DatFiles.wireframe, ObjectID, TR.Tag)
    End Sub
    Public Sub ReLoad(tDatFiles As SCDatFiles.DatFiles, ObjectID As Integer)
        ObjectID = ObjectID

        NameBar.ReLoad(ObjectID, SCDatFiles.DatFiles.units, UnitDatPage)

        JOINER.ReLoad(DatFiles, ObjectID, JOINER.Tag)
        ST.ReLoad(DatFiles, ObjectID, ST.Tag)
        DI.ReLoad(DatFiles, ObjectID, DI.Tag)


        BS.ReLoad(SCDatFiles.DatFiles.ButtonSet, ObjectID, BS.Tag)
        WI.ReLoad(SCDatFiles.DatFiles.wireframe, ObjectID, WI.Tag)
        GR.ReLoad(SCDatFiles.DatFiles.wireframe, ObjectID, GR.Tag)
        TR.ReLoad(SCDatFiles.DatFiles.wireframe, ObjectID, TR.Tag)
    End Sub
End Class
