using System;

namespace SIGCOMT.DTO.CustomModelMetadata
{
    public class MetadataConventionsAttribute : Attribute
    {
        public Type ResourceType { get; set; }
    }
}