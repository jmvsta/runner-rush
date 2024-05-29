﻿using System;
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
        // StreamLined = 3
    }

    public static class Fabric
    {
        private static readonly StrategyBase DefaultStrategy = new PlainStrategy();

        private static readonly Dictionary<Strategy, StrategyBase> StrategyMap = new()
        {
            { Strategy.Plain, DefaultStrategy },
            { Strategy.Mountain, new MountainStrategy() },
            { Strategy.River, new RiverStrategy() },
            // { Strategy.StreamLined, new StreamLined() }
        };

        public static StrategyBase GetStrategy(Strategy strategy)
        {
            return StrategyMap.GetValueOrDefault(strategy, DefaultStrategy);
        }
    }

    public abstract class StrategyBase
    {
        protected readonly Random Random = new();
        public abstract void Apply(List<GameObject> gameObjects, List<GameObject> obstacles, float roadPos);
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
        public override void Apply(List<GameObject> gameObjects, List<GameObject> obstacles, float roadPos)
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
        public override void Apply(List<GameObject> gameObjects, List<GameObject> obstacles, float roadPos)
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
        public override void Apply(List<GameObject> gameObjects, List<GameObject> obstacles, float roadPos)
        {
            var posX = Random.Next(-1, 2) * 3;
            var obstPos = 0;
            float y = 1;

            foreach (var coin in gameObjects)
            {
                if (obstPos < obstacles.Count)
                {
                    var position = obstacles[obstPos].transform.position;
                    var scale = obstacles[obstPos].transform.localScale;
                    var distance = Math.Round(position.z - roadPos);
                    // height = scale.y;

                    // obstacle is located approximately on posX
                    if (position.x + scale.x / 2  >= posX)
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

                coin.transform.position = new Vector3(posX, y, roadPos);
                roadPos += 5;
                coin.SetActive(true);
            }
        }
    }
}