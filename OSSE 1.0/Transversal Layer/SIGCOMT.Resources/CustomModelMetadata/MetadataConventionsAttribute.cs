using System;

namespace SIGCOMT.Resources.CustomModelMetadata
{
    public class MetadataConventionsAttribute : Attribute
    {
        public Type ResourceType { get; set; }
    }
}