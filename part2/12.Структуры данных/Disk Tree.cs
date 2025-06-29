using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskTree
{
    public class DirectoryNode
    {
        public string Name { get; }


        //обожаю комментарии на юлерне,StringComparer имба
        public SortedDictionary<string, DirectoryNode> Children { get; }
           = new SortedDictionary<string, DirectoryNode>(StringComparer.Ordinal);

        public DirectoryNode(string name)
        {
            Name = name;
        }

        public DirectoryNode GetOrAddChild(string name)
        {
            if (!Children.TryGetValue(name, out var child))
            {
                child = new DirectoryNode(name);
                Children[name] = child;
            }
            return child;
        }
    }

    public static class DiskTreeTask
    {
        public static List<string> Solve(List<string> input)
        {
            var root = new DirectoryNode("");
            foreach (var path in input)
            {
                var parts = path.Split('\\');
                var currentNode = root;
                foreach (var part in parts)
                {
                    currentNode = currentNode.GetOrAddChild(part);
                }
            }

            var result = new List<string>();
            RecursiveTreeTraversal(root, -1, result);
            return result;
        }

        private static void RecursiveTreeTraversal(DirectoryNode node, int level, List<string> result)
        {
            if (level >= 0)
            {
                result.Add(new string(' ', level) + node.Name);
            }

            foreach (var child in node.Children.Values)
            {
                RecursiveTreeTraversal(child, level + 1, result);
            }
        }
    }
}