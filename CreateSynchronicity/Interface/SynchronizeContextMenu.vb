'This file is part of Create Synchronicity.
'
'Create Synchronicity is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
'Create Synchronicity is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
'You should have received a copy of the GNU General Public License along with Create Synchronicity.  If not, see <http://www.gnu.org/licenses/>.
'Created by:	Clément Pit--Claudel.
'Web site:		http://synchronicity.sourceforge.net.


Partial Class SynchronizeForm
    Private Sub ContextMnuLeftCopyPath_Click(sender As Object, e As EventArgs) Handles ContextMnuLeftCopyPath.Click
        If PreviewList.SelectedIndices.Count > 0 Then Clipboard.SetText(String.Join(Environment.NewLine, GetFullPathsOfSelectedItems(True).ToArray()))
    End Sub

    Private Sub ContextMnuRightCopyPath_Click(sender As Object, e As EventArgs) Handles ContextMnuRightCopyPath.Click
        If PreviewList.SelectedIndices.Count > 0 Then Clipboard.SetText(String.Join(Environment.NewLine, GetFullPathsOfSelectedItems(False).ToArray()))
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
        'when picking just one of the selected items, let's use the focused item because it's the one the user last clicked
        If PreviewList.SelectedIndices.Count = 0 OrElse Status.ShowingErrors Then
            Return Nothing
        ElseIf PreviewList.FocusedItem IsNot Nothing AndAlso PreviewList.SelectedIndices.Contains(PreviewList.FocusedItem.Index) Then
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
End Class
