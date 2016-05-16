using UnityEngine;
using System.Collections;
using System;




public class Heap<T> :IEnumerable where T : IHeap<T>
{
    public T[] elements;
    public int MaxSize { get; private set; }
    public int Count { get; private set; }

    public Heap(int maxSize)
    {
        MaxSize = MaxSize;
        elements = new T[maxSize];
        Count = 0;
    }

    public void Add(T element)
    {
        element.HeapIndex = Count;
        elements[Count] = element;
        SortUp(element);
        Count++;
    }

    public T Remove()
    {

        Count--;
        T retElement = elements[0];
        elements[0] = elements[Count];
        elements[0].HeapIndex = 0;
        SortDown(elements[0]);

        return retElement;
    }
    public void Update(T element)
    {
        SortUp(element);
    }

    public void Clear()
    {
        Count = 0;
    }


    public bool Contains(T element)
    {
        if (element.HeapIndex < Count)
        {
            return element.Equals(elements[element.HeapIndex]);
        }
        return false;
    }


    void SortDown(T element)
    {
        int currentIndex = element.HeapIndex;
        int leftIndex, rightIndex;
        int swapIndex;
        while (true)
        {
            leftIndex = 2 * currentIndex + 1;
            rightIndex = leftIndex + 1;

            if (leftIndex <= Count)
            {
                swapIndex = leftIndex;

                if (rightIndex <= Count)
                {
                    if (elements[rightIndex].CompareTo(elements[leftIndex]) > 0)
                    {
                        swapIndex = rightIndex;
                    }
                }

            }
            else if (rightIndex <= Count)
            {
                swapIndex = rightIndex;
            }
            else
            {
                return;
            }

            if (element.CompareTo(elements[swapIndex]) < 0)
            {
                Swap(element, elements[swapIndex]);
            }
            currentIndex = swapIndex;
        }
    }

    void SortUp(T element)
    {
        int currentIndex = element.HeapIndex;
        int parentIndex;
        while (true)
        {

            parentIndex = currentIndex / 2;
            if (element.CompareTo(elements[parentIndex]) > 0)
            {
                Swap(element, elements[parentIndex]);
            }
            else
            {
                return;
            }

            currentIndex = parentIndex;

        }
    }

    void Swap(T elementA, T elementB)
    {
        int aIndex = elementA.HeapIndex;
        elements[aIndex] = elementB;
        elements[elementB.HeapIndex] = elementA;
        elementA.HeapIndex = elementB.HeapIndex;
        elementB.HeapIndex = aIndex;
    }

   

    public IEnumerator GetEnumerator()
    {
        for(int i = 0; i < Count; i++)
        {
            yield return elements[i];
        }
    }
}

public interface IHeap<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}



