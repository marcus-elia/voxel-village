using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseGenerator : MonoBehaviour
{
    public static int fixedDistance = 4;
    public static int area = fixedDistance * fixedDistance;
    public static int modulus, a, b, c;

    // Assume x and y are multiples of m
    // Replace x and y with x/fixedDistance and y/fixedDistance
    // Compute ((ax + by + c) % m) / m
    public static float GetFixedValue(int x, int y)
    {
        float value = Mod((a * x / fixedDistance + b * y / fixedDistance + c), modulus);
        
        return value / (float)modulus;
    }

    public static float GetInterpolatedValue(int x, int y)
    {
        // Get the random values of the corners of the square containing (x,y)
        float topLeft = GetFixedValue(NearestMultipleDown(x, fixedDistance), NearestMultipleUp(y, fixedDistance));
        float topRight = GetFixedValue(NearestMultipleUp(x, fixedDistance), NearestMultipleUp(y, fixedDistance));
        float bottomLeft = GetFixedValue(NearestMultipleDown(x, fixedDistance), NearestMultipleDown(y, fixedDistance));
        float bottomRight = GetFixedValue(NearestMultipleUp(x, fixedDistance), NearestMultipleDown(y, fixedDistance));
       
        // Interpolate
        float xScale = Mod(x, fixedDistance) / (float)fixedDistance;
        float yScale = Mod(y, fixedDistance) / (float)fixedDistance;

        float areaTL = xScale * (1 - yScale);
        float areaTR = (1 - xScale) * (1 - yScale);
        float areaBL = xScale * yScale;
        float areaBR = (1 - xScale) * yScale;

        return areaBR * topLeft + areaBL * topRight + areaTR * bottomLeft + areaTL * bottomRight;
    }

    // The primary function
    public static float GetRandomValue(int x, int y)
    {
        if(Mod(x, fixedDistance) == 0 && Mod(y, fixedDistance) == 0)
        {
            return GetFixedValue(x, y);
        }
        else
        {
            return GetInterpolatedValue(x, y);
        }
    }
    // Wrapper
    public static float GetRandomValue(Point2D p)
    {
        return GetRandomValue(p.x, p.z);
    }

    // Helper functions
    public static int NearestMultipleDown(int x, int distance)
    {
        return x - Mod(x, distance);
    }
    public static int NearestMultipleUp(int x, int distance)
    {
        return x - Mod(x, distance) + distance;
    }

    // Setters of static variables
    public static void SetA(int inputA)
    {
        a = inputA;
    }
    public static void SetB(int inputB)
    {
        b = inputB;
    }
    public static void SetC(int inputC)
    {
        c = inputC;
    }
    public static void SetModulus(int inputModulus)
    {
        modulus = inputModulus;
    }

    // Since Mod can return negatives, I think
    public static int Mod(int a, int m)
    {
        return ((a % m) + m) % m;
    }
}
