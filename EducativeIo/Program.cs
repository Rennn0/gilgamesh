using chapter_5;
using EducativeIo.Chapter4;
// Console.WriteLine("Hello, World!");
// Challenge7.nextGreaterElementStack([4, 6, 3, 2, 8, 1, 9, 9], 8);
// Challenge8.isBalanced("{[()]}");

Graph g = new Graph(5);
g.addEdge(0, 1);
g.addEdge(0, 2);
g.addEdge(0, 4);
g.addEdge(2, 3);

g.printGraph();
