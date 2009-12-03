using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MySpace.MSFast.Core.Utils
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class JSONObject : Attribute
    {
        public String ObjectName = null;

        public JSONObject(String ObjectName)
        {
            this.ObjectName = ObjectName;
        }

        public JSONObject() { }
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class JSONField : Attribute
    {
        public String FieldName = null;

        public JSONField()
        {
        }

        public JSONField(String FieldName)
        {
            this.FieldName = FieldName;
        }
    }

    public class SimpleJSONSerializer
    {
        public static String Serialize(object serialize)
        {
            return Serialize(serialize, null);
        }
        public static String Serialize(object serialize, String name)
        {
            if (serialize == null)
                throw new NullReferenceException();

            JSONObject baseObjectAtt = AttributesHelpers.GetAttributeFromType(typeof(JSONObject), serialize) as JSONObject;

            if (baseObjectAtt == null)
                throw new SimpleJSONSerializerException("Invalid JSON object");

            StringBuilder objectStringBuild = new StringBuilder();

            foreach (FieldInfo f in serialize.GetType().GetFields())
            {
                object[] o = f.GetCustomAttributes(typeof(JSONField), true);

                if (o == null || o.Length != 1)
                    continue;

                String fname = ((JSONField)o[0]).FieldName;
                Object fvalue = f.GetValue(serialize);

                if (fvalue != null)
                {
                    if (fvalue is Array)
                    {
                        StringBuilder arr = new StringBuilder();
                        foreach (object j in (Array)fvalue)
                        {
                            AppendValue(arr, j, "");
                        }

                        if (objectStringBuild.Length > 0) objectStringBuild.Append(",");
                        if (String.IsNullOrEmpty(fname) == false)
                        {
                            objectStringBuild.Append("\"").Append(fname).Append("\":[").Append(arr.ToString()).Append("]");
                        }
                        else
                        {
                            objectStringBuild.Append("[").Append(arr.ToString()).Append("]");
                        }
                    }
                    else
                    {
                        AppendValue(objectStringBuild, fvalue, fname);
                    }
                }
            }

            if (name == null)
                name = baseObjectAtt.ObjectName;

            if (String.IsNullOrEmpty(name))
            {
                return objectStringBuild.Insert(0, "{").Append("}").ToString();
            }
            else
            {
                return objectStringBuild.Insert(0, "\":{").Insert(0, name).Insert(0, "\"").Append("}").ToString();
            }
        }

        private static void AppendValue(StringBuilder objectStringBuild, object fvalue, string fname)
        {
            if ((AttributesHelpers.GetAttributeFromType(typeof(JSONObject), fvalue) as JSONObject) != null)
            {
                String ob = Serialize(fvalue, fname);
                if (String.IsNullOrEmpty(ob) == false)
                {
                    if (objectStringBuild.Length > 0) objectStringBuild.Append(",");
                    objectStringBuild.Append(ob);
                }
            }
            else
            {
                if (objectStringBuild.Length > 0) objectStringBuild.Append(",");
                if (String.IsNullOrEmpty(fname) == false)
                {
                    objectStringBuild.Append("\"").Append(fname).Append("\":");
                }

                if (fvalue is int || fvalue is long || fvalue is double || fvalue is uint || fvalue is ushort || fvalue is short || fvalue is float)
                {
                    objectStringBuild.Append(fvalue.ToString());
                }
                else if (fvalue is DateTime)
                {
                    objectStringBuild.Append("\"").Append(((DateTime)fvalue).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK")).Append("\"");
                }
                else
                {
                    objectStringBuild.Append("\"").Append(fvalue.ToString()).Append("\"");
                }
            }
        }
    }

    public class SimpleJSONSerializerException : Exception
    {
        public SimpleJSONSerializerException(String message)
            : base(message)
        {
        }
    }
}
