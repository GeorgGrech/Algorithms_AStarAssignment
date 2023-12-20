using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst() //Remove top item and return it
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]); //Reorganize heap
        return firstItem;
    }

    public void UpdateItem(T item) //Updating can only decrease costs, so attempt SortUp
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public bool Contains(T item) //check if item is in heap
    {
        return Equals(items[item.HeapIndex], item);
    }

    void SortDown(T item) //Move item down until in correct position
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2+1;
            int childIndexRight = item.HeapIndex * 2+2;

            int swapIndex = 0;

            if (childIndexLeft < currentItemCount) //Has at least one child
            {
                swapIndex = childIndexLeft; //Use left by default

                if (childIndexRight < currentItemCount) //Has second child
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) //if right has higher priority, use instead
                    {
                        swapIndex = childIndexRight;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0) //if parent has lower priority then highest child, swap
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    void SortUp(T item) //Move item up until in correct position
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem); //Swap node with parent
            }
            else
            {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA,T itemB)
    {
        items[itemA.HeapIndex] = itemB; //Swap positions in heap
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex; //Set proper indexes
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
