using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models.Models;
using RoosterPlanner.Service.DataModels;
namespace RoosterPlanner.Service.Services
{
    public interface ICertificateService
    {
        /// <summary>
        /// Makes a call to the repository layer and requests all certificateTypes.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <returns></returns>
        Task<TaskListResult<CertificateType>> GetAllCertificateTypesAsync();

        /// <summary>
        /// Makes a call to the repository layer and requests a certificateType based on an id.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskResult<CertificateType>> GetCertificateTypeAsync(Guid id);

        /// <summary>
        /// Makes a call to the repository layer and adds a certificateType to the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="certificateType"></param>
        /// <returns></returns>
        Task<TaskResult<CertificateType>> CreateCertificateTypeAsync(CertificateType certificateType);

        /// <summary>
        /// Makes a call to the repository layer and requests an update of a certificateType.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="certificateType"></param>
        /// <returns></returns>
        Task<TaskResult<CertificateType>> UpdateCertificateTypeAsync(CertificateType certificateType);

        /// <summary>
        /// Makes a call to the repository layer and requests an removal of a certificateType.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="certificateType"></param>
        /// <returns></returns>
        Task<TaskResult<CertificateType>> RemoveCertificateTypeAsync(CertificateType certificateType);

        /// <summary>
        /// Makes a call to the repository layer and requests an certificate based on a id.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskResult<Certificate>> GetCertificateAsync(Guid id);

        /// <summary>
        /// Makes a call to the repository layer and adds a certificate to the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        Task<TaskResult<Certificate>> CreateCertificateAsync(Certificate certificate);

        /// <summary>
        /// Makes a call to the repository layer and request an update of a certificate.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        Task<TaskResult<Certificate>> UpdateCertificateAsync(Certificate certificate);

        /// <summary>
        /// Makes a call to the repository layer and request an deletion of a certificate.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        Task<TaskResult<Certificate>> RemoveCertificateAsync(Certificate certificate);
    }

    public class CertificateService : ICertificateService
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CertificateService> logger;
        private readonly ICertificateRepository certificateRepository;
        private readonly ICertificateTypeRepository certificateTypeRepository;

        #endregion

        public CertificateService(IUnitOfWork unitOfWork, ILogger<CertificateService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            certificateRepository = unitOfWork.CertificateRepository;
            certificateTypeRepository = unitOfWork.CertificateTypeRepository;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests all certificateTypes.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <returns></returns>
        public async Task<TaskListResult<CertificateType>> GetAllCertificateTypesAsync()
        {
            TaskListResult<CertificateType> result = TaskListResult<CertificateType>.CreateDefault();
            try
            {
                result.Data = await certificateTypeRepository.GetAllCategoriesAsync();
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting all CertificateTypes ";
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests a certificateType based on an id.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskResult<CertificateType>> GetCertificateTypeAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            TaskResult<CertificateType> result = new TaskResult<CertificateType>();
            try
            {
                result.Data = await certificateTypeRepository.GetAsync(id);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting CertificateType " + id;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and adds a certificateType to the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="certificateType"></param>
        /// <returns></returns>
        public async Task<TaskResult<CertificateType>> CreateCertificateTypeAsync(CertificateType certificateType)
        {
            if (certificateType == null)
                throw new ArgumentNullException(nameof(certificateType));

            TaskResult<CertificateType> result = new TaskResult<CertificateType>();

            try
            {
                result.Data = certificateTypeRepository.Add(certificateType);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error creating CertificateType " + certificateType.Id;
                logger.LogError(ex, result.Message, certificateType);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests an update of a certificateType.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="certificateType"></param>
        /// <returns></returns>
        public async Task<TaskResult<CertificateType>> UpdateCertificateTypeAsync(CertificateType certificateType)
        {
            if (certificateType == null)
                throw new ArgumentNullException(nameof(certificateType));

            TaskResult<CertificateType> result = new TaskResult<CertificateType>();

            try
            {
                result.Data = certificateTypeRepository.Update(certificateType);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error updating CertificateType " + certificateType.Id;
                logger.LogError(ex, result.Message, certificateType);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests an removal of a certificateType.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="certificateType"></param>
        /// <returns></returns>
        public async Task<TaskResult<CertificateType>> RemoveCertificateTypeAsync(CertificateType certificateType)
        {
            if (certificateType == null)
                throw new ArgumentNullException(nameof(certificateType));

            TaskResult<CertificateType> result = new TaskResult<CertificateType>();
            try
            {
                result.Data = unitOfWork.CertificateTypeRepository.Remove(certificateType);
                result.Succeeded = await unitOfWork.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error removing category " + certificateType.Id;
                logger.LogError(ex, result.Message, certificateType);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests an certificate based on a id.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskResult<Certificate>> GetCertificateAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            TaskResult<Certificate> result = new TaskResult<Certificate>();
            try
            {
                result.Data = await certificateRepository.GetCertificateAsync(id);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting Certificate " + id;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and adds a certificate to the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public async Task<TaskResult<Certificate>> CreateCertificateAsync(Certificate certificate)
        {
            if (certificate == null)
                throw new ArgumentNullException(nameof(certificate));

            TaskResult<Certificate> result = new TaskResult<Certificate>();

            try
            {
                result.Data = certificateRepository.Add(certificate);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error creating Certificate " + certificate.Id;
                logger.LogError(ex, result.Message, certificate);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and request an update of a certificate.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public async Task<TaskResult<Certificate>> UpdateCertificateAsync(Certificate certificate)
        {
            if (certificate == null)
                throw new ArgumentNullException(nameof(certificate));

            TaskResult<Certificate> result = new TaskResult<Certificate>();

            try
            {
                result.Data = certificateRepository.Update(certificate);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error updating Certificate " + certificate.Id;
                logger.LogError(ex, result.Message, certificate);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and request an deletion of a certificate.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public async Task<TaskResult<Certificate>> RemoveCertificateAsync(Certificate certificate)
        {
            if (certificate == null)
                throw new ArgumentNullException(nameof(certificate));

            TaskResult<Certificate> result = new TaskResult<Certificate>();
            try
            {
                result.Data = unitOfWork.CertificateRepository.Remove(certificate);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error removing Certificate " + certificate.Id;
                logger.LogError(ex, result.Message, certificate);
                result.Error = ex;
            }

            return result;
        }
    }
}