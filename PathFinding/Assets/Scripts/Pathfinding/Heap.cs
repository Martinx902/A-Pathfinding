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
        //We add an item to the list, and we keep track of its position by adding at the end of the list
        item.heapIndex = currentItemCount;

        items[currentItemCount] = item;

        //Once thats added we need to check where the item really belongs on the list, so we sort it up to its legitimate position
        SortUp(item);
       
        currentItemCount++;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count()
    {
        return currentItemCount;
    }

    public bool Contains(T item)
    {
        //If there is an item with the same heap index on the list it will return true, implying it exists on the list
        return Equals(items[item.heapIndex], item);
    }

    public T RemoveFirst()
    {
        //Wrap the first item of the list
        T firstItem = items[0];
        currentItemCount --;
        //Set the last item to be the first one
        items[0] = items[currentItemCount];
        items[0].heapIndex = 0;
        //Sort it down to it's correspondent position on the list
        SortDown(items[0]);
        return firstItem;
    }

    public void SortDown(T item)
    {
        while (true)
        {
            //Get the indexes of the childs of the item to compare
            int childIndexLeft = item.heapIndex * 2 + 1;
            int childIndexRight = item.heapIndex * 2 + 2;
            //Temp swap index variable to store the index of the child to swap
            int swapIndex = 0;

            //First we check if the item has an left child, because if not, the item is the last item on the list, so no childs
            if(childIndexLeft < currentItemCount)
            {
                //Set the swap index to the left child index just to have a parameter to compare to
                swapIndex = childIndexLeft;

                //We check if the item has a right child
                if(childIndexRight < currentItemCount)
                {
                    //If the right children is less than the left one, then that is the one we will swap, if not, we will swap the left one
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                //Now with the lower child number obtained, we compare it to the fathers number, if the father is less, then we swap it, if not
                //it's in the correct position.
                if (item.CompareTo(items[swapIndex]) < 0)
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

    public void SortUp(T item)
    {
        //We get the parents item index so we can identify it numerically
        int parentIndex = (item.heapIndex - 1) / 2;

        while (true)
        {
            //We get the parent item
            T parentItem = items[parentIndex];
            
            //Compare it to the actual item we are adding
            if(item.CompareTo(parentItem) > 0)
            {
                //If its less than the parent, then we swap it 
                Swap(item, parentItem);
            }
            else
            {
                //If not, thats his place
                break;
            }

            //We keep looking until for the next parent
            parentIndex = (item.heapIndex + 1) / 2;

        }
    }

    void Swap(T itemA, T itemB)
    {
        //Swap the items references
        items[itemA.heapIndex] = itemB;
        items[itemB.heapIndex] = itemA;

        //Create a temp variable to store the indexes, and swap them later
        int tempIndex = itemA.heapIndex;

        itemA.heapIndex = itemB.heapIndex;
        itemB.heapIndex = tempIndex;

    }

}

//We implement an interface so that we can compare 2 items of T type, in this case Nodes
public interface IHeapItem<T> : IComparable<T>
{
    //Index value to each node we add to the items list
    int heapIndex
    {
        get;
        set;
    }
}
