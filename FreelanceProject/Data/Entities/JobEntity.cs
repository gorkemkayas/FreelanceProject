using FreelanceProject.Data.Entities.BaseEntities;
using Microsoft.AspNetCore.Builder;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace FreelanceProject.Data.Entities
{
    public class JobEntity : BaseEntity
    {
            public string Title { get; set; } = null!;
            public string Description { get; set; } = null!;

            public string Category { get; set; }

            public string? Requirements { get; set; } // İsteğe bağlı: aranan özellikler

            public decimal? Budget { get; set; } // Ödeme teklifi (sabit ya da aralık belirtilebilir)

            public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

            public DateTime? StartDate { get; set; }

            public DateTime? Deadline { get; set; } // Son başvuru tarihi vs.

            public bool IsCompleted { get; set; } = false; // İş tamamlandı mı
            public bool IsActive { get; set; } = true;
            public string? JobImage { get; set; }

            // ---- İlişkiler ----

            // İlanı oluşturan kullanıcı
            [ForeignKey("Owner")]
            public Guid OwnerId { get; set; }
            public virtual AppUser Owner { get; set; }

            // Bu ilana yapılan başvurular
            public virtual ICollection<JobApplicationEntity> Applications { get; set; } = new List<JobApplicationEntity>();
        }
    }
