Public Class StackOfSquares

    Private topCounter As Integer = -1
    Private maxSize As Integer
    Private squareStack() As Square 'as array starts at 0

    Public Sub New(ByVal inMaxSize As Integer) 'creates a new stack
        maxSize = inMaxSize
        ReDim squareStack(maxSize - 1)
    End Sub

    Public Sub Push(ByVal inSquare As Square) 'adds/pushes inputted square on top of the stack
        If IsFull() = False Then
            topCounter += 1
            squareStack(topCounter) = inSquare
        End If
    End Sub

    Public Function Pop() As Square 'removes and returns the square from the top of the stack
        If IsEmpty() = False Then
            topCounter -= 1
            Return squareStack(topCounter + 1)
        End If
    End Function

    Public Function Peek() As Square 'looks/returns (but doesn't remove) the square at the top of the stack
        If IsEmpty() = False Then
            Return squareStack(topCounter)
        End If
    End Function

    Public Function IsFull() As Boolean 'checks if the stack has exceeded its maximum size, returns true if it has and false if not
        If topCounter = maxSize Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function IsEmpty() As Boolean 'checks if the stack is empty, returns true if it has and false if not
        If topCounter = -1 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
