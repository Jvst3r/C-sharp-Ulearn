using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BinaryTrees
{
    internal class TreeNode<T> where T : IComparable
    {
        public T Value;
        public TreeNode<T> Left;
        public TreeNode<T> Right;
        public int Size;

        public TreeNode(T value)
        {
            Value = value;
            Size = 1; // Начальный размер поддерева=1
        }
    }

    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        internal TreeNode<T> Root;

        //реализация всего остального IEnum, индексатора ниже
        //Как оказалось сайту пофигу на индексатор (ну и ладно)
        //Решил слишком много интеграллов, индексатор - следующее задание)))
        public int Count { get; private set; }

        public void Add(T value)
        {
            Root = AddRecursive(Root, value);
            Count++;
        }

        private TreeNode<T> AddRecursive(TreeNode<T> node, T value)
        {
            if (node == null)
                return new TreeNode<T>(value);

            int cmp = value.CompareTo(node.Value);
            if (cmp < 0)
                node.Left = AddRecursive(node.Left, value);
            else
                node.Right = AddRecursive(node.Right, value);

            // Обновляем размер поддерева
            node.Size = 1 + GetSize(node.Left) + GetSize(node.Right);
            return node;
        }

        private TreeNode<T> FindPlaceToAdd(TreeNode<T> node, T value)
        {
            TreeNode<T> parent = null;
            TreeNode<T> current = Root;

            while (current != null)
            {
                parent = current;
                // Направление влево или вправо
                current = (value.CompareTo(current.Value) < 0) ? current.Left : current.Right;
            }
            return parent;
        }

        public bool Contains(T value)
        {
            var root = Root;
            while (root != null)
            {
                if (value.Equals(root.Value))
                    return true;

                root = (value.CompareTo(root.Value) > 0) ? root.Right : root.Left;
            }
            return false; // если элемент не найден
        }

        private int GetSize(TreeNode<T> node)
        {
            if (node == null)
                return 0;
            else
                return node.Size;
        }

        //индексатор
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException();

                return GetValueAtIndex(Root, index);
            }
        }

        private T GetValueAtIndex(TreeNode<T> node, int index)
        {
            if (node == null)
                throw new IndexOutOfRangeException();
            int leftSize = GetSize(node.Left);

            if (index < leftSize)
                return GetValueAtIndex(node.Left, index);
            else if (index == leftSize)
                return node.Value;
            else
                return GetValueAtIndex(node.Right, index - leftSize - 1);
        }

        // Реализация IEnumerable<T>
        public IEnumerator<T> GetEnumerator() => InOrderTraversal(Root).GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<T> InOrderTraversal(TreeNode<T> node)
        {
            if (node == null) yield break;

            foreach (var left in InOrderTraversal(node.Left))
                yield return left;

            yield return node.Value;

            foreach (var right in InOrderTraversal(node.Right))
                yield return right;
        }
    }
}