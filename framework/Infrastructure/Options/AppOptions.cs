using System.ComponentModel.DataAnnotations;

namespace FSH.Framework.Infrastructure.Options
{
    public class AppOptions : IOptionsRoot
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = "FSH.WebAPI";
    }
}
