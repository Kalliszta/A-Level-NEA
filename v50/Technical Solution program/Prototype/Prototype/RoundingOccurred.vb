Public Class RoundingOccurred
    Inherits Exception
    Public Sub New()
        MyBase.New("You cannot enter a decimal, the input must be an integer") 'the associated error message with this error
    End Sub

End Class
