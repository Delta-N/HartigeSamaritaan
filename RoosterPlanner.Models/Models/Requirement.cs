using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RoosterPlanner.Models
{
    [Index(nameof(CertificateTypeId),nameof(TaskId),IsUnique = true)]
    public class Requirement : Entity
    {
        public Guid? CertificateTypeId { get; set; }
        public Guid? TaskId { get; set; }

        [ForeignKey("CertificateTypeId")] 
        public CertificateType CertificateType { get; set; }

        [ForeignKey("TaskId")] 
        public Task Task { get; set; }

        public Requirement() : base(Guid.Empty)
        {
        }

        public Requirement(Guid id) : base(id)
        {
        }
    }
}