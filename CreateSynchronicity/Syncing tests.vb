﻿'This file is part of Create Synchronicity.
'
'Create Synchronicity is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
'Create Synchronicity is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
'You should have received a copy of the GNU General Public License along with Create Synchronicity.  If not, see <http://www.gnu.org/licenses/>.
'Created by:	Clément Pit--Claudel.
'Web site:		http://synchronicity.sourceforge.net.

Partial Class SynchronizeForm
    Private Shared Sub AssertEqual(Expected As Object, Received As Object)
        If (Expected.ToString() <> Received.ToString()) Then
            MessageBox.Show("Test failed: " & Expected.ToString() & " != " & Received.ToString())
            Throw New Exception()
        End If
    End Sub

    Friend Shared Sub Tests()
    End Sub

    Friend Shared Sub RunHighLevelTests(ShowUi As Boolean)
    End Sub
End Class
