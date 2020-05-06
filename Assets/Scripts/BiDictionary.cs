using System;
using System.Collections;
using System.Collections.Generic;

public class BiDictionary<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>
{
    public BiDictionary()
    {
        Forward = new Dictionary<T1, T2>();
        Reverse = new Dictionary<T2, T1>();
    }

    public Dictionary<T1, T2> Forward { get; private set; }
    public Dictionary<T2, T1> Reverse { get; private set; }

    public void Add(T1 t1, T2 t2)
    {
        Forward.Add(t1, t2);
        Reverse.Add(t2, t1);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
    {
        return Forward.GetEnumerator();
    }
}