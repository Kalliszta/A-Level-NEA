Public Class Maze

    Dim connection As New System.Data.Odbc.OdbcConnection("DRIVER={MySQL ODBC 5.3 ANSI Driver};SERVER=localhost;PORT=3306;DATABASE=pathfindingdatabase;USER=root;PASSWORD=root;OPTION=3;")

    Private id As Integer
    Private name As String
    Private grid(,) As Square
    Private adjacencyMatrix(,) As Boolean 'default boolean is false
    Private collectionOfNodeSquares() As Square
    Private algorithmVariable As Algorithm

    Private startSqu As StartSquare
    Private endSqu As EndSquare

    Private numberOfSquaresTraversed As Integer 'from startSqu to endSqu (estimation) (used as a measurement of the distance between the two points in squares
    Private endFound As Boolean
    Private pathToEnd As New List(Of Square) 'stores shortest path, its length-1 is the number squares needed to be traversed from the start to end square (including the start and end square)

    Private lengthOfTimeToComplete As Integer 'used to store the time it took for an algorithm to run
    Private startTime As DateTime 'used to store the starting time of when an algorithm is run

    Public Sub New(ByVal inName As String, ByVal width As Integer, ByVal height As Integer, ByVal inStartSqu As StartSquare, ByVal inEndSqu As EndSquare) 'creates a new Maze (object)

        connection.Open()

        Dim idFromTable As Odbc.OdbcDataReader
        Dim getID As New Odbc.OdbcCommand("SELECT MAX(mazeID) FROM mazeinfo", connection)

        idFromTable = getID.ExecuteReader()

        Try
            If idFromTable.Read = True Then
                id = idFromTable.GetInt32(0) + 1
            End If
        Catch empty As System.InvalidCastException 'if table is empty no data can be retrieved
            id = 1 'so id is made to be 1
        End Try


        name = inName
        ReDim grid(width * 2, height * 2) 'transformations so that walls can be stored in the grid variable
        inStartSqu.ChangeXCord(((inStartSqu.GetCoordinates.xValue) * 2) - 1)
        inStartSqu.ChangeYCord(((inStartSqu.GetCoordinates.yValue) * 2) - 1)
        startSqu = inStartSqu

        inEndSqu.ChangeXCord(((inEndSqu.GetCoordinates.xValue) * 2) - 1)
        inEndSqu.ChangeYCord(((inEndSqu.GetCoordinates.yValue) * 2) - 1)
        endSqu = inEndSqu

        ReDim collectionOfNodeSquares(((grid.GetLength(0) - 1) / 2) * ((grid.GetLength(1) - 1) / 2)) 'has a length of e.g. 16, but at index 0 it is empty.

        algorithmVariable = New Algorithm(Me, startSqu)

        GenerateMaze()

        connection.Close()

    End Sub

    Public Sub New(ByVal inputMazeID As Integer) 'creates a new Maze (object) by importing a maze from the table mazeinfo using the inputMazeID

        Dim tempWidth As Integer
        Dim tempHeight As Integer

        Dim tempXValue As Integer
        Dim tempYValue As Integer

        Dim wallStatusString As String = ""

        '-----

        id = inputMazeID
        connection.Open()

        '-----

        Dim processInfoFromTable As Odbc.OdbcDataReader
        Dim getProcessInfo As New Odbc.OdbcCommand("SELECT mazeName, mazeWidth, mazeHeight, startSquareX, startSquareY, endSquareX, endSquareY, wallStatus FROM mazeinfo WHERE mazeID = '" & id & "'", connection) 'selects the information needed to load the maze with inputted ID from the SQL table 'mazeinfo' from the database 'pathfindingdatabase'

        processInfoFromTable = getProcessInfo.ExecuteReader()


        While processInfoFromTable.Read = True 'stores the data retrieved from the table in variables
            name = (processInfoFromTable("mazeName"))

            tempWidth = (processInfoFromTable("mazeWidth"))
            tempHeight = (processInfoFromTable("mazeHeight"))
            ReDim grid(tempWidth * 2, tempHeight * 2)

            tempXValue = (processInfoFromTable("startSquareX"))
            tempYValue = (processInfoFromTable("startSquareY"))
            startSqu = New StartSquare((((tempXValue) * 2) - 1), (((tempYValue) * 2) - 1))

            tempXValue = (processInfoFromTable("endSquareX"))
            tempYValue = (processInfoFromTable("endSquareY"))
            endSqu = New EndSquare((((tempXValue) * 2) - 1), (((tempYValue) * 2) - 1))

            ReDim collectionOfNodeSquares(((grid.GetLength(0) - 1) / 2) * ((grid.GetLength(1) - 1) / 2)) 'has a length of e.g. 16, but at index 0 it is empty.

            wallStatusString = (processInfoFromTable("wallStatus"))

        End While

        '-----

        AddingValues(0, 0)

        'this double FOR loop uses the wallStatusString to assign whether a wall square is a wall or empty (so can be crossed)
        For y = 1 To (grid.GetLength(1))
            For x = 1 To (grid.GetLength(0))
                If Mid(wallStatusString, (x) + ((y - 1) * (grid.GetLength(0))), 1) = "1" Then
                    grid(x - 1, y - 1).ChangeWallStatus(True)
                Else
                    grid(x - 1, y - 1).ChangeWallStatus(False)
                End If
            Next
        Next

        GenerateAdjacencyMatrix()

        algorithmVariable = New Algorithm(Me, startSqu)

        connection.Close()
    End Sub

    Public Sub GenerateAdjacencyMatrix() 'creates an adjacency matrix which once created can easily & quickly be used to determine whether two squares neighbours/adjacent to one another
        ReDim adjacencyMatrix(((grid.GetLength(0) - 1) / 2) * ((grid.GetLength(1) - 1) / 2), ((grid.GetLength(0) - 1) / 2) * ((grid.GetLength(1) - 1) / 2))
        For y = 1 To (grid.GetLength(1) / 2) 'actual coordinates
            For x = 1 To (grid.GetLength(0) / 2)
                If grid((x * 2) - 1, (y * 2) - 1).GetNodeStatus = True Then

                    'looks at whether it is possible to go in the direction:

                    If grid((x * 2) - 1, (y * 2) - 2).GetWallStatus = False Then 'up
                        adjacencyMatrix(grid((x * 2) - 1, (y * 2) - 1).GetNodeNumber, grid((x * 2) - 1, (y * 2) - 3).GetNodeNumber) = True
                    End If

                    If grid((x * 2), (y * 2) - 1).GetWallStatus = False Then 'right
                        adjacencyMatrix(grid((x * 2) - 1, (y * 2) - 1).GetNodeNumber, grid((x * 2) + 1, (y * 2) - 1).GetNodeNumber) = True
                    End If

                    If grid((x * 2) - 1, (y * 2)).GetWallStatus = False Then 'down
                        adjacencyMatrix(grid((x * 2) - 1, (y * 2) - 1).GetNodeNumber, grid((x * 2) - 1, (y * 2) + 1).GetNodeNumber) = True
                    End If

                    If grid((x * 2) - 2, (y * 2) - 1).GetWallStatus = False Then 'left
                        adjacencyMatrix(grid((x * 2) - 1, (y * 2) - 1).GetNodeNumber, grid((x * 2) - 3, (y * 2) - 1).GetNodeNumber) = True
                    End If

                End If
            Next
        Next

    End Sub

    Public Function GetID() As String 'returns the id of the maze
        Return id
    End Function

    Public Function GetName() As String 'returns the name of the maze
        Return name
    End Function

    Public Function GetStartSquare() As StartSquare 'returns the start square of the maze
        Return startSqu
    End Function

    Public Function GetEndSquare() As EndSquare 'returns the end square of the maze
        Return endSqu
    End Function

    Sub GenerateWalls(ByVal tempSquare As Square) 'generates walls for the maze, can only be used to create mazes with one solution
        Randomize() 'needed for random number generation
        Dim randomPosition As Integer

        Dim tempVisitArray(3) As Boolean

        For i = 0 To tempVisitArray.Length - 1 'makes each each member in the array tempVisitArray store the boolean False
            tempVisitArray(i) = False
        Next

        tempSquare.Traverse() 'traverses tempSquare


        For i = 1 To 4 'to reach all possible traversable squares

            Do 'randomly chooses a position from 1, 2, 3, 4 where there will not be a wall
                randomPosition = (Int(Rnd() * (4))) + 1
            Loop Until tempVisitArray(randomPosition - 1) = False 'loops until the program randomly selects a direction that hasn't been visited yet (the -1 is there as array's start with an index of 0).

            Select Case randomPosition 'based on which number was randomly generated the wall in that direction becomes crossable
                Case 1 'up
                    If tempSquare.GetCoordinates.yValue - 2 > 0 Then
                        If grid(tempSquare.GetCoordinates.xValue, tempSquare.GetCoordinates.yValue - 2).GetTraversedStatus = False Then 'new random number if true, backtracking, counter to see if any squares left
                            grid(tempSquare.GetCoordinates.xValue, tempSquare.GetCoordinates.yValue - 1).ChangeWallStatus(False)
                            grid(tempSquare.GetCoordinates.xValue, tempSquare.GetCoordinates.yValue - 2).Traverse()
                            GenerateWalls(grid(tempSquare.GetCoordinates.xValue, tempSquare.GetCoordinates.yValue - 2))
                        End If
                    End If
                Case 2 'right
                    If tempSquare.GetCoordinates.xValue + 2 < grid.GetLength(0) - 1 Then
                        If grid(tempSquare.GetCoordinates.xValue + 2, tempSquare.GetCoordinates.yValue).GetTraversedStatus = False Then
                            grid(tempSquare.GetCoordinates.xValue + 1, tempSquare.GetCoordinates.yValue).ChangeWallStatus(False)
                            grid(tempSquare.GetCoordinates.xValue + 2, tempSquare.GetCoordinates.yValue).Traverse()
                            GenerateWalls(grid(tempSquare.GetCoordinates.xValue + 2, tempSquare.GetCoordinates.yValue))
                        End If
                    End If
                Case 3 'down
                    If tempSquare.GetCoordinates.yValue + 2 < grid.GetLength(1) - 1 Then
                        If grid(tempSquare.GetCoordinates.xValue, tempSquare.GetCoordinates.yValue + 2).GetTraversedStatus = False Then
                            grid(tempSquare.GetCoordinates.xValue, tempSquare.GetCoordinates.yValue + 1).ChangeWallStatus(False)
                            grid(tempSquare.GetCoordinates.xValue, tempSquare.GetCoordinates.yValue + 2).Traverse()
                            GenerateWalls(grid(tempSquare.GetCoordinates.xValue, tempSquare.GetCoordinates.yValue + 2))
                        End If
                    End If
                Case 4 'left
                    If tempSquare.GetCoordinates.xValue - 2 > 0 Then
                        If grid(tempSquare.GetCoordinates.xValue - 2, tempSquare.GetCoordinates.yValue).GetTraversedStatus = False Then
                            grid(tempSquare.GetCoordinates.xValue - 1, tempSquare.GetCoordinates.yValue).ChangeWallStatus(False)
                            grid(tempSquare.GetCoordinates.xValue - 2, tempSquare.GetCoordinates.yValue).Traverse()
                            GenerateWalls(grid(tempSquare.GetCoordinates.xValue - 2, tempSquare.GetCoordinates.yValue))
                        End If
                    End If
            End Select

            tempVisitArray(randomPosition - 1) = True 'this means that the square in that array with that position has been traversed (when generating the maze)

        Next

    End Sub

    Sub AddingValues(ByVal tempX As Integer, ByVal tempY As Integer) 'Adds/saves the start and end square to the correct coordinates within the maze/grid. The squares that should be walls in the grid are created using the Wall class. It also makes the border Walls in the grid, have a border status of TRUE so that the program won't make the wall boolean false for them later in the program
        Dim tempWall As New Wall(tempX, tempY)

        If tempX = startSqu.GetCoordinates.xValue And tempY = startSqu.GetCoordinates.yValue Then 'compares the x and y values of the temp square and the start square's coordinates to see if they are the same, if so then the square with those coordinates becomes the start square
            grid(tempX, tempY) = startSqu 'adds the start square to the grid (2D array) of squares

            collectionOfNodeSquares(startSqu.GetNodeNumber) = startSqu 'makes the inputted index (which is the associated node number of the start square) of the array store the start square itself (so that later the square can be accessed through the associated node number)

        ElseIf tempX = endSqu.GetCoordinates.xValue And tempY = endSqu.GetCoordinates.yValue Then 'compares the x and y values of the temp square and the end square's coordinates to see if they are the same, if so then the square with those coordinates becomes the end square
            grid(tempX, tempY) = endSqu 'adds the  end square to the grid (2D array) of squares

            collectionOfNodeSquares(endSqu.GetNodeNumber) = endSqu 'makes the inputted index (which is the associated node number of the end square) of the array store the end square itself (so that later the square can be accessed through the associated node number)

        ElseIf tempY Mod 2 = 0 Or tempX Mod 2 = 0 Then 'makes squares that should be walls as a default, Walls
            If tempY = 0 Or tempX = 0 Or tempY = (grid.GetLength(1) - 1) Or tempX = (grid.GetLength(0) - 1) Then 'sees whether the Wall needs to be a border or not, (when doing .GetLength on a 2D array 0 for x length and 1 for y length)
                tempWall.ChangeBorderStatus(True) 'makes the border status of the Wall TRUE
            End If
            grid(tempX, tempY) = tempWall 'adds the tempWall to the grid (2D array) of squares (& wall as Wall inherits from the Square class)
        Else
            Dim tempSquare As New NodeSquare(tempX, tempY) 'inside if statement as if outside then nodeNumber increases in the Square Class despite the square being a temp square

            grid(tempX, tempY) = tempSquare 'adds the tempSquare to the grid (2D array) of squares

            collectionOfNodeSquares(tempSquare.GetNodeNumber) = tempSquare 'makes the inputted index (which is the associated node number of the square) of the array store the square itself (so that later the square can be accessed through the associated node number)

        End If



        If tempX = grid.GetLength(0) - 1 Then 'custom loop, sees whether the tempX integer is equal to the width of the maze/grid (the 2D array)
            tempX = 0 'if so resets the tempX to 0 so that the next row can be generated where tempY increases by one and tempX is 0 again
            If tempY <> grid.GetLength(1) - 1 Then  'checks whether tempY is equal to the height of the maze/grid (the 2D array), if so the generation is complete
                tempY += 1 'if it isn't equal to the height then tempY increments by 1
                AddingValues(tempX, tempY) 'recursion until the generation is complete
            End If
        Else 'else tempX increases by one and recursion until the generation is complete
            tempX += 1
            AddingValues(tempX, tempY)
        End If

    End Sub

    Public Sub GenerateMaze() 'generates the maze so that is ready to have pathfinding algorithms run on it, error handling needed for unsuitable dimensions

        '======== Generation of Start & End Square, Walls/Borders & Adjacency Matrix ========'

        AddingValues(0, 0)
        GenerateWalls(startSqu) 'attempts to randomly generate a path from the start square to the end square 

        GenerateAdjacencyMatrix()

        '===================================================================================='


    End Sub

    Public Function OutputMaze() As String 'returns the string representation of the maze
        Dim mazeString As String = ""
        For i = 0 To grid.GetLength(1) - 1
            For k = 0 To grid.GetLength(0) - 1
                mazeString &= grid(k, i).OutputString()
            Next
            mazeString &= vbCrLf
        Next
        Return mazeString
    End Function

    Public Sub SaveMaze() 'saves the maze into the mazeinfo table in the pathfindingdatabase

        connection.Open()

        Dim wallStatusString As String = "" 'stored as a string of 1s and 0s

        For y = 0 To grid.GetLength(1) - 1
            For x = 0 To grid.GetLength(0) - 1

                If grid(x, y).GetWallStatus = True Then
                    wallStatusString &= "1" 'represents the wall status being TRUE
                Else
                    wallStatusString &= "0" 'represents the wall status being FALSE
                End If

            Next
        Next

        Dim sql As New Odbc.OdbcCommand("INSERT INTO mazeinfo(mazeID,mazeName,mazeWidth,mazeHeight,startSquareX,startSquareY,endSquareX,endSquareY,wallStatus) VALUES ('" & id & "', '" & name & "', '" & ((grid.GetLength(0) - 1) / 2) & "', '" & ((grid.GetLength(1) - 1) / 2) & "', '" & startSqu.GetCoordinatesActual.xValue & "', '" & startSqu.GetCoordinatesActual.yValue & "', '" & endSqu.GetCoordinatesActual.xValue & "', '" & endSqu.GetCoordinatesActual.yValue & "', '" & wallStatusString & "')", connection) 'data is stored in the SQL table mazeinfo
        sql.ExecuteNonQuery() 'executes the query above

        connection.Close()

    End Sub

    Public Sub ResetMaze() 'resets all squares in the maze/grid and all other required information that needs to be reset before creating a new maze

        Dim tempSquare As New Square(-1, -1)

        tempSquare.ResetNodeNumber()


    End Sub

    Public Sub ResetForSearch() 'resets all squares in the maze/grid and all other required information that needs to be reset

        For i = 0 To grid.GetLength(1) - 1
            For k = 0 To grid.GetLength(0) - 1
                grid(k, i).ResetSquare()
            Next
        Next

        numberOfSquaresTraversed = 0
        ChangeEndFoundStatus(False)
        algorithmVariable.ResetInformation()

    End Sub

    Public Sub RunAlgorithm(ByVal type As Algorithm.Algorithms) 'depending on the parameter input runs a certain algorithm

        ResetForSearch() 'as visit nodes were used previously when generating the maze

        If type = Algorithm.Algorithms.AStar Then
            StartTimer() 'timer is started here separately to make the time measured as accurate as possible
            pathToEnd = algorithmVariable.StartAStarSearch() 'once the algorithm is run, the path from the start square to the end square is returned and stored in the variable pathToEnd
            EndTimer() 'timer is ended here separately to make the time measured as accurate as possible

        ElseIf type = Algorithm.Algorithms.BreadthFirst Then
            StartTimer() 'timer is started here separately to make the time measured as accurate as possible
            pathToEnd = algorithmVariable.StartBreadthFirstSearch() 'once the algorithm is run, the path from the start square to the end square is returned and stored in the variable pathToEnd
            EndTimer() 'timer is ended here separately to make the time measured as accurate as possible

        ElseIf type = Algorithm.Algorithms.Dijkstras Then
            StartTimer() 'timer is started here separately to make the time measured as accurate as possible
            pathToEnd = algorithmVariable.StartDijkstrasAlgorithm() 'once the algorithm is run, the path from the start square to the end square is returned and stored in the variable pathToEnd
            EndTimer() 'timer is ended here separately to make the time measured as accurate as possible

        ElseIf type = Algorithm.Algorithms.MyAlgorithm Then
            StartTimer() 'timer is started here separately to make the time measured as accurate as possible
            pathToEnd = algorithmVariable.StartMyAlgorithm() 'once the algorithm is run, the path from the start square to the end square is returned and stored in the variable pathToEnd
            EndTimer() 'timer is ended here separately to make the time measured as accurate as possible

        End If


        'For i = 0 To pathToEnd.Count - 1 'remove as only needed for testing !!!
        '    Console.WriteLine(pathToEnd(i).GetCoordinatesActual.xValue & pathToEnd(i).GetCoordinatesActual.yValue)
        'Next

        SaveProcessInfo(type)

    End Sub

    Public Sub SaveProcessInfo(ByVal algorithmType As Algorithm.Algorithms) 'saves the current information stored in variables about a process in the SQL table algorithmsrun

        Dim algorithmID As Integer

        Dim processID As Integer
        Dim processIDFromTable As Odbc.OdbcDataReader
        Dim getprocessID As New Odbc.OdbcCommand("SELECT MAX(processID) FROM algorithmsrun", connection) 'gets the maximum processID from the algorithmsrun table, this is done so that the new process can have the next possible unique processID

        Dim pathString As String = ""

        connection.Open()

        If algorithmType = Algorithm.Algorithms.AStar Then
            algorithmID = 1 'the associated algorithmID of AStar
        ElseIf algorithmType = Algorithm.Algorithms.BreadthFirst Then
            algorithmID = 2 'the associated algorithmID of BreadthFirst
        ElseIf algorithmType = Algorithm.Algorithms.Dijkstras Then
            algorithmID = 3 'the associated algorithmID of Dijkstras
        ElseIf algorithmType = Algorithm.Algorithms.MyAlgorithm Then
            algorithmID = 4 'the associated algorithmID of MyAlgorithm
        End If


        processIDFromTable = getprocessID.ExecuteReader() 'gets the current maximum process ID

        Try
            If processIDFromTable.Read = True Then
                processID = processIDFromTable.GetInt32(0) + 1
            End If
        Catch empty As System.InvalidCastException 'if table is empty no data can be retrieved
            processID = 1 'so processID is made to 1
        End Try

        For i = 0 To pathToEnd.Count - 1 'gets the node number of each square in the list "pathToEnd" and puts them into a string where they are each separated by commas (",")
            If i <> pathToEnd.Count - 1 Then
                pathString = pathString & pathToEnd.Item(i).GetNodeNumber & ","
            Else
                pathString = pathString & pathToEnd.Item(i).GetNodeNumber
            End If
        Next

        Dim sql As New Odbc.OdbcCommand("INSERT INTO algorithmsrun(processID,mazeID,algorithmID,pathToEnd,timeTakenToRun,numberOfSquaresTraversed) VALUES ('" & processID & "', '" & id & "', '" & algorithmID & "', '" & pathString & "', '" & lengthOfTimeToComplete & "', '" & numberOfSquaresTraversed & "')", connection) 'inputs the values stored in the variables into the SQL table algorithmsrun
        sql.ExecuteNonQuery() 'executes the SQL statement above ^

        connection.Close()

    End Sub

    Public Function LoadProcess() As List(Of String()) 'loads information about processes (when an algorithm is run on a maze) from the pathfindingdatabase

        Dim results As New List(Of String()) 'a list as the number elements/members which will be stored in it is unknown
        Dim record(4) As String
        Dim emptyRecord(4) As String
        Dim pathString As String
        Dim nodeArray() As String

        connection.Open()

        Dim processInfoFromTable As Odbc.OdbcDataReader
        Dim getProcessInfo As New Odbc.OdbcCommand("SELECT processID, algorithmName, pathToEnd, timeTakenToRun, numberOfSquaresTraversed FROM algorithmsrun, typesofalgorithms WHERE algorithmsrun.mazeID = '" & id & "' AND typesofalgorithms.algorithmID = algorithmsrun.algorithmID", connection) 'allow user to import info based on processID

        processInfoFromTable = getProcessInfo.ExecuteReader()


        While processInfoFromTable.Read = True
            record = emptyRecord.Clone() 'the array "record" is updated to be a clone of "emptyRecord" (so that there is a new object reference associated with the array "record")
            record(0) = (processInfoFromTable("processID")) 'the retrieved processID of the current record/selection from the SQL table is put into the array record at index 0
            record(1) = (processInfoFromTable("algorithmName")) 'the retrieved algorithmName of the current record/selection from the SQL table is put into the array record at index 1

            nodeArray = Split((processInfoFromTable("pathToEnd")), ",") 'the retrieved pathToEnd (which is a list of node numbers that represent the path from the start square to the end square) of the current record/selection from the SQL table is split at comma (",") and put into the array nodeArray
            For Each nodeNumber In nodeArray
                pathString = pathString & "(" & GetSquareUsingNodeNumber(nodeNumber).GetCoordinatesActual.xValue & "," & GetSquareUsingNodeNumber(nodeNumber).GetCoordinatesActual.yValue & ")" 'using the node number of a square, the square's actual coordinates are retrieved, and add to a string of coordinates
            Next
            record(2) = pathString 'the string of coordinates (that show the path from the start square to the end square) is stored in the array "record" at the index 2
            pathString = "" 'pathString is made empty again for the record/selection from the SQL database (as multiple records/results may have been selected using the one query)
            record(3) = (processInfoFromTable("timeTakenToRun")) 'the retrieved timeTakenToRun (in microseconds) is put into the array 'record' at the index 3
            record(4) = (processInfoFromTable("numberOfSquaresTraversed")) 'the retrieved numberOfSquaresTraversed is put into the array 'record' at the index 4

            results.Add(record) 'the current record array is add to the list "results"

        End While

        connection.Close()

        Return results

    End Function

    Public Function GetIfAdjacentSquares(ByVal currentSquare As Square, ByVal targetSquare As Square) As Boolean 'returns whether the two squares are adjacent/neighbours or not
        If adjacencyMatrix(currentSquare.GetNodeNumber, targetSquare.GetNodeNumber) = True Then 'or other way round (but checking both is time consuming)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetNeighbours(ByVal currentSquare As Square) As List(Of Square) 'returns a list of the neighbours/adjacent squares of the square inputted into the parameter
        Dim neighbours As New List(Of Square)
        For i = 1 To adjacencyMatrix.GetLength(0) - 1
            If adjacencyMatrix(currentSquare.GetNodeNumber, i) = True Then
                neighbours.Add(GetSquareUsingNodeNumber(i))
            End If
        Next
        Return neighbours
    End Function

    Public Function GetSquareUsingNodeNumber(ByVal nodeNumber As Integer) As Square 'returns the square with the node number inputted into the parameter
        Return collectionOfNodeSquares(nodeNumber)
    End Function

    Public Function IsEndFound() As Boolean 'returns the boolean of endFound, if it is TRUE then the end square has been found, if it is FALSE then the end square hasn't been found
        Return endFound
    End Function

    Public Sub ChangeEndFoundStatus(ByVal status As Boolean) 'changes the endFound status to the boolean parameter input
        endFound = status
    End Sub

    Public Sub IncreaseNumberOfSquaresTraversed() 'increments the counter which stores the numberOfSquaresTraversed
        numberOfSquaresTraversed += 1
    End Sub

    Public Function GetPath() As List(Of Square) 'returns the list of squares which must be traversed to get from the start square to the end square
        Return pathToEnd
    End Function

    Public Function GetCurrentTime() As Date 'return the current time
        Return Date.Now
    End Function

    Public Sub StartTimer() 'used to assign the current time as the startTime (of when the algorithm started running)
        startTime = GetCurrentTime()
    End Sub

    Public Function CalculateMicroseconds(ByVal endTime As Date) As Integer 'returns the time in microseconds (but using integer data type)
        Return (endTime.Ticks - startTime.Ticks) / 10 ' calculates the time it took in microseconds for an algorithm to be run on a maze
    End Function

    Public Sub EndTimer() 'used to get the lengthOfTimeToComplete (the time it took for an algorithm to be run on the maze) through using the startTime and getting the current time (endTime)
        Dim endTime As New Date
        Dim newStart As New Date 'used to reset startTime
        Try
            endTime = GetCurrentTime() 'to stop the 'timer' here so that the following processes are not timed as well
            lengthOfTimeToComplete = CalculateMicroseconds(endTime) 'calculates the lengthOfTimeToComplete in microseconds
            startTime = newStart 'resets the startTime to default
        Catch tooLong As OverflowException
            lengthOfTimeToComplete = -1 'stored if it takes too long to run an algorithm on a maze
        End Try
    End Sub

    Public Function GetTimeLength() As Integer 'returns the time taken to run an algorithm in microseconds (using integer data type)
        Return lengthOfTimeToComplete
    End Function

    Public Function GetCollectionOfNodes() As Square() 'returns the array of squares where the index of each square is its node number
        Return collectionOfNodeSquares
    End Function

End Class