namespace Voting.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Answer")]
    public partial class Answer
    {
        public int ID { get; set; }

        public string AnswerTitle { get; set; }

        public int? Count { get; set; }

        public int? QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
