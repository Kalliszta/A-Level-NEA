Public Class Queue ' a circular priority queue

    Private maxSize As Integer 'the maxSize that the queue can be
    Private size As Integer = 0 'keeps track of the current size of the queue
    Private squareQueue() As Square 'an array to store the contents of the queue, as array starts at 0
    Private front As Integer = 0 'keeps track of the front of the queue
    Private rear As Integer = -1 'keeps track of the rear of the queue

    Enum sortByCost 'the types of costs that the BubbleSortQueue subroutine can order the queue by
        fCost
        hCost
        gCost
    End Enum

    Public Structure listHCost 'structure to make one variable (that can be put in e.g. a list) store two pieces information
        Dim contents As Square
        Dim index As Integer
    End Structure

    Public Sub New(ByVal inMaxSize As Integer) 'creates a new queue
        maxSize = inMaxSize
        ReDim squareQueue(maxSize)
    End Sub
    Public Sub Enqueue(ByVal inSquare As Square) 'adds the square passed into the parameter to the back of the queue
        If IsFull() = False Then
            rear += 1
            size += 1
            LoopRound()
            squareQueue(rear) = inSquare
        End If

    End Sub

    Public Function Dequeue() As Square 'returns the member/square at the front of the queue and removes it from the queue
        Dim originalFront As Integer = front
        If IsEmpty() = False Then
            front += 1
            size -= 1
            LoopRound()
            Return squareQueue(originalFront) 'return removed square
        End If

    End Function

    Public Function Peek() As Square 'returns the member/square at the front of the queue
        If IsEmpty() = False Then
            Return squareQueue(front)
        End If
    End Function

    Public Function IsFull() As Boolean 'returns whether the queue is full or not (returns True if it is and False if it isn't)
        If size = maxSize Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function IsEmpty() As Boolean 'returns whether the queue is empty or not (returns True if it is and False if it isn't)
        If size = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetMaxSize() As Integer 'returns the maxSize of the queue
        Return maxSize
    End Function

    Public Sub LoopRound() 'loops queue back to start if its front/rear is greater than the maxSize but not full, it makes the queue a circular queue
        If rear > maxSize Then
            rear = (rear Mod maxSize) - 1
        End If
        If front > maxSize Then
            front = (front Mod maxSize) - 1
        End If
    End Sub

    Public Function CheckNumInList(ByVal list As List(Of Integer), ByVal number As Integer) As Integer 'checks whether the inputted parameter value 'number' is in the inputted parameter value 'list', if so returns True
        For i = 0 To list.Count - 1
            If list(i) = number Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Sub BubbleSortQueue(ByVal costType As sortByCost) 'runs bubble sort on queue (to make it a priority queue) and orders it based on the costType passed into the parameter
        Randomize() 'allows the generation of random number

        Dim orderCostList As New List(Of Square)

        While IsEmpty() = False 'moves the contents of queue into a list
            orderCostList.Add(Dequeue)
        End While

        Dim orderCost(orderCostList.Count - 1) As Square

        For i = 0 To orderCost.Length - 1 'moves the contents of list into an array, the reason it was made a list first is because a list can change in size whereas an array cannot, by putting the squares into a list I can calculate the length which the array must be
            orderCost(i) = orderCostList.Item(i) 'they are put into an array as it is easier to change the order of items in an array
        Next

        Dim temp As Square
        Dim tempList As New List(Of listHCost)
        Dim tempListHCost As listHCost

        Dim fCostMin As Integer = -1
        Dim hCostMin As Integer = -1
        Dim randomNumber As Integer

        Dim change As Boolean
        Dim length As Integer = orderCost.Length - 1

        If orderCost.Length > 1 Then 'as no need to run the following code if there is only one element

            Do 'this DO loop performs a bubble sort on the contents of the array based in a certain costType
                change = False 'used to prevent the bubble sort from carrying on infinitely
                For i = 1 To length
                    If orderCost(i - 1).GetCost(costType) > orderCost(i).GetCost(costType) Then
                        temp = orderCost(i - 1)
                        orderCost(i - 1) = orderCost(i)
                        orderCost(i) = temp
                        change = True 'used to prevent the bubble sort from carrying on infinitely
                    End If
                Next
                length -= 1 'used to minimise the times the number FOR loops (as after every iteration of the DO loop the largest costType will be in the correct position)
            Loop Until change = False Or length = 0

            If costType = sortByCost.hCost Then

                For i = 0 To orderCost.Length - 1
                    If orderCost(i).GetFCost <> -1 And fCostMin = -1 Then 'only runs when fCostMin = -1 as fCost is in order from lowest to highest so anything after -1 (the default or the fCost of invalid squares) will be the lowest fCost
                        fCostMin = orderCost(i).GetFCost 'sets the F Cost of the current square (a member of orderCost) to be the minimum fCost
                        hCostMin = orderCost(i).GetHCost 'sets the H Cost of the current square (a member of orderCost) to be the minimum hCost
                    ElseIf orderCost(i).GetFCost = fCostMin And orderCost(i).GetHCost < hCostMin Then 'if a member of orderCost has the same fCost but has lower hCost as the square which set the previous minimum fCost & hCost, the following code is run
                        hCostMin = orderCost(i).GetHCost
                    End If
                Next

                For i = 0 To orderCost.Length - 1 'members of orderCost with the same fCost (lowest) and hCost (lowest) are put into a list
                    If orderCost(i).GetFCost = fCostMin And orderCost(i).GetHCost = hCostMin Then
                        tempListHCost.contents = orderCost(i)
                        tempListHCost.index = i
                        tempList.Add(tempListHCost)
                    End If
                Next

                randomNumber = (Int(Rnd() * (tempList.Count))) 'chooses random index, if there are e.g. two numbers same F and H cost then the index 0 or 1 can be chosen randomly

                temp = orderCost(randomNumber) 'as the contents of orderCost(i) is going to be replaced it is stored in a temp variable
                orderCost(randomNumber) = tempList.Item(randomNumber).contents 'the randomly selected member is stored in orderCost at the position 'i'
                orderCost(tempList.Item(randomNumber).index) = temp 'the previous contents of orderCost(i) is stored in the originally location of the randomly selected member




            End If

            If orderCost(0).GetFCost = orderCost(1).GetFCost And costType = sortByCost.fCost Then 'if the fCost of first two members of orderCost are the same, and the subroutine just ordered the queue based on the fCost the following code is run

                For i = 0 To (orderCost.Length - 1) 'puts the all the squares back into the queue
                    Enqueue(orderCost(i))
                Next

                BubbleSortQueue(sortByCost.hCost) 'runs the current subroutine again but passes hCost as the sortByCost

            Else

                For i = 0 To (orderCost.Length - 1) 'puts the all the squares back into the queue
                    Enqueue(orderCost(i))
                Next

            End If
        Else
            For i = 0 To (orderCost.Length - 1) 'puts the all the squares back into the queue
                Enqueue(orderCost(i))
            Next
        End If

    End Sub

End Class
