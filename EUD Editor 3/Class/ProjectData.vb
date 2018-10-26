﻿Imports System.IO

Public Class ProjectData
#Region "Filed"
    Private tFilename As String
    Public ReadOnly Property Filename As String
        Get
            Return tFilename
        End Get
    End Property
    Public ReadOnly Property SafeFilename As String
        Get
            Return tFilename.Split("\").Last
        End Get
    End Property

    Public Function IsNewFile() As Boolean
        If tFilename = "" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private tIsLoad As Boolean
    Public ReadOnly Property IsLoad As Boolean
        Get
            Return tIsLoad
        End Get
    End Property

    Private tIsDirty As Boolean
    Public Property IsDirty As Boolean
        Get
            Return tIsDirty
        End Get
        Set(value As Boolean)
            tIsDirty = value
        End Set
    End Property
#End Region


#Region "Member"
    Private mOpenMapName As String
    Public Property OpenMapName As Boolean
        Get
            Return mOpenMapName
        End Get
        Set(value As Boolean)
            mOpenMapName = value
        End Set
    End Property

    Private mSaveMapName As String
    Public Property SaveMapName As Boolean
        Get
            Return mSaveMapName
        End Get
        Set(value As Boolean)
            mSaveMapName = value
        End Set
    End Property
#End Region







    Private UnitTexts() As String 'Tbl상에서의 유닛 이름 배열 tbl이 바뀌면 리셋해줘야 됨
    Public ReadOnly Property UnitName(index As Byte) As String
        Get
            Return index & "미상"
        End Get
    End Property

    Public Sub New()
        '초기화



        InitProject()
    End Sub



    Private Sub InitProject()
        IsDirty = False
        tFilename = ""
    End Sub



    Public Sub NewFIle()
        InitProject()
        tIsLoad = True
        MsgBox("새파일")
    End Sub


    '여기에 모든게 들어간다
    '스타 dat데이터를 클래스로 만들어 관리하자.
    Public Sub Load()
        If Tool.LoadProjectDialog.ShowDialog() = Forms.DialogResult.OK Then
            tFilename = Tool.LoadProjectDialog.FileName '파일 이름 교체
        End If

        Dim fs As New FileStream(tFilename, FileMode.Open)





        fs.Close()



        tIsLoad = True
        MsgBox("로드")
    End Sub



    Public Sub Save(Optional IsSaveAs As Boolean = False)
        If IsSaveAs = True Then '다른이름으로 저장 일 경우 
            If Tool.SaveProjectDialog.ShowDialog() = Forms.DialogResult.OK Then
                tFilename = Tool.SaveProjectDialog.FileName '파일 이름 교체
            End If
        End If
        If tFilename = "" Then ' 새파일
            If Tool.SaveProjectDialog.ShowDialog() = Forms.DialogResult.OK Then
                tFilename = Tool.SaveProjectDialog.FileName '파일 이름 교체
            End If
        End If

        Dim fs As New FileStream(tFilename, FileMode.Create)





        fs.Close()


        IsDirty = False
        MsgBox("세이브")
    End Sub


    Public Sub CloseFile()
        If IsDirty Then '파일이 변형되었을 경우

        End If

        tIsLoad = False
        MsgBox("닫기")
    End Sub

    '일단 코드불러오는거 먼저 하자. stat_txt.bin을 불러오고 그걸 바탕으로 만들자.(이미지나 스프라이트를 제외하고는 한글 이름으로 가능하니까 이미지나 스프라이트는 데이터로 준비)


End Class