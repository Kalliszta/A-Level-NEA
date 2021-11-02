Public Class MazeNotFound
    Inherits Exception
    Public Sub New()
        MyBase.New("A maze with that ID cannot be found") 'the associated error message with this error
    End Sub

End Class
