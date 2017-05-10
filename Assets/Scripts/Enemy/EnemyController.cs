using UnityEngine;
using System.Collections;
using Common;

namespace Enemy
{
    public class EnemyController : GameBehavior
    {
		private GameObject player;
        private NavMeshAgent agent;
        public GameObject moveTarget;
		private Rigidbody target;
        private float width;
        private float height;
        private EnemyScan enemyScanScript;
		private EnemyShooting enemyShootingScript;

        private float headTurnStartTime;
        public Transform topTransform;
        private bool keepShooting;
        private bool topIsFocused;
        private bool firstTimeFocused;
        private float startRotation;
        private float endRotation;
        private float startTime;
        private float scanTime = 1f;
        private float currScanTime;
        private float lastSeenBuffer = 2.5f;

        RaycastHit lHit;
        RaycastHit rHit;
        RaycastHit cHit;

        Vector3 enemyToPlayer;
        float enemyToPlayerAngle;
        float xOffset;
        float zOffset;

        Vector3 enemyPositionL;
        Vector3 enemyPositionR;
        Vector3 enemyPositionC;

        Vector3 enemyDirectionL;
        Vector3 enemyDirectionR;
        Vector3 enemyDirectionC;

        bool didHitL;
        bool didHitR;
        bool didHitC;
        Vector3 topToPlayerLocal;
        Vector3 enemyTopToPlayer;
        float lastTimeSeenPlayer;

        void Start()
        {
			player = GameObject.FindGameObjectWithTag ("Player");
			moveTarget = player;
            agent = GetComponent<NavMeshAgent>();//transform.FindChild("NavMesh").
			enemyScanScript = GetComponent<EnemyScan>();
            enemyShootingScript = GetComponent<EnemyShooting>();
            width = 6.5f;
            height = 5;
            keepShooting = false;
            topIsFocused = false;
            firstTimeFocused = true;
            lastTimeSeenPlayer = Time.time - lastSeenBuffer;
			target = moveTarget.GetComponent<Rigidbody>();
        }
        
        void Update()
        {
            UpdateRaycast();
				

				int distanceToTarget = (int)Vector3.Distance (transform.position, moveTarget.transform.position);
				agent.avoidancePriority = distanceToTarget;
				
				//If can see entire target or target hasn't broken sight
				if (didHitL && didHitR && lHit.transform.name == GlobalVariables.PlayerName && rHit.transform.name == GlobalVariables.PlayerName || keepShooting) {
					//If can see player stop scanning and this
					if (enemyScanScript) {
						enemyScanScript.StopScanning ();
					}

					agent.SetDestination (transform.position);
					agent.avoidancePriority = distanceToTarget;

					//If target moves and are reacquired in less that last buffer then aim immediately
					if (Time.time - lastTimeSeenPlayer < lastSeenBuffer) {
						topIsFocused = true;
					}

					//If the top is lined up with the target
					if (topIsFocused) {
						topTransform.LookAt (moveTarget.transform);
						enemyShootingScript.SetShoot (true, moveTarget);
						lastTimeSeenPlayer = Time.time;
					}
                //Play animation to aim on target
                else {
						//First time this is trying to focus on target
						if (firstTimeFocused) {
							startRotation = topTransform.localEulerAngles.y;
							topToPlayerLocal = transform.InverseTransformDirection (enemyTopToPlayer);
							endRotation = Mathf.Atan (topToPlayerLocal.x / topToPlayerLocal.z) * Mathf.Rad2Deg;

							startTime = Time.time;
							firstTimeFocused = false;
						}
                    //Runs smoothing
                    else {
							float simpleStartRot = GetSimplifiedAngle (startRotation);
							float simpleEndRot = GetSimplifiedAngle (endRotation);

							//Fixes problem when comparing for example 2 and 355 by making distance 7 instead of 333
							if (simpleStartRot >= 180) {
								simpleStartRot = 360 - simpleStartRot;
							}
							if (simpleEndRot >= 180) {
								simpleEndRot = 360 - simpleEndRot;
							}

							//Keeps same speed as the scanner
							float dist = Mathf.Abs (simpleStartRot - endRotation);
							if (enemyScanScript) {
								currScanTime = dist * enemyScanScript.GetTimePerScanDegree ();
							} else {
								currScanTime = scanTime;
							}
							float tRot = Mathf.LerpAngle (startRotation, endRotation, (Time.time - startTime) / currScanTime);
							topTransform.localEulerAngles = new Vector3 (topTransform.localEulerAngles.x, tRot, topTransform.localEulerAngles.z);
							if (AngleRoughlyEqual (tRot, endRotation, 0.01f)) {
								topIsFocused = true;
							}
						}
					}
                
					//After the target is completely visible then hides, but is still visible, this keeps the focus on the target
					if (didHitC && cHit.transform.name != GlobalVariables.PlayerName) {
						keepShooting = false;
					} else {
						keepShooting = true;
					}
				}
            //If the target is not visible
            	else {
					if (enemyScanScript) {
						enemyScanScript.StartScanning ();
					}
					agent.avoidancePriority = 99;
					agent.SetDestination (moveTarget.transform.position);
					enemyShootingScript.SetShoot (false, null);
					firstTimeFocused = true;
					topIsFocused = false;
				}

        }


		void OnCollisionEnter(Collision otherObject)
		{
			if (otherObject.gameObject.tag == "enemy") 
			{
				Debug.Log ("bump");
			}
		
		}

        void UpdateRaycast()
        {
            //Define direction from this to target
            enemyDirectionL = enemyPositionL - moveTarget.transform.position;
            enemyDirectionR = enemyPositionR - moveTarget.transform.position;
            enemyDirectionC = topTransform.rotation * Vector3.forward * 20;
            enemyTopToPlayer = new Vector3(topTransform.position.x, topTransform.position.y, topTransform.position.z) - moveTarget.transform.position;
            enemyPositionC = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
            enemyToPlayer = enemyPositionC - moveTarget.transform.position;

            //Define offset of raycast so that they come off tangent from this to the target
            enemyToPlayerAngle = Mathf.Atan2(enemyToPlayer.x, enemyToPlayer.z);
            xOffset = width * Mathf.Cos(enemyToPlayerAngle);
            zOffset = width * Mathf.Sin(enemyToPlayerAngle);

            //Define left and right position
            enemyPositionL = new Vector3(transform.position.x - xOffset, transform.position.y + height, transform.position.z + zOffset);
            enemyPositionR = new Vector3(transform.position.x + xOffset, transform.position.y + height, transform.position.z - zOffset);

            //Makes sure direction is going the correct way
            enemyDirectionL *= -1;
            enemyDirectionR *= -1;
            enemyToPlayer *= -1;
            enemyTopToPlayer *= -1;

            //Debug
            Debug.DrawRay(enemyPositionL, enemyDirectionL, Color.blue);
            Debug.DrawRay(enemyPositionR, enemyDirectionR, Color.blue);
            Debug.DrawRay(enemyPositionC, enemyDirectionC, Color.red);
            Debug.DrawRay(enemyPositionC, enemyToPlayer, Color.green);

            //Defines if raycast hit anything and what it hit
            didHitL = Physics.Raycast(enemyPositionL, enemyDirectionL, out lHit);
            didHitR = Physics.Raycast(enemyPositionR, enemyDirectionR, out rHit);
            didHitC = Physics.Raycast(enemyPositionC, enemyDirectionC, out cHit);
        }
    }
}
