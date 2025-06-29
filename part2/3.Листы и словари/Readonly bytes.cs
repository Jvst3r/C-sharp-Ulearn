using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace hashes
{
    // TODO: Создайте класс ReadonlyBytes
    public class ReadonlyBytes : IEnumerable<byte>
    {
        private readonly byte[] bytes;
        private int? hash;

        internal int Length => bytes.Length;

        public override string ToString()
        {
            if (bytes == null) return "[]";
            return "[" + string.Join(", ", bytes) + "]";
        }

        public ReadonlyBytes(params byte[] input)
        {
            //если на входе не null
            if (input is null)
                throw new System.ArgumentNullException(nameof(input));

            this.bytes = input;
        }

        public byte this[int index] => bytes[index]; //индексатор

        public override bool Equals(object input)
        {
            if ((input is null) || (this.GetType() != input.GetType()))
                return false;

            var secondBytes = (ReadonlyBytes)input;
            if (secondBytes == null ||
                this.Length != secondBytes.Length) return false;

            for (int i = 0; i < this.Length; i++) //поэлементно сравниваем
                if (this[i] != secondBytes[i]) return false;

            return true;
        }

        public override int GetHashCode()
        {
            if (bytes == null) return 0;
            if (!hash.HasValue) //если нет хеша
            {
                var newHash = 0;
                int fnv = 16777619;
                foreach (var b in this)
                    unchecked
                    {
                        newHash ^= b;
                        newHash *= fnv;
                    }

                hash = new int?(newHash);
            }
            return hash.Value;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            for (int i = 0; i < this.Length; i++)
                yield return bytes[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}