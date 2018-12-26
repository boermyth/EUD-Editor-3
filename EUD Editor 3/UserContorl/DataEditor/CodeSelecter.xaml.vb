﻿Imports System.ComponentModel

Public Class CodeSelecter
    Private factoryPanel As FrameworkElementFactory = New FrameworkElementFactory(GetType(WrapPanel))
    Private Templat As ItemsPanelTemplate = New ItemsPanelTemplate()



    Public Event ListSelect As RoutedEventHandler


    Private Sub List_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim es As SelectionChangedEventArgs = e
        If es.AddedItems.Count <> 0 Then
            Dim index As Integer

            Dim tempItem As ListBoxItem = es.AddedItems(0)
            index = tempItem.Tag

            Dim returnval() As Integer = {CurrentPage, index}

            RaiseEvent ListSelect(returnval, e)
        End If
    End Sub
    Private Sub CodeIndexerTree_SelectedItemChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Object))
        If e.NewValue IsNot Nothing Then
            Dim selectNode As TreeViewItem = e.NewValue

            If selectNode.Items.Count = 0 Then
                Dim returnval() As Integer = {CurrentPage, selectNode.Tag}
                RaiseEvent ListSelect(returnval, e)
            End If
        End If

        ' RaiseEvent ListSelect(sender, e)
    End Sub



    Public Sub New()

        ' 디자이너에서 이 호출이 필요합니다.
        InitializeComponent()

        ' InitializeComponent() 호출 뒤에 초기화 코드를 추가하세요.
        Fliter = New tFliter
        factoryPanel.SetValue(WrapPanel.IsItemsHostProperty, True)
        Templat.VisualTree = factoryPanel
    End Sub

    Private Structure UnitName
        Public Name As String
        Public index As Integer

        Public Sub New(tname As String, tindex As Integer)
            Name = tname
            index = tindex
        End Sub
    End Structure

    Private CurrentPage As EPageType
    Public Enum EPageType
        Unit = 0
        Weapon = 1
        Fligy = 2
        Sprite = 3
        Image = 4
        Upgrade = 5
        Tech = 6
        Order = 7
        ButtonSet = 11
        Nottting

    End Enum
    Public Enum ESortType
        ABC
        Tree
        n123
    End Enum
    Private Fliter As tFliter
    Private Structure tFliter
        Public fliterText As String

        Public IsEdit As Boolean
        Public IsIcon As Boolean
        Private TSortType As ESortType
        Public ReadOnly Property SortType As ESortType
            Get
                Return TSortType
            End Get
        End Property




        Public Sub SetFliter(type As ESortType)
            TSortType = type
        End Sub
    End Structure
    Public Sub SetFliter(tfliter As ESortType)


        Fliter.SetFliter(tfliter)
    End Sub

    Public Sub ListReset(Optional pagetype As EPageType = EPageType.Nottting)
        If pagetype = EPageType.Nottting Then
            pagetype = CurrentPage
        Else
            CurrentPage = pagetype
        End If

        Select Case Fliter.SortType
            Case ESortType.n123, ESortType.ABC
                If Fliter.IsIcon Then
                    CodeIndexerImage.Visibility = Visibility.Visible
                    CodeIndexerTree.Visibility = Visibility.Hidden
                    CodeIndexerList.Visibility = Visibility.Hidden
                Else
                    CodeIndexerImage.Visibility = Visibility.Hidden
                    CodeIndexerTree.Visibility = Visibility.Hidden
                    CodeIndexerList.Visibility = Visibility.Visible
                End If
            Case ESortType.Tree
                CodeIndexerImage.Visibility = Visibility.Hidden
                CodeIndexerTree.Visibility = Visibility.Visible
                CodeIndexerList.Visibility = Visibility.Hidden
        End Select



        ListResetData(pagetype)




    End Sub
    Private Function GetIcon(iconIndex As Integer, isSizeBig As Boolean) As Border
        Dim imgSource As ImageSource = scData.GetIcon(iconIndex, False)
        Dim bitmap As New Image
        bitmap.BeginInit()
        bitmap.Source = imgSource
        If isSizeBig Then
            bitmap.Width = 56
            bitmap.Height = 56
        Else
            bitmap.Width = 30
            bitmap.Height = 30
        End If
        bitmap.EndInit()

        Dim tborder As New Border
        tborder.Child = bitmap
        tborder.Background = Brushes.Black
        If isSizeBig Then
            tborder.Margin = New Thickness(-7)
        Else
            tborder.Margin = New Thickness(-10)
        End If

        Return tborder
    End Function
    Private Function GetImage(grpIndex As Integer, isSizeBig As Boolean) As Border
        Dim imgSource As ImageSource = scData.GetGRP(grpIndex, 37, False)
        Dim bitmap As New Image
        bitmap.BeginInit()
        bitmap.Source = imgSource
        If isSizeBig Then
            bitmap.Width = 56
            bitmap.Height = 56
        Else
            bitmap.Width = 30
            bitmap.Height = 30
        End If
        bitmap.EndInit()

        Dim tborder As New Border
        tborder.Child = bitmap
        tborder.Background = Brushes.Black
        If isSizeBig Then
            tborder.Margin = New Thickness(-7)
        Else
            tborder.Margin = New Thickness(-10)
        End If

        Return tborder
    End Function

    Private Sub ListResetData(pagetype As EPageType)
        '리스트에 들어갈 거는 리스트이름과 아이콘, 그룹패스뿐임.
        '이것만 잘 넘겨주면 됨
        Dim ObjectNames As New List(Of String)
        Dim ObjectImages As New List(Of Border)

        Select Case pagetype
            Case EPageType.Unit
                For i = 0 To SCUnitCount - 1
                    Dim tGraphics As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.units, "Graphics", i)
                    Dim tSprite As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.flingy, "Sprite", tGraphics)
                    Dim timage As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", tSprite)

                    ObjectNames.Add(pjData.CodeLabel(pagetype, i))
                    ObjectImages.Add(GetImage(timage, Fliter.IsIcon))
                Next
            Case EPageType.Weapon
                For i = 0 To SCWeaponCount - 1
                    Dim tLabel As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.weapons, "Label", i) - 1
                    Dim tIcon As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.weapons, "Icon", i)

                    Dim tooltip As String = pjData.Dat.ToolTip(pagetype, i)
                    Dim cname As String

                    If tooltip = "" Then
                        cname = pjData.CodeLabel(pagetype, i)
                    Else
                        cname = pjData.CodeLabel(pagetype, i) & " (" & tooltip & ")"
                    End If

                    ObjectNames.Add(cname)
                    ObjectImages.Add(GetIcon(tIcon, Fliter.IsIcon))
                Next
            Case EPageType.Fligy
                For i = 0 To SCFlingyCount - 1
                    Dim tSprite As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.flingy, "Sprite", i)
                    Dim timage As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", tSprite)

                    Dim tooltip As String = pjData.Dat.ToolTip(pagetype, i)
                    Dim cname As String

                    If tooltip = "" Then
                        cname = pjData.CodeLabel(pagetype, i)
                    Else
                        cname = pjData.CodeLabel(pagetype, i) & " (" & tooltip & ")"
                    End If

                    ObjectNames.Add(cname)
                    ObjectImages.Add(GetImage(timage, Fliter.IsIcon))
                Next
            Case EPageType.Sprite
                For i = 0 To SCSpriteCount - 1
                    Dim timage As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", i)

                    Dim tooltip As String = pjData.Dat.ToolTip(pagetype, i)
                    Dim cname As String

                    If tooltip = "" Then
                        cname = pjData.CodeLabel(pagetype, i)
                    Else
                        cname = pjData.CodeLabel(pagetype, i) & " (" & tooltip & ")"
                    End If

                    ObjectNames.Add(cname)
                    ObjectImages.Add(GetImage(timage, Fliter.IsIcon))
                Next
            Case EPageType.Image
                For i = 0 To SCImageCount - 1
                    Dim tooltip As String = pjData.Dat.ToolTip(pagetype, i)
                    Dim cname As String

                    If tooltip = "" Then
                        cname = pjData.CodeLabel(pagetype, i)
                    Else
                        cname = pjData.CodeLabel(pagetype, i) & " (" & tooltip & ")"
                    End If

                    ObjectNames.Add(cname)
                    ObjectImages.Add(GetImage(i, Fliter.IsIcon))
                Next
            Case EPageType.Upgrade
                For i = 0 To SCUpgradeCount - 1
                    Dim tIcon As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.upgrades, "Icon", i)

                    Dim tooltip As String = pjData.Dat.ToolTip(pagetype, i)
                    Dim cname As String

                    If tooltip = "" Then
                        cname = pjData.CodeLabel(pagetype, i)
                    Else
                        cname = pjData.CodeLabel(pagetype, i) & " (" & tooltip & ")"
                    End If

                    ObjectNames.Add(cname)
                    ObjectImages.Add(GetIcon(tIcon, Fliter.IsIcon))
                Next
            Case EPageType.Tech
                For i = 0 To SCTechCount - 1
                    Dim tIcon As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.techdata, "Icon", i)

                    Dim tooltip As String = pjData.Dat.ToolTip(pagetype, i)
                    Dim cname As String

                    If tooltip = "" Then
                        cname = pjData.CodeLabel(pagetype, i)
                    Else
                        cname = pjData.CodeLabel(pagetype, i) & " (" & tooltip & ")"
                    End If

                    ObjectNames.Add(cname)
                    ObjectImages.Add(GetIcon(tIcon, Fliter.IsIcon))
                Next
            Case EPageType.Order
                For i = 0 To SCOrderCount - 1
                    Dim tIcon As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.orders, "Highlight", i)

                    Dim tooltip As String = pjData.Dat.ToolTip(pagetype, i)
                    Dim cname As String

                    If tooltip = "" Then
                        cname = pjData.CodeLabel(pagetype, i)
                    Else
                        cname = pjData.CodeLabel(pagetype, i) & " (" & tooltip & ")"
                    End If

                    ObjectNames.Add(cname)
                    ObjectImages.Add(GetIcon(tIcon, Fliter.IsIcon))
                Next
            Case EPageType.ButtonSet
                '어캐작성하노 ㅋㅋ 
                For i = 0 To SCUnitCount - 1
                    Dim tGraphics As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.units, "Graphics", i)
                    Dim tSprite As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.flingy, "Sprite", tGraphics)
                    Dim timage As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", tSprite)

                    ObjectNames.Add(pjData.CodeLabel(pagetype, i))
                    ObjectImages.Add(GetImage(timage, Fliter.IsIcon))
                Next
        End Select
        Select Case Fliter.SortType
            Case ESortType.n123, ESortType.ABC
                Dim tList As New List(Of UnitName)
                If Fliter.SortType = ESortType.ABC Then
                    For i = 0 To ObjectNames.Count - 1
                        tList.Add(New UnitName(ObjectNames(i), i))
                    Next
                    Try
                        tList.Sort(Function(x, y) x.Name.CompareTo(y.Name))
                    Catch ex As Exception

                    End Try

                End If

                If Fliter.IsIcon Then
                    CodeIndexerImage.Items.Clear()

                    For i = 0 To ObjectNames.Count - 1
                        Dim index As Integer
                        If Fliter.SortType = ESortType.ABC Then
                            index = tList(i).index
                        Else
                            index = i
                        End If

                        Dim tListItem As New ListBoxItem()
                        tListItem.Tag = index
                        tListItem.Content = ObjectImages(index)

                        CodeIndexerImage.Items.Add(tListItem)
                    Next
                Else
                    CodeIndexerList.Items.Clear()
                    For i = 0 To ObjectNames.Count - 1
                        Dim index As Integer
                        If Fliter.SortType = ESortType.ABC Then
                            index = tList(i).index
                        Else
                            index = i
                        End If


                        Dim unitname As String = ObjectNames(index)  '"[" & Format(i, "000") & "]  " & pjData.UnitName(i)

                        Dim textblock As New TextBlock
                        textblock.Text = unitname
                        textblock.Padding = New Thickness(15, 0, 0, 0)

                        Dim stackpanel As New StackPanel
                        stackpanel.Orientation = Orientation.Horizontal
                        stackpanel.Children.Add(ObjectImages(index))
                        stackpanel.Children.Add(textblock)

                        Dim tListItem As New ListBoxItem()
                        tListItem.Tag = index
                        tListItem.Content = stackpanel
                        If index = 10 Then
                            tListItem.Background = Brushes.PaleVioletRed
                        End If

                        CodeIndexerList.Items.Add(tListItem)
                    Next
                End If

            Case ESortType.Tree
                CodeIndexerTree.Items.Clear()


                CodeIndexerTree.BeginInit()
                For i = 0 To ObjectNames.Count - 1
                    Dim Codename As String = ObjectNames(i)
                    Dim CodeGroup As String
                    If CurrentPage = EPageType.ButtonSet Then
                        CodeGroup = ""
                    Else
                        CodeGroup = pjData.Dat.Group(CurrentPage, i)
                    End If

                    AddTreeList(CodeIndexerTree, CodeGroup, Codename, ObjectImages(i), Fliter.IsIcon, i)


                    'Dim tGraphics As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.units, "Graphics", i)
                    'Dim tSprite As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.flingy, "Sprite", tGraphics)
                    'Dim timage As Integer = pjData.Dat.Data(SCDatFiles.DatFiles.sprites, "Image File", tSprite)

                    'Dim textblock As New TextBlock
                    'textblock.Text = unitname
                    'textblock.Padding = New Thickness(15, 0, 0, 0)

                    'Dim stackpanel As New StackPanel
                    'stackpanel.Orientation = Orientation.Horizontal
                    'stackpanel.Children.Add(GetImage(timage, Fliter.IsIcon))
                    'stackpanel.Children.Add(textblock)


                    'Dim tListItem As New TreeViewItem()
                    'tListItem.Tag = i
                    'tListItem.Header = stackpanel


                    ''MsgBox(Flags(1))

                    'Dim flag As Byte = scData.DefaultDat.Data(SCDatFiles.DatFiles.units, "Staredit Group Flags", i)

                    'Dim Race As Byte
                    'If (flag And 1) = 1 Then '저그
                    '    Race = 0
                    'ElseIf (flag And 2) = 2 Then '테란
                    '    Race = 1
                    'ElseIf (flag And 4) = 4 Then '프로토스
                    '    Race = 2
                    'ElseIf (flag And &H80) = &H80 Then '자연
                    '    Race = 3
                    'Else '미분류
                    '    Race = 4
                    'End If
                    'If 0 <= Race And Race <= 2 Then
                    '    If Flags(1) = "*" Then '일반 유닛
                    '        If Flags(2) = "Zerg" Or Flags(2) = "Protoss" Or Flags(2) = "Addons" Then
                    '            CodeIndexerTree.Items(Race).items(7).items.Add(tListItem)
                    '        ElseIf Flags(2) = "Terran" Then
                    '            CodeIndexerTree.Items(Race).items(8).items.Add(tListItem)
                    '        Else
                    '            CodeIndexerTree.Items(Race).items(TypeKeyname.ToList.IndexOf(Flags(2))).items.Add(tListItem)
                    '        End If
                    '    Else
                    '        'textblock.Text = unitname & "(" & Flags(1) & ")"
                    '        CodeIndexerTree.Items(Race).items(3).items.Add(tListItem)
                    '    End If
                    'ElseIf Race = 3 Then
                    '    If Flags(1) = "*" Then '일반 유닛
                    '        If Flags(2) = "Zerg" Then
                    '            CodeIndexerTree.Items(Race).items(7).items.Add(tListItem)
                    '        ElseIf Flags(2) = "Protoss" Then
                    '            CodeIndexerTree.Items(Race).items(8).items.Add(tListItem)
                    '        Else
                    '            CodeIndexerTree.Items(Race).items(TypeNetKeyname.ToList.IndexOf(Flags(2))).items.Add(tListItem)
                    '        End If
                    '    Else '크리쳐 또는 자원
                    '        If Flags(2) = "Critters" Then
                    '            'textblock.Text = unitname & "(" & Flags(1) & ")"
                    '            CodeIndexerTree.Items(Race).items(0).items.Add(tListItem)
                    '        Else
                    '            'textblock.Text = unitname & "(" & Flags(1) & ")"
                    '            CodeIndexerTree.Items(Race).items(3).items.Add(tListItem)
                    '        End If

                    '    End If
                    'Else
                    '    CodeIndexerTree.Items(Race).items.Add(tListItem)
                    'End If

                Next
                CodeIndexerTree.EndInit()
        End Select
    End Sub


    Private Sub AddTreeList(tv As TreeView, itemPath As String, itemName As String, timage As Border, Isbig As Boolean, index As Integer)
        Dim groups As String() = itemPath.Split({"\"}, StringSplitOptions.RemoveEmptyEntries)

        Dim ItempColl As ItemCollection = tv.Items
        Dim SelectTreeitem As TreeViewItem = Nothing

        Dim PassGroup As Boolean = False


        For gindex = 0 To groups.Count - 1
            PassGroup = False
            For i = 0 To ItempColl.Count - 1
                Dim ttreeitem As TreeViewItem = ItempColl(i)
                If groups(gindex) = ttreeitem.Header.ToString Then
                    SelectTreeitem = ttreeitem
                    ItempColl = SelectTreeitem.Items
                    PassGroup = True
                    Exit For
                End If
            Next
            If Not PassGroup Then
                Dim ttreeitem As New TreeViewItem()

                ttreeitem.Header = groups(gindex)
                ItempColl.Add(ttreeitem)

                SelectTreeitem = ttreeitem
                ItempColl = SelectTreeitem.Items
            End If
        Next

        '만약 
        If Isbig Then
            If ItempColl.Count = 0 Then
                If SelectTreeitem Is Nothing Then
                    tv.ItemsPanel = Templat
                Else
                    SelectTreeitem.ItemsPanel = Templat
                End If
            End If

            Dim CodeItem As New TreeViewItem()



            CodeItem.Header = timage
            CodeItem.Margin = New Thickness(0, 0, -32, 0)

            CodeItem.Tag = index
            ItempColl.Add(CodeItem)

        Else
            Dim textblock As New TextBlock
            textblock.Text = itemName
            textblock.Padding = New Thickness(15, 0, 0, 0)

            Dim stackpanel As New StackPanel
            stackpanel.Orientation = Orientation.Horizontal
            stackpanel.Children.Add(timage)
            stackpanel.Children.Add(textblock)

            Dim CodeItem As New TreeViewItem()


            CodeItem.Header = stackpanel
            CodeItem.Tag = index
            ItempColl.Add(CodeItem)
        End If




    End Sub





    Private Sub Btn_sortn123(sender As Object, e As RoutedEventArgs)
        SetFliter(ESortType.n123)
        ListReset()
    End Sub
    Private Sub Btn_sortABC(sender As Object, e As RoutedEventArgs)
        SetFliter(ESortType.ABC)
        ListReset()
    End Sub

    Private Sub Btn_sortTree(sender As Object, e As RoutedEventArgs)
        SetFliter(ESortType.Tree)
        ListReset()
    End Sub

    Private Sub ToolBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Fliter.IsIcon = L_IconBtn.IsSelected
        Fliter.IsEdit = L_IsEditBtn.IsSelected

        ListReset()
    End Sub


End Class

