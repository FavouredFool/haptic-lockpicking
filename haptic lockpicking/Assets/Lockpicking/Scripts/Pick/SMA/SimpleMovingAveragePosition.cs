using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SimpleMovingAveragePosition
{
    // https://andrewlock.net/creating-a-simple-moving-average-calculator-in-csharp-1-a-simple-moving-average-calculator/

    private readonly int _k;
    private readonly Vector3[] _values;

    private int _index = 0;
    private Vector3 _sum = Vector3.zero;

    public SimpleMovingAveragePosition(int k)
    {
        if (k <= 0) throw new ArgumentOutOfRangeException(nameof(k), "Must be greater than 0");

        _k = k;
        _values = new Vector3[k];
    }

    public void PopulateValues(Vector3 value)
    {
        for (int i = 0; i < _values.Length; i++)
        {
            _values[i] = value;
        }
    }

    public Vector3 Step(Vector3 nextInput)
    {
        // overwrite the old value with the new one
        _values[_index] = nextInput;

        // increment the index (wrapping back to 0)
        _index = (_index + 1) % _k;

        // calculate the average
        return CalculateAveragePosition();
    }

    Vector3 CalculateAveragePosition()
    {
        Vector3 baseVector = Vector3.zero;

        foreach (Vector3 vector in _values)
        {
            baseVector += vector;
        }

        return baseVector / _k;
    }
}


