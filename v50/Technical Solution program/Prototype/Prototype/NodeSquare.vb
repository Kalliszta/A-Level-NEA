Public Class NodeSquare
    Inherits Square

    Public Sub New(ByVal xValue As Integer, ByVal yValue As Integer) 'creates a new NodeSquare (a square that can be traversed)
        MyBase.New(xValue, yValue)
        node = True
        nodeNumber = nextNodeNumber
        nextNodeNumber += 1
    End Sub

End Class
