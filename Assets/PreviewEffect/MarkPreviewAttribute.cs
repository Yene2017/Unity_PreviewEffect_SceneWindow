using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.PreviewEffect
{
    public class MarkPreviewAttribute : Attribute
    {
        public Type type;

        public MarkPreviewAttribute(Type previewType)
        {
            type = previewType;
        }
    }
}
