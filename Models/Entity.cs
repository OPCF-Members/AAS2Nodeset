﻿
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class Entity : SubmodelElement
    {
        [Required]
        [DataMember(Name = "entityType")]
        [XmlElement(ElementName = "entityType")]
        public string EntityType { get; set; }

        [DataMember(Name = "globalAssetId")]
        [XmlElement(ElementName = "globalAssetId")]
        public string GlobalAssetId { get; set; }

        // Important note: XML serialization uses SubModel Element Wrappers while JSON serialization does not!
        // So we have to first deserialize into a placeholder Json member and then copy the contents into the correct member
        [JsonIgnore]
        [XmlArray(ElementName = "statements")]
        public List<SubmodelElementWrapper> Statements { get; set; } = new();

        [XmlIgnore]
        [DataMember(Name = "statements")]
        public SubmodelElement[] JsonStatements
        {
            get
            {
                var submodelElements = new List<SubmodelElement>();

                foreach (SubmodelElementWrapper smew in Statements)
                {
                    submodelElements.Add(smew.SubmodelElement);
                }

                return submodelElements.ToArray();
            }

            set
            {
                if (value != null)
                {
                    Statements.Clear();

                    foreach (SubmodelElement sme in value)
                    {
                        Statements.Add(new SubmodelElementWrapper() { SubmodelElement = sme });
                    }
                }
            }
        }

        public Entity()
        {
            ModelType = ModelTypes.Entity;
        }

        public Entity(SubmodelElement src)
            : base(src)
        {
            if (!(src is Entity ent))
            {
                return;
            }

            if (ent.Statements != null)
            {
                Statements = new List<SubmodelElementWrapper>();
                foreach (var smw in ent.Statements)
                {
                    Statements.Add(smw);
                }
            }

            EntityType = ent.EntityType;
            ModelType = ModelTypes.Entity;
        }
    }
}
