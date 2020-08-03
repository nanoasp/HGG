using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.reddit.com/r/gamedev/comments/31vwyg/game_math_precise_control_over_numeric_springing/
/*
  x     - value             (input/output)
  v     - velocity          (input/output)
  xt    - target value      (input)
  zeta  - damping ratio     (input)
  omega - angular frequency (input)
  h     - time step         (input)
*/
public class NumbericSpring
{
    static public void spring(ref float x, ref float v, float xt, float zeta, float omega, float h)
    {
        float f = 1.0f + 2.0f * h * zeta * omega;
        float oo = omega * omega;
        float hoo = h * oo;
        float hhoo = h * hoo;
        float detInv = 1.0f / (f + hhoo);
        float detX = f * x + h * v + hhoo * xt;
        float detV = v + hoo * (xt - x);
        x = detX * detInv;
        v = detV * detInv;
    }
};
