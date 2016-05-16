using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    interface IMovementHandler
    {
         void HandleMovement(Transform transform, Vector3 targetPosition);

    }
}
