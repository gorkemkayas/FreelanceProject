using Microsoft.AspNetCore.Identity;
namespace FreelanceProject.Utilities;
public class ServiceResult<T>
{
    public bool IsSuccess { get; set; }
    public List<IdentityError> Errors { get; set; } = new List<IdentityError>();
    public T? Data { get; set; }

    public bool isCritical { get; set; } = false;
}