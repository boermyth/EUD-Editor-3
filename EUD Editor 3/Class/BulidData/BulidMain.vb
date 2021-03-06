﻿Imports System.IO
Imports System.Media
Imports System.Text

Partial Public Class BuildData

    '1. 일단 내장된 파일을 빌드할때 뺴내는 형식에서
    '파일을 내장하지 않고 외부에 빼놓는다
    '2. 외부에 빼놓은 파일을 사용하지 않고 플립설치폴더나 직접 위치를 지정해 해당 플러그인을 기본으로 사용가능

    '3. 임시파일을 어디다가 생성할지 결정, 폴더가 없을 경우 기본 폴더에 저장
    '맵 파일 위치와 동일한 부분으로 설정 할 경우 맵 파일들이 자동으로 상대경로로 저장!

    '임시파일 지정 방식 설정이 사용자 지정일 경우
    '만약 지정한 폴더가 맵폴더와 같으면 상대경로로 저장한다.

    '맵폴더로 지정되어 있으면 맵두개의 폴더가 같으면 상대 경로로 저장
    '맵 두개가 다른 폴더면 기본 폴더에 저장
    Private MainThread As Threading.Thread
    Public Sub Build()
        If pjData.Filename <> "" Then ' 새파일
            pjData.Save()
        End If



        'Tool.GetRelativePath(EudPlibFilePath & "\EUDEditor.eds", pjData.OpenMapName)
        'Tool.GetRelativePath("zzz\asd\c\ㅎㅎ.txt", "zzz\asd\bcx\aqw\zxv\하이.txt")
        'Tool.GetRelativePath("zzz\asd\bcx\aqw\zxv\하이.txt", "zzz\asd\c\ㅎㅎ.txt")

        If CheckBuildable() Then
            MainThread = New System.Threading.Thread(AddressOf BuildProgress)
            MainThread.Start()
        End If
        'MsgBox("최종 폴더 :" & TempFloder)
    End Sub


    '빌드가 가능한지 판단(필수 프로그램 연결)
    Private Function CheckBuildable() As Boolean
        If Not My.Computer.FileSystem.FileExists(pjData.OpenMapName) Then
            If MsgBox(Tool.GetText("Error OpenMap is not exist reset"), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                If Not Tool.OpenMapSet Then
                    Tool.ErrorMsgBox(Tool.GetText("Error complieFail OpenMap is not exist!"))
                    Return False
                End If
                Tool.RefreshMainWindow()
            Else
                Tool.ErrorMsgBox(Tool.GetText("Error complieFail OpenMap is not exist!"))
                Return False
            End If
        End If
        If Not My.Computer.FileSystem.DirectoryExists(pjData.SaveMapdirectory) Then
            If MsgBox(Tool.GetText("Error SaveMap is not exist reset"), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                If Not Tool.SaveMapSet Then
                    Tool.ErrorMsgBox(Tool.GetText("Error complieFail SaveMap is not exist!"))
                    Return False
                End If
                Tool.RefreshMainWindow()
            Else
                Tool.ErrorMsgBox(Tool.GetText("Error complieFail SaveMap is not exist!"))
                Return False
            End If
        End If
        If Not My.Computer.FileSystem.FileExists(pgData.Setting(ProgramData.TSetting.euddraft)) Then
            If MsgBox(Tool.GetText("Error euddraft is not exist reset"), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                Dim opendialog As New System.Windows.Forms.OpenFileDialog With {
                .Filter = "euddraft.exe|euddraft.exe",
                .FileName = "euddraft.exe",
                .Title = Tool.GetText("euddraftExe Select")
            }


                If opendialog.ShowDialog() = Forms.DialogResult.OK Then
                    pgData.Setting(ProgramData.TSetting.euddraft) = opendialog.FileName
                    Tool.RefreshMainWindow()
                Else
                    Tool.ErrorMsgBox(Tool.GetText("Error complieFail euddraft is not exist!"))
                    Return False
                End If
            Else
                Tool.ErrorMsgBox(Tool.GetText("Error complieFail euddraft is not exist!"))
                Return False
            End If
        End If
        If pjData.TempFileLoc <> "0" And pjData.TempFileLoc <> "1" Then
            If pjData.TempFileLoc = "2" Then
                If Not My.Computer.FileSystem.FileExists(pjData.Filename) Then
                    Tool.ErrorMsgBox(Tool.GetText("Error complieFail NotSaved Project"))
                    Return False
                End If
            Else
                If Not My.Computer.FileSystem.DirectoryExists(pjData.TempFileLoc) Then
                    If MsgBox(Tool.GetText("Error TempFolder is not exist reset"), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                        Dim folderSelect As New System.Windows.Forms.FolderBrowserDialog

                        If folderSelect.ShowDialog = Forms.DialogResult.OK Then
                            pjData.TempFileLoc = folderSelect.SelectedPath
                        Else
                            Tool.ErrorMsgBox(Tool.GetText("Error complieFail TempFolder is not exist!"))
                            Return False
                        End If

                        If Not My.Computer.FileSystem.DirectoryExists(pjData.TempFileLoc) Then
                            Tool.ErrorMsgBox(Tool.GetText("Error complieFail TempFolder is not exist!"))
                            Return False
                        End If
                    Else
                        Tool.ErrorMsgBox(Tool.GetText("Error complieFail TempFolder is not exist!"))
                        Return False
                    End If
                End If
            End If
        End If
        Return True
    End Function


    '########################### 빌드 작업 메인 함수 ############################
    Private Sub BuildProgress()
        '일단 프로그램 못끄게 막고 빌드중이라는 문구와 함께 진행 바 넣어야됨
        pgData.IsCompilng = True
        Tool.RefreshMainWindow()

        '각각의 임시파일들을 만들어야함


        'Req데이터 작성
        WriteRequireData()


        WriteTEFile()


        'Dat설정 파일을 저장하는 py제작
        WriteDatFile()
        WriteExtraDatFile()

        'Tbl파일 작성(옵션에 따라)
        If pjData.UseCustomtbl Then
            WriteTbl()
        End If



        'TE관련 삽입
        'MSQC, 채팅인식, 언리미터, AI스크립트교체
        'CT(Tbl옵션에 따라)

        'eds파일을 만들고 해당 파일 실행
        WriteedsFile()
        Starteds()


        'Threading.Thread.Sleep(3000)



        pgData.IsCompilng = False
        Tool.RefreshMainWindow()

        Dim notificationSound As New SoundPlayer(My.Resources.success)
        notificationSound.PlaySync()
    End Sub





    Private Sub Starteds()
        Dim StandardOutput As String = ""
        Dim StandardError As String = ""

        Dim RestartCount As Integer
        While True
            Dim process As Process = ProcessStart()

            While Not process.HasExited
                process.StandardInput.Write(vbCrLf)
                StandardOutput = process.StandardOutput.ReadToEnd
                StandardError = process.StandardError.ReadToEnd
                Threading.Thread.Sleep(100)
            End While
            If InStr(StandardError, "zipimport.ZipImportError: can't decompress data; zlib not available") <> 0 Then
                'MsgBox("빌드 실패. 재시도 합니다  재시도 횟수: " & RestartCount & vbCrLf & StandardOutput & StandardError)
                StandardError = ""
                RestartCount += 1
                Continue While
            End If
            'Dim tempstr() As String = StandardOutput.Split(vbCrLf)

            'MsgBox(tempstr(tempstr.Count - 2))
            '임시 판단
            If StandardOutput.IndexOf("Output scenario.chk") < 0 Then
                MsgBox(StandardOutput & vbCrLf & StandardError, MsgBoxStyle.Critical)
            End If
            Exit While
        End While
    End Sub
    Private Function ProcessStart() As Process
        Dim process As New Process
        Dim startInfo As New ProcessStartInfo

        Dim filename As String = EdsFilePath

        startInfo.FileName = pgData.Setting(ProgramData.TSetting.euddraft)
        startInfo.Arguments = """" & filename & """"


        'startInfo.StandardOutputEncoding = Text.Encoding.UTF32
        'startInfo.StandardErrorEncoding = Text.Encoding.UTF32
        startInfo.RedirectStandardOutput = True
        startInfo.RedirectStandardInput = True
        startInfo.RedirectStandardError = True
        startInfo.WindowStyle = ProcessWindowStyle.Hidden
        startInfo.CreateNoWindow = True

        startInfo.UseShellExecute = False


        process.StartInfo = startInfo
        process.Start()

        Return process
    End Function


End Class
