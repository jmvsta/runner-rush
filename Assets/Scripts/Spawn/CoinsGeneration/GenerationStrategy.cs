using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

namespace Spawn.CoinsGeneration
{
    public enum Strategy
    {
        Plain = 0,
        Mountain = 1,
        River = 2,
        SimpleIgnoreStrategy = 3
    }

    public static class Fabric
    {
        private static readonly StrategyBase DefaultStrategy = new PlainStrategy();

        private static readonly Dictionary<Strategy, StrategyBase> StrategyMap = new()
        {
            { Strategy.Plain, DefaultStrategy },
            { Strategy.Mountain, new MountainStrategy() },
            { Strategy.River, new RiverStrategy() },
            { Strategy.SimpleIgnoreStrategy, new SimpleIgnoreStrategy() }
        };

        public static StrategyBase GetStrategy(int strategy)
        {
            return StrategyMap.GetValueOrDefault((Strategy) strategy, DefaultStrategy);
        }
    }

    public abstract class StrategyBase
    {
        protected readonly Random Random = new();

        public abstract void Apply(List<GameObject> gameObjects, List<GameObject> obstacles, float roadPos,
            GameObject tail = null);
    }

    // public class StreamLined : StrategyBase
    // {
    //     private List<GameObject> _obstacles;
    //
    //     public StreamLined()
    //     {
    //         _obstacles = obstacles;
    //     }
    //
    //     public override void Apply(List<GameObject> coins, float roadPos)
    //     {
    //         throw new NotImplementedException();
    //     }
    // }

    public class MountainStrategy : StrategyBase
    {
        public override void Apply(List<GameObject> gameObjects, List<GameObject> obstacles, float roadPos,
            GameObject tail)
        {
            var posX = Random.Next(-1, 2) * 3;
            var len = gameObjects.Count;
            var c = 1;

            for (int i = 0, y = 1; i < len; i++, y += c, roadPos += 5)
            {
                c = y % 5 != 0 ? c : -c;
                gameObjects[i].transform.position = new Vector3(posX, y, roadPos);
                gameObjects[i].SetActive(true);
            }
        }
    }

    public class RiverStrategy : StrategyBase
    {
        public override void Apply(List<GameObject> gameObjects, List<GameObject> obstacles, float roadPos,
            GameObject tail)
        {
            var posX = Random.Next(-1, 2) * 3;
            var len = gameObjects.Count;
            var r = posX == 0 ? Random.Next(0, 2) == 0 ? -3 : 3 : -posX;
            var c = r < 0 ? -1 : 1;
            var border = Math.Abs(posX - r);

            for (int i = 0, x = posX; i < len; i++, x += c, roadPos += 5)
            {
                gameObjects[i].transform.position = new Vector3(Math.Abs(r + x) < border ? x : r, 1, roadPos);
                gameObjects[i].SetActive(true);
            }
        }
    }

    public class PlainStrategy : StrategyBase
    {
        public override void Apply(List<GameObject> gameObjects, List<GameObject> obstacles, float roadPos,
            GameObject tail)
        {
            var posX = Random.Next(-1, 2) * 3;
            var obstPos = 0;
            float y = 1;
            var preCount = 0;

            if (tail != null)
            {
                var oldX = tail.transform.position.x;
                if (tail.transform.position.y == 1f)
                {
                    while (oldX != posX)
                    {
                        oldX += oldX < posX ? 1 : -1;
                        gameObjects[preCount].transform.position = new Vector3(oldX, y, roadPos);
                        roadPos += 5;
                        gameObjects[preCount].SetActive(true);
                        preCount++;
                    }
                }
            }

            for (var i = preCount + 1; i < gameObjects.Count; i++)
            {
                if (obstPos < obstacles.Count)
                {
                    var position = obstacles[obstPos].transform.position;
                    var scale = obstacles[obstPos].transform.localScale;
                    var distance = Math.Round(position.z - roadPos);
                    // height = scale.y;

                    // obstacle is located approximately on posX
                    if (position.x + scale.x / 2 >= posX)
                    {
                        switch (distance)
                        {
                            case >= 5 when distance < scale.y * 5:
                                y++;
                                break;
                            case <= -5 when distance > -scale.y * 5:
                                y = y > 1 ? y - 1 : 1;
                                break;
                            case < 5 and > -5:
                                y = scale.y + 1;
                                break;
                            default:
                                y = 1;
                                break;
                        }
                    }
                    else if (distance < 20)
                    {
                        y = 1;
                        obstPos++;
                    }
                }

                gameObjects[i].transform.position = new Vector3(posX, y, roadPos);
                roadPos += 5;
                gameObjects[i].SetActive(true);
            }
        }
    }

    public class SimpleIgnoreStrategy : StrategyBase
    {
        public override void Apply(List<GameObject> gameObjects, List<GameObject> obstacles, float roadPos,
            GameObject tail)
        {
            var posX = Random.Next(-1, 2) * 3;
            using var enumerator = obstacles.GetEnumerator();
            GameObject obstacle = null;
            if (enumerator.MoveNext())
            {
                obstacle = enumerator.Current;
            }

            for (var i = 1; i < gameObjects.Count; i++)
            {
                if (obstacle != null && Math.Abs(posX - obstacle.transform.position.x) < 3 &&
                    Math.Abs(roadPos - obstacle.transform.position.z) <= 5)
                {
                    roadPos += 5;
                    obstacle = enumerator.MoveNext() ? enumerator.Current : null;
                    continue;
                }
                gameObjects[i].transform.position = new Vector3(posX, 1, roadPos);
                roadPos += 5;
                gameObjects[i].SetActive(true);
            }
        }
    }
}