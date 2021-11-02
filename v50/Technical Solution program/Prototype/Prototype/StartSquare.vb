Public Class StartSquare
    Inherits Square 'inherits from Square class

    Public Sub New(ByVal xValue As Integer, ByVal yValue As Integer) 'creates new StartSquare using parameter inputs
        MyBase.New(xValue, yValue) 'runs the 'New' subroutine in the Square class that creates a new square
        startSquare = True
        node = True
        nodeNumber = nextNodeNumber
        nextNodeNumber += 1
    End Sub

    Public Overrides Function OutputString() As String 'overrides function in Square class and runs this subroutine instead
        Return "S"
    End Function

End Class
