Public Class EmptyInputBox
    Inherits Exception
    Public Sub New()
        MyBase.New("This box cannot be left empty") 'the associated error message with this error
    End Sub
End Class
