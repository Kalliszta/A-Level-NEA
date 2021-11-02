
Public Class EndSquare
    Inherits Square 'inherits from Square class

    Public Sub New(ByVal xValue As Integer, ByVal yValue As Integer) 'creates new EndSquare using parameter inputs
        MyBase.New(xValue, yValue) 'runs the 'New' subroutine in the Square class that creates a new square
        endSquare = True
        node = True
        nodeNumber = nextNodeNumber
        nextNodeNumber += 1
    End Sub

    Public Overrides Function OutputString() As String 'overrides function in Square class and runs this subroutine instead
        Return "E"
    End Function

End Class
