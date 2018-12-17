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
        Dim TestForm As SynchronizeForm = InitHighlevelTests(ConfigPath, TempDir, StrictDate:=True, Checksum:=False, Checksize:=False)
        Test_ExpectedFilesInList(TestForm, TempDir, ShowUi)
        Test_CopyLeftPaths(TestForm, TempDir, ShowUi)
        Test_CopyRightPaths(TestForm, TempDir, ShowUi)
        Test_CopyLeftOnePath(TestForm, TempDir, ShowUi)
        Test_CopyRightOnePath(TestForm, TempDir, ShowUi)
        Test_CopyPathnames(TestForm, TempDir, ShowUi)
        'Perform sync
        Test_ExpectedFilesAfterSyncAll(TestForm, TempDir, ShowUi)

        'Last-modified-time checks 
        WriteTestFilesLmtTest(TempDir, TempDir & "\testsync\src\d1", TempDir & "\testsync\dest\d1")
        Test_StrictDate(ConfigPath, TempDir, ShowUi)
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
        Dim Expected As String = "dest\d1\deldir\fileindeldir1.txt=abc3|dest\d1\deldir\fileindeldir2.txt=abc4|dest\d1\oldfile.txt=oldfile|dest\d1\samedir\fileinsamedir.txt=a0|dest\d1\samefile.txt=abc|dest\d1\updatedbetter.txt=abc|dest\d1\updatedworse.txt=xyz123|src\d1\newdir\fileinnewdir1.txt=abc1|src\d1\newdir\fileinnewdir2.txt=abc2|src\d1\newfile.txt=newfile|src\d1\samedir\fileinsamedir.txt=a0|src\d1\samefile.txt=abc|src\d1\updatedbetter.txt=abc12345|src\d1\updatedworse.txt=xyz|"
        AssertEqual(Expected, DirectoryFileContentsToString(Dir))
        AssertEqual(10, TestForm.SyncingList.Count)
        AssertEqual("Path=\deldir Action=Delete Side=Right Type=Folder IsUpdate=None", TestUtil_ToStringWithoutRealId(TestForm.SyncingList(0)))
        AssertEqual("Path=\deldir\fileindeldir1.txt Action=Delete Side=Right Type=File IsUpdate=None", TestUtil_ToStringWithoutRealId(TestForm.SyncingList(1)))
        AssertEqual("Path=\deldir\fileindeldir2.txt Action=Delete Side=Right Type=File IsUpdate=None", TestUtil_ToStringWithoutRealId(TestForm.SyncingList(2)))
        AssertEqual("Path=\newdir Action=Copy Side=Left Type=Folder IsUpdate=None", TestUtil_ToStringWithoutRealId(TestForm.SyncingList(3)))
        AssertEqual("Path=\newdir\fileinnewdir1.txt Action=Copy Side=Left Type=File IsUpdate=None", TestUtil_ToStringWithoutRealId(TestForm.SyncingList(4)))
        AssertEqual("Path=\newdir\fileinnewdir2.txt Action=Copy Side=Left Type=File IsUpdate=None", TestUtil_ToStringWithoutRealId(TestForm.SyncingList(5)))
        AssertEqual("Path=\newfile.txt Action=Copy Side=Left Type=File IsUpdate=None", TestUtil_ToStringWithoutRealId(TestForm.SyncingList(6)))
        AssertEqual("Path=\oldfile.txt Action=Delete Side=Right Type=File IsUpdate=None", TestUtil_ToStringWithoutRealId(TestForm.SyncingList(7)))
        AssertEqual("Path=\updatedbetter.txt Action=Copy Side=Left Type=File IsUpdate=ReplaceWithNewerFile", TestUtil_ToStringWithoutRealId(TestForm.SyncingList(8)))
        AssertEqual("Path=\updatedworse.txt Action=Copy Side=Left Type=File IsUpdate=ReplaceWithOlderFile", TestUtil_ToStringWithoutRealId(TestForm.SyncingList(9)))
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

    Private Shared Sub Test_ExpectedFilesAfterSyncAll(TestForm As SynchronizeForm, TempDir As String, ShowUi As Boolean)

    End Sub

    Private Shared Sub Test_StrictDate(ConfigPath As String, TempDir As String, ShowUi As Boolean)
        'Check disk contents
        Dim Dir As String = TempDir & "\testsync"
        Dim Expected As String = "dest\d1\~content,~size,~time.txt=1234|dest\d1\~content,~size,=time.txt=1234|dest\d1\~content,=size,~time.txt=123|dest\d1\~content,=size,=time.txt=123|dest\d1\=content,=size,~time.txt=abc|dest\d1\=content,=size,=time.txt=abc|dest\d1\time+3591s.txt=abc|dest\d1\time+3599s.txt=abc|dest\d1\time+3601s.txt=abc|dest\d1\time+3609s.txt=abc|dest\d1\time+3s.txt=abc|dest\d1\time+60s.txt=abc|dest\d1\time+9s.txt=abc|dest\d1\time0s.txt=abc|dest\d1\time-3591s.txt=abc|dest\d1\time-3599s.txt=abc|dest\d1\time-3601s.txt=abc|dest\d1\time-3609s.txt=abc|dest\d1\time-3s.txt=abc|dest\d1\time-60s.txt=abc|dest\d1\time-9s.txt=abc|src\d1\~content,~size,~time.txt=abc|src\d1\~content,~size,=time.txt=abc|src\d1\~content,=size,~time.txt=abc|src\d1\~content,=size,=time.txt=abc|src\d1\=content,=size,~time.txt=abc|src\d1\=content,=size,=time.txt=abc|src\d1\time+3591s.txt=abc|src\d1\time+3599s.txt=abc|src\d1\time+3601s.txt=abc|src\d1\time+3609s.txt=abc|src\d1\time+3s.txt=abc|src\d1\time+60s.txt=abc|src\d1\time+9s.txt=abc|src\d1\time0s.txt=abc|src\d1\time-3591s.txt=abc|src\d1\time-3599s.txt=abc|src\d1\time-3601s.txt=abc|src\d1\time-3609s.txt=abc|src\d1\time-3s.txt=abc|src\d1\time-60s.txt=abc|src\d1\time-9s.txt=abc|"
        AssertEqual(Expected, DirectoryFileContentsToString(Dir))

        'Get 100% branch coverage for checking filesize, modified time, and checksum
        Using TestFormLmt As SynchronizeForm = InitHighlevelTests(ConfigPath, TempDir, Checksum:=True, StrictDate:=True, Checksize:=True)
            'Everything is included except =content,=size,=time.txt and time0s.txt
            AssertEqual(18, TestFormLmt.PreviewList.Items.Count - 1)
            AssertEqual("Path=\~content,~size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(0)))
            AssertEqual("Path=\~content,~size,=time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(1)))
            AssertEqual("Path=\~content,=size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(2)))
            AssertEqual("Path=\~content,=size,=time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(3)))
            AssertEqual("Path=\=content,=size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(4)))
            AssertEqual("Path=\time+3591s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(5)))
            AssertEqual("Path=\time+3599s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(6)))
            AssertEqual("Path=\time+3601s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(7)))
            AssertEqual("Path=\time+3609s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(8)))
            AssertEqual("Path=\time+3s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(9)))
            AssertEqual("Path=\time+60s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(10)))
            AssertEqual("Path=\time+9s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(11)))
            AssertEqual("Path=\time-3591s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(12)))
            AssertEqual("Path=\time-3599s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(13)))
            AssertEqual("Path=\time-3601s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(14)))
            AssertEqual("Path=\time-3609s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(15)))
            AssertEqual("Path=\time-3s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(16)))
            AssertEqual("Path=\time-60s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(17)))
            AssertEqual("Path=\time-9s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(18)))
        End Using
        Using TestFormLmt As SynchronizeForm = InitHighlevelTests(ConfigPath, TempDir, Checksum:=True, StrictDate:=True, Checksize:=False)
            'Same as before, since the fizesize won't change without changing the checksum
            AssertEqual(18, TestFormLmt.PreviewList.Items.Count - 1)
            AssertEqual("Path=\~content,~size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(0)))
            AssertEqual("Path=\~content,~size,=time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(1)))
            AssertEqual("Path=\~content,=size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(2)))
            AssertEqual("Path=\~content,=size,=time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(3)))
            AssertEqual("Path=\=content,=size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(4)))
            AssertEqual("Path=\time+3591s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(5)))
            AssertEqual("Path=\time+3599s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(6)))
            AssertEqual("Path=\time+3601s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(7)))
            AssertEqual("Path=\time+3609s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(8)))
            AssertEqual("Path=\time+3s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(9)))
            AssertEqual("Path=\time+60s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(10)))
            AssertEqual("Path=\time+9s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(11)))
            AssertEqual("Path=\time-3591s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(12)))
            AssertEqual("Path=\time-3599s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(13)))
            AssertEqual("Path=\time-3601s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(14)))
            AssertEqual("Path=\time-3609s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(15)))
            AssertEqual("Path=\time-3s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(16)))
            AssertEqual("Path=\time-60s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(17)))
            AssertEqual("Path=\time-9s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(18)))
        End Using
        Using TestFormLmt As SynchronizeForm = InitHighlevelTests(ConfigPath, TempDir, Checksum:=True, StrictDate:=False, Checksize:=True)
            'Same as before except doesn't include the six items around 0s or around 3600s
            AssertEqual(12, TestFormLmt.PreviewList.Items.Count - 1)
            AssertEqual("Path=\~content,~size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(0)))
            AssertEqual("Path=\~content,~size,=time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(1)))
            AssertEqual("Path=\~content,=size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(2)))
            AssertEqual("Path=\~content,=size,=time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(3)))
            AssertEqual("Path=\=content,=size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(4)))
            AssertEqual("Path=\time+3591s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(5)))
            AssertEqual("Path=\time+3609s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(6)))
            AssertEqual("Path=\time+60s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(7)))
            AssertEqual("Path=\time+9s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(8)))
            AssertEqual("Path=\time-3591s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(9)))
            AssertEqual("Path=\time-3609s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(10)))
            AssertEqual("Path=\time-60s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(11)))
            AssertEqual("Path=\time-9s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(12)))
        End Using
        Using TestFormLmt As SynchronizeForm = InitHighlevelTests(ConfigPath, TempDir, Checksum:=True, StrictDate:=False, Checksize:=False)
            'Same as before, since the fizesize won't change without changing the checksum
            AssertEqual(12, TestFormLmt.PreviewList.Items.Count - 1)
            AssertEqual("Path=\~content,~size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(0)))
            AssertEqual("Path=\~content,~size,=time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(1)))
            AssertEqual("Path=\~content,=size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(2)))
            AssertEqual("Path=\~content,=size,=time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(3)))
            AssertEqual("Path=\=content,=size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(4)))
            AssertEqual("Path=\time+3591s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(5)))
            AssertEqual("Path=\time+3609s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(6)))
            AssertEqual("Path=\time+60s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(7)))
            AssertEqual("Path=\time+9s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(8)))
            AssertEqual("Path=\time-3591s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(9)))
            AssertEqual("Path=\time-3609s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(10)))
            AssertEqual("Path=\time-60s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(11)))
            AssertEqual("Path=\time-9s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(12)))
        End Using
        Using TestFormLmt As SynchronizeForm = InitHighlevelTests(ConfigPath, TempDir, Checksum:=False, StrictDate:=True, Checksize:=True)
            'Everything is included except =content,=size,=time.txt and time0s.txt and ~content,=size,=time.txt
            AssertEqual(17, TestFormLmt.PreviewList.Items.Count - 1)
            AssertEqual("Path=\~content,~size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(0)))
            AssertEqual("Path=\~content,~size,=time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(1)))
            AssertEqual("Path=\~content,=size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(2)))
            AssertEqual("Path=\=content,=size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(3)))
            AssertEqual("Path=\time+3591s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(4)))
            AssertEqual("Path=\time+3599s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(5)))
            AssertEqual("Path=\time+3601s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(6)))
            AssertEqual("Path=\time+3609s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(7)))
            AssertEqual("Path=\time+3s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(8)))
            AssertEqual("Path=\time+60s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(9)))
            AssertEqual("Path=\time+9s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(10)))
            AssertEqual("Path=\time-3591s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(11)))
            AssertEqual("Path=\time-3599s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(12)))
            AssertEqual("Path=\time-3601s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(13)))
            AssertEqual("Path=\time-3609s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(14)))
            AssertEqual("Path=\time-3s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(15)))
            AssertEqual("Path=\time-60s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(16)))
            AssertEqual("Path=\time-9s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(17)))
        End Using
        Using TestFormLmt As SynchronizeForm = InitHighlevelTests(ConfigPath, TempDir, Checksum:=False, StrictDate:=False, Checksize:=True)
            'Same as before except doesn't include the six items around 0s or around 3600s
            AssertEqual(11, TestFormLmt.PreviewList.Items.Count - 1)
            AssertEqual("Path=\~content,~size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(0)))
            AssertEqual("Path=\~content,~size,=time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(1)))
            AssertEqual("Path=\~content,=size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(2)))
            AssertEqual("Path=\=content,=size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(3)))
            AssertEqual("Path=\time+3591s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(4)))
            AssertEqual("Path=\time+3609s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(5)))
            AssertEqual("Path=\time+60s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(6)))
            AssertEqual("Path=\time+9s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(7)))
            AssertEqual("Path=\time-3591s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(8)))
            AssertEqual("Path=\time-3609s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(9)))
            AssertEqual("Path=\time-60s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(10)))
            AssertEqual("Path=\time-9s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(11)))
        End Using
        Using TestFormLmt As SynchronizeForm = InitHighlevelTests(ConfigPath, TempDir, Checksum:=False, StrictDate:=True, Checksize:=False)
            'Everything is included except =content,=size,=time.txt and time0s.txt and ~content,=size,=time.txt and ~content,~size,=time.txt
            AssertEqual(16, TestFormLmt.PreviewList.Items.Count - 1)
            AssertEqual("Path=\~content,~size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(0)))
            AssertEqual("Path=\~content,=size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(1)))
            AssertEqual("Path=\=content,=size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(2)))
            AssertEqual("Path=\time+3591s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(3)))
            AssertEqual("Path=\time+3599s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(4)))
            AssertEqual("Path=\time+3601s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(5)))
            AssertEqual("Path=\time+3609s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(6)))
            AssertEqual("Path=\time+3s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(7)))
            AssertEqual("Path=\time+60s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(8)))
            AssertEqual("Path=\time+9s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(9)))
            AssertEqual("Path=\time-3591s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(10)))
            AssertEqual("Path=\time-3599s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(11)))
            AssertEqual("Path=\time-3601s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(12)))
            AssertEqual("Path=\time-3609s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(13)))
            AssertEqual("Path=\time-3s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(14)))
            AssertEqual("Path=\time-60s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(15)))
            AssertEqual("Path=\time-9s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(16)))
        End Using
        Using TestFormLmt As SynchronizeForm = InitHighlevelTests(ConfigPath, TempDir, Checksum:=False, StrictDate:=False, Checksize:=False)
            'Same as before except doesn't include the six items around 0s or around 3600s
            AssertEqual(10, TestFormLmt.PreviewList.Items.Count - 1)
            AssertEqual("Path=\~content,~size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(0)))
            AssertEqual("Path=\~content,=size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(1)))
            AssertEqual("Path=\=content,=size,~time.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(2)))
            AssertEqual("Path=\time+3591s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(3)))
            AssertEqual("Path=\time+3609s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(4)))
            AssertEqual("Path=\time+60s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(5)))
            AssertEqual("Path=\time+9s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(6)))
            AssertEqual("Path=\time-3591s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(7)))
            AssertEqual("Path=\time-3609s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(8)))
            AssertEqual("Path=\time-60s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(9)))
            AssertEqual("Path=\time-9s.txt Action=Copy Side=Left Type=File Update=Changed", TestUtil_ToStringWithoutUpdateType(TestFormLmt.SyncingList(10)))
        End Using
    End Sub

    Private Shared Function InitHighlevelTests(ConfigPath As String, TempDir As String, StrictDate As Boolean, Checksum As Boolean, Checksize As Boolean) As SynchronizeForm
        'Write profile settings
        Dim ProfileText As String = "Strict mirror:True|Discard after:0|Propagate Updates:True|Time Offset:0|Synchronization Method:0|Files restrictions:0|Source folders to be synchronized:*;|Destination folders to be synchronized:*;|Included Filetypes:|Excluded FileTypes:|Replicate Empty Directories:True|Check contents before deleting folders:True"
        ProfileText &= "|Strict date comparison:" & If(StrictDate, "True", "False")
        ProfileText &= "|Check file size:" & If(Checksize, "True", "False")
        ProfileText &= "|Checksum:" & If(Checksum, "True", "False")
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
        IO.File.WriteAllText(PathSrc & "\samedir\fileinsamedir.txt", "a0")
        IO.File.WriteAllText(PathSrc & "\newfile.txt", "newfile")
        IO.File.WriteAllText(PathSrc & "\updatedbetter.txt", "abc12345")
        IO.File.WriteAllText(PathSrc & "\updatedworse.txt", "xyz")
        IO.File.WriteAllText(PathSrc & "\newdir\fileinnewdir1.txt", "abc1")
        IO.File.WriteAllText(PathSrc & "\newdir\fileinnewdir2.txt", "abc2")
        IO.Directory.CreateDirectory(PathDest & "\deldir")
        IO.Directory.CreateDirectory(PathDest & "\samedir")
        IO.File.Copy(PathSrc & "\samefile.txt", PathDest & "\samefile.txt") 'Ensure same lmt
        IO.File.Copy(PathSrc & "\samedir\fileinsamedir.txt", PathDest & "\samedir\fileinsamedir.txt") 'Ensure same lmt
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

    Private Shared Sub WriteTestFilesLmtTest(TempDir As String, PathSrc As String, PathDest As String)
        If IO.Directory.Exists(TempDir) Then IO.Directory.Delete(TempDir, True)
        AssertEqual(False, IO.Directory.Exists(TempDir))
        IO.Directory.CreateDirectory(PathSrc)
        'All possible permutations.
        IO.File.WriteAllText(PathSrc & "\=content,=size,=time.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\=content,=size,~time.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\~content,=size,=time.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\~content,~size,=time.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\~content,=size,~time.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\~content,~size,~time.txt", "abc")
        'Time differences
        IO.File.WriteAllText(PathSrc & "\time0s.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\time+3s.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\time+9s.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\time+60s.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\time+3591s.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\time+3599s.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\time+3601s.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\time+3609s.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\time-3s.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\time-9s.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\time-60s.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\time-3591s.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\time-3599s.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\time-3601s.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\time-3609s.txt", "abc")
        IO.Directory.CreateDirectory(PathDest)
        IO.File.WriteAllText(PathDest & "\=content,=size,=time.txt", "abc")
        IO.File.WriteAllText(PathDest & "\=content,=size,~time.txt", "abc")
        IO.File.WriteAllText(PathDest & "\~content,=size,=time.txt", "123")
        IO.File.WriteAllText(PathDest & "\~content,~size,=time.txt", "1234")
        IO.File.WriteAllText(PathDest & "\~content,=size,~time.txt", "123")
        IO.File.WriteAllText(PathDest & "\~content,~size,~time.txt", "1234")
        IO.File.WriteAllText(PathDest & "\time0s.txt", "abc")
        IO.File.WriteAllText(PathDest & "\time+3s.txt", "abc")
        IO.File.WriteAllText(PathDest & "\time+9s.txt", "abc")
        IO.File.WriteAllText(PathDest & "\time+60s.txt", "abc")
        IO.File.WriteAllText(PathDest & "\time+3591s.txt", "abc")
        IO.File.WriteAllText(PathDest & "\time+3599s.txt", "abc")
        IO.File.WriteAllText(PathDest & "\time+3601s.txt", "abc")
        IO.File.WriteAllText(PathDest & "\time+3609s.txt", "abc")
        IO.File.WriteAllText(PathDest & "\time-3s.txt", "abc")
        IO.File.WriteAllText(PathDest & "\time-9s.txt", "abc")
        IO.File.WriteAllText(PathDest & "\time-60s.txt", "abc")
        IO.File.WriteAllText(PathDest & "\time-3591s.txt", "abc")
        IO.File.WriteAllText(PathDest & "\time-3599s.txt", "abc")
        IO.File.WriteAllText(PathDest & "\time-3601s.txt", "abc")
        IO.File.WriteAllText(PathDest & "\time-3609s.txt", "abc")

        'Set all lmts to be the same
        Dim BasisTime As Date = NTFSToFATTime(IO.File.GetLastWriteTimeUtc(PathSrc & "\time0s.txt"))
        For Each Fl As String In IO.Directory.GetFiles(TempDir, "*", IO.SearchOption.AllDirectories)
            IO.File.SetLastWriteTimeUtc(Fl, BasisTime)
        Next

        'Adjust some lmts
        IO.File.SetLastWriteTimeUtc(PathDest & "\=content,=size,~time.txt", BasisTime - New TimeSpan(0, 0, 10))
        IO.File.SetLastWriteTimeUtc(PathDest & "\~content,=size,~time.txt", BasisTime - New TimeSpan(0, 0, 10))
        IO.File.SetLastWriteTimeUtc(PathDest & "\~content,~size,~time.txt", BasisTime - New TimeSpan(0, 0, 10))
        IO.File.SetLastWriteTimeUtc(PathDest & "\time+3s.txt", BasisTime + New TimeSpan(0, 0, 3))
        IO.File.SetLastWriteTimeUtc(PathDest & "\time+9s.txt", BasisTime + New TimeSpan(0, 0, 9))
        IO.File.SetLastWriteTimeUtc(PathDest & "\time+60s.txt", BasisTime + New TimeSpan(0, 0, 60))
        IO.File.SetLastWriteTimeUtc(PathDest & "\time+3591s.txt", BasisTime + New TimeSpan(0, 0, 3591))
        IO.File.SetLastWriteTimeUtc(PathDest & "\time+3599s.txt", BasisTime + New TimeSpan(0, 0, 3599))
        IO.File.SetLastWriteTimeUtc(PathDest & "\time+3601s.txt", BasisTime + New TimeSpan(0, 0, 3601))
        IO.File.SetLastWriteTimeUtc(PathDest & "\time+3609s.txt", BasisTime + New TimeSpan(0, 0, 3609))
        IO.File.SetLastWriteTimeUtc(PathDest & "\time-3s.txt", BasisTime - New TimeSpan(0, 0, 3))
        IO.File.SetLastWriteTimeUtc(PathDest & "\time-9s.txt", BasisTime - New TimeSpan(0, 0, 9))
        IO.File.SetLastWriteTimeUtc(PathDest & "\time-60s.txt", BasisTime - New TimeSpan(0, 0, 60))
        IO.File.SetLastWriteTimeUtc(PathDest & "\time-3591s.txt", BasisTime - New TimeSpan(0, 0, 3591))
        IO.File.SetLastWriteTimeUtc(PathDest & "\time-3599s.txt", BasisTime - New TimeSpan(0, 0, 3599))
        IO.File.SetLastWriteTimeUtc(PathDest & "\time-3601s.txt", BasisTime - New TimeSpan(0, 0, 3601))
        IO.File.SetLastWriteTimeUtc(PathDest & "\time-3609s.txt", BasisTime - New TimeSpan(0, 0, 3609))
    End Sub

    Public Shared Function TestUtil_ToStringWithoutRealId(Item As SyncingItem) As String
        Return "Path=" & Item.Path & " Action=" & Item.Action.ToString() &
            " Side=" & Item.Side.ToString() & " Type=" & Item.Type.ToString() & " IsUpdate=" & Item.Update.ToString()
    End Function

    Public Shared Function TestUtil_ToStringWithoutUpdateType(Item As SyncingItem) As String
        Return "Path=" & Item.Path & " Action=" & Item.Action.ToString() &
            " Side=" & Item.Side.ToString() & " Type=" & Item.Type.ToString() & " Update=" & If(Item.Update = TypeOfUpdate.None, "None", "Changed")
    End Function

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
