﻿'This file is part of Create Synchronicity.
'
'Create Synchronicity is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
'Create Synchronicity is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
'You should have received a copy of the GNU General Public License along with Create Synchronicity.  If not, see <http://www.gnu.org/licenses/>.
'Created by:	Clément Pit--Claudel.
'Web site:		http://synchronicity.sourceforge.net.

Friend Class AboutForm
    Private Shared Sub SetLinkArea(ByVal Link As LinkLabel)
        If Link.Text.IndexOf("\"c) = -1 Or Link.Text.IndexOf("/"c) = -1 Then Exit Sub

        Dim Area As New LinkArea
        Area.Start = Link.Text.IndexOf("\"c)
        Link.Text = Link.Text.Remove(Area.Start, 1)
        Area.Length = Link.Text.IndexOf("/"c) - Area.Start
        Link.Text = Link.Text.Remove(Area.Start + Area.Length, 1)
        Link.LinkArea = Area
    End Sub

    Private Sub About_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Translation.TranslateControl(Me)
        VersionInfo.Text = String.Format(VersionInfo.Text, String.Format("{0} (r{1})", Application.ProductVersion.TrimEnd(".0".ToCharArray), Revision.Build))

        SetLinkArea(BugReport)
        SetLinkArea(ContactLink)
        SetLinkArea(LinkToLicense)
        SetLinkArea(LinkToProductPage)
        SetLinkArea(LinkToWebsite)
        SetLinkArea(VersionInfo)

        ProgramConfig.LoadProgramSettings()
        LanguageHandler.FillLanguagesComboBox(LanguagesList)
        UpdatesOption.Checked = ProgramConfig.GetProgramSetting(Of Boolean)(ProgramSetting.AutoUpdates, False)

#If CONFIG = "Linux" Then
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Sizable
#End If
    End Sub

    Private Sub LinkToProductPage_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkToProductPage.LinkClicked
        Interaction.StartProcess(Branding.Web)
    End Sub

    Private Sub LinkToWebsite_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkToWebsite.LinkClicked
        Interaction.StartProcess(Branding.CompanyWeb)
    End Sub

    Private Sub VersionInfo_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles VersionInfo.LinkClicked
        Updates.CheckForUpdates(False)
    End Sub

    Private Sub ContactLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ContactLink.LinkClicked
        Interaction.StartProcess(Branding.Support)
    End Sub

    Private Sub LinkToLicense_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkToLicense.LinkClicked
        Interaction.StartProcess(Branding.License)
    End Sub

    Private Sub BugReport_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles BugReport.LinkClicked
        Interaction.StartProcess(Branding.BugReport)
    End Sub

    Private Sub AboutForm_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        If LanguagesList.SelectedIndex <> -1 Then
            Dim SelectedLanguage As String = LanguagesList.SelectedItem.ToString.Split("-"c)(0).Trim
            Dim LanguageChanged As Boolean = ProgramConfig.GetProgramSetting(Of String)(ProgramSetting.Language, ProgramSetting.DefaultLanguage) <> SelectedLanguage

            ProgramConfig.SetProgramSetting(Of String)(ProgramSetting.Language, SelectedLanguage)

            If LanguageChanged Then
                ReloadNeeded = True
                Translation = LanguageHandler.GetSingleton(True)
            End If
        End If

        ProgramConfig.SetProgramSetting(Of Boolean)(ProgramSetting.AutoUpdates, UpdatesOption.Checked)
        ProgramConfig.SaveProgramSettings()
    End Sub
End Class
