Imports System.Drawing
Imports System.Windows.Forms

Public Class Maze_Generation_Screen

    'textboxes
    Private nameInputBox As New TextBox 'an object that can be written in on the windows form

    Private widthInputBox As New TextBox
    Private heightInputBox As New TextBox

    Private startSquareXInputBox As New TextBox
    Private startSquareYInputBox As New TextBox

    Private endSquareXInputBox As New TextBox
    Private endSquareYInputBox As New TextBox


    'Error Messages
    Private errorMName As New Label
    Private errorMWidth As New Label
    Private errorMHeight As New Label
    Private errorMStartSquX As New Label
    Private errorMStartSquY As New Label
    Private errorMEndSquX As New Label
    Private errorMEndSquY As New Label

    Private errorMessageArray() As Label = {errorMName, errorMWidth, errorMHeight, errorMStartSquX, errorMStartSquY, errorMEndSquX, errorMEndSquY}

    'Maze
    Private generatedMaze As Maze

    Private Sub LoadScreen(sender As Object, e As EventArgs) Handles MyBase.Load 'when the window loads this subroutine runs

        CreateMazeGenerationScreen() 'creates/loads the maze generation screen, the reason the code for it is in a separate procedure and not within the LoadScreen subroutine is because it can be called upon by a different procedure as well

    End Sub

    Public Sub CreateMazeGenerationScreen() 'creates/loads the maze generation screen where the user generate a maze

        Dim nameGroupBox As New GroupBox '0 (position in groupBoxList)
        Dim mazeSizeGroupBox As New GroupBox '1 (position in groupBoxList), keeps information about size on interface grouped together
        Dim coordinatesGroupBox As New GroupBox '2 (position in groupBoxList)
        Dim acceptInputsButton As New Button
        Dim backButton As New Button

        'Label (normal text)
        Dim nameLabel As New Label
        Dim widthLabel As New Label
        Dim heightLabel As New Label
        Dim startSquareLabel As New Label
        Dim endSquareLabel As New Label

        Me.Size = New Drawing.Size(750, 600) 'defines size for window

        '===== objects in the name input section =====

        nameGroupBox.Name = "nameGroupBox"
        nameGroupBox.Location = New Drawing.Point(10, 10)
        nameGroupBox.Size = New Drawing.Size(420, 100)

        nameLabel.Name = "nameLabel" 'identifier for the object (for windows forms)
        nameLabel.Text = "Name for maze:" 'text displayed by label
        nameLabel.Location = New Drawing.Point(25, 45) 'location of label
        nameGroupBox.Controls.Add(nameLabel) 'adds the label into the groupbox

        nameInputBox.Name = "nameInputBox" 'class variable
        nameInputBox.Location = New Drawing.Point(125, 42)
        nameGroupBox.Controls.Add(nameInputBox) 'adds the label into the groupbox

        errorMName.Name = "errorMName" 'class variable
        errorMName.Text = ""
        errorMName.Location = New Drawing.Point(25, 70)
        errorMName.Size = New Drawing.Point(390, 15) 'size of label
        errorMName.ForeColor = Drawing.Color.Red 'makes the colour the colour of the text red
        nameGroupBox.Controls.Add(errorMName) 'adds the label into the groupbox

        Me.Controls.Add(nameGroupBox) 'adds the nameGroupBox and everything it contains onto the screen


        '===== objects in the size input section =====

        mazeSizeGroupBox.Name = "mazeSizeGroupBox"
        mazeSizeGroupBox.Location = New Drawing.Point(10, 100)
        mazeSizeGroupBox.Size = New Drawing.Size(420, 200)


        widthLabel.Name = "widthLabel"
        widthLabel.Text = "Width:"
        widthLabel.Location = New Drawing.Point(25, 70)
        mazeSizeGroupBox.Controls.Add(widthLabel)

        widthInputBox.Name = "widthInputBox"
        widthInputBox.Location = New Drawing.Point(125, 67)
        mazeSizeGroupBox.Controls.Add(widthInputBox)

        errorMWidth.Name = "errorMWidth"
        errorMWidth.Text = ""
        errorMWidth.Location = New Drawing.Point(25, 95)
        errorMWidth.Size = New Drawing.Point(390, 15)
        errorMWidth.ForeColor = Drawing.Color.Red
        mazeSizeGroupBox.Controls.Add(errorMWidth)


        heightLabel.Name = "heightLabel"
        heightLabel.Text = "Height:"
        heightLabel.Location = New Drawing.Point(25, 120)
        mazeSizeGroupBox.Controls.Add(heightLabel)

        heightInputBox.Name = "heightInputBox"
        heightInputBox.Location = New Drawing.Point(125, 117)
        mazeSizeGroupBox.Controls.Add(heightInputBox)
        Me.Controls.Add(mazeSizeGroupBox)

        errorMHeight.Name = "errorMHeight"
        errorMHeight.Text = ""
        errorMHeight.Location = New Drawing.Point(25, 145)
        errorMHeight.Size = New Drawing.Point(390, 15)
        errorMHeight.ForeColor = Drawing.Color.Red
        mazeSizeGroupBox.Controls.Add(errorMHeight)

        '===== objects in the coordinate input section =====

        coordinatesGroupBox.Name = "coordinatesGroupBox"
        coordinatesGroupBox.Location = New Drawing.Point(10, 290)
        coordinatesGroupBox.Size = New Drawing.Size(420, 250)


        startSquareLabel.Name = "startSquareLabel"
        startSquareLabel.Text = "Start Square Coordinates (x|y):"
        startSquareLabel.Location = New Drawing.Point(25, 60)
        startSquareLabel.Size = New Drawing.Point(100, 30)
        coordinatesGroupBox.Controls.Add(startSquareLabel)

        startSquareXInputBox.Name = "startSquareXInputBox"
        startSquareXInputBox.Location = New Drawing.Point(125, 65)
        startSquareXInputBox.Size = New Drawing.Point(30, 20)
        coordinatesGroupBox.Controls.Add(startSquareXInputBox)

        errorMStartSquX.Name = "errorMStartSquX"
        errorMStartSquX.Text = ""
        errorMStartSquX.Location = New Drawing.Point(25, 90)
        errorMStartSquX.Size = New Drawing.Point(390, 15)
        errorMStartSquX.ForeColor = Drawing.Color.Red
        coordinatesGroupBox.Controls.Add(errorMStartSquX)

        startSquareYInputBox.Name = "startSquareYInputBox"
        startSquareYInputBox.Location = New Drawing.Point(175, 65)
        startSquareYInputBox.Size = New Drawing.Point(30, 20)
        coordinatesGroupBox.Controls.Add(startSquareYInputBox)

        errorMStartSquY.Name = "errorMStartSquY"
        errorMStartSquY.Text = ""
        errorMStartSquY.Location = New Drawing.Point(25, 105)
        errorMStartSquY.Size = New Drawing.Point(390, 15)
        errorMStartSquY.ForeColor = Drawing.Color.Red
        coordinatesGroupBox.Controls.Add(errorMStartSquY)


        endSquareLabel.Name = "endSquareLabel"
        endSquareLabel.Text = "End Square Coordinates (x|y):"
        endSquareLabel.Location = New Drawing.Point(25, 130)
        endSquareLabel.Size = New Drawing.Point(100, 30)
        coordinatesGroupBox.Controls.Add(endSquareLabel)

        endSquareXInputBox.Name = "endSquareXInputBox"
        endSquareXInputBox.Location = New Drawing.Point(125, 135)
        endSquareXInputBox.Size = New Drawing.Point(30, 20)
        coordinatesGroupBox.Controls.Add(endSquareXInputBox)

        errorMEndSquX.Name = "errorMEndSquX"
        errorMEndSquX.Text = ""
        errorMEndSquX.Location = New Drawing.Point(25, 160)
        errorMEndSquX.Size = New Drawing.Point(390, 15)
        errorMEndSquX.ForeColor = Drawing.Color.Red
        coordinatesGroupBox.Controls.Add(errorMEndSquX)

        endSquareYInputBox.Name = "endSquareYInputBox"
        endSquareYInputBox.Location = New Drawing.Point(175, 135)
        endSquareYInputBox.Size = New Drawing.Point(30, 20)
        coordinatesGroupBox.Controls.Add(endSquareYInputBox)

        errorMEndSquY.Name = "errorMEndSquy"
        errorMEndSquY.Text = ""
        errorMEndSquY.Location = New Drawing.Point(25, 175)
        errorMEndSquY.Size = New Drawing.Point(390, 15)
        errorMEndSquY.ForeColor = Drawing.Color.Red
        coordinatesGroupBox.Controls.Add(errorMEndSquY)

        Me.Controls.Add(coordinatesGroupBox)

        '===== modification of the accept button =====

        acceptInputsButton.Name = "acceptInputsButton"
        acceptInputsButton.Text = "Accept"
        acceptInputsButton.Location = New Drawing.Point(480, 125)
        acceptInputsButton.Size = New Drawing.Size(200, 100)
        acceptInputsButton.FlatStyle = FlatStyle.Popup
        AddHandler acceptInputsButton.Click, AddressOf Me.AcceptInputsClick
        Me.Controls.Add(acceptInputsButton)


        backButton.Name = "backButton"
        backButton.Text = "Back"
        backButton.Location = New Drawing.Point(480, 275)
        backButton.Size = New Drawing.Size(200, 100)
        backButton.FlatStyle = FlatStyle.Popup
        AddHandler backButton.Click, AddressOf Me.BackButtonClickInputScreen
        Me.Controls.Add(backButton)

    End Sub


    Private Sub AcceptInputsClick() 'when the "Accept" button is pressed, this subroutine is run, it checks whether all the inputs are valid

        Dim validValues As Boolean = True


        Dim nameInput As String = ""
        Dim nameMaxLength As Integer = 20

        Dim widthInput As Integer
        Dim heightInput As Integer

        Dim startSquareXInput As Integer
        Dim startSquareYInput As Integer

        Dim endSquareXInput As Integer
        Dim endSquareYInput As Integer


        Dim rangeMin As Integer = 2
        Dim rangeMax As Integer = 30

        For i = 0 To errorMessageArray.Length - 1
            errorMessageArray(i).Text = ""
        Next

        Try
            nameInput = nameInputBox.Text
            If nameInput = "" Then
                Throw New EmptyInputBox
            ElseIf nameInput.Length > nameMaxLength Then
                Throw New InputLengthInvalid(nameMaxLength)
            End If
        Catch empty As EmptyInputBox
            validValues = False
            OutputError(nameInputBox, errorMName, empty)
        Catch tooLong As InputLengthInvalid
            validValues = False
            OutputError(nameInputBox, errorMName, tooLong)
        Catch invalidDataType As System.InvalidCastException
            validValues = False
            OutputError(nameInputBox, errorMName, invalidDataType)
        Catch otherError As Exception
            validValues = False
            OutputError(nameInputBox, errorMName, otherError)
        End Try


        Try
            widthInput = widthInputBox.Text

            If Convert.ToDouble(widthInput) <> Convert.ToDouble(widthInputBox.Text) Then
                Throw New RoundingOccurred
            ElseIf widthInput > rangeMax Or widthInput < rangeMin Then
                Throw New InputOutOfRange
            ElseIf widthInput = vbEmpty Then
                Throw New EmptyInputBox
            End If
        Catch empty As EmptyInputBox
            validValues = False
            OutputError(widthInputBox, errorMWidth, empty)
        Catch invalidDataType As System.InvalidCastException
            validValues = False
            OutputError(widthInputBox, errorMWidth, invalidDataType)
        Catch outOfRange As InputOutOfRange
            validValues = False
            OutputError(widthInputBox, errorMWidth, outOfRange)
        Catch decimalInput As RoundingOccurred
            OutputError(widthInputBox, errorMWidth, decimalInput)
        Catch otherError As Exception
            validValues = False
            OutputError(widthInputBox, errorMWidth, otherError)
        End Try


        Try
            heightInput = heightInputBox.Text

            If Convert.ToDouble(heightInput) <> Convert.ToDouble(heightInputBox.Text) Then
                Throw New RoundingOccurred
            ElseIf heightInput > rangeMax Or heightInput < rangeMin Then
                Throw New InputOutOfRange
            ElseIf heightInput = vbEmpty Then
                Throw New EmptyInputBox
            End If
        Catch empty As EmptyInputBox
            validValues = False
            OutputError(heightInputBox, errorMHeight, empty)
        Catch invalidDataType As System.InvalidCastException
            validValues = False
            OutputError(heightInputBox, errorMHeight, invalidDataType)
        Catch outOfRange As InputOutOfRange
            validValues = False
            OutputError(heightInputBox, errorMHeight, outOfRange)
        Catch decimalInput As RoundingOccurred
            OutputError(heightInputBox, errorMHeight, decimalInput)
        Catch otherError As Exception
            validValues = False
            OutputError(heightInputBox, errorMHeight, otherError)
        End Try


        Try
            startSquareXInput = startSquareXInputBox.Text

            If Convert.ToDouble(startSquareXInput) <> Convert.ToDouble(startSquareXInputBox.Text) Then
                Throw New RoundingOccurred
            ElseIf startSquareXInput > widthInput Or startSquareXInput < 1 Then
                Throw New OutOfMazeDimensions("X", widthInput)
            ElseIf startSquareXInput = vbEmpty Then
                Throw New EmptyInputBox
            End If
        Catch empty As EmptyInputBox
            validValues = False
            OutputError(startSquareXInputBox, errorMStartSquX, empty)
        Catch invalidDataType As System.InvalidCastException
            validValues = False
            OutputError(startSquareXInputBox, errorMStartSquX, invalidDataType)
        Catch notInMaze As OutOfMazeDimensions
            validValues = False
            OutputError(startSquareXInputBox, errorMStartSquX, notInMaze)
        Catch decimalInput As RoundingOccurred
            OutputError(startSquareXInputBox, errorMStartSquX, decimalInput)
        Catch otherError As Exception
            validValues = False
            OutputError(startSquareXInputBox, errorMStartSquX, otherError)
        End Try

        Try
            startSquareYInput = startSquareYInputBox.Text

            If Convert.ToDouble(startSquareYInput) <> Convert.ToDouble(startSquareYInputBox.Text) Then
                Throw New RoundingOccurred
            ElseIf startSquareYInput > heightInput Or startSquareYInput < 1 Then
                Throw New OutOfMazeDimensions("Y", heightInput)
            ElseIf startSquareYInput = vbEmpty Then
                Throw New EmptyInputBox
            End If
        Catch decimalInput As RoundingOccurred
            OutputError(startSquareYInputBox, errorMStartSquY, decimalInput)
        Catch empty As EmptyInputBox
            validValues = False
            OutputError(startSquareYInputBox, errorMStartSquY, empty)
        Catch invalidDataType As System.InvalidCastException
            validValues = False
            OutputError(startSquareYInputBox, errorMStartSquY, invalidDataType)
        Catch notInMaze As OutOfMazeDimensions
            validValues = False
            OutputError(startSquareYInputBox, errorMStartSquY, notInMaze)
        Catch otherError As Exception
            validValues = False
            OutputError(startSquareYInputBox, errorMStartSquY, otherError)
        End Try


        Try
            endSquareXInput = endSquareXInputBox.Text

            If Convert.ToDouble(endSquareXInput) <> Convert.ToDouble(endSquareXInputBox.Text) Then
                Throw New RoundingOccurred
            ElseIf endSquareXInput > widthInput Or endSquareXInput < 1 Then
                Throw New OutOfMazeDimensions("X", widthInput)
            ElseIf endSquareXInput = vbEmpty Then
                Throw New EmptyInputBox
            End If
        Catch empty As EmptyInputBox
            validValues = False
            OutputError(endSquareXInputBox, errorMEndSquX, empty)
        Catch invalidDataType As System.InvalidCastException
            validValues = False
            OutputError(endSquareXInputBox, errorMEndSquX, invalidDataType)
        Catch notInMaze As OutOfMazeDimensions
            validValues = False
            OutputError(endSquareXInputBox, errorMEndSquX, notInMaze)
        Catch decimalInput As RoundingOccurred
            OutputError(endSquareXInputBox, errorMEndSquX, decimalInput)
        Catch otherError As Exception
            validValues = False
            OutputError(endSquareXInputBox, errorMEndSquX, otherError)
        End Try

        Try
            endSquareYInput = endSquareYInputBox.Text

            If Convert.ToDouble(endSquareYInput) <> Convert.ToDouble(endSquareYInputBox.Text) Then
                Throw New RoundingOccurred
            ElseIf endSquareYInput > heightInput Or endSquareYInput < 1 Then
                Throw New OutOfMazeDimensions("Y", heightInput)
            ElseIf endSquareYInput = vbEmpty Then
            ElseIf endSquareYInput = vbEmpty Then
                Throw New EmptyInputBox
            End If
        Catch empty As EmptyInputBox
            validValues = False
            OutputError(endSquareYInputBox, errorMEndSquY, empty)
        Catch invalidDataType As System.InvalidCastException
            validValues = False
            OutputError(endSquareYInputBox, errorMEndSquY, invalidDataType)
        Catch notInMaze As OutOfMazeDimensions
            validValues = False
            OutputError(endSquareYInputBox, errorMEndSquY, notInMaze)
        Catch decimalInput As RoundingOccurred
            OutputError(endSquareYInputBox, errorMEndSquY, decimalInput)
        Catch otherError As Exception
            validValues = False
            OutputError(endSquareYInputBox, errorMEndSquY, otherError)
        End Try

        If validValues = True Then 'to prevent the program from attempting to compare coordinates if they are invalid

            Try
                If startSquareXInput = endSquareXInput And startSquareYInput = endSquareYInput Then
                    Throw New SameCoordinates
                End If
            Catch samePosition As SameCoordinates
                validValues = False
                OutputError(startSquareXInputBox, errorMStartSquX, samePosition)
                OutputError(startSquareYInputBox, errorMStartSquY, samePosition)
                OutputError(endSquareXInputBox, errorMEndSquX, samePosition)
                OutputError(endSquareYInputBox, errorMEndSquY, samePosition)
            Catch otherError As Exception
                validValues = False
                OutputError(startSquareXInputBox, errorMStartSquX, otherError)
                OutputError(startSquareYInputBox, errorMStartSquY, otherError)
                OutputError(endSquareXInputBox, errorMEndSquX, otherError)
                OutputError(endSquareYInputBox, errorMEndSquY, otherError)
            End Try
        End If

        If validValues = True Then
            GeneratingMaze(nameInput, widthInput, heightInput, startSquareXInput, startSquareYInput, endSquareXInput, endSquareYInput)
        End If


    End Sub

    Public Sub SaveScreen() 'creates/loads the save screen where the user can choose to save the maze after seeing what it looks like

        Dim saveScreenGroupBox As New GroupBox
        Dim mazeOutputBox As New Label
        Dim saveInputsButton As New Button
        Dim backButton As New Button

        Me.Controls.Clear()
        Me.Size = New Drawing.Size(800, 700)

        saveScreenGroupBox.Name = "saveScreenGroupBox"
        saveScreenGroupBox.Location = New Drawing.Point(1, 1)
        saveScreenGroupBox.Size = New Drawing.Size(Me.Width - 50, Me.Height - 10)

        mazeOutputBox.Name = "mazeOutputBox"
        mazeOutputBox.Text = generatedMaze.OutputMaze()
        mazeOutputBox.Location = New Drawing.Point(7, 7)
        mazeOutputBox.Size = New Drawing.Size(450, 650)
        mazeOutputBox.Font = New Font("Lucida Console", 8, style:=FontStyle.Regular) 'font that has character's which have a fixed-width/monospaced (important when displaying maze)
        saveScreenGroupBox.Controls.Add(mazeOutputBox)


        saveInputsButton.Name = "saveInputsButton"
        saveInputsButton.Text = "Save"
        saveInputsButton.Location = New Drawing.Point(500, 100)
        saveInputsButton.Size = New Drawing.Size(200, 100)
        saveInputsButton.FlatStyle = FlatStyle.Popup
        AddHandler saveInputsButton.Click, AddressOf Me.SaveMaze
        saveScreenGroupBox.Controls.Add(saveInputsButton)

        backButton.Name = "backButton"
        backButton.Text = "Back"
        backButton.Location = New Drawing.Point(500, 250)
        backButton.Size = New Drawing.Size(200, 100)
        backButton.FlatStyle = FlatStyle.Popup
        AddHandler backButton.Click, AddressOf Me.BackButtonClickSaveScreen
        saveScreenGroupBox.Controls.Add(backButton)

        Me.Controls.Add(saveScreenGroupBox)

    End Sub

    Private Sub BackButtonClickInputScreen() 'if this subroutine is called upon (by pressing the back button on the generate maze screen) it clears and closes the screen
        Me.Controls.Clear()
        Me.Close()
    End Sub

    Private Sub BackButtonClickSaveScreen() 'if this subroutine is called upon (by pressing the back button on the save maze screen) it clears the screen and loads the maze generation screen
        Me.Controls.Clear()
        Me.CreateMazeGenerationScreen()
    End Sub


    Private Sub OutputError(ByVal textBox As TextBox, ByVal labelToChange As Label, ByVal errorType As Exception) 'outputs/deals with errors
        textBox.Clear() 'clears invalid input
        labelToChange.Text = errorType.Message 'outputs new error message
    End Sub

    Public Sub GeneratingMaze(ByVal name As String, ByVal width As Integer, ByVal height As Integer, ByVal startSquareX As Integer, ByVal startSquareY As Integer, ByVal endSquareX As Integer, ByVal endSquareY As Integer) 'generates the maze

        Dim tempSquare As New Square(-1, -1)
        tempSquare.ResetNodeNumber()

        Dim startSqu As New StartSquare(startSquareX, startSquareY) 'creating start square
        Dim endSqu As New EndSquare(endSquareX, endSquareY) 'creating end square

        '======== Generation of maze ========'
        generatedMaze = New Maze(name, width, height, startSqu, endSqu) 'creates the maze using the inputs
        SaveScreen()
        '===================================='
    End Sub

    Private Sub SaveMaze() 'saves the generated maze and outputs its ID in a MsgBox

        generatedMaze.SaveMaze() 'saves the maze
        MsgBox("The generated maze has an ID of: " & generatedMaze.GetID) 'outputs the ID of the maze
        Me.Close() 'closes the screen

    End Sub

End Class
