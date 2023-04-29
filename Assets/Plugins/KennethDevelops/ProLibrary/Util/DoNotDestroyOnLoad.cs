using System.Collections.Generic;
using UnityEngine;

namespace KennethDevelops.ProLibrary.Util{
	public class DoNotDestroyOnLoad : MonoBehaviour{

		public static HashSet<string> instances = new HashSet<string>();
	
		[Header("Removes duplicates on the new loaded scene")]
		public bool removeDuplicates;
		[Space]
		public ExecutionEventType executeOn = ExecutionEventType.Awake;
		public float customTimeToExecute;
	

		void Awake(){
			if (executeOn != ExecutionEventType.Awake)
				return;
		
			Perform();
		}

		void Start(){
			if (executeOn == ExecutionEventType.Custom)
				StartCoroutine(new WaitForSecondsAndDo(customTimeToExecute, Perform));
			else if (executeOn != ExecutionEventType.Start)
				return;
		
			Perform();
		}

		private void Perform(){
			if (instances.Contains(gameObject.name)){
				Destroy(gameObject);
				return;
			}
		
			instances.Add(gameObject.name);
			DontDestroyOnLoad(gameObject);
		}
	
	
	
		public enum ExecutionEventType{Awake, Start, Custom}
	
	}
}
