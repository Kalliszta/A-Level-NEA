Public Class InputLengthInvalid

    Inherits Exception
    Public Sub New()
        MyBase.New("Input's length is too long") 'an associated error message with this error
    End Sub

    Public Sub New(ByVal length As Integer)
        MyBase.New("Input's length is too long, must be under " & length & " character(s) long") 'an associated error message with this error
    End Sub

End Class
