Imports Microsoft.Win32
Imports System.IO
Imports System.IO.Compression
Imports System.Reflection

Public Class Form1
    Dim CodeBlocksPath As String
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim key As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\CodeBlocks")

        If key IsNot Nothing Then
            CodeBlocksPath = key.GetValue("Path")
        Else
            MsgBox("CodeBlocks is not installed on your computer. Install CodeBlocks first and then try again",
                vbCritical, "Error")
            End
        End If


    End Sub

    Sub ExtractToPath(Path As String)
        If Directory.GetFiles(Path).Count <> 0 Then
            Throw New Exception("Please choose an empty folder")
        End If

        Dim ExeAsm As Assembly = Assembly.GetExecutingAssembly()
        Using stream As Stream = ExeAsm.GetManifestResourceStream("MangoSDL.SDL Starter.zip")
            Dim Template As New ZipArchive(stream)
            Template.ExtractToDirectory(Path)
        End Using
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            ExtractToPath(txtProjectFolder.Text)
            If MsgBox("Project Successfully Created. Open it in CodeBlocks?", vbQuestion + vbYesNo, "Question") = vbYes Then
                Dim command As String = CodeBlocksPath & "\codeblocks.exe """ & txtProjectFolder.Text & "\SDL Starter.cbp"" --file=""" & txtProjectFolder.Text & "\main.c"":9"
                'Clipboard.Clear()
                'Clipboard.SetText(command)
                'MsgBox(command)
                Shell(command, , False)
                End
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Using ofolder As New FolderBrowserDialog()
Again:
            If ofolder.ShowDialog() = DialogResult.OK Then
                If Directory.GetFiles(ofolder.SelectedPath).Count <> 0 Then
                    MsgBox("Please select an empty folder.", vbOK + vbInformation, "Info")
                    GoTo Again
                End If
                txtProjectFolder.Text = ofolder.SelectedPath
            End If
        End Using
    End Sub

    Private Sub txtProjectFolder_TextChanged(sender As Object, e As EventArgs) Handles txtProjectFolder.TextChanged
        If txtProjectFolder.Text = "" Then
            Button2.Enabled = False
        Else
            Button2.Enabled = True
        End If
    End Sub
End Class
