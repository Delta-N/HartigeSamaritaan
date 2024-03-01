using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace RoosterPlanner.Models.Models
{
    [Index(nameof(CertificateTypeId), nameof(TaskId), IsUnique = true)]
    public class Requirement : Entity
    {
        /// <summary>
        /// Gets or sets the CertificateTypeId
        /// </summary>
        public Guid? CertificateTypeId { get; set; }

        /// <summary>
        /// Gets or sets the TaskId 
        /// </summary>
        public Guid? TaskId { get; set; }

        /// <summary>
        /// Gets or sets the CertificateType 
        /// </summary>
        [ForeignKey("CertificateTypeId")]
        public CertificateType CertificateType { get; set; }

        /// <summary>
        /// Gets or sets the Task 
        /// </summary>
        [ForeignKey("TaskId")]
        public Task Task { get; set; }

        //constructor
        public Requirement() : base(Guid.Empty)
        {
        }

        //constructor
        public Requirement(Guid id) : base(id)
        {
        }
    }
}