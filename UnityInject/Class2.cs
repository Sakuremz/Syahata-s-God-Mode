using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Net;
using UnityEngine.Experimental.GlobalIllumination;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.Experimental.Playables;
using UnityEngine.SceneManagement;

namespace UnityInjectX
{
    class Main : MonoBehaviour
    {
        GameObject syahata;             //Player

        private List<GameObject> Enemy = new List<GameObject>();

        private Vector2 enemyPos;

        int hp;

        bool godMode;           //无敌
        bool infAmmo;           //无限弹药

        GUIStyle cStyle = new GUIStyle();
        GUIStyle menu = new GUIStyle();
        

        public void Start()
        {
            /*                  GUI Option                */
            GUI.backgroundColor = new Color(255, 122, 255, 0.3f);

            menu.fontSize = 18;
            menu.normal.textColor = Color.white;

            cStyle.fontSize = 30;
            cStyle.normal.textColor = Color.red;

            StartCoroutine(LoadData());
        }
        public void Update()
        {
            if (syahata != null) 
            { 
                hp = syahata.GetComponent<PlayerController>().Hp;
                allSwitch();
            }
            else
            {
                syahata = GameObject.Find("Syahata");
            }
        }

        private IEnumerator LoadData()
        {
            while (true)
            {
                // 模拟耗时操作
                yield return new WaitForSeconds(0.2f);

                // 获取当前场景中的所有游戏对象的引用
                GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
                // 遍历所有游戏对象
                foreach (GameObject obj in allGameObjects)
                {

                    Match zombie = Regex.Match(obj.name, @"\bZombie");
                    Match boss = Regex.Match(obj.name, @"\bBoss");

                    if (zombie.Success)
                    {
                        Enemy.Add(obj);
                    }

                    if (boss.Success)
                    {
                        Enemy.Add(obj);
                    }
                }

                // 耗时操作完成后继续执行其他操作
                if (Enemy != null)
                {
                    Debug.Log("敌人数据缓存完成.");
                }

                //锁定值
                if (godMode)
                {
                    syahata.GetComponent<PlayerController>().Hp = 100;
                    syahata.GetComponent<PlayerController>().stamina = 100;
                }
                if (infAmmo)
                {
                    syahata.GetComponent<Weapon>().ammunition = 10;
                    syahata.GetComponent<Weapon>().R_ammunition = 30;
                }
            }
        }

        private void allSwitch()
        {
            if (Input.GetKeyDown(KeyCode.F1))    //I键实现无敌、无视障碍物、加速      press down I to switch god mode
            {
                godMode = !godMode;
                if (godMode)
                {
                    syahata.GetComponent<Rigidbody2D>().gravityScale = 0;
                    syahata.GetComponent<BoxCollider2D>().enabled = false;
                    syahata.GetComponent<PlayerController>().walkSpeed = 100;
                    syahata.GetComponent<PlayerController>().H = 0;
                }
                else
                {
                    syahata.GetComponent<Rigidbody2D>().gravityScale = 10;
                    syahata.GetComponent<BoxCollider2D>().enabled = true;
                    syahata.GetComponent<PlayerController>().walkSpeed = 10;
                }
            }
            if (Input.GetKeyDown(KeyCode.F2))       //无限子弹      Infinite Ammo
            {
                infAmmo = !infAmmo;
                if (infAmmo)
                {
                    syahata.GetComponent<Weapon>().P_fireDelatTime = 0;
                }
                else
                {
                    syahata.GetComponent<Weapon>().P_fireDelatTime = 0.7f;
                }
            }
        }

        public void OnGUI()
        {
            string HP = hp.ToString();

            if (Enemy != null)
            {
                foreach (GameObject enemy in Enemy)
                {
                    
                    if (enemy != null)      //draw enemy box
                    {
                        if (enemy.GetComponent<BoxCollider2D>().enabled != false)
                        {
                            enemyPos = Camera.main.WorldToScreenPoint(enemy.transform.position);
                            Rect rect = new Rect(enemyPos.x - 30f, Screen.height - enemyPos.y, 60f, 140f);

                            GUI.Box(rect, "敌人", cStyle);        //enemy box
                        }
                    }
                }
            }
            GUI.Box(new Rect(5f, 360f, 180f, 300f), "Menu");
            GUI.Label(new Rect(10f, 400f, 150f, 50f), string.Format("当前血量：{0}", HP), menu); //current hp
            GUI.Label(new Rect(10f, 420f, 150f, 50f), string.Format("无敌：{0}", godMode), menu);      //god mode
            GUI.Label(new Rect(10f, 440f, 150f, 50f), string.Format("无限子弹：{0}", infAmmo), menu);     //Infinite Ammo
        }
    }
}