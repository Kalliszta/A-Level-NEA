Imports System.Drawing
Imports System.Windows.Forms
Public Class Import_Maze_Screen
    Dim connection As New System.Data.Odbc.OdbcConnection("DRIVER={MySQL ODBC 5.3 ANSI Driver};SERVER=localhost;PORT=3306;DATABASE=pathfindingdatabase;USER=root;PASSWORD=root;OPTION=3;")

    Private idInputBox As New TextBox
    Private errorMId As New Label
    Private importedMaze As Maze 'the maze imported is assigned this variable

    Private Sub CreateImportMazeScreen(sender As Object, e As EventArgs) Handles MyBase.Load 'when this class loaded this subroutine is run, this subroutine creates/loads the screen that allows the user to import a maze

        Dim idGroupBox As New GroupBox 'groupboxes are used to group together objects
        Dim idLabel As New Label

        Dim searchButton As New Button
        Dim backButton As New Button

        Me.Size = New Drawing.Size(750, 600) 'defines size for window

        idGroupBox.Name = "idGroupBox"
        idGroupBox.Location = New Drawing.Point(10, 10)
        idGroupBox.Size = New Drawing.Size(420, 100)

        idLabel.Name = "idLabel"
        idLabel.Text = "Enter ID of maze to import it:"
        idLabel.Location = New Drawing.Point(25, 45)
        idLabel.Size = New Drawing.Size(150, 25)
        idGroupBox.Controls.Add(idLabel)

        idInputBox.Name = "idInputBox"
        idInputBox.Location = New Drawing.Point(200, 42)
        idInputBox.Size = New Drawing.Size(75, 50)
        idGroupBox.Controls.Add(idInputBox)

        errorMId.Name = "errorMID"
        errorMId.Text = ""
        errorMId.Location = New Drawing.Point(25, 70)
        errorMId.Size = New Drawing.Point(390, 15)
        errorMId.ForeColor = Drawing.Color.Red
        idGroupBox.Controls.Add(errorMId)

        Me.Controls.Add(idGroupBox)

        searchButton.Name = "searchButton"
        searchButton.Text = "Search"
        searchButton.Location = New Drawing.Point(480, 125)
        searchButton.Size = New Drawing.Size(200, 100)
        searchButton.FlatStyle = FlatStyle.Popup
        AddHandler searchButton.Click, AddressOf Me.SearchButtonClick
        Me.Controls.Add(searchButton)


        backButton.Name = "backButton"
        backButton.Text = "Back"
        backButton.Location = New Drawing.Point(480, 275)
        backButton.Size = New Drawing.Size(200, 100)
        backButton.FlatStyle = FlatStyle.Popup
        AddHandler backButton.Click, AddressOf Me.BackButtonMenuScreen
        Me.Controls.Add(backButton)

    End Sub

    Private Sub SearchButtonClick() 'this subroutine is run when the search button is clicked, it attempts to search for the maze's ID inputted into the textbox in the pathfindingdatabase
        Dim id As Integer 'must save as ID, because the user may change the input within the input box while the program is running
        Dim tempSquare As New Square(-1, -1) 'used to reset the nodeNumber before start and end squares are created

        Try
            id = idInputBox.Text 'takes the input of the textbox and saves it in the id variable
            If Convert.ToDouble(id) <> Convert.ToDouble(idInputBox.Text) Then
                Throw New RoundingOccurred
            ElseIf SearchForMaze(id) = True Then 'if maze exists with the ID them it is imported as importedMaze ready to have algorithms run on it
            tempSquare.ResetNodeNumber() 'rests Shared variable to 1 as new maze
                importedMaze = New Maze(id) 'the maze is imported using the ID
                RunAlgorithmsScreen()
            Else
                Throw New MazeNotFound 'error which occurs when the maze with the associated ID cannot be found
            End If
        Catch notFound As MazeNotFound 'occurs when the maze with the associated ID cannot be found
            OutputError(idInputBox, errorMId, notFound)
        Catch invalidDataType As InvalidCastException 'occurs when the input is not an integer (so an invalid data type)
            OutputError(idInputBox, errorMId, invalidDataType)
        Catch otherError As Exception 'catches any other errors that occur
            OutputError(idInputBox, errorMId, otherError)
        End Try

    End Sub

    Private Sub BackButtonMenuScreen() 'if this subroutine is called upon (by pressing the back button on the run algorithms screen) it closes the connection as well as this screen
        connection.Close() 'closes connection
        Me.Close() 'closes this screen
    End Sub

    Private Sub BackButtonRunAlgorithmsScreen() 'if this subroutine is called upon (by pressing the back button on the compare data screen) it will load the 'RunAlgorithmsScreen'
        RunAlgorithmsScreen()
    End Sub

    Private Function SearchForMaze(ByVal id As Integer) 'searches maze with the inputted ID within the 'pathfindingdatabase'
        connection.Open() 'opens connection, to enable data to be retrieved/read from the 'pathfindingdatabase'

        Dim idFromTable As Odbc.OdbcDataReader
        Dim getID As New Odbc.OdbcCommand("SELECT mazeID FROM mazeinfo WHERE mazeID = '" & id & "'", connection)

        idFromTable = getID.ExecuteReader()

        Try 'try catch to see if a maze with inputted ID exists in the database if it does not or the input is invalid the code deals with it in the required way and outputs the associated error message
            If idFromTable.Read = True Then
                id = idFromTable.GetInt32(0)
            Else
                Throw New MazeNotFound
            End If
        Catch mazeNotExist As MazeNotFound
            connection.Close() 'closes connection
            Return False
        Catch empty As System.InvalidCastException
            connection.Close() 'closes connection
            Return False
        Catch otherError As Exception
            connection.Close() 'closes connection
            Return False
        End Try

        connection.Close() 'closes connection
        Return True

    End Function

    Private Sub OutputError(ByVal textBox As TextBox, ByVal labelToChange As Label, ByVal errorType As Exception) 'outputs/deals with errors
        textBox.Clear() 'clears invalid input
        labelToChange.Text = errorType.Message 'outputs new error message
    End Sub

    Private Sub RunAlgorithmsScreen() 'loads the screen that allows the user to run algorithms on the imported maze

        Dim optionsGroupBox As New GroupBox

        Dim mazeOutputBox As New Label

        Dim aStarSearchButton As New Button
        Dim breadthFirstSearchButton As New Button
        Dim dijkstrasAlgorithmButton As New Button
        Dim myAlgorithmButton As New Button '(extension)

        Dim compareDataButton As New Button

        Dim backButton As New Button

        Me.Controls.Clear()
        Me.Size = New Drawing.Size(830, 700) 'changes the size of the window/screen


        mazeOutputBox.Name = "mazeOutputBox"
        mazeOutputBox.Text = importedMaze.OutputMaze() 'the maze as a string is made to be the text of the label
        mazeOutputBox.Location = New Drawing.Point(27, 7)
        mazeOutputBox.Size = New Drawing.Size(450, 650)
        mazeOutputBox.Font = New Font("Lucida Console", 8, style:=FontStyle.Regular) 'I choose this font, as it has characters which have a fixed-width/monospaced (which is important when displaying maze)
        Me.Controls.Add(mazeOutputBox)


        optionsGroupBox.Name = "optionsGroupBox"
        optionsGroupBox.Location = New Drawing.Point(500, 25)
        optionsGroupBox.Size = New Drawing.Size(250, 600)


        aStarSearchButton.Name = "AStarSearchButton"
        aStarSearchButton.Text = "A Star Search"
        aStarSearchButton.Location = New Drawing.Point(50, 25)
        aStarSearchButton.Size = New Drawing.Size(150, 50)
        aStarSearchButton.FlatStyle = FlatStyle.Popup
        AddHandler aStarSearchButton.Click, AddressOf Me.RunAStar
        optionsGroupBox.Controls.Add(aStarSearchButton)

        breadthFirstSearchButton.Name = "BreadthFirstSearchButton"
        breadthFirstSearchButton.Text = "Breadth First Search"
        breadthFirstSearchButton.Location = New Drawing.Point(50, 125)
        breadthFirstSearchButton.Size = New Drawing.Size(150, 50)
        breadthFirstSearchButton.FlatStyle = FlatStyle.Popup
        AddHandler breadthFirstSearchButton.Click, AddressOf Me.RunBreadthFirst
        optionsGroupBox.Controls.Add(breadthFirstSearchButton)

        dijkstrasAlgorithmButton.Name = "DijkstrasAlgorithmButton"
        dijkstrasAlgorithmButton.Text = "Dijkstra's Algorithm"
        dijkstrasAlgorithmButton.Location = New Drawing.Point(50, 225)
        dijkstrasAlgorithmButton.Size = New Drawing.Size(150, 50)
        dijkstrasAlgorithmButton.FlatStyle = FlatStyle.Popup
        AddHandler dijkstrasAlgorithmButton.Click, AddressOf Me.RunDijkstras
        optionsGroupBox.Controls.Add(dijkstrasAlgorithmButton)

        myAlgorithmButton.Name = "myAlgorithmButton" 'extension
        myAlgorithmButton.Text = "My Algorithm"
        myAlgorithmButton.Location = New Drawing.Point(50, 325)
        myAlgorithmButton.Size = New Drawing.Size(150, 50)
        myAlgorithmButton.FlatStyle = FlatStyle.Popup
        AddHandler myAlgorithmButton.Click, AddressOf Me.RunMyAlgorithm
        optionsGroupBox.Controls.Add(myAlgorithmButton)


        compareDataButton.Name = "compareDataButton"
        compareDataButton.Text = "Compare Data"
        compareDataButton.Location = New Drawing.Point(50, 425)
        compareDataButton.Size = New Drawing.Size(150, 50)
        compareDataButton.FlatStyle = FlatStyle.Popup
        AddHandler compareDataButton.Click, AddressOf Me.CompareDataScreen
        optionsGroupBox.Controls.Add(compareDataButton)


        backButton.Name = "backButton"
        backButton.Text = "Back"
        backButton.Location = New Drawing.Point(50, 525)
        backButton.Size = New Drawing.Size(150, 50)
        backButton.FlatStyle = FlatStyle.Popup
        AddHandler backButton.Click, AddressOf Me.BackButtonMenuScreen
        optionsGroupBox.Controls.Add(backButton)


        Me.Controls.Add(optionsGroupBox)

    End Sub

    Private Sub RunAStar() 'runs the AStar search on the maze
        importedMaze.RunAlgorithm(Algorithm.Algorithms.AStar)
    End Sub

    Private Sub RunBreadthFirst() 'runs the BreadthFirst search on the maze
        importedMaze.RunAlgorithm(Algorithm.Algorithms.BreadthFirst)
    End Sub

    Private Sub RunDijkstras() 'runs the Dijkstras algorithm on the maze
        importedMaze.RunAlgorithm(Algorithm.Algorithms.Dijkstras)
    End Sub

    Private Sub RunMyAlgorithm() 'runs myAlgorithm search on the maze
        importedMaze.RunAlgorithm(Algorithm.Algorithms.MyAlgorithm)
    End Sub

    Private Function CreateLabel(ByVal text As String) As Label 'returns a label that was created using the parameter input
        Dim newLabel As New Label
        Dim newGroup As New GroupBox
        newLabel.Name = (text & "Label") 'given name for label
        newLabel.Text = text 'assigning the parameter input as the associated text of the label
        newLabel.AutoSize = True 'the label can change size if it needs to in order to fit the text into the label
        newLabel.Width = 120 'the original width of the label
        newLabel.Height = 150 'the original height of the label
        Return newLabel 'returns the created label
    End Function

    Private Function CreateTextBox(ByVal text As String) As TextBox 'returns a textbox that was created using the parameter input. A textbox is created so that the output inside it can be scrolled as textboxes can have scrollbars whereas labels cannot
        Dim newTextBox As New TextBox 'modified textbox so that behaviour is similar to a label's
        newTextBox.Name = (text & "TextBox") 'given name for textbox
        newTextBox.Text = text 'assigns the parameter input to be the text outputted in the textbox
        newTextBox.Multiline = True 'lets the text in the textbox be on multiple lines and not just one
        newTextBox.AutoSize = True 'enables the textbox to change size to fit the required data
        newTextBox.ReadOnly = True 'so no data can be inputted, only outputted (this is what makes it behave similar to a label)
        newTextBox.Width = 300 'the original width of the textbox
        newTextBox.Height = 150 'the original height of the textbox
        newTextBox.ScrollBars = ScrollBars.Vertical 'enables the vertical scrollbar within the textbox
        newTextBox.MaximumSize = New Size(400, 10000) 'maxiumum size of textbox
        Return newTextBox 'returns the created textbox
    End Function

    Private Sub CompareDataScreen() 'loads the screen that allows the user to compare data about process previously run on the imported maze
        Dim processData As List(Of String())
        Dim tableForProcessData As New TableLayoutPanel
        Dim backButton As New Button
        Dim columnNumber As Integer = 5

        Me.Controls.Clear() 'clears the window/screen

        tableForProcessData.Name = "tableForProcessData"
        tableForProcessData.Location = New Drawing.Point(10, 10)
        tableForProcessData.Size = New Size(775, 500)
        tableForProcessData.AutoScrollMinSize = New Size(0, 0)
        tableForProcessData.BorderStyle = BorderStyle.FixedSingle
        tableForProcessData.ColumnCount = columnNumber
        For i = 0 To columnNumber - 1
            tableForProcessData.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
        Next
        tableForProcessData.CellBorderStyle = BorderStyle.FixedSingle
        tableForProcessData.GrowStyle = TableLayoutPanelGrowStyle.AddRows

        backButton.Name = "backButton"
        backButton.Text = "Back"
        backButton.Location = New Drawing.Point(600, 550)
        backButton.Size = New Drawing.Size(150, 75)
        backButton.FlatStyle = FlatStyle.Popup
        AddHandler backButton.Click, AddressOf Me.BackButtonRunAlgorithmsScreen
        Me.Controls.Add(backButton)


        processData = importedMaze.LoadProcess()
        tableForProcessData.RowStyles.Add(New RowStyle(SizeType.AutoSize)) 'adds a new row
        tableForProcessData.RowCount += 1

        tableForProcessData.Controls.Add(CreateLabel("processID"), 0, tableForProcessData.RowCount - 1)
        tableForProcessData.Controls.Add(CreateLabel("algorithm"), 1, tableForProcessData.RowCount - 1)
        tableForProcessData.Controls.Add(CreateLabel("path"), 2, tableForProcessData.RowCount - 1)
        tableForProcessData.Controls.Add(CreateLabel("timeTaken"), 3, tableForProcessData.RowCount - 1)
        tableForProcessData.Controls.Add(CreateLabel("numberOfSquaresTraversed"), 4, tableForProcessData.RowCount - 1)

        For i = 0 To (processData.Count - 1)
            tableForProcessData.RowCount += 1 'the numerical row count is increased
            For k = 0 To processData(i).Length - 1
                If k = 2 Then 'when k=2 the current information being dealt with is the path from the start square to the end square, as this is long it is outputted in a textbox
                    tableForProcessData.Controls.Add(CreateTextBox((processData(i))(k)), k, tableForProcessData.RowCount - 1) 'the data is outputted in a textbox which is put into a cell within the table
                Else
                    tableForProcessData.Controls.Add(CreateLabel((processData(i))(k)), k, tableForProcessData.RowCount - 1) 'the data is outputted in a label which is put into a cell within the table
                End If
                tableForProcessData.RowStyles.Add(New RowStyle(SizeType.AutoSize)) 'the row count for row style
            Next
        Next

        'scrollbars
        tableForProcessData.AutoScroll = True
        tableForProcessData.AutoScrollPosition = New Point(0, tableForProcessData.VerticalScroll.Maximum)
        tableForProcessData.VerticalScroll.Enabled = True

        Me.Controls.Add(tableForProcessData) 'adds the table to the screen

    End Sub

End Class