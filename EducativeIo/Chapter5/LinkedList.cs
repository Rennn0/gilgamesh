using System;

namespace chapter_5
{
    public class LinkedList
    {
        public class Node
        {
            internal int data; //Data to store (could be int,string,object etc)
            internal Node nextElement; //Pointer to next element

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
            public Node()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
            {
                //Constructor to initialize nextElement of newlyCreated Node
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                nextElement = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            }
        };
        Node head;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public LinkedList()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            head = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
        public Node GetHead()
        {
            return head;
        }
        bool IsEmpty()
        {
            if (head == null) //Check whether the head points to null
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
            Node temp = head;
            Console.Write("List : ");

            while (temp != null)
            {
                Console.Write(temp.data + "->");
                temp = temp.nextElement;
            }
            Console.WriteLine("null ");
            return true;
        }
        public void InsertAtHead(int value)
        {
            Node newNode = new Node();
            newNode.data = value;
            newNode.nextElement = head; //Linking newNode to head's nextNode
            head = newNode;

        }
        public string Elements()
        { // this function will return all values of linked List
            string elementsList = "";
            Node start = head;
            Node check = head;

            elementsList += start.data.ToString();
            elementsList += "->";
            start = start.nextElement;

            while (start != null && start.data != check.data)
            {
                elementsList += start.data.ToString();
                elementsList += "->";
                start = start.nextElement;
            }

            if (start == null)
                return elementsList + "null";

            elementsList += check.data.ToString();
            return elementsList;
        }
        public void InsertAtTail(int value)
        {
            if (IsEmpty())
            { // inserting first element in list
                InsertAtHead(value); // head will point to first element of the list      
            }
            else
            {
                Node newNode = new Node(); // creating new node
                Node last = head; // last pointing at the head of the list

                while (last.nextElement != null)
                { // traversing through the list
                    last = last.nextElement;
                }

                newNode.data = value;
                Console.Write(value + " Inserted!    ");
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                newNode.nextElement = null; // point last's nextElement to nullptr
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                last.nextElement = newNode; // adding the newNode at the end of the list
            }
        }

        // function to check if element exists in the list
        public bool Search(int value)
        {
            Node temp = head; // pointing temp to the head

            while (temp != null)
            {
                if (temp.data == value)
                { // if passed value matches with temp's data
                    return true;
                }
                temp = temp.nextElement; // pointig to temp's nextElement
            }
            return false; // if not found
        }

        public bool Delete(int value)
        {
            bool deleted = false; //returns true if the node is deleted, false otherwise

            if (IsEmpty())
            { //check if the list is empty
                Console.WriteLine("List is Empty");
                return deleted; //deleted will be false
            }

            //if list is not empty, start traversing it from the head
            Node currentNode = head; //currentNode to traverse the list
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Node previousNode = null; //previousNode starts from null
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (currentNode.data == value)
            { // deleting value of head
                deleted = DeleteAtHead();
                Console.WriteLine(value + " deleted!");
                deleted = true; // true because value found and deleted
                return deleted; //returns true if the node is deleted
            }
            previousNode = currentNode;
            currentNode = currentNode.nextElement; // pointing current to current's nextElement
                                                   //Traversing/Searching for Node to Delete
            while (currentNode != null)
            {

                //Node to delete is found
                if (value == currentNode.data)
                {
                    // pointing previousNode's nextElement to currentNode's nextElement
                    previousNode.nextElement = currentNode.nextElement;
                    // delete currentNode;
                    currentNode = previousNode.nextElement;
                    deleted = true;
                    break;
                }
                previousNode = currentNode;
                currentNode = currentNode.nextElement; // pointing current to current's nextElement
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
        bool DeleteAtHead()
        {
            if (IsEmpty())
            { // check if list is empty
                Console.WriteLine("List is Empty");
                return false;
            }

            head = head.nextElement; //nextNode points to head's nextElement


            return true;
        }

        public int Length()
        {
            Node current = head; // Start from the first element
            int count = 0; // in start count is 0

            while (current != null)
            { // Traverse the list and count the number of nodes
                count++; // increment everytime as element is found
                current = current.nextElement; // pointing to current's nextElement
            }
            return count;
        }


        public string Reverse()
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Node previous = null; //To keep track of the previous element, will be used in swapping links
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            Node current = head; //firstElement
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Node next = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            //While Traversing the list, swap links
            while (current != null)
            {
                next = current.nextElement;
#pragma warning disable CS8601 // Possible null reference assignment.
                current.nextElement = previous;
#pragma warning restore CS8601 // Possible null reference assignment.
                previous = current;
                current = next;
            }

#pragma warning disable CS8601 // Possible null reference assignment.
            head = previous; // pointing head to start of the list
#pragma warning restore CS8601 // Possible null reference assignment.
            return Elements(); // calling elements to return a string of values in list
        }

        public bool DetectLoop()
        {
            Node slow = head, fast = head; //starting from head of the list

            while ((slow != null) && (fast != null) && (fast.nextElement != null)) //checking if all elements exist 
            {
                slow = slow.nextElement;
                fast = fast.nextElement.nextElement;

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
            Node temp = head;
            // traversing to get to last element of the list
            while (temp.nextElement != null)
            {
                temp = temp.nextElement;
            }
            temp.nextElement = head; // pointing last element to head of the list
        }

        public int FindMid()
        {
            //list is empty
            if (IsEmpty())
                return 0;

            //currentNode starts at the head
            Node currentNode = head;

            if (currentNode.nextElement == null)
            {
                //Only 1 element exist in array so return its value.
                return currentNode.data;
            }

            Node midNode = currentNode; //midNode starts at head
            currentNode = currentNode.nextElement.nextElement; //currentNode moves two steps forward

            //Move midNode (Slower) one step at a time
            //Move currentNode (Faster) two steps at a time
            //When currentNode reaches at end, midNode will be at the middle of List
            while (currentNode != null)
            {       // traversing from head to end

                midNode = midNode.nextElement;

                currentNode = currentNode.nextElement;     // pointing to current's next
                if (currentNode != null)
                    currentNode = currentNode.nextElement;     // pointing to current's next

            }
            if (midNode != null)     // pointing at middle of the list
                return midNode.data;
            return 0;
        }

        public string RemoveDuplicates()
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Node start, startNext = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            start = head;

            /* Pick elements one by one */
            while ((start != null) && (start.nextElement != null))
            {
                startNext = start;

                /* Compare the picked element with rest 
                   of the elements */
                while (startNext.nextElement != null)
                {
                    /* If duplicate then delete it */
                    if (start.data == startNext.nextElement.data)
                    {

                        // skipping elements from the list to be deleted
                        startNext.nextElement = startNext.nextElement.nextElement;

                    }
                    else
                        startNext = startNext.nextElement; // pointing to next of startNext
                }
                start = start.nextElement;
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

            Node start = list1.head; // starting from head of list 1

            //Traverse first list till the last element
            while (start.nextElement != null)
            {
                start = start.nextElement;
            }

            //Link last element of first list to the first element of second list
            start.nextElement = list2.head; // appendinf list2 with list 1
            return list1.RemoveDuplicates(); // removing duplicates from list and return list
        }


        //To Find nth node from end of list
        public int FindNth(int n)
        {

            if (IsEmpty()) // if list is empty return -1
                return -1;

            int length = 0;
            Node currentNode = head; // pointing to head of the list

            // finding the length of the list
            while (currentNode != null)
            {
                currentNode = currentNode.nextElement;
                length++;
            }

            //Find the Node which is at (len - n) position from start
            currentNode = head;
            int position = length - n;

            if (position < 0 || position > length) // check if out of the range of the list
                return -1;

            int count = 0;
            // traversing till the nth Element of the list
            while (count != position)
            { // finding the position of the element
                currentNode = currentNode.nextElement;
                count++;
            }

            if (currentNode != null) // if node exists
                return currentNode.data;

            return -1;

        }
    }

}
