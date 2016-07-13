﻿using UnityEngine;
using System.Collections;
using Common;

namespace Enemy
{
    public class EnemyShooting : GameBehavior
    {
        public bool shouldDebug = false;
        public float fireRate = 1;
        public float damage = 1;
        public float possibleBulletLocationRadius = 1;
        private float lastTimeShot;
        private bool canShoot;
        private GameObject target;
        private Vector3 hitBox;
        private Vector3 shotPos;
        private Vector3 shotDir;

        private float startingSize = 1.4f;
        private float endingSize = 4;
        private float minDistance = 30;
        private float maxDistance = 100;
        private float sizeScale;
        private float distanceOffset;

        private float currX;
        private float currY;
        private float distanceToTarget;

        private float randomXShot;
        private float randomYShot;

        private float debugOffsetX;
        private float debugOffsetZ;
        private float offsetX;
        private float offsetZ;
        private Transform topTransform;

        private const float DISTANCE_X_PROPORTION_SCALE = 1;
        private const float DISTANCE_Y_PROPORTION_SCALE = 2;

        void Awake()
        {
            topTransform = transform.FindChild("Top");
            lastTimeShot = Time.time - fireRate;
        }

        void Update()
        {
            if (target != null)
            {
                //Hitbox centered at target
                hitBox = target.transform.position;
                distanceToTarget = Vector3.Distance(topTransform.position, target.transform.position);
                
                //Creates lower and upper bounds of distance
                if(distanceToTarget < minDistance)
                {
                    distanceToTarget = minDistance;
                } else if(distanceToTarget > maxDistance)
                {
                    distanceToTarget = maxDistance;
                }

                //Using a linear function it gets the size of the hitbox based on distance in 2D
                currX = GetProportionalDistance(distanceToTarget, DISTANCE_X_PROPORTION_SCALE);
                currY = GetProportionalDistance(distanceToTarget, DISTANCE_Y_PROPORTION_SCALE);

                //Pick a random number inside box in 2D
                randomXShot = Random.Range(-currX, currX);
                randomYShot = Random.Range(-currY, currY);

                //Converts 2D values into 3D
                offsetX = randomXShot * Mathf.Cos(topTransform.eulerAngles.y * Mathf.Deg2Rad);
                offsetZ = -randomXShot * Mathf.Sin(topTransform.eulerAngles.y * Mathf.Deg2Rad);

                //Creates visual representation of 3D hitbox
                if (shouldDebug)
                {
                    debugOffsetX = currX * Mathf.Cos(topTransform.eulerAngles.y * Mathf.Deg2Rad);
                    debugOffsetZ = currX * Mathf.Sin(topTransform.eulerAngles.y * Mathf.Deg2Rad);

                    //Top
                    Debug.DrawLine(new Vector3(hitBox.x - debugOffsetX, hitBox.y + currY, hitBox.z + debugOffsetZ), new Vector3(hitBox.x + debugOffsetX, hitBox.y + currY, hitBox.z - debugOffsetZ), Color.red);
                    //Right
                    Debug.DrawLine(new Vector3(hitBox.x + debugOffsetX, hitBox.y + currY, hitBox.z - debugOffsetZ), new Vector3(hitBox.x + debugOffsetX, hitBox.y - currY, hitBox.z - debugOffsetZ), Color.red);
                    //Bottom
                    Debug.DrawLine(new Vector3(hitBox.x + debugOffsetX, hitBox.y - currY, hitBox.z - debugOffsetZ), new Vector3(hitBox.x - debugOffsetX, hitBox.y - currY, hitBox.z + debugOffsetZ), Color.red);
                    //Left
                    Debug.DrawLine(new Vector3(hitBox.x - debugOffsetX, hitBox.y - currY, hitBox.z + debugOffsetZ), new Vector3(hitBox.x - debugOffsetX, hitBox.y + currY, hitBox.z + debugOffsetZ), Color.red);
                }

                //If there is a target, focused, and is fire rate is good
                if (target != null && canShoot && Time.time - lastTimeShot > fireRate)
                {
                    lastTimeShot = Time.time;
                    shotPos = new Vector3(hitBox.x + offsetX, hitBox.y + randomYShot, hitBox.z + offsetZ);
                    shotDir = topTransform.position - shotPos;
                    Debug.DrawRay(shotPos, shotDir, Color.cyan, 1);

                    //TODO: Call shoot function
                }
            }
        }

        float GetProportionalDistance(float distToTarg, float propScale)
        {
            distToTarg = (sizeScale * distToTarg + distanceOffset) * propScale;
            return distToTarg;
        }

        void OnDrawGizmosSelected()
        {
            if (shouldDebug)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(shotPos, new Vector3(0.5f, 0.5f, 0.5f));
            }
        }

        public void SetShoot(bool shoot, GameObject obj)
        {
            canShoot = shoot;
            target = obj;
            sizeScale = (endingSize - startingSize) / (maxDistance - minDistance);
            distanceOffset = startingSize - sizeScale * minDistance;
        }
    }
}
