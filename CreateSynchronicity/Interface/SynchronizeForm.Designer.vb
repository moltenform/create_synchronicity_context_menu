﻿'This file is part of Create Synchronicity.
'
'Create Synchronicity is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
'Create Synchronicity is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
'You should have received a copy of the GNU General Public License along with Create Synchronicity.  If not, see <http://www.gnu.org/licenses/>.
'Created by:	Clément Pit--Claudel.
'Web site:		http://synchronicity.sourceforge.net.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SynchronizeForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SynchronizeForm))
        Me.MainLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.Step3LayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.Step3_ProgressLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.Step3StatusLabel = New System.Windows.Forms.Label()
        Me.Step3ProgressBar = New System.Windows.Forms.ProgressBar()
        Me.Step3Label = New System.Windows.Forms.Label()
        Me.Step2LayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.Step2ProgressLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.Step2StatusLabel = New System.Windows.Forms.Label()
        Me.Step2ProgressBar = New System.Windows.Forms.ProgressBar()
        Me.Step2Label = New System.Windows.Forms.Label()
        Me.Step1LayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.Step1ProgressLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.Step1StatusLabel = New System.Windows.Forms.Label()
        Me.Step1ProgressBar = New System.Windows.Forms.ProgressBar()
        Me.Step1Label = New System.Windows.Forms.Label()
        Me.ButtonsLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.SyncBtn = New System.Windows.Forms.Button()
        Me.StopBtn = New System.Windows.Forms.Button()
        Me.StatisticsPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.FilesCreatedLabel = New System.Windows.Forms.Label()
        Me.FilesCreated = New System.Windows.Forms.Label()
        Me.FilesDeletedLabel = New System.Windows.Forms.Label()
        Me.FilesDeleted = New System.Windows.Forms.Label()
        Me.FoldersCreatedLabel = New System.Windows.Forms.Label()
        Me.FoldersCreated = New System.Windows.Forms.Label()
        Me.FoldersDeletedLabel = New System.Windows.Forms.Label()
        Me.FoldersDeleted = New System.Windows.Forms.Label()
        Me.ElapsedTimeLabel = New System.Windows.Forms.Label()
        Me.ElapsedTime = New System.Windows.Forms.Label()
        Me.SpeedLabel = New System.Windows.Forms.Label()
        Me.Speed = New System.Windows.Forms.Label()
        Me.DoneLabel = New System.Windows.Forms.Label()
        Me.Done = New System.Windows.Forms.Label()
        Me.PreviewList = New System.Windows.Forms.ListView()
        Me.TypeColumn = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ActionColumn = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.DirectionColumn = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.PathColumn = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SyncingIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.SyncingTimer = New System.Windows.Forms.Timer(Me.components)
        Me.MainLayoutPanel.SuspendLayout()
        Me.Step3LayoutPanel.SuspendLayout()
        Me.Step3_ProgressLayout.SuspendLayout()
        Me.Step2LayoutPanel.SuspendLayout()
        Me.Step2ProgressLayout.SuspendLayout()
        Me.Step1LayoutPanel.SuspendLayout()
        Me.Step1ProgressLayout.SuspendLayout()
        Me.ButtonsLayoutPanel.SuspendLayout()
        Me.StatisticsPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainLayoutPanel
        '
        Me.MainLayoutPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MainLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
        Me.MainLayoutPanel.ColumnCount = 1
        Me.MainLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.MainLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23.0!))
        Me.MainLayoutPanel.Controls.Add(Me.Step3LayoutPanel, 0, 2)
        Me.MainLayoutPanel.Controls.Add(Me.Step2LayoutPanel, 0, 1)
        Me.MainLayoutPanel.Controls.Add(Me.Step1LayoutPanel, 0, 0)
        Me.MainLayoutPanel.Location = New System.Drawing.Point(12, 12)
        Me.MainLayoutPanel.MaximumSize = New System.Drawing.Size(65536, 400)
        Me.MainLayoutPanel.Name = "MainLayoutPanel"
        Me.MainLayoutPanel.RowCount = 3
        Me.MainLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.MainLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.MainLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.MainLayoutPanel.Size = New System.Drawing.Size(601, 268)
        Me.MainLayoutPanel.TabIndex = 0
        '
        'Step3LayoutPanel
        '
        Me.Step3LayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset
        Me.Step3LayoutPanel.ColumnCount = 1
        Me.Step3LayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.Step3LayoutPanel.Controls.Add(Me.Step3_ProgressLayout, 0, 1)
        Me.Step3LayoutPanel.Controls.Add(Me.Step3Label, 0, 0)
        Me.Step3LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Step3LayoutPanel.Location = New System.Drawing.Point(4, 182)
        Me.Step3LayoutPanel.Name = "Step3LayoutPanel"
        Me.Step3LayoutPanel.RowCount = 2
        Me.Step3LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.Step3LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.Step3LayoutPanel.Size = New System.Drawing.Size(593, 82)
        Me.Step3LayoutPanel.TabIndex = 2
        '
        'Step3_ProgressLayout
        '
        Me.Step3_ProgressLayout.ColumnCount = 1
        Me.Step3_ProgressLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.Step3_ProgressLayout.Controls.Add(Me.Step3StatusLabel, 0, 0)
        Me.Step3_ProgressLayout.Controls.Add(Me.Step3ProgressBar, 0, 1)
        Me.Step3_ProgressLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Step3_ProgressLayout.Location = New System.Drawing.Point(5, 30)
        Me.Step3_ProgressLayout.Name = "Step3_ProgressLayout"
        Me.Step3_ProgressLayout.RowCount = 2
        Me.Step3_ProgressLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.Step3_ProgressLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.Step3_ProgressLayout.Size = New System.Drawing.Size(583, 47)
        Me.Step3_ProgressLayout.TabIndex = 0
        '
        'Step3StatusLabel
        '
        Me.Step3StatusLabel.AutoEllipsis = True
        Me.Step3StatusLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Step3StatusLabel.Location = New System.Drawing.Point(3, 0)
        Me.Step3StatusLabel.Name = "Step3StatusLabel"
        Me.Step3StatusLabel.Size = New System.Drawing.Size(577, 14)
        Me.Step3StatusLabel.TabIndex = 2
        Me.Step3StatusLabel.Text = "\WAITING"
        Me.Step3StatusLabel.UseMnemonic = False
        '
        'Step3ProgressBar
        '
        Me.Step3ProgressBar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Step3ProgressBar.Location = New System.Drawing.Point(3, 17)
        Me.Step3ProgressBar.Name = "Step3ProgressBar"
        Me.Step3ProgressBar.Size = New System.Drawing.Size(577, 27)
        Me.Step3ProgressBar.TabIndex = 3
        '
        'Step3Label
        '
        Me.Step3Label.AutoSize = True
        Me.Step3Label.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Step3Label.Location = New System.Drawing.Point(5, 2)
        Me.Step3Label.Name = "Step3Label"
        Me.Step3Label.Padding = New System.Windows.Forms.Padding(0, 5, 0, 5)
        Me.Step3Label.Size = New System.Drawing.Size(583, 23)
        Me.Step3Label.TabIndex = 1
        Me.Step3Label.Text = "\STEP_3"
        '
        'Step2LayoutPanel
        '
        Me.Step2LayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset
        Me.Step2LayoutPanel.ColumnCount = 1
        Me.Step2LayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.Step2LayoutPanel.Controls.Add(Me.Step2ProgressLayout, 0, 1)
        Me.Step2LayoutPanel.Controls.Add(Me.Step2Label, 0, 0)
        Me.Step2LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Step2LayoutPanel.Location = New System.Drawing.Point(4, 93)
        Me.Step2LayoutPanel.Name = "Step2LayoutPanel"
        Me.Step2LayoutPanel.RowCount = 2
        Me.Step2LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.Step2LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.Step2LayoutPanel.Size = New System.Drawing.Size(593, 82)
        Me.Step2LayoutPanel.TabIndex = 1
        '
        'Step2ProgressLayout
        '
        Me.Step2ProgressLayout.ColumnCount = 1
        Me.Step2ProgressLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.Step2ProgressLayout.Controls.Add(Me.Step2StatusLabel, 0, 0)
        Me.Step2ProgressLayout.Controls.Add(Me.Step2ProgressBar, 0, 1)
        Me.Step2ProgressLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Step2ProgressLayout.Location = New System.Drawing.Point(5, 30)
        Me.Step2ProgressLayout.Name = "Step2ProgressLayout"
        Me.Step2ProgressLayout.RowCount = 2
        Me.Step2ProgressLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.Step2ProgressLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.Step2ProgressLayout.Size = New System.Drawing.Size(583, 47)
        Me.Step2ProgressLayout.TabIndex = 0
        '
        'Step2StatusLabel
        '
        Me.Step2StatusLabel.AutoEllipsis = True
        Me.Step2StatusLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Step2StatusLabel.Location = New System.Drawing.Point(3, 0)
        Me.Step2StatusLabel.Name = "Step2StatusLabel"
        Me.Step2StatusLabel.Size = New System.Drawing.Size(577, 14)
        Me.Step2StatusLabel.TabIndex = 2
        Me.Step2StatusLabel.Text = "\WAITING"
        Me.Step2StatusLabel.UseMnemonic = False
        '
        'Step2ProgressBar
        '
        Me.Step2ProgressBar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Step2ProgressBar.Location = New System.Drawing.Point(3, 17)
        Me.Step2ProgressBar.Name = "Step2ProgressBar"
        Me.Step2ProgressBar.Size = New System.Drawing.Size(577, 27)
        Me.Step2ProgressBar.TabIndex = 3
        '
        'Step2Label
        '
        Me.Step2Label.AutoSize = True
        Me.Step2Label.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Step2Label.Location = New System.Drawing.Point(5, 2)
        Me.Step2Label.Name = "Step2Label"
        Me.Step2Label.Padding = New System.Windows.Forms.Padding(0, 5, 0, 5)
        Me.Step2Label.Size = New System.Drawing.Size(583, 23)
        Me.Step2Label.TabIndex = 1
        Me.Step2Label.Text = "\STEP_2"
        '
        'Step1LayoutPanel
        '
        Me.Step1LayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset
        Me.Step1LayoutPanel.ColumnCount = 1
        Me.Step1LayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.Step1LayoutPanel.Controls.Add(Me.Step1ProgressLayout, 0, 1)
        Me.Step1LayoutPanel.Controls.Add(Me.Step1Label, 0, 0)
        Me.Step1LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Step1LayoutPanel.Location = New System.Drawing.Point(4, 4)
        Me.Step1LayoutPanel.Name = "Step1LayoutPanel"
        Me.Step1LayoutPanel.RowCount = 2
        Me.Step1LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.Step1LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.Step1LayoutPanel.Size = New System.Drawing.Size(593, 82)
        Me.Step1LayoutPanel.TabIndex = 0
        '
        'Step1ProgressLayout
        '
        Me.Step1ProgressLayout.ColumnCount = 1
        Me.Step1ProgressLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.Step1ProgressLayout.Controls.Add(Me.Step1StatusLabel, 0, 0)
        Me.Step1ProgressLayout.Controls.Add(Me.Step1ProgressBar, 0, 1)
        Me.Step1ProgressLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Step1ProgressLayout.Location = New System.Drawing.Point(5, 30)
        Me.Step1ProgressLayout.Name = "Step1ProgressLayout"
        Me.Step1ProgressLayout.RowCount = 2
        Me.Step1ProgressLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.Step1ProgressLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.Step1ProgressLayout.Size = New System.Drawing.Size(583, 47)
        Me.Step1ProgressLayout.TabIndex = 0
        '
        'Step1StatusLabel
        '
        Me.Step1StatusLabel.AutoEllipsis = True
        Me.Step1StatusLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Step1StatusLabel.Location = New System.Drawing.Point(3, 0)
        Me.Step1StatusLabel.Name = "Step1StatusLabel"
        Me.Step1StatusLabel.Size = New System.Drawing.Size(577, 14)
        Me.Step1StatusLabel.TabIndex = 2
        Me.Step1StatusLabel.Text = "\WAITING"
        Me.Step1StatusLabel.UseMnemonic = False
        '
        'Step1ProgressBar
        '
        Me.Step1ProgressBar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Step1ProgressBar.Location = New System.Drawing.Point(3, 17)
        Me.Step1ProgressBar.MarqueeAnimationSpeed = 50
        Me.Step1ProgressBar.Name = "Step1ProgressBar"
        Me.Step1ProgressBar.Size = New System.Drawing.Size(577, 27)
        Me.Step1ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.Step1ProgressBar.TabIndex = 3
        Me.Step1ProgressBar.Value = 100
        '
        'Step1Label
        '
        Me.Step1Label.AutoSize = True
        Me.Step1Label.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Step1Label.Location = New System.Drawing.Point(5, 2)
        Me.Step1Label.Name = "Step1Label"
        Me.Step1Label.Padding = New System.Windows.Forms.Padding(0, 5, 0, 5)
        Me.Step1Label.Size = New System.Drawing.Size(583, 23)
        Me.Step1Label.TabIndex = 1
        Me.Step1Label.Text = "\STEP_1"
        '
        'ButtonsLayoutPanel
        '
        Me.ButtonsLayoutPanel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonsLayoutPanel.ColumnCount = 1
        Me.ButtonsLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.ButtonsLayoutPanel.Controls.Add(Me.SyncBtn, 0, 0)
        Me.ButtonsLayoutPanel.Controls.Add(Me.StopBtn, 0, 1)
        Me.ButtonsLayoutPanel.Location = New System.Drawing.Point(489, 286)
        Me.ButtonsLayoutPanel.Name = "ButtonsLayoutPanel"
        Me.ButtonsLayoutPanel.RowCount = 2
        Me.ButtonsLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.ButtonsLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.ButtonsLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.ButtonsLayoutPanel.Size = New System.Drawing.Size(124, 69)
        Me.ButtonsLayoutPanel.TabIndex = 1
        '
        'SyncBtn
        '
        Me.SyncBtn.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SyncBtn.Location = New System.Drawing.Point(3, 3)
        Me.SyncBtn.Name = "SyncBtn"
        Me.SyncBtn.Size = New System.Drawing.Size(118, 28)
        Me.SyncBtn.TabIndex = 4
        Me.SyncBtn.Text = "\SYNC"
        Me.SyncBtn.UseVisualStyleBackColor = True
        '
        'StopBtn
        '
        Me.StopBtn.Dock = System.Windows.Forms.DockStyle.Fill
        Me.StopBtn.Location = New System.Drawing.Point(3, 37)
        Me.StopBtn.Name = "StopBtn"
        Me.StopBtn.Size = New System.Drawing.Size(118, 29)
        Me.StopBtn.TabIndex = 1
        Me.StopBtn.Tag = "\CANCEL_CLOSE"
        Me.StopBtn.Text = "\CANCEL"
        Me.StopBtn.UseVisualStyleBackColor = True
        '
        'StatisticsPanel
        '
        Me.StatisticsPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StatisticsPanel.ColumnCount = 4
        Me.StatisticsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.StatisticsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.StatisticsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.StatisticsPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.StatisticsPanel.Controls.Add(Me.FilesCreatedLabel, 0, 0)
        Me.StatisticsPanel.Controls.Add(Me.FilesCreated, 1, 0)
        Me.StatisticsPanel.Controls.Add(Me.FilesDeletedLabel, 0, 1)
        Me.StatisticsPanel.Controls.Add(Me.FilesDeleted, 1, 1)
        Me.StatisticsPanel.Controls.Add(Me.FoldersCreatedLabel, 0, 2)
        Me.StatisticsPanel.Controls.Add(Me.FoldersCreated, 1, 2)
        Me.StatisticsPanel.Controls.Add(Me.FoldersDeletedLabel, 0, 3)
        Me.StatisticsPanel.Controls.Add(Me.FoldersDeleted, 1, 3)
        Me.StatisticsPanel.Controls.Add(Me.ElapsedTimeLabel, 2, 0)
        Me.StatisticsPanel.Controls.Add(Me.ElapsedTime, 3, 0)
        Me.StatisticsPanel.Controls.Add(Me.SpeedLabel, 2, 1)
        Me.StatisticsPanel.Controls.Add(Me.Speed, 3, 1)
        Me.StatisticsPanel.Controls.Add(Me.DoneLabel, 2, 3)
        Me.StatisticsPanel.Controls.Add(Me.Done, 3, 3)
        Me.StatisticsPanel.Location = New System.Drawing.Point(12, 286)
        Me.StatisticsPanel.Name = "StatisticsPanel"
        Me.StatisticsPanel.RowCount = 4
        Me.StatisticsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.StatisticsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.StatisticsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.StatisticsPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.StatisticsPanel.Size = New System.Drawing.Size(471, 69)
        Me.StatisticsPanel.TabIndex = 2
        '
        'FilesCreatedLabel
        '
        Me.FilesCreatedLabel.AutoSize = True
        Me.FilesCreatedLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FilesCreatedLabel.Location = New System.Drawing.Point(3, 0)
        Me.FilesCreatedLabel.Name = "FilesCreatedLabel"
        Me.FilesCreatedLabel.Size = New System.Drawing.Size(127, 17)
        Me.FilesCreatedLabel.TabIndex = 10
        Me.FilesCreatedLabel.Text = "\FILES_CREATED"
        Me.FilesCreatedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FilesCreated
        '
        Me.FilesCreated.AutoEllipsis = True
        Me.FilesCreated.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FilesCreated.Location = New System.Drawing.Point(136, 0)
        Me.FilesCreated.Name = "FilesCreated"
        Me.FilesCreated.Size = New System.Drawing.Size(128, 17)
        Me.FilesCreated.TabIndex = 9
        Me.FilesCreated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FilesDeletedLabel
        '
        Me.FilesDeletedLabel.AutoSize = True
        Me.FilesDeletedLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FilesDeletedLabel.Location = New System.Drawing.Point(3, 17)
        Me.FilesDeletedLabel.Name = "FilesDeletedLabel"
        Me.FilesDeletedLabel.Size = New System.Drawing.Size(127, 17)
        Me.FilesDeletedLabel.TabIndex = 12
        Me.FilesDeletedLabel.Text = "\FILES_DELETED"
        Me.FilesDeletedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FilesDeleted
        '
        Me.FilesDeleted.AutoEllipsis = True
        Me.FilesDeleted.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FilesDeleted.Location = New System.Drawing.Point(136, 17)
        Me.FilesDeleted.Name = "FilesDeleted"
        Me.FilesDeleted.Size = New System.Drawing.Size(128, 17)
        Me.FilesDeleted.TabIndex = 14
        Me.FilesDeleted.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FoldersCreatedLabel
        '
        Me.FoldersCreatedLabel.AutoSize = True
        Me.FoldersCreatedLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FoldersCreatedLabel.Location = New System.Drawing.Point(3, 34)
        Me.FoldersCreatedLabel.Name = "FoldersCreatedLabel"
        Me.FoldersCreatedLabel.Size = New System.Drawing.Size(127, 17)
        Me.FoldersCreatedLabel.TabIndex = 8
        Me.FoldersCreatedLabel.Text = "\FOLDERS_CREATED"
        Me.FoldersCreatedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FoldersCreated
        '
        Me.FoldersCreated.AutoEllipsis = True
        Me.FoldersCreated.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FoldersCreated.Location = New System.Drawing.Point(136, 34)
        Me.FoldersCreated.Name = "FoldersCreated"
        Me.FoldersCreated.Size = New System.Drawing.Size(128, 17)
        Me.FoldersCreated.TabIndex = 11
        Me.FoldersCreated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FoldersDeletedLabel
        '
        Me.FoldersDeletedLabel.AutoSize = True
        Me.FoldersDeletedLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FoldersDeletedLabel.Location = New System.Drawing.Point(3, 51)
        Me.FoldersDeletedLabel.Name = "FoldersDeletedLabel"
        Me.FoldersDeletedLabel.Size = New System.Drawing.Size(127, 18)
        Me.FoldersDeletedLabel.TabIndex = 13
        Me.FoldersDeletedLabel.Text = "\FOLDERS_DELETED"
        Me.FoldersDeletedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FoldersDeleted
        '
        Me.FoldersDeleted.AutoEllipsis = True
        Me.FoldersDeleted.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FoldersDeleted.Location = New System.Drawing.Point(136, 51)
        Me.FoldersDeleted.Name = "FoldersDeleted"
        Me.FoldersDeleted.Size = New System.Drawing.Size(128, 18)
        Me.FoldersDeleted.TabIndex = 15
        Me.FoldersDeleted.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ElapsedTimeLabel
        '
        Me.ElapsedTimeLabel.AutoSize = True
        Me.ElapsedTimeLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ElapsedTimeLabel.Location = New System.Drawing.Point(270, 0)
        Me.ElapsedTimeLabel.Name = "ElapsedTimeLabel"
        Me.ElapsedTimeLabel.Size = New System.Drawing.Size(64, 17)
        Me.ElapsedTimeLabel.TabIndex = 0
        Me.ElapsedTimeLabel.Text = "\ELAPSED"
        Me.ElapsedTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ElapsedTime
        '
        Me.ElapsedTime.AutoEllipsis = True
        Me.ElapsedTime.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ElapsedTime.Location = New System.Drawing.Point(340, 0)
        Me.ElapsedTime.Name = "ElapsedTime"
        Me.ElapsedTime.Size = New System.Drawing.Size(128, 17)
        Me.ElapsedTime.TabIndex = 1
        Me.ElapsedTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'SpeedLabel
        '
        Me.SpeedLabel.AutoSize = True
        Me.SpeedLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SpeedLabel.Location = New System.Drawing.Point(270, 17)
        Me.SpeedLabel.Name = "SpeedLabel"
        Me.SpeedLabel.Size = New System.Drawing.Size(64, 17)
        Me.SpeedLabel.TabIndex = 2
        Me.SpeedLabel.Text = "\SPEED"
        Me.SpeedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Speed
        '
        Me.Speed.AutoEllipsis = True
        Me.Speed.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Speed.Location = New System.Drawing.Point(340, 17)
        Me.Speed.Name = "Speed"
        Me.Speed.Size = New System.Drawing.Size(128, 17)
        Me.Speed.TabIndex = 3
        Me.Speed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DoneLabel
        '
        Me.DoneLabel.AutoSize = True
        Me.DoneLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DoneLabel.Location = New System.Drawing.Point(270, 51)
        Me.DoneLabel.Name = "DoneLabel"
        Me.DoneLabel.Size = New System.Drawing.Size(64, 18)
        Me.DoneLabel.TabIndex = 4
        Me.DoneLabel.Text = "\DONE"
        Me.DoneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Done
        '
        Me.Done.AutoEllipsis = True
        Me.Done.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Done.Location = New System.Drawing.Point(340, 51)
        Me.Done.Name = "Done"
        Me.Done.Size = New System.Drawing.Size(128, 18)
        Me.Done.TabIndex = 5
        Me.Done.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PreviewList
        '
        Me.PreviewList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PreviewList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.TypeColumn, Me.ActionColumn, Me.DirectionColumn, Me.PathColumn})
        Me.PreviewList.FullRowSelect = True
        Me.PreviewList.Location = New System.Drawing.Point(12, 12)
        Me.PreviewList.MultiSelect = False
        Me.PreviewList.Name = "PreviewList"
        Me.PreviewList.Size = New System.Drawing.Size(601, 268)
        Me.PreviewList.SmallImageList = Me.SyncingIcons
        Me.PreviewList.TabIndex = 4
        Me.PreviewList.UseCompatibleStateImageBehavior = False
        Me.PreviewList.View = System.Windows.Forms.View.Details
        Me.PreviewList.Visible = False
        '
        'TypeColumn
        '
        Me.TypeColumn.Text = "\TYPE"
        Me.TypeColumn.Width = 80
        '
        'ActionColumn
        '
        Me.ActionColumn.Text = "\ACTION"
        Me.ActionColumn.Width = 80
        '
        'DirectionColumn
        '
        Me.DirectionColumn.Text = "\DIRECTION"
        Me.DirectionColumn.Width = 80
        '
        'PathColumn
        '
        Me.PathColumn.Text = "\PATH"
        Me.PathColumn.Width = 230
        '
        'SyncingIcons
        '
        Me.SyncingIcons.ImageStream = CType(resources.GetObject("SyncingIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.SyncingIcons.TransparentColor = System.Drawing.Color.Transparent
        Me.SyncingIcons.Images.SetKeyName(0, "go-next-upd.png")
        Me.SyncingIcons.Images.SetKeyName(1, "go-next.png")
        Me.SyncingIcons.Images.SetKeyName(2, "go-previous-upd.png")
        Me.SyncingIcons.Images.SetKeyName(3, "go-previous.png")
        Me.SyncingIcons.Images.SetKeyName(4, "list-remove.png")
        Me.SyncingIcons.Images.SetKeyName(5, "folder-new.png")
        Me.SyncingIcons.Images.SetKeyName(6, "folder.png")
        Me.SyncingIcons.Images.SetKeyName(7, "delete-folder.png")
        Me.SyncingIcons.Images.SetKeyName(8, "process-stop.png")
        '
        'SyncingTimer
        '
        Me.SyncingTimer.Enabled = True
        Me.SyncingTimer.Interval = 50
        '
        'SynchronizeForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(625, 367)
        Me.Controls.Add(Me.PreviewList)
        Me.Controls.Add(Me.StatisticsPanel)
        Me.Controls.Add(Me.ButtonsLayoutPanel)
        Me.Controls.Add(Me.MainLayoutPanel)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.KeyPreview = True
        Me.Name = "SynchronizeForm"
        Me.Text = "\SYNCHRONIZING"
        Me.MainLayoutPanel.ResumeLayout(False)
        Me.Step3LayoutPanel.ResumeLayout(False)
        Me.Step3LayoutPanel.PerformLayout()
        Me.Step3_ProgressLayout.ResumeLayout(False)
        Me.Step2LayoutPanel.ResumeLayout(False)
        Me.Step2LayoutPanel.PerformLayout()
        Me.Step2ProgressLayout.ResumeLayout(False)
        Me.Step1LayoutPanel.ResumeLayout(False)
        Me.Step1LayoutPanel.PerformLayout()
        Me.Step1ProgressLayout.ResumeLayout(False)
        Me.ButtonsLayoutPanel.ResumeLayout(False)
        Me.StatisticsPanel.ResumeLayout(False)
        Me.StatisticsPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MainLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Step1LayoutPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Step1ProgressLayout As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Step3LayoutPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Step3_ProgressLayout As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Step3StatusLabel As System.Windows.Forms.Label
    Friend WithEvents Step3ProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents Step3Label As System.Windows.Forms.Label
    Friend WithEvents Step2LayoutPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Step2ProgressLayout As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Step2StatusLabel As System.Windows.Forms.Label
    Friend WithEvents Step2ProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents Step2Label As System.Windows.Forms.Label
    Friend WithEvents Step1StatusLabel As System.Windows.Forms.Label
    Friend WithEvents Step1ProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents Step1Label As System.Windows.Forms.Label
    Friend WithEvents ButtonsLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents StopBtn As System.Windows.Forms.Button
    Friend WithEvents StatisticsPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ElapsedTimeLabel As System.Windows.Forms.Label
    Friend WithEvents ElapsedTime As System.Windows.Forms.Label
    Friend WithEvents SyncBtn As System.Windows.Forms.Button
    Friend WithEvents PreviewList As System.Windows.Forms.ListView
    Friend WithEvents PathColumn As System.Windows.Forms.ColumnHeader
    Friend WithEvents ActionColumn As System.Windows.Forms.ColumnHeader
    Friend WithEvents DirectionColumn As System.Windows.Forms.ColumnHeader
    Friend WithEvents TypeColumn As System.Windows.Forms.ColumnHeader
    Friend WithEvents SpeedLabel As System.Windows.Forms.Label
    Friend WithEvents Done As System.Windows.Forms.Label
    Friend WithEvents DoneLabel As System.Windows.Forms.Label
    Friend WithEvents Speed As System.Windows.Forms.Label
    Friend WithEvents FoldersCreated As System.Windows.Forms.Label
    Friend WithEvents FilesCreatedLabel As System.Windows.Forms.Label
    Friend WithEvents FilesCreated As System.Windows.Forms.Label
    Friend WithEvents FoldersCreatedLabel As System.Windows.Forms.Label
    Friend WithEvents SyncingIcons As System.Windows.Forms.ImageList
    Friend WithEvents SyncingTimer As System.Windows.Forms.Timer
    Friend WithEvents FoldersDeleted As System.Windows.Forms.Label
    Friend WithEvents FoldersDeletedLabel As System.Windows.Forms.Label
    Friend WithEvents FilesDeletedLabel As System.Windows.Forms.Label
    Friend WithEvents FilesDeleted As System.Windows.Forms.Label
End Class
