using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Common
{
    public class ObjectPooling : MonoBehaviour
    {

        public GameObject poolObject;
        public string objectName;

        [Range(1, 10000000)]
        public int numberOfPooledObjects;

        [Range(1, 10000000)]
        public int regenerateMoreObjs;

        public Vector3 spawnLocation = new Vector3(0, -100, 0);
		private Stack<GameObject> collectionOfObjs;
        private GameObject poolGroup;

        void Awake()
        {
            collectionOfObjs = new Stack<GameObject>();
            poolGroup = new GameObject();
            poolGroup.name = objectName + " Pool";
            GenerateCollection(numberOfPooledObjects);
			//print (gameObject.name + ": pool generated");
        }

		public IEnumerator ReturnObject(GameObject returnObj)
        {
			Rigidbody rb = returnObj.GetComponent<Rigidbody> (); // this could be sped up by having a list of static rigid body objects or have a map of objects where the key is the object and the value is the rigid body
			if (rb != null) {
				rb.isKinematic = true;
			}

            returnObj.transform.position = spawnLocation;

			/* only doing this because the smoke affect will go away instantly on hit for player projectile otherwise
			 * should change to use pooling and just separate smoke object when projectile hits, then return smoke to pool and attach to whoever needs smoke (doesn't have to go back to the same parent)
			 * ---
			 * do the same thing for explosion object
			 */
			yield return new WaitForSeconds (1.5F);

			returnObj.SetActive (false);
            collectionOfObjs.Push(returnObj);
			//print (returnObj.name + " is swimming in object pool");
        }

        public GameObject GetNextObject()
        {
			if (collectionOfObjs == null) {
				//print ("null stack!");
			}
            if (collectionOfObjs.Peek() != null)
            {
				return PopObj ();
            }
            else
            {
				GenerateCollection(regenerateMoreObjs);
				return PopObj ();
            }
			/*
            print("Error: Cannot get another GameObject");
            return null;
            */
        }

		private GameObject PopObj() {
			GameObject nextObj = collectionOfObjs.Pop();
			Rigidbody rb = nextObj.GetComponent<Rigidbody> (); // this could be sped up by having a list of static rigid body objects or have a map of objects where the key is the object and the value is the rigid body
			if (rb != null) {
				rb.isKinematic = false;
			}
			nextObj.SetActive(true);
			return nextObj;
		}

        private void GenerateCollection(int length)
        {
            GameObject objectRef;
            //print(length);
            for (int i = 0; i < length; i++)
            {
                objectRef = Instantiate(poolObject, Vector3.zero, Quaternion.identity) as GameObject;
                objectRef.name = objectName;
                objectRef.transform.position = spawnLocation;
                objectRef.transform.parent = poolGroup.transform;
                objectRef.SetActive(false);
                collectionOfObjs.Push(objectRef);
            }
        }
    }
}
