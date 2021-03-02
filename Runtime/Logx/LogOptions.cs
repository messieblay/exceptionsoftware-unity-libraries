using UnityEngine;
using System.Collections;
using System;

[Flags]
public enum LogOptions
{
	None = 1 << 0,
	Delayed = 1 << 1,
	SameRow = 1 << 2,
	Bold = 1 << 3,
	ShowClass = 1 << 4
}
 