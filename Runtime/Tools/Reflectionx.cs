using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

public class Reflectionx
{

	/// <summary>
	/// Gets the all classes of a Type Assembly
	/// </summary>
	/// <returns>The classes.</returns>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static List<Type> GetClasses<T> ()
	{
		return GetClasses<T> (Assembly.GetAssembly (typeof(T)));
	}

	/// <summary>
	/// Gets the classes in a Assembly
	/// </summary>
	/// <returns>The classes.</returns>
	/// <param name="assembly">Assembly.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static List<Type> GetClasses<T> (Assembly assembly)
	{
		var derivedType = typeof(T);
		return assembly
			.GetTypes ()
			.Where (t =>
				t != derivedType &&
		derivedType.IsAssignableFrom (t)
		).ToList ();

	}

	public static IEnumerable<Type> ForeachTypeInAssembly (Assembly a)
	{
		foreach (Type t in a.GetTypes())
		{
			yield return t;
		}
	}

	public static List<MethodInfo> GetMethodsWithAttribute<T> ()
	{
		return GetMethodsWithAttribute<T> (Assembly.GetAssembly (typeof(T)));
	}

	public static List<MethodInfo> GetMethodsWithAttribute<T> (Assembly assembly)
	{
		return assembly.GetTypes ()
			.SelectMany (t => t.GetMethods ())
			.Where (m => m.GetCustomAttributes (typeof(T), false).Length > 0)
			.ToList ();
	}

	public static void GetAttributeOfClass<T> (Type t, System.Action<Type, T> action)
	{
		T[] attrib; 
		attrib = t.GetCustomAttributes (typeof(T), false).Cast<T> ().ToArray ();
		if (attrib.Length > 0)
		{
			if (action != null)
			{
				action (t, attrib [0]);
			}
		}
	}

	public static void GetAttributeOfProperty<T> (Type t, System.Action<PropertyInfo, T> action)
	{
		foreach (PropertyInfo p in t.GetProperties())
		{
			object[] attrobj = p.GetCustomAttributes (false);
			if (attrobj.Length == 0) continue;
			foreach (object obj in attrobj)
			{
				if (obj is T && action != null)
				{
					action (p, (T)obj);
				}
			}
		}
	}

}

