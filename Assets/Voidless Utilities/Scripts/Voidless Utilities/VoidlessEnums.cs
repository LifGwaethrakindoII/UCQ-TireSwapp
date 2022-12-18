using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using VoidlessUtilities.Services;

namespace VoidlessUtilities
{
[Flags]
public enum Axes3D 												/// <summary>Axes on a 3D space.</summary>
{
	None = 0, 													/// <summary>No Axis.</summary>
	X = 1, 														/// <summary>X Axis.</summary>
	Y = 2, 														/// <summary>Y Axis.</summary>
	Z = 4, 														/// <summary>Z Axis.</summary>
	XAndY = X | Y, 												/// <summary>X and Y Axes.</summary>
	XAndZ = X | Z, 												/// <summary>X and Z Axes.</summary>
	YAndZ = Y | Z, 												/// <summary>Y and Z Axes.</summary>
	XAndYAndZ = X | Y | Z 										/// <summary>X, Y and Z Axes.</summary>
}

[Flags]
public enum TransformProperties
{
	Position = 1,
	Rotation = 2,
	Scale = 4,

	PositionAndRotation = Position | Rotation,
	PositionAndScale = Position | Scale,
	All = Position | Rotation | Scale,
	RotationAndScale = Rotation | Scale
}

public static class VoidlessEnums
{

#region AgnosticEnumOperations:
	/// <summary>Gets the number of active flags on a given int.</summary>
	/// <param name="_enumFlag">Enum Flag to count active [1] bits.</param>
	/// <returns>Number of active bits on enum flag.</returns>
	public static int GetActiveFlagsCount(int _enumFlag)
	{
		int count = 0;

		while(_enumFlag > 0)
		{
			_enumFlag &= (_enumFlag - 1);
			count++;
		}

		return count;
	}

	/// <summary>Checks if int enumerator contains flag.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the int enumerator contains flag.</returns>
	public static bool HasFlag(int _enumFlag, int _flag){ return ((_enumFlag & _flag) == _flag); }
	
	/// <summary>Checks if int enumerator contains flags.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the int enumerator contains all flags.</returns>
	public static bool HasFlags(int _enumFlag, params int[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enumFlag & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to int enumerator.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the int enumerator.</param>
	public static void AddFlag(ref int _enumFlag, int _flag){ if(!HasFlag(_enumFlag, _flag)) _enumFlag |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to int enumerator.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the int enumerator.</param>
	public static void AddFlags(ref int _enumFlag, params int[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!HasFlag(_enumFlag, _flags[i])) _enumFlag |= _flags[i]; }
	
	/// <summary>Removes flag from int enumerator, if it has it.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from int enumerator.</param>
	public static void RemoveFlag(ref int _enumFlag, int _flag){ if(HasFlag(_enumFlag, _flag)) _enumFlag ^= _flag; }
	
	/// <summary>Removes flags from int enumerator, if it has it.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from int enumerator.</param>
	public static void RemoveFlags(ref int _enumFlag, params int[] _flags){ for(int i = 0; i < _flags.Length; i++) if(HasFlag(_enumFlag, _flags[i])) _enumFlag ^= _flags[i]; }
	
	/// <summary>Toggles flag from int enumerator, if it has it.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from int enumerator.</param>
	public static void ToggleFlag(ref int _enumFlag, int _flag){ _enumFlag ^= _flag; }
	
	/// <summary>Toggles flags from int enumerator, if it has it.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from int enumerator.</param>
	public static void ToggleFlags(ref int _enumFlag, params int[] _flags){ for(int i = 0; i < _flags.Length; i++) _enumFlag ^= _flags[i]; }
	
	/// <summary>Removes all int enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flags operation.</param>
	public static void RemoveAllFlags(ref int _enumFlag){ _enumFlag = (int)0; }
#endregion

	/*/// <summary>Converts XBoxInputKey enumerator value to KeyCode value.</summary>
	/// <param name="_XBoxInputKey">XBoxInputKey value.</param>
	/// <returns>XBoxInputKey value to KeyCode, mapped relative to the platform.</returns>
	public static KeyCode ToKeyCode(this XBoxInputKey _XBoxInputKey)
	{
		switch(_XBoxInputKey)
		{
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
			case XBoxInputKey.A: 				return KeyCode.JoystickButton0;
			case XBoxInputKey.B: 				return KeyCode.JoystickButton1;
			case XBoxInputKey.X: 				return KeyCode.JoystickButton2;
			case XBoxInputKey.Y: 				return KeyCode.JoystickButton3;
			case XBoxInputKey.LB: 				return KeyCode.JoystickButton4;
			case XBoxInputKey.RB: 				return KeyCode.JoystickButton5;
			case XBoxInputKey.Back: 			return KeyCode.JoystickButton6;
			case XBoxInputKey.Start: 			return KeyCode.JoystickButton7;
			case XBoxInputKey.LeftStickClick: 	return KeyCode.JoystickButton8;
			case XBoxInputKey.RightStickClick: 	return KeyCode.JoystickButton9;
#elif UNITY_STANDALONE_LINUX
			case XBoxInputKey.A: 				return KeyCode.JoystickButton0;
			case XBoxInputKey.B: 				return KeyCode.JoystickButton1;
			case XBoxInputKey.X: 				return KeyCode.JoystickButton2;
			case XBoxInputKey.Y: 				return KeyCode.JoystickButton3;
			case XBoxInputKey.LB: 				return KeyCode.JoystickButton4;
			case XBoxInputKey.RB: 				return KeyCode.JoystickButton5;
			case XBoxInputKey.Back: 			return KeyCode.JoystickButton6;
			case XBoxInputKey.Start: 			return KeyCode.JoystickButton7;
			case XBoxInputKey.LeftStickClick: 	return KeyCode.JoystickButton9;
			case XBoxInputKey.RightStickClick: 	return KeyCode.JoystickButton10;
			case XBoxInputKey.DPadUp: 			return KeyCode.JoystickButton13;
			case XBoxInputKey.DPadDown: 		return KeyCode.JoystickButton14;
			case XBoxInputKey.DPadLeft: 		return KeyCode.JoystickButton11;
			case XBoxInputKey.DPadRight: 		return KeyCode.JoystickButton12;
#elif (UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX)
			case XBoxInputKey.A: 				return KeyCode.JoystickButton16;
			case XBoxInputKey.B: 				return KeyCode.JoystickButton17;
			case XBoxInputKey.X: 				return KeyCode.JoystickButton18;
			case XBoxInputKey.Y: 				return KeyCode.JoystickButton19;
			case XBoxInputKey.LB: 				return KeyCode.JoystickButton13;
			case XBoxInputKey.RB: 				return KeyCode.JoystickButton14;
			case XBoxInputKey.Back: 			return KeyCode.JoystickButton10;
			case XBoxInputKey.Start: 			return KeyCode.JoystickButton9;
			case XBoxInputKey.LeftStickClick: 	return KeyCode.JoystickButton11;
			case XBoxInputKey.RightStickClick: 	return KeyCode.JoystickButton12;
			case XBoxInputKey.DPadUp: 			return KeyCode.JoystickButton5;
			case XBoxInputKey.DPadDown: 		return KeyCode.JoystickButton6;
			case XBoxInputKey.DPadLeft: 		return KeyCode.JoystickButton7;
			case XBoxInputKey.DPadRight: 		return KeyCode.JoystickButton8;
			case XBoxInputKey.XBoxButton: 		return KeyCode.JoystickButton15;
#endif	
		}

		return default(KeyCode);
	}

#region DetectableControllersFlagEnumOperations:
	/// <summary>Checks if DetectableControllers enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the DetectableControllers enumerator contains flag.</returns>
	public static bool HasFlag(this DetectableControllers _enum, DetectableControllers _flag){ return ((_enum & _flag) == _flag); }
	
	/// <summary>Checks if DetectableControllers enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the DetectableControllers enumerator contains all flags.</returns>
	public static bool HasFlags(this DetectableControllers _enum, params DetectableControllers[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to DetectableControllers enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the DetectableControllers enumerator.</param>
	public static void AddFlag(ref DetectableControllers _enum, DetectableControllers _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to DetectableControllers enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the DetectableControllers enumerator.</param>
	public static void AddFlags(ref DetectableControllers _enum, params DetectableControllers[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }
	
	/// <summary>Removes flag from DetectableControllers enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from DetectableControllers enumerator.</param>
	public static void RemoveFlag(ref DetectableControllers _enum, DetectableControllers _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }
	
	/// <summary>Removes flags from DetectableControllers enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from DetectableControllers enumerator.</param>
	public static void RemoveFlags(ref DetectableControllers _enum, params DetectableControllers[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }
	
	/// <summary>Toggles flag from DetectableControllers enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from DetectableControllers enumerator.</param>
	public static void ToggleFlag(ref DetectableControllers _enum, DetectableControllers _flag){ _enum ^= _flag; }
	
	/// <summary>Toggles flags from DetectableControllers enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from DetectableControllers enumerator.</param>
	public static void ToggleFlags(ref DetectableControllers _enum, params DetectableControllers[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }
	
	/// <summary>Removes all DetectableControllers enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref DetectableControllers _enum){ _enum = (DetectableControllers)0; }
#endregion

#region FSMStatesFlagEnumOperations:
	/// <summary>Checks if FSMStates enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the FSMStates enumerator contains flag.</returns>
	public static bool HasFlag(this FSMStates _enum, FSMStates _flag){ return ((_enum & _flag) == _flag); }
	
	/// <summary>Checks if FSMStates enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the FSMStates enumerator contains all flags.</returns>
	public static bool HasFlags(this FSMStates _enum, params FSMStates[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to FSMStates enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the FSMStates enumerator.</param>
	public static void AddFlag(ref FSMStates _enum, FSMStates _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to FSMStates enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the FSMStates enumerator.</param>
	public static void AddFlags(ref FSMStates _enum, params FSMStates[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }
	
	/// <summary>Removes flag from FSMStates enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from FSMStates enumerator.</param>
	public static void RemoveFlag(ref FSMStates _enum, FSMStates _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }
	
	/// <summary>Removes flags from FSMStates enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from FSMStates enumerator.</param>
	public static void RemoveFlags(ref FSMStates _enum, params FSMStates[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }
	
	/// <summary>Toggles flag from FSMStates enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from FSMStates enumerator.</param>
	public static void ToggleFlag(ref FSMStates _enum, FSMStates _flag){ _enum ^= _flag; }
	
	/// <summary>Toggles flags from FSMStates enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from FSMStates enumerator.</param>
	public static void ToggleFlags(ref FSMStates _enum, params FSMStates[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }
	
	/// <summary>Removes all FSMStates enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref FSMStates _enum){ _enum = (FSMStates)0; }
#endregion

#region MovementConstrainsFlagEnumOperations:
	/// <summary>Checks if MovementConstrains enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the MovementConstrains enumerator contains flag.</returns>
	public static bool HasFlag(this MovementConstrains _enum, MovementConstrains _flag){ return ((_enum & _flag) == _flag); }
	
	/// <summary>Checks if MovementConstrains enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the MovementConstrains enumerator contains all flags.</returns>
	public static bool HasFlags(this MovementConstrains _enum, params MovementConstrains[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to MovementConstrains enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the MovementConstrains enumerator.</param>
	public static void AddFlag(ref MovementConstrains _enum, MovementConstrains _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to MovementConstrains enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the MovementConstrains enumerator.</param>
	public static void AddFlags(ref MovementConstrains _enum, params MovementConstrains[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }
	
	/// <summary>Removes flag from MovementConstrains enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from MovementConstrains enumerator.</param>
	public static void RemoveFlag(ref MovementConstrains _enum, MovementConstrains _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }
	
	/// <summary>Removes flags from MovementConstrains enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from MovementConstrains enumerator.</param>
	public static void RemoveFlags(ref MovementConstrains _enum, params MovementConstrains[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }
	
	/// <summary>Toggles flag from MovementConstrains enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from MovementConstrains enumerator.</param>
	public static void ToggleFlag(ref MovementConstrains _enum, MovementConstrains _flag){ _enum ^= _flag; }
	
	/// <summary>Toggles flags from MovementConstrains enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from MovementConstrains enumerator.</param>
	public static void ToggleFlags(ref MovementConstrains _enum, params MovementConstrains[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }
	
	/// <summary>Removes all MovementConstrains enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref MovementConstrains _enum){ _enum = (MovementConstrains)0; }
#endregion*/

#region Axes3DFlagEnumOperations:
	/// <summary>Checks if Axes3D enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the Axes3D enumerator contains flag.</returns>
	public static bool HasFlag(this Axes3D _enum, Axes3D _flag){ return ((_enum & _flag) == _flag); }
	
	/// <summary>Checks if Axes3D enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the Axes3D enumerator contains all flags.</returns>
	public static bool HasFlags(this Axes3D _enum, params Axes3D[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to Axes3D enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the Axes3D enumerator.</param>
	public static void AddFlag(ref Axes3D _enum, Axes3D _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to Axes3D enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the Axes3D enumerator.</param>
	public static void AddFlags(ref Axes3D _enum, params Axes3D[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }
	
	/// <summary>Removes flag from Axes3D enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from Axes3D enumerator.</param>
	public static void RemoveFlag(ref Axes3D _enum, Axes3D _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }
	
	/// <summary>Removes flags from Axes3D enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from Axes3D enumerator.</param>
	public static void RemoveFlags(ref Axes3D _enum, params Axes3D[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }
	
	/// <summary>Toggles flag from Axes3D enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from Axes3D enumerator.</param>
	public static void ToggleFlag(ref Axes3D _enum, Axes3D _flag){ _enum ^= _flag; }
	
	/// <summary>Toggles flags from Axes3D enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from Axes3D enumerator.</param>
	public static void ToggleFlags(ref Axes3D _enum, params Axes3D[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }
	
	/// <summary>Removes all Axes3D enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref Axes3D _enum){ _enum = (Axes3D)0; }
#endregion

#region TransformPropertiesFlaggableOperations:
	/// <summary>Checks if TransformProperties enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the TransformProperties enumerator contains flag.</returns>
	public static bool HasFlag(this TransformProperties _enum, TransformProperties _flag){ return ((_enum & _flag) == _flag); }
	
	/// <summary>Checks if TransformProperties enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the TransformProperties enumerator contains all flags.</returns>
	public static bool HasFlags(this TransformProperties _enum, params TransformProperties[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to TransformProperties enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the TransformProperties enumerator.</param>
	public static void AddFlag(ref TransformProperties _enum, TransformProperties _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to TransformProperties enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the TransformProperties enumerator.</param>
	public static void AddFlags(ref TransformProperties _enum, params TransformProperties[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }
	
	/// <summary>Removes flag from TransformProperties enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from TransformProperties enumerator.</param>
	public static void RemoveFlag(ref TransformProperties _enum, TransformProperties _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }
	
	/// <summary>Removes flags from TransformProperties enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from TransformProperties enumerator.</param>
	public static void RemoveFlags(ref TransformProperties _enum, params TransformProperties[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }
	
	/// <summary>Toggles flag from TransformProperties enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from TransformProperties enumerator.</param>
	public static void ToggleFlag(ref TransformProperties _enum, TransformProperties _flag){ _enum ^= _flag; }
	
	/// <summary>Toggles flags from TransformProperties enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from TransformProperties enumerator.</param>
	public static void ToggleFlags(ref TransformProperties _enum, params TransformProperties[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }
	
	/// <summary>Removes all TransformProperties enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref TransformProperties _enum){ _enum = (TransformProperties)0; }
#endregion
/*
#region AbilityStatesFlagEnumOperations:
	/// <summary>Checks if AbilityState enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the AbilityState enumerator contains flag.</returns>
	public static bool HasFlag(this AbilityState _enum, AbilityState _flag){ return ((_enum & _flag) == _flag); }
	
	/// <summary>Checks if AbilityState enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the AbilityState enumerator contains all flags.</returns>
	public static bool HasFlags(this AbilityState _enum, params AbilityState[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to AbilityState enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the AbilityState enumerator.</param>
	public static void AddFlag(ref AbilityState _enum, AbilityState _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to AbilityState enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the AbilityState enumerator.</param>
	public static void AddFlags(ref AbilityState _enum, params AbilityState[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }
	
	/// <summary>Removes flag from AbilityState enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from AbilityState enumerator.</param>
	public static void RemoveFlag(ref AbilityState _enum, AbilityState _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }
	
	/// <summary>Removes flags from AbilityState enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from AbilityState enumerator.</param>
	public static void RemoveFlags(ref AbilityState _enum, params AbilityState[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }
	
	/// <summary>Toggles flag from AbilityState enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from AbilityState enumerator.</param>
	public static void ToggleFlag(ref AbilityState _enum, AbilityState _flag){ _enum ^= _flag; }
	
	/// <summary>Toggles flags from AbilityState enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from AbilityState enumerator.</param>
	public static void ToggleFlags(ref AbilityState _enum, params AbilityState[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }
	
	/// <summary>Removes all AbilityState enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref AbilityState _enum){ _enum = (AbilityState)0; }
#endregion

#region HitColliderEventTypesFlagEnumOperators:
	/// <summary>Checks if HitColliderEventTypes enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the HitColliderEventTypes enumerator contains flag.</returns>
	public static bool HasFlag(this HitColliderEventTypes _enum, HitColliderEventTypes _flag){ return ((_enum & _flag) == _flag); }
	
	/// <summary>Checks if HitColliderEventTypes enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the HitColliderEventTypes enumerator contains all flags.</returns>
	public static bool HasFlags(this HitColliderEventTypes _enum, params HitColliderEventTypes[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to HitColliderEventTypes enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the HitColliderEventTypes enumerator.</param>
	public static void AddFlag(ref HitColliderEventTypes _enum, HitColliderEventTypes _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to HitColliderEventTypes enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the HitColliderEventTypes enumerator.</param>
	public static void AddFlags(ref HitColliderEventTypes _enum, params HitColliderEventTypes[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }
	
	/// <summary>Removes flag from HitColliderEventTypes enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from HitColliderEventTypes enumerator.</param>
	public static void RemoveFlag(ref HitColliderEventTypes _enum, HitColliderEventTypes _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }
	
	/// <summary>Removes flags from HitColliderEventTypes enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from HitColliderEventTypes enumerator.</param>
	public static void RemoveFlags(ref HitColliderEventTypes _enum, params HitColliderEventTypes[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }
	
	/// <summary>Toggles flag from HitColliderEventTypes enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from HitColliderEventTypes enumerator.</param>
	public static void ToggleFlag(ref HitColliderEventTypes _enum, HitColliderEventTypes _flag){ _enum ^= _flag; }
	
	/// <summary>Toggles flags from HitColliderEventTypes enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from HitColliderEventTypes enumerator.</param>
	public static void ToggleFlags(ref HitColliderEventTypes _enum, params HitColliderEventTypes[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }
	
	/// <summary>Removes all HitColliderEventTypes enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref HitColliderEventTypes _enum){ _enum = (HitColliderEventTypes)0; }
#endregion
*/
}
}