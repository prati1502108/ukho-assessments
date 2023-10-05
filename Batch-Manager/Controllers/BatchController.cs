using Batch_Manager.Models;
using Batch_Manager.Services;
using Batch_Manager.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Batch_Manager.Controllers
{
    [ApiController]
    public class BatchController : ControllerBase
    {
        private readonly IBatch _batch;
        private readonly ICorrelationIdGenerator _correlationIdGenerator;
        private readonly IBatchValidator _batchValidator;
        private readonly ErrorDetails _errorDetails;
        private readonly List<Error> _errors;
        private readonly Error _error;

        public BatchController(IBatch batch, ICorrelationIdGenerator correlationIdGenerator, IBatchValidator batchValidator)
        {
            _batch = batch;
            _correlationIdGenerator = correlationIdGenerator;
            _batchValidator = batchValidator;
            _errorDetails = new ErrorDetails();
            _errors = new List<Error>();
            _error = new Error();
        }

        /// <summary>
        /// Create a batch by passing batch information in request body.
        ///  This endpoint will create a batch.
        /// </summary>
        /// <param name="batch"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        /// <response code="400">If the item is null and validations failed</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("Batch")]
        public IActionResult CreateBatch([FromBody] Batch batch)
        {
            try
            {
                if (batch == null)
                    throw new ApplicationException("Batch object is null.");

                var validationResult = _batchValidator.Validate(batch);
                if (!validationResult.IsValid)
                    return BadRequest();

                BatchResponse batchResponse = new BatchResponse();
                batchResponse = _batch.CreateBatch(batch);
                return Created("Batch", batchResponse);
            }
            catch (Exception exception)
            {
                _errorDetails.CorrelationId = this._correlationIdGenerator.Get();
                _error.Source = "CreateBatch Controller | CreateBatch endpoint";
                _error.Description = exception.Message;
                _errors.Add(_error);
                _errorDetails.Errors = _errors;
                return BadRequest(_errorDetails);
            }
        }

        /// <summary>
        /// This endpoint returns the batch details if given batch id exists in database.
        ///  BatchId should be a Guid format input.
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        [HttpGet]
        [Route("batch/{batchId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBatchByBatchId(string batchId)
        {
            try
            {
                if (!Guid.TryParse(batchId, out _))
                    throw new ApplicationException("Batch Id should be in Guid format.");
                try
                {
                    Batch batch = new Batch();
                    batch = _batch.GetBatches(batchId);
                    return Ok(batch);
                }
                catch (Exception exception)
                {
                    _errorDetails.CorrelationId = this._correlationIdGenerator.Get();
                    _error.Source = "Batch Controller | GetBatchByBatchId endpoint";
                    _error.Description = exception.Message;
                    _errors.Add(_error);
                    _errorDetails.Errors = _errors;
                    return NotFound(_errorDetails);
                }
            }
            catch (Exception ex)
            {
                _errorDetails.CorrelationId = this._correlationIdGenerator.Get();
                _error.Source = "Batch Controller";
                _error.Description = ex.Message;
                _errors.Add(_error);
                _errorDetails.Errors = _errors;
                return BadRequest(_errorDetails);
            }
        }

        /// <summary>
        /// Create a file in batch.
        /// </summary>
        /// <param name="batchId"></param>
        /// <param name="fileName"></param>
        /// <param name="fileType"></param>
        /// <param name="contentSize"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{batchId}/{fileName}")]
        public IActionResult AddFile(string batchId, string fileName, string fileType, string contentSize)
        {
            try
            {
                string response = _batch.AddFile(batchId, fileName, fileType, contentSize);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _errorDetails.CorrelationId = this._correlationIdGenerator.Get();
                _error.Source = "Batch Controller";
                _error.Description = ex.Message;
                _errors.Add(_error);
                _errorDetails.Errors = _errors;
                return BadRequest(_errorDetails);
            }
        }

    }
}
