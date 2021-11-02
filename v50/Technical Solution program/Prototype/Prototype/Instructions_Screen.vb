Imports System.Windows.Forms
Public Class Instructions_Screen
    Private Sub LoadInstructions() Handles MyBase.Load 'creates/loads instruction screen which outputs instructions

        Dim screenWidth As Integer = 750
        Dim screenHeight As Integer = 400

        Dim instructionsGroupBox As New GroupBox
        Dim groupBoxWidth As Integer = 500
        Dim groupBoxHeight As Integer = 300

        Dim instructions As New Label
        Dim labelWidth As Integer = 400
        Dim labelHeight As Integer = 250

        Dim backButton As New Button
        Dim backButtonWidth As Integer = 120
        Dim backButtonHeight As Integer = 75

        Me.Size = New Drawing.Size(screenWidth, screenHeight) 'defines size for window

        instructionsGroupBox.Name = "instructionsGroupBox"
        instructionsGroupBox.Location = New Drawing.Point(20, 20)
        instructionsGroupBox.Size = New Drawing.Size(groupBoxWidth, groupBoxHeight)


        instructions.Name = "instructions"

        instructions.Text = "
Instructions:

- Click the ‘Generate Maze’ button in order to generate a maze, you will be required to enter information about the maze you want to randomly generate into the input boxes provided on the screen. Once you have entered the required information press ‘Accept’, and then using the information a maze will be randomly generated and displayed for you. If you wish to save the generated maze just press ‘Save’, you will then be outputted the associated ID for the maze which you must remember.

- Click the ‘Import Maze’ button to import a maze (you must have first generated a maze to import one). To import a maze just input the maze’s associated ID. After that you can choose from a selection of algorithms to run on the maze. In addition to that, you can press ‘Compare Data’ to compare data about the maze such as the time taken for an specific algorithm to be run on it compared to another algorithm (or the same).

- Click the ‘Exit’ button to close the program.”

        instructions.Location = New Drawing.Point(25, 25)
        instructions.Size = New Drawing.Size(labelWidth, labelHeight)
        instructions.TextAlign = Drawing.ContentAlignment.TopLeft
        instructionsGroupBox.Controls.Add(instructions)

        Me.Controls.Add(instructionsGroupBox)


        backButton.Name = "backButton"
        backButton.Text = "Back"
        backButton.Location = New Drawing.Point(570, 25)
        backButton.Size = New Drawing.Size(backButtonWidth, backButtonHeight)
        backButton.FlatStyle = FlatStyle.Popup
        AddHandler backButton.Click, AddressOf Me.BackButtonClick
        Me.Controls.Add(backButton)

    End Sub

    Private Sub BackButtonClick() 'closes the instructions screen
        Me.Close()
    End Sub

End Class