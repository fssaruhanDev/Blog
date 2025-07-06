using System;
namespace Jotem.Api.Domain.Models
{
	public abstract class BaseEntity
	{
		public Guid ID { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public Guid CreatedBy { get; set; }
		public Guid? UpdatedBy { get; set; }

        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
		public bool isModified { get; set; }
    }
}

