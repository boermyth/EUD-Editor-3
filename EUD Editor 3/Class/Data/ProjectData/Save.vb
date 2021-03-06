﻿Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Partial Public Class ProjectData
    Private SaveData As SaveableData

    Public Function Save(Optional IsSaveAs As Boolean = False) As Boolean
        SaveData.LastVersion = pgData.Version
        TETempData.SaveTabitems()

        If IsSaveAs = True Then '다른이름으로 저장 일 경우 
            Tool.SaveProjectDialog.FileName = SafeFilename

            Dim exten As String() = Tool.SaveProjectDialog.Filter.Split("|")
            For i = 1 To exten.Count - 1 Step 2
                If Extension = exten(i).Split(".").Last Then
                    Tool.SaveProjectDialog.FilterIndex = ((i - 1) \ 2) + 1
                    Exit For
                End If

            Next




            If Tool.SaveProjectDialog.ShowDialog() = Forms.DialogResult.OK Then
                Filename = Tool.SaveProjectDialog.FileName '파일 이름 교체
            Else
                Return False
            End If
        End If
        If Filename = "" Then ' 새파일
            Tool.SaveProjectDialog.FileName = SafeFilename
            If Tool.SaveProjectDialog.ShowDialog() = Forms.DialogResult.OK Then
                Filename = Tool.SaveProjectDialog.FileName '파일 이름 교체
            Else
                Return False
            End If
        End If


        Dim stm As Stream = File.Open(Filename, FileMode.Create, FileAccess.ReadWrite)
        Dim bf As BinaryFormatter = New BinaryFormatter()
        bf.Serialize(stm, Me.SaveData)
        stm.Close()

        'Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(ProjectData))
        'Dim file As New System.IO.StreamWriter(tFilename)
        'writer.Serialize(file, Me)
        'file.Close()


        'Dim fs As New FileStream(tFilename, FileMode.Create)
        'Dim sw As New StreamWriter(fs)


        'sw.Write(JsonConvert.SerializeObject(pjData))

        'sw.Close()
        'fs.Close()

        tIsLoad = True
        tIsDirty = False
        Return True
    End Function
End Class
