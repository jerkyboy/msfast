using System;
using System.Collections.Generic;
using System.Text;

namespace MySpace.MSFast.Core.Utils
{
    public class AttributesHelpers
    {
        public static Attribute GetAttributeFromType(Type attributeType, Object searchIn)
        {
            if (searchIn == null || attributeType == null)
                return null;

            foreach (object a in searchIn.GetType().GetCustomAttributes(true))
            {
                if (a.GetType().Equals(attributeType))
                {
                    return (Attribute)a;
                }
            }
            return null;
        }
    }
}
