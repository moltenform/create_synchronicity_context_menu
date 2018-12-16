'This file is part of Create Synchronicity.
'
'Create Synchronicity is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
'Create Synchronicity is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
'You should have received a copy of the GNU General Public License along with Create Synchronicity.  If not, see <http://www.gnu.org/licenses/>.
'Created by:	Clément Pit--Claudel.
'Web site:		http://synchronicity.sourceforge.net.

Partial Class SynchronizeForm
    Private Shared Sub RunEachTest(ConfigPath As String, TempDir As String, ShowUi As Boolean)
        Dim TestForm As SynchronizeForm = Test_MakeTestForm(TempDir)
        Using TestForm
            'Run low-level tests
            Test_ContextMenuGetPaths(TestForm)
            Test_CanRunChildWindowCopying(TestForm)
            TestForm.Close()
        End Using

        'Run high-level tests
        TestForm = InitHighlevelTests(ConfigPath, TempDir)
        Using TestForm
            TestForm.Close()
        End Using
    End Sub

    Friend Shared Sub RunTests(ShowUi As Boolean)
        Dim TempDir As String = IO.Path.Combine(IO.Path.GetTempPath(), "test_create_synchronicity")
        Dim ConfigPath As String = ProgramConfig.ConfigRootDir & "\high_level_create_sync_testonly.sync"
        Try
            RunEachTest(ConfigPath, TempDir, ShowUi)
            If IO.Directory.Exists(TempDir) Then IO.Directory.Delete(TempDir, True)
        Finally
            If IO.File.Exists(ConfigPath) Then IO.File.Delete(ConfigPath)
        End Try
    End Sub

    Private Shared Function Test_MakeTestForm(TempDir As String) As SynchronizeForm
        'Create a test form. (Remember that for a Delete action, the source is deleted.)
        Dim TestForm As New SynchronizeForm("testing", True, False)
        TestForm.Visible = False
        TestForm.LeftRootPath = "C:\left\"
        TestForm.RightRootPath = "C:\right\"
        TestForm.AddToSyncingList("file_createLR", TypeOfItem.File, SideOfSource.Left, TypeOfAction.Copy, TypeOfUpdate.None)
        TestForm.AddToSyncingList("file_updateLR", TypeOfItem.File, SideOfSource.Left, TypeOfAction.Copy, TypeOfUpdate.ReplaceWithNewerFile)
        TestForm.AddToSyncingList("file_deleteLR", TypeOfItem.File, SideOfSource.Right, TypeOfAction.Delete, TypeOfUpdate.None)
        TestForm.AddToSyncingList("folder_createLR", TypeOfItem.Folder, SideOfSource.Left, TypeOfAction.Copy, TypeOfUpdate.None)
        TestForm.AddToSyncingList("folder_deleteLR", TypeOfItem.Folder, SideOfSource.Right, TypeOfAction.Delete, TypeOfUpdate.None)
        TestForm.AddToSyncingList("file_createRL", TypeOfItem.File, SideOfSource.Right, TypeOfAction.Copy, TypeOfUpdate.None)
        TestForm.AddToSyncingList("file_updateRL", TypeOfItem.File, SideOfSource.Right, TypeOfAction.Copy, TypeOfUpdate.ReplaceWithNewerFile)
        TestForm.AddToSyncingList("file_deleteRL", TypeOfItem.File, SideOfSource.Left, TypeOfAction.Delete, TypeOfUpdate.None)
        TestForm.AddToSyncingList("folder_createRL", TypeOfItem.Folder, SideOfSource.Right, TypeOfAction.Copy, TypeOfUpdate.None)
        TestForm.AddToSyncingList("folder_deleteRL", TypeOfItem.Folder, SideOfSource.Left, TypeOfAction.Delete, TypeOfUpdate.None)
        TestForm.AddToSyncingList("a.txt", TypeOfItem.File, SideOfSource.Left, TypeOfAction.Copy, TypeOfUpdate.ReplaceWithNewerFile)
        TestForm.StepCompleted(StatusData.SyncStep.Scan)

        'Create temp files
        If IO.Directory.Exists(TempDir) Then IO.Directory.Delete(TempDir, True)
        TestForm.LeftRootPath = TempDir & "\src\"
        TestForm.RightRootPath = TempDir & "\dest\"
        IO.Directory.CreateDirectory(TempDir & "\src")
        IO.Directory.CreateDirectory(TempDir & "\dest")
        IO.File.WriteAllText(TestForm.LeftRootPath & "a.txt", "abcdefg")
        IO.File.WriteAllText(TestForm.RightRootPath & "a.txt", "abcd")

        'Adjust the timestamp of one of the files so that it is earlier on the left
        Dim SpanThreeSeconds As New TimeSpan(0, 0, 3)
        IO.File.SetLastWriteTimeUtc(TestForm.RightRootPath & "a.txt",
            IO.File.GetLastWriteTimeUtc(TestForm.RightRootPath & "a.txt") - SpanThreeSeconds)
        Return TestForm
    End Function

    Private Shared Sub Test_ContextMenuGetPaths(TestForm As SynchronizeForm)
        Dim ExpectedLefts As String() = {"C:\left\file_createLR", "C:\left\file_updateLR", "C:\left\file_deleteLR", "C:\left\folder_createLR", "C:\left\folder_deleteLR", "C:\right\file_createRL", "C:\right\file_updateRL", "C:\right\file_deleteRL", "C:\right\folder_createRL", "C:\right\folder_deleteRL"}
        Dim ExpectedRights As String() = {"C:\right\file_createLR", "C:\right\file_updateLR", "C:\right\file_deleteLR", "C:\right\folder_createLR", "C:\right\folder_deleteLR", "C:\left\file_createRL", "C:\left\file_updateRL", "C:\left\file_deleteRL", "C:\left\folder_createRL", "C:\left\folder_deleteRL"}
        For Index As Integer = 0 To ExpectedLefts.Length - 1
            TestForm.PreviewList.SelectedIndices.Clear()
            TestForm.PreviewList.SelectedIndices.Add(Index)
            AssertEqual(ExpectedLefts(Index), TestForm.GetFullPathsOfOneSelectedItem(True).Trim())
            TestForm.PreviewList.SelectedIndices.Clear()
            TestForm.PreviewList.SelectedIndices.Add(Index)
            AssertEqual(ExpectedRights(Index), TestForm.GetFullPathsOfOneSelectedItem(False).Trim())
        Next
    End Sub

    Private Shared Sub Test_CanRunChildWindowCopying(TestForm As SynchronizeForm)
        'Nothing selected; can't start
        TestForm.PreviewList.SelectedIndices.Clear()
        AssertEqual(False, TestForm.CanRunChildWindowCopying())
        'Two bad things; can't start
        TestForm.PreviewList.SelectedIndices.Clear()
        TestForm.PreviewList.SelectedIndices.Add(5)
        TestForm.PreviewList.SelectedIndices.Add(8)
        AssertEqual(False, TestForm.CanRunChildWindowCopying())
        'One good thing and one bad thing; can't start
        TestForm.PreviewList.SelectedIndices.Clear()
        TestForm.PreviewList.SelectedIndices.Add(1)
        TestForm.PreviewList.SelectedIndices.Add(5)
        AssertEqual(False, TestForm.CanRunChildWindowCopying())
        'Two good things; can start
        TestForm.PreviewList.SelectedIndices.Clear()
        TestForm.PreviewList.SelectedIndices.Add(1)
        TestForm.PreviewList.SelectedIndices.Add(3)
        AssertEqual(True, TestForm.CanRunChildWindowCopying())
        'All combinations of one selection
        For Index As Integer = 0 To TestForm.PreviewList.Items.Count - 1
            TestForm.PreviewList.SelectedIndices.Clear()
            TestForm.PreviewList.SelectedIndices.Add(Index)
            AssertEqual(Index <= 4, TestForm.CanRunChildWindowCopying())
        Next
    End Sub
    Private Shared Sub Test_CreateParentDirs(TestForm As SynchronizeForm)
        'test AddCreateDirectoryEntries, it should add two 'CreateDirectory' entries for this input.
        Dim Work1 As New List(Of SyncingItem)
        Work1.Add(MakeSyncingItem("ignoredirdelete", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.Folder, TypeOfUpdate.None))
        Work1.Add(MakeSyncingItem("ignorefiledelete", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.None))
        Work1.Add(MakeSyncingItem("ignorefileupdate\a.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.ReplaceWithNewerFile))
        Work1.Add(MakeSyncingItem("addeddir", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.Folder, TypeOfUpdate.None))
        Work1.Add(MakeSyncingItem("needdir1\a.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.None))
        Work1.Add(MakeSyncingItem("needdir2\a.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.None))
        Work1.Add(MakeSyncingItem("needdir2\b.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.None))
        Dim Work1Result As List(Of SyncingItem) = ChildWindowCopy_CreateParentDirs(TestForm.RightRootPath, SideOfSource.Left, Work1)
        AssertEqual(Work1.Count + 2, Work1Result.Count)
        AssertEqual(Work1(0), Work1Result(0))
        AssertEqual(Work1(1), Work1Result(1))
        AssertEqual(Work1(2), Work1Result(2))
        AssertEqual(Work1(3), Work1Result(3))
        AssertEqual(MakeSyncingItem("needdir1", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.Folder).ToString(), Work1Result(4))
        AssertEqual(Work1(4), Work1Result(5))
        AssertEqual(MakeSyncingItem("needdir2", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.Folder).ToString(), Work1Result(6))
        AssertEqual(Work1(5), Work1Result(7))
        AssertEqual(Work1(6), Work1Result(8))
    End Sub

    Private Shared Sub Test_IsDirectoryAlreadyDeleted(TestForm As SynchronizeForm)
        Dim DeletedDirectories As New Dictionary(Of String, Boolean)
        AssertEqual(False, IsRedundantDelete(DeletedDirectories, MakeSyncingItem("d1\s1", TypeOfAction.Delete, SideOfSource.Left, TypeOfItem.Folder)))
        AssertEqual(True, IsRedundantDelete(DeletedDirectories, MakeSyncingItem("d1\s1", TypeOfAction.Delete, SideOfSource.Left, TypeOfItem.Folder)))
        AssertEqual(True, IsRedundantDelete(DeletedDirectories, MakeSyncingItem("d1\s1\g2", TypeOfAction.Delete, SideOfSource.Left, TypeOfItem.Folder)))
        AssertEqual(True, IsRedundantDelete(DeletedDirectories, MakeSyncingItem("d1\s1\g1\g1\g1", TypeOfAction.Delete, SideOfSource.Left, TypeOfItem.Folder)))
        AssertEqual(True, IsRedundantDelete(DeletedDirectories, MakeSyncingItem("d1\s1\c.txt", TypeOfAction.Delete, SideOfSource.Left, TypeOfItem.File)))
        AssertEqual(True, IsRedundantDelete(DeletedDirectories, MakeSyncingItem("d1\s1\g2\g2\c.txt", TypeOfAction.Delete, SideOfSource.Left, TypeOfItem.File)))
        AssertEqual(False, IsRedundantDelete(DeletedDirectories, MakeSyncingItem("d1\s1", TypeOfAction.Delete, SideOfSource.Left, TypeOfItem.File)))
        AssertEqual(False, IsRedundantDelete(DeletedDirectories, MakeSyncingItem("d1\s1.txt", TypeOfAction.Delete, SideOfSource.Left, TypeOfItem.File)))
        AssertEqual(False, IsRedundantDelete(DeletedDirectories, MakeSyncingItem("d1\s123.txt", TypeOfAction.Delete, SideOfSource.Left, TypeOfItem.File)))
        AssertEqual(False, IsRedundantDelete(DeletedDirectories, MakeSyncingItem("d1\s12", TypeOfAction.Delete, SideOfSource.Left, TypeOfItem.File)))
    End Sub

    Private Shared Sub Test_CreateList(TestForm As SynchronizeForm)
        TestForm.PreviewList.SelectedIndices.Clear()
        For Index As Integer = 0 To 4
            TestForm.PreviewList.SelectedIndices.Add(Index)
        Next

        'Should simply copy the items into the list
        Dim Work1Result As List(Of SyncingItem) = TestForm.ChildWindowCopy_CreateList(True)
        AssertEqual(5, Work1Result.Count)
        For Index As Integer = 0 To 4
            AssertEqual(TestForm.PreviewList.SelectedIndices(Index), Work1Result(Index))
        Next

        'Should turn all Copies into Deletes and vice versa.
        Dim Work2Result As List(Of SyncingItem) = TestForm.ChildWindowCopy_CreateList(False)
        AssertEqual(5, Work2Result.Count)
        AssertEqual(MakeSyncingItem("file_createLR", TypeOfAction.Delete, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.None, 0), Work2Result(0))
        AssertEqual(MakeSyncingItem("file_updateLR", TypeOfAction.Copy, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.ReplaceWithNewerFile, 1), Work2Result(1))
        AssertEqual(MakeSyncingItem("file_deleteLR", TypeOfAction.Copy, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.None, 2), Work2Result(2))
        AssertEqual(MakeSyncingItem("folder_createLR", TypeOfAction.Delete, SideOfSource.Left, TypeOfItem.Folder, TypeOfUpdate.None, 3), Work2Result(3))
        AssertEqual(MakeSyncingItem("folder_deleteLR", TypeOfAction.Copy, SideOfSource.Right, TypeOfItem.Folder, TypeOfUpdate.None, 4), Work2Result(4))
    End Sub

    Private Shared Sub Test_DidItemCompleteSuccessfully(TestForm As SynchronizeForm, TempDir As String)
        'Simple cases
        IO.Directory.CreateDirectory(TempDir & "\dest\createddir")
        AssertEqual(True, TestForm.DidSyncingItemComplete(TestForm.LeftRootPath, TestForm.RightRootPath, MakeSyncingItem("zz.txt", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.File)))
        AssertEqual(False, TestForm.DidSyncingItemComplete(TestForm.LeftRootPath, TestForm.RightRootPath, MakeSyncingItem("a.txt", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.File)))
        AssertEqual(True, TestForm.DidSyncingItemComplete(TestForm.LeftRootPath, TestForm.RightRootPath, MakeSyncingItem("createddir", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.Folder)))
        AssertEqual(False, TestForm.DidSyncingItemComplete(TestForm.LeftRootPath, TestForm.RightRootPath, MakeSyncingItem("notdir", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.Folder)))
        AssertEqual(True, TestForm.DidSyncingItemComplete(TestForm.LeftRootPath, TestForm.RightRootPath, MakeSyncingItem("notdir", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.Folder)))
        AssertEqual(False, TestForm.DidSyncingItemComplete(TestForm.LeftRootPath, TestForm.RightRootPath, MakeSyncingItem("createddir", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.Folder)))
        'Only one side exists
        IO.File.WriteAllText(TestForm.LeftRootPath & "comp.txt", "abcdefg")
        AssertEqual(False, TestForm.DidSyncingItemComplete(TestForm.LeftRootPath, TestForm.RightRootPath, MakeSyncingItem("comp.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File)))
        IO.File.Move(TestForm.LeftRootPath & "comp.txt", TestForm.RightRootPath & "comp.txt")
        AssertEqual(False, TestForm.DidSyncingItemComplete(TestForm.LeftRootPath, TestForm.RightRootPath, MakeSyncingItem("comp.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File)))
        'Both sides exist
        IO.File.WriteAllText(TestForm.LeftRootPath & "comp.txt", "abcdefg")
        IO.File.Copy(TestForm.LeftRootPath & "comp.txt", TestForm.RightRootPath & "comp.txt", True)
        AssertEqual(True, TestForm.DidSyncingItemComplete(TestForm.LeftRootPath, TestForm.RightRootPath, MakeSyncingItem("comp.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File)))
        'RightRootPath is older
        Dim SpanThreeSeconds As New TimeSpan(0, 0, 3)
        IO.File.SetLastWriteTimeUtc(TestForm.RightRootPath & "comp.txt", IO.File.GetLastWriteTimeUtc(TestForm.RightRootPath & "comp.txt") - SpanThreeSeconds)
        AssertEqual(False, TestForm.DidSyncingItemComplete(TestForm.LeftRootPath, TestForm.RightRootPath, MakeSyncingItem("comp.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File)))
        'RightRootPath is newer
        IO.File.SetLastWriteTimeUtc(TestForm.RightRootPath & "comp.txt", IO.File.GetLastWriteTimeUtc(TestForm.RightRootPath & "comp.txt") + SpanThreeSeconds + SpanThreeSeconds)
        AssertEqual(False, TestForm.DidSyncingItemComplete(TestForm.LeftRootPath, TestForm.RightRootPath, MakeSyncingItem("comp.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File)))
    End Sub

    Private Shared Sub Test_SkipDeletingNewlyCopiedFile()
        'Set up mock data
        Dim MockCreatedFiles As String() = New String() {"dir1\a.txt", "dir1\b.txt", "dir2\c.txt"}
        Dim Work As New List(Of SyncingItem)
        Work.Add(MakeSyncingItem("LRignoredirdelete", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.Folder, TypeOfUpdate.None))
        Work.Add(MakeSyncingItem("LRignorefiledelete", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.None))
        Work.Add(MakeSyncingItem("LRignorefileupdate\a.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.ReplaceWithNewerFile))
        Work.Add(MakeSyncingItem("LRignoreaddeddir", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.Folder, TypeOfUpdate.None))
        Work.Add(MakeSyncingItem("LRignorefileadd\a.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.None))
        Work.Add(MakeSyncingItem("dir1", TypeOfAction.Delete, SideOfSource.Left, TypeOfItem.Folder, TypeOfUpdate.None)) 'delete is in wrong direction
        Work.Add(MakeSyncingItem("dir1", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.Folder, TypeOfUpdate.None))
        Work.Add(MakeSyncingItem("RLignoredirdelete", TypeOfAction.Delete, SideOfSource.Left, TypeOfItem.Folder, TypeOfUpdate.None))
        Work.Add(MakeSyncingItem("RLignorefiledelete", TypeOfAction.Delete, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.None))
        Work.Add(MakeSyncingItem("RLignorefileupdate\a.txt", TypeOfAction.Copy, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.ReplaceWithNewerFile))
        Work.Add(MakeSyncingItem("RLignoreaddeddir", TypeOfAction.Copy, SideOfSource.Right, TypeOfItem.Folder, TypeOfUpdate.None))
        Work.Add(MakeSyncingItem("RLignorefileadd\a.txt", TypeOfAction.Copy, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.None))
        Work.Add(MakeSyncingItem("DIR2", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.Folder, TypeOfUpdate.None)) 'test case-sensitivity

        'Run ChildWindowCopy_SkipDeletingNewlyCopiedFile
        Dim WorkResult As New List(Of SyncingItem)(Work)
        ChildWindowCopy_SkipDeletingNewlyCopiedFile(WorkResult, New List(Of String)(MockCreatedFiles))
        AssertEqual(Work.Count - 2, WorkResult.Count)
        AssertEqual(Work(0), WorkResult(0))
        AssertEqual(Work(1), WorkResult(1))
        AssertEqual(Work(2), WorkResult(2))
        AssertEqual(Work(3), WorkResult(3))
        AssertEqual(Work(4), WorkResult(4))
        AssertEqual(Work(5), WorkResult(5))
        AssertEqual(Work(7), WorkResult(6))
        AssertEqual(Work(8), WorkResult(7))
        AssertEqual(Work(9), WorkResult(8))
        AssertEqual(Work(10), WorkResult(9))
        AssertEqual(Work(11), WorkResult(10))
    End Sub

    Private Shared Function InitHighlevelTests(ConfigPath As String, TempDir As String) As SynchronizeForm
        'write test files on disk
        If IO.Directory.Exists(TempDir) Then IO.Directory.Delete(TempDir, True)
        WriteTestFiles(TempDir & "\testsync\src\d1", TempDir & "\testsync\dest\d1")
        WriteTestFiles(TempDir & "\testsync\src\d2", TempDir & "\testsync\dest\d2")
        WriteTestFiles(TempDir & "\testsync\src\d3", TempDir & "\testsync\dest\d3")

        'write profile settings
        Dim ProfileText As String = "Strict mirror:True|Discard after:0|Strict date comparison:True|Propagate Updates:True|Time Offset:0|Checksum:False|Check file size:False|Synchronization Method:0|Files restrictions:0|Source folders to be synchronized:*;|Destination folders to be synchronized:*;|Included Filetypes:|Excluded FileTypes:|Replicate Empty Directories:True|Check contents before deleting folders:True"
        ProfileText &= "|Source Directory:" & TempDir & "\testsync\src\"
        ProfileText &= "|Destination Directory:" & TempDir & "\testsync\dest\"
        ProfileText &= "|Indicate if destination is newer:True"
        ProfileText = ProfileText.Replace("|", Environment.NewLine)
        IO.File.WriteAllText(ConfigPath, ProfileText)

        'run scan and sort by path
        Dim TestForm As New SynchronizeForm("high_level_create_sync_testonly", True, False)
        TestForm.Visible = False
        TestForm.Scan() 'run on the main thread
        TestForm.PreviewList_ColumnClick(Nothing, New ColumnClickEventArgs(3)) 'now sorted by path z-a
        TestForm.PreviewList_ColumnClick(Nothing, New ColumnClickEventArgs(3)) 'now sorted by path a-z
        Return TestForm
    End Function

    Private Shared Sub Test_SyncingList(TestForm As SynchronizeForm)
        AssertEqual(27, TestForm.SyncingList.Count)
        Dim Index As Integer = -1
        For I As Integer = 0 To 2
            Dim Dir As String = "\d" & (I + 1).ToString()
            Index += 1 : AssertEqual(MakeSyncingItem(Dir & "\deldir", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.Folder, TypeOfUpdate.None, TestForm.SyncingList(Index).RealId), TestForm.SyncingList(Index))
            Index += 1 : AssertEqual(MakeSyncingItem(Dir & "\deldir\fileindeldir1.txt", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.None, TestForm.SyncingList(Index).RealId), TestForm.SyncingList(Index))
            Index += 1 : AssertEqual(MakeSyncingItem(Dir & "\deldir\fileindeldir2.txt", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.None, TestForm.SyncingList(Index).RealId), TestForm.SyncingList(Index))
            Index += 1 : AssertEqual(MakeSyncingItem(Dir & "\newdir", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.Folder, TypeOfUpdate.None, TestForm.SyncingList(Index).RealId), TestForm.SyncingList(Index))
            Index += 1 : AssertEqual(MakeSyncingItem(Dir & "\newdir\fileinnewdir.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.None, TestForm.SyncingList(Index).RealId), TestForm.SyncingList(Index))
            Index += 1 : AssertEqual(MakeSyncingItem(Dir & "\newfile.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.None, TestForm.SyncingList(Index).RealId), TestForm.SyncingList(Index))
            Index += 1 : AssertEqual(MakeSyncingItem(Dir & "\oldfile.txt", TypeOfAction.Delete, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.None, TestForm.SyncingList(Index).RealId), TestForm.SyncingList(Index))
            Index += 1 : AssertEqual(MakeSyncingItem(Dir & "\updatedbetter.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.ReplaceWithNewerFile, TestForm.SyncingList(Index).RealId), TestForm.SyncingList(Index))
            Index += 1 : AssertEqual(MakeSyncingItem(Dir & "\updatedworse.txt", TypeOfAction.Copy, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.ReplaceWithOlderFile, TestForm.SyncingList(Index).RealId), TestForm.SyncingList(Index))
        Next
    End Sub

    Private Shared Sub Test_CanRunChildWindowCopyingHighlevel(TestForm As SynchronizeForm)
        For I As Integer = 0 To TestForm.SyncingList.Count - 1
            TestForm.PreviewList.SelectedIndices.Clear()
            TestForm.PreviewList.SelectedIndices.Add(I)
            AssertEqual(True, TestForm.CanRunChildWindowCopying())
        Next
    End Sub

    Private Shared Sub Test_PerformSync(TestForm As SynchronizeForm, ShowUi As Boolean)
        TestForm.PreviewList.SelectedIndices.Clear()
        For I As Integer = 0 To 8
            TestForm.PreviewList.SelectedIndices.Add(I)
        Next

        Dim OriginalList As List(Of SyncingItem) = New List(Of SyncingItem)(TestForm.SyncingList)
        If ShowUi Then Interaction.ShowMsg("Please click 'Synchronize', and then when complete, click 'Close', in the next dialog.")
        Dim ResultList As List(Of SyncingItem) = TestForm.ChildWindowCopy(True, If(ShowUi, StartWithoutAsking.None, StartWithoutAsking.Start))

        'verify contents of ResultList
        AssertEqual(18, TestForm.SyncingList.Count)
        AssertEqual(7, ResultList.Count)
        AssertEqual(0, ResultList(0).RealId)
        ResultList(0).RealId = OriginalList(0).RealId
        AssertEqual(ResultList(0), OriginalList(0))
        AssertEqual(True, TestForm.DidSyncingItemComplete(TestForm.LeftRootPath, TestForm.RightRootPath, ResultList(0)))
        For I As Integer = 1 To 6
            AssertEqual(I, ResultList(I).RealId)
            ResultList(I).RealId = OriginalList(I + 2).RealId
            AssertEqual(ResultList(I), OriginalList(I + 2))
            AssertEqual(True, TestForm.DidSyncingItemComplete(TestForm.LeftRootPath, TestForm.RightRootPath, ResultList(I)))
        Next

        'check files on disk
        Dim ExpectedDirs1 As String = "newdir|samedir|"
        Dim ExpectedFiles1 As String = "newdir\fileinnewdir.txt=abc|newfile.txt=newfile|samefile.txt=abc|updatedbetter.txt=abc12345|updatedworse.txt=xyz|"
        AssertEqual(ExpectedDirs1, DirectoryListToString(TestForm.LeftRootPath & "\d1"))
        AssertEqual(ExpectedDirs1, DirectoryListToString(TestForm.RightRootPath & "\d1"))
        AssertEqual(ExpectedFiles1, DirectoryFileContentsToString(TestForm.LeftRootPath & "\d1"))
        AssertEqual(ExpectedFiles1, DirectoryFileContentsToString(TestForm.RightRootPath & "\d1"))
    End Sub

    Private Shared Sub Test_PerformSyncRightToLeft(TestForm As SynchronizeForm, ShowUi As Boolean)
        TestForm.PreviewList.SelectedIndices.Clear()
        For I As Integer = 0 To 8
            TestForm.PreviewList.SelectedIndices.Add(I)
        Next
        If ShowUi Then Interaction.ShowMsg("Please click 'Synchronize', and then when complete, click 'Close', in the next dialog.")
        Dim ResultList As List(Of SyncingItem) = TestForm.ChildWindowCopy(False, If(ShowUi, StartWithoutAsking.None, StartWithoutAsking.Start))

        'verify contents of ResultList
        AssertEqual(9, TestForm.SyncingList.Count)
        AssertEqual(8, ResultList.Count)
        AssertEqual(MakeSyncingItem("\d2\deldir", TypeOfAction.Copy, SideOfSource.Right, TypeOfItem.Folder, TypeOfUpdate.None, 0), ResultList(0))
        AssertEqual(MakeSyncingItem("\d2\deldir\fileindeldir1.txt", TypeOfAction.Copy, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.None, 1), ResultList(1))
        AssertEqual(MakeSyncingItem("\d2\deldir\fileindeldir2.txt", TypeOfAction.Copy, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.None, 2), ResultList(2))
        AssertEqual(MakeSyncingItem("\d2\newdir", TypeOfAction.Delete, SideOfSource.Left, TypeOfItem.Folder, TypeOfUpdate.None, 3), ResultList(3))
        AssertEqual(MakeSyncingItem("\d2\newfile.txt", TypeOfAction.Delete, SideOfSource.Left, TypeOfItem.File, TypeOfUpdate.None, 4), ResultList(4))
        AssertEqual(MakeSyncingItem("\d2\oldfile.txt", TypeOfAction.Copy, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.None, 5), ResultList(5))
        AssertEqual(MakeSyncingItem("\d2\updatedbetter.txt", TypeOfAction.Copy, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.ReplaceWithNewerFile, 6), ResultList(6))
        AssertEqual(MakeSyncingItem("\d2\updatedworse.txt", TypeOfAction.Copy, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.ReplaceWithOlderFile, 7), ResultList(7))

        'verify files on disk
        Dim ExpectedDirs2 As String = "deldir|samedir|"
        Dim ExpectedFiles2 As String = "deldir\fileindeldir1.txt=abc|deldir\fileindeldir2.txt=abc|oldfile.txt=oldfile|samefile.txt=abc|updatedbetter.txt=abc|updatedworse.txt=xyz123|"
        AssertEqual(ExpectedDirs2, DirectoryListToString(TestForm.LeftRootPath & "\d2"))
        AssertEqual(ExpectedDirs2, DirectoryListToString(TestForm.RightRootPath & "\d2"))
        AssertEqual(ExpectedFiles2, DirectoryFileContentsToString(TestForm.LeftRootPath & "\d2"))
        AssertEqual(ExpectedFiles2, DirectoryFileContentsToString(TestForm.RightRootPath & "\d2"))
    End Sub

    Private Shared Sub Test_CanceledSync(TestForm As SynchronizeForm, ShowUi As Boolean)
        TestForm.PreviewList.SelectedIndices.Clear()
        For I As Integer = 0 To 8
            TestForm.PreviewList.SelectedIndices.Add(I)
        Next
        If ShowUi Then Interaction.ShowMsg("Please click 'Close', to cancel the next dialog.")
        Dim ResultList As List(Of SyncingItem) = TestForm.ChildWindowCopy(True, If(ShowUi, StartWithoutAsking.None, StartWithoutAsking.Cancel))

        'verify contents of ResultList
        AssertEqual(7, ResultList.Count)
        AssertEqual(9, TestForm.SyncingList.Count)
        For I As Integer = 0 To 6
            AssertEqual(False, TestForm.DidSyncingItemComplete(TestForm.LeftRootPath, TestForm.RightRootPath, ResultList(I)))
            AssertEqual(False, TestForm.DidSyncingItemComplete(TestForm.LeftRootPath, TestForm.RightRootPath, TestForm.SyncingList(I)))
        Next

        'verify files on disk
        Dim ExpectedDirs1 As String = "newdir|samedir|"
        Dim ExpectedDirs2 As String = "deldir|samedir|"
        Dim ExpectedFiles1 As String = "newdir\fileinnewdir.txt=abc|newfile.txt=newfile|samefile.txt=abc|updatedbetter.txt=abc12345|updatedworse.txt=xyz|"
        Dim ExpectedFiles2 As String = "deldir\fileindeldir1.txt=abc|deldir\fileindeldir2.txt=abc|oldfile.txt=oldfile|samefile.txt=abc|updatedbetter.txt=abc|updatedworse.txt=xyz123|"
        AssertEqual(ExpectedDirs1, DirectoryListToString(TestForm.LeftRootPath & "\d3"))
        AssertEqual(ExpectedDirs2, DirectoryListToString(TestForm.RightRootPath & "\d3"))
        AssertEqual(ExpectedFiles1, DirectoryFileContentsToString(TestForm.LeftRootPath & "\d3"))
        AssertEqual(ExpectedFiles2, DirectoryFileContentsToString(TestForm.RightRootPath & "\d3"))
    End Sub
    Private Shared Sub Test_CaseThatRequiresAddCreateDirectoryEntries(TestForm As SynchronizeForm, ShowUi As Boolean)
        TestForm.PreviewList.SelectedIndices.Clear() : TestForm.PreviewList.SelectedIndices.Add(2)
        AssertEqual(False, IO.File.Exists(TestForm.LeftRootPath & "\d3\deldir\fileindeldir2.txt"))
        Dim OriginalList As List(Of SyncingItem) = New List(Of SyncingItem)(TestForm.SyncingList)
        If ShowUi Then Interaction.ShowMsg("Please click 'Synchronize', and then when complete, click 'Close', in the next dialog.")
        Dim ResultList As List(Of SyncingItem) = TestForm.ChildWindowCopy(False, If(ShowUi, StartWithoutAsking.None, StartWithoutAsking.Start))

        'verify contents of ResultList
        AssertEqual(2, ResultList.Count)
        AssertEqual(MakeSyncingItem("\d3\deldir", TypeOfAction.Copy, SideOfSource.Right, TypeOfItem.Folder, TypeOfUpdate.None, 0), ResultList(0))
        AssertEqual(MakeSyncingItem("\d3\deldir\fileindeldir2.txt", TypeOfAction.Copy, SideOfSource.Right, TypeOfItem.File, TypeOfUpdate.None, 1), ResultList(1))
        AssertEqual(True, TestForm.DidSyncingItemComplete(TestForm.LeftRootPath, TestForm.RightRootPath, ResultList(0)))
        AssertEqual(True, TestForm.DidSyncingItemComplete(TestForm.LeftRootPath, TestForm.RightRootPath, ResultList(1)))
        AssertEqual(True, IO.File.Exists(TestForm.LeftRootPath & "\d3\deldir\fileindeldir2.txt"))

        'ChildWindowCopy_SkipDeletingNewlyCopiedFile should have removed the first SyncingList item
        AssertEqual(7, TestForm.SyncingList.Count)
        AssertEqual(OriginalList(19), TestForm.SyncingList(0))
        For I As Integer = 1 To 6
            AssertEqual(OriginalList(20 + I), TestForm.SyncingList(I))
        Next
    End Sub

    Private Shared Sub AssertEqual(Expected As Object, Received As Object)
        If (Expected.ToString() <> Received.ToString()) Then
            MessageBox.Show("Test failed: " & Expected.ToString() & " != " & Received.ToString())
            Throw New Exception()
        End If
    End Sub

    Private Shared Sub WriteTestFiles(PathSrc As String, PathDest As String)
        IO.Directory.CreateDirectory(PathSrc & "\newdir")
        IO.Directory.CreateDirectory(PathSrc & "\samedir")
        IO.File.WriteAllText(PathSrc & "\samefile.txt", "abc")
        IO.File.WriteAllText(PathSrc & "\newfile.txt", "newfile")
        IO.File.WriteAllText(PathSrc & "\updatedbetter.txt", "abc12345")
        IO.File.WriteAllText(PathSrc & "\updatedworse.txt", "xyz")
        IO.File.WriteAllText(PathSrc & "\newdir\fileinnewdir.txt", "abc")
        IO.Directory.CreateDirectory(PathDest & "\deldir")
        IO.Directory.CreateDirectory(PathDest & "\samedir")
        IO.File.Copy(PathSrc & "\samefile.txt", PathDest & "\samefile.txt")
        IO.File.WriteAllText(PathDest & "\oldfile.txt", "oldfile")
        IO.File.WriteAllText(PathDest & "\deldir\fileindeldir1.txt", "abc")
        IO.File.WriteAllText(PathDest & "\deldir\fileindeldir2.txt", "abc")
        IO.File.WriteAllText(PathDest & "\updatedbetter.txt", "abc")
        IO.File.WriteAllText(PathDest & "\updatedworse.txt", "xyz123")
        Dim SpanTenSeconds As New TimeSpan(0, 0, 10)
        IO.File.SetLastWriteTimeUtc(PathDest & "\updatedbetter.txt", IO.File.GetLastWriteTimeUtc(PathSrc & "\updatedbetter.txt") - SpanTenSeconds)
        IO.File.SetLastWriteTimeUtc(PathSrc & "\updatedworse.txt", IO.File.GetLastWriteTimeUtc(PathDest & "\updatedbetter.txt") - SpanTenSeconds)
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
