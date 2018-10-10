using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ToolsInterface {

	/**
		All tools need to inherit from this 
		abstract class and must provide an 
		interact method that will be triggered
		by the player colliding with the tool
		while holding on the jump (space) button
	 */
	void Interact(GameObject player);
}
