using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class ExReflect
{
    public static Assembly CurrentAssembly { get { return Assembly.GetExecutingAssembly(); } }
    #region Classes
    public static Type[] GetTypes() { return GetTypes(CurrentAssembly); }
    public static Type[] GetTypes(Assembly assembly) { return assembly.GetTypes(); }

    public static Type FindType(string type) => AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(s => s.FullName == type).FirstOrDefault();
    public static List<Type> FindTypes(string type) => AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(s => s.Name == type).ToList();

    /// <summary>
    /// Get All derived Clases of T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<Type> GetDerivedClasses<T>() { return GetDerivedClasses<T>(Assembly.GetAssembly(typeof(T))); }
    public static List<Type> GetDerivedClasses<T>(Assembly assembly) { return GetDerivedClasses(assembly, typeof(T)); }
    public static List<Type> GetDerivedClasses(Assembly assembly, Type derivedType) { return assembly.GetTypes().Where(t => t != derivedType && derivedType.IsAssignableFrom(t)).ToList(); }
    public static List<Type> GetDerivedClassesAllAsseblys<T>() { return AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(t => t != typeof(T) && typeof(T).IsAssignableFrom(t)).ToList(); }

    public static bool HasDerivedClasses<T>() { return HasDerivedClasses<T>(Assembly.GetAssembly(typeof(T))); }
    public static bool HasDerivedClasses<T>(Assembly assembly) { return HasDerivedClasses(assembly, typeof(T)); }
    public static bool HasDerivedClasses(Assembly assembly, Type derivedType) { return assembly.GetTypes().Where(t => t != derivedType && derivedType.IsAssignableFrom(t)).Count() > 0; }



    public static List<Type> GetFinalDerivedClasses<T>() { return GetFinalDerivedClasses(Assembly.GetAssembly(typeof(T)), typeof(T)); }
    public static List<Type> GetFinalDerivedClasses<T>(Assembly assembly) { return GetFinalDerivedClasses(assembly, typeof(T)); }
    public static List<Type> GetFinalDerivedClasses(Assembly assembly, Type derived) { return GetDerivedClasses(assembly, derived).Where(s => !HasDerivedClasses(assembly, s)).ToList(); }

    /// <summary>
    /// Get All types found in a namespace
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="nameSpace"></param>
    /// <returns></returns>
    public static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
    {
        return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
    }

    /// <summary>
    ///  Get All types found.
    ///  Assembly -> ExecutingAssembly
    /// </summary>
    /// <param name="nameSpace"></param>
    /// <returns></returns>
    public static Type[] GetTypesInNamespace(string nameSpace)
    {
        return CurrentAssembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
    }


    public static IEnumerable<Type> ForeachTypeInAssembly(Assembly a)
    {
        foreach (Type t in a.GetTypes())
        {
            yield return t;
        }
    }

    public static IEnumerable<Type> ForeachDerivedClasses<T>()
    {
        return ForeachDerivedClasses(typeof(T));
    }

    public static IEnumerable<Type> ForeachDerivedClasses(Type type)
    {
        return Assembly.GetAssembly(type).GetTypes().Where(t => t != type && type.IsAssignableFrom(t)).AsEnumerable();
    }
    public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
    {
        while (toCheck != null && toCheck != typeof(object))
        {
            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur)
            {
                return true;
            }
            toCheck = toCheck.BaseType;
        }
        return false;
    }
    #endregion

    #region Filds
    public static void GetStaticFildsOfClass<T>(System.Action<FieldInfo> action)
    {
        Type t = typeof(T);
        FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.Static);
        foreach (FieldInfo f in fields)
        {
            //Debug.Log(f.Name);
            action(f);
        }
    }
    public static void SetValueNonPublicReflect(object controller, string fieldName, object val, bool setInBaseClass = true)
    {
        Type type = controller.GetType();

        if (setInBaseClass)
            type = type.BaseType;

        type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
        .SetValue(controller, val);
    }
    #endregion
    #region Methods

    public static List<MethodInfo> GetMethodsWithAttribute<T>()
    {
        return GetMethodsWithAttribute<T>(Assembly.GetAssembly(typeof(T)));
    }

    public static List<MethodInfo> GetMethodsWithAttribute<T>(Assembly assembly)
    {
        return assembly.GetTypes().SelectMany(t => t.GetMethods()).Where(m => m.GetCustomAttributes(typeof(T), false).Length > 0).ToList();
    }

    #endregion
    #region Properties

    public static List<PropertyInfo> GetPropertiesWithAttribute<T>()
    {
        return GetPropertiesWithAttribute<T>(Assembly.GetAssembly(typeof(T)));
    }

    public static List<PropertyInfo> GetPropertiesWithAttribute<T>(Assembly assembly)
    {
        return assembly.GetTypes().SelectMany(t => t.GetProperties()).Where(m => m.GetCustomAttributes(typeof(T), false).Length > 0).ToList();
    }

    #endregion


    #region Attributes
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="K">Class type</typeparam>
    /// <typeparam name="T">Attribute type</typeparam>
    /// <returns></returns>
    public static T[] GetAttributesOfClass<K, T>()
    {
        return Attribute.GetCustomAttributes(typeof(K)).OfType<T>().ToArray();
    }
    public static IEnumerable<T> GetAttributesOfClassAsEnumerable<K, T>()
    {
        return Attribute.GetCustomAttributes(typeof(K)).OfType<T>();
    }
    public static IEnumerable<T> GetAttributesOfClassAsEnumerable<T>(Type classType)
    {
        return Attribute.GetCustomAttributes(classType).OfType<T>();
    }

    public static void GetAttributeOfClass<T>(Type classType, System.Action<Type, T> action)
    {
        T[] attrib;
        attrib = classType.GetCustomAttributes(typeof(T), false).Cast<T>().ToArray();
        if (attrib.Length > 0)
        {
            if (action != null)
            {
                action(classType, attrib[0]);
            }
        }
    }

    public static void GetAttributeOfProperty<T>(Type t, System.Action<PropertyInfo, T> action)
    {
        foreach (PropertyInfo p in t.GetProperties())
        {
            object[] attrobj = p.GetCustomAttributes(false);
            if (attrobj.Length == 0) continue;
            foreach (object obj in attrobj)
            {
                if (obj is T && action != null)
                {
                    action(p, (T)obj);
                }
            }
        }
    }
    #endregion

}
