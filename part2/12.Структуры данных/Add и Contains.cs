using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryTrees
{
    internal class TreeNode<T> where T : IComparable
    {
        public T Value;
        public TreeNode<T> Left;
        public TreeNode<T> Right;

        public TreeNode(T value) => this.Value = value;
    }

    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        internal TreeNode<T> Root;

        //реализация всего остального IEnum, индексатора ниже
        //Как оказалось сайту пофигу на индексатор (ну и ладно)
        public int Count { get; private set; }

        public void Add(T value)
        {
            var parent = FindPlaceToAdd(value); // Находим родителя
            var newNode = new TreeNode<T>(value); // Создаём новый узел

            // Связываем новый узел с родителем
            if (parent == null)
                Root = newNode; // Дерево было пустым
            else
            {
                // влево или вправо
                if (value.CompareTo(parent.Value) < 0)
                    parent.Left = newNode;
                else
                    parent.Right = newNode;
            }
        }

        private TreeNode<T> FindPlaceToAdd(T value)
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
            return false;// если элемент не найден
        }


        //индексатор
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException();

                return InOrderTraversal(Root).ElementAt(index);
            }
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
