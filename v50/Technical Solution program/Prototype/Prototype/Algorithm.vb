Public Class Algorithm 'stores a collection of different algorithms

    Protected queueOfSquaresToVisit As Queue
    Protected startSqu As StartSquare
    Protected maze As Maze

    Public Enum Algorithms 'algorithm types
        AStar
        BreadthFirst
        Dijkstras
        MyAlgorithm
    End Enum

    Public Sub New(ByVal inputMaze As Maze, ByVal parameterStartSqu As StartSquare) 'creates a new Algorithm object
        startSqu = parameterStartSqu 'stores the parameter input in the startSqu variable
        maze = inputMaze
        queueOfSquaresToVisit = New Queue(maze.GetCollectionOfNodes.Length - 1) 'creates a new queue
    End Sub

    Public Function StartAStarSearch() As List(Of Square) 'a function that returns the shortest path from the start node/square to the end node/square which has been calculated using the A* Search
        queueOfSquaresToVisit.Enqueue(startSqu) 'adds the start square to the queue
        startSqu.Discover() 'makes the start square discovered
        AStarSearch(0) 'starts the A* Search on the maze
        Return GetPath(maze.GetEndSquare) 'returns the path from the start square to the end square
    End Function

    Public Sub AStarSearch(ByVal distanceFromStart As Integer) 'subroutine that runs an A* Search on the maze
        Dim currentSquare As Square
        Dim neighbours As List(Of Square) 'a list which will store a list of neighbouring/adjacent squares of the current square


        If queueOfSquaresToVisit.IsEmpty = False And maze.IsEndFound = False Then 'sees whether the queue is empty or the end has been found
            currentSquare = queueOfSquaresToVisit.Dequeue() 'gets the square at the front of the priority queue
            currentSquare.Traverse() 'marks the square as traversed

            maze.IncreaseNumberOfSquaresTraversed() 'increments the counter that keeps track of the numberOfSquaresTraversed

            If currentSquare.IsEndSquare = True Then 'checks whether the current square is the end square
                maze.ChangeEndFoundStatus(True) 'the endFound variable is made to be TRUE in the Maze class, to indicate that end square has been found

            Else

                neighbours = maze.GetNeighbours(currentSquare) 'gets a list of the current square's neighbouring/adjacent squares

                distanceFromStart += 1 'increments the counter that stores the distance from the start which is used to calculate the gCost of a square

                For Each square In neighbours 'every square (item) in the neighbours list, has the code in the FOR loop run on it

                    If square.GetTraversedStatus = False And square.GetDiscoveredStatus = False Then 'if GetDiscoveredStatus = True, it means the square is/has already been in queueofSquaresToVisit. GetTraversedStatus = True , means that the square has been fully explored
                        queueOfSquaresToVisit.Enqueue(square) 'adds the neighbouring/adjacent square of current square being traversed to the queue
                        square.Discover() 'marks the neighbouring/adjacent square as discovered, so that they cannot be added to the queue again
                        square.ChangePreviousSquare(currentSquare) 'makes the current square of the neighbour square the previous/parent square
                        square.ChangeGCost(distanceFromStart) 'changes the gCost
                        square.ChangeHCost(maze.GetEndSquare) 'changes the fCost, square is updated automatically once a cost is changed
                    ElseIf square.GetTraversedStatus = False Then 'if a square has been discovered (so found) but not traversed then the following code is run
                        Dim tempSquare As Square 'a tempSquare is used so that if the newly calculated fCost isn't lower than the previous one, nothing is change. Another reason it is used, is so that the two fCosts can be compared
                        tempSquare = square
                        tempSquare.ChangePreviousSquare(currentSquare)
                        tempSquare.ChangeGCost(distanceFromStart)
                        tempSquare.ChangeHCost(maze.GetEndSquare)
                        If square.GetFCost > tempSquare.GetFCost Then 'sees whether the new fCost is lower than the current one, if it is lower than it is made to be the fCost of the square by making the square the tempSquare
                            square = tempSquare
                        End If
                    End If
                Next

                queueOfSquaresToVisit.BubbleSortQueue(Queue.sortByCost.fCost) 'a bubble sort is run on the queue based on the fCost
                AStarSearch(distanceFromStart) 'recursion occurs

            End If

        End If

    End Sub

    Public Function StartBreadthFirstSearch() As List(Of Square) 'a function that returns the shortest path from the start node/square to the end node/square which has been calculated using Breadth First Search
        queueOfSquaresToVisit.Enqueue(startSqu) 'adds the start square to the queue
        startSqu.Discover() 'makes the start square discovered
        BreadthFirstSearch() 'starts Breadth First Search on the maze
        Return GetPath(maze.GetEndSquare) 'returns the path from the start square to the end square
    End Function

    Public Sub BreadthFirstSearch() 'subroutine that runs Breadth First Search on the maze, uses a normal queue as the bubblesort procedure is never run on the queue (which is what makes the queue a priority queue)
        Dim currentSquare As Square
        Dim neighbours As List(Of Square) 'a list which will store a list of neighbouring/adjacent squares of the current square


        If queueOfSquaresToVisit.IsEmpty = False And maze.IsEndFound = False Then 'sees whether the queue is empty, if it isn't than the following code is run

            currentSquare = queueOfSquaresToVisit.Dequeue() 'gets the square at the front of the queue
            'no need to mark a square as traversed as once it is discovered, it is added to the queue and there is no rearrangement of the queue

            maze.IncreaseNumberOfSquaresTraversed() 'increments the counter that keeps track of the numberOfSquaresTraversed

            If currentSquare.IsEndSquare = True Then 'checks whether the current square is the end square
                maze.ChangeEndFoundStatus(True) 'the endFound variable is made to be TRUE in the Maze class, to indicate that end square has been found

            Else

                neighbours = maze.GetNeighbours(currentSquare) 'gets a list of the current square's neighbouring/adjacent squares

                For Each square In neighbours 'every square (item) in the neighbours list, has the code in the FOR loop run on it
                    If square.GetDiscoveredStatus = False Then 'if GetDiscoveredStatus = True, it means the square is in queueofSquaresToVisit
                        queueOfSquaresToVisit.Enqueue(square) 'adds the neighbouring/adjacent square of current square being traversed to the queue
                        square.Discover() 'marks the neighbouring/adjacent square as discovered, so that they cannot be added to the queue again
                        square.ChangePreviousSquare(currentSquare) 'makes the current square of the neighbour square the previous/parent square
                    End If
                Next

                BreadthFirstSearch() 'recursion occurs

            End If

        End If
    End Sub

    Public Function StartDijkstrasAlgorithm() As List(Of Square) 'a function that returns the shortest path from the start node/square to the end node/square which has been calculated using Dijkstra's Algorithm
        queueOfSquaresToVisit.Enqueue(startSqu) 'adds the start square to the queue
        startSqu.Discover() 'makes the start square discovered
        DijkstrasAlgorithm(0) 'starts Dijkstra's Algorithm on the maze
        Return GetPath(maze.GetEndSquare) 'returns the path from the start square to the end square
    End Function

    Public Sub DijkstrasAlgorithm(ByVal distanceFromStart As Integer) 'subroutine that runs Dijkstra's Algorithm on the maze
        Dim currentSquare As Square
        Dim neighbours As List(Of Square) 'a list which will store a list of neighbouring/adjacent squares of the current square


        If queueOfSquaresToVisit.IsEmpty = False And maze.IsEndFound = False Then

            currentSquare = queueOfSquaresToVisit.Dequeue() 'gets the square at the front of the priority queue
            currentSquare.Traverse() 'marks the square as traversed

            maze.IncreaseNumberOfSquaresTraversed()  'increments the counter that keeps track of the numberOfSquaresTraversed

            If currentSquare.IsEndSquare = True Then 'checks whether the current square is the end square
                maze.ChangeEndFoundStatus(True) 'the endFound variable is made to be TRUE in the Maze class, to indicate that end square has been found

            Else
                'remove as only need for testing !!!
                '                Console.WriteLine("
                'GCost: " & currentSquare.GetGCost & "
                'HCost: " & currentSquare.GetHCost & "
                'FCost: " & currentSquare.GetFCost & "
                'Coords: " & currentSquare.GetCoordinatesActual.xValue & " " & currentSquare.GetCoordinatesActual.yValue)

                neighbours = maze.GetNeighbours(currentSquare) 'gets a list of the current square's neighbouring/adjacent squares

                distanceFromStart += 1 'increments the counter that stores the distance from the start which is used to calculate the gCost of a square

                For Each square In neighbours 'every square (item) in the neighbours list, has the code in the FOR loop run on it
                    If square.GetTraversedStatus = False And square.GetDiscoveredStatus = False Then 'if GetDiscoveredStatuss = True, it means the square is/has already been in queueofSquaresToVisit. GetTraversedStatus = True , means that the square has been fully explored
                        queueOfSquaresToVisit.Enqueue(square) 'adds the neighbouring/adjacent square of current square being traversed to the queue
                        square.Discover() 'marks the neighbouring/adjacent square as discovered, so that they cannot be added to the queue again
                        square.ChangePreviousSquare(currentSquare) 'makes the current square of the neighbour square the previous/parent square
                        square.ChangeGCost(distanceFromStart) 'gCost is the distance from the start, changes the associated gCost of the square

                    ElseIf square.GetTraversedStatus = False Then 'if a square has been discovered (so found) but not traversed then the following code is run
                        Dim tempSquare As Square 'a tempSquare is used so that if the newly calculated gCost isn't lower than the previous one, nothing is change. Another reason it is used, is so that the two gCosts can be compared
                        tempSquare = square
                        tempSquare.ChangePreviousSquare(currentSquare)
                        tempSquare.ChangeGCost(distanceFromStart)

                        If square.GetGCost > tempSquare.GetGCost Then 'sees whether the new gCost is lower than the current one, if it is lower than it is made to be the gCost of the square by making the square the tempSquare
                            square = tempSquare
                        End If

                    End If

                Next

                queueOfSquaresToVisit.BubbleSortQueue(Queue.sortByCost.gCost) 'a bubble sort is run on the queue based on the gCost
                DijkstrasAlgorithm(distanceFromStart) 'recursion occurs

            End If

        End If
    End Sub

    Public Function StartMyAlgorithm() As List(Of Square) 'a function that returns the shortest path from the start node/square to the end node/square which has been calculated using My Algorithm
        queueOfSquaresToVisit.Enqueue(startSqu) 'adds the start square to the queue
        startSqu.Discover() 'makes the start square discovered
        MyAlgorithm(startSqu) 'starts My Algorithm on the maze
        Return GetPath(maze.GetEndSquare) 'returns the path from the start square to the end square
    End Function

    Public Sub MyAlgorithm(ByVal previousSquare As Square) 'extension, subroutine runs My Algorithm on the maze
        Dim currentSquare As Square
        Dim neighbours As List(Of Square)

        If queueOfSquaresToVisit.IsEmpty = False And maze.IsEndFound = False Then
            currentSquare = queueOfSquaresToVisit.Dequeue() 'gets the square at the front of the priority queue
            currentSquare.ChangePreviousSquare(previousSquare) 'makes the current square of the neighbour square the previous/parent square
            maze.IncreaseNumberOfSquaresTraversed() 'increments the counter that keeps track of the numberOfSquaresTraversed

            If currentSquare.IsEndSquare = True Then
                maze.ChangeEndFoundStatus(True) 'the endFound variable is made to be TRUE in the Maze class, to indicate that end square has been found
            Else

                neighbours = maze.GetNeighbours(currentSquare) 'gets a list of the current square's neighbouring/adjacent squares


                For Each square In neighbours 'every square (item) in the neighbours list, has the code in the FOR loop run on it
                    If square.GetDiscoveredStatus = False Then 'if GetDiscoveredStatus = True, it means the square is in queueofSquaresToVisit, make more clear !!! ???
                        queueOfSquaresToVisit.Enqueue(square) 'adds the neighbouring/adjacent square of current square being traversed to the queue
                        square.Discover() 'marks the neighbouring/adjacent square as discovered, so that they cannot be added to the queue again
                        MyAlgorithm(currentSquare) 'recursion occurs
                    End If
                Next


            End If

        End If
    End Sub

    Public Sub ResetInformation() 'empties the queue as some elements may still be in it
        While queueOfSquaresToVisit.IsEmpty = False 'until the queue is empty
            queueOfSquaresToVisit.Dequeue() 'every element is dequeued (removed)
        End While
    End Sub

    Function GetPath(ByVal lastSquare As Square) As List(Of Square) 'returns the path from the start square to the end square (including them) found by the pathfinding algorithm
        Dim path As New List(Of Square)
        Dim pathStack As New StackOfSquares(900) 'I chose to use a stack as it is first in last out so the start square which will be the last on, will be the first off. The maximum possible size is 900 (as the max size of a maze is 30 * 30 (traversable squares) = 900)

        Do
            pathStack.Push(lastSquare) 'the lastSquare is added to the stack
            lastSquare = lastSquare.GetPreviousSquare 'the previous/parent square of the lastSquare is retrieved and made the lastSquare
        Loop Until lastSquare.GetCoordinatesActual.xValue = startSqu.GetCoordinatesActual.xValue And lastSquare.GetCoordinatesActual.yValue = startSqu.GetCoordinatesActual.yValue 'loops until the start square is reached
        pathStack.Push(lastSquare) 'the start square is also added to the stack

        While pathStack.IsEmpty = False 'until the stack is empty
            path.Add(pathStack.Pop()) 'each element in the stack is popped off (removed from the top of the stack) and added to the path list
        End While

        Return path 'the path (list) is returned
    End Function

End Class
