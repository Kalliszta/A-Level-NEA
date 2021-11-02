Imports System.Windows.Forms
Public Class Main_Menu_Screen

    Private Sub CreateMainMenuScreen(sender As Object, e As EventArgs) Handles MyBase.Load 'creates/loads windows/screen when the program is run

        'variables
        Dim width As Integer = 750
        Dim height As Integer = 600

        Dim optionGroupBox As New GroupBox

        Dim instructionButton As New Button
        Dim generateMazeButton As New Button
        Dim importMazeButton As New Button
        Dim exitButton As New Button


        Me.Size = New Drawing.Size(width, height) 'defines size for window

        '===== Assigning information about objects to be put onto the windows form ====

        'determines information about groupbox (that acts as a container) to contain anything added to it 
        optionGroupBox.Name = "optionGroupBox"
        optionGroupBox.Location = New Drawing.Point((width / 2) - 150, (height / 2) - 200) 'determines the location of the groupbox
        optionGroupBox.Size = New Drawing.Size(300, 350) 'determines the size of the groupbox

        'determines information about button 
        instructionButton.Name = "instructionButton"
        instructionButton.Text = "Instructions"
        instructionButton.Location = New Drawing.Point((((width / 2) - 150) / 2) - 60, (((height / 2) - 150) / 2) - 40)
        instructionButton.Size = New Drawing.Size(200, 50)
        instructionButton.FlatStyle = FlatStyle.Popup
        AddHandler instructionButton.Click, AddressOf Me.InstructionsButtonClick
        optionGroupBox.Controls.Add(instructionButton) 'adds the button to the "optionGroupBox" groupbox

        'determines information about button
        generateMazeButton.Name = "generateMazeButton"
        generateMazeButton.Text = "Generate Maze"
        generateMazeButton.Location = New Drawing.Point((((width / 2) - 150) / 2) - 60, (((height / 2) - 150) / 2) + 35)
        generateMazeButton.Size = New Drawing.Size(200, 50)
        generateMazeButton.FlatStyle = FlatStyle.Popup
        AddHandler generateMazeButton.Click, AddressOf Me.GenerateMazeButtonClick
        optionGroupBox.Controls.Add(generateMazeButton) 'adds the button to the "optionGroupBox" groupbox

        'determines information about button
        importMazeButton.Name = "importMazeButton"
        importMazeButton.Text = "Import Maze"
        importMazeButton.Location = New Drawing.Point((((width / 2) - 150) / 2) - 60, (((height / 2) - 150) / 2) + 110)
        importMazeButton.Size = New Drawing.Size(200, 50)
        importMazeButton.FlatStyle = FlatStyle.Popup
        AddHandler importMazeButton.Click, AddressOf Me.ImportMazeButtonClick
        optionGroupBox.Controls.Add(importMazeButton) 'adds the button to the "optionGroupBox" groupbox

        'determines information about button
        exitButton.Name = "exitButton"
        exitButton.Text = "Exit"
        exitButton.Location = New Drawing.Point((((width / 2) - 150) / 2) - 60, (((height / 2) - 150) / 2) + 185)
        exitButton.Size = New Drawing.Size(200, 50)
        exitButton.FlatStyle = FlatStyle.Popup
        AddHandler exitButton.Click, AddressOf Me.ExitButtonClick
        optionGroupBox.Controls.Add(exitButton) 'adds the button to the "optionGroupBox" groupbox

        Me.Controls.Add(optionGroupBox) 'the groupbox "optionGroupBox" along with its contents is added to the windows form

    End Sub

    Private Sub InstructionsButtonClick() 'runs when button with the text "Instructions" is clicked
        Dim instructionsScreen As New Instructions_Screen
        instructionsScreen.ShowDialog()
    End Sub

    Private Sub GenerateMazeButtonClick() 'runs when button with the text "Generate Maze" is clicked
        Dim mazeGenerationScreen As New Maze_Generation_Screen
        mazeGenerationScreen.ShowDialog() 'open the generate maze screen
    End Sub

    Private Sub ImportMazeButtonClick() 'runs when button with the text "Import Maze" is clicked
        Dim importMazeScreen As New Import_Maze_Screen
        importMazeScreen.ShowDialog() 'opens the import maze screen
    End Sub

    Private Sub ExitButtonClick() 'closes the entire program
        Me.Close()
    End Sub

End Class
