public class CustomQueue<T>
{
    T[] queueArray = { };

    public int Size
    {
        get
        {
            return queueArray != null ? queueArray.Length : 0;
        }
    }

    public void Enqueue(T item)
    {
        int queueSize = queueArray.Length + 1;

        T[] newQueueArray = new T[queueSize];
        for (int i = 0; i < queueArray.Length; i++)
        {
            newQueueArray[i] = queueArray[i];
        }
        newQueueArray[queueSize - 1] = item;
        queueArray = newQueueArray;
    }

    public T Dequeue()
    {
        T deQueuedItem = default(T);
        if (queueArray.Length > 0)
        {
            deQueuedItem = queueArray[0];
            T[] newQueueArray;
            if (queueArray.Length > 1)
            {
                newQueueArray = new T[queueArray.Length - 1];
                for (int i = 1, j = 0; i < newQueueArray.Length; i++, j++)
                {
                    newQueueArray[j] = queueArray[i];
                }
                newQueueArray[newQueueArray.Length - 1] = queueArray[queueArray.Length - 1];
            }
            else
            {
                newQueueArray = null;
            }
            queueArray = newQueueArray;
        }
        return deQueuedItem;
    }

    public T Peek()
    {
        T peekedItem = default(T);
        if (queueArray.Length > 0)
        {
            peekedItem = queueArray[0];
        }
        return peekedItem;
    }

    public int IndexOf(T t)
    {
        for (int i = 0; i < queueArray.Length; i++)
        {
            if (queueArray[i].Equals(t))
            {
                return i;
            }
        }
        return -1;
    }

    public T Remove(int index)
    {
        T removedItem = default(T);
        if (queueArray != null && queueArray.Length > 0)
        {
            removedItem = queueArray[index];
            T[] newQueueArray;
            if (queueArray.Length > 1)
            {
                newQueueArray = new T[queueArray.Length - 1];
                int newfsIndex = 0;
                for (int i = 0; i < queueArray.Length; i++)
                {
                    if (i != index)
                    {
                        newQueueArray[newfsIndex] = queueArray[i];
                        newfsIndex++;
                    }
                }
            }
            else
            {
                newQueueArray = null;
            }
            queueArray = newQueueArray;
        }
        return removedItem;
    }
}