using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using System.Text.RegularExpressions;
using Sirenix.Utilities;

public static class GameUtilities
{
    public static GameObject ToPrefab(this string p_prefabPath)
    {
        return Resources.Load<GameObject>(p_prefabPath);
    }

    public static string GetPrettyName(this FieldInfo field)
    {
        return Regex.Replace(field.Name.ToCamelCase(), "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
    }

    public static string FormatFullTypeName(this FieldInfo field)
    {
        if (string.IsNullOrEmpty(field.FieldType.FullName)) return string.Empty;
        return field.FieldType.FullName.Replace('+', '.');
    }

    public static IEnumerable<TSource> DistinctBy<TSource, TKey>
        (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = new HashSet<TKey>();
        foreach (TSource element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }

    public static void Push<T>(this IList<T> list, T item)
    {
        if (list == null)
        {
            Debug.LogError("Cannot add items to a null list!");
            return;
        }
        list.Insert(0, item);
    }

    public static T GetOrAddComponent<T>(this Component component) where T : Component
    {
        var c = component.GetComponent<T>();

        if (c == null)
        {
            c = component.gameObject.AddComponent<T>();
        }

        return c;
    }

    /// <summary>
    /// Adds an item to the list only if it has not already been added.
    /// </summary>
    /// <param name="list"></param>
    /// <param name="item"></param>
    /// <typeparam name="T"></typeparam>
    public static void AddDistinct<T>(this IList<T> list, T item)
    {
        if (list == null)
        {
            Debug.LogError("Cannot add items to a null list!");
            return;
        }

        if (!list.Contains(item))
        {
            list.Add(item);
        }
    }

   /// <summary>
   /// Function that gets all Permutations at all sizes and all orders - its a combination of GetAllPermutations + GetPermutationsAtFixedSize
   /// {1,2,3} = {1},{2},{1,2},{2,1},{3},{1,3},{3,1},{2,3},{3,2},{1,2,3},{1,3,2},{2,1,3},{2,3,1},{3,2,1},{3,1,2},
   /// </summary>
   /// <param name="list"></param>
   /// <typeparam name="T"></typeparam>
   /// <returns></returns>
    public static List<List<T>> GetAllPermutationsInAllOrders<T>(List<T> list)
    {
        List<List<T>> result = new List<List<T>>();
        var combos = GetAllPermutations(list);

        for (var i = 0; i < combos.Count; i++)
        {
            var comboList = combos[i];

            if (comboList.Count <= 1) // does not need to get anymore permutations
            {
                result.Add(comboList);
            }
            else // needs to get even more permutations
            {
                var combosPerm = GetPermutationsAtFixedSize(comboList).ToList();
                for (var j = 0; j < combosPerm.Count; j++)
                {
                    var comboPermList = combosPerm[j].ToList();
                    result.Add(comboPermList);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Function that gets all possible permutations in a list of T.
    /// where {1,2,3} = {1},{2},{1,2},{3},{1,3},{2,3},{1,2,3}
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<List<T>> GetAllPermutations<T>(List<T> list)
    {
        int comboCount = (int) Math.Pow(2, list.Count) - 1;
        List<List<T>> result = new List<List<T>>();
        for (int i = 1; i < comboCount + 1; i++)
        {
            // make each combo here
            result.Add(new List<T>());
            for (int j = 0; j < list.Count; j++)
            {
                if ((i >> j) % 2 != 0)
                    result.Last().Add(list[j]);
            }
        }
        return result;
    }
    /// <summary>
    /// Function that gets all possible permutations in a list of T keeping its size.
    /// where {1,2,3} = {1,2,3},{1,3,2},{2,1,3},{2,3,1},{3,2,1},{3,1,2},
    /// </summary>
    /// <param name="enumerable"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> GetPermutationsAtFixedSize<T>(this IEnumerable<T> enumerable)
    {
        var array = enumerable as T[] ?? enumerable.ToArray();

        var factorials = Enumerable.Range(0, array.Length + 1)
            .Select(Factorial)
            .ToArray();

        for (var i = 0L; i < factorials[array.Length]; i++)
        {
            var sequence = GenerateSequence(i, array.Length - 1, factorials);

            yield return GeneratePermutation(array, sequence);
        }
    }

    private static IEnumerable<T> GeneratePermutation<T>(T[] array, IReadOnlyList<int> sequence)
    {
        var clone = (T[]) array.Clone();

        for (int i = 0; i < clone.Length - 1; i++)
        {
            Swap(ref clone[i], ref clone[i + sequence[i]]);
        }

        return clone;
    }

    private static int[] GenerateSequence(long number, int size, IReadOnlyList<long> factorials)
    {
        var sequence = new int[size];

        for (var j = 0; j < sequence.Length; j++)
        {
            var facto = factorials[sequence.Length - j];

            sequence[j] = (int)(number / facto);
            number = (int)(number % facto);
        }

        return sequence;
    }

    static void Swap<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }

    private static long Factorial(int n)
    {
        long result = n;

        for (int i = 1; i < n; i++)
        {
            result = result * i;
        }

        return result;
    }

    public static void CenterTransformsHorizontal(Transform parent, List<Transform> controls, float itemWidth)
    {
        if (controls.Count <= 0)
        {
            Debug.LogError("No controls in list!");
            return;
        }

        for (var i = 0; i < controls.Count; i++)
        {
            controls[i].localPosition = new Vector3(itemWidth * i, 0);
        }

        var maxWidth = itemWidth * controls.Count;
        var maxWidthHalf = -maxWidth / 2f;
        var additionalMove = controls.Count >= 5 ? 0.0625f : controls.Count % 2 == 0 ? itemWidth / 2f : 0f;
        parent.localPosition += new Vector3(maxWidthHalf + additionalMove, 0);
    }

    public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
    {
        return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }

    public static bool IsNullOrEmpty<T>(this IList<T> list)
    {
        if(list is null || list.Count == 0) return true;
        return false;
    }

    public static T Pop<T>(this List<T> list)
    {
        if (list.Count <= 0)
            return default;

        var item = list[0];
        list.RemoveAt(0);
        return item;
    }

    public static T Peek<T>(this List<T> list)
    {
        if (list.Count <= 0)
            return default;

        var item = list[0];
        return item;
    }


    public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
    {
        var item = list[oldIndex];

        list.RemoveAt(oldIndex);

        if (newIndex > oldIndex) newIndex--;
        // the actual index could have shifted due to the removal

        list.Insert(newIndex, item);
    }

    public static void Move<T>(this List<T> list, T item, int newIndex)
    {
        if (item != null)
        {
            var oldIndex = list.IndexOf(item);
            if (oldIndex > -1)
            {
                list.RemoveAt(oldIndex);

                if (newIndex > oldIndex) newIndex--;
                // the actual index could have shifted due to the removal

                list.Insert(newIndex, item);
            }
        }
    }

    public static void MoveUp<T>(this List<T> list, T item)
    {
        if (item != null)
        {
            var oldIndex = list.IndexOf(item);
            if (oldIndex > -1 && oldIndex < list.Count-1)
            {
                list.RemoveAt(oldIndex);
                var newIndex = oldIndex + 1;
                list.Insert(newIndex, item);
            }
        }
    }

    public static void MoveDown<T>(this List<T> list, T item)
    {
        if (item != null)
        {
            var oldIndex = list.IndexOf(item);
            if (oldIndex > -1 && oldIndex > 0)
            {
                list.RemoveAt(oldIndex);
                var newIndex = oldIndex - 1;
                list.Insert(newIndex, item);
            }
        }
    }

    public static Type GetEnumType(string enumName)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            var type = assembly.GetType(enumName);
            if (type == null)
                continue;
            if (type.IsEnum)
                return type;
        }
        return null;
    }

    public static T ParseEnum<T>(string value, T defaultValue) where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");

        if (Enum.TryParse(value, out T t))
        {
            return t;
        }

        // Debug.Log($"Could not parse enum value. [{value}] returning default! [{defaultValue}]");

        return defaultValue;
    }

    public static T ParseEnum<T>(string value) where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");

        if (Enum.TryParse(value, out T t))
        {
            return t;
        }

        return default;

        // Debug.Log($"Could not parse enum value. [{value}] returning default! [{defaultValue}]");

        //return defaultValue;
    }

    public static string ToCamelCase(this string s)
    {
        var x = s.Replace("_", "");
        if (x.Length == 0) return "Null";
        x = Regex.Replace(x, "([A-Z])([A-Z]+)($|[A-Z])",
            m => m.Groups[1].Value + m.Groups[2].Value.ToLower() + m.Groups[3].Value);
        return char.ToUpper(x[0]) + x.Substring(1);
    }

    public static void SetActive(this Component c, bool p_active)
    {
        if (c != null)
        {
            c.gameObject.SetActive(p_active);
        }
    }

    public static T Copy<T>(T obj)
    {
        string json = JsonUtility.ToJson(obj);
        T t = JsonUtility.FromJson<T>(json);
        return t;
    }

    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }

    public static void DestroyChildren(this Transform t, bool immediate = false, int stopAtIndex = -1)
    {
        for (int i = t.childCount - 1; i > stopAtIndex; i--)
        {
            if (immediate)
            {
                Object.DestroyImmediate(t.GetChild(i).gameObject);
            }
            else
            {
                Object.Destroy(t.GetChild(i).gameObject);
            }
        }
    }
    public static Vector2 WorldToUISpace(this Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }

    public static Vector2 PositionRelative(this RectTransform from, RectTransform to)
    {
        Vector2 localPoint;
        Vector2 fromPivotDerivedOffset = new Vector2(from.rect.width * from.pivot.x + from.rect.xMin, from.rect.height * from.pivot.y + from.rect.yMin);
        Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, from.position);
        screenP += fromPivotDerivedOffset;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(to, screenP, null, out localPoint);
        Vector2 pivotDerivedOffset = new Vector2(to.rect.width * to.pivot.x + to.rect.xMin, to.rect.height * to.pivot.y + to.rect.yMin);
        return to.anchoredPosition + localPoint - pivotDerivedOffset;
    }

    public static Rect RectTransformToScreenSpace(this RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        return new Rect((Vector2)transform.position - (size * 0.5f), size);
    }

    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public static bool IsActive(this Component c)
    {
        if (c == null)
            return false;

        if (c.gameObject == null)
            return false;

        return c.gameObject.activeSelf && c.gameObject.activeInHierarchy;
    }

    public static Vector2 Midpoint(this Vector2 a, Vector2 b)
    {
        return new Vector2((a.x + b.x) / 2, (a.y + b.y) / 2);
    }

    public static void Shuffle<T>(this IList<T> ts)
    {
        ts.Shuffle(new System.Random());
    }

    /// <summary>
    /// thread safe shuffle
    /// </summary>
    /// <param name="ts"></param>
    /// <param name="random"></param>
    /// <typeparam name="T"></typeparam>
    public static void Shuffle<T>(this IList<T> ts, System.Random random)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = random.Next(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public static bool Overlaps(this RectTransform rectTrans1, RectTransform rectTrans2, Rect offset=default)
    {
        var rect1 = rectTrans1.rect;
        var rect1Size = new Vector2(rect1.width+offset.width, rect1.height+offset.height);
        var rect1Pos = (Vector2)rectTrans1.position + offset.position;
        var rect1Overlap = new Rect(rect1Pos.x, rect1Pos.y, rect1Size.x, rect1Size.y);

        var rect2 = rectTrans2.rect;
        var rect2Size = new Vector2(rect2.width, rect2.height);
        var rect2Pos = rectTrans2.position;
        var rect2Overlap = new Rect(rect2Pos.x, rect2Pos.y, rect2Size.x, rect2Size.y);

        return rectTrans1.rect.Overlaps(rectTrans2.rect);
    }

    public static RectTransform RectTransform(this Component p_component)
    {
        if (p_component == null || p_component.transform == null) return null;
        return p_component.transform as RectTransform;
    }

    public static RectTransform RectTransform(this GameObject p_component)
    {
        if (p_component == null || p_component.transform == null) return null;
        return p_component.transform as RectTransform;
    }

    public static float GetCircleAngle(int count, int i)
    {
        return (360f / count) * i;
    }

    public static float Angle(this Vector2 v)
    {
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    public static Vector2 GetCirclePointXY(Vector2 center, float radius, float angle)
    {
        Vector2 pos;
        pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);

        return pos;
    }

    public static Vector3 GetCirclePointXZ(Vector3 center, float radius, float angle)
    {
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);

        return pos;
    }

    public static void OrbitMovementXZ<T>(List<T> orbs, Vector2 center, float radius, float speed, float time) where T : Component
    {
        for (int i = 0; i < orbs.Count; i++)
        {
            float a = (360 / orbs.Count) * i;

            if (orbs[i] != null)
            {
                Transform orb = orbs[i].transform;
                Vector3 pos = GetCirclePointXZ(center, radius, Mathf.RoundToInt(a + (time * speed)));
                orb.position = pos;
            }
        }
    }

    public static void OrbitMovementXY(List<Transform> orbs, Vector2 center, float radius, float speed, float time)
    {
        for (int i = 0; i < orbs.Count; i++)
        {
            float a = (360 / orbs.Count) * i;

            if (orbs[i] != null)
            {
                Transform orb = orbs[i];
                Vector3 pos = GetCirclePointXY(center, radius, Mathf.RoundToInt(a + (time * speed)));
                orb.position = pos;
            }
        }
    }

    public static void OrbitMovementXY<T>(List<T> orbs, Vector2 center, float radius, float speed, float time) where T : Component
    {
        for (int i = 0; i < orbs.Count; i++)
        {
            float a = (360 / orbs.Count) * i;

            if (orbs[i] != null)
            {
                Transform orb = orbs[i].transform;
                Vector3 pos = GetCirclePointXY(center, radius, Mathf.RoundToInt(a + (time * speed)));
                orb.position = pos;
            }
        }
    }

    public static void OrbitMovementXY<T>(T[] orbs, Vector2 center, float radius, float offset) where T : Component
    {
        for (int i = 0; i < orbs.Length; i++)
        {
            float a = (360 / orbs.Length) * i;

            if (orbs[i] != null)
            {
                Transform orb = orbs[i].transform;
                Vector3 pos = GetCirclePointXY(center, radius, a + offset);
                orb.position = pos;
            }
        }
    }

    public static Color ToColor(this string hex, float alpha = 1)
    {
        hex = hex.Replace("#", string.Empty);
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, (byte)(255f * alpha));
    }

    //This implies that no solution exists for this situation as the target may literally outrun the projectile with its current direction
    //In cases like that, we simply aim at the place where the target will be 1 to 5 seconds from now.
    //Feel free to randomize t at your discretion for your specific game situation if you want that guess to feel appropriately noisier
    static float PredictiveAimWildGuessAtImpactTime()
    {
        return Random.Range(0.5f, 2f);
    }

    //////////////////////////////////////////////////////////////////////////////
    //returns true if a valid solution is possible
    //projectileVelocity will be a non-normalized vector representing the muzzle velocity of a lobbed projectile in 3D space
    //if it returns false, projectileVelocity will be filled with a reasonable-looking attempt
    //The reason we return true/false here instead of Vector3 is because you might want your AI to hold that shot until a solution exists
    //This is meant to hit a target moving at constant velocity
    //Full derivation by Kain Shin exists here:
    //http://www.gamasutra.com/blogs/KainShin/20090515/83954/Predictive_Aim_Mathematics_for_AI_Targeting.php
    //gravity is assumed to be a positive number. It will be calculated in the downward direction, feel free to change that if you game takes place in Spaaaaaaaace
    static public bool PredictiveAim(Vector3 muzzlePosition, float projectileSpeed, Vector3 targetPosition, Vector3 targetVelocity, float gravity, out Vector3 projectileVelocity)
    {
        Debug.Assert(projectileSpeed > 0, "What are you doing shooting at something with a projectile that doesn't move?");

        if (muzzlePosition == targetPosition)
        {
            //Why dost thou hate thyself so?
            //Do something smart here. I dunno... whatever.
            projectileVelocity = projectileSpeed * (Random.rotation * Vector3.forward);
            return true;
        }

        //Much of this is geared towards reducing floating point precision errors
        float projectileSpeedSq = projectileSpeed * projectileSpeed;
        float targetSpeedSq = targetVelocity.sqrMagnitude; //doing this instead of self-multiply for maximum accuracy
        float targetSpeed = Mathf.Sqrt(targetSpeedSq);
        Vector3 targetToMuzzle = muzzlePosition - targetPosition;
        float targetToMuzzleDistSq = targetToMuzzle.sqrMagnitude; //doing this instead of self-multiply for maximum accuracy
        float targetToMuzzleDist = Mathf.Sqrt(targetToMuzzleDistSq);
        Vector3 targetToMuzzleDir = targetToMuzzle;
        targetToMuzzleDir.Normalize();

        Vector3 targetVelocityDir = targetVelocity;
        targetVelocityDir.Normalize();

        //Law of Cosines: A*A + B*B - 2*A*B*cos(theta) = C*C
        //A is distance from muzzle to target (known value: targetToMuzzleDist)
        //B is distance traveled by target until impact (targetSpeed * t)
        //C is distance traveled by projectile until impact (projectileSpeed * t)
        float cosTheta = Vector3.Dot(targetToMuzzleDir, targetVelocityDir);

        bool validSolutionFound = true;
        float t;
        if (Mathf.Approximately(projectileSpeedSq, targetSpeedSq))
        {
            //a = projectileSpeedSq - targetSpeedSq = 0
            //We want to avoid div/0 that can result from target and projectile traveling at the same speed
            //We know that C and B are the same length because the target and projectile will travel the same distance to impact
            //Law of Cosines: A*A + B*B - 2*A*B*cos(theta) = C*C
            //Law of Cosines: A*A + B*B - 2*A*B*cos(theta) = B*B
            //Law of Cosines: A*A - 2*A*B*cos(theta) = 0
            //Law of Cosines: A*A = 2*A*B*cos(theta)
            //Law of Cosines: A = 2*B*cos(theta)
            //Law of Cosines: A/(2*cos(theta)) = B
            //Law of Cosines: 0.5f*A/cos(theta) = B
            //Law of Cosines: 0.5f * targetToMuzzleDist / cos(theta) = targetSpeed * t
            //We know that cos(theta) of zero or less means there is no solution, since that would mean B goes backwards or leads to div/0 (infinity)
            if (cosTheta > 0)
            {
                t = 0.5f * targetToMuzzleDist / (targetSpeed * cosTheta);
            }
            else
            {
                validSolutionFound = false;
                t = PredictiveAimWildGuessAtImpactTime();
            }
        }
        else
        {
            //Quadratic formula: Note that lower case 'a' is a completely different derived variable from capital 'A' used in Law of Cosines (sorry):
            //t = [ -b � Sqrt( b*b - 4*a*c ) ] / (2*a)
            float a = projectileSpeedSq - targetSpeedSq;
            float b = 2.0f * targetToMuzzleDist * targetSpeed * cosTheta;
            float c = -targetToMuzzleDistSq;
            float discriminant = b * b - 4.0f * a * c;

            if (discriminant < 0)
            {
                //Square root of a negative number is an imaginary number (NaN)
                //Special thanks to Rupert Key (Twitter: @Arakade) for exposing NaN values that occur when target speed is faster than or equal to projectile speed
                validSolutionFound = false;
                t = PredictiveAimWildGuessAtImpactTime();
            }
            else
            {
                //a will never be zero because we protect against that with "if (Mathf.Approximately(projectileSpeedSq, targetSpeedSq))" above
                float uglyNumber = Mathf.Sqrt(discriminant);
                float t0 = 0.5f * (-b + uglyNumber) / a;
                float t1 = 0.5f * (-b - uglyNumber) / a;
                //Assign the lowest positive time to t to aim at the earliest hit
                t = Mathf.Min(t0, t1);
                if (t < Mathf.Epsilon)
                {
                    t = Mathf.Max(t0, t1);
                }

                if (t < Mathf.Epsilon)
                {
                    //Time can't flow backwards when it comes to aiming.
                    //No real solution was found, take a wild shot at the target's future location
                    validSolutionFound = false;
                    t = PredictiveAimWildGuessAtImpactTime();
                }
            }
        }

        //Vb = Vt - 0.5*Ab*t + [(Pti - Pbi) / t]
        projectileVelocity = targetVelocity + (-targetToMuzzle / t);
        if (!validSolutionFound)
        {
            //PredictiveAimWildGuessAtImpactTime gives you a t that will not result in impact
            // Which means that all that math that assumes projectileSpeed is enough to impact at time t breaks down
            // In this case, we simply want the direction to shoot to make sure we
            // don't break the gameplay rules of the cannon's capabilities aside from gravity compensation
            projectileVelocity = projectileSpeed * projectileVelocity.normalized;
        }

        if (!Mathf.Approximately(gravity, 0))
        {
            //By adding gravity as projectile acceleration, we are essentially breaking real world rules by saying that the projectile
            // gets any upwards/downwards gravity compensation velocity for free, since the projectileSpeed passed in is a constant that assumes zero gravity
            Vector3 projectileAcceleration = gravity * Vector3.down;
            //assuming gravity is a positive number, this next line will apply a free magical upwards lift to compensate for gravity
            Vector3 gravityCompensation = (0.5f * projectileAcceleration * t);
            //Let's cap gravityCompensation to avoid AIs that shoot infinitely high
            float gravityCompensationCap = 0.5f * projectileSpeed;  //let's assume we won't lob higher than 50% of the canon's shot range
            if (gravityCompensation.magnitude > gravityCompensationCap)
            {
                gravityCompensation = gravityCompensationCap * gravityCompensation.normalized;
            }
            projectileVelocity -= gravityCompensation;
        }

        //FOR CHECKING ONLY (valid only if gravity is 0)...
        //float calculatedprojectilespeed = projectileVelocity.magnitude;
        //bool projectilespeedmatchesexpectations = (projectileSpeed == calculatedprojectilespeed);
        //...FOR CHECKING ONLY

        return validSolutionFound;
    }

    public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
    {
        return new UnityWebRequestAwaiter(asyncOp);
    }
}

public class UnityWebRequestAwaiter : INotifyCompletion
{
    private UnityWebRequestAsyncOperation asyncOp;
    private Action continuation;

    public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
    {
        this.asyncOp = asyncOp;
        asyncOp.completed += OnRequestCompleted;
    }

    public bool IsCompleted { get { return asyncOp.isDone; } }

    public void GetResult() { }

    public void OnCompleted(Action continuation)
    {
        this.continuation = continuation;
    }

    private void OnRequestCompleted(AsyncOperation obj)
    {
        continuation();
    }
}
