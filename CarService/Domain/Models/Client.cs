namespace CarService.Data.Local
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Client")]
    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors"
        )]
        public Client()
        {
            ClientServices = new HashSet<ClientService>();
            Tags = new HashSet<Tag>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Patronymic { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Birthday { get; set; }

        public DateTime RegistrationDate { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        [StringLength(1)]
        public string GenderCode { get; set; }

        [StringLength(1000)]
        public string PhotoPath { get; set; }

        public string LastVisit =>
            ClientServices
                .LastOrDefault(clientServices => clientServices.ClientID == ID)
                ?.StartTime.ToString();

        public int CountVisit =>
            ClientServices.Select(clientServices => clientServices.ClientID == ID).Count();

        public virtual Gender Gender { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly"
        )]
        public virtual ICollection<ClientService> ClientServices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly"
        )]
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
