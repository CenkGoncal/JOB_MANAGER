//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JOB_MANAGER.Models
{
    using System;
    using System.Collections.Generic;
    using static JOB_MANAGER.Models.Abstact.Entity;

    public partial class DOCUMENT_TYPES: IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DOCUMENT_TYPES()
        {
            this.DOCUMENTS = new HashSet<DOCUMENTS>();
        }
    
        public short DOCUMENT_TYPE_ID { get; set; }
        public string DOCUMENT_TYPE_NAME { get; set; }
        public string DOCUMENT_EXTENSION { get; set; }
        public string FONT_AWESOME_ICON { get; set; }
        public System.DateTime CREATION_DATE { get; set; }
        public System.DateTime MODIFIED_DATE { get; set; }
        public int CREATED_BY { get; set; }
        public int UPDATED_BY { get; set; }
        public bool IS_CANCELED { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTS> DOCUMENTS { get; set; }
    }
}
