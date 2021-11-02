Public Class Square
    Protected gCost As Integer
    Protected hCost As Integer
    Protected fCost As Integer

    Protected previousSquare As Square
    Protected discovered As Boolean = False
    Protected traversed As Boolean = False
    Protected wall As Boolean = False
    Protected node As Boolean = False 'all traversable squares (NodeSquare) have node = True, TempSquares (Square) / Wall (which a type of square) have node = False . The reason there is a separate variable called 'node', is so that the program can store whether a square was originally a traversable Square or a Wall. As you cannot just use the wall boolean to determine this as once a Wall (Square) becomes crossable (so there is no longer a wall between two squares), the wall boolean which was originally True becomes False. TempSquares cannot have a nodeNumer as they are not nodes. 
    Protected nodeNumber As Integer = 0
    Protected Shared nextNodeNumber As Integer = 1

    Protected startSquare As Boolean = False 'boolean to store whether the square is the start square (if so then True)
    Protected endSquare As Boolean = False 'boolean to store whether the end is the start square (if so then True)

    Public Structure coord 'a structure that stores two integers: xValue and yValue
        Dim xValue As Integer
        Dim yValue As Integer
    End Structure

    'the difference between coordinate and coordinateActual is that coordinate is the coordinates of square in the 2D array (grid) when the wall squares are included and coordinateActual is the actual coordinates of the square when the walls (between each traversable square) are excluded
    Protected coordinate As New coord
    Protected coordinateActual As New coord

    Public Sub New(ByVal xValue As Integer, ByVal yValue As Integer) 'creates a new square using inputs
        coordinate.xValue = xValue
        coordinate.yValue = yValue
        coordinateActual.xValue = (xValue + 1) / 2
        coordinateActual.yValue = (yValue + 1) / 2
    End Sub

    Public Function GetCoordinates() As coord 'returns coordinates for square in coord structure form
        Return coordinate
    End Function

    Public Sub ChangeXCord(ByVal xValue As Integer) 'changes x cord of a square to input, then applies required transformations
        coordinate.xValue = xValue
        coordinateActual.xValue = (xValue + 1) / 2
    End Sub

    Public Sub ChangeYCord(ByVal yValue As Integer) 'changes y cord of a square to input, then applies required transformations
        coordinate.yValue = yValue
        coordinateActual.yValue = (yValue + 1) / 2
    End Sub

    Public Function GetCoordinatesActual() As coord 'returns the actual coordinates for square in coord structure form
        Return coordinateActual
    End Function

    Public Sub UpdateSquare() 'updates fCost (using new gCost and hCost)
        fCost = gCost + hCost
    End Sub

    Public Sub ResetSquare() 'resets square so that it isn't discovered or traversed
        Dim tempSquare As Square
        discovered = False
        traversed = False
        ChangeGCost(0)
        ChangeHCost(0)
        ChangePreviousSquare(tempSquare) 'resets previous square to empty
    End Sub

    Public Function GetDiscoveredStatus() As Boolean 'returns the discovered status/boolean of square
        Return discovered
    End Function

    Public Sub Discover() 'discovers the square, represented by changing the discovered boolean of the square to true
        discovered = True
    End Sub

    Public Function GetTraversedStatus() As Boolean 'returns the traveresed status/boolean of square
        Return traversed
    End Function

    Public Sub Traverse() 'traverses the square, represented by changing the traversed boolean of the square to true
        traversed = True
    End Sub

    Public Function GetWallStatus() As Boolean 'return whether the square is a wall or not
        Return wall
    End Function

    Public Overridable Sub ChangeWallStatus(ByVal inWall As Boolean) 'changes the wall status of the square (if overridden)

    End Sub

    Public Overridable Function OutputString() As String 'returns associated string of square,(space outputted if not overridable)
        Return " "
    End Function

    Public Function GetGCost() As Integer 'return the gCost of associated square
        Return gCost
    End Function

    Public Function GetFCost() As Integer 'return the fCost of associated square
        Return fCost
    End Function

    Public Function GetHCost() As Integer 'return the hCost of associated square
        Return hCost
    End Function

    Public Sub ChangeGCost(ByVal inGCost As Integer) 'changes gCost to inputted value
        gCost = inGCost
        UpdateSquare()
    End Sub

    Public Sub ChangeFCost(ByVal inFCost As Integer) 'changes fCost to inputted value
        fCost = inFCost
        UpdateSquare()
    End Sub

    Public Sub ChangeHCost(ByVal inHCost As Integer) 'changes hCost to inputted value
        hCost = inHCost
        UpdateSquare()
    End Sub

    Public Sub ChangeHCost(ByVal endSquare As EndSquare) 'calulates hCost based on input
        hCost = ((endSquare.GetCoordinatesActual.xValue - Me.GetCoordinatesActual.xValue) * (endSquare.GetCoordinatesActual.xValue - Me.GetCoordinatesActual.xValue)) + ((endSquare.GetCoordinatesActual.yValue - Me.GetCoordinatesActual.yValue) * (endSquare.GetCoordinatesActual.yValue - Me.GetCoordinatesActual.yValue))
        UpdateSquare()
    End Sub

    Public Function GetCost(ByVal costType As Queue.sortByCost) As Integer 'returns the inputted cost type's associated value
        If costType = Queue.sortByCost.fCost Then
            Return GetFCost()
        ElseIf costType = Queue.sortByCost.hCost Then
            Return GetHCost()
        ElseIf costType = Queue.sortByCost.gCost Then
            Return GetGCost()
        Else
            Return -1
        End If
    End Function

    Public Function GetNodeStatus() As Boolean 'gets node status of associated square
        Return node
    End Function

    Public Function IsStartSquare() As Boolean 'returns whether square is the start square
        Return startSquare
    End Function

    Public Function IsEndSquare() As Boolean 'returns whether square is the end square
        Return endSquare
    End Function

    Public Function GetNodeNumber() As Integer 'returns the associated nodeNumber of the square
        If Me.GetNodeStatus = True Then
            Return nodeNumber
        Else
            Return 0 'returns zero if it isn't a node
        End If
    End Function

    Public Sub ResetNodeNumber() 'resets the Shared variable "nextNodeNumber" back to 1
        nextNodeNumber = 1
    End Sub

    Public Sub ChangePreviousSquare(ByVal inPreviousSquare As Square) 'changes the squares parent/previous square to the inputted square
        previousSquare = inPreviousSquare
    End Sub

    Public Function GetPreviousSquare() As Square 'returns the previous square of this square
        Return previousSquare
    End Function

End Class
