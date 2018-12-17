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
            Clipboard.SetText(Builder.ToString().Trim())
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

    Private Function CanRunChildWindowCopying() As Boolean
        If Me.IsChildDialog Then Return False
        If Status.ShowingErrors Then Return False
        If (Not Handler.GetSetting(Of Boolean)(ProfileSetting.PropagateUpdates, True)) Then Return False
        Return CanRunChildWindowCopyingImpl(SyncingList, Me.PreviewList)
    End Function

    Private Shared Function CanRunChildWindowCopyingImpl(L As List(Of SyncingItem), Lview As ListView) As Boolean
        If Lview.SelectedIndices.Count = 0 Then Return False
        'We only support child window copying for
        '1) left-to-right copies (a Copy where SideOfSource=Left)
        '2) left-to-right deletes (a Delete where SideOfSource=Right)
        For Each Index As Integer In Lview.SelectedIndices
            If Not ((L(Index).Action = TypeOfAction.Copy AndAlso L(Index).Side = SideOfSource.Left) OrElse
                (L(Index).Action = TypeOfAction.Delete AndAlso L(Index).Side = SideOfSource.Right)) Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Sub ContextMnuLr_Click(sender As Object, e As EventArgs) Handles ContextMnuSrcToDest.Click
        ChildWindowCopy(True)
    End Sub

    Private Sub ContextMnuRl_Click(sender As Object, e As EventArgs) Handles ContextMnuDestToSrc.Click
        ChildWindowCopy(False)
    End Sub

    Enum StartWithoutAsking
        None
        Start
        Cancel
    End Enum

    Private Function ChildWindowCopy(FromSrcToDest As Boolean, Optional ShouldStartWithoutAsking As StartWithoutAsking = StartWithoutAsking.None) As List(Of SyncingItem)
        If Not CanRunChildWindowCopying() Then Return Nothing

        Dim NewSrcRoot As String = If(FromSrcToDest, LeftRootPath, RightRootPath)
        Dim NewDestRoot As String = If(FromSrcToDest, RightRootPath, LeftRootPath)
        Dim Work As List(Of SyncingItem) = ChildWindowCopy_CreateList(FromSrcToDest)

        Dim Str As String = If(FromSrcToDest, "\LR", "\RL")
        Dim Title As String = " " & Translation.Translate(Str)
        Dim ResultCompletedItems As New Dictionary(Of String, Boolean)
        Dim Results As List(Of SyncingItem) = ChildWindowCopy_Show(Work, LeftRootPath, RightRootPath, Me.Handler.ProfileName, Title, ShouldStartWithoutAsking, ResultCompletedItems)
        Me.PreviewList.SelectedIndices.Clear()

        Dim NewSyncingList As List(Of SyncingItem) = UpdateSyncingListAfterChildWindowCopy(SyncingList, ResultCompletedItems, FromSrcToDest)
        Me.SyncingList.Clear()
        Me.SyncingList.AddRange(NewSyncingList)
        Me.PreviewList.VirtualListSize = SyncingList.Count
        Me.PreviewList.Refresh()
        Return Results
    End Function

    Private Shared Function ChildWindowCopy_Show(Work As List(Of SyncingItem), LeftRootPath As String, RightRootPath As String, ProfileName As String,
            Title As String, ShouldStartWithoutAsking As StartWithoutAsking, ByRef ResultCompletedItems As Dictionary(Of String, Boolean)) As List(Of SyncingItem)
        Dim ChildForm As New SynchronizeForm(ProfileName, True, False)
        ChildForm.IsChildDialog = True
        ChildForm.LeftRootPath = LeftRootPath
        ChildForm.RightRootPath = RightRootPath
        ChildForm.TitleText = Title

        'emulate Scan()
        For Each Item As SyncingItem In Work
            ChildForm.AddToSyncingList(Item.Path, Item.Type, Item.Side, Item.Action, Item.Update)
        Next
        ChildForm.UpdateStatuses()
        ChildForm.StepCompleted(StatusData.SyncStep.Scan)
        If ShouldStartWithoutAsking = StartWithoutAsking.Start Then ChildForm.Sync()
        If ShouldStartWithoutAsking = StartWithoutAsking.None Then ChildForm.ShowDialog()
        ResultCompletedItems = ChildForm.CompletedItems
        Return ChildForm.SyncingList
    End Function

    Private Function ChildWindowCopy_CreateList(FromSrcToDest As Boolean) As List(Of SyncingItem)
        Dim Work As New List(Of SyncingItem)
        For Each Index As Integer In Me.PreviewList.SelectedIndices
            Work.Add(ChildWindowCopy_ConvertItem(SyncingList(Index), Index, FromSrcToDest))
        Next
        Return Work
    End Function

    Private Shared Function ChildWindowCopy_ConvertItem(Item As SyncingItem, Index As Int32, FromSrcToDest As Boolean) As SyncingItem
        'If we are syncing from dest to src, we'll reverse the direction here.
        Dim NewItem As SyncingItem = MakeSyncingItem(Item.Path, Item.Action, Item.Side, Item.Type, Item.Update, Index)

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
        Return NewItem
    End Function

    Private Shared Function UpdateSyncingListAfterChildWindowCopy(SyncingList As List(Of SyncingItem), CompletedItems As Dictionary(Of String, Boolean), FromSrcToDest As Boolean) As List(Of SyncingItem)
        Dim NewSyncingList As New List(Of SyncingItem)
        Dim ProtectTheseDirectories As New Dictionary(Of String, Boolean)
        If Not FromSrcToDest Then
            'If we created a new file, remove anything in the list that would delete its parent directory
            For Each Pair As KeyValuePair(Of String, Boolean) In CompletedItems
                Dim Parts As String() = Pair.Key.Split(New String() {" Action=Copy Side=Right Type=File IsUpdate=None"}, StringSplitOptions.None)
                If Parts.Length = 2 AndAlso Parts(1) = "" AndAlso Parts(0).StartsWith("Path=") Then
                    'We used to use string.startswith, but it was O(n*m) - slow - and we don't want C:\myfile to be seen as a "parent" of C:\myfile1234
                    Dim Path As String = Parts(0).Substring("Path=".Length)
                    UpdateSyncingList_AddAllParents(ProtectTheseDirectories, Path)
                End If
            Next
        End If

        For Each Item As SyncingItem In SyncingList
            Dim ConvertedItem As SyncingItem = ChildWindowCopy_ConvertItem(Item, 0, FromSrcToDest)
            If CompletedItems.ContainsKey(ConvertedItem.ToStringWithoutRealId) Then
                'We processed this item
            ElseIf Not FromSrcToDest AndAlso Item.Action = TypeOfAction.Delete AndAlso Item.Type = TypeOfItem.Folder _
                AndAlso Item.Side = SideOfSource.Right AndAlso ProtectTheseDirectories.ContainsKey(Item.Path) Then
                'We should remove this item because it would delete a file we just made
            Else
                NewSyncingList.Add(Item)
            End If
        Next

        Return NewSyncingList
    End Function

    Private Shared Sub UpdateSyncingList_AddAllParents(ProtectTheseDirectories As Dictionary(Of String, Boolean), Path As String)
        While True
            Dim CurLength As Int32 = Path.Length
            Path = IO.Path.GetDirectoryName(Path)
            If Path Is Nothing OrElse Path.Length = 0 OrElse Path.Length >= CurLength Then
                'This will never be an infinite loop because the length of the string must always be decreasing.
                Exit While
            Else
                ProtectTheseDirectories(Path) = True
            End If
        End While
    End Sub

    Private Shared Function MakeSyncingItem(Path As String, Action As TypeOfAction, Side As SideOfSource, ItemType As TypeOfItem, Optional Update As TypeOfUpdate = TypeOfUpdate.None, Optional RealId As Integer = -1) As SyncingItem
        Return New SyncingItem() With {.RealId = RealId, .Path = Path, .Action = Action, .Side = Side, .Type = ItemType, .Update = Update}
    End Function
End Class
