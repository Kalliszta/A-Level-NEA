Public Class InputOutOfRange
    Inherits Exception

    Public Sub New()
        MyBase.New("Input isn't in the valid range") 'the associated error message with this error
    End Sub

End Class
