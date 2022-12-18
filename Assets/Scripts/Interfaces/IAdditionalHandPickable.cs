using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public interface IAdditionalHandPickable : IPickable
{
	Hand additionalHand { get; set; } 	/// <summary>Pickable's Additional Hand.</summary>
}
}