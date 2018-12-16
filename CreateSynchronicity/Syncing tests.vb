'This file is part of Create Synchronicity.
'
'Create Synchronicity is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
'Create Synchronicity is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
'You should have received a copy of the GNU General Public License along with Create Synchronicity.  If not, see <http://www.gnu.org/licenses/>.
'Created by:	Clément Pit--Claudel.
'Web site:		http://synchronicity.sourceforge.net.

Partial Class SynchronizeForm
    Private Shared Sub RunEachTest(ConfigPath As String, TempDir As String, ShowUi As Boolean)
        'Context menu tests
        WriteTestFiles(TempDir, TempDir & "\testsync\src\d1", TempDir & "\testsync\dest\d1")
        Dim TestForm As SynchronizeForm = InitHighlevelTests(ConfigPath, TempDir, True)
        Test_ExpectedFilesInList(TestForm, TempDir, ShowUi)
        Test_CopyLeftPaths(TestForm, TempDir, ShowUi)
        Test_CopyRightPaths(TestForm, TempDir, ShowUi)
        Test_CopyLeftOnePath(TestForm, TempDir, ShowUi)
        Test_CopyRightOnePath(TestForm, TempDir, ShowUi)
        Test_CopyPathnames(TestForm, TempDir, ShowUi)

        If IO.Directory.Exists(TempDir) Then IO.Directory.Delete(TempDir, True)
    End Sub

    Friend Shared Sub RunTests(ShowUi As Boolean)
        Dim TempDir As String = IO.Path.Combine(IO.Path.GetTempPath(), "test_create_synchronicity")
        Dim ConfigPath As String = ProgramConfig.ConfigRootDir & "\high_level_create_sync_testonly.sync"
        Try
            RunEachTest(ConfigPath, TempDir, ShowUi)
        Finally
            If IO.File.Exists(ConfigPath) Then IO.File.Delete(ConfigPath)
        End Try
    End Sub

    Private Shared Sub TestUtil_SetSelection(TestForm As SynchronizeForm, Indices As Int32())
        TestForm.PreviewList.SelectedIndices.Clear()
        For Each Index As Int32 In Indices
            TestForm.PreviewList.SelectedIndices.Add(Index)
        Next
    End Sub

    Private Shared Function TestUtil_ToNewline(S As String, Dir As String) As String
        Return S.Replace("%dir", Dir).Replace("||", Environment.NewLine)
    End Function

    Private Shared Sub Test_ExpectedFilesInList(TestForm As SynchronizeForm, TempDir As String, ShowUi As Boolean)
        Dim Dir As String = TempDir & "\testsync"
        AssertEqual(MakeSyncingItem("\deldir", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.Folder, TypeOfUpdate.None, TestForm.SyncingList(0).RealId), TestForm.SyncingList(0))
        AssertEqual(MakeSyncingItem("\deldir\fileindeldir1.txt", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.None, TestForm.SyncingList(1).RealId), TestForm.SyncingList(1))
        AssertEqual(MakeSyncingItem("\deldir\fileindeldir2.txt", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.None, TestForm.SyncingList(2).RealId), TestForm.SyncingList(2))
        AssertEqual(MakeSyncingItem("\newdir", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.Folder, TypeOfUpdate.None, TestForm.SyncingList(3).RealId), TestForm.SyncingList(3))
        AssertEqual(MakeSyncingItem("\newdir\fileinnewdir1.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.None, TestForm.SyncingList(4).RealId), TestForm.SyncingList(4))
        AssertEqual(MakeSyncingItem("\newdir\fileinnewdir2.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.None, TestForm.SyncingList(5).RealId), TestForm.SyncingList(5))
        AssertEqual(MakeSyncingItem("\newfile.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.None, TestForm.SyncingList(6).RealId), TestForm.SyncingList(6))
        AssertEqual(MakeSyncingItem("\oldfile.txt", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.None, TestForm.SyncingList(7).RealId), TestForm.SyncingList(7))
        AssertEqual(MakeSyncingItem("\updatedbetter.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.ReplaceWithNewerFile, TestForm.SyncingList(8).RealId), TestForm.SyncingList(8))
        AssertEqual(MakeSyncingItem("\updatedworse.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.ReplaceWithOlderFile, TestForm.SyncingList(9).RealId), TestForm.SyncingList(9))
        Dim Expected As String = "dest\d1\deldir\fileindeldir1.txt=abc3|dest\d1\deldir\fileindeldir2.txt=abc4|dest\d1\oldfile.txt=oldfile|dest\d1\samedir\fileinsamedir.txt=abc|dest\d1\samefile.txt=abc|dest\d1\updatedbetter.txt=abc|dest\d1\updatedworse.txt=xyz123|src\d1\newdir\fileinnewdir1.txt=abc1|src\d1\newdir\fileinnewdir2.txt=abc2|src\d1\newfile.txt=newfile|src\d1\samedir\fileinsamedir.txt=a|src\d1\samefile.txt=abc|src\d1\updatedbetter.txt=abc12345|src\d1\updatedworse.txt=xyz|"
        AssertEqual(Expected, DirectoryFileContentsToString(Dir))
    End Sub

    Private Shared Sub Test_CopyLeftPaths(TestForm As SynchronizeForm, TempDir As String, ShowUi As Boolean)
        Clipboard.SetText("_")
        Dim Dir As String = TempDir & "\testsync\src\d1"
        '0 items selected
        TestUtil_SetSelection(TestForm, New Int32() {})
        TestForm.ContextMnuLeftCopyPath_Click(Nothing, Nothing)
        AssertEqual("_", Clipboard.GetText())
        '1 item selected, deleted dir
        TestUtil_SetSelection(TestForm, New Int32() {0})
        TestForm.ContextMnuLeftCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\deldir", Dir), Clipboard.GetText())
        '1 item selected, deleted file
        TestUtil_SetSelection(TestForm, New Int32() {1})
        TestForm.ContextMnuLeftCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\deldir\fileindeldir1.txt", Dir), Clipboard.GetText())
        '1 item selected, new dir
        TestUtil_SetSelection(TestForm, New Int32() {3})
        TestForm.ContextMnuLeftCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\newdir", Dir), Clipboard.GetText())
        '1 item selected, new file
        TestUtil_SetSelection(TestForm, New Int32() {4})
        TestForm.ContextMnuLeftCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\newdir\fileinnewdir1.txt", Dir), Clipboard.GetText())
        '1 item selected, changed file (newer)
        TestUtil_SetSelection(TestForm, New Int32() {8})
        TestForm.ContextMnuLeftCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\updatedbetter.txt", Dir), Clipboard.GetText())
        '1 item selected, changed file (older)
        TestUtil_SetSelection(TestForm, New Int32() {9})
        TestForm.ContextMnuLeftCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\updatedworse.txt", Dir), Clipboard.GetText())
        '2 items selected, new dir, new file
        TestUtil_SetSelection(TestForm, New Int32() {3, 4})
        TestForm.ContextMnuLeftCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\newdir||%dir\newdir\fileinnewdir1.txt", Dir), Clipboard.GetText())
        '4 items selected, new dir, new file, changed files
        TestUtil_SetSelection(TestForm, New Int32() {3, 4, 8, 9})
        TestForm.ContextMnuLeftCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\newdir||%dir\newdir\fileinnewdir1.txt||%dir\updatedbetter.txt||%dir\updatedworse.txt", Dir), Clipboard.GetText())
        '4 items selected, deleted dir, deleted file, changed files, different order added
        TestUtil_SetSelection(TestForm, New Int32() {8, 9, 1, 0})
        TestForm.ContextMnuLeftCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\deldir||%dir\deldir\fileindeldir1.txt||%dir\updatedbetter.txt||%dir\updatedworse.txt", Dir), Clipboard.GetText())
    End Sub

    Private Shared Sub Test_CopyRightPaths(TestForm As SynchronizeForm, TempDir As String, ShowUi As Boolean)
        Clipboard.SetText("_")
        Dim Dir As String = TempDir & "\testsync\dest\d1"
        '0 items selected
        TestUtil_SetSelection(TestForm, New Int32() {})
        TestForm.ContextMnuRightCopyPath_Click(Nothing, Nothing)
        AssertEqual("_", Clipboard.GetText())
        '1 item selected, deleted dir
        TestUtil_SetSelection(TestForm, New Int32() {0})
        TestForm.ContextMnuRightCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\deldir", Dir), Clipboard.GetText())
        '1 item selected, deleted file
        TestUtil_SetSelection(TestForm, New Int32() {1})
        TestForm.ContextMnuRightCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\deldir\fileindeldir1.txt", Dir), Clipboard.GetText())
        '1 item selected, new dir
        TestUtil_SetSelection(TestForm, New Int32() {3})
        TestForm.ContextMnuRightCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\newdir", Dir), Clipboard.GetText())
        '1 item selected, new file
        TestUtil_SetSelection(TestForm, New Int32() {4})
        TestForm.ContextMnuRightCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\newdir\fileinnewdir1.txt", Dir), Clipboard.GetText())
        '1 item selected, changed file (newer)
        TestUtil_SetSelection(TestForm, New Int32() {8})
        TestForm.ContextMnuRightCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\updatedbetter.txt", Dir), Clipboard.GetText())
        '1 item selected, changed file (older)
        TestUtil_SetSelection(TestForm, New Int32() {9})
        TestForm.ContextMnuRightCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\updatedworse.txt", Dir), Clipboard.GetText())
        '2 items selected, new dir, new file
        TestUtil_SetSelection(TestForm, New Int32() {3, 4})
        TestForm.ContextMnuRightCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\newdir||%dir\newdir\fileinnewdir1.txt", Dir), Clipboard.GetText())
        '4 items selected, new dir, new file, changed files
        TestUtil_SetSelection(TestForm, New Int32() {3, 4, 8, 9})
        TestForm.ContextMnuRightCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\newdir||%dir\newdir\fileinnewdir1.txt||%dir\updatedbetter.txt||%dir\updatedworse.txt", Dir), Clipboard.GetText())
        '4 items selected, deleted dir, deleted file, changed files, different order added
        TestUtil_SetSelection(TestForm, New Int32() {8, 9, 1, 0})
        TestForm.ContextMnuRightCopyPath_Click(Nothing, Nothing)
        AssertEqual(TestUtil_ToNewline("%dir\deldir||%dir\deldir\fileindeldir1.txt||%dir\updatedbetter.txt||%dir\updatedworse.txt", Dir), Clipboard.GetText())
    End Sub

    Private Shared Sub Test_CopyLeftOnePath(TestForm As SynchronizeForm, TempDir As String, ShowUi As Boolean)
        Dim Dir As String = TempDir & "\testsync\src\d1"
        '0 items selected
        TestUtil_SetSelection(TestForm, New Int32() {})
        AssertEqual(Nothing, TestForm.GetFullPathsOfOneSelectedItem(True))
        '1 item selected, deleted dir
        TestUtil_SetSelection(TestForm, New Int32() {0})
        AssertEqual(TestUtil_ToNewline("%dir\deldir", Dir), TestForm.GetFullPathsOfOneSelectedItem(True))
        '1 item selected, deleted file
        TestUtil_SetSelection(TestForm, New Int32() {1})
        AssertEqual(TestUtil_ToNewline("%dir\deldir\fileindeldir1.txt", Dir), TestForm.GetFullPathsOfOneSelectedItem(True))
        '1 item selected, new dir
        TestUtil_SetSelection(TestForm, New Int32() {3})
        AssertEqual(TestUtil_ToNewline("%dir\newdir", Dir), TestForm.GetFullPathsOfOneSelectedItem(True))
        '1 item selected, new file
        TestUtil_SetSelection(TestForm, New Int32() {4})
        AssertEqual(TestUtil_ToNewline("%dir\newdir\fileinnewdir1.txt", Dir), TestForm.GetFullPathsOfOneSelectedItem(True))
        '1 item selected, changed file (newer)
        TestUtil_SetSelection(TestForm, New Int32() {8})
        AssertEqual(TestUtil_ToNewline("%dir\updatedbetter.txt", Dir), TestForm.GetFullPathsOfOneSelectedItem(True))
        '1 item selected, changed file (older)
        TestUtil_SetSelection(TestForm, New Int32() {9})
        AssertEqual(TestUtil_ToNewline("%dir\updatedworse.txt", Dir), TestForm.GetFullPathsOfOneSelectedItem(True))
        '2 items selected, new dir, new file
        TestUtil_SetSelection(TestForm, New Int32() {3, 4})
        AssertEqual(TestUtil_ToNewline("%dir\newdir", Dir), TestForm.GetFullPathsOfOneSelectedItem(True))
        '4 items selected, new dir, new file, changed files
        TestUtil_SetSelection(TestForm, New Int32() {3, 4, 8, 9})
        AssertEqual(TestUtil_ToNewline("%dir\newdir", Dir), TestForm.GetFullPathsOfOneSelectedItem(True))
        '4 items selected, deleted dir, deleted file, changed files, different order added
        TestUtil_SetSelection(TestForm, New Int32() {8, 9, 1, 0})
        AssertEqual(TestUtil_ToNewline("%dir\deldir", Dir), TestForm.GetFullPathsOfOneSelectedItem(True))
    End Sub

    Private Shared Sub Test_CopyRightOnePath(TestForm As SynchronizeForm, TempDir As String, ShowUi As Boolean)
        Dim Dir As String = TempDir & "\testsync\dest\d1"
        '0 items selected
        TestUtil_SetSelection(TestForm, New Int32() {})
        AssertEqual(Nothing, TestForm.GetFullPathsOfOneSelectedItem(False))
        '1 item selected, deleted dir
        TestUtil_SetSelection(TestForm, New Int32() {0})
        AssertEqual(TestUtil_ToNewline("%dir\deldir", Dir), TestForm.GetFullPathsOfOneSelectedItem(False))
        '1 item selected, deleted file
        TestUtil_SetSelection(TestForm, New Int32() {1})
        AssertEqual(TestUtil_ToNewline("%dir\deldir\fileindeldir1.txt", Dir), TestForm.GetFullPathsOfOneSelectedItem(False))
        '1 item selected, new dir
        TestUtil_SetSelection(TestForm, New Int32() {3})
        AssertEqual(TestUtil_ToNewline("%dir\newdir", Dir), TestForm.GetFullPathsOfOneSelectedItem(False))
        '1 item selected, new file
        TestUtil_SetSelection(TestForm, New Int32() {4})
        AssertEqual(TestUtil_ToNewline("%dir\newdir\fileinnewdir1.txt", Dir), TestForm.GetFullPathsOfOneSelectedItem(False))
        '1 item selected, changed file (newer)
        TestUtil_SetSelection(TestForm, New Int32() {8})
        AssertEqual(TestUtil_ToNewline("%dir\updatedbetter.txt", Dir), TestForm.GetFullPathsOfOneSelectedItem(False))
        '1 item selected, changed file (older)
        TestUtil_SetSelection(TestForm, New Int32() {9})
        AssertEqual(TestUtil_ToNewline("%dir\updatedworse.txt", Dir), TestForm.GetFullPathsOfOneSelectedItem(False))
        '2 items selected, new dir, new file
        TestUtil_SetSelection(TestForm, New Int32() {3, 4})
        AssertEqual(TestUtil_ToNewline("%dir\newdir", Dir), TestForm.GetFullPathsOfOneSelectedItem(False))
        '4 items selected, new dir, new file, changed files
        TestUtil_SetSelection(TestForm, New Int32() {3, 4, 8, 9})
        AssertEqual(TestUtil_ToNewline("%dir\newdir", Dir), TestForm.GetFullPathsOfOneSelectedItem(False))
        '4 items selected, deleted dir, deleted file, changed files, different order added
        TestUtil_SetSelection(TestForm, New Int32() {8, 9, 1, 0})
        AssertEqual(TestUtil_ToNewline("%dir\deldir", Dir), TestForm.GetFullPathsOfOneSelectedItem(False))
    End Sub

    Private Shared Sub Test_CopyPathnames(TestForm As SynchronizeForm, TempDir As String, ShowUi As Boolean)
        Clipboard.SetText("_")
        Dim Dir1 As String = TempDir & "\testsync\src\d1"
        Dim Dir2 As String = TempDir & "\testsync\dest\d1"
        '0 items selected
        TestUtil_SetSelection(TestForm, New Int32() {})
        TestForm.ContextMnuCopyPathnames_Click(Nothing, Nothing)
        AssertEqual("_", Clipboard.GetText())
        '1 item selected, deleted dir
        TestUtil_SetSelection(TestForm, New Int32() {0})
        TestForm.ContextMnuCopyPathnames_Click(Nothing, Nothing)
        AssertEqual(String.Format("{0}\deldir|{1}\deldir", Dir1, Dir2), Clipboard.GetText())
        '1 item selected, deleted file
        TestUtil_SetSelection(TestForm, New Int32() {1})
        TestForm.ContextMnuCopyPathnames_Click(Nothing, Nothing)
        AssertEqual(String.Format("{0}\deldir\fileindeldir1.txt|{1}\deldir\fileindeldir1.txt", Dir1, Dir2), Clipboard.GetText())
        '1 item selected, new dir
        TestUtil_SetSelection(TestForm, New Int32() {3})
        TestForm.ContextMnuCopyPathnames_Click(Nothing, Nothing)
        AssertEqual(String.Format("{0}\newdir|{1}\newdir", Dir1, Dir2), Clipboard.GetText())
        '1 item selected, new file
        TestUtil_SetSelection(TestForm, New Int32() {4})
        TestForm.ContextMnuCopyPathnames_Click(Nothing, Nothing)
        AssertEqual(String.Format("{0}\newdir\fileinnewdir1.txt|{1}\newdir\fileinnewdir1.txt", Dir1, Dir2), Clipboard.GetText())
        '1 item selected, changed file (newer)
        TestUtil_SetSelection(TestForm, New Int32() {8})
        TestForm.ContextMnuCopyPathnames_Click(Nothing, Nothing)
        AssertEqual(String.Format("{0}\updatedbetter.txt|{1}\updatedbetter.txt", Dir1, Dir2), Clipboard.GetText())
        '1 item selected, changed file (older)
        TestUtil_SetSelection(TestForm, New Int32() {9})
        TestForm.ContextMnuCopyPathnames_Click(Nothing, Nothing)
        AssertEqual(String.Format("{0}\updatedworse.txt|{1}\updatedworse.txt", Dir1, Dir2), Clipboard.GetText())
        '2 items selected, new dir, new file
        TestUtil_SetSelection(TestForm, New Int32() {3, 4})
        TestForm.ContextMnuCopyPathnames_Click(Nothing, Nothing)
        Dim Expected As String = String.Format("{0}\newdir|{1}\newdir||{0}\newdir\fileinnewdir1.txt|{1}\newdir\fileinnewdir1.txt", Dir1, Dir2)
        AssertEqual(TestUtil_ToNewline(Expected, ""), Clipboard.GetText())
        '4 items selected, new dir, new file, changed files
        TestUtil_SetSelection(TestForm, New Int32() {3, 4, 8, 9})
        TestForm.ContextMnuCopyPathnames_Click(Nothing, Nothing)
        Expected = String.Format("{0}\newdir|{1}\newdir||{0}\newdir\fileinnewdir1.txt|{1}\newdir\fileinnewdir1.txt||{0}\updatedbetter.txt|{1}\updatedbetter.txt||{0}\updatedworse.txt|{1}\updatedworse.txt", Dir1, Dir2)
        AssertEqual(TestUtil_ToNewline(Expected, ""), Clipboard.GetText())
        '4 items selected, deleted dir, deleted file, changed files, different order added
        TestUtil_SetSelection(TestForm, New Int32() {8, 9, 1, 0})
        TestForm.ContextMnuCopyPathnames_Click(Nothing, Nothing)
        Expected = String.Format("{0}\deldir|{1}\deldir||{0}\deldir\fileindeldir1.txt|{1}\deldir\fileindeldir1.txt||{0}\updatedbetter.txt|{1}\updatedbetter.txt||{0}\updatedworse.txt|{1}\updatedworse.txt", Dir1, Dir2)
        AssertEqual(TestUtil_ToNewline(Expected, ""), Clipboard.GetText())
    End Sub



    Private Shared Function InitHighlevelTests(ConfigPath As String, TempDir As String, StrictDate As Boolean) As SynchronizeForm
        'Write profile settings
        Dim ProfileText As String = "Strict mirror:True|Discard after:0|Propagate Updates:True|Time Offset:0|Checksum:False|Check file size:False|Synchronization Method:0|Files restrictions:0|Source folders to be synchronized:*;|Destination folders to be synchronized:*;|Included Filetypes:|Excluded FileTypes:|Replicate Empty Directories:True|Check contents before deleting folders:True"
        ProfileText &= "|Strict date comparison:" & If(StrictDate, "True", "False")
        ProfileText &= "|Source Directory:" & TempDir & "\testsync\src\d1"
        ProfileText &= "|Destination Directory:" & TempDir & "\testsync\dest\d1"
        ProfileText &= "|Indicate if destination is newer:True"
        ProfileText = ProfileText.Replace("|", Environment.NewLine)
        IO.File.WriteAllText(ConfigPath, ProfileText)

        'Run scan and sort by path
        Dim TestForm As New SynchronizeForm("high_level_create_sync_testonly", True, False)
        TestForm.Visible = False
        TestForm.Scan() 'Note: runs on the main thread
        TestForm.PreviewList_ColumnClick(Nothing, New ColumnClickEventArgs(3)) 'Now sorted by path z-a
        TestForm.PreviewList_ColumnClick(Nothing, New ColumnClickEventArgs(3)) 'Now sorted by path a-z
        Return TestForm
    End Function


    Private Shared Sub AssertEqual(Expected As Object, Received As Object)
        If (Expected Is Nothing And Received Is Nothing) Then
        ElseIf (Expected.ToString() <> Received.ToString()) Then
            MessageBox.Show("Test failed: " & Expected.ToString() & " != " & Received.ToString())
            Throw New Exception()
        End If
    End Sub

    Private Shared Sub WriteTestFiles(TempDir As String, PathSrc As String, PathDest As String)
        If IO.Directory.Exists(TempDir) Then IO.Directory.Delete(TempDir, True)
        AssertEqual(False, IO.Directory.Exists(TempDir))
        IO.Directory.CreateDirectory(PathSrc & "\newdir")
        IO.Directory.CreateDirectory(PathSrc & "\samedir")
        IO.File.WriteAllText(PathSrc & "\samefile.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\samedir\fileinsamedir.txt", "a")
        IO.File.WriteAllText(PathSrc & "\newfile.txt", "newfile")
        IO.File.WriteAllText(PathSrc & "\updatedbetter.txt", "abc12345")
        IO.File.WriteAllText(PathSrc & "\updatedworse.txt", "xyz")
        IO.File.WriteAllText(PathSrc & "\newdir\fileinnewdir1.txt", "abc1")
        IO.File.WriteAllText(PathSrc & "\newdir\fileinnewdir2.txt", "abc2")
        IO.Directory.CreateDirectory(PathDest & "\deldir")
        IO.Directory.CreateDirectory(PathDest & "\samedir")
        IO.File.Copy(PathSrc & "\samefile.txt", PathDest & "\samefile.txt") 'Ensure same lmt
        IO.File.Copy(PathSrc & "\samefile.txt", PathDest & "\samedir\fileinsamedir.txt") 'Ensure same lmt
        IO.File.WriteAllText(PathDest & "\oldfile.txt", "oldfile")
        IO.File.WriteAllText(PathDest & "\deldir\fileindeldir1.txt", "abc3")
        IO.File.WriteAllText(PathDest & "\deldir\fileindeldir2.txt", "abc4")
        IO.File.WriteAllText(PathDest & "\updatedbetter.txt", "abc")
        IO.File.WriteAllText(PathDest & "\updatedworse.txt", "xyz123")

        'Ensure different lmt
        Dim SpanTenSeconds As New TimeSpan(0, 0, 10)
        Dim LmtBetter As Date = IO.File.GetLastWriteTimeUtc(PathSrc & "\updatedbetter.txt")
        IO.File.SetLastWriteTimeUtc(PathDest & "\updatedbetter.txt", LmtBetter - SpanTenSeconds)
        IO.File.SetLastWriteTimeUtc(PathSrc & "\updatedworse.txt", LmtBetter - SpanTenSeconds)
    End Sub

    Private Shared Function DirectoryFileContentsToString(Path As String) As String
        Dim Files As New List(Of String)(IO.Directory.GetFiles(Path, "*", IO.SearchOption.AllDirectories))
        Dim Builder As New Text.StringBuilder()
        Files.Sort(StringComparer.InvariantCulture)
        For Each File As String In Files
            Builder.Append(File.Substring(Path.Length + 1) & "=")
            Builder.Append(IO.File.ReadAllText(File) & "|")
        Next
        Return Builder.ToString()
    End Function

    Private Shared Function DirectoryListToString(Path As String) As String
        Dim Dirs As New List(Of String)(IO.Directory.GetDirectories(Path, "*", IO.SearchOption.AllDirectories))
        Dim Builder As New Text.StringBuilder()
        Dirs.Sort(StringComparer.InvariantCulture)
        For Each Dir As String In Dirs
            Builder.Append(Dir.Substring(Path.Length + 1) & "|")
        Next
        Return Builder.ToString()
    End Function
End Class
