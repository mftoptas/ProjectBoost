using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    [SerializeField] float movementFactor; // ranges variable on inspector between 0 and 1.
    [SerializeField] float period = 5f;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position; // gives current position.
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) // The smallest float is Mathf.Epsilon
        {
            return; // prevents period = 0 so cycles will not be divide to 0 and i will not get NaN error
        }
        float cycles = Time.time / period; // continually growing over time
        const float tau = Mathf.PI * 2; // constant value of 6.283
        float rawSinWawe = Mathf.Sin(cycles * tau); // going from -1 to 1

        movementFactor = (rawSinWawe + 1) / 2; // recalculated to go from 0 to 1 so its cleaner

        Vector3 offset = movementVector * movementFactor; // calculated the offset.
        transform.position = startingPosition + offset; // change current position by adding offset.
    }
}
