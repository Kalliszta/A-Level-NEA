Public Class Wall
    Inherits Square 'inherits from Square class

    Protected border As Boolean = False

    Public Sub New(ByVal xValue As Integer, ByVal yValue As Integer) 'creates new Wall using parameter inputs
        MyBase.New(xValue, yValue) 'runs the 'New' subroutine in the Square class that creates a new square
        wall = True
        node = False
    End Sub
    Public Function IsBorder() As Boolean 'returns whether the wall is a border. If it is, then it cannot become a space between two traversable squares that can be crossed
        Return border
    End Function
    Public Sub ChangeBorderStatus(ByVal inBorder As Boolean) 'changes the border status of wall (square) to inputted status
        border = inBorder
    End Sub
    Public Overrides Sub ChangeWallStatus(ByVal inWall As Boolean) 'changes wall status of wall (allowing it to be crossed)
        wall = inWall
    End Sub

    Public Overrides Function OutputString() As String 'overrides function in Square class and outputs a specific string depending on whether the Wall's wall boolean is true or false
        If wall = True Then
            Return "#" 'wall that cannot be crossed
        Else
            Return " " 'wall that can be crossed
        End If
    End Function

End Class
