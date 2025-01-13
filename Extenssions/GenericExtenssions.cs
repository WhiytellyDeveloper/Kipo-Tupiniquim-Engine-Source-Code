using System;
using System.Collections.Generic;
using UnityEngine;

namespace KipoTupiniquimEngine.Extenssions
{
    public static class GenericExtenssions
    {
        public static List<int> GenerateHighList(int start, int step, int totalCount)
        {
            List<int> numberList = [];

            int currentNumber = start;

            for (int i = 0; i < totalCount; i++)
            {
                numberList.Add(currentNumber);
                currentNumber += step;
            }

            return numberList;
        }

        public static List<T> Reverse<T>(this List<T> list)
        {
            List<T> reverseList = [];

            for (int i = list.Count - 1; i >= 0; i--)
                reverseList.Add(list[i]);

            return reverseList;
        }

        public static T ToWeighted<T, A>(this object obj, int weighted) where T : WeightedSelection<A>, new() where A : class
        {
            var castedObject = obj as A;

            if (castedObject == null)
                throw new InvalidCastException($"Unable to convert object to type {typeof(A).Name}.");

            T weightedSelection = new T
            {
                selection = castedObject,
                weight = weighted
            };

            return weightedSelection;
        }

        public static List<WeightedSelection<T>> Convert<T>(this List<T> list, int defaultWeight) where T : class
        {
            List<WeightedSelection<T>> weightedList = new List<WeightedSelection<T>>();
            foreach (T item in list)
            {
                weightedList.Add(new WeightedSelection<T>
                {
                    selection = item,
                    weight = defaultWeight
                });
            }
            return weightedList;
        }

        public static T CatchRandomItem<T>(this T[] array) where T : class
        {
            return array[UnityEngine.Random.RandomRangeInt(0, array.Length)];
        }

        public static T CatchRandomItem<T>(this List<T> array) where T : class
        {
            return array[UnityEngine.Random.RandomRangeInt(0, array.Count)];
        }

        public static float GetDistanceFrom(this Vector3 pos1, Vector3 pos2)
        {
            return Vector3.Distance(pos1, pos2);
        }

        public static float GetVector2DistanceFrom(this Vector3 pos1, Vector3 pos2)
        {
            return Vector2Int.Distance(new(((int)pos1.x), ((int)pos1.z)), new(((int)pos2.x), ((int)pos1.z)));
        }

        public static float GetVector2DistanceFrom(this IntVector2 pos1, IntVector2 pos2)
        {
            return Vector2Int.Distance(new(pos1.x, pos2.z), new(pos2.x, pos2.z));
        }

        public static GameCamera GetPlayerCamera(this PlayerManager player)
        {
            return Singleton<CoreGameManager>.Instance.GetCamera(player.playerNumber);
        }
        public static T Duplicate<T>(this T original) where T : ScriptableObject
        {
            if (original == null)
                return null;
            T novoObjeto = ScriptableObject.CreateInstance<T>();
            novoObjeto.CopyFrom(original);
            return novoObjeto;
        }

        private static void CopyFrom<T>(this T novoObjeto, T original) where T : ScriptableObject
        {
            var type = typeof(T);
            var fields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var field in fields)
                field.SetValue(novoObjeto, field.GetValue(original));
        }

        public static void ResizeCollider(this Collider colliderComponent, Renderer spriteRenderer)
        {
            Vector3 spriteSize = spriteRenderer.bounds.size;
            if (colliderComponent is BoxCollider)
                ((BoxCollider)colliderComponent).size = spriteSize;

            else if (colliderComponent is CapsuleCollider)
            {
                CapsuleCollider capsuleCollider = (CapsuleCollider)colliderComponent;
                float radius = Mathf.Max(spriteSize.x, spriteSize.y) / 2f;
                float height = spriteSize.z;
                capsuleCollider.radius = radius;
                capsuleCollider.height = height;
            }
            else if (colliderComponent is SphereCollider)
            {
                float radius = Mathf.Max(spriteSize.x, Mathf.Max(spriteSize.y, spriteSize.z)) / 2f;
                ((SphereCollider)colliderComponent).radius = radius;
            }

            else
                Debug.LogError("Collider type is not supported by this script!");
        }

        public static Direction GetDirection(this Transform transform)
        {
            float angle = Mathf.Atan2(transform.forward.z, transform.forward.x) * Mathf.Rad2Deg;

            angle = (angle + 360) % 360;

            if (angle >= 45 && angle < 135)
                return Direction.North;
            else if (angle >= 135 && angle < 225)
                return Direction.West;
            else if (angle >= 225 && angle < 315)
                return Direction.South;
            else
                return Direction.East;
        }
        
        public static Vector3 GetRotationFromDirection(this Direction dir)
        {
            Vector3[] directions = [Vector3.forward, Vector3.right, -Vector3.forward, -Vector3.right];
            return directions[((int)dir)];
        }

        //Made By Pixel Guy
        public static void BlockAllDirs(this EnvironmentController ec, IntVector2 pos, bool block)
        {
            ec.FreezeNavigationUpdates(true);
            var origin = ec.CellFromPosition(pos);
            for (int i = 0; i < 4; i++)
            {
                var dir = (Direction)i;
                var cell = ec.CellFromPosition(pos + dir.ToIntVector2());
                if (origin.ConstNavigable(dir))
                    cell.Block(dir.GetOpposite(), block);
            }
            ec.FreezeNavigationUpdates(false);
        }
    }
}
