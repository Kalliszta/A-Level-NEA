Public Class OutOfMazeDimensions
    Inherits Exception
    Public Sub New(ByVal valueType As Char, ByVal length As Integer)
        MyBase.New(valueType & ": Input isn't inside the given maze dimensions, input must be between 1 & " & length) 'the associated error message with this error
    End Sub
End Class
