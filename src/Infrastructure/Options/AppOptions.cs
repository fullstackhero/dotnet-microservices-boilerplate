using System.ComponentModel.DataAnnotations;

namespace FSH.Microservices.Infrastructure.Options
{
    public class AppOptions : IOptionsRoot
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = "FSH.WebAPI";
    }
}
