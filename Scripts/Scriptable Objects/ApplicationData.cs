using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
[CreateAssetMenu(menuName = ASSET_PATH_DATA_ROOT + "Application Data")]
public class ApplicationData : ScriptableObject
{
	public const string ASSET_PATH_DATA_ROOT = "Uriel's Challenge Data / "; 	/// <summary>Data's Asset root path.</summary>
	public const string SCENE_PATH_PREFIX = "Scene_"; 							/// <summary>Scene's Path prefix.</summary>
	public const string SCENE_PATH_MENU = SCENE_PATH_PREFIX + "Menu"; 			/// <summary>Menu Scene's Path.</summary>
	public const int INDEX_RIM_CROSS_RIGHT_HANDLER_A = 0; 						/// <summary>Right Handler A's Index.</summary>
	public const int INDEX_RIM_CROSS_RIGHT_HANDLER_B = 1; 						/// <summary>Right Handler b's Index.</summary>
	public const int INDEX_RIM_CROSS_FORWARD_HANDLER_A = 2; 					/// <summary>Forward Handler A's Index.</summary>
	public const int INDEX_RIM_CROSS_FORWARD_HANDLER_B = 3; 					/// <summary>Forward Handler B's Index.</summary>

	[Header("Prefabs' References:")]
	[SerializeField] private User _user; 										/// <summary>User's Prefab reference.</summary>
	[Space(5f)]
	[Header("Layer Masks:")]
	[SerializeField] private LayerMask _pickableLayer; 							/// <summary>Pickable's Layer Mask.</summary>
	[SerializeField] private LayerMask _pickableHandlerLayer; 					/// <summary>Pickable Handler's Layer Mask.</summary>

	/// <summary>Gets user property.</summary>
	public User user { get { return _user; } }

	/// <summary>Gets pickableLayer property.</summary>
	public LayerMask pickableLayer { get { return _pickableLayer; } }

	/// <summary>Gets pickableHandlerLayer property.</summary>
	public LayerMask pickableHandlerLayer { get { return _pickableHandlerLayer; } }
}
}