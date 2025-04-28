namespace EducativeIo.Graph
{
    public class LinkedList
    {
        public class Node
        {
            internal int m_data; //Value to store (could be int,string,object etc)
            internal Node m_nextElement; //Pointer to next element
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
            public Node()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
            {
                //Constructor to initialize nextElement of newlyCreated Node
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                m_nextElement = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            }
        };

        private Node m_head;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public LinkedList()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            m_head = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }

        public Node GetHead()
        {
            return m_head;
        }

        private bool IsEmpty()
        {
            if (m_head == null) //Check whether the head points to null
                return true;
            else
                return false;
        }

        public bool PrintList()
        {
            if (IsEmpty())
            {
                Console.Write("List is Empty!");
                return false;
            }

            Node temp = m_head;
            Console.Write("List : ");

            while (temp != null)
            {
                Console.Write(temp.m_data + "->");
                temp = temp.m_nextElement;
            }

            Console.WriteLine("null ");
            return true;
        }

        public void InsertAtHead(int value)
        {
            Node newNode = new Node();
            newNode.m_data = value;
            newNode.m_nextElement = m_head; //Linking newNode to head's nextNode
            m_head = newNode;
        }

        public string Elements()
        {
            // this function will return all values of linked List
            string elementsList = "";
            Node start = m_head;
            Node check = m_head;

            elementsList += start.m_data.ToString();
            elementsList += "->";
            start = start.m_nextElement;

            while (start != null && start.m_data != check.m_data)
            {
                elementsList += start.m_data.ToString();
                elementsList += "->";
                start = start.m_nextElement;
            }

            if (start == null)
                return elementsList + "null";

            elementsList += check.m_data.ToString();
            return elementsList;
        }

        public void InsertAtTail(int value)
        {
            if (IsEmpty())
            {
                // inserting first element in list
                InsertAtHead(value); // head will point to first element of the list
            }
            else
            {
                Node newNode = new Node(); // creating new node
                Node last = m_head; // last pointing at the head of the list

                while (last.m_nextElement != null)
                {
                    // traversing through the list
                    last = last.m_nextElement;
                }

                newNode.m_data = value;
                Console.Write(value + " Inserted!    ");
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                newNode.m_nextElement = null; // point last's nextElement to nullptr
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                last.m_nextElement = newNode; // adding the newNode at the end of the list
            }
        }

        // function to check if element exists in the list
        public bool Search(int value)
        {
            Node temp = m_head; // pointing temp to the head

            while (temp != null)
            {
                if (temp.m_data == value)
                {
                    // if passed value matches with temp's data
                    return true;
                }

                temp = temp.m_nextElement; // pointig to temp's nextElement
            }

            return false; // if not found
        }

        public bool Delete(int value)
        {
            bool deleted = false; //returns true if the node is deleted, false otherwise

            if (IsEmpty())
            {
                //check if the list is empty
                Console.WriteLine("List is Empty");
                return deleted; //deleted will be false
            }

            //if list is not empty, start traversing it from the head
            Node currentNode = m_head; //currentNode to traverse the list
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Node previousNode = null; //previousNode starts from null
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (currentNode.m_data == value)
            {
                // deleting value of head
                deleted = DeleteAtHead();
                Console.WriteLine(value + " deleted!");
                deleted = true; // true because value found and deleted
                return deleted; //returns true if the node is deleted
            }

            previousNode = currentNode;
            currentNode = currentNode.m_nextElement; // pointing current to current's nextElement
            //Traversing/Searching for Node to Delete
            while (currentNode != null)
            {
                //Node to delete is found
                if (value == currentNode.m_data)
                {
                    // pointing previousNode's nextElement to currentNode's nextElement
                    previousNode.m_nextElement = currentNode.m_nextElement;
                    // delete currentNode;
                    currentNode = previousNode.m_nextElement;
                    deleted = true;
                    break;
                }

                previousNode = currentNode;
                currentNode = currentNode.m_nextElement; // pointing current to current's nextElement
            }

            //deleted is true only when value is found and deleted
            if (deleted == false)
            {
                Console.WriteLine(value + " does not exist!");
            }
            else
            {
                Console.WriteLine(value + " deleted!");
            }

            return deleted;
        } //end of delete()

        private bool DeleteAtHead()
        {
            if (IsEmpty())
            {
                // check if list is empty
                Console.WriteLine("List is Empty");
                return false;
            }

            m_head = m_head.m_nextElement; //nextNode points to head's nextElement

            return true;
        }

        public int Length()
        {
            Node current = m_head; // Start from the first element
            int count = 0; // in start count is 0

            while (current != null)
            {
                // Traverse the list and count the number of nodes
                count++; // increment everytime as element is found
                current = current.m_nextElement; // pointing to current's nextElement
            }

            return count;
        }

        public string Reverse()
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Node previous = null; //To keep track of the previous element, will be used in swapping links
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            Node current = m_head; //firstElement
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Node next = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            //While Traversing the list, swap links
            while (current != null)
            {
                next = current.m_nextElement;
#pragma warning disable CS8601 // Possible null reference assignment.
                current.m_nextElement = previous;
#pragma warning restore CS8601 // Possible null reference assignment.
                previous = current;
                current = next;
            }

#pragma warning disable CS8601 // Possible null reference assignment.
            m_head = previous; // pointing head to start of the list
#pragma warning restore CS8601 // Possible null reference assignment.
            return Elements(); // calling elements to return a string of values in list
        }

        public bool DetectLoop()
        {
            Node slow = m_head,
                fast = m_head; //starting from head of the list

            while (slow != null && fast != null && fast.m_nextElement != null) //checking if all elements exist
            {
                slow = slow.m_nextElement;
                fast = fast.m_nextElement.m_nextElement;

                /* If slow and fast meet at some point then there
                    is a loop */
                if (slow == fast)
                {
                    // Return 1 to indicate that loop is found */
                    return true;
                }
            }

            // Return 0 to indicate that ther is no loop*/
            return false;
        }

        public void InsertLoop()
        {
            Node temp = m_head;
            // traversing to get to last element of the list
            while (temp.m_nextElement != null)
            {
                temp = temp.m_nextElement;
            }

            temp.m_nextElement = m_head; // pointing last element to head of the list
        }

        public int FindMid()
        {
            //list is empty
            if (IsEmpty())
                return 0;

            //currentNode starts at the head
            Node currentNode = m_head;

            if (currentNode.m_nextElement == null)
            {
                //Only 1 element exist in array so return its value.
                return currentNode.m_data;
            }

            Node midNode = currentNode; //midNode starts at head
            currentNode = currentNode.m_nextElement.m_nextElement; //currentNode moves two steps forward

            //Move midNode (Slower) one step at a time
            //Move currentNode (Faster) two steps at a time
            //When currentNode reaches at end, midNode will be at the middle of List
            while (currentNode != null)
            {
                // traversing from head to end

                midNode = midNode.m_nextElement;

                currentNode = currentNode.m_nextElement; // pointing to current's next
                if (currentNode != null)
                    currentNode = currentNode.m_nextElement; // pointing to current's next
            }

            if (midNode != null) // pointing at middle of the list
                return midNode.m_data;
            return 0;
        }

        public string RemoveDuplicates()
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Node start,
                startNext = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            start = m_head;

            /* Pick elements one by one */
            while (start != null && start.m_nextElement != null)
            {
                startNext = start;

                /* Compare the picked element with rest
                   of the elements */
                while (startNext.m_nextElement != null)
                {
                    /* If duplicate then delete it */
                    if (start.m_data == startNext.m_nextElement.m_data)
                    {
                        // skipping elements from the list to be deleted
                        startNext.m_nextElement = startNext.m_nextElement.m_nextElement;
                    }
                    else
                        startNext = startNext.m_nextElement; // pointing to next of startNext
                }

                start = start.m_nextElement;
            }

            return Elements();
        }

        public string Union(LinkedList list1, LinkedList list2)
        {
            //Return other List if one of them is empty
            if (list1.IsEmpty())
                return list2.Elements();
            else if (list2.IsEmpty())
                return list1.Elements();

            Node start = list1.m_head; // starting from head of list 1

            //Traverse first list till the last element
            while (start.m_nextElement != null)
            {
                start = start.m_nextElement;
            }

            //Link last element of first list to the first element of second list
            start.m_nextElement = list2.m_head; // appendinf list2 with list 1
            return list1.RemoveDuplicates(); // removing duplicates from list and return list
        }

        //To Find nth node from end of list
        public int FindNth(int n)
        {
            if (IsEmpty()) // if list is empty return -1
                return -1;

            int length = 0;
            Node currentNode = m_head; // pointing to head of the list

            // finding the length of the list
            while (currentNode != null)
            {
                currentNode = currentNode.m_nextElement;
                length++;
            }

            //Find the Node which is at (len - n) position from start
            currentNode = m_head;
            int position = length - n;

            if (position < 0 || position > length) // check if out of the range of the list
                return -1;

            int count = 0;
            // traversing till the nth Element of the list
            while (count != position)
            {
                // finding the position of the element
                currentNode = currentNode.m_nextElement;
                count++;
            }

            if (currentNode != null) // if node exists
                return currentNode.m_data;

            return -1;
        }
    }
}