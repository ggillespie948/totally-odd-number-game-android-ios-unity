using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Observable
{
	void AddObserver(Observer ob);

	void DeleteObserver(Observer ob);

	void NotifyObservers(MonoBehaviour _class, string _event);
	
	
} 
