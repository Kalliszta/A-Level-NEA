Public Class SameCoordinates
    Inherits Exception

    Public Sub New()
        MyBase.New("The start and end square cannot have the same position/coordinates") 'the associated error message with this error
    End Sub

End Class
