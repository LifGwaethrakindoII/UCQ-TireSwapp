using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UrielChallenge
{
public interface IPickableHandler : IPickable
{
	event OnHandlerPicked onHandlerPicked; 	/// <summary>OnHandlerPicked's subscription delegate.</summary>
	int ID { get; set; } 					/// <summary>Handler's ID.</summary>
}
}