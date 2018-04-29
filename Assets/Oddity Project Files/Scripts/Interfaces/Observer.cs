using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Observer
{
	void OnNotification(MonoBehaviour _class, string _event);
	
} 