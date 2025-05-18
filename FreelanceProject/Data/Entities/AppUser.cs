using Microsoft.AspNetCore.Identity;
using static FreelanceProject.Models.ViewModels.ExtendedProfileViewModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelanceProject.Data.Entities
{
    public class AppUser: IdentityUser<Guid>
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string FullName { get { return Name + " " + Surname; } }
        public DateTime BirthDate { get; set; }

        public float Wallet { get; set; } = 0;
       
        public string? Title { get; set; }
        public string? Bio { get; set; }
        public string? WorkingAt { get; set; }
        public string? WorkingAtLogo { get; set; }
        

        //public string? LivesIn { get; set; }


        public string? Country { get; set; }

        // yeni eklediklerim
        public string? CurrentPosition { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }

        public string? XAddress { get; set; }
        public string? LinkedinAddress { get; set; }
        public string? GithubAddress { get; set; }
        public string? MediumAddress { get; set; }
        public string? YoutubeAddress { get; set; }
        public string? PersonalWebAddress { get; set; }

        public string? HighSchoolName { get; set; }
        public string? HighSchoolStartYear { get; set; }
        public string? HighSchoolGraduationYear { get; set; }

        public string? UniversityName { get; set; }
        public string? UniversityStartYear { get; set; }
        public string? UniversityGraduationYear { get; set; }

        public DateTime RegisteredDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string? CVPath { get; set; }

        public string? ProfilePicture { get; set; }
        public string? CoverImagePicture { get; set; }

        public int FollowersCount { get; set; } = 0;
        public int FollowingCount { get; set; } = 0;
        public int NotificationCount { get; set; } = 0;

        public virtual ICollection<PostEntity>? Posts { get; set; }
        public virtual ICollection<CommentEntity>? Comments { get; set; }
        public virtual ICollection<NotificationEntity>? Notifications { get; set; }
        public virtual ICollection<LikeEntity>? Likes { get; set; }

        public virtual ICollection<FollowEntity>? Followers { get; set; } // Kullanıcıyı takip edenler
        public virtual ICollection<FollowEntity>? Followings { get; set; } // Kullanıcının takip ettikleri

        public virtual ICollection<MessageEntity>? SentMessages { get; set; }
        public virtual ICollection<MessageEntity>? ReceivedMessages { get; set; }

        public virtual ICollection<JobEntity>? CreatedJobs { get; set; } // İşveren olarak oluşturduğu işler
        public virtual ICollection<JobApplicationEntity>? JobApplications { get; set; } // Başvurduğu işler




        public string GetPropertyValue(PhotoType type)
        {
            switch (type)
            {
                case PhotoType.ProfilePicture:
                    return ProfilePicture;
                case PhotoType.CoverImagePicture:
                    return CoverImagePicture;
                case PhotoType.WorkingAtLogo:
                    return WorkingAtLogo;
                case PhotoType.CV:
                    return CVPath;
                default:
                    return "";
            }
        }
    }
}

