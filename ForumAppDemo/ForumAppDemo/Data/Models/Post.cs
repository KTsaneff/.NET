using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static ForumAppDemo.Constants.DataConstants.Post;

namespace ForumAppDemo.Data.Models
{
    [Comment("Published posts")]
    public class Post
    {
        [Key]
        [Comment("Posts Identifier")]
        public int Id { get; set; }

        [Required]
        [Comment("Post title")]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [Comment("Post content")]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; } = null!;

        [Comment("Marks record as deleted")]
        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}
