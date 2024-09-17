using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericHorizontalEntityBehaviour : GenericEntityBehaviour
{
    // These constants define an entities drag, acceleration and maximum speed from it's own acceleration in the horizontal axis
    public float HorizontalDrag;
    public float HorizontalAccelerationPower;
    public float MaximumHorizontalSpeedFromPower;

    // This variable contains the direction of the acceleration from movement horizontally
    [HideInInspector]
    public float horizontalAccelerationDirection;
}
