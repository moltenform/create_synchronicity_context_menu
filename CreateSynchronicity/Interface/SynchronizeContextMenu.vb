'This file is part of Create Synchronicity.
'
'Create Synchronicity is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
'Create Synchronicity is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
'You should have received a copy of the GNU General Public License along with Create Synchronicity.  If not, see <http://www.gnu.org/licenses/>.
'Created by:	Clément Pit--Claudel.
'Web site:		http://synchronicity.sourceforge.net.


Imports CS

Partial Class SynchronizeForm
    Private Sub ContextMnuLeftCopyPath_Click(sender As Object, e As EventArgs) Handles ContextMnuLeftCopyPath.Click
        If PreviewList.SelectedIndices.Count > 0 Then
            Clipboard.SetText(String.Join(Environment.NewLine, GetFullPathsOfSelectedItems(True).ToArray()))
        End If
    End Sub

    Private Sub ContextMnuRightCopyPath_Click(sender As Object, e As EventArgs) Handles ContextMnuRightCopyPath.Click
        If PreviewList.SelectedIndices.Count > 0 Then
            Clipboard.SetText(String.Join(Environment.NewLine, GetFullPathsOfSelectedItems(False).ToArray()))
        End If
    End Sub

    Private Sub ContextMnuLeftOpen_Click(sender As Object, e As EventArgs) Handles ContextMnuLeftOpen.Click
        ContextMnuOpenFiles(GetFullPathsOfOneSelectedItem(True))
    End Sub

    Private Sub ContextMnuRightOpen_Click(sender As Object, e As EventArgs) Handles ContextMnuRightOpen.Click
        ContextMnuOpenFiles(GetFullPathsOfOneSelectedItem(False))
    End Sub

    Private Sub ContextMnuLeftExplorer_Click(sender As Object, e As EventArgs) Handles ContextMnuLeftExplorer.Click
        ContextMnuShowInExplorer(GetFullPathsOfOneSelectedItem(True))
    End Sub

    Private Sub ContextMnuRightExplorer_Click(sender As Object, e As EventArgs) Handles ContextMnuRightExplorer.Click
        ContextMnuShowInExplorer(GetFullPathsOfOneSelectedItem(False))
    End Sub

    Private Sub ContextMnuCopyPathnames_Click(sender As Object, e As EventArgs) Handles ContextMnuCopyPathnames.Click
        Dim PathnamesLeft As List(Of String) = GetFullPathsOfSelectedItems(True)
        Dim PathnamesRight As List(Of String) = GetFullPathsOfSelectedItems(False)
        If PathnamesLeft.Count = PathnamesRight.Count And PathnamesLeft.Count > 0 Then
            Dim Builder As New Text.StringBuilder()
            For Index As Integer = 0 To PathnamesLeft.Count - 1
                Builder.AppendLine(PathnamesLeft(Index) & "|" & PathnamesRight(Index))
            Next
            Clipboard.SetText(Builder.ToString())
        End If
    End Sub

    Private Function GetFullPathsOfSelectedItems(LeftOrRight As Boolean) As List(Of String)
        Dim Ret As New List(Of String)()
        If PreviewList.SelectedIndices.Count = 0 OrElse Status.ShowingErrors Then Return Ret

        For Each Index As Integer In Me.PreviewList.SelectedIndices
            Ret.Add(If(LeftOrRight, LeftRootPath, RightRootPath) & SyncingList(Index).Path)
        Next
        Return Ret
    End Function

    Private Function GetFullPathsOfOneSelectedItem(LeftOrRight As Boolean) As String
        'When picking just one of the selected items, let's use the focused item because it's the one the user last clicked
        If PreviewList.SelectedIndices.Count = 0 OrElse Status.ShowingErrors Then
            Return Nothing
        ElseIf PreviewList.FocusedItem IsNot Nothing AndAlso
                PreviewList.SelectedIndices.Contains(PreviewList.FocusedItem.Index) Then
            Return If(LeftOrRight, LeftRootPath, RightRootPath) & SyncingList(PreviewList.FocusedItem.Index).Path
        Else
            Return If(LeftOrRight, LeftRootPath, RightRootPath) & SyncingList(PreviewList.SelectedIndices(0)).Path
        End If
    End Function

    Private Sub ContextMnuShowInExplorer(Path As String)
        If Not String.IsNullOrEmpty(Path) AndAlso Not Path.Contains("""") Then
            If IO.File.Exists(Path) OrElse IO.Directory.Exists(Path) Then
                StartProcess(Environment.SystemDirectory & "\..\explorer.exe", "/select, """ & Path & """")
            End If
        End If
    End Sub

    Private Sub ContextMnuOpenFiles(Path As String)
        Dim ExeExtensions As String = "|exe|bat|com|cmd|vb|vbs|vbe|js|jse|ws|wsf|wsc|wsh|ps1|ps1xml|ps2|ps2xml|psc1|psc2|msh|msh1|msh2|mshxml|msh1xml|msh2xml|lnk|py|rb|pif|application|gadget|msi|msp|scr|hta|cpl|msc|jar|scf|reg|"
        Dim Notepad As String = Environment.SystemDirectory & "\notepad.exe"
        If Not String.IsNullOrEmpty(Path) Then
            If IO.Directory.Exists(Path) Then
                StartProcess(IO.Path.GetDirectoryName(Path))
            ElseIf IO.File.Exists(Path) Then
                Dim Ext As String = IO.Path.GetExtension(Path).TrimStart({"."c}).ToLowerInvariant()
                If Ext.Length = 0 OrElse ExeExtensions.Contains("|" & Ext & "|") Then
                    StartProcess(Notepad, """" & Path & """")
                Else
                    StartProcess(Path)
                End If
            End If
        End If
    End Sub

    Private Sub ContextMnuCompare_Click(sender As Object, e As EventArgs) Handles ContextMnuCompare.Click
        Dim DiffProgram As String = ProgramConfig.GetProgramSetting(Of String)(ProgramSetting.DiffProgram, "")
        Dim DiffProgramExpectedChecksum As String = ProgramConfig.GetProgramSetting(Of String)(ProgramSetting.DiffProgramChecksum, "")
        If DiffProgram.Length = 0 OrElse Not IO.File.Exists(DiffProgram) OrElse DiffProgramExpectedChecksum <> GetFileChecksum(DiffProgram) Then
            DiffProgram = OpenFileDialog("(*.exe)|*.exe", Translation.Translate("\PATH_TO_WINMERGE_EXE"))
            If DiffProgram.Length = 0 Then Exit Sub
            ProgramConfig.SetProgramSetting(Of String)(ProgramSetting.DiffProgram, DiffProgram)
            ProgramConfig.SetProgramSetting(Of String)(ProgramSetting.DiffProgramChecksum, GetFileChecksum(DiffProgram))
        End If

        Dim Args As String = ""
        If PreviewList.SelectedIndices.Count = 0 OrElse Status.ShowingErrors Then
            Exit Sub
        ElseIf PreviewList.SelectedIndices.Count = 1 Then
            Args &= " """ & Me.LeftRootPath & SyncingList(PreviewList.SelectedIndices(0)).Path & """ "
            Args &= " """ & Me.RightRootPath & SyncingList(PreviewList.SelectedIndices(0)).Path & """ "
        ElseIf PreviewList.SelectedIndices.Count > 1 Then
            Args &= " """ & IO.Path.GetDirectoryName(Me.LeftRootPath & SyncingList(PreviewList.SelectedIndices(0)).Path) & """ "
            Args &= " """ & IO.Path.GetDirectoryName(Me.RightRootPath & SyncingList(PreviewList.SelectedIndices(0)).Path) & """ "
        End If

        StartProcess(DiffProgram, Args)
    End Sub

    Private Function GetFileChecksum(Path As String) As String
        Using Stream As IO.FileStream = IO.File.OpenRead(Path)
            Using Sha As New Security.Cryptography.SHA1Managed()
                Dim Checksum As Byte() = Sha.ComputeHash(Stream)
                Return BitConverter.ToString(Checksum)
            End Using
        End Using
    End Function

    Public Function CanRunChildWindowCopying() As Boolean
        If Me.IsChildDialog Then Return False
        If PreviewList.SelectedIndices.Count = 0 Then Return False
        If Status.ShowingErrors Then Return False
        If (Not Handler.GetSetting(Of Boolean)(ProfileSetting.PropagateUpdates, True)) Then Return False

        'We only support child window copying for
        '1) left-to-right copies (a Copy where SideOfSource=Left)
        '2) left-to-right deletes (a Delete where SideOfSource=Right)
        For Each Index As Integer In Me.PreviewList.SelectedIndices
            If Not ((SyncingList(Index).Action = TypeOfAction.Copy AndAlso SyncingList(Index).Side = SideOfSource.Left) OrElse
                (SyncingList(Index).Action = TypeOfAction.Delete AndAlso SyncingList(Index).Side = SideOfSource.Right)) Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Sub ContextMnuLr_Click(sender As Object, e As EventArgs) Handles ContextMnuSrcToDest.Click
        ChildWindowCopy_Open(True)
    End Sub

    Private Sub ContextMnuRl_Click(sender As Object, e As EventArgs) Handles ContextMnuDestToSrc.Click
        ChildWindowCopy_Open(False)
    End Sub

    Enum StartSyncingWithoutAsking
        None
        Start
        Cancel
    End Enum

    Private Function ChildWindowCopy_Open(FromSrcToDest As Boolean, Optional Automated As StartSyncingWithoutAsking = StartSyncingWithoutAsking.None) As List(Of SyncingItem)
        If Not CanRunChildWindowCopying() Then Return Nothing

        Dim NewSrcRoot As String = If(FromSrcToDest, LeftRootPath, RightRootPath)
        Dim NewDestRoot As String = If(FromSrcToDest, RightRootPath, LeftRootPath)
        Dim Work As List(Of SyncingItem) = ChildWindowCopy_CreateList(FromSrcToDest)
        Work = ChildWindowCopy_CreateParentDirs(NewDestRoot, If(FromSrcToDest, SideOfSource.Left, SideOfSource.Right), Work)
        Work = ChildWindowCopy_FilterOutRedundantDeletes(Work)
        Dim Arrow As String = Char.ConvertFromUtf32(&H2192)
        Dim Title As String = " " & Me.LeftRootPath & " " & Arrow & " " & Me.RightRootPath
        'Dim Results As List(Of SyncingItem) = ShowChildDialogForManualCopying(Work, LeftRootPath, RightRootPath, Me.Handler.ProfileName, Title, Automated)
        Me.PreviewList.SelectedIndices.Clear()

        'If an item was successfully sync'd, remove it from the list
        Work.Reverse()
        Dim CreatedFiles As New List(Of String)
        For Each Item As SyncingItem In Work
            If Item.RealId >= 0 AndAlso DidSyncingItemComplete(NewSrcRoot, NewDestRoot, Item) Then
                SyncingList.RemoveAt(Item.RealId)
                If Item.Action = TypeOfAction.Copy AndAlso Item.Type = TypeOfItem.File Then CreatedFiles.Add(Item.Path)
            End If
        Next

        If Not FromSrcToDest AndAlso CreatedFiles.Count > 0 Then ChildWindowCopy_SkipDeletingNewlyCopiedFile(Me.SyncingList, CreatedFiles)
        Me.PreviewList.VirtualListSize = SyncingList.Count
        Me.PreviewList.Refresh()
        'Return Results
        Return Nothing
    End Function

    Private Function ChildWindowCopy_CreateList(FromSrcToDest As Boolean) As List(Of SyncingItem)
        Dim Work As New List(Of SyncingItem)
        For Each Index As Integer In Me.PreviewList.SelectedIndices
            Dim NewItem As SyncingItem = MakeSyncingItem(SyncingList(Index).Path, SyncingList(Index).Action, SyncingList(Index).Side, SyncingList(Index).Type, SyncingList(Index).Update, Index)

            If Not FromSrcToDest Then
                If NewItem.Action = TypeOfAction.Delete Then
                    'Make deletions into creations
                    NewItem.Action = TypeOfAction.Copy
                    NewItem.Side = SideOfSource.Right
                ElseIf NewItem.Action = TypeOfAction.Copy AndAlso NewItem.Update = TypeOfUpdate.None Then
                    'Make creations into deletions
                    NewItem.Action = TypeOfAction.Delete
                    NewItem.Side = SideOfSource.Left
                Else
                    'For a changed file, reverse direction
                    NewItem.Side = If(NewItem.Side = SideOfSource.Left, SideOfSource.Right, SideOfSource.Left)
                End If
            End If
            Work.Add(NewItem)
        Next
        Return Work
    End Function

    Private Shared Function ChildWindowCopy_CreateParentDirs(Root As String, Side As SideOfSource, Work As List(Of SyncingItem)) As List(Of SyncingItem)
        'A copy-file will fail if the containing directory doesn't yet exist,
        'so we need to add create-directory entries. It's ok to be redundant,
        'it's fine if we create both C:\dira\dirb and C:\dira
        Dim NewWork As New List(Of SyncingItem)
        Dim DictDirsMade As New Dictionary(Of String, Boolean)

        For Index As Integer = 0 To Work.Count - 1
            Dim Item As SyncingItem = Work(Index)
            If Item.Action = TypeOfAction.Copy AndAlso Item.Type = TypeOfItem.Folder Then
                DictDirsMade(Root & Item.Path) = True
            ElseIf Item.Action = TypeOfAction.Copy AndAlso Item.Update = TypeOfUpdate.None Then
                Dim Parent As String = IO.Path.GetDirectoryName(Root & Item.Path)
                If Not DictDirsMade.ContainsKey(Parent) AndAlso Not IO.Directory.Exists(Parent) Then
                    NewWork.Add(MakeSyncingItem(Parent.Substring(Root.Length), TypeOfAction.Copy, Side, TypeOfItem.Folder))
                    DictDirsMade(Parent) = True
                End If
            End If
            NewWork.Add(Item)
        Next
        Return NewWork
    End Function

    Private Function ChildWindowCopy_FilterOutRedundantDeletes(Work As List(Of SyncingItem)) As List(Of SyncingItem)
        'If we have 1) delete dir 2) delete dir/file.txt, the second delete will fail. so don't add the second delete.
        Dim NewWork As New List(Of SyncingItem)
        Dim DeletedDirectories As New Dictionary(Of String, Boolean)
        For Index As Integer = 0 To Work.Count - 1
            If Not IsRedundantDelete(DeletedDirectories, Work(Index)) Then
                NewWork.Add(Work(Index))
            End If
        Next
        Return NewWork
    End Function

    Private Shared Sub ChildWindowCopy_SkipDeletingNewlyCopiedFile(SyncList As List(Of SyncingItem), CreatedFiles As List(Of String))
        For Index As Integer = 0 To CreatedFiles.Count - 1
            CreatedFiles(Index) = NormalizeCase(CreatedFiles(Index))
        Next

        'If we created a new file, remove anything in the list that would delete its parent directory
        For Index As Integer = SyncList.Count - 1 To 0 Step -1
            If SyncList(Index).Action = TypeOfAction.Delete AndAlso SyncList(Index).Type = TypeOfItem.Folder AndAlso
                SyncList(Index).Side = SideOfSource.Right Then
                For Each CreatedFile As String In CreatedFiles
                    If CreatedFile.StartsWith(NormalizeCase(SyncList(Index).Path), StringComparison.Ordinal) Then
                        SyncList.RemoveAt(Index)
                        Exit For
                    End If
                Next
            End If
        Next
    End Sub

    Function DidSyncingItemComplete(SrcRoot As String, DestRoot As String, Item As SyncingItem) As Boolean
        If Item.Action = TypeOfAction.Copy AndAlso Item.Type = TypeOfItem.Folder Then
            Return IO.Directory.Exists(DestRoot & Item.Path)
        ElseIf Item.Action = TypeOfAction.Delete AndAlso Item.Type = TypeOfItem.Folder Then
            Return Not IO.Directory.Exists(DestRoot & Item.Path)
        ElseIf Item.Action = TypeOfAction.Copy AndAlso Item.Type = TypeOfItem.File Then
            'Are files are the same?
            'Use SourceIsMoreRecent instead of == because some media like FAT format has imprecise LMTs
            Return IO.File.Exists(SrcRoot & Item.Path) AndAlso IO.File.Exists(DestRoot & Item.Path) AndAlso
                Not SourceIsMoreRecent(SrcRoot & Item.Path, DestRoot & Item.Path) AndAlso
                Not SourceIsMoreRecent(DestRoot & Item.Path, SrcRoot & Item.Path)
        ElseIf Item.Action = TypeOfAction.Delete AndAlso Item.Type = TypeOfItem.File Then
            Return Not IO.File.Exists(DestRoot & Item.Path)
        End If
        Return False
    End Function

    Private Shared Function IsRedundantDelete(Dict As Dictionary(Of String, Boolean), Item As SyncingItem) As Boolean
        'If we have 1) delete dir 2) delete dir/file.txt, the second delete will fail. so don't add the second delete.
        Dim FirstPath As String = If(Item.Type = TypeOfItem.File, IO.Path.GetDirectoryName(Item.Path), Item.Path)
        If Item.Action <> TypeOfAction.Delete OrElse FirstPath = "" OrElse FirstPath = "." OrElse FirstPath = "\" Then Return False
        Dim Info As New IO.DirectoryInfo(FirstPath)
        Dim LengthToSubtract As Integer = Info.FullName.Length - FirstPath.Length
        While Info IsNot Nothing AndAlso Info.FullName.Length >= LengthToSubtract
            'Loop upwards, checking every ancestor
            If Dict.ContainsKey(Info.FullName.Substring(LengthToSubtract)) Then Return True
            Info = Info.Parent
        End While
        If Item.Type = TypeOfItem.Folder Then Dict(Item.Path) = True
        Return False
    End Function

    Private Shared Function MakeSyncingItem(Path As String, Action As TypeOfAction, Side As SideOfSource, ItemType As TypeOfItem, Optional Update As TypeOfUpdate = TypeOfUpdate.None, Optional RealId As Integer = -1) As SyncingItem
        Return New SyncingItem() With {.RealId = RealId, .Path = Path, .Action = Action, .Side = Side, .Type = ItemType, .Update = Update}
    End Function
End Class
