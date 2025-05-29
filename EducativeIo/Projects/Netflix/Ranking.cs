// namespace EducativeIo.Projects.Netflix;

// public partial class Netflix
// {
//     protected virtual LinkedListNode Merge(LinkedListNode? l1, LinkedListNode? l2)
//     {
//         LinkedListNode temp = new LinkedListNode(-1);
//         LinkedListNode crawler = temp;

//         while (l1 != null && l2 != null)
//         {
//             if (l1.data < l2.data)
//             {
//                 crawler.next = l1;
//                 l1 = l1.next;
//             }
//             else
//             {
//                 crawler.next = l2;
//                 l2 = l2.next;
//             }
//             crawler = crawler.next;
//         }

//         crawler.next = l1 ?? l2;

//         return temp.next;
//     }

//     public virtual LinkedListNode Merge(List<LinkedListNode> list)
//     {
//         if (list.Count <= 0)
//             return new LinkedListNode(-1);

//         LinkedListNode node = list[0];
//         for (int i = 1; i < list.Count; i++)
//         {
//             node = Merge(node, list[i]);
//         }

//         return node;
//     }
// }

// public class LinkedListNode
// {
//     public int key;
//     public int data;
//     public LinkedListNode next;
//     public LinkedListNode arbitraryPointer;

//     public LinkedListNode(int data)
//     {
//         this.data = data;
//         this.next = null;
//     }

//     public LinkedListNode(int key, int data)
//     {
//         this.key = key;
//         this.data = data;
//         this.next = null;
//     }

//     public LinkedListNode(int data, LinkedListNode next)
//     {
//         this.data = data;
//         this.next = next;
//     }

//     public LinkedListNode(int data, LinkedListNode next, LinkedListNode arbitraryPointer)
//     {
//         this.data = data;
//         this.next = next;
//         this.arbitraryPointer = arbitraryPointer;
//     }
// }

// public class LinkedList
// {
//     public static LinkedListNode insertAtHead(LinkedListNode head, int data)
//     {
//         LinkedListNode newNode = new LinkedListNode(data);
//         newNode.next = head;
//         return newNode;
//     }

//     public static LinkedListNode insertAtTail(LinkedListNode head, int data)
//     {
//         LinkedListNode newNode = new LinkedListNode(data);
//         if (head == null)
//         {
//             return newNode;
//         }
//         LinkedListNode temp = head;
//         while (temp.next != null)
//         {
//             temp = temp.next;
//         }
//         temp.next = newNode;
//         return head;
//     }

//     public static LinkedListNode insertAtTail(LinkedListNode head, LinkedListNode node)
//     {
//         if (head == null)
//         {
//             return node;
//         }
//         LinkedListNode temp = head;
//         while (temp.next != null)
//         {
//             temp = temp.next;
//         }
//         temp.next = node;
//         return head;
//     }

//     public static LinkedListNode createLinkedList(List<int> lst)
//     {
//         LinkedListNode head = null;
//         LinkedListNode tail = null;
//         foreach (int x in lst)
//         {
//             LinkedListNode newNode = new LinkedListNode(x);
//             if (head == null)
//             {
//                 head = newNode;
//             }
//             else
//             {
//                 tail.next = newNode;
//             }
//             tail = newNode;
//         }
//         return head;
//     }

//     public static LinkedListNode createLinkedList(int[] arr)
//     {
//         LinkedListNode head = null;
//         LinkedListNode tail = null;
//         for (int i = 0; i < arr.Length; ++i)
//         {
//             LinkedListNode newNode = new LinkedListNode(arr[i]);
//             if (head == null)
//             {
//                 head = newNode;
//             }
//             else
//             {
//                 tail.next = newNode;
//             }
//             tail = newNode;
//         }
//         return head;
//     }

//     public static LinkedListNode createRandomList(int length)
//     {
//         LinkedListNode listHead = null;
//         Random generator = new Random();
//         for (int i = 0; i < length; ++i)
//         {
//             listHead = insertAtHead(listHead, generator.Next(100));
//         }
//         return listHead;
//     }

//     public static List<int> toList(LinkedListNode head)
//     {
//         List<int> lst = new List<int>();
//         LinkedListNode temp = head;
//         while (temp != null)
//         {
//             lst.Add(temp.data);
//             temp = temp.next;
//         }
//         return lst;
//     }

//     public static void display(LinkedListNode head)
//     {
//         LinkedListNode temp = head;
//         while (temp != null)
//         {
//             Console.Write("{0}", temp.data);
//             temp = temp.next;
//             if (temp != null)
//             {
//                 Console.Write(", ");
//             }
//         }
//         Console.WriteLine("");
//     }

//     public static LinkedListNode mergeAlternating(LinkedListNode list1, LinkedListNode list2)
//     {
//         if (list1 == null)
//         {
//             return list2;
//         }

//         if (list2 == null)
//         {
//             return list1;
//         }

//         LinkedListNode head = list1;

//         while (list1.next != null && list2 != null)
//         {
//             LinkedListNode temp = list2;
//             list2 = list2.next;

//             temp.next = list1.next;
//             list1.next = temp;
//             list1 = temp.next;
//         }

//         if (list1.next == null)
//         {
//             list1.next = list2;
//         }

//         return head;
//     }

//     static bool isEqual(LinkedListNode list1, LinkedListNode list2)
//     {
//         if (list1 == list2)
//         {
//             return true;
//         }

//         while (list1 != null && list2 != null)
//         {
//             if (list1.data != list2.data)
//             {
//                 return false;
//             }

//             list1 = list1.next;
//             list2 = list2.next;
//         }

//         return (list1 == list2);
//     }
// }
